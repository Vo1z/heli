using UnityEngine;

namespace EcsTools.Physics
{
	public struct OnTriggerExitRequest
	{
		public Collider sender;
		public Collider other;
	}
}