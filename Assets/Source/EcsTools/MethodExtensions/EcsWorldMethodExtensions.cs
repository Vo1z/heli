using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static class EcsWorldMethodExtensions
	{
		public static void SendSignal<T>(this EcsWorld world) where T : struct
		{
			int entity = world.NewEntity();
			var tPool = world.GetPool<T>();

			tPool.Add(entity);
		}
		
		public static void SendSignal<T>(this EcsWorld world, T component) where T : struct
		{
			int entity = world.NewEntity();
			var tPool = world.GetPool<T>();

			tPool.Add(entity) = component;
		}
	}
}