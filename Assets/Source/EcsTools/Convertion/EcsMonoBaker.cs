using Leopotam.EcsLite;
using UnityEngine;

namespace EcsTools.Convertion
{
	[RequireComponent(typeof(ConvertToEcsEntity))]
	public abstract class EcsMonoBaker : MonoBehaviour
	{
		public abstract void Bake(int entity, EcsWorld world);
	}
}