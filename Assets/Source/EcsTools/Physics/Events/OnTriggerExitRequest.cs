using UnityEngine;

namespace EcsExtensions.Physics
{
	public struct OnTriggerExitRequest
	{
		public Collider sender;
		public Collider other;
	}
}