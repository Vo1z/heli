﻿using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Combat
{
	public sealed class UnguidedRocketSpawnerBaker : EcsMonoBaker
	{
		[Required, SerializeField] private UnguidedRocketBaker unguidedRocketPrefab;
		[SerializeField] private Transform[] spawnOriginTransforms;

		public override void Bake(int entity, EcsWorld world)
		{
			var rocketSpawnerCmpPool = world.GetPool<UnguidedRocketSpawnerComponent>();
			
			ref var rocketSpawnerCmp = ref rocketSpawnerCmpPool.Add(entity);

			rocketSpawnerCmp.unguidedRocketPrefab = unguidedRocketPrefab;
			rocketSpawnerCmp.spawnOriginTransforms = spawnOriginTransforms;
		}
	}
}