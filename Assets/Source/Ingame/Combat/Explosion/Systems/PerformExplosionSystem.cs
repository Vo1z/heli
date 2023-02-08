using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Ingame.Health;
using Ingame.Vfx.Explosion;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace Ingame.Combat
{
	public sealed class PerformExplosionSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _world = default;

		private readonly EcsFilterInject<Inc<TransformModel, ExplosionComponent, PerformExplosionTag>> _performExplosionFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<ExplosionComponent> _explosionCmpPool;
		private readonly EcsPoolInject<PerformExplosionTag> _performExplosionTagPool;
		private readonly EcsPoolInject<HealthComponent> _healthCmpPool;
		private readonly EcsPoolInject<ApplyDamageComponent> _applyDamageCmpPool;
		private readonly EcsPoolInject<DeleteEntityAfterExplosionTag> _deleteEntityAfterExplosionTagPool;

		private readonly Collider[] _sphereCastCollidersBuffer = new Collider[64];
		
		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _performExplosionFilter.Value)
			{
				var explosionOriginTransform = _transformMdlPool.Value.Get(entity).transform;
				ref var explosionCmp = ref _explosionCmpPool.Value.Get(entity);

				_performExplosionTagPool.Value.Del(entity);
				_world.Value.SendSignal(new SpawnExplosionVfxRequest
				{
					position = explosionOriginTransform.position,
					damageType = explosionCmp.damageType
				});

				int foundCollidersCount = Physics.OverlapSphereNonAlloc
				(
					explosionOriginTransform.position,
					explosionCmp.explosionRadius,
					_sphereCastCollidersBuffer
				);

				for (int i = 0; i < foundCollidersCount; i++)
				{
					var hitCollider = _sphereCastCollidersBuffer[i];
					
					if(!hitCollider.TryGetComponent(out EcsEntityReference entityRef))
						continue;

					if(!_healthCmpPool.Value.Has(entityRef.entity) || _applyDamageCmpPool.Value.Has(entityRef.entity))
						continue;

					_applyDamageCmpPool.Value.Add(entityRef.entity) = new ApplyDamageComponent
					{
						damageType = explosionCmp.damageType,
						amountOfDamage = explosionCmp.amountOfDamage
					};
				}

				if (_deleteEntityAfterExplosionTagPool.Value.Has(entity))
				{
					Object.Destroy(explosionOriginTransform.gameObject);
					_world.Value.DelEntity(entity);
				}
			}
		}
	}
}