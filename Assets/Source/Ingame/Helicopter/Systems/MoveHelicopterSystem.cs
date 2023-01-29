using System.Runtime.CompilerServices;
using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Ingame.ConfigProvision;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Helicopter
{
	public readonly struct MoveHelicopterSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _gameSettingsFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;
		
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		private readonly EcsFilterInject<Inc<TransformModel, RigidBodyModel, HelicopterComponent>> _helicopterFilter;
		
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RigidBodyModel> _rigidbodyMdlPool;
		private readonly EcsPoolInject<HelicopterComponent> _helicopterCmpPool;
		
		public void Run(IEcsSystems systems)
		{
			ref var settingsCmp = ref _gameSettingsCmpPool.Value.GetFirstComponent(_gameSettingsFilter.Value);
			
			foreach (var helicopterEntity in _helicopterFilter.Value)
			{
				ref var helicopterCmp = ref _helicopterCmpPool.Value.Get(helicopterEntity);
				var heliTransform = _transformMdlPool.Value.Get(helicopterEntity).transform;
				var heliRigidbody = _rigidbodyMdlPool.Value.Get(helicopterEntity).rigidbody;
				var heliConfigData = _configProvider.Value.helicoptersConfig.GetHelicopterConfigData(helicopterCmp.helicopterId);

				if (settingsCmp.gameSettings.isHardcoreControlSchemeApplied)
					ApplyRealisticControls(heliTransform, heliRigidbody, helicopterCmp, heliConfigData);
				else
					ApplyCasualControls(heliTransform, heliRigidbody, helicopterCmp, heliConfigData);
				
				ApplyNormalization(heliRigidbody, heliConfigData);
			}
		}

		private void ApplyCasualControls(Transform heliTransform, Rigidbody heliRigidbody, in HelicopterComponent helicopterCmp, in HelicopterConfigData heliConfig)
		{
			var flyingForce = heliTransform.up * heliConfig.casualFlyForce * Time.deltaTime;
			var targetVelocity = heliRigidbody.velocity;
			targetVelocity.y = helicopterCmp.currentThrottle;
			var yawTorque = heliTransform.up * helicopterCmp.currentYaw * heliConfig.casualYawRotationSpeed * Time.fixedDeltaTime;
			var targetLocalEulerAngles = heliTransform.localEulerAngles;
			targetLocalEulerAngles.x = helicopterCmp.currentPitch * heliConfig.casualRotationAngle;
			targetLocalEulerAngles.z = -helicopterCmp.currentRoll * heliConfig.casualRotationAngle;
			var targetLocalRotation = Quaternion.Euler(targetLocalEulerAngles);
			
			heliRigidbody.AddForce(flyingForce, ForceMode.Force);
			heliRigidbody.velocity = targetVelocity;
			heliRigidbody.rotation = Quaternion.Slerp(heliRigidbody.rotation, targetLocalRotation, 1f - Mathf.Pow(heliConfig.casualRotationDamping, Time.fixedDeltaTime));
			heliRigidbody.AddTorque(yawTorque);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ApplyRealisticControls(Transform heliTransform, Rigidbody heliRigidbody, in HelicopterComponent helicopterCmp, in HelicopterConfigData heliConfig)
		{
			var throttleForth = heliTransform.up * helicopterCmp.currentThrottle * Time.fixedDeltaTime;
			var pitchTorque = heliTransform.right * helicopterCmp.currentPitch * Time.fixedDeltaTime;
			var yawTorque = heliTransform.up * helicopterCmp.currentYaw * Time.fixedDeltaTime;
			var rollTorque = -heliTransform.forward * helicopterCmp.currentRoll * Time.fixedDeltaTime;
			var additionalGravitation = Vector3.down * heliConfig.hcoreAdditionalGravitation * Time.fixedDeltaTime;


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