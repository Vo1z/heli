using EcsExtensions.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Combat
{
	public struct MoveRocketSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, RocketComponent>> _rocketFilter;

		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidBodyMdlPool;
		private readonly EcsPoolInject<RocketComponent> _rocketCmpPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _rocketFilter.Value)
			{
				var rocketTransform = _transformMdlPool.Value.Get(entity).transform;
				var rocketRigidbody = _rigidBodyMdlPool.Value.Get(entity).rigidbody;

				rocketRigidbody.position += rocketTransform.forward * 500f * Time.deltaTime;
			}
		}
	}
}