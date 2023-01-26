using UnityEngine;

namespace EcsExtensions.Physics
{
	public struct OnTriggerEnterRequest
	{
		public Collider sender;
		public Collider other;
	}
}