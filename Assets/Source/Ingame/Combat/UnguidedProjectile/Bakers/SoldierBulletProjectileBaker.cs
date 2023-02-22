using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.Physics;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Combat
{
	[RequireComponent(typeof(Rigidbody), typeof(Collider))]
	[RequireComponent(typeof(OnTriggerEventSender), typeof(EcsEntityReference))]
	public sealed class SoldierBulletProjectileBaker : EcsMonoBaker
	{
		[FormerlySerializedAs("rocketFlyingSpeed")] [BoxGroup("Bullet"), SerializeField] [Min(0f)] private float flyingSpeed = 500f;
		[FormerlySerializedAs("rocketSafeTime")]
		[Tooltip("Amount of time that has to be passed after rocket launch in order to explode")]
		[BoxGroup("Bullet"), SerializeField] [Min(0f)] private float safeTime = .05f;
		[BoxGroup("Bullet"), SerializeField] [Min(0f)] private float maxFlyTime = 10f;
		
		[BoxGroup("Explosion"), SerializeField] private DamageType damageType = DamageType.LightFirearm;
		[BoxGroup("Explosion"), SerializeField] [Min(0f)] private float explosionRadius = .3f;
		[BoxGroup("Explosion"), SerializeField] [Min(0f)] private float amountOfDamage = 1f;

		public override void Bake(int entity, EcsWorld world)
		{
			var rocketCmpPool = world.GetPool<UnguidedProjectileComponent>();
			var explosionCmpPool = world.GetPool<ExplosionComponent>();
			var timerCmpPool = world.GetPool<TimerComponent>();
			var transformMdlPool = world.GetPool<TransformModel>();
			var rigidbodyMdlPool = world.GetPool<RigidBodyModel>();
			var soldierBulletProjectileTagPool = world.GetPool<SoldierBulletProjectileTag>();

			rocketCmpPool.Add(entity) = new UnguidedProjectileComponent
			{
				flyingSpeed = flyingSpeed,
				safeTime = safeTime,
				maxFlyTime = maxFlyTime
			};

			explosionCmpPool.TryAdd(entity, new ExplosionComponent
			{
				damageType = damageType,
				explosionRadius = explosionRadius,
				amountOfDamage = amountOfDamage
			});
			
			timerCmpPool.TryAdd(entity);
			
			transformMdlPool.TryAdd(entity, new TransformModel
			{
				transform = transform
			});
			
			rigidbodyMdlPool.TryAdd(entity, new RigidBodyModel
			{
				rigidbody = GetComponent<Rigidbody>()
			});
			
			soldierBulletProjectileTagPool.TryAdd(entity);
		}
	}
}