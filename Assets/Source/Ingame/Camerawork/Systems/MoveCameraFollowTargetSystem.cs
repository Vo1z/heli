using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.Camerawork
{
	public readonly struct MoveCameraFollowTargetSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, PlayerTag>> _playerFilter;
		private readonly EcsFilterInject<Inc<TransformModel, CameraFollowTargetTag>> _followTargetTagFilter;
		
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		
		public void Run(IEcsSystems systems)
		{
			int playerEntity = _playerFilter.Value.GetFirstEntity();
			ref var playerTransformCmp = ref _transformMdlPool.Value.Get(playerEntity);
			
			int followTargetEntity = _followTargetTagFilter.Value.GetFirstEntity();
			var followTargetTransform = _transformMdlPool.Value.Get(followTargetEntity).transform;
			
			followTargetTransform.position = playerTransformCmp.transform.position;
		}
	}
}