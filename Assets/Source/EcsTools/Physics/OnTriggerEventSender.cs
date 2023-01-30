using EcsTools.ClassExtensions;
using Ingame.Setup;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace EcsTools.Physics
{
	[RequireComponent(typeof(Collider))]
	public sealed class OnTriggerEventSender : MonoBehaviour
	{
		[SerializeField] private bool isSentOnEnter;
		[SerializeField] private bool isSentOnStay;
		[SerializeField] private bool isSentOnExit;

		private EcsWorld _world;
		private Collider _collider;
		
		[Inject]
		private void Construct([Inject(Id = EcsWorldContext.SceneContext)]EcsWorld world)
		{
			_world = world;
			_collider = GetComponent<Collider>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if(!isSentOnEnter)
				return;
			
			_world.SendSignal(new OnTriggerEnterRequest
			{
				sender = _collider,
				other = other
			});
		}

		private void OnTriggerStay(Collider other)
		{
			if(!isSentOnStay)
				return;
			
			_world.SendSignal(new OnTriggerStayRequest
			{
				sender = _collider,
				other = other
			});
		}

		private void OnTriggerExit(Collider other)
		{
			if(!isSentOnExit)
				return;
			
			_world.SendSignal(new OnTriggerExitRequest
			{
				sender = _collider,
				other = other
			});
		}
	}
}