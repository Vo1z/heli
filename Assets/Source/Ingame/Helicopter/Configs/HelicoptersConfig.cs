using System;
using UnityEngine;

namespace Ingame.Helicopter
{
	[CreateAssetMenu(menuName = "Configs/HelicoptersConfig")]
	public sealed class HelicoptersConfig : ScriptableObject
	{
		[SerializeField] private HelicopterConfigData[] helicopterConfigs;
		
		public HelicopterConfigData this[int helicopterId] => helicopterConfigs[helicopterId];

		public bool HasHelicopterId(int id)
		{
			return id > -1 && id < helicopterConfigs.Length;
		}
	}

	[Serializable]
	public struct HelicopterConfigData
	{
		public string helicopterName;
		public float speed;
	}
}