using System;
using UnityEngine;
using UnityEngine.Serialization;

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
		[Header("Common")]
		public string helicopterName;
	
		[Header("Hardcore controls")]
		[Min(0f)] public float hcoreThrottleGainSpeed;
		[Min(0f)] public float hcoreMaxThrottle;
		[Min(0f)] public float hcoreRotationResponsiveness;
		[Min(0f)] public float hcoreAdditionalGravitation;

		[Header("Casual controls")]
		[Range(0f, 90f)] public float casualRotationAngle;
		[Range(0f, 1f)] public float casualRotationDamping;
		[Min(0f)] public float casualFlyForce;
		[Min(0f)] public float casualYawRotationSpeed;
		[Min(0f)] public float casualThrottleForce;
		[Range(0f, 1f)] public float casualThrottleGainDamping;

		/// <summary>
		/// This is when heli is trying to face the same direction as the velocity vector.
		/// As speed is smaller that min value helicopter is not facing velocity direction at all.
		/// As speed meets maximum speed then heli becomes uncontrollable at all.
		/// </summary>
		[Header("Normalization")]
		[Min(0f)] public Vector2 minMaxSpeedForNormalization;
		[Min(0f)] public float minNormalizationDamping;
	}
}