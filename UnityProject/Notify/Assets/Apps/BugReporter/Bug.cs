using UnityEngine;

namespace BugReporter
{
	public class Bug
	{
		public int id;

		public Sprite profileSprite;
		public Sprite waveSprite;

		public bool isWaiting;

		public Bug (int _id)
		{
			id = _id;
		}

		public string GetName ()
		{
			return id.ToString ();
		}
	}
}