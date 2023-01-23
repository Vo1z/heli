using Zenject;

namespace Ingame.Input
{
	public sealed class InputInstaller : MonoInstaller
	{
		private InputActions _inputActions;
		
		public override void InstallBindings()
		{
			_inputActions ??= new InputActions();

			Container.Bind<InputActions>()
				.FromInstance(_inputActions)
				.AsSingle()
				.NonLazy();
		}
	}
}