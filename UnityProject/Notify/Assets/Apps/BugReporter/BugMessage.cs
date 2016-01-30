using UnityEngine;
using UnityEngine.UI;

namespace BugReporter
{
	public class Message : MonoBehaviour
	{
		public Image profileImage;
		public Image waveImage;
		private BugReporterApp _app;

		private Bug _contact;

		public void Initialize (BugReporterApp app, Bug contact)
		{
			_app = app;
			_contact = contact;

			profileImage.sprite = contact.profileSprite;
			waveImage.sprite = contact.waveSprite;
		}

		public void Dismiss ()
		{
			_app.DismissNotification (new BugNotification (_contact));
		}
	}
}
