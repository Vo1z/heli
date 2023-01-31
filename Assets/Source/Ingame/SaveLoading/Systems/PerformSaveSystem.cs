using System;
using System.Collections.Generic;
using EcsTools.ClassExtensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json;
using UnityEngine;

namespace Ingame.SaveLoading
{
	public sealed class PerformSaveSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<SaveLoadComponent>> _saveLoadCmpFilter;
		private readonly EcsPoolInject<SaveLoadComponent> _saveLoadCmpPool;

		private readonly EcsFilterInject<Inc<RequestSavingEvent>> _requestSavingEvnFilter;
		private readonly EcsPoolInject<RequestSavingEvent> _requestSavingEvnPool;

		private readonly List<Type> _savedTypes = new(32);
		
		public void Run(IEcsSystems systems)
		{
			if(_requestSavingEvnFilter.Value.IsEmpty() || _saveLoadCmpFilter.Value.IsEmpty())
				return;
			
			ref var saveLoadCmp = ref _saveLoadCmpPool.Value.GetFirstComponent(_saveLoadCmpFilter.Value);
			
			_requestSavingEvnPool.Value.RemoveAllComponents(_requestSavingEvnFilter.Value);
			_savedTypes.Clear();
			
			foreach (var keyValuePair in saveLoadCmp._savedTypes)
			{
				var typeOfSavedData = keyValuePair.Key;
				bool wasDataModified = keyValuePair.Value;
				
				_savedTypes.Add(typeOfSavedData);
				
				if(!wasDataModified)
					continue;

				string serializedValue = saveLoadCmp._serializedComponents[typeOfSavedData];
				PlayerPrefs.SetString(typeOfSavedData.ToString(), serializedValue);
			}

			string serializedTypes = JsonConvert.SerializeObject(_savedTypes);
			PlayerPrefs.SetString(SaveLoadComponent.PLAYER_PREFS_TYPES_KEY, serializedTypes);
			PlayerPrefs.Save();
		}
	}
}