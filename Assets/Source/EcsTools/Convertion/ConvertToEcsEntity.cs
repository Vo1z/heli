using System;
using System.Runtime.InteropServices;
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
		}

		private void Awake()
		{
			EcsWorld ecsWorld = null;

			if (ecsWorldContext == EcsWorldContext.ProjectContext)
				ecsWorld = _worldProject;
			else
				ecsWorld = _worldScene;

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
				Destroy(ecsBaker);
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