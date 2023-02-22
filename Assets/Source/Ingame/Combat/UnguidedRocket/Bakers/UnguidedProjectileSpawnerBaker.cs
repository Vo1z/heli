using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Combat
{
	public sealed class UnguidedProjectileSpawnerBaker : EcsMonoBaker
	{
		[FormerlySerializedAs("unguidedRocketPrefab")] [Required, SerializeField] private UnguidedProjectileBaker unguidedProjectilePrefab;
		[SerializeField] private Transform[] spawnOriginTransforms;

		public override void Bake(int entity, EcsWorld world)
		{
			var rocketSpawnerCmpPool = world.GetPool<UnguidedProjectileSpawnerComponent>();
			
			ref var rocketSpawnerCmp = ref rocketSpawnerCmpPool.Add(entity);

			rocketSpawnerCmp.unguidedProjectilePrefab = unguidedProjectilePrefab;
			rocketSpawnerCmp.spawnOriginTransforms = spawnOriginTransforms;
		}
	}
}