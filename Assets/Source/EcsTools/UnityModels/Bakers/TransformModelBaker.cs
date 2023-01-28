using EcsTools.UnityModels;
using EcsTools.Convertion;
using Leopotam.EcsLite;

namespace EcsTools.UnityModels
{
	public sealed class TransformModelBaker : EcsMonoBaker
	{
		public override void Bake(int entity, EcsWorld world)
		{
			var transformModelPool = world.GetPool<TransformModel>();
			
			transformModelPool.Add(entity).transform = transform;
		}
	}
}