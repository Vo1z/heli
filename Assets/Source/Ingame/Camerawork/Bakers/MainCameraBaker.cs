using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ingame.Camerawork
{
	[RequireComponent(typeof(Camera))]
	public sealed class MainCameraBaker : EcsMonoBaker
	{
		public override void Bake(int entity, EcsWorld world)
		{
			var cameraMdlPool = world.GetPool<CameraModel>();
			var mainCameraTagPool = world.GetPool<MainCameraTag>();

			cameraMdlPool.TryAdd(entity, new CameraModel
			{
				camera = GetComponent<Camera>()
			});
			mainCameraTagPool.TryAdd(entity);
		}
	}
}