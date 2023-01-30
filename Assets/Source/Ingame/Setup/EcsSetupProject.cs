using Ingame.ConfigProvision;
using Ingame.Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using EcsTools.UnityModels;
using Ingame.LevelMamengement;
using Ingame.LevelMamengement.UI;
using Ingame.Settings;
using Ingame.Setup;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

public sealed class EcsSetupProject : MonoBehaviour
{
	private EcsWorld _worldProject;
	private EcsSystems _updateSystems;
#if UNITY_EDITOR
	private EcsSystems _editorSystems;
#endif

	private InputActions _inputActions;
	private ConfigProvider _configProvider;
	private DiContainer _diContainer;
	
	[Inject]
	private void Construct([Inject(Id = EcsWorldContext.ProjectContext)]EcsWorld worldProject, InputActions inputActions, ConfigProvider configProvider, DiContainer diContainer)
	{
		_worldProject = worldProject;
		_updateSystems = new EcsSystems(worldProject);
#if UNITY_EDITOR
		_editorSystems = new EcsSystems(_worldProject);
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
	
	private void OnDestroy()
	{
		_updateSystems.Destroy();
		_updateSystems = null;
		
#if UNITY_EDITOR
		_editorSystems.Destroy();
		_editorSystems = null;
#endif

		_worldProject.Destroy();
		_worldProject = null;
	}

	private void AddInjections()
	{
		_updateSystems
			.Inject(_worldProject)
			.Inject(_inputActions)
			.Inject(_configProvider)
			.Inject(_diContainer);
	}

	private void AddSystems()
	{
		_updateSystems
			//One frame (these systems are at the beginning because events should be processed by scene EcsWorld)
			.Add(new RemoveLevelManagementEventsSystem())
			//Initialization
			.Add(new InitializeGameSettingsSystem())
			.Add(new InitializeUnityModelsSystem())
			.Add(new InitializeLevelManagementSystem())
			//Input 
			.Add(new ReceiveInputSystem())
			//Level management
			.Add(new ChangeLevelSystem())
			.Add(new UpdateLoadingUiSystem());


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
	}
}
