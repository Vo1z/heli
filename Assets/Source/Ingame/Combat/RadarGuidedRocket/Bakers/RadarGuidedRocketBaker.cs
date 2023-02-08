using EcsTools.Convertion;
using EcsTools.Physics;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace Ingame.Combat
{
	[RequireComponent(typeof(EcsEntityReference), typeof(OnTriggerEventSender), typeof(Rigidbody))]
	public sealed class RadarGuidedRocketBaker : EcsMonoBaker 
	{
		[BoxGroup("Radar guided rocket")]
		[SerializeField] [Min(0f)] private float movementSpeed = 200f;
		[BoxGroup("Radar guided rocket")]
		[SerializeField] [Range(0, 1f)] private float rotationDumping = .3f;
		[Tooltip("Amount of time that has to be passed after rocket launch in order to explode")]
		[BoxGroup("Radar guided rocket")]
		[SerializeField] [Range(0, 1f)] private float safeTime = .05f;
		[Tooltip("How many seconds should pass after missing the target in order to explode")]
		[BoxGroup("Radar guided rocket")]
		[SerializeField] [Range(0, 5f)] private float lifetimeAfterMiss = 1f;
		
		[BoxGroup("Explosion")]
		[SerializeField] private DamageType damageType;
		[BoxGroup("Explosion")]
		[SerializeField] [Min(0f)] private float explosionRadius;
		[BoxGroup("Explosion")]
		[SerializeField] [Min(0f)] private float amountOfDamage;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var radarGuidedRocketCmpPool = world.GetPool<RadarGuidedRocketComponent>();
			var isWaitingForLaunchTagPool = world.GetPool<IsWaitingForLaunchTag>();
			var explosionCmpPool = world.GetPool<ExplosionComponent>();
			var timerCmpPool = world.GetPool<TimerComponent>();
			var transformMdlPool = world.GetPool<TransformModel>();
			var rigidBodyMdlPool = world.GetPool<RigidBodyModel>();

			radarGuidedRocketCmpPool.Add(entity) = new RadarGuidedRocketComponent
			{
				movementSpeed = movementSpeed,
				rotationDumping = rotationDumping,
				safeTime = safeTime,
				lifetimeAfterMiss = lifetimeAfterMiss
			};
			isWaitingForLaunchTagPool.Add(entity);
			explosionCmpPool.Add(entity) = new ExplosionComponent
			{
				damageType = damageType,
				explosionRadius = explosionRadius,
				amountOfDamage = amountOfDamage
			};
			timerCmpPool.Add(entity);
			transformMdlPool.Add(entity).transform = transform;
			rigidBodyMdlPool.Add(entity).rigidbody = GetComponent<Rigidbody>();
		}
	}
}