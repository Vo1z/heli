using Ingame.Helicopter;
using UnityEditor;
using UnityEngine;

namespace Tools.HelicopterAttributte
{
	[CustomPropertyDrawer(typeof(HelicopterIdAttribute))]
	public sealed class HelicopterIdAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int inputHelicopterId = property.intValue;
			var helicoptersConfig = AssetDatabase.LoadAssetAtPath<HelicoptersConfig>("Assets/Configs/HelicoptersConfig.asset");

			var fieldPosition = position;
			var validationLabelPos = position;
			
			fieldPosition.width /= 2;
			validationLabelPos.x += fieldPosition.width + 5; 

			property.intValue = EditorGUI.IntField(fieldPosition, inputHelicopterId);

			if (helicoptersConfig.HasHelicopterId(inputHelicopterId))
			{
				EditorGUI.DrawRect(validationLabelPos, new Color(0.09f, 0.26f, 0.09f));
				EditorGUI.LabelField(validationLabelPos, $"Valid ID: ({helicoptersConfig.GetHelicopterConfigData(inputHelicopterId).helicopterName})");
			}
			else
			{
				EditorGUI.DrawRect(validationLabelPos, new Color(0.4f, 0f, 0f));
				EditorGUI.LabelField(validationLabelPos, $"Invalid ID");
			}
		}
	}
}