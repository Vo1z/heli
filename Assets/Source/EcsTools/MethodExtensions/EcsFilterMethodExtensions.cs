using Leopotam.EcsLite;

namespace EcsTools.ClassExtensions
{
	public static partial class EcsMethodExtensions
	{
		public static bool IsEmpty(this EcsFilter ecsFilter)
		{
			return ecsFilter.GetEntitiesCount() < 1;
		}

		public static int GetFirstEntity(this EcsFilter ecsFilter)
		{
			return ecsFilter.GetRawEntities()[0];
		}
	}
}