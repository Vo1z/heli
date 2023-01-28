using System.Runtime.CompilerServices;
using EcsTools.UnityModels;
using Ingame.ConfigProvision;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Helicopter
{
	public struct MoveHelicopterSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, HelicopterComponent>> _helicopterFilter;
		
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidbodyMdlPool;
		private readonly EcsPoolInject<HelicopterComponent> _helicopterCmpPool;

		public void Init(IEcsSystems systems)
		{
			foreach (var helicopterEntity in _helicopterFilter.Value)
			{
				// ref var helicopterCmp = ref _helicopterCmpPool.Value.Get(helicopterEntity);
				// var heliTransform = _transformMdlPool.Value.Get(helicopterEntity).transform;
				// var heliRigidbody = _rigidbodyMdlPool.Value.Get(helicopterEntity).rigidbody;
				//
				// heliRigidbody.maxDepenetrationVelocity = .1f;
				// heliRigidbody.maxAngularVelocity = .1f;
			}
		}
		
		public void Run(IEcsSystems systems)
		{
			foreach (var helicopterEntity in _helicopterFilter.Value)
			{
				ref var helicopterCmp = ref _helicopterCmpPool.Value.Get(helicopterEntity);
				var heliTransform = _transformMdlPool.Value.Get(helicopterEntity).transform;
				var heliRigidbody = _rigidbodyMdlPool.Value.Get(helicopterEntity).rigidbody;
				var heliConfigData = _configProvider.Value.HelicoptersConfig.GetHelicopterConfigData(helicopterCmp.helicopterId);

				ApplyControls(heliTransform, heliRigidbody, helicopterCmp, heliConfigData);
				ApplyNormalization(heliRigidbody, heliConfigData);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ApplyControls(Transform heliTransform, Rigidbody heliRigidbody, in HelicopterComponent helicopterCmp, in HelicopterConfigData heliConfig)
		{
			var throttleForth = heliTransform.up * helicopterCmp.currentThrottle * Time.fixedDeltaTime;
			var pitchTorque = heliTransform.right * helicopterCmp.currentPitch * Time.fixedDeltaTime;
			var yawTorque = heliTransform.up * helicopterCmp.currentYaw * Time.fixedDeltaTime;
			var rollTorque = -heliTransform.forward * helicopterCmp.currentRoll * Time.fixedDeltaTime;
			var additionalGravitation = Vector3.down * heliConfig.additionalGravitation * Time.fixedDeltaTime;


			heliRigidbody.AddForce(throttleForth, ForceMode.Force);
			heliRigidbody.AddForce(additionalGravitation, ForceMode.Force);
			heliRigidbody.AddTorque(pitchTorque);
			heliRigidbody.AddTorque(yawTorque);
			heliRigidbody.AddTorque(rollTorque);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ApplyNormalization(Rigidbody heliRigidbody, HelicopterConfigData heliConfigData)
		{
			var movementDirection = heliRigidbody.velocity;
			movementDirection.y = 0f;

			if (movementDirection.sqrMagnitude < .01f)
				return;

			var targetLookRotation = Quaternion.LookRotation(movementDirection);
			float damping = 1f - Mathf.InverseLerp
			(
				heliConfigData.minMaxSpeedForNormalization.x,
				heliConfigData.minMaxSpeedForNormalization.y,
				heliRigidbody.velocity.magnitude
			);

			damping = Mathf.Clamp(damping, heliConfigData.minNormalizationDamping, 1f);

			heliRigidbody.rotation = Quaternion.Slerp(heliRigidbody.rotation, targetLookRotation, 1f - Mathf.Pow(damping, Time.fixedDeltaTime));
		}
	}
}	