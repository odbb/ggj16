using System.Collections.Generic;

public class NotificationInfo
{
	public Dictionary<string, Notification> notifications = new Dictionary<string, Notification>();

	public void AddNotification(Notification notificationData)
	{
		notifications[notificationData.id] = notificationData;
	}

	public void DismissNotification(Notification notificationData)
	{
		notifications.Remove(notificationData.id);
	}
}