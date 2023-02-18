using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
using Leopotam.EcsLite;

namespace Ingame.Vfx.BilboardEffect
{
	public sealed class BillboardBaker : EcsMonoBaker
	{
		public override void Bake(int entity, EcsWorld world)
		{
			var billboardTagPool = world.GetPool<BillboardTag>();
			var transformMdlPool = world.GetPool<TransformModel>();

			billboardTagPool.TryAdd(entity);
			transformMdlPool.TryAdd(entity, new TransformModel
			{
				transform = transform
			});
		}
	}
}