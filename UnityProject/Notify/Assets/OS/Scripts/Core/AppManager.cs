using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
	private static AppManager _staticAppManager;

	private bool _enableNextFrame;
	public List<AppBehaviour> apps = new List<AppBehaviour>();
	public CanvasGroup canvasGroup;

	public RectTransform iconPanel;

	public Camera mainCamera;

	public Transform instanceContainer;
	public Transform trayContainer;

	private readonly Dictionary<string, AppBehaviour> _appInstances = new Dictionary<string, AppBehaviour>(); 
	private readonly Dictionary<string, NotificationInfo> _appNotifications = new Dictionary<string, NotificationInfo>();

	public AppIcon appIconTemplate;
	public TrayIcon trayIconTemplate;
	private Dictionary<string, TrayIcon> _trayIcons = new Dictionary<string, TrayIcon>();

	public static AppManager GetSingleton()
	{
		return _staticAppManager;
	}

	public void Start()
	{
#if UNITY_STANDALONE_WIN
		Screen.SetResolution(640, 960, false);
#endif

		_staticAppManager = this;

		foreach (var app in apps)
		{
			var appName = AppNameLowerCase(app);

			var appIcon = Instantiate(appIconTemplate);

			appIcon.transform.SetParent(iconPanel);

			appIcon.Initialize(this, app);
			//appIcon.gameObject.name.Replace( "(Clone)", "" );

			var trayIcon = Instantiate(trayIconTemplate);
			trayIcon.Initialize(app);
			trayIcon.transform.SetParent(trayContainer, false);

			_appInstances.Add(appName, appIcon.GetAppBehaviour());
			_appNotifications.Add(appName, new NotificationInfo());
			_trayIcons.Add(appName, trayIcon);

			trayIcon.gameObject.SetActive(false);
		}
	}

	public void AppLaunched(AppBehaviour app)
	{
		var appNameLowerCase = AppNameLowerCase(app);

		DisableMainOS(appNameLowerCase);

		SceneManager.LoadScene(appNameLowerCase, LoadSceneMode.Additive);
	}


	public void AppDone(AppBehaviour app)
	{
		SceneManager.UnloadScene(AppNameLowerCase(app));

		_enableNextFrame = true;
	}

	[UsedImplicitly]
	private void Update()
	{
		CheckEnable();
	}

	private void DisableMainOS(string appName)
	{
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;
		mainCamera.gameObject.SetActive(false);
	}

	private void EnableMainOS()
	{
		canvasGroup.interactable = true;
		canvasGroup.alpha = 1;
		mainCamera.gameObject.SetActive(true);
	}

	private void CheckEnable()
	{
		if (!_enableNextFrame) return;

		_enableNextFrame = false;

		EnableMainOS();
	}


	public AppBehaviour GetAppBehaviour(string appName)
	{
		return _appInstances[appName.ToLower()];
	}

	public void AddAppNotification(AppBehaviour app, Notification notificationData)
	{
		var appName = AppNameLowerCase(app);
		var notificationInfo = _appNotifications[appName];

		var numPrevNotifications = notificationInfo.notifications.Count;

		notificationInfo.AddNotification(notificationData);

		if (numPrevNotifications == 0 && notificationInfo.notifications.Count > 0)
		{
			_trayIcons[appName].gameObject.SetActive(true);
		}
	}

	public NotificationInfo GetAppNotifications(AppBehaviour app)
	{
		return _appNotifications[AppNameLowerCase(app)];
	}

	private static string AppNameLowerCase(AppBehaviour app)
	{
		return app.name.ToLower();
	}

	public void DismissAppNotification(AppBehaviour app, Notification notificationData)
	{
		var appName = AppNameLowerCase(app);

		var notificationInfo = _appNotifications[appName];

		var numPrevNotifications = notificationInfo.notifications.Count;

		notificationInfo.DismissNotification(notificationData);

		if (numPrevNotifications != 0 && notificationInfo.notifications.Count == 0)
		{
			_trayIcons[appName].gameObject.SetActive(false);
		}
	}
}