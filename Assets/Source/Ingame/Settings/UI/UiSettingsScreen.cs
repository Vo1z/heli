using DG.Tweening;
using EcsTools.ClassExtensions;
using Ingame.SaveLoading;
using Ingame.Setup;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Tools.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.Settings.UI
{
	public sealed class UiSettingsScreen : MonoBehaviour
	{
		[BoxGroup("References (Common)")] 
		[Required, SerializeField] private CanvasGroup parentCanvasGroup;
		[BoxGroup("References (Common)")] 
		[Required, SerializeField] private Button saveButton;
		[BoxGroup("References (Common)")] 
		[Required, SerializeField] private Button backButton;
		
		[BoxGroup("References (game settings)")]
		[Required, SerializeField] private Toggle enableCasualControlsToggle;
		
		[BoxGroup("Animation settings")]
		[SerializeField] [Min(0f)] private float showHideAnimationDuration = .2f;
		

		private EcsWorld _worldProject;
		private EcsWorld _worldScene;
		
		[Inject]
		private void Construct([Inject(Id = EcsWorldContext.ProjectContext)] EcsWorld worldProject, [Inject(Id = EcsWorldContext.SceneContext)] EcsWorld worldScene)
		{
			_worldProject = worldProject;
			_worldScene = worldScene;
		}
		
		private void Awake()
		{
			saveButton.onClick.AddListener(OnSaveButtonClicked);
			backButton.onClick.AddListener(OnBackButtonClicked);

			parentCanvasGroup.alpha = 0f;
			parentCanvasGroup.SetGoInactive();	
		}

		private void OnDestroy()
		{
			saveButton.onClick.RemoveListener(OnSaveButtonClicked);
			backButton.onClick.RemoveListener(OnBackButtonClicked);
		}

		private void OnSaveButtonClicked()
		{
			ref var gameSettingsCmp = ref _worldProject.GetFirstComponent<GameSettingsComponent>();
			ref var saveLoadCmp = ref _worldProject.GetFirstComponent<SaveLoadComponent>();

			gameSettingsCmp.gameSettings.isHardcoreControlSchemeApplied = !enableCasualControlsToggle.isOn;
			saveLoadCmp.AddSaveComponent(gameSettingsCmp);
			
			_worldProject.SendSignal<PerformSavingEvent>();
		}

		private void OnBackButtonClicked()
		{
			_worldScene.SendSignal<HideSettingsUiEvent>();
		}

		public void SetSettingsData(in GameSettingsComponent gameSettingsComponent)
		{
			enableCasualControlsToggle.isOn = !gameSettingsComponent.gameSettings.isHardcoreControlSchemeApplied;
		}

		public void Show()
		{
			parentCanvasGroup.SetGoActive();
			parentCanvasGroup.DOFade(1, showHideAnimationDuration);
		}

		public void Hide()
		{
			parentCanvasGroup.DOFade(0, showHideAnimationDuration)
				.OnComplete(parentCanvasGroup.SetGoInactive);
		}
	}
}