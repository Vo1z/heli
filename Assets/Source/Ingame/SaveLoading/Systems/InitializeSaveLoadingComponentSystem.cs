using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;

namespace Ingame.SaveLoading
{
	public readonly struct InitializeSaveLoadingComponentSystem : IEcsInitSystem
	{
		private readonly EcsWorldInject _worldProject;
		
		private readonly EcsFilterInject<Inc<SaveLoadComponent>> _saveLoadCmpFilter;
		private readonly EcsPoolInject<SaveLoadComponent> _saveLoadCmpPool;

		public void Init(IEcsSystems systems)
		{
			if (!_saveLoadCmpFilter.Value.IsEmpty())
			{
				ref var existingSaveLoadCmp = ref _saveLoadCmpPool.Value.GetFirstComponent(_saveLoadCmpFilter.Value);
				FillSaveLoadComponentWithData(ref existingSaveLoadCmp);
				
				return;
			}

			int saveLoadEntity = _worldProject.Value.NewEntity();
			ref var newSaveLoadingCmp = ref _saveLoadCmpPool.Value.Add(saveLoadEntity);
			
			FillSaveLoadComponentWithData(ref newSaveLoadingCmp);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void FillSaveLoadComponentWithData(ref SaveLoadComponent saveLoadCmp)
		{
			saveLoadCmp._savedTypes ??= new(32);
			saveLoadCmp._serializedComponents ??= new(32);
			
			var componentTypes = JsonConvert.DeserializeObject<Type[]>(PlayerPrefs.GetString(SaveLoadComponent.PLAYER_PREFS_TYPES_KEY));
			
			if(componentTypes == null || componentTypes.IsEmpty())
				return;
			
			foreach (var savedCmpType in componentTypes)
			{
				string serializedSaveComponent = PlayerPrefs.GetString(savedCmpType.ToString());
				
				saveLoadCmp._savedTypes.Add(savedCmpType, false);
				saveLoadCmp._serializedComponents.Add(savedCmpType, serializedSaveComponent);
			}
		}
	}
}