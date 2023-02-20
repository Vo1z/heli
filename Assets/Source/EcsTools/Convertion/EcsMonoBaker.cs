using System;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.EcsExtensions.EntityReference;
using UnityEngine;

namespace EcsTools.Convertion
{
	[RequireComponent(typeof(EcsEntityReference))]
	public abstract class EcsMonoBaker : MonoBehaviour
	{
		[BoxGroup("Baker properties")]
		[Tooltip("Dont destroy after baking if you are using gizmos, in any other case baker should be destroyed.")]
		public BakerLifetimeOptions destroyOptions  = BakerLifetimeOptions.DestroyAfterBaking;
		
		public abstract void Bake(int entity, EcsWorld world);
	}

	[Serializable]
	public enum BakerLifetimeOptions
	{
		DestroyAfterBaking,
		DontDestroyInEditor,
		DontDestroyEverywhere
	}
}