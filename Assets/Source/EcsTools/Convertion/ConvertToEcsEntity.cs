using Leopotam.EcsLite;
using Source.EcsExtensions.EntityReference;
using UnityEngine;
using Zenject;

namespace EcsExtensions.Convertion
{
	public sealed class ConvertToEcsEntity : MonoBehaviour
	{
		private EcsWorld _world;
		
		[Inject]
		private void Construct(EcsWorld world)
		{
			_world = world;
		}
		
		private void Awake()
		{
			int entity = _world.NewEntity();

			SetupBakers(entity);
			SetupEntityReference(entity);
			
			Destroy(this);
		}

		private void SetupBakers(int entity)
		{
			var bakers = GetComponents<EcsMonoBaker>();
			
			foreach (var ecsBaker in bakers)
			{
				ecsBaker.Bake(entity, _world);
				Destroy(ecsBaker);
			}
		}

		private void SetupEntityReference(int entity)
		{
			if(!TryGetComponent(out EcsEntityReference entityRef))
				return;
			
			entityRef.Entity = entity;
		}
	}
}