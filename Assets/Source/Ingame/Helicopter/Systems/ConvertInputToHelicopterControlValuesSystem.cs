using Ingame.ConfigProvision;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;

namespace Ingame.Helicopter
{
	public struct ConvertInputToHelicopterControlValuesSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		
		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;
		private readonly EcsFilterInject<Inc<HelicopterComponent, PlayerTag>> _helicopterFilter;
		
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
		private readonly EcsPoolInject<HelicopterComponent> _helicopterCmpPool;

		public void Run(IEcsSystems systems)
		{
			ref var inputCmp = ref _inputCmpPool.Value.Get(_inputFilter.Value.GetRawEntities()[0]);

			foreach (var heliEntity in _helicopterFilter.Value)
			{
				ref var heliCmp = ref _helicopterCmpPool.Value.Get(heliEntity);
				var heliConfig = _configProvider.Value.helicoptersConfig.GetHelicopterConfigData(heliCmp.helicopterId);

				heliCmp.currentPitch = inputCmp.pitchInput * heliConfig.rotationResponsiveness;
				heliCmp.currentYaw = inputCmp.yawInput * heliConfig.rotationResponsiveness;
				heliCmp.currentRoll = inputCmp.rollInput * heliConfig.rotationResponsiveness;
				heliCmp.currentThrottle += inputCmp.throttleInput * heliConfig.throttleGainSpeed * Time.deltaTime;

				heliCmp.currentThrottle = Mathf.Clamp(heliCmp.currentThrottle, 0, heliConfig.maxThrottle);
			}
		}
	}
}	