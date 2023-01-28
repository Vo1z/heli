using EcsTools.ClassExtensions;
using EcsTools.ObjectPooling;
using EcsTools.Physics;
using EcsTools.Timer;
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
		private readonly EcsPoolInject<FreeToReuseEntityTag> _freeToReuseTagPool;
		private readonly EcsPoolInject<TimerComponent> _timerCompPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _onTriggerEnterReqFilter.Value)
			{
				ref var onTriggerEnterReq = ref _onTriggerEnterReqPool.Value.Get(entity);

				if (!onTriggerEnterReq.sender.TryGetComponent(out EcsEntityReference entityRef))
					continue;

				bool hasRocketCmp = _rocketPool.Value.Has(entityRef.entity);
				bool hasExplosionCmp = _explosionCmpPool.Value.Has(entityRef.entity);
				bool hasTimerCmp = _timerCompPool.Value.Has(entityRef.entity);
				bool hasPerformExplosionTag = _performExplosionTagPool.Value.Has(entityRef.entity);
				bool wasRocketUsedAlready = _freeToReuseTagPool.Value.Has(entityRef.entity);
				
				if(!hasRocketCmp || !hasExplosionCmp || !hasTimerCmp || hasPerformExplosionTag || wasRocketUsedAlready)
					continue;
				
				ref var unguidedRocketCmp = ref _rocketPool.Value.Get(entityRef.entity); 
				ref var timerCmp = ref _timerCompPool.Value.Get(entityRef.entity);

				if(unguidedRocketCmp.safeTime > timerCmp.timePassed)
					continue;
				
				entityRef.SetGoInactive();
				
				_performExplosionTagPool.Value.Add(entityRef.entity);
				_freeToReuseTagPool.Value.Add(entityRef.entity);
			}
		}
	}
}