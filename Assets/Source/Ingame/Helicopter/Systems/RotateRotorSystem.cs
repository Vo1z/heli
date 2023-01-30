using System.Runtime.CompilerServices;
using EcsTools.UnityModels;
using EcsTools.ClassExtensions;
using Ingame.ConfigProvision;
using Ingame.Player;
using Ingame.Settings;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Helicopter
{
	public sealed class RotateRotorSystem : IEcsRunSystem
	{
		private readonly EcsWorldInject _worldProject = "project";
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		
		private readonly EcsFilterInject<Inc<GameSettingsComponent>> _gameSettingsFilter;
		private readonly EcsPoolInject<GameSettingsComponent> _gameSettingsCmpPool;

		private readonly EcsFilterInject<Inc<TransformModel, HelicopterComponent, PlayerTag>> _playerHeliFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<HelicopterComponent> _heliCmpPool;
		
		private readonly EcsFilterInject<Inc<RotorComponent>> _rotorCmpFilter;
		private readonly EcsPoolInject<RotorComponent> _rotorCmpPool;

		public void Run(IEcsSystems systems)
		{
			if(_playerHeliFilter.Value.IsEmpty())
				return;
			
			var gameSettingsCmpFilter = _worldProject.Value.Filter<GameSettingsComponent>().End();
			var gameSettingsCmpPool = _worldProject.Value.GetPool<GameSettingsComponent>();
			
			ref var settingsCmp = ref gameSettingsCmpPool.GetFirstComponent(gameSettingsCmpFilter);
			ref var playerHeliCmp = ref _heliCmpPool.Value.Get(_playerHeliFilter.Value.GetFirstEntity());
			var currentHeliConfigData = _configProvider.Value.helicoptersConfig.GetHelicopterConfigData(playerHeliCmp.helicopterId);
			
			if(settingsCmp.gameSettings.isHardcoreControlSchemeApplied)
				RotateRotorDueToHardcoreInput(playerHeliCmp, currentHeliConfigData);
			else
				RotateRotorDueToCasualInput(playerHeliCmp, currentHeliConfigData);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RotateRotorDueToCasualInput(in HelicopterComponent playerHeliCmp, in HelicopterConfigData currentHeliConfigData)
		{
			foreach (var entity in _rotorCmpFilter.Value)
			{
				ref var rotorCmp = ref _rotorCmpPool.Value.Get(entity);
				var rotorTransform = _transformMdlPool.Value.Get(entity).transform;
				float targetSpeed = rotorCmp.speed * Mathf.InverseLerp(-currentHeliConfigData.casualThrottleForce, currentHeliConfigData.casualThrottleForce, playerHeliCmp.currentThrottle);

				rotorTransform.Rotate(rotorCmp.rotateAroundVector, targetSpeed * Time.deltaTime);
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RotateRotorDueToHardcoreInput(in HelicopterComponent playerHeliCmp, in HelicopterConfigData currentHeliConfigData)
		{
			foreach (var entity in _rotorCmpFilter.Value)
			{
				ref var rotorCmp = ref _rotorCmpPool.Value.Get(entity);
				var rotorTransform = _transformMdlPool.Value.Get(entity).transform;
				float targetSpeed = rotorCmp.speed * Mathf.InverseLerp(0, currentHeliConfigData.hcoreMaxThrottle, playerHeliCmp.currentThrottle);

				rotorTransform.Rotate(rotorCmp.rotateAroundVector, targetSpeed * Time.deltaTime);
			}
		}
	}
}