namespace Ingame.Health
{
	public struct HealthComponent
	{
		/// <summary>
		/// Use DamageType enum flags to identify this value
		/// </summary>
		public int appliedDamageType;
		public float currentHealth;

		public bool IsDamageTypeApplied(in DamageType damageType)
		{
			return (appliedDamageType & (int)damageType) != 0;
		}
	}
}