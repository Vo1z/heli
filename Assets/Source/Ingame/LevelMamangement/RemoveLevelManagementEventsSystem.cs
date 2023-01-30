using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.LevelMamengement
{
	public readonly struct RemoveLevelManagementEventsSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<OnLevelLoadingStartedEvent>> _onLevelLoadingStartedEvtFilter;
		private readonly EcsPoolInject<OnLevelLoadingStartedEvent> _onLevelLoadingStartedEvtPool;
		
		private readonly EcsFilterInject<Inc<OnLevelLoadingEndedEvent>> _onLevelLoadingEndedEvtFilter;
		private readonly EcsPoolInject<OnLevelLoadingEndedEvent> _onLevelLoadingEndedEvtPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var lvlStartedEntity in _onLevelLoadingStartedEvtFilter.Value)
			{
				_onLevelLoadingStartedEvtPool.Value.Del(lvlStartedEntity);
			}
			
			foreach (var lvlEndedEntity in _onLevelLoadingEndedEvtFilter.Value)
			{
				_onLevelLoadingEndedEvtPool.Value.Del(lvlEndedEntity);
			}
		}
	}
}