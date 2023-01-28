using System.Collections.Generic;
using UnityEngine;

namespace Tools.ClassExtensions
{
	public static partial class CollectionsMethodExtensions
	{
		public static T GetRandomElement<T>(this List<T> list)
		{
			return list[Random.Range(0, list.Count - 1)];
		}
	}
}