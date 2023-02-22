using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
using Ingame.Detection.Radar;
using Ingame.Detection.Vision;
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
			var detectionTargetTagPool = world.GetPool<RadarDetectionTargetTag>();
			var visualDetectionTargetTagPool = world.GetPool<VisualDetectionTargetTag>();

			transformMdlPool.TryAdd(entity, new TransformModel
			{
				transform = transform
			});
			
			rigidbodyMdlPool.TryAdd(entity, new RigidBodyModel
			{
				rigidbody = GetComponent<Rigidbody>()
			});
			
			heliCmpPool.TryAdd(entity, new HelicopterComponent
			{
				helicopterId = heliId
			});
			
			detectionTargetTagPool.TryAdd(entity);
			visualDetectionTargetTagPool.TryAdd(entity);
		}
	}
}