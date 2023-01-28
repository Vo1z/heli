using EcsTools.Convertion;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ingame.Combat
{
	[RequireComponent(typeof(Rigidbody))]
	public sealed class UnguidedRocketBaker : EcsMonoBaker
	{
		public override void Bake(int entity, EcsWorld world)
		{
			var rocketCmpPool = world.GetPool<UnguidedRocketComponent>();
			var transformMdlPool = world.GetPool<TransformModel>();
			var rigidbodyMdlPool = world.GetPool<RigidBodyModel>();

			rocketCmpPool.Add(entity);
			transformMdlPool.Add(entity).transform = transform;
			rigidbodyMdlPool.Add(entity).rigidbody = GetComponent<Rigidbody>();
		}
	}
}