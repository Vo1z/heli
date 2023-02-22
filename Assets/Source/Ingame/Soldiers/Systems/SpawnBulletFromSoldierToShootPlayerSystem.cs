using EcsTools.ClassExtensions;
using EcsTools.ObjectPooling;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Ingame.Combat;
using Ingame.Detection.Vision;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.EcsExtensions.EntityReference;
using Tools.ClassExtensions;
using UnityEngine;
using Zenject;

namespace Ingame.Soldiers
{
	public readonly struct SpawnBulletFromSoldierToShootPlayerSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<DiContainer> _diContainer;

		private readonly EcsFilterInject<Inc<TransformModel, PlayerTag>> _playerFilter;
		private readonly EcsFilterInject<Inc<TransformModel, SoldierComponent, UnguidedProjectileSpawnerComponent, IsAvailableToVisuallySeeTargetTag>> _soldierFilter;
		private readonly EcsFilterInject<Inc<TransformModel, TimerComponent, UnguidedProjectileComponent, SoldierBulletProjectileTag, FreeToReuseTag>> _freeToReuseBulletsFilter;
		
		private readonly EcsPoolInject<SoldierComponent> _soldierCmpPool;
		private readonly EcsPoolInject<UnguidedProjectileSpawnerComponent> _unguidedProjectileSpawnerCmpPool;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		private readonly EcsPoolInject<FreeToReuseTag> _freeToReuseTagPool;
		private readonly EcsPoolInject<SoldierBulletProjectileTag> _soldierBulletProjectileTagPool;
		
		public void Run(IEcsSystems systems)
		{
			if(_playerFilter.Value.IsEmpty())
				return;

			var playerPos = _transformMdlPool.Value.GetFirstComponent(_playerFilter.Value).transform.position;

			foreach (var entity in _soldierFilter.Value)
			{
				ref var soldierCmp = ref _soldierCmpPool.Value.Get(entity);
				ref var projectileSpawner = ref _unguidedProjectileSpawnerCmpPool.Value.Get(entity);

				soldierCmp.timePassedFromLastShot += Time.deltaTime;
				
				if(soldierCmp.timePassedFromLastShot < soldierCmp.pauseBetweenShots)
					continue;

				soldierCmp.timePassedFromLastShot = 0f;
				var aimTarget = playerPos + Random.insideUnitSphere * soldierCmp.shootingAccuracyError;

				foreach (var spawnOriginTransform in projectileSpawner.spawnOriginTransforms)
				{
					spawnOriginTransform.LookAt(aimTarget);
					SpawnProjectile(projectileSpawner.unguidedProjectilePrefab, spawnOriginTransform);
				}
			}
		}

		private void SpawnProjectile(Transform projectilePrefab, Transform spawnOrigin)
		{
			if (_freeToReuseBulletsFilter.Value.IsEmpty())
			{
				var projectile = _diContainer.Value.InstantiatePrefab
				(
					projectilePrefab,
					spawnOrigin.position,
					Quaternion.LookRotation(spawnOrigin.forward),
					null
				);
				
#if UNITY_EDITOR || DEBUG
				if (projectile.TryGetComponent(out EcsEntityReference projectileEntityReference))
					if (!_soldierBulletProjectileTagPool.Value.Has(projectileEntityReference.entity))
						Debug.LogError($"There is no {nameof(SoldierBulletProjectileTag)} attached to soldier projectile inside {nameof(SpawnBulletFromSoldierToShootPlayerSystem)}");
#endif
				return;
			}

			int projectileEntityToReuse = _freeToReuseBulletsFilter.Value.GetFirstEntity();
			var bulletTransform = _transformMdlPool.Value.Get(projectileEntityToReuse).transform;
			ref var timerCmp = ref _timerCmpPool.Value.Get(projectileEntityToReuse);

			bulletTransform.position = spawnOrigin.position;
			bulletTransform.rotation = spawnOrigin.rotation;
			bulletTransform.SetGoActive();
						
			_freeToReuseTagPool.Value.Del(projectileEntityToReuse);
			timerCmp.timePassed = 0f;
		}
	}
}