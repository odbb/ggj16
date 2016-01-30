using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BugReporter
{
	public class BugReporterApp : AppBehaviour
	{

		public List<Bug> bugs = new List<Bug> ();
		public List<Sprite> errorSprites = new List<Sprite> ();

		public List<Sprite> warningSprites = new List<Sprite> ();

		private static void OnNotificationDismiss (object data)
		{
			var bugNotification = (BugNotification)data;

			bugNotification.bug.isWaiting = false;
		}

		private float _difficulty = 1;

		private float _maxDifficulty = 20;

		private IEnumerator Spam ()
		{
			while (isActiveAndEnabled) {
				yield return new WaitForSeconds (Mathf.Min (_maxDifficulty, 
					5 / _difficulty + Random.value * 20 / _difficulty));

				_difficulty++;

				var inactiveContacts = bugs.Where (bug => !bug.isWaiting).ToArray ();

				if (inactiveContacts.Length > 0) {
					var bug = inactiveContacts [Random.Range (0, inactiveContacts.Length - 1)];

					bug.isWaiting = true;

					SendNotification (new BugNotification (bug));
				}
			}
		}

		public override void Launch ()
		{
			for (var i = 0; i < 10; ++i) {
				var errorSprite = errorSprites [Random.Range (0, errorSprites.Count - 1)];
				var warningSprite = warningSprites [Random.Range (0, warningSprites.Count - 1)];

				bugs.Add (new Bug (i) {
					isWaiting = Random.value >= 0.5f,
					errorSprite = errorSprite,
					warningSprite = warningSprite
				});
			}

			foreach (var bug in bugs.Where(bug => bug.isWaiting)) {
				SendNotification (new BugNotification (bug));
			}

			StartCoroutine (Spam ());

			On (AppEvent.DismissNotification, OnNotificationDismiss);
		}


		public override void Cleanup ()
		{
		}
	}
}
