using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Health
{
	public readonly struct ApplyDamageSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<HealthComponent, ApplyDamageComponent>> _damageFilter;
		
		private readonly EcsPoolInject<HealthComponent> _healthCmpPool;
		private readonly EcsPoolInject<ApplyDamageComponent> _damageCmpPool;
		private readonly EcsPoolInject<IsDeadTag> _isDeadTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _damageFilter.Value)
			{
				var applyDamageCmp = _damageCmpPool.Value.Get(entity);
				ref var healthCmp = ref _healthCmpPool.Value.Get(entity);
				
				_damageCmpPool.Value.Del(entity);

				if (healthCmp.currentHealth <= 0f)
					continue;

				if(!healthCmp.IsDamageTypeApplied(applyDamageCmp.damageType))
					continue;

				healthCmp.currentHealth -= applyDamageCmp.amountOfDamage;
				healthCmp.currentHealth = Mathf.Max(0f, healthCmp.currentHealth);

				if (healthCmp.currentHealth == 0f)
					_isDeadTagPool.Value.Add(entity);
			}
		}
	}
}