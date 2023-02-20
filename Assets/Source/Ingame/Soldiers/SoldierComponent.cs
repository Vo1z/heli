using UnityEngine;

namespace Ingame.Soldiers
{
	public struct SoldierComponent
	{
		/// <summary>
		/// Time after which destination point will be changed
		/// </summary>
		public float travelTimeToDestination;
		
		public Vector3 currentDestinationPos;
		public float currentTravelTime;
	}
}