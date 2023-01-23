using Ingame.Helicopter;
using UnityEngine;
using Zenject;

namespace Ingame.ConfigProvision
{
	public sealed class ConfigProviderInstaller : MonoInstaller
	{
		[SerializeField] private HelicoptersConfig helicoptersConfig;
		
		public override void InstallBindings()
		{
			var configProvider = new ConfigProvider
			{
				helicoptersConfig = helicoptersConfig
			};

			Container.Bind<ConfigProvider>()
				.FromInstance(configProvider)
				.AsSingle()
				.NonLazy();
		}
	}
}