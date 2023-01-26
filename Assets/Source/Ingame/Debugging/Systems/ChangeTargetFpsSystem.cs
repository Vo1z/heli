using System.Runtime.CompilerServices;
using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Input;
using UnityEngine;

namespace Ingame.Debugging
{
	public sealed class ChangeTargetFpsSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly EcsWorldInject _world;
		
		private readonly EcsFilterInject<Inc<InputComponent>> _inputCmpFilter;
		private readonly EcsPoolInject<InputComponent> _inputCmpPool;

		private int[] _targetFramerates = { 144, 30}; 
		private int _currentTargetFramerateIndex = 0;
		
		public void Init(IEcsSystems systems)
		{
			SetNextTargetFramerate();
		}
		
		public void Run(IEcsSystems systems)
		{
			if(_inputCmpFilter.Value.IsEmpty())
				return;

			int inputEntity = _inputCmpFilter.Value.GetFirstEntity();
			ref var inputCmp = ref _inputCmpPool.Value.Get(inputEntity);
			
			if(!inputCmp.changeFpsInput)
				return;

			SetNextTargetFramerate();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetNextTargetFramerate()
		{
			int framerateIndex = _currentTargetFramerateIndex % _targetFramerates.Length;
			Application.targetFrameRate = _targetFramerates[framerateIndex];

			_currentTargetFramerateIndex++;
		}
	}
}