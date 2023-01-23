using Ingame.EcsExtensions;
using Ingame.UnityModels;

namespace Source.Ingame.UnityIntegration
{
	public sealed class TransformModelBaker : EcsBaker
	{
		protected override void Bake(int entity)
		{
			var transformModelPool = _world.GetPool<TransformModel>();
			
			transformModelPool.Add(entity).transform = transform;
		}
	}
}