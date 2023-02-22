namespace Ingame.Combat
{
	public struct UnguidedProjectileComponent
	{
		/// <summary>
		/// Amount of time that has to be passed after projectile launch in order to explode
		/// </summary>
		public float safeTime;
		/// <summary>
		/// Maximum flight time after which projectile will explode
		/// </summary>
		public float maxFlyTime;
		
		public float flyingSpeed;
	}
}