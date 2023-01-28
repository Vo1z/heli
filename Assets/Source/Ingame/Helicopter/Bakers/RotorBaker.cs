using EcsTools.Convertion;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ingame.Helicopter
{
	public sealed class RotorBaker : EcsMonoBaker
	{
		[SerializeField] [Min(0f)] private float rotorSpeed;
		[SerializeField] private Vector3 rotateAroundVector = Vector3.up;

		public override void Bake(int entity, EcsWorld world)
		{
			var transformMdlPool = world.GetPool<TransformModel>();
			var rotorCmpPool = world.GetPool<RotorComponent>();

			transformMdlPool.Add(entity).transform = transform;
			rotorCmpPool.Add(entity) = new RotorComponent
			{
				speed = rotorSpeed,
				rotateAroundVector = rotateAroundVector
			};
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			var currentPos = transform.position;
			
			Gizmos.DrawLine(currentPos, currentPos + rotateAroundVector);
		}
	}
}