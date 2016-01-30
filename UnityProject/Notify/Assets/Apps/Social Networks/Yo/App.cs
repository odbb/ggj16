using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Yo
{
	public class App : AppBehaviour
	{
		public List<Contact> contacts = new List<Contact>();
		public List<Sprite> profileSprites = new List<Sprite>();

		public List<Sprite> waveSprites = new List<Sprite>();

		public void Start()
		{
			for (var i = 0; i < 10; ++i)
			{
				var profileSprite = profileSprites[Random.Range(0, profileSprites.Count - 1)];
				var waveSprite = waveSprites[Random.Range(0, waveSprites.Count - 1)];

				contacts.Add(new Contact(i)
				{
					isWaiting = Random.value >= 0.5f,
					profileSprite = profileSprite,
					waveSprite = waveSprite
				});
			}

			foreach (var contact in contacts.Where(contact => contact.isWaiting))
			{
				SendNotification(new YoNotification(contact));
			}

			StartCoroutine(Spam());

			On(AppEvent.DismissNotification, OnNotificationDismiss);
		}

		private static void OnNotificationDismiss(object data)
		{
			var yoNotification = (YoNotification) data;

			yoNotification.contact.isWaiting = false;
		}

		private IEnumerator Spam()
		{
			while (isActiveAndEnabled)
			{
				yield return new WaitForSeconds(5 + Random.value * 5);

				var inactiveContacts = contacts.Where(contact => !contact.isWaiting).ToArray();

				if (inactiveContacts.Length > 0)
				{
					var contact = inactiveContacts[Random.Range(0, inactiveContacts.Length - 1)];

					contact.isWaiting = true;

					SendNotification(new YoNotification(contact));
				}
			}
		}
	}
}