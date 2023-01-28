using System;
using Ingame.Health;
using Ingame.Vfx.Explosion;
using UnityEngine;

namespace Ingame.Vfx
{
	[CreateAssetMenu(menuName = "Configs/VfxConfig")]
	public sealed class VfxConfig : ScriptableObject
	{
		[SerializeField] private DamageType_ExplosionVfxBaker_Dictionary explosionVfxDictionary;

		public ExplosionVfxBaker GetExplosionVfxPrefab(DamageType damageType)
		{
			return explosionVfxDictionary[damageType];
		}
	}

	[Serializable]
	public sealed class DamageType_ExplosionVfxBaker_Dictionary : SerializableDictionary<DamageType, ExplosionVfxBaker> { }
}