using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static partial class EcsMethodExtensions
	{
		public static ref T GetFirstComponent<T>(this EcsPool<T> pool, EcsFilter filter) where T : struct
		{
			return ref pool.Get(filter.GetFirstEntity());
		}
	}
}