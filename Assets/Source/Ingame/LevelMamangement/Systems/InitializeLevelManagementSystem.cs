using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.SceneManagement;

namespace Ingame.LevelMamengement
{
	public readonly struct InitializeLevelManagementSystem : IEcsInitSystem
	{
		private readonly EcsWorldInject _world;
		
		private readonly EcsFilterInject<Inc<LevelComponent>> _levelCmpFilter;
		private readonly EcsPoolInject<LevelComponent> _levelCmpPool;

		public void Init(IEcsSystems systems)
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			
			if (_levelCmpFilter.Value.IsEmpty())
			{
				_world.Value.SendSignal(new LevelComponent
				{
					sceneIndex = currentSceneIndex
				});
				
				return;
			}

			_levelCmpPool.Value.Get(_levelCmpFilter.Value.GetEntitiesCount()) = new LevelComponent
			{
				sceneIndex = currentSceneIndex
			};
		}
	}
}