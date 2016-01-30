using UnityEngine;

namespace Yo
{
	public class Contact
	{
		public int id;

		public Sprite profileSprite;
		public Sprite waveSprite;

		public bool isWaiting;

		public Contact(int _id)
		{
			id = _id;
		}

		public string GetName()
		{
			return id.ToString();
		}
	}
}