using UnityEngine;
using UnityEngine.UI;

namespace Yo
{
	public class Message : MonoBehaviour
	{
		public Image profileImage;
		public Image waveImage;
		private App _app;

		private Contact _contact;

		public void Initialize(App app, Contact contact)
		{
			_app = app;
			_contact = contact;

			profileImage.sprite = contact.profileSprite;
			waveImage.sprite = contact.waveSprite;
		}

		public void Dismiss()
		{
			_app.DismissNotification(new YoNotification(_contact));
		}
	}
}
