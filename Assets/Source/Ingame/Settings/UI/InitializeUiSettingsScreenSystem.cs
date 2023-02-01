using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.Settings.UI
{
	public sealed class InitializeUiSettingsScreenSystem : IEcsInitSystem
	{
		private readonly EcsWorldInject _worldProject = "project";
		
		private readonly EcsFilterInject<Inc<UiSettingsScreenModel>> _uiSettingsScreenMdlFilter;
		private readonly EcsPoolInject<UiSettingsScreenModel> _uiSettingsScreenMdlPool;

		public void Init(IEcsSystems systems)
		{
			var gameSettingsCmpFilter = _worldProject.Value.Filter<GameSettingsComponent>().End();
			var gameSettingsCmpPool = _worldProject.Value.GetPool<GameSettingsComponent>();

			if(_uiSettingsScreenMdlFilter.Value.IsEmpty() || gameSettingsCmpFilter.IsEmpty())
				return;

			ref var gameSettingsCmp = ref gameSettingsCmpPool.GetFirstComponent(gameSettingsCmpFilter);
			var uiSettingsScreen = _uiSettingsScreenMdlPool.Value.GetFirstComponent(_uiSettingsScreenMdlFilter.Value).uiSettingsScreen;
			
			uiSettingsScreen.SetSettingsData(gameSettingsCmp);
		}
	}
}