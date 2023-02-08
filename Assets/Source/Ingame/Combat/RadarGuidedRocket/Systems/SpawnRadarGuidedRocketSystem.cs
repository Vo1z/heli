using System.Runtime.CompilerServices;
using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Ingame.Detection;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.EcsExtensions.EntityReference;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Ingame.Combat
{
	public readonly struct SpawnRadarGuidedRocketSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<DiContainer> _diContainer;

		private readonly EcsFilterInject<Inc<RadarGuidedRocketSpawnerComponent>, Exc<IsDeadTag>> _radarGuidedRocketSpawnerFilter;
		private readonly EcsFilterInject<Inc<IsRadarDetectedTag>> _detectedTargetFilter;
		
		private readonly EcsPoolInject<RadarGuidedRocketSpawnerComponent> _radarGuidedRocketSpawnerCmpPool;
		private readonly EcsPoolInject<IsWaitingForLaunchTag> _isWaitingForLaunchTagPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidbodyMdlPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _radarGuidedRocketSpawnerFilter.Value)
			{
				ref var radarRocketSpawnerCmp = ref _radarGuidedRocketSpawnerCmpPool.Value.Get(entity);
				
				LoadRocketToTheSpawner(ref radarRocketSpawnerCmp);

				if (!_detectedTargetFilter.Value.IsEmpty())
				{
					LaunchRocket(ref radarRocketSpawnerCmp);
					continue;
				}

				radarRocketSpawnerCmp.timePassedSinceTargetDetection = 0f;
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LoadRocketToTheSpawner(ref RadarGuidedRocketSpawnerComponent spawnerComponent)
		{
			if (spawnerComponent.loadedRocketsEntitiesStack.Count >= spawnerComponent.spawnPositions.Length)
			{
				spawnerComponent.timePassedSinceReload = 0f;
				return;
			}

			spawnerComponent.timePassedSinceReload += Time.deltaTime;
			
			if (spawnerComponent.timePassedSinceReload > spawnerComponent.rocketReloadDuration)
			{
				int spawnOriginIndex = spawnerComponent.loadedRocketsEntitiesStack.Count;
				
				var spawnTransform  = spawnerComponent.spawnPositions[spawnOriginIndex];
				var spawnPos = spawnTransform.position;
				var spawnRotation = Quaternion.LookRotation(spawnTransform.forward);
				
				var rocketInstance = _diContainer.Value.InstantiatePrefab(spawnerComponent.radarGuidedRocketPrefab, spawnPos, spawnRotation, spawnTransform);
				var rocketInstanceEntityRef = rocketInstance.GetComponent<EcsEntityReference>();

				if (rocketInstanceEntityRef == null)
				{
					Debug.LogError($"{nameof(RadarGuidedRocketComponent)} prefab does not have {nameof(EcsEntityReference)} attached");
					Object.Destroy(rocketInstance);
					return;
				}

				if (_rigidbodyMdlPool.Value.Has(rocketInstanceEntityRef.entity))
				{
					var rocketRigidBody = _rigidbodyMdlPool.Value.Get(rocketInstanceEntityRef.entity).rigidbody;
					rocketRigidBody.isKinematic = true;
				}
				
				spawnerComponent.timePassedSinceReload = 0f;
				spawnerComponent.loadedRocketsEntitiesStack.Push(rocketInstanceEntityRef);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LaunchRocket(ref RadarGuidedRocketSpawnerComponent spawnerComponent)
		{
			spawnerComponent.timePassedSinceRocketLaunch += Time.deltaTime;
			spawnerComponent.timePassedSinceTargetDetection += Time.deltaTime;

			bool isAnyRocketsLoaded = spawnerComponent.loadedRocketsEntitiesStack.Count > 0;
			bool isEnoughTimePassedSinceLastLaunch = spawnerComponent.pauseBetweenLaunchingLoadedRockets <= spawnerComponent.timePassedSinceRocketLaunch;
			bool isEnoughTimePassedFromLastTargetDetection = spawnerComponent.delayBetweenDetectingTargetAndLaunchingRocket <= spawnerComponent.timePassedSinceTargetDetection;

			if(!isAnyRocketsLoaded || !isEnoughTimePassedSinceLastLaunch || !isEnoughTimePassedFromLastTargetDetection)
				return;

			spawnerComponent.timePassedSinceRocketLaunch = 0f;
			
			var rocketEntityRef = spawnerComponent.loadedRocketsEntitiesStack.Pop();
			var rocketTransform = rocketEntityRef.transform;

			if (_rigidbodyMdlPool.Value.Has(rocketEntityRef.entity))
			{
				var rocketRigidBody = _rigidbodyMdlPool.Value.Get(rocketEntityRef.entity).rigidbody;
				rocketRigidBody.isKinematic = false;
			}

			if(_isWaitingForLaunchTagPool.Value.Has(rocketEntityRef.entity))
				_isWaitingForLaunchTagPool.Value.Del(rocketEntityRef.entity);
			
			rocketTransform.SetParent(null);
		}
	}
}