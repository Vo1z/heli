using System.Runtime.CompilerServices;
using EcsTools.UnityModels;
using Ingame.Detection;
using Ingame.Detection.Radar;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Combat
{
	public sealed class MoveRadarGuidedRocketSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RadarGuidedRocketComponent>, Exc<IsWaitingForLaunchTag, PerformExplosionTag>> _radarGuidedRocketFilter;
		private readonly EcsFilterInject<Inc<TransformModel, IsRadarDetectedTag>> _rocketTargetFilter;
		
		private readonly EcsPoolInject<RadarGuidedRocketComponent> _radarGuidedRocketCmpPool;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		
		private readonly EcsPoolInject<ExplosionComponent> _explosionCmpPool;
		private readonly EcsPoolInject<PerformExplosionTag> _performExplosionTagPool;
		private readonly EcsPoolInject<DeleteEntityAfterExplosionTag> _deleteEntityAfterExplosionTagPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var rocketEntity in _radarGuidedRocketFilter.Value)
			{
				ref var radarGuidedRocketCmp = ref _radarGuidedRocketCmpPool.Value.Get(rocketEntity); 
				var rocketTransform = _transformMdlPool.Value.Get(rocketEntity).transform;
				var rocketPos = rocketTransform.position;
				
				Transform closestTargetTransform = null;
				float smallestDistanceToTarget = float.MaxValue;

				foreach (var targetEntity in _rocketTargetFilter.Value)
				{
					var targetTransform = _transformMdlPool.Value.Get(targetEntity).transform;
					float distanceToTheTarget = Vector3.SqrMagnitude(targetTransform.position - rocketPos);
					
					if(smallestDistanceToTarget <= distanceToTheTarget)
						continue;

					smallestDistanceToTarget = distanceToTheTarget;
					closestTargetTransform = targetTransform;
				}

				if (radarGuidedRocketCmp.squaredDistanceToTheTarget < smallestDistanceToTarget)
					radarGuidedRocketCmp.timePassedMovingFartherFromTarget += Time.deltaTime;

				radarGuidedRocketCmp.squaredDistanceToTheTarget = smallestDistanceToTarget;

				if (radarGuidedRocketCmp.timePassedMovingFartherFromTarget >= radarGuidedRocketCmp.lifetimeAfterMiss)
					ExplodeRocket(rocketEntity, rocketTransform.gameObject);

				MoveRocketTowardsTarget(radarGuidedRocketCmp, rocketTransform, closestTargetTransform);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void MoveRocketTowardsTarget(in RadarGuidedRocketComponent guidedRocketComponent, Transform rocketTransform, Transform targetTransform)
		{
			rocketTransform.position += rocketTransform.forward * guidedRocketComponent.movementSpeed * Time.deltaTime;
			
			if(targetTransform == null)
				return;
			
			var targetRotation = Quaternion.LookRotation(targetTransform.position - rocketTransform.position);
			rocketTransform.rotation = Quaternion.Slerp(rocketTransform.rotation, targetRotation, 1 - Mathf.Pow(guidedRocketComponent.rotationDumping, Time.deltaTime));
		}

		private void ExplodeRocket(in int rocketEntity, GameObject rocketGameObject)
		{
			_performExplosionTagPool.Value.Add(rocketEntity);
			_deleteEntityAfterExplosionTagPool.Value.Add(rocketEntity);
			
			Object.Destroy(rocketGameObject);
		}
	}
}