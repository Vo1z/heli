using EcsTools.UnityModels;
using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Ingame.Detection;
using Ingame.Detection.Radar;
using Ingame.Detection.Vision;
using Ingame.Helicopter;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.UI.Debugging
{
	public sealed class PresentDebuggingInfoToUiSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _worldProject = "project";
		private readonly EcsCustomInject<ConfigProvider> _configProvider;

		private readonly EcsFilterInject<Inc<UiDebuggingViewModel>> _uiDebuggingViewMdlFilter;
		private readonly EcsPoolInject<UiDebuggingViewModel> _uiDebuggingViewMdlPool;

		private readonly EcsFilterInject<Inc<HelicopterComponent>> _heliCmpFilter;
		private readonly EcsPoolInject<HelicopterComponent> _heliCmpPool;
		
		private readonly EcsPoolInject<RigidBodyModel> _rigidbodyMdlPool;
		private readonly EcsPoolInject<IsRadarDetectedTag> _isRadarDetectedTagPool;
		private readonly EcsPoolInject<IsVisuallyDetectedTag> _isVisuallyDetectedTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _uiDebuggingViewMdlFilter.Value)
			{
				var uiDebuggingView = _uiDebuggingViewMdlPool.Value.Get(entity).uiDebuggingView;

				PresentFpsInfo(uiDebuggingView);
				PresentHelicopterInfo(uiDebuggingView);
			}
		}

		private void PresentFpsInfo(UiDebuggingView uiDebuggingView)
		{
			int fps = Mathf.RoundToInt(1 / Time.deltaTime);
			uiDebuggingView.SetFpsContent(fps);
		}

		private void PresentHelicopterInfo(UiDebuggingView uiDebuggingView)
		{
			if(_heliCmpFilter.Value.IsEmpty())
				return;

			var gameSettingsCmpFilter = _worldProject.Value.Filter<GameSettingsComponent>().End();
			var gameSettingsCmpPool = _worldProject.Value.GetPool<GameSettingsComponent>();
			
			int heliEntity = _heliCmpFilter.Value.GetFirstEntity();
			ref var heliCmp = ref _heliCmpPool.Value.Get(heliEntity);
			ref var settingsCmp = ref gameSettingsCmpPool.GetFirstComponent(gameSettingsCmpFilter);
			bool isHardcoreControlScheme = settingsCmp.gameSettings.isHardcoreControlSchemeApplied;
			bool isDetectedBuyTheRadar = _isRadarDetectedTagPool.Value.Has(heliEntity);
			bool isVisuallyDetected = _isVisuallyDetectedTagPool.Value.Has(heliEntity);
			
			uiDebuggingView.SetHelicopterContent
				(
					heliCmp,
					GetVelocity(heliEntity),
					GetHeliNormalizationDumping(heliEntity, heliCmp),
					isHardcoreControlScheme,
					isDetectedBuyTheRadar,
					isVisuallyDetected
				);
		}

		private float GetHeliNormalizationDumping(in int heliEntity, in HelicopterComponent heliCmp)
		{
			if (!_rigidbodyMdlPool.Value.Has(heliEntity))
				return -1f;

			var heliRigidbody = _rigidbodyMdlPool.Value.Get(heliEntity).rigidbody;
			var heliConfigData = _configProvider.Value.helicoptersConfig.GetHelicopterConfigData(heliCmp.helicopterId);
			
			var movementDirection = heliRigidbody.velocity;
			movementDirection.y = 0f;

			if (movementDirection.sqrMagnitude < .01f)
				return -1f;
			
			float damping = 1f - Mathf.InverseLerp
			(
				heliConfigData.minMaxSpeedForNormalization.x,
				heliConfigData.minMaxSpeedForNormalization.y,
				heliRigidbody.velocity.magnitude
			);

			damping = Mathf.Clamp(damping, heliConfigData.minNormalizationDamping, 1f);
			
			return damping;
		}

		private Vector3 GetVelocity(in int heliEntity)
		{
			if (!_rigidbodyMdlPool.Value.Has(heliEntity))
				return Vector3.zero;
			
			var heliRigidbody = _rigidbodyMdlPool.Value.Get(heliEntity).rigidbody;

			return heliRigidbody.velocity;
		}
	}
}