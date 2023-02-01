using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame.SplashScreen
{
	public sealed class UiSplashScreen : MonoBehaviour
	{
		[SerializeField] [Scene] private int sceneToLoad;

		private void Start()
		{
			StartCoroutine(LoadScene());
		}

		private IEnumerator LoadScene()
		{
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene(sceneToLoad);
		}
	}
}