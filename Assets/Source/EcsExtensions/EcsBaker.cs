using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Ingame.EcsExtensions
{
	public abstract class EcsBaker : MonoBehaviour
	{
		protected EcsWorld _world;
		
		[Inject]
		protected virtual void Construct(EcsWorld world)
		{
			_world = world;
		}
		
		private void Awake()
		{
			int entity = _world.NewEntity();
			Bake(entity);
			
			Destroy(this);
		}

		protected abstract void Bake(int entity);
	}
}