namespace Ingame.Combat
{
	public struct RadarGuidedRocketComponent
	{
#region Backed
		public float movementSpeed;
		public float rotationDumping;
		/// <summary>
		/// Time after launch when rocket can not explode 
		/// </summary>
		public float safeTime;
		/// <summary>
		/// How many seconds should pass after missing the target in order to explode
		/// </summary>
		public float lifetimeAfterMiss;
#endregion Backed
		
#region Runtime
		public float squaredDistanceToTheTarget;
		public float timePassedMovingFartherFromTarget;
#endregion Runtime
	}
}