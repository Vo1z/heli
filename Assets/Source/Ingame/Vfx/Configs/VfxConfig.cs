using System;
using Ingame.Health;
using Ingame.Vfx.Explosion;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Vfx
{
	[CreateAssetMenu(menuName = "Configs/VfxConfig")]
	public sealed class VfxConfig : ScriptableObject
	{
		[SerializeField] private DamageType_ExplosionVfxBaker_Dictionary explosionVfxDictionary;
		[InfoBox("Index of array identifies scene index in build settings")]
		[SerializeField] private PerSceneMaterialConfiguration[] perSceneMaterialConfigurations;

		public ExplosionVfxBaker GetExplosionVfxPrefab(DamageType damageType)
		{
			return explosionVfxDictionary[damageType];
		}

		public PerSceneMaterialConfiguration GetSceneMaterialConfiguration(int sceneIndex)
		{
			if (sceneIndex < 0 || sceneIndex > perSceneMaterialConfigurations.Length)
			{
				Debug.LogError($"Incorrect {nameof(sceneIndex)} ({sceneIndex}) argument. No configuration under this index exists, returning default value");
				return  perSceneMaterialConfigurations[0];
			}

			return perSceneMaterialConfigurations[sceneIndex];
		}
	}

	[Serializable]
	public sealed class DamageType_ExplosionVfxBaker_Dictionary : SerializableDictionary<DamageType, ExplosionVfxBaker> { }

	[Serializable]
	public struct PerSceneMaterialConfiguration
	{
		public Material[] requireHelicopterPositionMaterials;
	}
}