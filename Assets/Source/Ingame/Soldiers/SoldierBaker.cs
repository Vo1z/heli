using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
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
		[SerializeField] [Min(0f)] private float travelTimeToDestination;
		
		[BoxGroup("Attached group")]
		[InfoBox("This EcsEntityReference game object must have GroupComponent")]
		[SerializeField] [Min(0f)] private EcsEntityReference boundedGroupEntityReference;

		public override void Bake(int entity, EcsWorld world)
		{
			var soldierCmpPool = world.GetPool<SoldierComponent>();
			var attachedCmpPool = world.GetPool<AttachedGroupComponent>();
			var navMeshAgentMdlPool = world.GetPool<NavMeshAgentModel>();

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
		}

		private void OnDrawGizmos()
		{
			if(boundedGroupEntityReference == null)
				return;
			
			Gizmos.color = Color.blue;
			
			Gizmos.DrawLine(transform.position, boundedGroupEntityReference.transform.position);
		}
	}
}