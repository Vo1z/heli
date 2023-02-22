using EcsTools.Physics;
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
using Ingame.CursorManagement;
using Ingame.Detection;
using Ingame.Detection.Radar;
using Ingame.Detection.Vision;
using Ingame.Settings.UI;
using Ingame.Setup;
using Ingame.Soldiers;
using Ingame.Vehicle.Turret;
using Ingame.Vfx.BilboardEffect;
using Ingame.Vfx.Destruction;
using Ingame.Vfx.Explosion;
using Ingame.Vfx.Helicopter;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

public sealed class EcsSetupScene : MonoBehaviour
{
	private EcsWorld _worldProject;
	private EcsWorld _sceneWorld;
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
	private void Construct
	(
		[Inject(Id = EcsWorldContext.ProjectContext)]EcsWorld worldProject,
		[Inject(Id = EcsWorldContext.SceneContext)]EcsWorld worldScene,
		InputActions inputActions, 
		ConfigProvider configProvider,
		DiContainer diContainer)
	{
		_worldProject = worldProject;
		_sceneWorld = worldScene;
		_updateSystems = new EcsSystems(worldScene);
		_lateUpdateSystems = new EcsSystems(worldScene);
		_fixUpdateSystems = new EcsSystems(worldScene);
#if UNITY_EDITOR
		_editorSystems = new EcsSystems(_sceneWorld);
#endif
		
		_inputActions = inputActions;
		_configProvider = configProvider;
		_diContainer = diContainer;
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

		_sceneWorld.Destroy();
		_sceneWorld = null;
	}

	private void AddInjections()
	{
		_updateSystems
			.AddWorld(_worldProject, "project")
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);

		_lateUpdateSystems
			.AddWorld(_worldProject, "project")
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);
		
		_fixUpdateSystems
			.AddWorld(_worldProject, "project")
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);
	}

	private void AddSystems()
	{
		_updateSystems
			.Add(new InitializeUnityModelsSystem())
			.Add(new InitializeUiSettingsScreenSystem())
			.Add(new SetCursorPropertiesSystem())
			//Time
			.Add(new IncrementTimerTimeSystem())
			//Helicopter
			.Add(new ConvertInputToHelicopterControlValuesSystem())
			.Add(new CalculateHelicopterStatsSystem())
			.Add(new RotateRotorSystem())
			//Combat
			.Add(new SpawnUnguidedProjectileSystem())
			.Add(new SpawnBulletFromSoldierToShootPlayerSystem())
			.Add(new MoveUnguidedProjectileSystem())
			.Add(new ExplodeUnguidedProjectileSystem())
			.Add(new SpawnRadarGuidedRocketSystem())
			.Add(new MoveRadarGuidedRocketSystem())
			.Add(new ExplodeRadarGuidedRocketSystem())
			.Add(new PerformExplosionSystem())
			//Health
			.Add(new ApplyDamageSystem())
			//Detection
			.Add(new PerformDetectionBetweenRadarAndTargetSystem())
			.Add(new BreakDetectionBetweenRadarAndTargetSystem())
			.Add(new PerformVisualDetectionSystem())
			.Add(new BreakVisualDetectionSystem())
			//Soldiers
			.Add(new GenerateSoldierDestinationPositionSystem())
			.Add(new ChangeSoldierDestinationPositionSystem())
			//Vehicle
			.Add(new RotateTurretTowardRadarTargetSystem())
			//Camerawork
			.Add(new RotateCameraFollowTargetSystem())
			//VFX
			.Add(new SpawnExplosionVfxSystem())
			.Add(new PutExplosionVfxBackToPoolSystem())
			.Add(new SetHelicopterPositionToTheMaterialsSystem())
			.Add(new SetDestructionViewSystem())
			.Add(new RotateBillboardObjectsTowardsCameraSystem())
			//Settings UI
			.Add(new ShowHideSettingsUiSystem())
			//Debugging
			.Add(new ChangeTargetFpsSystem())
			.Add(new ChangeControlsTypeSystem())
			.Add(new ReloadLevelSystem())
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
