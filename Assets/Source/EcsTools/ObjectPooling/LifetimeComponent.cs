namespace EcsTools.ObjectPooling
{
	/// <summary>
	/// Use this component to identify the lifetime of an entity for object pooling.
	/// Recommended to use with TimerComponent
	/// </summary>
	public struct LifetimeComponent
	{
		public float lifetime;
	}
}