using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
using Ingame.Combat;
using Ingame.Detection.Vision;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using UnityEngine;
using UnityEngine.AI;

namespace Ingame.Soldiers
{
	[RequireComponent(typeof(NavMeshAgent))]
	public sealed class SoldierBaker : EcsMonoBaker
	{
		[BoxGroup("Soldier")]
		[SerializeField] [Min(0f)] private float travelTimeToDestination = 5f;
		[BoxGroup("Soldier")]
		[SerializeField] [Min(0f)] private float shootingAccuracyError = 5f;
		[BoxGroup("Soldier")]
		[SerializeField] [Min(0f)] private float pauseBetweenShots = 1f;
		
		[BoxGroup("Attached group")]
		[InfoBox("This EcsEntityReference game object must have GroupComponent")]
		[SerializeField] [Min(0f)] private EcsEntityReference boundedGroupEntityReference;
		
		[BoxGroup("Visual detection")]
		[SerializeField] [Min(0f)] private float detectionDistance;

		[BoxGroup("Projectile spawner")]
		[SerializeField, Required] private SoldierBulletProjectileBaker bulletProjectilePrefab;
		[BoxGroup("Projectile spawner")]
		[SerializeField] private Transform[] spawnOriginTransforms;

		public override void Bake(int entity, EcsWorld world)
		{
			var soldierCmpPool = world.GetPool<SoldierComponent>();
			var attachedCmpPool = world.GetPool<AttachedGroupComponent>();
			var navMeshAgentMdlPool = world.GetPool<NavMeshAgentModel>();
			var transformMdlPool = world.GetPool<TransformModel>();
			var visualDetectorCmpPool = world.GetPool<VisualDetectorComponent>();
			var unguidedProjectileSpawnerComponentPool = world.GetPool<UnguidedProjectileSpawnerComponent>();

			soldierCmpPool.TryAdd(entity, new SoldierComponent
			{
				travelTimeToDestination = travelTimeToDestination,
				shootingAccuracyError = shootingAccuracyError,
				pauseBetweenShots = pauseBetweenShots
			});

			attachedCmpPool.TryAdd(entity, new AttachedGroupComponent
			{
				boundedGroupEntityReference = boundedGroupEntityReference
			});

			navMeshAgentMdlPool.TryAdd(entity, new NavMeshAgentModel
			{
				navMeshAgent = GetComponent<NavMeshAgent>()
			});

			transformMdlPool.TryAdd(entity, new TransformModel
			{
				transform = transform
			});
			
			visualDetectorCmpPool.TryAdd(entity, new VisualDetectorComponent
			{
				detectionDistance = detectionDistance
			});

			unguidedProjectileSpawnerComponentPool.TryAdd(entity, new UnguidedProjectileSpawnerComponent
			{
				unguidedProjectilePrefab = bulletProjectilePrefab.transform,
				spawnOriginTransforms = spawnOriginTransforms
			});
		}

		private void OnDrawGizmos()
		{
			if(boundedGroupEntityReference == null)
				return;
			
			var soldierPos = transform.position;
			
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(soldierPos, boundedGroupEntityReference.transform.position);
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(soldierPos, detectionDistance);
		}
	}
}