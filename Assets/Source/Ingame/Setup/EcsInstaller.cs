using Leopotam.EcsLite;
using Zenject;

namespace Ingame.Setup
{
	public sealed class EcsInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var ecsWorld = new EcsWorld();

			Container.Bind<EcsWorld>()
				.FromInstance(ecsWorld)
				.AsSingle()
				.NonLazy();
		}
	}
}