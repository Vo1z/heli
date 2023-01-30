using EcsTools.ClassExtensions;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;

namespace Ingame.Debugging
{
	public sealed class ChangeControlsTypeSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _worldProject = "project";

		public void Run(IEcsSystems systems)
		{
			var gameSettingsCmpFilter = _worldProject.Value.Filter<GameSettingsComponent>().End();
			var gameSettingsCmpPool = _worldProject.Value.GetPool<GameSettingsComponent>();
			var inputCmpFilter = _worldProject.Value.Filter<InputComponent>().End();
			var inputCmpPool = _worldProject.Value.GetPool<InputComponent>();

			ref var settingsCmp = ref gameSettingsCmpPool.GetFirstComponent(gameSettingsCmpFilter);
			ref var inputCmp = ref inputCmpPool.GetFirstComponent(inputCmpFilter);

			if(!inputCmp.changeContolsType)
				return;
			
			settingsCmp.gameSettings.isHardcoreControlSchemeApplied = !settingsCmp.gameSettings.isHardcoreControlSchemeApplied; 
		}
	}
}