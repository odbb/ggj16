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

		public List<Bug> contacts = new List<Bug> ();
		public List<Sprite> profileSprites = new List<Sprite> ();

		public List<Sprite> waveSprites = new List<Sprite> ();

		private static void OnNotificationDismiss (object data)
		{
			var yoNotification = (BugNotification)data;

			yoNotification.contact.isWaiting = false;
		}

		private float _difficulty = 1;

		private float _maxDifficulty = 20;

		private IEnumerator Spam ()
		{
			while (isActiveAndEnabled) {
				yield return new WaitForSeconds (Mathf.Min (_maxDifficulty, 
					5 / _difficulty + Random.value * 20 / _difficulty));

				_difficulty++;

				var inactiveContacts = contacts.Where (contact => !contact.isWaiting).ToArray ();

				if (inactiveContacts.Length > 0) {
					var contact = inactiveContacts [Random.Range (0, inactiveContacts.Length - 1)];

					contact.isWaiting = true;

					SendNotification (new BugNotification (contact));
				}
			}
		}

		public override void Launch ()
		{
			for (var i = 0; i < 10; ++i) {
				var profileSprite = profileSprites [Random.Range (0, profileSprites.Count - 1)];
				var waveSprite = waveSprites [Random.Range (0, waveSprites.Count - 1)];

				contacts.Add (new Bug (i) {
					isWaiting = Random.value >= 0.5f,
					profileSprite = profileSprite,
					waveSprite = waveSprite
				});
			}

			foreach (var contact in contacts.Where(contact => contact.isWaiting)) {
				SendNotification (new BugNotification (contact));
			}

			StartCoroutine (Spam ());

			On (AppEvent.DismissNotification, OnNotificationDismiss);
		}


		public override void Cleanup ()
		{
		}
	}
}
