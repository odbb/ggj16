using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
	private static AppManager _staticAppManager;

	private readonly Dictionary<string, AppBehaviour> _appInstances = new Dictionary<string, AppBehaviour>();
	private readonly Dictionary<string, NotificationInfo> _appNotifications = new Dictionary<string, NotificationInfo>();
	private readonly Dictionary<string, TrayIcon> _trayIcons = new Dictionary<string, TrayIcon>();

	private bool _enableNextFrame;

	private bool _gameBegun;

	private float _nextUpdateTime;

	private float _startTime;

	public AppIcon appIconTemplate;
	public List<AppBehaviour> apps = new List<AppBehaviour>();
	public CanvasGroup canvasGroup;

	public RectTransform iconPanel;

	public Transform instanceContainer;

	public Camera mainCamera;

	public float score;

	public Text scoreText;

	public SpeedIndicator speedIndicator;

	public float timeLimit = 120.0f;
	public Transform trayContainer;
	public TrayIcon trayIconTemplate;

	public float updateInterval = 0.1f;

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

			appIcon.transform.SetParent(iconPanel, false);

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


		canvasGroup.interactable = false;
		canvasGroup.alpha = 0.5f;
	}

	public void AppLaunched(AppBehaviour app)
	{
		var appNameLowerCase = AppNameLowerCase(app);

		DisableMainOS();

		_loadedScene = appNameLowerCase;
		SceneManager.LoadScene(appNameLowerCase, LoadSceneMode.Additive);
	}


	public void AppDone(AppBehaviour app)
	{
		_loadedScene = null;
		SceneManager.UnloadScene(AppNameLowerCase(app));

		_enableNextFrame = true;
	}

	public void BeginGame()
	{
		if (!_gameBegun)
		{
			_gameBegun = true;
			_startTime = Time.time;

			EnableMainOS();
		}
	}

	public Slider slider;

	[UsedImplicitly]
	private void Update()
	{
		CheckEnable();

		if (_gameBegun)
		{
			if (Time.time > _nextUpdateTime)
			{
				_nextUpdateTime = Time.time + updateInterval;


				float numNotifications = _appNotifications.Values
					.Sum(notificationInfo => notificationInfo.notifications.Count);

				if (numNotifications < 1.0f) // hyper speed time!
				{
					score += 1000.0f;
				}
				else
				{
					score += 100.0f/(5 + numNotifications);
				}

				speedIndicator.SetNumNotifications(numNotifications);

				scoreText.text = Mathf.RoundToInt(score) + "";
			}

			var elapsedTime = Time.time - _startTime;

			slider.value = elapsedTime/timeLimit;

			if (elapsedTime > timeLimit)
			{
				_gameBegun = false;

				if (_loadedScene != null)
				{
					SceneManager.UnloadScene(_loadedScene);
					mainCamera.gameObject.SetActive(true);
					_loadedScene = null;
				}

				canvasGroup.interactable = false;
				canvasGroup.alpha = 0;

				winner.gameObject.SetActive(true);
				winnerText.text = Mathf.RoundToInt(score) + "";
			}
		}
	}

	public RectTransform winner;
	public Text winnerText;
	private string _loadedScene;

	private void DisableMainOS()
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


	public NotificationInfo GetAppNotifications(string appName)
	{
		return _appNotifications[appName.ToLower()];
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