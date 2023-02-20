using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Ingame.Soldiers
{
	public readonly struct ChangeSoldierDestinationPositionSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<SoldierComponent, NavMeshAgentModel, UpdateDestinationTag>, Exc<IsDeadTag>> _soldierFilter;
		private readonly EcsPoolInject<SoldierComponent> _soldierCmpPool;
		private readonly EcsPoolInject<NavMeshAgentModel> _navMeshAgentMdlPool;
		private readonly EcsPoolInject<UpdateDestinationTag> _updateDestinationTagPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _soldierFilter.Value)
			{
				ref var soldierCmp = ref _soldierCmpPool.Value.Get(entity);
				ref var navMeshAgentMdl = ref _navMeshAgentMdlPool.Value.Get(entity);
				
				navMeshAgentMdl.navMeshAgent.SetDestination(soldierCmp.currentDestinationPos);
				
				_updateDestinationTagPool.Value.Del(entity);
			}
		}
	}
}