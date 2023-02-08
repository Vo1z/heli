using EcsTools.ClassExtensions;
using EcsTools.Convertion;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Vehicle.Turret
{
	/// <summary>
	/// Turret that rotates towards radar target
	/// </summary>
	public sealed class RadarTargetTurretBaker : EcsMonoBaker
	{
		[BoxGroup("Turret")]
		[Required, SerializeField] private Transform horizontalRotatorTransform;
		[BoxGroup("Turret")]
		[Required, SerializeField] private Transform verticalRotatorTransform;
		[BoxGroup("Turret")]
		[SerializeField] [Range(0, 1f)] private float rotationDumping = .05f;

		public override void Bake(int entity, EcsWorld world)
		{
			var turretCmpPool = world.GetPool<TurretComponent>();
			var rotateTurretTowardRadarTargetTagPool = world.GetPool<RotateTurretTowardRadarTargetTag>();
			var transformMdl = world.GetPool<TransformModel>();

			turretCmpPool.TryAdd(entity, new TurretComponent
			{
				horizontalRotatorTransform = horizontalRotatorTransform,
				verticalRotatorTransform = verticalRotatorTransform,
				rotationDumping = rotationDumping
			});

			rotateTurretTowardRadarTargetTagPool.TryAdd(entity);
			
			transformMdl.TryAdd(entity, new TransformModel
			{
				transform = transform
			});
		}
	}
}