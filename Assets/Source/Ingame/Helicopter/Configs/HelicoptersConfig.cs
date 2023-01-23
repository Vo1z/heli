using System;
using UnityEngine;

namespace Ingame.Helicopter
{
	[CreateAssetMenu(menuName = "Configs/HelicoptersConfig")]
	public sealed class HelicoptersConfig : ScriptableObject
	{
		[SerializeField] private HelicopterConfigData[] helicopterConfigs;

		public bool HasHelicopterId(int id)
		{
			return id > -1 && id < helicopterConfigs.Length;
		}

		public HelicopterConfigData GetHelicopterConfigData(int id)
		{
			return helicopterConfigs[id];
		}
	}

	[Serializable]
	public struct HelicopterConfigData
	{
		public string helicopterName;
		
		public float throttleGainSpeed;
		public float rotationResponsiveness;
	}
}