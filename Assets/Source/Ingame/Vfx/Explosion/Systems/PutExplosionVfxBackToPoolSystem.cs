using EcsTools.ObjectPooling;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Tools.ClassExtensions;

namespace Ingame.Vfx.Explosion
{
	public readonly struct PutExplosionVfxBackToPoolSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, TimerComponent, LifetimeComponent, ExplosionVfxComponent>, Exc<FreeToReuseTag>> _explosionVfxFilter;
		
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		private readonly EcsPoolInject<LifetimeComponent> _lifetimeCmpPool;
		private readonly EcsPoolInject<FreeToReuseTag> _freeToReuseTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _explosionVfxFilter.Value)
			{
				ref var timerCmp = ref _timerCmpPool.Value.Get(entity);
				ref var lifetimeCmp = ref _lifetimeCmpPool.Value.Get(entity);
				
				if(lifetimeCmp.lifetime > timerCmp.timePassed)
					continue;

				var vfxTransform = _transformMdlPool.Value.Get(entity).transform;
				
				vfxTransform.SetGoInactive();
				_freeToReuseTagPool.Value.Add(entity);
			}
		}
	}
}