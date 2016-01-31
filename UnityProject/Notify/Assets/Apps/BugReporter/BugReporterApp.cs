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
		public List<Sprite> noErrorSprites = new List<Sprite> ();
		public List<Sprite> noWarningSprites = new List<Sprite> ();

		private static void OnNotificationDismiss (object data)
		{
			var bugNotification = (BugNotification)data;

			bugNotification.bug.isWaiting = false;
		}

		private float _difficulty = 1;

		private float _maxDifficulty = 5;

		private IEnumerator Spam ()
		{
			while (isActiveAndEnabled) {
				yield return new WaitForSeconds ( 
					5 / _difficulty + Random.value * 20 / _difficulty);

				_difficulty = Mathf.Min(_maxDifficulty, _difficulty + 1);

				var numNotifications = Random.Range(0, 5);

				for(var i=0; i< numNotifications; ++i)
				{
					var inactiveContacts = bugs.Where(bug => !bug.isWaiting).ToArray();

					if (inactiveContacts.Length > 0)
					{
						var bug = inactiveContacts[Random.Range(0, inactiveContacts.Length)];

						bug.isWaiting = true;

						SendNotification(new BugNotification(bug));
					}
					else
					{
						break;
					}
				}
			}
		}

		public void Start ()
		{
			for (var i = 0; i < 40; ++i) {
				var randomRange = Random.Range (0, errorSprites.Count);
				var noRandomRange = Random.Range (0, noErrorSprites.Count);
				var errorSprite = errorSprites [randomRange];
				var warningSprite = warningSprites [randomRange];
				var noErrorSprite = noErrorSprites [noRandomRange];
				var noWarningSprite = noWarningSprites [noRandomRange];
				var isWaiting = Random.value >= 0.5f;

				bugs.Add (new Bug (i) {
					isWaiting = isWaiting,
					errorSprite = isWaiting ? errorSprite : noErrorSprite,
					warningSprite = isWaiting ? warningSprite : noWarningSprite
				});
			}

			foreach (var bug in bugs.Where(bug => bug.isWaiting)) {
				SendNotification (new BugNotification (bug));
			}

			StartCoroutine (Spam ());

			On (AppEvent.DismissNotification, OnNotificationDismiss);
		}

	}
}
