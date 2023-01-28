using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;
using Zenject;

namespace Ingame.Combat
{
	public struct SpawnUnguidedRocketSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<DiContainer> _diContainer;

		private readonly EcsFilterInject<Inc<InputComponent>> _inputCmpFilter;
		private readonly EcsFilterInject<Inc<UnguidedRocketSpawnerComponent>> _rocketSpawnerCmpFilter;
		
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
		private readonly EcsPoolInject<UnguidedRocketSpawnerComponent> _rocketSpawnerCmpPool;

		public void Run(IEcsSystems systems)
		{
			ref var inputCmp = ref _inputCmpPool.Value.Get(_inputCmpFilter.Value.GetRawEntities()[0]);

			if(!inputCmp.shootInput)
				return;

			foreach (var entity in _rocketSpawnerCmpFilter.Value)
			{
				ref var rocketSpawnerCmp = ref _rocketSpawnerCmpPool.Value.Get(entity);

				foreach (var spawnOriginTransform in rocketSpawnerCmp.spawnOriginTransforms)
				{
					if (spawnOriginTransform == null)
						continue;

					_diContainer.Value.InstantiatePrefab
					(
						rocketSpawnerCmp.unguidedRocketPrefab,
						spawnOriginTransform.position,
						Quaternion.LookRotation(spawnOriginTransform.forward),
						null
					);
				}
			}
		}
	}
}