using UnityEngine;

namespace BugReporter
{
	public class Bug
	{
		public int id;

		public Sprite errorSprite;
		public Sprite warningSprite;

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