using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.LevelMamengement.UI
{
	public sealed class UiLoadingScreenBaker : EcsMonoBaker
	{
		[Required, SerializeField] private UiLoadingScreen uiLoadingScreen;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var uiLoadingScreenModelPool = world.GetPool<UiLoadingScreenModel>();

			uiLoadingScreenModelPool.Add(entity).uiLoadingScreen = uiLoadingScreen;
		}
	}
}