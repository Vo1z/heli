using Ingame.Helicopter;
using Ingame.Vfx;
using UnityEngine;
using Zenject;

namespace Ingame.ConfigProvision
{
	public sealed class ConfigProviderInstaller : MonoInstaller
	{
		[SerializeField] private HelicoptersConfig helicoptersConfig;
		[SerializeField] private VfxConfig vfxConfig;
		
		public override void InstallBindings()
		{
			var configProvider = new ConfigProvider
			{
				helicoptersConfig = helicoptersConfig,
				vfxConfig = vfxConfig
			};

			Container.Bind<ConfigProvider>()
				.FromInstance(configProvider)
				.AsSingle()
				.NonLazy();
		}
	}
}