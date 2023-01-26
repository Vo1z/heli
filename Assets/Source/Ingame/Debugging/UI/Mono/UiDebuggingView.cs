using Ingame.ConfigProvision;
using Ingame.Helicopter;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

namespace Ingame.UI.Debugging
{
	public sealed class UiDebuggingView : MonoBehaviour
	{
		[Required, SerializeField] private TMP_Text helicopterInfoText;
		[Required, SerializeField] private TMP_Text fpsText;

		private HelicoptersConfig _helicoptersConfig;
		
		[Inject]
		private void Construct(ConfigProvider configProvider)
		{
			_helicoptersConfig = configProvider.HelicoptersConfig;
		}
		
		public void SetFpsContent(int fps)
		{
			fpsText.SetText($"FPS: {fps}");
		}

		public void SetHelicopterContent(in HelicopterComponent heliCmp, in Vector3 velocity, float normalizationDumping)
		{
			var heliConfigData = _helicoptersConfig.GetHelicopterConfigData(heliCmp.helicopterId);
			float throttleInPercentage = Mathf.RoundToInt(Mathf.InverseLerp(0, heliConfigData.maxThrottle, heliCmp.currentThrottle) * 100f);

			string heliContent = $"ID: {heliCmp.helicopterId}\n" +
			                     $"Name: {heliConfigData.helicopterName}\n" +
			                     "===========================\n" +
			                     $"Velocity: {velocity}\n" +
			                     $"Normalization: {normalizationDumping}\n" +
			                     "===========================\n" +
			                     $"Throttle: {throttleInPercentage}% ({heliCmp.currentThrottle})\n" +
			                     $"SPEED: {heliCmp.currentSpeed}\n" +
			                     $"ALT: {heliCmp.currentAltitude}";
			
			helicopterInfoText.SetText(heliContent);
		}
	}
}