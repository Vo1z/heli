using EcsTools.ClassExtensions;
using Ingame.LevelMamengement;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine.SceneManagement;

namespace Ingame.Debugging
{
	public sealed class ReloadLevelSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _worldProject = "project";
		
		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
		
		public void Run(IEcsSystems systems)
		{
			var inputCmpFilter = _worldProject.Value.Filter<InputComponent>().End();
			var inputCmpPool = _worldProject.Value.GetPool<InputComponent>();
			
			ref var inputCmp = ref inputCmpPool.GetFirstComponent(inputCmpFilter);

			if(!inputCmp.reloadLevel)
				return;
			
			_worldProject.Value.SendSignal(new ChangeLevelRequest
			{
				sceneIndex = SceneManager.GetActiveScene().buildIndex
			});
		}
	}
}