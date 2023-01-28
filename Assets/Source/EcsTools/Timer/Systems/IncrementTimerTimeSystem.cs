using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsTools.Timer
{
	public struct IncrementTimerTimeSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TimerComponent>> _timerCmpFilter;
		private readonly EcsPoolInject<TimerComponent> _timerCmpPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _timerCmpFilter.Value)
			{
				ref var timerCmp = ref _timerCmpPool.Value.Get(entity);

				timerCmp.timePassed += Time.deltaTime;
			}
		}
	}
}