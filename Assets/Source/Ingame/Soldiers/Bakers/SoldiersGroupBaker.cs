using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace Ingame.Soldiers
{
	[RequireComponent(typeof(EcsEntityReference))]
	public sealed class SoldiersGroupBaker : EcsMonoBaker
	{
		[BoxGroup("Group")]
		[SerializeField] [Min(0)] private float operatingRadius = 10f;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var groupCmpPool = world.GetPool<GroupComponent>();

			groupCmpPool.TryAdd(entity, new GroupComponent
			{
				operatingRadius = operatingRadius
			});
		}

		private void OnDrawGizmos()
		{
			var position = transform.position;
			
			Gizmos.color = Color.blue;

			Gizmos.DrawSphere(position, .5f);
			Gizmos.DrawWireSphere(position, operatingRadius);
		}
	}
}