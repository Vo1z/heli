using System;

namespace Ingame.Settings
{
	[Serializable]
	public struct GameSettingsComponent
	{
		[Serializable]
		public struct MouseSettings
		{
			public float sensitivityX;
			public float sensitivityY;
		}
		
		[Serializable]
		public struct GamepadSettings
		{
			public float sensitivityX;
			public float sensitivityY;
		}
		
		[Serializable]
		public struct GameSettings
		{
			public float resetCameraPosDelay;
		}

		public MouseSettings mouseSettings;
		public GamepadSettings gamepadSettings;
		public GameSettings gameSettings;
	}
}