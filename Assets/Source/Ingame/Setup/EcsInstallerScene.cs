using Leopotam.EcsLite;
using Zenject;

namespace Ingame.Setup
{
	public sealed class EcsInstallerScene : MonoInstaller
	{
		public override void InstallBindings()
		{
			var ecsWorldScene = new EcsWorld();

			Container.Bind<EcsWorld>()
				.WithId(EcsWorldContext.SceneContext)
				.FromInstance(ecsWorldScene)
				.AsCached();
		}
	}
}