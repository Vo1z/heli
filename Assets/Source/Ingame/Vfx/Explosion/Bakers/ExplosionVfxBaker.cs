using EcsTools.Convertion;
using EcsTools.ObjectPooling;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Vfx.Explosion
{
	public sealed class ExplosionVfxBaker : EcsMonoBaker
	{
		[BoxGroup("Explosion VFX")]
		[Tooltip("Is used to identify what VFX should be used to show particular damage type effect")]
		[SerializeField] private DamageType damageType = DamageType.LightExplosive;
		[BoxGroup("Explosion VFX")]
		[Required, SerializeField] private ParticleSystem particles;
		[BoxGroup("Pooling")]
		[SerializeField] [Min(0f)] private float lifetime = 2f;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var transformPool = world.GetPool<TransformModel>();
			var particleSystemMdlPool = world.GetPool<ParticleSystemModel>();
			var lifetimeCmpPool = world.GetPool<LifetimeComponent>();
			var timerCmpPool = world.GetPool<TimerComponent>();
			var explosionVfxCmpPool = world.GetPool<ExplosionVfxComponent>();

			transformPool.Add(entity).transform = transform;
			particleSystemMdlPool.Add(entity).particleSystem = particles;
			lifetimeCmpPool.Add(entity).lifetime = lifetime;
			timerCmpPool.Add(entity);
			explosionVfxCmpPool.Add(entity).damageType = damageType;
		}
	}
}