using Cinemachine;
using EcsExtensions.Convertion;
using EcsExtensions.UnityModels;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Camerawork
{
	public sealed class PlayerVCameraBaker : EcsMonoBaker
	{
		[Required, SerializeField] private CinemachineVirtualCamera virtualCamera;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var vCamMdlPool = world.GetPool<VirtualCameraModel>();
			var playerCameraTagPool = world.GetPool<PlayerCameraTag>();

			vCamMdlPool.Add(entity).virtualCamera = virtualCamera;
			playerCameraTagPool.Add(entity);
		}
	}
}