using Leopotam.EcsLite;
using UnityEngine;

namespace EcsExtensions.Convertion
{
	[RequireComponent(typeof(ConvertToEcsEntity))]
	public abstract class EcsMonoBaker : MonoBehaviour
	{
		public abstract void Bake(int entity, EcsWorld world);
	}
}