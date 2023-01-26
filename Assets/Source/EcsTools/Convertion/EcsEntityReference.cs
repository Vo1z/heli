using UnityEngine;

namespace Source.EcsExtensions.EntityReference
{
	[DisallowMultipleComponent]
	public sealed class EcsEntityReference : MonoBehaviour
	{
		private int entity;
		
		public int Entity
		{
			get => entity;
			set => entity = value;
		}
	}
}