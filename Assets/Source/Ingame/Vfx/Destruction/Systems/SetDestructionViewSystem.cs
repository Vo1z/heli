using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Tools.ClassExtensions;

namespace Ingame.Vfx.Destruction
{
	public readonly struct SetDestructionViewSystem : IEcsRunSystem
	{	
		private readonly EcsFilterInject<Inc<HealthComponent, DestructibleObjectComponent>> _destructibleObjectsFilter;
		private readonly EcsPoolInject<HealthComponent> _healthCmpPool;
		private readonly EcsPoolInject<DestructibleObjectComponent> _destructibleObjectCmpPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _destructibleObjectsFilter.Value)
			{
				ref var healthCmp = ref _healthCmpPool.Value.Get(entity);
				
				if(healthCmp.currentHealth > 0f)
					continue;
				
				ref var destructibleObject = ref _destructibleObjectCmpPool.Value.Get(entity);

				destructibleObject.originalView.SetGoInactive();
				destructibleObject.destroyedView.SetGoActive();
				
				_destructibleObjectCmpPool.Value.Del(entity);
			}
		}
	}
}