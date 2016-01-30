using System.Collections.Generic;
using UnityEngine;

namespace Yo
{
	public class YoNotification : Notification
	{
		public Contact contact;

		public YoNotification(Contact contact) : base(contact.GetName())
		{
			text = new List<Sprite>
			{
				contact.profileSprite,
				contact.waveSprite
			};

			this.contact = contact;
		}
	}
}