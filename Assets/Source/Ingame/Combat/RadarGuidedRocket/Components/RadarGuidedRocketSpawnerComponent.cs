using System.Collections.Generic;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace Ingame.Combat
{
	public struct RadarGuidedRocketSpawnerComponent
	{
		public RadarGuidedRocketBaker radarGuidedRocketPrefab;
		
		public Stack<EcsEntityReference> loadedRocketsEntitiesStack;
		
		/// <summary>
		/// Amount of rocket spawn positions define capacity of the spawner
		/// </summary>
		public Transform[] spawnPositions;

		public float rocketReloadDuration;
		public float pauseBetweenLaunchingLoadedRockets;
		public float delayBetweenDetectingTargetAndLaunchingRocket;
		
		public float timePassedSinceReload;
		public float timePassedSinceRocketLaunch;
		public float timePassedSinceTargetDetection;
	}
}