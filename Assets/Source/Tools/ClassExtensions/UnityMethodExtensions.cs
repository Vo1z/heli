using UnityEngine;

namespace Tools.ClassExtensions
{
	public static partial class UnityMethodExtensions
	{
		public static void SetGoInactive(this Component component)
		{
			component.gameObject.SetActive(false);
		}
		
		public static void SetGoActive(this Component component)
		{
			component.gameObject.SetActive(true);
		}
	}
}