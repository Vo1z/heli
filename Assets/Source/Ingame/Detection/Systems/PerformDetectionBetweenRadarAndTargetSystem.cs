using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Detection
{
	public readonly struct PerformDetectionBetweenRadarAndTargetSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, RadarComponent>, Exc<IsDeadTag>> _radarFilter;
		private readonly EcsFilterInject<Inc<TransformModel, DetectionTargetTag>, Exc<IsDetectedTag, IsDeadTag>> _detectionTargetFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RadarComponent> _radarCmpPool;
		private readonly EcsPoolInject<IsDetectedTag> _isDetectedTagPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var radarEntity in _radarFilter.Value)
			{
				ref var radarCmp = ref _radarCmpPool.Value.Get(radarEntity);
				var radarTransform = _transformMdlPool.Value.Get(radarEntity).transform;

				foreach (var targetEntity in _detectionTargetFilter.Value)
				{
					var targetTransform = _transformMdlPool.Value.Get(targetEntity).transform;
					var fromRadarToTargetVector = targetTransform.position - radarTransform.position;
					
					if(fromRadarToTargetVector.magnitude > radarCmp.detectionDistance)
						continue;
					
					if(Vector3.Angle(Vector3.up, fromRadarToTargetVector) > radarCmp.detectionAngle)
						continue;

					_isDetectedTagPool.Value.Add(targetEntity);
				}
			}
		}
	}
}