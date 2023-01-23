using Ingame.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Helicopter
{
	public struct MoveHelicopterSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, HelicopterComponent>> _helicopterFilter;
		
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidbodyMdlPool;
		private readonly EcsPoolInject<HelicopterComponent> _helicopterCmpPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var helicopterEntity in _helicopterFilter.Value)
			{
				ref var helicopterCmp = ref _helicopterCmpPool.Value.Get(helicopterEntity);
				var heliTransform = _transformMdlPool.Value.Get(helicopterEntity).transform;
				var heliRigidbody = _rigidbodyMdlPool.Value.Get(helicopterEntity).rigidbody;

				var throttleForth = heliTransform.up * helicopterCmp.currentThrottle;
				var pitchTorque = heliTransform.right * helicopterCmp.currentThrottle;
				var yawTorque = heliTransform.up * helicopterCmp.currentThrottle;
				var rollTorque = -heliTransform.forward * helicopterCmp.currentThrottle;

				heliRigidbody.AddForce(throttleForth, ForceMode.Impulse);
					
				heliRigidbody.AddTorque(pitchTorque);
				heliRigidbody.AddTorque(yawTorque);
				heliRigidbody.AddTorque(rollTorque);
			}
		}
	}
}