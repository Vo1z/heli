using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.Settings.UI
{
	public readonly struct ShowHideSettingsUiSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ShowSettingsUiEvent>> _showSettingsUiEvnFilter;
		private readonly EcsPoolInject<ShowSettingsUiEvent> _showSettingsUiEvnPool;

		private readonly EcsFilterInject<Inc<HideSettingsUiEvent>> _hideSettingsUiEvnFilter;
		private readonly EcsPoolInject<HideSettingsUiEvent> _hideSettingsUiEvnPool;

		private readonly EcsFilterInject<Inc<UiSettingsScreenModel>> _uiSettingsScreenMdlFilter;
		private readonly EcsPoolInject<UiSettingsScreenModel> _uiSettingsScreenMdlPool;

		public void Run(IEcsSystems systems)
		{
			if(_uiSettingsScreenMdlFilter.Value.IsEmpty())
				return;

			var uiSettingsScreen = _uiSettingsScreenMdlPool.Value.GetFirstComponent(_uiSettingsScreenMdlFilter.Value).uiSettingsScreen;

			if (!_showSettingsUiEvnFilter.Value.IsEmpty())
			{
				uiSettingsScreen.Show();
				_showSettingsUiEvnPool.Value.RemoveAllComponents(_showSettingsUiEvnFilter.Value);
				return;
			}
			
			if (!_hideSettingsUiEvnFilter.Value.IsEmpty())
			{
				uiSettingsScreen.Hide();
				_hideSettingsUiEvnPool.Value.RemoveAllComponents(_hideSettingsUiEvnFilter.Value);
				return;
			}
		}
	}
}