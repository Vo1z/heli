using Source.EcsExtensions.EntityReference;

namespace Ingame.Soldiers
{
	public struct AttachedGroupComponent
	{
		/// <summary>
		/// Group to which unit (soldier) is attached to
		/// </summary>
		public EcsEntityReference boundedGroupEntityReference;
	}
}