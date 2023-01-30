using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Ingame.Player;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;

namespace Ingame.Helicopter
{
	public sealed class ConvertInputToHelicopterControlValuesSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		private readonly EcsWorldInject _worldProject = "project";

		private readonly EcsFilterInject<Inc<HelicopterComponent, PlayerTag>> _helicopterFilter;
		private readonly EcsPoolInject<HelicopterComponent> _helicopterCmpPool;

		public void Run(IEcsSystems systems)
		{
			var gameSettingsCmpFilter = _worldProject.Value.Filter<GameSettingsComponent>().End();
			var gameSettingsCmpPool = _worldProject.Value.GetPool<GameSettingsComponent>();
			var inputCmpFilter = _worldProject.Value.Filter<InputComponent>().End();
			var inputCmpPool = _worldProject.Value.GetPool<InputComponent>();
			
			ref var settingsCmp = ref gameSettingsCmpPool.GetFirstComponent(gameSettingsCmpFilter);
			ref var inputCmp = ref inputCmpPool.GetFirstComponent(inputCmpFilter);

			if (settingsCmp.gameSettings.isHardcoreControlSchemeApplied)
				ConvertAsHardcoreInput(inputCmp);
			else
				ConvertAsCasualInput(inputCmp);
		}

		private void ConvertAsCasualInput(in InputComponent inputCmp)
		{
			foreach (var heliEntity in _helicopterFilter.Value)
			{
				ref var heliCmp = ref _helicopterCmpPool.Value.Get(heliEntity);
				var heliConfig = _configProvider.Value.helicoptersConfig.GetHelicopterConfigData(heliCmp.helicopterId);

				heliCmp.currentPitch = inputCmp.pitchInput;
				heliCmp.currentYaw = inputCmp.yawInput;
				heliCmp.currentRoll = inputCmp.rollInput;

				heliCmp.currentThrottle = Mathf.Lerp
				(
					heliCmp.currentThrottle,
					inputCmp.throttleInput * heliConfig.casualThrottleForce,
					1f - Mathf.Pow(heliConfig.casualThrottleGainDamping, Time.deltaTime)
				);
			}
		}

		private void ConvertAsHardcoreInput(in InputComponent inputCmp)
		{
			foreach (var heliEntity in _helicopterFilter.Value)
			{
				ref var heliCmp = ref _helicopterCmpPool.Value.Get(heliEntity);
				var heliConfig = _configProvider.Value.helicoptersConfig.GetHelicopterConfigData(heliCmp.helicopterId);

				heliCmp.currentPitch = inputCmp.pitchInput * heliConfig.hcoreRotationResponsiveness;
				heliCmp.currentYaw = inputCmp.yawInput * heliConfig.hcoreRotationResponsiveness;
				heliCmp.currentRoll = inputCmp.rollInput * heliConfig.hcoreRotationResponsiveness;
				heliCmp.currentThrottle += inputCmp.throttleInput * heliConfig.hcoreThrottleGainSpeed * Time.deltaTime;

				heliCmp.currentThrottle = Mathf.Clamp(heliCmp.currentThrottle, 0, heliConfig.hcoreMaxThrottle);
			}
		}
	}
}	