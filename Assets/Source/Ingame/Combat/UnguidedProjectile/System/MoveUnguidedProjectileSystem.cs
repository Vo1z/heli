using EcsTools.ObjectPooling;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Tools.ClassExtensions;
using UnityEngine;

namespace Ingame.Combat 
{
	public readonly struct MoveUnguidedProjectileSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, UnguidedProjectileComponent, TimerComponent>, Exc<FreeToReuseTag>> _projectileFilter;

		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidBodyMdlPool;
		private readonly EcsPoolInject<UnguidedProjectileComponent> _projectileCmpPool;
		
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		private readonly EcsPoolInject<PerformExplosionTag> _performExplosionTagPool;
		private readonly EcsPoolInject<FreeToReuseTag> _freeToReuseTagPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _projectileFilter.Value)
			{
				ref var unguidedProjectileCmp = ref _projectileCmpPool.Value.Get(entity);
				ref var timerCmp = ref _timerCmpPool.Value.Get(entity);
				var projectileTransform = _transformMdlPool.Value.Get(entity).transform;
				var projectileRigidbody = _rigidBodyMdlPool.Value.Get(entity).rigidbody;

				projectileRigidbody.position += projectileTransform.forward * unguidedProjectileCmp.flyingSpeed * Time.deltaTime;

				if (timerCmp.timePassed > unguidedProjectileCmp.maxFlyTime)
				{
					projectileTransform.SetGoInactive();
					
					_performExplosionTagPool.Value.Add(entity);
					_freeToReuseTagPool.Value.Add(entity);
				}
			}
		}
	}
}