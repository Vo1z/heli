using EcsTools.Physics;
using EcsTools.Timer;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace Ingame.Combat
{
	public readonly struct ExplodeRadarGuidedRocketSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<OnTriggerEnterRequest>> _onTriggerEnterReqFilter;
		private readonly EcsPoolInject<OnTriggerEnterRequest> _onTriggerEnterReqPool;
		private readonly EcsPoolInject<RadarGuidedRocketComponent> _radarGuidedRocketPool;
		private readonly EcsPoolInject<ExplosionComponent> _explosionCmpPool;
		private readonly EcsPoolInject<PerformExplosionTag> _performExplosionTagPool;
		private readonly EcsPoolInject<DeleteEntityAfterExplosionTag> _deleteEntityAfterExplosionTagPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		private readonly EcsPoolInject<IsWaitingForLaunchTag> _isWaitingForLaunchTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var triggerEntity in _onTriggerEnterReqFilter.Value)
			{
				ref var onTriggerEnterReq = ref _onTriggerEnterReqPool.Value.Get(triggerEntity);
				
				if (!onTriggerEnterReq.sender.TryGetComponent(out EcsEntityReference entityRef))
					continue;

				bool hasRocketCmp = _radarGuidedRocketPool.Value.Has(entityRef.entity);
				bool hasExplosionCmp = _explosionCmpPool.Value.Has(entityRef.entity);
				bool hasTimerCmp = _timerCmpPool.Value.Has(entityRef.entity);
				bool isRocketLaunched = !_isWaitingForLaunchTagPool.Value.Has(entityRef.entity);
				bool hasPerformExplosionTag = _performExplosionTagPool.Value.Has(entityRef.entity);

				if(!hasRocketCmp || !hasExplosionCmp || !hasTimerCmp || !isRocketLaunched || hasPerformExplosionTag)
					continue;

				ref var rocketCmp = ref _radarGuidedRocketPool.Value.Get(entityRef.entity); 
				ref var timerCmp = ref _timerCmpPool.Value.Get(entityRef.entity);

				if(rocketCmp.safeTime > timerCmp.timePassed)
					continue;

				_performExplosionTagPool.Value.Add(entityRef.entity);
				_deleteEntityAfterExplosionTagPool.Value.Add(entityRef.entity);
			}
		}
	}
}