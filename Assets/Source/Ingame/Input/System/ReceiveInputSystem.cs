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

		private InputAction _moveInputAction;
		private InputAction _rotateInputAction;

		public void PreInit(IEcsSystems systems)
		{
			_inputActions.Value.Enable();

			_moveInputAction = _inputActions.Value.Helicopter.Movement;
			_rotateInputAction = _inputActions.Value.Helicopter.Rotation;
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

			inputCmp.movementInput = _moveInputAction.ReadValue<Vector2>();
			inputCmp.rotationInput = _rotateInputAction.ReadValue<Vector2>();
		}
	}
}