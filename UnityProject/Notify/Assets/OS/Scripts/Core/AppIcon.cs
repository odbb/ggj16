using System;
using UnityEngine;
using UnityEngine.UI;

public class AppIcon : MonoBehaviour
{
	public AppManager manager;
	public AppBehaviour app;
	private AppBehaviour _appBehaviourInstance;

	public CanvasGroup notificationGroup;
	public Text notificationText;

	private int _numNotifications = 0;

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
			var notificationData = (INotification) data;

			_numNotifications++;

			if (_numNotifications > 0)
			{
				ShowNotifications();
			}

			manager.AddAppNotification(app, notificationData);
		});
	}

	private void ShowNotifications()
	{
		notificationGroup.alpha = 1;
		notificationText.text = _numNotifications + "";
	}

	public AppBehaviour GetAppBehaviour()
	{
		return _appBehaviourInstance;
	}
}