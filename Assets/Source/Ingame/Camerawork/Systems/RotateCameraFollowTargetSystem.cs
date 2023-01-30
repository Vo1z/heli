using EcsTools.UnityModels;
using EcsTools.ClassExtensions;
using EcsTools.Timer;
using Ingame.Input;
using Ingame.Player;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;

namespace Ingame.Camerawork
{
	public sealed class RotateCameraFollowTargetSystem : IEcsPostRunSystem
	{
		private readonly EcsWorldInject _worldProject = "project";

		private readonly EcsFilterInject<Inc<TransformModel, PlayerTag>> _playerFilter;
		private readonly EcsFilterInject<Inc<TransformModel, TimerComponent, CameraFollowTargetTag>> _followTargetTagFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;

		public void PostRun(IEcsSystems systems)
		{
			var gameSettingsCmpFilter = _worldProject.Value.Filter<GameSettingsComponent>().End();
			var gameSettingsCmpPool = _worldProject.Value.GetPool<GameSettingsComponent>();
			var inputCmpFilter = _worldProject.Value.Filter<InputComponent>().End();
			var inputCmpPool = _worldProject.Value.GetPool<InputComponent>();

			if (inputCmpFilter.IsEmpty() || _followTargetTagFilter.Value.IsEmpty() || _playerFilter.Value.IsEmpty())
				return;

			ref var settingsCmp = ref gameSettingsCmpPool.GetFirstComponent(gameSettingsCmpFilter);
			ref var inputCmp = ref inputCmpPool.GetFirstComponent(inputCmpFilter);

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

			float sensitivityX = inputCmp.currentInputDeviceType == InputDeviceType.Keyboard ? settingsCmp.mouseSettings.sensitivityX : settingsCmp.gamepadSettings.sensitivityX;
			float sensitivityY = inputCmp.currentInputDeviceType == InputDeviceType.Keyboard ? settingsCmp.mouseSettings.sensitivityY : settingsCmp.gamepadSettings.sensitivityY;
			
			followTargetTransform.Rotate(Vector3.up, inputCmp.rotationInput.x * sensitivityX * Time.deltaTime);
			followTargetTransform.Rotate(Vector3.right, -inputCmp.rotationInput.y * sensitivityY * Time.deltaTime);

			var targetLocalEulerAngles = followTargetTransform.localEulerAngles;
			targetLocalEulerAngles.z = 0f;

			followTargetTransform.localEulerAngles = targetLocalEulerAngles;
		}
	}
}