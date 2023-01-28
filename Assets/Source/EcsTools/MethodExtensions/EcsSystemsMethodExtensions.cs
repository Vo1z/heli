using EcsTools.ReactiveSystem;
using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static partial class EcsMethodExtensions
	{
		public static void AddReactiveSystem(this IEcsSystems ecsSystems, IEcsReactiveSystem reactiveSystem)
		{
			var world = ecsSystems.GetWorld();
			
			world.AddEventListener(reactiveSystem);
			ecsSystems.Add(reactiveSystem);
		}
	}
}