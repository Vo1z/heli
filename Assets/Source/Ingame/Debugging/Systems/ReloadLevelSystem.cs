using EcsTools.ClassExtensions;
using Ingame.LevelMamengement;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine.SceneManagement;

namespace Ingame.Debugging
{
	public readonly struct ReloadLevelSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _world;
		
		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
		
		public void Run(IEcsSystems systems)
		{
			ref var inputCmp = ref _inputCmpPool.Value.GetFirstComponent(_inputFilter.Value);
			
			if(!inputCmp.reloadLevel)
				return;
			
			_world.Value.SendSignal(new ChangeLevelRequest
			{
				sceneIndex = SceneManager.GetActiveScene().buildIndex
			});
		}
	}
}