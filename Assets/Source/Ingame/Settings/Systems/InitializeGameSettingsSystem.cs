using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Ingame.SaveLoading;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json;
using UnityEngine;

namespace Ingame.Settings
{
	public sealed class InitializeGameSettingsSystem : IEcsInitSystem
	{
		private readonly EcsWorldInject _worldProject = default;

		private readonly EcsCustomInject<ConfigProvider> _configProvider;

		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _gameSettingsCmpFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;

		public void Init(IEcsSystems systems)
		{
			ref var saveLoadCmp = ref _worldProject.Value.GetFirstComponent<SaveLoadComponent>();

			if (!_gameSettingsCmpFilter.Value.IsEmpty())
			{
				int gameSettingsEntity = _gameSettingsCmpFilter.Value.GetFirstEntity();
				ref var existingGameSettingsCmp = ref _gameSettingsCmpPool.Value.Add(gameSettingsEntity);

				LoadDataIntoSettings(ref saveLoadCmp, ref existingGameSettingsCmp);
				
				return;
			}

			int entity = _worldProject.Value.NewEntity();
			ref var newGameSettingCmp = ref _gameSettingsCmpPool.Value.Add(entity);
			
			LoadDataIntoSettings(ref saveLoadCmp, ref newGameSettingCmp);
		}

		private void LoadDataIntoSettings(ref SaveLoadComponent saveLoadComp, ref GameSettingsComponent gameSettingsComp)
		{
			if (saveLoadComp.HasSavedComponent<GameSettingsComponent>())
			{
				gameSettingsComp = saveLoadComp.GetSaveComponent<GameSettingsComponent>();
				return;
			}

			gameSettingsComp = _configProvider.Value.GameSettingsConfig.DefaultSettings;
			saveLoadComp.AddSaveComponent(gameSettingsComp);

			_worldProject.Value.SendSignal<PerformSavingEvent>();
		}
	}
}