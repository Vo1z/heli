using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ingame.Input
{
	public struct ReceiveInputSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private readonly EcsWorldInject _world;
		private readonly EcsCustomInject<InputActions> _inputActions;

		private readonly EcsPoolInject<InputComponent> _inputCmpPool;
		private readonly EcsFilterInject<Inc<InputComponent>> _inputCmpFilter;

		//Helicopter
		private InputAction _pitchInputAction;
		private InputAction _yawInputAction;
		private InputAction _rollInputAction;
		private InputAction _throttleInputAction;
		
		//Combat
		private InputAction _shootInputAction;
		
		//Camera
		private InputAction _rotationInputAction;

		//Utils
		private InputDeviceType _currentInputDevice;
		
		private InputAction _changeTargetFpsInputAction;
		private InputAction _reloadLevelInputAction;
		private InputAction _changeControlsTypeInputAction;
		
		private InputAction _keyboardSchemeDetectionInputAction;
		private InputAction _xboxGamepadSchemeDetectionInputAction;

		public void PreInit(IEcsSystems systems)
		{
			_inputActions.Value.Enable();

			SetUpHelicopterInput();
			SetUpCombatInput();
			SetUpCameraInput();
			SetupUtilsInput();
		}

		public void Init(IEcsSystems systems)
		{
			int inputEntity = _world.Value.NewEntity();
			_inputCmpPool.Value.Add(inputEntity);
		}

		public void Run(IEcsSystems systems)
		{
			int inputEntity = _inputCmpFilter.Value.GetRawEntities()[0];
			ref var inputCmp = ref _inputCmpPool.Value.Get(inputEntity);

			ReceiveHelicopterInput(ref inputCmp);
			ReceiveCombatInput(ref inputCmp);
			ReceiveCameraInput(ref inputCmp);
			ReceiveUtilsInput(ref inputCmp);
		}

#region Setup
		private void SetUpHelicopterInput()
		{
			_pitchInputAction = _inputActions.Value.Helicopter.Pitch;
			_yawInputAction = _inputActions.Value.Helicopter.Yaw;
			_rollInputAction = _inputActions.Value.Helicopter.Roll;
			_throttleInputAction = _inputActions.Value.Helicopter.Throttle;
		}

		private void SetUpCombatInput()
		{
			_shootInputAction = _inputActions.Value.Combat.Shoot;
		}
		
		private void SetUpCameraInput()
		{
			_rotationInputAction = _inputActions.Value.Camera.Rotation;
		}

		private void SetupUtilsInput()
		{
			_changeTargetFpsInputAction = _inputActions.Value.Utils.ChangeTargetFPS;
			_reloadLevelInputAction = _inputActions.Value.Utils.ReloadLevel;
			_changeControlsTypeInputAction = _inputActions.Value.Utils.ChangeControlsType;

			_keyboardSchemeDetectionInputAction = _inputActions.Value.Utils.KeyboardSchemeDetection;
			_xboxGamepadSchemeDetectionInputAction = _inputActions.Value.Utils.XboxGamepadSchemeDetection;
		}
#endregion Setup

#region ReceiveInput
		private void ReceiveHelicopterInput(ref InputComponent inputCmp)
		{
			inputCmp.pitchInput = _pitchInputAction.ReadValue<float>();
			inputCmp.yawInput = _yawInputAction.ReadValue<float>();
			inputCmp.rollInput = _rollInputAction.ReadValue<float>();
			inputCmp.throttleInput = _throttleInputAction.ReadValue<float>();
		}
		
		private void ReceiveCombatInput(ref InputComponent inputCmp)
		{
			inputCmp.shootInput = _shootInputAction.WasPerformedThisFrame();
		}
		
		private void ReceiveCameraInput(ref InputComponent inputCmp)
		{
			inputCmp.rotationInput = _rotationInputAction.ReadValue<Vector2>();
		}
		
		private void ReceiveUtilsInput(ref InputComponent inputCmp)
		{
			inputCmp.changeFpsInput = _changeTargetFpsInputAction.WasPerformedThisFrame();
			inputCmp.reloadLevel = _reloadLevelInputAction.WasPerformedThisFrame();
			inputCmp.changeContolsType = _changeControlsTypeInputAction.WasPerformedThisFrame();

			//Detection of current input device type
			inputCmp.isInputDeviceTypeChangedThisFrame = false;
			
			if (_keyboardSchemeDetectionInputAction.WasPerformedThisFrame())
				_currentInputDevice = InputDeviceType.Keyboard;
			else if (_xboxGamepadSchemeDetectionInputAction.WasPerformedThisFrame())
				_currentInputDevice = InputDeviceType.XboxGamepad;

			if (_currentInputDevice == inputCmp.currentInputDeviceType)
				return;

			Debug.Log($"Is changed to {_currentInputDevice}");
			
			inputCmp.isInputDeviceTypeChangedThisFrame = true;
			inputCmp.currentInputDeviceType = _currentInputDevice;
		}
#endregion ReceiveInput
	}
}