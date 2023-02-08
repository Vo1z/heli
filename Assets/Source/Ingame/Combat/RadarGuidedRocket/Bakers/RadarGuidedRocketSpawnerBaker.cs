using System.Collections.Generic;
using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace Ingame.Combat
{
	public sealed class RadarGuidedRocketSpawnerBaker : EcsMonoBaker
	{
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
		
		public override void Bake(int entity, EcsWorld world)
		{
			var radarGuiderRocketSpawnerCmpPool = world.GetPool<RadarGuidedRocketSpawnerComponent>();

			radarGuiderRocketSpawnerCmpPool.Add(entity) = new RadarGuidedRocketSpawnerComponent
			{
				radarGuidedRocketPrefab = radarGuidedRocketPrefab,
				loadedRocketsEntitiesStack = new Stack<EcsEntityReference>(spawnPositions.Length),
				spawnPositions = spawnPositions,
				rocketReloadDuration = rocketReloadDuration,
				pauseBetweenLaunchingLoadedRockets = pauseBetweenLaunchingLoadedRockets,
				delayBetweenDetectingTargetAndLaunchingRocket = delayBetweenDetectingTargetAndLaunchingRocket
			};
		}
	}
}