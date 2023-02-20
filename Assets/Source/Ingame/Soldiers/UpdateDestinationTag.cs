namespace Ingame.Soldiers
{
	/// <summary>
	/// Use this tag to attach to soldier in order to update destination position on nav mesh.
	/// This tag is used in optimization purposes so we dont assign destination pos each frame for each soldiers
	/// </summary>
	public struct UpdateDestinationTag
	{
		
	}
}