using UnityEngine;
using System.Collections;
using System.Linq;

public class BallApp : AppBehaviour
{
	private void Start()
	{
		StartCoroutine(Spam());
	}

	private int _nextNotificationId = 1;

	private IEnumerator Spam()
	{
		while (isActiveAndEnabled)
		{
			yield return new WaitForSeconds(Random.value * 10);

			_nextNotificationId++;

			SendNotification(new Notification(_nextNotificationId + ""));
		}
	}

	public override void Cleanup ()
	{
		var currentScore = PlayerPrefs.GetInt("BallGame.BallsCollected");

		// reduce the notifications by score from the game!
		var notifications = AppManager.GetSingleton().GetAppNotifications("BallGame").notifications;
		foreach (var notification in notifications.ToArray())
		{
			if (currentScore > 0)
			{
				currentScore --;
				DismissNotification(new Notification(notification.Key));
			}
			else
			{
				break;
			}
		}
	}
}
