using EcsTools.Convertion;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame.Detection.Radar
{
	public sealed class RadarBaker : EcsMonoBaker
	{
		[BoxGroup("Detection")]
		[SerializeField] [Range(0f, 90f)] private float detectionAngle = 45f;
		[BoxGroup("Detection")]
		[SerializeField] [Min(0f)] private float detectionDistance = 500f;
		
		public override void Bake(int entity, EcsWorld world)
		{
			var radarCmpPool = world.GetPool<RadarComponent>();
			var transformMdlPool = world.GetPool<TransformModel>();

			radarCmpPool.Add(entity) = new RadarComponent
			{
				detectionAngle = detectionAngle,
				detectionDistance = detectionDistance
			};

			transformMdlPool.Add(entity).transform = transform;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			var pos = transform.position;
			var forwardPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.forward) * Vector3.up * detectionDistance;
			var backPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.back) * Vector3.up * detectionDistance;
			var rightPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.right) * Vector3.up * detectionDistance;
			var leftPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.left) * Vector3.up * detectionDistance;
			
			var frPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.forward - Vector3.right) * Vector3.up * detectionDistance;
			var flPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.forward - Vector3.left) * Vector3.up * detectionDistance;
			var brPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.back - Vector3.right) * Vector3.up * detectionDistance;
			var blPos = pos + Quaternion.AngleAxis(detectionAngle, Vector3.back - Vector3.left) * Vector3.up * detectionDistance;

			var circleCenter = (leftPos + rightPos) / 2;
			float circleRadius = Vector3.Distance(leftPos, circleCenter);

			Handles.color = Color.yellow;
			Handles.DrawLine(pos, forwardPos);
			Handles.DrawLine(pos, backPos);
			Handles.DrawLine(pos, rightPos);
			Handles.DrawLine(pos, leftPos);
			Handles.DrawLine(pos, frPos);
			Handles.DrawLine(pos, flPos);
			Handles.DrawLine(pos, brPos);
			Handles.DrawLine(pos, blPos);
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(pos, detectionDistance);

			Handles.DrawWireDisc(circleCenter, Vector3.up, circleRadius);
		}
#endif
	}
}