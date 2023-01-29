using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.SceneManagement;

namespace Ingame.LevelMamengement
{
	public readonly struct ChangeLevelSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ChangeLevelRequest>> _changeLevelReqFilter;
		private readonly EcsPoolInject<ChangeLevelRequest> _changeLevelReqPool;
		
		public void Run(IEcsSystems systems)
		{
			if(_changeLevelReqFilter.Value.IsEmpty())
				return;

			int requestEntity = _changeLevelReqFilter.Value.GetFirstEntity();
			var changeLevelReq = _changeLevelReqPool.Value.Get(requestEntity);
			
			_changeLevelReqPool.Value.Del(requestEntity);

			SceneManager.LoadScene(changeLevelReq.sceneIndex);
		}
	}
}