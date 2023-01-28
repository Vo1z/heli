using NaughtyAttributes;
using UnityEngine;

namespace Source.EcsExtensions.EntityReference
{
	[DisallowMultipleComponent]
	public sealed class EcsEntityReference : MonoBehaviour
	{
		[ReadOnly]
		public int entity;
	}
}