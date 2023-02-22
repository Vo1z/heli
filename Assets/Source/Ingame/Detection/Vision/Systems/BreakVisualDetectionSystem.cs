using EcsTools.ClassExtensions;
using EcsTools.UnityModels;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Tools.ClassExtensions;
using UnityEngine;

namespace Ingame.Detection.Vision
{
	public readonly struct BreakVisualDetectionSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, VisualDetectionTargetTag, IsVisuallyDetectedTag>> _visualDetectionTargetFilter;
		private readonly EcsFilterInject<Inc<TransformModel, VisualDetectorComponent>> _visualDetectorFilter;
		
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<VisualDetectorComponent> _visualDetectorCmpPool;
		private readonly EcsPoolInject<IsVisuallyDetectedTag> _isVisuallyDetectedTagPool;
		private readonly EcsPoolInject<IsAvailableToVisuallySeeTargetTag> _isAvailableToVisuallySeeTargetTagPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var targetEntity in _visualDetectionTargetFilter.Value)
			{
				var targetTransform = _transformMdlPool.Value.Get(targetEntity).transform;
				bool isTargetVisibleByTheDetector = false;

				foreach (var detectorEntity in _visualDetectorFilter.Value)
				{
					ref var detectorCmp = ref _visualDetectorCmpPool.Value.Get(detectorEntity);
					var detectorTransform = _transformMdlPool.Value.Get(detectorEntity).transform;

					if (Vector3.Distance(targetTransform.position, detectorTransform.position) > detectorCmp.detectionDistance)
					{
						_isAvailableToVisuallySeeTargetTagPool.Value.TryDel(detectorEntity);
						continue;
					}

					if (PhysicsUtilities.IsThereAnyObstacleBetween(detectorTransform, targetTransform))
					{
						_isAvailableToVisuallySeeTargetTagPool.Value.TryDel(detectorEntity);
						continue;
					}
					
					isTargetVisibleByTheDetector = true;
				}
				
				if(!isTargetVisibleByTheDetector)
					_isVisuallyDetectedTagPool.Value.Del(targetEntity);
			}
		}
	}
}