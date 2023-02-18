using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Ingame.Camerawork;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.Vfx.BilboardEffect
{
	public readonly struct RotateBillboardObjectsTowardsCameraSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<CameraModel, MainCameraTag>> _mainCameraFilter;
		private readonly EcsPoolInject<CameraModel> _cameraMdlPool;

		private readonly EcsFilterInject<Inc<TransformModel, BillboardTag>> _billboardFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;

		public void Run(IEcsSystems systems)
		{
			if(_mainCameraFilter.Value.IsEmpty())
				return;

			var mainCameraTransform = _cameraMdlPool.Value.GetFirstComponent(_mainCameraFilter.Value).camera.transform;
			var targetForwardVectorForBillboard = -mainCameraTransform.forward; 
			
			foreach (var entity in _billboardFilter.Value)
			{
				var billboardTransform = _transformMdlPool.Value.Get(entity).transform;
				billboardTransform.forward = targetForwardVectorForBillboard;
			}
		}
	}
}