using System;
using Ingame.Setup;
using Leopotam.EcsLite;
using Source.EcsExtensions.EntityReference;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace EcsTools.Convertion
{
	public sealed class ConvertToEcsEntity : MonoBehaviour
	{
		[FormerlySerializedAs("worldContext")] [SerializeField] private EcsWorldContext ecsWorldContext = EcsWorldContext.SceneContext;
		
		private EcsWorld _worldProject;
		private EcsWorld _worldScene;

		[Inject]
		private void Construct
		(
			[Inject(Id = EcsWorldContext.ProjectContext, Optional = true)]
			EcsWorld worldProject,
			[Inject(Id = EcsWorldContext.SceneContext, Optional = true)]
			EcsWorld worldScene
		)
		{
			_worldProject = worldProject;
			_worldScene = worldScene;

			if (ecsWorldContext == EcsWorldContext.ProjectContext && worldProject == null)
				throw new NullReferenceException($"{ecsWorldContext} ECS world is missing for {nameof(ConvertToEcsEntity)}");

			if (ecsWorldContext == EcsWorldContext.SceneContext && worldScene == null)
				throw new NullReferenceException($"{ecsWorldContext} ECS world is missing for {nameof(ConvertToEcsEntity)}");
			
			InitializeEntity();
		}

		private void InitializeEntity()
		{
			EcsWorld ecsWorld = null;

			ecsWorld = ecsWorldContext == EcsWorldContext.ProjectContext ? _worldProject : _worldScene;

			int entity = ecsWorld.NewEntity();
			
			SetupBakers(entity, ecsWorld);
			SetupEntityReference(entity);
			
			Destroy(this);
		}

		private void SetupBakers(int entity, EcsWorld world)
		{
			var bakers = GetComponents<EcsMonoBaker>();
			
			foreach (var ecsBaker in bakers)
			{
				ecsBaker.Bake(entity, world);
				
				switch (ecsBaker.destroyOptions)
				{
					case BakerLifetimeOptions.DestroyAfterBaking:
						Destroy(ecsBaker);
						break;
					case BakerLifetimeOptions.DontDestroyInEditor:
#if !UNITY_EDITOR
						Destroy(ecsBaker);
#endif
						break;
					case BakerLifetimeOptions.DontDestroyEverywhere:
						break;
				}
			}
		}

		private void SetupEntityReference(int entity)
		{
			if(!TryGetComponent(out EcsEntityReference entityRef))
				return;
			
			entityRef.entity = entity;
			entityRef.ecsWorldContext = ecsWorldContext;
		}
	}
}