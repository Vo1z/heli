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
	public sealed class SetHelicopterPositionToTheMaterialsSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<ConfigProvider> _configProvider;
		private readonly EcsWorldInject _worldProject = "project";

		private readonly EcsFilterInject<Inc<TransformModel, HelicopterComponent, PlayerTag>> _playerHeliFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		
		private static readonly int HELICOPTER_POS_PROPERTY_ID = Shader.PropertyToID("_HelicopterPosition");

		public void Run(IEcsSystems systems)
		{
			var levelCmpFilter = _worldProject.Value.Filter<LevelComponent>().End();
			var levelCmpPool = _worldProject.Value.GetPool<LevelComponent>();
			
			if(_playerHeliFilter.Value.IsEmpty() || levelCmpFilter.IsEmpty())
				return;

			ref var levelCmp = ref levelCmpPool.GetFirstComponent(levelCmpFilter);
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