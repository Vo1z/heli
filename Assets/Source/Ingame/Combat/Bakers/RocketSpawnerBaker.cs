using EcsExtensions.Convertion;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ingame.Combat
{
	public sealed class RocketSpawnerBaker : EcsMonoBaker
	{
		[SerializeField] private RocketBaker rocketPrefab;
		[SerializeField] private Transform[] spawnOriginTransforms;

		public override void Bake(int entity, EcsWorld world)
		{
			var rocketSpawnerCmpPool = world.GetPool<RocketSpawnerComponent>();
			
			ref var rocketSpawnerCmp = ref rocketSpawnerCmpPool.Add(entity);

			rocketSpawnerCmp.rocketPrefab = rocketPrefab;
			rocketSpawnerCmp.spawnOriginTransforms = spawnOriginTransforms;
		}
	}
}