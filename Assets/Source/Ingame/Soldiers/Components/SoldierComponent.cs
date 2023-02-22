using UnityEngine;

namespace Ingame.Soldiers
{
	public struct SoldierComponent
	{
		/// <summary>
		/// Time after which destination point will be changed
		/// </summary>
		public float travelTimeToDestination;
		/// <summary>
		/// Maximum distance from the target that soldier can aim at.
		/// Use this value to control accuracy. As the value bigger then less accurate soldier will be
		/// </summary>
		public float shootingAccuracyError;
		public float pauseBetweenShots;
		
		public Vector3 currentDestinationPos;
		public float currentTravelTime;
		public float timePassedFromLastShot;
	}
}