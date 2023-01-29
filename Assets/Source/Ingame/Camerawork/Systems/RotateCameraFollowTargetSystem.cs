using EcsTools.UnityModels;
using EcsTools.ClassExtensions;
using EcsTools.Timer;
using Ingame.Player;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;

namespace Ingame.Camerawork
{
	public readonly struct RotateCameraFollowTargetSystem : IEcsPostRunSystem
	{
		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _gameSettingsCmpFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;

		private readonly EcsFilterInject<Inc<TransformModel, PlayerTag>> _playerFilter;
		private readonly EcsFilterInject<Inc<TransformModel, TimerComponent, CameraFollowTargetTag>> _followTargetTagFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;
		
		private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter;

		public void PostRun(IEcsSystems systems)
		{
			if (_inputFilter.Value.IsEmpty() || _followTargetTagFilter.Value.IsEmpty() || _playerFilter.Value.IsEmpty())
				return;

			ref var settingsCmp = ref _gameSettingsCmpPool.Value.Get(_gameSettingsCmpFilter.Value.GetFirstEntity());
			
			int inputEntity = _inputFilter.Value.GetFirstEntity();
			ref var inputCmp = ref _inputCmpPool.Value.Get(inputEntity);

			int playerEntity = _playerFilter.Value.GetFirstEntity();
			ref var playerTransformCmp = ref _transformMdlPool.Value.Get(playerEntity);

			int followTargetEntity = _followTargetTagFilter.Value.GetFirstEntity();
			ref var followTargetTimerCmp = ref _timerCmpPool.Value.Get(followTargetEntity);
			var followTargetTransform = _transformMdlPool.Value.Get(followTargetEntity).transform;

			//Rotation
			if (inputCmp.rotationInput.sqrMagnitude < .01f)
			{
				if (followTargetTimerCmp.timePassed > settingsCmp.gameSettings.resetCameraPosDelay)
				{
					var targetLookForwardVector = playerTransformCmp.transform.forward;
					targetLookForwardVector.y = 0f;

					followTargetTransform.localRotation = Quaternion.SlerpUnclamped
					(
						followTargetTransform.localRotation,
						Quaternion.LookRotation(targetLookForwardVector),
						1f - Mathf.Pow(.05f, Time.deltaTime)
					);
				}

				return;
			}

			followTargetTimerCmp.timePassed = 0f;
			
			followTargetTransform.Rotate(Vector3.up, inputCmp.rotationInput.x * settingsCmp.gamepadSettings.sensitivityX * Time.deltaTime);
			followTargetTransform.Rotate(Vector3.right, -inputCmp.rotationInput.y * settingsCmp.gamepadSettings.sensitivityY * Time.deltaTime);

			var targetLocalEulerAngles = followTargetTransform.localEulerAngles;
			targetLocalEulerAngles.z = 0f;

			followTargetTransform.localEulerAngles = targetLocalEulerAngles;
		}

		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
	}
}