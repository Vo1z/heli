using EcsTools.Convertion;
using EcsTools.UnityModels;
using Ingame.Detection;
using Leopotam.EcsLite;
using Tools.HelicopterAttributte;
using UnityEngine;

namespace Ingame.Helicopter
{
	[RequireComponent(typeof(Rigidbody))]
	public sealed class HelicopterBaker : EcsMonoBaker
	{
		[HelicopterId] 
		[SerializeField] private int heliId;

		public override void Bake(int entity, EcsWorld world)
		{
			var transformMdlPool = world.GetPool<TransformModel>();
			var rigidbodyMdlPool = world.GetPool<RigidBodyModel>();
			var heliCmpPool = world.GetPool<HelicopterComponent>();
			var detectionTargetTagPool = world.GetPool<DetectionTargetTag>();

			transformMdlPool.Add(entity).transform = transform;
			rigidbodyMdlPool.Add(entity).rigidbody = GetComponent<Rigidbody>();
			heliCmpPool.Add(entity).helicopterId = heliId;
			detectionTargetTagPool.Add(entity);
		}
	}
}