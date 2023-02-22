using EcsTools.ClassExtensions;
using EcsTools.ObjectPooling;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using Tools.ClassExtensions;
using UnityEngine;
using Zenject;

namespace Ingame.Combat
{
	public sealed class SpawnUnguidedProjectileSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _worldProject = "project";
		private readonly EcsCustomInject<DiContainer> _diContainer;

		private readonly EcsFilterInject<Inc<UnguidedProjectileSpawnerComponent>> _rocketSpawnerCmpFilter;
		private readonly EcsPoolInject<UnguidedProjectileSpawnerComponent> _rocketSpawnerCmpPool;
		
		private readonly EcsFilterInject<Inc<TransformModel, TimerComponent, UnguidedProjectileComponent, FreeToReuseTag>> _freeReusedRocketsFilter;
		private readonly EcsPoolInject<TransformModel> _transformModelPool;
		private readonly EcsPoolInject<UnguidedProjectileComponent> _unguidedRocketCmp;
		private readonly EcsPoolInject<FreeToReuseTag> _freeToReuseEntityTagPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		
		public void Run(IEcsSystems systems)
		{
			var inputCmpFilter = _worldProject.Value.Filter<InputComponent>().End();
			var inputCmpPool = _worldProject.Value.GetPool<InputComponent>();

			ref var inputCmp = ref inputCmpPool.GetFirstComponent(inputCmpFilter);

			if(!inputCmp.shootInput)
				return;
			
			foreach (var rocketSpawnerEntity in _rocketSpawnerCmpFilter.Value)
			{
				ref var rocketSpawnerCmp = ref _rocketSpawnerCmpPool.Value.Get(rocketSpawnerEntity);

				foreach (var spawnOriginTransform in rocketSpawnerCmp.spawnOriginTransforms)
				{
					if (spawnOriginTransform == null)
						continue;

					if (!_freeReusedRocketsFilter.Value.IsEmpty())
					{
						int rocketEntityToReuse = _freeReusedRocketsFilter.Value.GetFirstEntity();
						var rocketTransform = _transformModelPool.Value.Get(rocketEntityToReuse).transform;
						ref var timerCmp = ref _timerCmpPool.Value.Get(rocketEntityToReuse);

						rocketTransform.position = spawnOriginTransform.position;
						rocketTransform.rotation = spawnOriginTransform.rotation;
						rocketTransform.SetGoActive();
						
						_freeToReuseEntityTagPool.Value.Del(rocketEntityToReuse);
						timerCmp.timePassed = 0f;

						continue;
					}

					_diContainer.Value.InstantiatePrefab
					(
						rocketSpawnerCmp.unguidedProjectilePrefab,
						spawnOriginTransform.position,
						Quaternion.LookRotation(spawnOriginTransform.forward),
						null
					);
				}
			}
		}
	}
}