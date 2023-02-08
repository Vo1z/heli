using System.Collections.Generic;
using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
using Ingame.Combat;
using Ingame.Health;
using Ingame.Vehicle.Turret;
using Ingame.Vfx.Destruction;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using Tools.ClassExtensions;
using UnityEngine;

namespace Ingame.Enemy
{
	[RequireComponent(typeof(EcsEntityReference), typeof(Collider))]
	public sealed class KubeAntiAirEnemyBaker : EcsMonoBaker
	{
		[BoxGroup("Health")]
		[SerializeField] [EnumFlags] private DamageType appliedDamage;
		
		[BoxGroup("Radar guided rocket spawner")]
		[Required, SerializeField] private RadarGuidedRocketBaker radarGuidedRocketPrefab;
		[BoxGroup("Radar guided rocket spawner")]
		[InfoBox("Amount of spawn positions define capacity of the spawner")]
		[SerializeField] private Transform[] spawnPositions;
		[BoxGroup("Radar guided rocket spawner")]
		[SerializeField] [Min(0f)] private float rocketReloadDuration;
		[BoxGroup("Radar guided rocket spawner")]
		[SerializeField] [Min(0f)] private float pauseBetweenLaunchingLoadedRockets;
		[BoxGroup("Radar guided rocket spawner")]
		[SerializeField] [Min(0f)] private float delayBetweenDetectingTargetAndLaunchingRocket;
		
		[BoxGroup("Turret")]
		[Required, SerializeField] private Transform horizontalRotatorTransform;
		[BoxGroup("Turret")]
		[Required, SerializeField] private Transform verticalRotatorTransform;
		[BoxGroup("Turret")]
		[SerializeField] [Range(0, 1f)] private float rotationDumping = .05f;

		[BoxGroup("Destruction VFX")]
		[Required, SerializeField] private Transform originalView; 
		[BoxGroup("Destruction VFX")]
		[Required, SerializeField] private Transform destroyedView;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var radarGuiderRocketSpawnerCmpPool = world.GetPool<RadarGuidedRocketSpawnerComponent>();
			var turretCmpPool = world.GetPool<TurretComponent>();
			var rotateTurretTowardRadarTargetTagPool = world.GetPool<RotateTurretTowardRadarTargetTag>();
			var destructiveObjCmpPool = world.GetPool<DestructibleObjectComponent>();
			var healthCmpPool = world.GetPool<HealthComponent>();
			var transformMdl = world.GetPool<TransformModel>();

			radarGuiderRocketSpawnerCmpPool.TryAdd(entity, new RadarGuidedRocketSpawnerComponent
			{
				radarGuidedRocketPrefab = radarGuidedRocketPrefab,
				loadedRocketsEntitiesStack = new Stack<EcsEntityReference>(spawnPositions.Length),
				spawnPositions = spawnPositions,
				rocketReloadDuration = rocketReloadDuration,
				pauseBetweenLaunchingLoadedRockets = pauseBetweenLaunchingLoadedRockets,
				delayBetweenDetectingTargetAndLaunchingRocket = delayBetweenDetectingTargetAndLaunchingRocket
			});
			
			turretCmpPool.TryAdd(entity, new TurretComponent
			{
				horizontalRotatorTransform = horizontalRotatorTransform,
				verticalRotatorTransform = verticalRotatorTransform,
				rotationDumping = rotationDumping
			});

			rotateTurretTowardRadarTargetTagPool.TryAdd(entity);
			
			transformMdl.TryAdd(entity, new TransformModel
			{
				transform = transform
			});

			destructiveObjCmpPool.TryAdd(entity, new DestructibleObjectComponent
			{
				originalView = originalView,
				destroyedView = destroyedView
			});

			healthCmpPool.TryAdd(entity, new HealthComponent
			{
				appliedDamageType = (int)appliedDamage,
				currentHealth = 100f
			});
			
			originalView.SetGoActive();
			destroyedView.SetGoInactive();
		}
	}
}