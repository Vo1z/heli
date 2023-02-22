using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
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
		
		[BoxGroup("Attached group")]
		[InfoBox("This EcsEntityReference game object must have GroupComponent")]
		[SerializeField] [Min(0f)] private EcsEntityReference boundedGroupEntityReference;
		
		[BoxGroup("Visual detection")]
		[SerializeField] [Min(0f)] private float detectionDistance;

		public override void Bake(int entity, EcsWorld world)
		{
			var soldierCmpPool = world.GetPool<SoldierComponent>();
			var attachedCmpPool = world.GetPool<AttachedGroupComponent>();
			var navMeshAgentMdlPool = world.GetPool<NavMeshAgentModel>();
			var transformMdlPool = world.GetPool<TransformModel>();
			var visualDetectorCmp = world.GetPool<VisualDetectorComponent>();

			soldierCmpPool.TryAdd(entity, new SoldierComponent
			{
				travelTimeToDestination = travelTimeToDestination
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
			
			visualDetectorCmp.TryAdd(entity, new VisualDetectorComponent
			{
				detectionDistance = detectionDistance
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