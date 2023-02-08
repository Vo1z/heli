using EcsTools.Convertion;
using Ingame.Health;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using Tools.ClassExtensions;
using UnityEngine;

namespace Ingame.Vfx.Destruction
{
	[RequireComponent(typeof(Collider), typeof(EcsEntityReference))]
	public sealed class DestructibleObjectBaker : EcsMonoBaker
	{
		[BoxGroup("Health")]
		[SerializeField] [EnumFlags] private DamageType appliedDamage;
		
		[BoxGroup("Destruction VFX")]
		[Required, SerializeField] private Transform originalView; 
		[BoxGroup("Destruction VFX")]
		[Required, SerializeField] private Transform destroyedView;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var destructiveObjCmpPool = world.GetPool<DestructibleObjectComponent>();
			var healthCmpPool = world.GetPool<HealthComponent>();

			destructiveObjCmpPool.Add(entity) = new DestructibleObjectComponent
			{
				originalView = originalView,
				destroyedView = destroyedView
			};

			healthCmpPool.Add(entity) = new HealthComponent
			{
				appliedDamageType = (int)appliedDamage,
				currentHealth = 100f
			};
			
			originalView.SetGoActive();
			destroyedView.SetGoInactive();
		}
	}
}