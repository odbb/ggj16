using System.Collections.Generic;
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

	public EventSystem eventSystem;

	public Camera mainCamera;

	public Transform instanceContainer;

	private readonly Dictionary<string, AppBehaviour> _appInstances = new Dictionary<string, AppBehaviour>(); 
	private readonly Dictionary<string, NotificationInfo> _appNotifications = new Dictionary<string, NotificationInfo>();

	public AppIcon appIconTemplate;

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

			appIcon.GetComponent<AppIcon>().Initialize(this, app);

			_appInstances.Add(appName, appIcon.GetAppBehaviour());
			_appNotifications.Add(appName, new NotificationInfo());
		}
	}

	public void AppLaunched(AppBehaviour app)
	{
		DisableMainOS();
		SceneManager.LoadScene(AppNameLowerCase(app), LoadSceneMode.Additive);
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

	private void DisableMainOS()
	{
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;
		eventSystem.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(false);
	}

	private void EnableMainOS()
	{
		canvasGroup.interactable = true;
		canvasGroup.alpha = 1;
		eventSystem.gameObject.SetActive(true);
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
		_appNotifications[AppNameLowerCase(app)].AddNotification(notificationData);
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
		_appNotifications[AppNameLowerCase(app)].DismissNotification(notificationData);
	}
}