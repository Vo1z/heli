namespace Ingame.Health
{
	public enum DamageType
	{
		None = 0,
		LightFirearm = 1 << 0,
		HeavyFirearm = 1 << 1,
		LightExplosive = 1 << 2,
		HeavyExplosive = 1 << 3
	}
}