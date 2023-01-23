using Ingame.EcsExtensions;
using Ingame.Player;

namespace Source.Ingame.Player
{
	public sealed class PlayerBaker : EcsBaker
	{
		protected override void Bake(int entity)
		{
			var playerTagPool = _world.GetPool<PlayerTag>();
			playerTagPool.Add(entity);
		}
	}
}