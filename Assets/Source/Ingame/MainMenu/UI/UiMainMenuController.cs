using EcsTools.ClassExtensions;
using Ingame.LevelMamengement;
using Ingame.Settings.UI;
using Ingame.Setup;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.MainMenu.UI
{
	public sealed class UiMainMenuController : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private Button playButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button optionsButton;
		[BoxGroup("References")]
		[Required, SerializeField] private Button exitButton;
		
		[BoxGroup("Level loading")]
		[SerializeField] [Scene] private int sceneToLoadOnPlayButtonClicked;

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
			playButton.onClick.AddListener(OnPlayButtonClicked);
			optionsButton.onClick.AddListener(OnOptionsButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		private void OnDestroy()
		{
			playButton.onClick.RemoveListener(OnPlayButtonClicked);
			optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
			exitButton.onClick.RemoveListener(OnExitButtonClicked);
		}

		private void OnPlayButtonClicked()
		{
			_worldProject.SendSignal(new ChangeLevelRequest
			{
				sceneIndex = sceneToLoadOnPlayButtonClicked
			});
		}

		private void OnOptionsButtonClicked()
		{
			_worldScene.SendSignal<ShowSettingsUiEvent>();
		}

		private void OnExitButtonClicked()
		{
			Application.Quit();
		}
	}
}