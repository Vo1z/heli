using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ingame.SaveLoading
{
	[Serializable]
	public struct SaveLoadComponent
	{
		public const string PLAYER_PREFS_TYPES_KEY = "types";

		/// <summary>
		/// Identifies what types were modified and what types were not (true = was modified, false = was not modified)
		/// </summary>
		public Dictionary<Type, bool> _savedTypes;
		public Dictionary<Type, string> _serializedComponents;
		
		public void AddSaveComponent<T>(T cmp)
		{
			var typeOfSavedData = typeof(T);
			
			_savedTypes ??= new Dictionary<Type, bool>(32);
			_serializedComponents ??= new Dictionary<Type, string>(32);

			if (!_savedTypes.ContainsKey(typeOfSavedData))
			{
				_savedTypes.Add(typeOfSavedData, true);
				_serializedComponents.Add(typeOfSavedData, JsonConvert.SerializeObject(cmp, Formatting.Indented));
				
				return;
			}

			_savedTypes[typeOfSavedData] = true;
			_serializedComponents[typeOfSavedData] = JsonConvert.SerializeObject(cmp, Formatting.Indented);
		}

		public readonly T GetSaveComponent<T>()
		{
			return JsonConvert.DeserializeObject<T>(_serializedComponents[typeof(T)]);
		}

		public readonly bool HasSavedComponent<T>()
		{
			return _savedTypes.ContainsKey(typeof(T));
		}
	}
}