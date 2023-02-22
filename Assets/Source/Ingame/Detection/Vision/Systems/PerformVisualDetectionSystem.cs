using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Tools.ClassExtensions;
using UnityEngine;

namespace Ingame.Detection.Vision
{
	public readonly struct PerformVisualDetectionSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, VisualDetectionTargetTag>, Exc<IsDeadTag, IsVisuallyDetectedTag>> _visualDetectionTargetFilter;
		private readonly EcsFilterInject<Inc<TransformModel, VisualDetectorComponent>, Exc<IsDeadTag>> _visualDetectorFilter;

		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<VisualDetectorComponent> _visualDetectorCmpPool;
		private readonly EcsPoolInject<IsVisuallyDetectedTag> _isVisuallyDetectedTagPool;
		private readonly EcsPoolInject<IsAvailableToVisuallySeeTargetTag> _isAvailableToVisuallySeeTargetTagPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var detectorEntity in _visualDetectorFilter.Value)
			{
				ref var visualDetectorCmp = ref _visualDetectorCmpPool.Value.Get(detectorEntity);
				var detectorTransform = _transformMdlPool.Value.Get(detectorEntity).transform;
				
				foreach (var targetEntity in _visualDetectionTargetFilter.Value)
				{
					var targetTransform = _transformMdlPool.Value.Get(targetEntity).transform;
					var fromDetectorToTargetVector = targetTransform.position - detectorTransform.position;

					if(fromDetectorToTargetVector.magnitude > visualDetectorCmp.detectionDistance)
						continue;

					if(PhysicsUtilities.IsThereAnyObstacleBetween(detectorTransform, targetTransform))
						continue;

					_isAvailableToVisuallySeeTargetTagPool.Value.Add(detectorEntity);
					_isVisuallyDetectedTagPool.Value.Add(targetEntity);
				}
			}
		}
	}
}