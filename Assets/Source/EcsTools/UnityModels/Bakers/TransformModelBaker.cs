using EcsExtensions.Convertion;
using EcsExtensions.UnityModels;
using Leopotam.EcsLite;

namespace EcsExtensions.UnityModels
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