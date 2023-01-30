using DG.Tweening;
using NaughtyAttributes;
using Tools.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.LevelMamengement.UI
{
	public sealed class UiLoadingScreen : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup parentCanvasGroup;
		[BoxGroup("References")]
		[Required, SerializeField] private Image loadingBarImage;
		
		[BoxGroup("Animation properties ")]
		[SerializeField] [Min(0f)] private float showHideAnimationDuration = .2f;

		private const float ADDITIONAL_LOADING_PROGRESS_OFFSET = .2f;

		private void Awake()
		{
			parentCanvasGroup.alpha = 0f;
			parentCanvasGroup.SetGoInactive();
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

		public void SetLoadingProgress(float loadingProgress)
		{
			loadingBarImage.fillAmount = loadingProgress + ADDITIONAL_LOADING_PROGRESS_OFFSET;
		}
	}
}