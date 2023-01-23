using Ingame.EcsExtensions;
using Ingame.Tools;
using Ingame.UnityModels;
using UnityEngine;

namespace Ingame.Helicopter
{
	[RequireComponent(typeof(Rigidbody))]
	public sealed class HelicopterBaker : EcsBaker
	{
		[HelicopterId] 
		[SerializeField] private int heliId;
		[SerializeField] private Transform rotorTransform;

		protected override void Bake(int entity)
		{
			var transformMdlPool = _world.GetPool<TransformModel>();
			var rigidbodyMdlPool = _world.GetPool<RigidBodyModel>();
			var heliCmpPool = _world.GetPool<HelicopterComponent>();
			var rotorCmpPool = _world.GetPool<RotorComponent>();

			transformMdlPool.Add(entity).transform = transform;
			rigidbodyMdlPool.Add(entity).rigidbody = GetComponent<Rigidbody>();
			heliCmpPool.Add(entity).helicopterId = heliId;
			rotorCmpPool.Add(entity).rotorTransform = rotorTransform;
		}
	}
}