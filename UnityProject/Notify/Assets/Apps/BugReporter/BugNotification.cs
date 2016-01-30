using System.Collections.Generic;
using UnityEngine;

namespace BugReporter
{
	public class BugNotification : Notification
	{
		public Bug bug;

		public BugNotification (Bug bug) : base (bug.GetName ())
		{
			text = new List<Sprite> {
				bug.errorSprite,
				bug.warningSprite
			};

			this.bug = bug;
		}
	}
}