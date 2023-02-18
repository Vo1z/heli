using System;
using Ingame.CursorManagement;
using Ingame.Helicopter;
using Ingame.Settings;
using Ingame.Vfx;

namespace Ingame.ConfigProvision
{
	[Serializable]
	public sealed class ConfigProvider
	{
		public HelicoptersConfig helicoptersConfig;
		public VfxConfig vfxConfig;
		public GameSettingsConfig gameSettingsConfig;
		public CursorConfig cursorConfig;
	}
}