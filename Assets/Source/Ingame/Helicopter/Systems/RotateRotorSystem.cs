using EcsTools.UnityModels;
using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Helicopter
{
	public struct RotateRotorSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;

		private readonly EcsFilterInject<Inc<TransformModel, HelicopterComponent, PlayerTag>> _playerHeliFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<HelicopterComponent> _heliCmpPool;
		
		private readonly EcsFilterInject<Inc<RotorComponent>> _rotorCmpFilter;
		private readonly EcsPoolInject<RotorComponent> _rotorCmpPool;

		public void Run(IEcsSystems systems)
		{
			if(_playerHeliFilter.Value.IsEmpty())
				return;

			ref var playerHeliCmp = ref _heliCmpPool.Value.Get(_playerHeliFilter.Value.GetFirstEntity());
			var currentHeliConfigData = _configProvider.Value.HelicoptersConfig.GetHelicopterConfigData(playerHeliCmp.helicopterId);
			
			foreach (var entity in _rotorCmpFilter.Value)
			{
				ref var rotorCmp = ref _rotorCmpPool.Value.Get(entity);
				var rotorTransform = _transformMdlPool.Value.Get(entity).transform;
				float targetSpeed = rotorCmp.speed * Mathf.InverseLerp(0, currentHeliConfigData.maxThrottle, playerHeliCmp.currentThrottle);

				rotorTransform.Rotate(rotorCmp.rotateAroundVector, targetSpeed * Time.deltaTime);
			}
		}
	}
}