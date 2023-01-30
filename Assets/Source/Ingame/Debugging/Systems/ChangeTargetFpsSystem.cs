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
		private readonly EcsWorldInject _worldProject = "project";

		private readonly int[] _targetFramerates = { 144, 30}; 
		private int _currentTargetFramerateIndex = 0;
		
		public void Init(IEcsSystems systems)
		{
			SetNextTargetFramerate();
		}
		
		public void Run(IEcsSystems systems)
		{
			var inputCmpFilter = _worldProject.Value.Filter<InputComponent>().End();
			var inputCmpPool = _worldProject.Value.GetPool<InputComponent>();

			if(inputCmpFilter.IsEmpty())
				return;

			ref var inputCmp = ref inputCmpPool.GetFirstComponent(inputCmpFilter);

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