using UnityEngine;
using UnityEngine.UI;

public class AppIcon : MonoBehaviour
{
	private AppBehaviour _appBehaviourInstance;
	public AppBehaviour app;
	public AppManager manager;

	public CanvasGroup notificationGroup;
	public Text notificationText;

	public void Start()
	{
	}

	private void LaunchApp()
	{
		manager.AppLaunched(app);

		_appBehaviourInstance.Launch();
	}

	public void Initialize(AppManager appManager, AppBehaviour installedApp)
	{
		manager = appManager;
		app = installedApp;

		GetComponent<Image>().sprite = app.iconTexture;
		GetComponent<Button>().onClick.AddListener(LaunchApp);

		_appBehaviourInstance = Instantiate(app);

		_appBehaviourInstance.transform.SetParent(appManager.instanceContainer);

		_appBehaviourInstance.On(AppBehaviour.AppEvent.Done, data =>
		{
			_appBehaviourInstance.Cleanup();

			manager.AppDone(app);
		});

		_appBehaviourInstance.On(AppBehaviour.AppEvent.Notification, data =>
		{
			var notificationData = (Notification) data;

			manager.AddAppNotification(app, notificationData);

			ShowNotifications(manager.GetAppNotifications(app));
		});

		_appBehaviourInstance.On(AppBehaviour.AppEvent.DismissNotification, data =>
		{
			var notificationData = (Notification) data;

			manager.DismissAppNotification(app, notificationData);

			ShowNotifications(manager.GetAppNotifications(app));
		});
	}

	private void ShowNotifications(NotificationInfo appNotifications)
	{
		var numNotifications = appNotifications.notifications.Count;

		notificationGroup.alpha = numNotifications > 0 ? 1 : 0;
		var rectTransform = notificationGroup.GetComponent<RectTransform>();
		if (numNotifications < 10)
		{
			rectTransform.sizeDelta = new Vector2(16, 16);
		}
		else if (numNotifications < 100)
		{
			rectTransform.sizeDelta = new Vector2(24, 24);
		}
		else if (numNotifications < 1000)
		{
			rectTransform.sizeDelta = new Vector2(32, 32);
		}
		else
		{
			rectTransform.sizeDelta = new Vector2(48, 48);
		}

		notificationText.text = numNotifications + "";
	}

	public AppBehaviour GetAppBehaviour()
	{
		return _appBehaviourInstance;
	}
}