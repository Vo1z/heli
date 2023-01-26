using EcsExtensions.UnityModels;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Helicopter
{
	public struct CalculateHelicopterStatsSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, HelicopterComponent, PlayerTag>> _playerHeliFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidbodyMdlPool;
		private readonly EcsPoolInject<HelicopterComponent> _heliCmpPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _playerHeliFilter.Value)
			{
				ref var heliCmp = ref _heliCmpPool.Value.Get(entity);
				var heliTransform = _transformMdlPool.Value.Get(entity).transform;
				var heliRigidbody = _rigidbodyMdlPool.Value.Get(entity).rigidbody;
				var heliPosition = heliTransform.position;
				
				var raycastRay = new Ray(heliPosition, Vector3.down);
				int layerMask = ~LayerMask.GetMask("Ignore Raycast", "Player");

				if (!Physics.Raycast(raycastRay, out RaycastHit hit, 9999, layerMask, QueryTriggerInteraction.Ignore))
					heliCmp.currentAltitude = -1f;
				else
					heliCmp.currentAltitude = Vector3.Distance(heliPosition, hit.point);

				heliCmp.currentSpeed = heliRigidbody.velocity.magnitude;
			}
		}
	}
}