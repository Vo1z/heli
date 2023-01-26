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
		public float maxThrottle;
		public float rotationResponsiveness;

		/// <summary>
		/// This is when heli is trying to face the same direction as the velocity vector.
		/// As speed is smaller that min value helicopter is not facing velocity direction at all.
		/// As speed meets maximum speed then heli becomes uncontrollable at all.
		/// </summary>
		public Vector2 minMaxSpeedForNormalization;
		[Min(0f)] public float minNormalizationDamping;

		[Min(0f)] public float additionalGravitation;
	}
}