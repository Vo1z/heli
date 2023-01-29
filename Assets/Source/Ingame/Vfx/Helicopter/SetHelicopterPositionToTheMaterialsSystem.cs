using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Ingame.ConfigProvision;
using Ingame.Helicopter;
using Ingame.LevelMamengement;
using Ingame.Player;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Vfx.Helicopter
{
	public readonly struct SetHelicopterPositionToTheMaterialsSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;

		private readonly EcsFilterInject<Inc<LevelComponent>> _levelCmpFilter;
		private readonly EcsPoolInject<LevelComponent> _levelCmpPool;

		private readonly EcsFilterInject<Inc<TransformModel, HelicopterComponent, PlayerTag>> _playerHeliFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		
		private static readonly int HELICOPTER_POS_PROPERTY_ID = Shader.PropertyToID("_HelicopterPosition");

		public void Run(IEcsSystems systems)
		{
			if(_playerHeliFilter.Value.IsEmpty() || _levelCmpFilter.Value.IsEmpty())
				return;

			ref var levelCmp = ref _levelCmpPool.Value.Get(_levelCmpFilter.Value.GetFirstEntity());
			var heliTransform = _transformMdlPool.Value.Get(_playerHeliFilter.Value.GetFirstEntity()).transform;
			var requireHeliPositionMaterials = _configProvider
				.Value
				.vfxConfig
				.GetSceneMaterialConfiguration(levelCmp.sceneIndex)
				.requireHelicopterPositionMaterials;

			foreach (var material in requireHeliPositionMaterials)
			{
				if(material == null)
					continue;
				
				material.SetVector(HELICOPTER_POS_PROPERTY_ID, heliTransform.position);
			}
		}
	}
}