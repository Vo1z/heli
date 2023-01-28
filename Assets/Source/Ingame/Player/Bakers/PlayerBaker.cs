using EcsTools.Convertion;
using Ingame.Player;
using Leopotam.EcsLite;

namespace Source.Ingame.Player
{
	public sealed class PlayerBaker : EcsMonoBaker
	{
		public override void Bake(int entity, EcsWorld world)
		{
			var playerTagPool = world.GetPool<PlayerTag>();
			
			playerTagPool.Add(entity);
		}
	}
}