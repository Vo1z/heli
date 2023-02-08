using System.Runtime.CompilerServices;
using EcsTools.UnityModels;
using Ingame.Detection;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Vehicle.Turret
{
	public readonly struct RotateTurretTowardRadarTargetSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, TurretComponent, RotateTurretTowardRadarTargetTag>, Exc<IsDeadTag>> _turretFilter;
		private readonly EcsFilterInject<Inc<TransformModel, IsRadarDetectedTag>> _detectedRadarTargetFilter;

		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<TurretComponent> _turretCmpPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var turretEntity in _turretFilter.Value)
			{
				ref var turretCmp = ref _turretCmpPool.Value.Get(turretEntity);
				var turretTransform = _transformMdlPool.Value.Get(turretEntity).transform;
				var turretPos = turretTransform.position;
				
				Transform closestTargetTransform = null;
				float smallestDistanceToTarget = float.MaxValue;

				foreach (var targetEntity in _detectedRadarTargetFilter.Value)
				{
					var targetTransform = _transformMdlPool.Value.Get(targetEntity).transform;
					float distanceToTheTarget = Vector3.SqrMagnitude(targetTransform.position - turretPos);
					
					if(smallestDistanceToTarget <= distanceToTheTarget)
						continue;

					smallestDistanceToTarget = distanceToTheTarget;
					closestTargetTransform = targetTransform;
				}

				if(closestTargetTransform == null)
					continue;
				
				RotateTurretTowardsTarget(turretCmp, closestTargetTransform.position);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RotateTurretTowardsTarget(in TurretComponent turretCmp, Vector3 targetPos)
		{
			var lookRotation = Quaternion.LookRotation(targetPos - turretCmp.horizontalRotatorTransform.position);

			var horizontalEulerAngles = lookRotation.eulerAngles;
			horizontalEulerAngles.x = 0f;
			horizontalEulerAngles.z = 0f;

			var verticalEulerAngles = lookRotation.eulerAngles;
			horizontalEulerAngles.z = 0f;

			var targetHorizontalRotation = Quaternion.Euler(horizontalEulerAngles);
			var targetVerticalRotation = Quaternion.Euler(verticalEulerAngles);

			turretCmp.horizontalRotatorTransform.rotation = Quaternion.Slerp
			(
				turretCmp.horizontalRotatorTransform.rotation,
				targetHorizontalRotation,
				1f - Mathf.Pow(turretCmp.rotationDumping, Time.deltaTime)
			);

			turretCmp.verticalRotatorTransform.rotation = Quaternion.Slerp
			(
				turretCmp.verticalRotatorTransform.rotation,
				targetVerticalRotation,
				1f - Mathf.Pow(turretCmp.rotationDumping, Time.deltaTime)
			);

			turretCmp.verticalRotatorTransform.localEulerAngles = Vector3.right * turretCmp.verticalRotatorTransform.localEulerAngles.x;
		}
	}
}