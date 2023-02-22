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
	[RequireComponent(typeof(Rigidbody), typeof(Collider))]
	[RequireComponent(typeof(OnTriggerEventSender), typeof(EcsEntityReference))]
	public sealed class UnguidedProjectileBaker : EcsMonoBaker
	{
		[BoxGroup("Rocket"), SerializeField] [Min(0f)] private float rocketFlyingSpeed = 500f;
		[Tooltip("Amount of time that has to be passed after rocket launch in order to explode")]
		[BoxGroup("Rocket"), SerializeField] [Min(0f)] private float rocketSafeTime = .05f;
		
		[BoxGroup("Explosion"), SerializeField] private DamageType damageType = DamageType.LightExplosive;
		[BoxGroup("Explosion"), SerializeField] [Min(0f)] private float explosionRadius = 5f;
		[BoxGroup("Explosion"), SerializeField] [Min(0f)] private float amountOfDamage = 100f;

		public override void Bake(int entity, EcsWorld world)
		{
			var rocketCmpPool = world.GetPool<UnguidedProjectileComponent>();
			var explosionCmpPool = world.GetPool<ExplosionComponent>();
			var timerCmpPool = world.GetPool<TimerComponent>();
			var transformMdlPool = world.GetPool<TransformModel>();
			var rigidbodyMdlPool = world.GetPool<RigidBodyModel>();

			rocketCmpPool.Add(entity) = new UnguidedProjectileComponent
			{
				flyingSpeed = rocketFlyingSpeed,
				safeTime = rocketSafeTime
			};
			
			explosionCmpPool.Add(entity) = new ExplosionComponent
			{
				damageType = damageType,
				explosionRadius = explosionRadius,
				amountOfDamage = amountOfDamage
			};
			timerCmpPool.Add(entity);
			transformMdlPool.Add(entity).transform = transform;
			rigidbodyMdlPool.Add(entity).rigidbody = GetComponent<Rigidbody>();
		}
	}
}