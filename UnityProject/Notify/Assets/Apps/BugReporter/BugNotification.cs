using System.Collections.Generic;
using UnityEngine;

namespace BugReporter
{
	public class BugNotification : Notification
	{
		public Bug contact;

		public BugNotification (Bug contact) : base (contact.GetName ())
		{
			text = new List<Sprite> {
				contact.profileSprite,
				contact.waveSprite
			};

			this.contact = contact;
		}
	}
}