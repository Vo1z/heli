using System.Runtime.CompilerServices;
using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame.LevelMamengement
{
	public sealed class ChangeLevelSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _worldProject;
		
		private readonly EcsFilterInject<Inc<ChangeLevelRequest>> _changeLevelReqFilter;
		private readonly EcsPoolInject<ChangeLevelRequest> _changeLevelReqPool;
		
		private readonly EcsFilterInject<Inc<SceneLoadingProgressComponent>> _sceneLoadingCmpFilter;
		private readonly EcsPoolInject<SceneLoadingProgressComponent> _sceneLoadingCmpPool;

		private readonly EcsFilterInject<Inc<LevelComponent>> _levelCmpFilter;
		private readonly EcsPoolInject<LevelComponent> _levelCmpPool;

		public void Run(IEcsSystems systems)
		{
			if (!_sceneLoadingCmpFilter.Value.IsEmpty())
			{
				ProcessSceneLoading();
				return;
			}

			if (_changeLevelReqFilter.Value.IsEmpty())
				return;

			StartSceneLoading();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ProcessSceneLoading()
		{
			int sceneLoadingCmpEntity = _sceneLoadingCmpFilter.Value.GetFirstEntity();
			ref var sceneLoadingProgressCmp = ref _sceneLoadingCmpPool.Value.Get(sceneLoadingCmpEntity);
			
			if(!sceneLoadingProgressCmp.sceneLoadingAsyncOperation.isDone)
				return;
			
			_levelCmpPool.Value.GetFirstComponent(_levelCmpFilter.Value).sceneIndex = SceneManager.GetActiveScene().buildIndex;
			
			_worldProject.Value.SendSignal(new OnLevelLoadingEndedEvent());
			_sceneLoadingCmpPool.Value.Del(sceneLoadingCmpEntity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void StartSceneLoading()
		{
			int requestEntity = _changeLevelReqFilter.Value.GetFirstEntity();
			var changeLevelReq = _changeLevelReqPool.Value.Get(requestEntity);
			
			_changeLevelReqPool.Value.Del(requestEntity);
			
			_worldProject.Value.SendSignal(new OnLevelLoadingStartedEvent());
			_worldProject.Value.SendSignal(new SceneLoadingProgressComponent
			{
				sceneLoadingAsyncOperation = SceneManager.LoadSceneAsync(changeLevelReq.sceneIndex)
			});
		}
	}
}