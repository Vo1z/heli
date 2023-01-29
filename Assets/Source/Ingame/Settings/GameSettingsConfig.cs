using UnityEngine;

namespace Ingame.Settings
{
	[CreateAssetMenu(menuName = "Configs/GameSettingsConfig")]
	public sealed class GameSettingsConfig : ScriptableObject
	{
		[SerializeField] private GameSettingsComponent defaultSettings;
		
		public GameSettingsComponent DefaultSettings => defaultSettings;
	}
}