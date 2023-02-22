using UnityEngine;

namespace Tools.ClassExtensions
{
	public static class PhysicsUtilities
	{
		public static bool IsThereAnyObstacleBetween(Transform transform1, Transform transform2)
		{
			if (Physics.Linecast(transform1.position, transform2.position, out RaycastHit hit))
			{
				if (hit.collider.transform == transform1 || hit.collider.transform == transform2)
					return false;
				
				return true;
			}

			return false;
		}
	}
}