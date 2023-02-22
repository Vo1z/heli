using EcsTools.ObjectPooling;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Combat 
{
	public readonly struct MoveUnguidedProjectileSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, UnguidedProjectileComponent>, Exc<FreeToReuseTag>> _rocketFilter;

		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidBodyMdlPool;
		private readonly EcsPoolInject<UnguidedProjectileComponent> _rocketCmpPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _rocketFilter.Value)
			{
				ref var unguidedRocketCmp = ref _rocketCmpPool.Value.Get(entity);
				var rocketTransform = _transformMdlPool.Value.Get(entity).transform;
				var rocketRigidbody = _rigidBodyMdlPool.Value.Get(entity).rigidbody;

				rocketRigidbody.position += rocketTransform.forward * unguidedRocketCmp.flyingSpeed * Time.deltaTime;
			}
		}
	}
}