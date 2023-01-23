using Ingame.EcsExtensions;
using Ingame.Movement;
using Ingame.Player;
using Source.Ingame.UnityModels;
using UnityEngine;

namespace Source.Ingame.Player
{
	public sealed class PlayerBaker : EcsBaker
	{
		protected override void Bake(int entity)
		{
			var velocityPool = _world.GetPool<VelocityComponent>();
			var playerTagPool = _world.GetPool<PlayerTag>();
			var transformModelPool = _world.GetPool<TransformModel>();

			velocityPool.Add(entity).velocity = Vector3.up;
			playerTagPool.Add(entity);
			transformModelPool.Add(entity).transform = transform;
		}
	}
}