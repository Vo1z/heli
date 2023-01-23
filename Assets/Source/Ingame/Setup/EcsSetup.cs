using Ingame.Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Ingame.Movement;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

public sealed class EcsSetup : MonoBehaviour
{
	private EcsWorld _world;
	private EcsSystems _updateSystems;
	private EcsSystems _fixUpdateSystems;
#if UNITY_EDITOR
	private EcsSystems _editorSystems;
#endif

	private InputActions _inputActions;

	[Inject]
	private void Construct(EcsWorld world, InputActions inputActions)
	{
		_world = world;
		_updateSystems = new EcsSystems(world);
		_fixUpdateSystems = new EcsSystems(world);
#if UNITY_EDITOR
		_editorSystems = new EcsSystems(_world);
#endif
		_inputActions = inputActions;
	}

	private void Awake()
	{
		AddSystems();
		AddInjections();
		InitializeSystems();
	}

	private void Update()
	{
		_updateSystems.Run();
		
#if UNITY_EDITOR
		_editorSystems.Run();
#endif
	}

	private void FixedUpdate()
	{
		_fixUpdateSystems.Run();
	}

	private void OnDestroy()
	{
#if UNITY_EDITOR
		_editorSystems.Destroy();
		_editorSystems = null;
#endif
		
		_updateSystems.Destroy();
		_updateSystems = null;
		
		_fixUpdateSystems.Destroy();
		_fixUpdateSystems = null;
		
		_world.Destroy();
		_world = null;
	}

	private void AddInjections()
	{
		_updateSystems
			.Inject(_world)
			.Inject(_inputActions);
		
		_fixUpdateSystems
			.Inject(_world);
	}

	private void AddSystems()
	{
#if UNITY_EDITOR
		_editorSystems.Add(new EcsWorldDebugSystem());
#endif
		_updateSystems
			//Input
			.Add(new ReceiveInputSystem())
			//Movement
			.Add(new MoveObjectDueToItsVelocitySystem());
	}
	
	private void InitializeSystems()
	{
#if UNITY_EDITOR
		_editorSystems.Init();
#endif
		
		_updateSystems.Init();
		_fixUpdateSystems.Init();
	}
}
