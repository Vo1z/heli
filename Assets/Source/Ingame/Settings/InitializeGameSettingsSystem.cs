using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.Settings
{
	public readonly struct InitializeGameSettingsSystem : IEcsInitSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		private readonly EcsWorldInject _world;

		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _gameSettingsCmpFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;

		public void Init(IEcsSystems systems)
		{
			if (!_gameSettingsCmpFilter.Value.IsEmpty())
			{
				int gameSettingsEntity = _gameSettingsCmpFilter.Value.GetFirstEntity();
				ref var gameSettingsCmp = ref _gameSettingsCmpPool.Value.Add(gameSettingsEntity);

				gameSettingsCmp = _configProvider.Value.GameSettingsConfig.DefaultSettings;
				
				return;
			}
			
			_world.Value.SendSignal(_configProvider.Value.GameSettingsConfig.DefaultSettings);
		}
	}
}