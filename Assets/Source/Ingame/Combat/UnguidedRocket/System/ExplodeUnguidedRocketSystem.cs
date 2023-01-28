using EcsTools.Physics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.EcsExtensions.EntityReference;

namespace Ingame.Combat
{
	public readonly struct ExplodeUnguidedRocketSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<OnTriggerEnterRequest>> _onTriggerEnterReqFilter;
		private readonly EcsPoolInject<OnTriggerEnterRequest> _onTriggerEnterReqPool;
		private readonly EcsPoolInject<UnguidedRocketComponent> _rocketPool;
		private readonly EcsPoolInject<ExplosionComponent> _explosionCmpPool;
		private readonly EcsPoolInject<PerformExplosionTag> _performExplosionTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _onTriggerEnterReqFilter.Value)
			{
				ref var onTriggerEnterReq = ref _onTriggerEnterReqPool.Value.Get(entity);

				if (!onTriggerEnterReq.sender.TryGetComponent(out EcsEntityReference entityRef))
					return;

				bool hasRocketCmp = _rocketPool.Value.Has(entityRef.entity);
				bool hasExplosionCmp = _explosionCmpPool.Value.Has(entityRef.entity);
				bool hasPerformExplosionTag = _performExplosionTagPool.Value.Has(entityRef.entity);
				
				if(!hasRocketCmp || !hasExplosionCmp || hasPerformExplosionTag)
					return;

				_performExplosionTagPool.Value.Add(entityRef.entity);
			}
		}
	}
}