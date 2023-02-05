using EcsTools.UnityModels;
using Ingame.Health;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Ingame.Detection
{
	public readonly struct BreakDetectionBetweenRadarAndTargetSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel, DetectionTargetTag, IsDetectedTag>, Exc<IsDeadTag>> _detectedTargetFilter;
		private readonly EcsFilterInject<Inc<TransformModel, RadarComponent>, Exc<IsDeadTag>> _radarFilter;

		private readonly EcsPoolInject<TransformModel> _transformMdlPool;
		private readonly EcsPoolInject<RadarComponent> _radarCmpPool;
		private readonly EcsPoolInject<IsDetectedTag> _isDetectedTagPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var targetEntity in _detectedTargetFilter.Value)
			{
				var targetTransform = _transformMdlPool.Value.Get(targetEntity).transform;

				foreach (var radarEntity in _radarFilter.Value)
				{
					ref var radarCmp = ref _radarCmpPool.Value.Get(radarEntity);
					var radarTransform = _transformMdlPool.Value.Get(radarEntity).transform;
					var fromRadarToTargetVector = targetTransform.position - radarTransform.position;
					
					if(fromRadarToTargetVector.magnitude <= radarCmp.detectionDistance)
						if(Vector3.Angle(Vector3.up, fromRadarToTargetVector) < radarCmp.detectionAngle)
							return;
				}
				
				_isDetectedTagPool.Value.Del(targetEntity);
			}
		}
	}
}