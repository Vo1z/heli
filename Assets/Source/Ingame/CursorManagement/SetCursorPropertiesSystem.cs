using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Ingame.LevelMamengement;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.CursorManagement
{
	public sealed class SetCursorPropertiesSystem : IEcsInitSystem
	{
		private readonly EcsWorldInject _worldProject = "project";
		private readonly EcsCustomInject<ConfigProvider> _cursorConfig;

		public void Init(IEcsSystems systems)
		{
			var levelCmpFilter = _worldProject.Value.Filter<LevelComponent>().End();
			var levelCmpPool = _worldProject.Value.GetPool<LevelComponent>();
			int currentSceneIndex = levelCmpPool.GetFirstComponent(levelCmpFilter).sceneIndex;
			var cursorProperties = _cursorConfig.Value.cursorConfig.GetCursorProperties(currentSceneIndex);

			Cursor.visible = cursorProperties.isEnabledByDefault;
			Cursor.lockState = cursorProperties.isEnabledByDefault ? CursorLockMode.Confined : CursorLockMode.Locked;
		}
	}
}