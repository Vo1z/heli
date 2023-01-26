using UnityEngine;

namespace EcsExtensions.Physics
{
	public struct OnTriggerStayRequest
	{
		public Collider sender;
		public Collider other;
	}
}