using EcsTools.Convertion;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Leopotam.EcsLite;

namespace Ingame.Camerawork
{
	public sealed class CameraFollowTargetBaker : EcsMonoBaker
	{
		public override void Bake(int entity, EcsWorld world)
		{
			var cameraFollowTagPool = world.GetPool<CameraFollowTargetTag>();
			var transformModelPool = world.GetPool<TransformModel>();
			var timerCmpPool = world.GetPool<TimerComponent>();

			cameraFollowTagPool.Add(entity);
			transformModelPool.Add(entity).transform = transform;
			timerCmpPool.Add(entity);
		}
	}
}