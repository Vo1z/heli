using EcsTools.ClassExtensions;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;

namespace Ingame.Debugging
{
	public readonly struct ChangeControlsTypeSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;

		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _settingsFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;

		public void Run(IEcsSystems systems)
		{
			ref var inputCmp = ref _inputCmpPool.Value.GetFirstComponent(_inputFilter.Value);
			ref var settingsCmp = ref _gameSettingsCmpPool.Value.GetFirstComponent(_settingsFilter.Value);
			
			if(!inputCmp.changeContolsType)
				return;
			
			settingsCmp.gameSettings.isHardcoreControlSchemeApplied = !settingsCmp.gameSettings.isHardcoreControlSchemeApplied; 
		}
	}
}