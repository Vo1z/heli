using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static partial class EcsMethodExtensions
	{
		public static ref T SendSignal<T>(this EcsWorld world) where T : struct
		{
			int entity = world.NewEntity();
			var tPool = world.GetPool<T>();

			return ref tPool.Add(entity);
		}
		
		public static void SendSignal<T>(this EcsWorld world, T component) where T : struct
		{
			int entity = world.NewEntity();
			var tPool = world.GetPool<T>();

			tPool.Add(entity) = component;
		}

		public static ref T GetFirstComponent<T>(this EcsWorld world, out int entity) where T : struct
		{
			var tCmpFilter = world.Filter<T>().End();
			var tCmpPool = world.GetPool<T>();

			entity = tCmpFilter.GetFirstEntity();
			ref T tCmp = ref tCmpPool.Get(entity);
			
			return ref tCmp;
		}
		
		public static ref T GetFirstComponent<T>(this EcsWorld world) where T : struct
		{
			var tCmpFilter = world.Filter<T>().End();
			var tCmpPool = world.GetPool<T>();
			
			ref T tCmp = ref tCmpPool.GetFirstComponent(tCmpFilter);
			
			return ref tCmp;
		}
	}
}