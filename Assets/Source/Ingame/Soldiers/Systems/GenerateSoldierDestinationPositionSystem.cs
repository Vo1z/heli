using EcsTools.ClassExtensions;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Soldiers
{
	public readonly struct GenerateSoldierDestinationPositionSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<SoldierComponent, AttachedGroupComponent>, Exc<IsDeadTag>> _soldierFilter;
		private readonly EcsPoolInject<SoldierComponent> _soldierCmpPool;
		private readonly EcsPoolInject<AttachedGroupComponent> _attachedGroupCmpPool;
		private readonly EcsPoolInject<GroupComponent> _groupCmpPool;
		private readonly EcsPoolInject<UpdateDestinationTag> _updateDestinationTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _soldierFilter.Value)
			{
				ref var soldierCmp = ref _soldierCmpPool.Value.Get(entity);
				ref var attachedGroupCmp = ref _attachedGroupCmpPool.Value.Get(entity);
				int groupEntity = attachedGroupCmp.boundedGroupEntityReference.entity;

				soldierCmp.currentTravelTime += Time.deltaTime;

				if (soldierCmp.currentTravelTime < soldierCmp.travelTimeToDestination)
					continue;

				ref var groupCmp = ref _groupCmpPool.Value.Get(groupEntity);
				var targetDestinationPos = Random.insideUnitSphere * groupCmp.operatingRadius;
				targetDestinationPos += attachedGroupCmp.boundedGroupEntityReference.transform.position;

				soldierCmp.currentTravelTime = 0f;
				soldierCmp.currentDestinationPos = targetDestinationPos;

				_updateDestinationTagPool.Value.TryAdd(entity);
			}
		}
	}
}