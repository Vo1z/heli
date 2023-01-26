using EcsExtensions.UnityModels;
using EcsTools.ClassExtensions;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;

namespace Ingame.Camerawork
{
	public struct RotateCameraAroundHelicopterSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<VirtualCameraModel, PlayerCameraTag>> _playerCameraFilter;
		private readonly EcsFilterInject<Inc<TransformModel, PlayerTag>> _playerFilter;
		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;

		private readonly EcsPoolInject<VirtualCameraModel> _vCamMdlPool;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;

		public void Run(IEcsSystems systems)
		{
			if(_inputFilter.Value.IsEmpty() || _playerFilter.Value.IsEmpty() || _playerCameraFilter.Value.IsEmpty())
				return;

			var playerVCamera = _vCamMdlPool.Value.Get(_playerCameraFilter.Value.GetFirstEntity()).virtualCamera;
			var playerTransform = _transformMdlPool.Value.Get(_playerFilter.Value.GetFirstEntity()).transform;
			ref var inputCmp = ref _inputCmpPool.Value.Get(_inputFilter.Value.GetFirstEntity());
			
			playerVCamera.transform.Translate(Vector3.right * -inputCmp.rotationInput.y * 100f * Time.deltaTime);
			playerVCamera.transform.RotateAround(playerTransform.position, Vector3.up, inputCmp.rotationInput.x * 100f * Time.deltaTime);
			playerVCamera.transform.RotateAround(playerTransform.position, Vector3.right, -inputCmp.rotationInput.y * 100f * Time.deltaTime);
		}
	}
}