using EcsTools.ClassExtensions;
using EcsTools.ObjectPooling;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Ingame.ConfigProvision;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Tools.ClassExtensions;
using UnityEngine;
using Zenject;

namespace Ingame.Vfx.Explosion
{
	public readonly struct SpawnExplosionVfxSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<DiContainer> _diContainer;
		private readonly EcsCustomInject<ConfigProvider> _configProvider;

		private readonly EcsFilterInject<Inc<TransformModel, TimerComponent, ExplosionVfxComponent, FreeToReuseTag>> _reuseExplosionVfxFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		private readonly EcsPoolInject<FreeToReuseTag> _freeToReuseTagPool;

		private readonly EcsFilterInject<Inc<SpawnExplosionVfxRequest>> _spawnExplosionVfxReqFilter;
		private readonly EcsPoolInject<SpawnExplosionVfxRequest> _spawnExplosionVfxReqPool;
		
		private readonly EcsPoolInject<ParticleSystemModel> _particleSystemMdlPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var spawnReqEntity in _spawnExplosionVfxReqFilter.Value)
			{
				var spawnExplosionVfxReq = _spawnExplosionVfxReqPool.Value.Get(spawnReqEntity);
				
				_spawnExplosionVfxReqPool.Value.Del(spawnReqEntity);

				if (!_reuseExplosionVfxFilter.Value.IsEmpty())
				{
					int reusedExplosionVfxEntity = _reuseExplosionVfxFilter.Value.GetFirstEntity();
					var explosionVfxTransform = _transformMdlPool.Value.Get(reusedExplosionVfxEntity).transform;
					ref var timerCmp = ref _timerCmpPool.Value.Get(reusedExplosionVfxEntity);
					
					explosionVfxTransform.position = spawnExplosionVfxReq.position;
					explosionVfxTransform.SetGoActive();
					timerCmp.timePassed = 0f;
					
					_freeToReuseTagPool.Value.Del(reusedExplosionVfxEntity);

					if (_particleSystemMdlPool.Value.Has(reusedExplosionVfxEntity))
					{
						var particleSystem = _particleSystemMdlPool.Value.Get(reusedExplosionVfxEntity).particleSystem;
						particleSystem.Clear();
						particleSystem.Play();
					}
					
					continue;
				}
				
				var explosionPrefab = _configProvider.Value.vfxConfig.GetExplosionVfxPrefab(spawnExplosionVfxReq.damageType);
				_diContainer.Value.InstantiatePrefab(explosionPrefab, spawnExplosionVfxReq.position, Quaternion.identity, null);
			}
		}
	}
}