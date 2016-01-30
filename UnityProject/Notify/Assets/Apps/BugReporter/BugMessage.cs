using UnityEngine;
using UnityEngine.UI;

namespace BugReporter
{
	public class BugMessage : MonoBehaviour
	{
		public Image errorImage;
		public Image warningImage;
		private BugReporterApp _app;

		private Bug _bug;

		public void Initialize (BugReporterApp app, Bug bug)
		{
			_app = app;
			_bug = bug;

			errorImage.sprite = bug.errorSprite;
			warningImage.sprite = bug.warningSprite;
		}

		public void Dismiss ()
		{
			_app.DismissNotification (new BugNotification (_bug));
		}
	}
}
