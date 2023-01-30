using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.LevelMamengement.UI
{
	public sealed class UpdateLoadingUiSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<UiLoadingScreenModel>> _uiLoadingScreenMdlFilter;
		private readonly EcsPoolInject<UiLoadingScreenModel> _uiLoadingScreenCmpPool;
		
		private readonly EcsFilterInject<Inc<SceneLoadingProgressComponent>> _sceneLoadingCmpFilter;
		private readonly EcsPoolInject<SceneLoadingProgressComponent> _sceneLoadingCmpPool;
		
		private readonly EcsFilterInject<Inc<OnLevelLoadingEndedEvent>> _sceneLoadingEndedEvnFilter;
		private readonly EcsPoolInject<OnLevelLoadingEndedEvent> _sceneLoadingEndedEvnPool;
		
		private readonly EcsFilterInject<Inc<OnLevelLoadingStartedEvent>> _sceneLoadingStartedEvnFilter;
		private readonly EcsPoolInject<OnLevelLoadingStartedEvent> _sceneLoadingStartedEvnPool;

		public void Run(IEcsSystems systems)
		{
			if(_uiLoadingScreenMdlFilter.Value.IsEmpty())
				return;

			var uiLoadingScreen = _uiLoadingScreenCmpPool.Value.GetFirstComponent(_uiLoadingScreenMdlFilter.Value).uiLoadingScreen;
			
			if (!_sceneLoadingStartedEvnFilter.Value.IsEmpty())
			{
				uiLoadingScreen.Show();
			}
			
			if (!_sceneLoadingCmpFilter.Value.IsEmpty())
			{
				ref var sceneLoadingCmp = ref _sceneLoadingCmpPool.Value.GetFirstComponent(_sceneLoadingCmpFilter.Value);
				
				uiLoadingScreen.SetLoadingProgress(sceneLoadingCmp.sceneLoadingAsyncOperation.progress);
				return;
			}
			
			if (!_sceneLoadingEndedEvnFilter.Value.IsEmpty())
			{
				uiLoadingScreen.Hide();
				return;
			}
		}
	}
}