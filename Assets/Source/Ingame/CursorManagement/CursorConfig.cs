using System;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.CursorManagement
{
	[CreateAssetMenu(menuName = "Configs/CursorConfig")]
	public sealed class CursorConfig : ScriptableObject
	{
		[SerializeField] private CursorSceneProperties defaultCursorProperties;
		[InfoBox("Index of the array corresponds to the index of the scene")]
		[SerializeField] private CursorSceneProperties[] cursorProperties;

		public CursorSceneProperties GetCursorProperties(int sceneIndex)
		{
			if (sceneIndex < 0 || sceneIndex >= cursorProperties.Length)
			{
				Debug.LogWarning($"[{nameof(CursorConfig)}] there is no configuration for scene with index {sceneIndex}");
				return defaultCursorProperties;
			}

			return cursorProperties[sceneIndex];
		}
	}

	[Serializable]
	public struct CursorSceneProperties
	{
		public bool isEnabledByDefault;
	}
}