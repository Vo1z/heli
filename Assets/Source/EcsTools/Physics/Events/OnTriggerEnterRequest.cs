using UnityEngine;

namespace EcsTools.Physics
{
	public struct OnTriggerEnterRequest
	{
		public Collider sender;
		public Collider other;
	}
}