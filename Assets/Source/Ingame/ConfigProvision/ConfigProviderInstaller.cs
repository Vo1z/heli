using Ingame.Helicopter;
using Ingame.Settings;
using Ingame.Vfx;
using UnityEngine;
using Zenject;

namespace Ingame.ConfigProvision
{
	public sealed class ConfigProviderInstaller : MonoInstaller
	{
		[SerializeField] private HelicoptersConfig helicoptersConfig;
		[SerializeField] private VfxConfig vfxConfig;
		[SerializeField] private GameSettingsConfig gameSettingsConfig;
		
		public override void InstallBindings()
		{
			var configProvider = new ConfigProvider
			{
				helicoptersConfig = helicoptersConfig,
				vfxConfig = vfxConfig,
				GameSettingsConfig = gameSettingsConfig
			};

			Container.Bind<ConfigProvider>()
				.FromInstance(configProvider)
				.AsSingle()
				.NonLazy();
		}
	}
}