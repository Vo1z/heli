using UnityEngine;

namespace Source.Ingame.Input
{
	public struct InputComponent
	{
		//Helicopter
		public float pitchInput;
		public float rollInput;
		public float yawInput;
		public float throttleInput;

		//Combat
		public bool shootInput;

		//Camera
		public Vector2 rotationInput;

		//Utils
		public bool changeFpsInput;
	}
}