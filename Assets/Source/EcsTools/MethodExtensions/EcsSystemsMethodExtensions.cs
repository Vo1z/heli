using EcsTools.ReactiveSystem;
using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static class EcsSystemsMethodExtensions
	{
		public static void AddReactiveSystem(this IEcsSystems ecsSystems, IEcsReactiveSystem reactiveSystem)
		{
			var world = ecsSystems.GetWorld();
			
			world.AddEventListener(reactiveSystem);
			ecsSystems.Add(reactiveSystem);
		}
	}
}