using UnityEngine;

namespace EcsTools.ClassExtensions
{
	public static class UnityComponentsMethodExtensions
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