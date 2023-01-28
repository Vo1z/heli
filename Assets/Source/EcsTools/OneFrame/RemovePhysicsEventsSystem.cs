using EcsTools.Physics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTools.OneFrame
{
	public readonly struct RemovePhysicsEventsSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<OnTriggerEnterRequest>> _onTriggerEnterReqFilter;
		private readonly EcsPoolInject<OnTriggerEnterRequest> _onTriggerEnterReqPool;
		
		private readonly EcsFilterInject<Inc<OnTriggerStayRequest>> _onTriggerStayReqFilter;
		private readonly EcsPoolInject<OnTriggerStayRequest> _onTriggerStayReqPool;
		
		private readonly EcsFilterInject<Inc<OnTriggerExitRequest>> _onTriggerExitReqFilter;
		private readonly EcsPoolInject<OnTriggerExitRequest> _onTriggerExitReqPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _onTriggerEnterReqFilter.Value)
			{
				_onTriggerEnterReqPool.Value.Del(entity);
			}
			
			foreach (var entity in _onTriggerStayReqFilter.Value)
			{
				_onTriggerStayReqPool.Value.Del(entity);
			}
			
			foreach (var entity in _onTriggerExitReqFilter.Value)
			{
				_onTriggerExitReqPool.Value.Del(entity);
			}
		}
	}
}