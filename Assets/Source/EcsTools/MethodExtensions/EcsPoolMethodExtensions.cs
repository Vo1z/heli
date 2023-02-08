using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static partial class EcsMethodExtensions
	{
		public static ref T GetFirstComponent<T>(this EcsPool<T> pool, EcsFilter filter) where T : struct
		{
			return ref pool.Get(filter.GetFirstEntity());
		}

		public static void RemoveAllComponents<T>(this EcsPool<T> pool, EcsFilter filter) where T : struct
		{
			foreach (var entity in filter)
			{
				pool.Del(entity);
			}
		}

		/// <summary>
		/// Adds component to the entity without throwing an exception if entity already contains such component
		/// </summary>
		/// <returns>true if component was added. false otherwise</returns>
		public static bool TryAdd<T>(this EcsPool<T> pool, in int entity, in T component) where T : struct
		{
			if (pool.Has(entity))
				return false;

			pool.Add(entity) = component;
			
			return true;
		}
		
		/// <summary>
		/// Adds component to the entity without throwing an exception if entity already contains such component
		/// </summary>
		/// <returns>true if component was added. false otherwise</returns>
		public static bool TryAdd<T>(this EcsPool<T> pool, in int entity) where T : struct
		{
			if (pool.Has(entity))
				return false;

			pool.Add(entity);
			
			return true;
		}
	}
}