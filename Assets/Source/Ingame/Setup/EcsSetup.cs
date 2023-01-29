using System;
using EcsTools.OneFrame;
using Ingame.Camerawork;
using Ingame.Combat;
using Ingame.ConfigProvision;
using Ingame.Debugging;
using Ingame.Health;
using Ingame.Helicopter;
using Ingame.Input;
using Ingame.UI.Debugging;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using EcsTools.Timer;
using EcsTools.UnityModels;
using Ingame.LevelMamengement;
using Ingame.Settings;
using Ingame.Vfx.Explosion;
using Ingame.Vfx.Helicopter;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

public sealed class EcsSetup : MonoBehaviour
{
	private EcsWorld _world;
	private EcsSystems _updateSystems;
	private EcsSystems _lateUpdateSystems;
	private EcsSystems _fixUpdateSystems;
#if UNITY_EDITOR
	private EcsSystems _editorSystems;
#endif

	private InputActions _inputActions;
	private ConfigProvider _configProvider;
	private DiContainer _diContainer;

	[Inject]
	private void Construct(EcsWorld world, InputActions inputActions, ConfigProvider configProvider, DiContainer diContainer)
	{
		_world = world;
		_updateSystems = new EcsSystems(world);
		_lateUpdateSystems = new EcsSystems(world);
		_fixUpdateSystems = new EcsSystems(world);
#if UNITY_EDITOR
		_editorSystems = new EcsSystems(_world);
#endif
		
		_inputActions = inputActions;
		_configProvider = configProvider;
		_diContainer = diContainer;
		
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

	private void LateUpdate()
	{
		_lateUpdateSystems.Run();
	}

	private void FixedUpdate()
	{
		_fixUpdateSystems.Run();
	}

	private void OnDestroy()
	{
		_updateSystems.Destroy();
		_updateSystems = null;
		
		_lateUpdateSystems.Destroy();
		_lateUpdateSystems = null;
		
		_fixUpdateSystems.Destroy();
		_fixUpdateSystems = null;
	
#if UNITY_EDITOR
		_editorSystems.Destroy();
		_editorSystems = null;
#endif

		_world.Destroy();
		_world = null;
	}

	private void AddInjections()
	{
		_updateSystems
			.Inject(_world)
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);

		_lateUpdateSystems
			.Inject(_world)
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);
		
		_fixUpdateSystems
			.Inject(_world)
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);
	}

	private void AddSystems()
	{
		_updateSystems
			//Initialization
			.Add(new InitializeGameSettingsSystem())
			.Add(new InitializeUnityModelsSystem())
			.Add(new InitializeLevelComponentSystem())
			//Input 
			.Add(new ReceiveInputSystem())
			//Time
			.Add(new IncrementTimerTimeSystem())
			//Helicopter
			.Add(new ConvertInputToHelicopterControlValuesSystem())
			.Add(new CalculateHelicopterStatsSystem())
			.Add(new RotateRotorSystem())
			//Combat
			.Add(new SpawnUnguidedRocketSystem())
			.Add(new MoveUnguidedRocketSystem())
			.Add(new ExplodeUnguidedRocketSystem())
			.Add(new PerformExplosionSystem())
			//Health
			.Add(new ApplyDamageSystem())
			//Camerawork
			.Add(new RotateCameraFollowTargetSystem())
			//VFX
			.Add(new SpawnExplosionVfxSystem())
			.Add(new PutExplosionVfxBackToPoolSystem())
			.Add(new SetHelicopterPositionToTheMaterialsSystem())
			//Debugging
			.Add(new ChangeTargetFpsSystem())
			.Add(new PresentDebuggingInfoToUiSystem())
			//One frame
			.Add(new RemovePhysicsEventsSystem());

		// _lateUpdateSystems


		_fixUpdateSystems
			//Helicopter
			.Add(new MoveHelicopterSystem())
			//Camerawork
			.Add(new MoveCameraFollowTargetSystem());

#if UNITY_EDITOR
		_editorSystems.Add(new EcsWorldDebugSystem());
#endif
	}

	private void InitializeSystems()
	{
#if UNITY_EDITOR
		_editorSystems.Init();
#endif
		
		_updateSystems.Init();
		_lateUpdateSystems.Init();
		_fixUpdateSystems.Init();
	}
}
