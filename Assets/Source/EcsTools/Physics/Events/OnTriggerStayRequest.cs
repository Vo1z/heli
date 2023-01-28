using UnityEngine;

namespace EcsTools.Physics
{
	public struct OnTriggerStayRequest
	{
		public Collider sender;
		public Collider other;
	}
}