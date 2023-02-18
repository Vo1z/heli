using UnityEngine;
using Zenject;

namespace Ingame.ConfigProvision
{
	public sealed class ConfigProviderInstaller : MonoInstaller
	{
		[SerializeField] private ConfigProvider configProvider;

		public override void InstallBindings()
		{
			Container.Bind<ConfigProvider>()
				.FromInstance(configProvider)
				.AsSingle()
				.NonLazy();
		}
	}
}