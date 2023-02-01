using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Settings.UI
{
	public sealed class UiSettingsScreenModelBaker : EcsMonoBaker
	{
		[Required, SerializeField] private UiSettingsScreen uiSettingsScreen;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var uiSettingsScreenMdlPool = world.GetPool<UiSettingsScreenModel>();
			
			uiSettingsScreenMdlPool.Add(entity).uiSettingsScreen = uiSettingsScreen;
		}
	}
}