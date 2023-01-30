using EcsTools.ClassExtensions;
using Ingame.LevelMamengement;
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
		
		[Inject]
		private void Construct([Inject(Id = EcsWorldContext.ProjectContext)] EcsWorld world)
		{
			_worldProject = world;
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
			
		}

		private void OnExitButtonClicked()
		{
			Application.Quit();
		}
	}
}