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
	public readonly struct ConvertInputToHelicopterControlValuesSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		
		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _gameSettingsFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;

		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;
		private readonly EcsFilterInject<Inc<HelicopterComponent, PlayerTag>> _helicopterFilter;
		
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
		private readonly EcsPoolInject<HelicopterComponent> _helicopterCmpPool;

		public void Run(IEcsSystems systems)
		{
			ref var settingsCmp = ref _gameSettingsCmpPool.Value.GetFirstComponent(_gameSettingsFilter.Value);

			if (settingsCmp.gameSettings.isHardcoreControlSchemeApplied)
				ConvertAsHardcoreInput();
			else
				ConvertAsCasualInput();
		}

		private void ConvertAsCasualInput()
		{
			ref var inputCmp = ref _inputCmpPool.Value.Get(_inputFilter.Value.GetRawEntities()[0]);

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

		private void ConvertAsHardcoreInput()
		{
			ref var inputCmp = ref _inputCmpPool.Value.Get(_inputFilter.Value.GetRawEntities()[0]);

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