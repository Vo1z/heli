using Leopotam.EcsLite;
using Zenject;

namespace Ingame.Setup
{
	public sealed class EcsInstallerProject : MonoInstaller
	{
		public override void InstallBindings()
		{
			var ecsWorldProject = new EcsWorld();

			Container.Bind<EcsWorld>()
				.WithId(EcsWorldContext.ProjectContext)
				.FromInstance(ecsWorldProject)
				.AsCached();
		}
	}
}