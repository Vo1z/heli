using System.Collections;
using EcsTools.ClassExtensions;
using Ingame.LevelMamengement;
using Ingame.Setup;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.SplashScreen
{
	public sealed class UiSplashScreen : MonoBehaviour
	{
		[SerializeField] [Scene] private int sceneToLoad;

		private EcsWorld _worldProject;
		
		[Inject]
		private void Construct([Inject(Id = EcsWorldContext.ProjectContext)]EcsWorld worldProject)
		{
			_worldProject = worldProject;
		}

		private void Start()
		{
			StartCoroutine(LoadScene());
		}

		private IEnumerator LoadScene()
		{
			yield return new WaitForSeconds(1f);
			_worldProject.SendSignal(new ChangeLevelRequest
			{
				sceneIndex = sceneToLoad
			});
		}
	}
}