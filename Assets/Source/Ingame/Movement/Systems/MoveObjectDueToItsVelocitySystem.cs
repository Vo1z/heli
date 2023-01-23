using Ingame.Movement;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.UnityModels;
using UnityEngine;

namespace Source.Ingame.Movement
{
	public readonly struct MoveObjectDueToItsVelocitySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<VelocityComponent, TransformModel>> _velocityObjectFilter;
		
		private readonly EcsPoolInject<VelocityComponent> _velocityPool;
		private readonly EcsPoolInject<TransformModel> _transformPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _velocityObjectFilter.Value)
			{
				ref var velocityCmp = ref _velocityPool.Value.Get(entity);
				ref var transformModel = ref _transformPool.Value.Get(entity);

				transformModel.transform.position += velocityCmp.velocity * Time.deltaTime;
			}
		}
	}
}