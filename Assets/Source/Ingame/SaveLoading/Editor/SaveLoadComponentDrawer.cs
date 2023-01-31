using System.Text;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Ingame.SaveLoading.Editor
{
	public sealed class SaveLoadComponentDrawer : EcsComponentInspectorTyped<SaveLoadComponent>
	{
		private StringBuilder _stringBuilder = new(32);
		
		public override bool OnGuiTyped(string label, ref SaveLoadComponent saveLoadCmp, EcsEntityDebugView entityView)
		{
			_stringBuilder.Clear();

			if (saveLoadCmp._savedTypes == null)
			{
				EditorGUILayout.LabelField("Saved components are null");
				return false;
			}

			EditorGUILayout.LabelField($"Count: {saveLoadCmp._savedTypes.Count}");
			
			foreach (var pair in saveLoadCmp._serializedComponents)
			{
				GUILayout.Label($"{pair.Key}\n" +
				                $"{pair.Value}");
			}
			
			return false;
		}
	}
}