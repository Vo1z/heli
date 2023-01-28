using EcsTools.Convertion;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.UI.Debugging
{
	public sealed class UiDebuggingBaker : EcsMonoBaker
	{
		[Required, SerializeField] private UiDebuggingView _uiDebuggingView;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var uiDebuggingViewMdlPool = world.GetPool<UiDebuggingViewModel>();
			uiDebuggingViewMdlPool.Add(entity).uiDebuggingView = _uiDebuggingView;
		}
	}
}