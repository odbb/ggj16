using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
	private static AppManager _staticAppManager;

	private bool _enableNextFrame;
	public List<App> apps = new List<App>();
	public CanvasGroup canvasGroup;

	public RectTransform iconPanel;
	public List<App> installedApps = new List<App>();

	private readonly Dictionary<string, AppBehaviour> _appInstances = new Dictionary<string, AppBehaviour>(); 

	public static AppManager GetSingleton()
	{
		return _staticAppManager;
	}

	public void Start()
	{
		_staticAppManager = this;

		foreach (var installedApp in installedApps)
		{
			var appIcon = new GameObject(installedApp.appName, typeof (Image), typeof (Button), typeof (AppIcon)).GetComponent<AppIcon>();

			appIcon.transform.SetParent(iconPanel);

			appIcon.GetComponent<AppIcon>().Initialize(this, installedApp);

			_appInstances.Add(installedApp.appName, appIcon.GetAppBehaviour());
		}
	}

	public void AppLaunched(App app)
	{
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;
		SceneManager.LoadScene(app.sceneName, LoadSceneMode.Additive);
	}

	public void AppDone(App app)
	{
		SceneManager.UnloadScene(app.sceneName);

		_enableNextFrame = true;
	}

	[UsedImplicitly]
	private void Update()
	{
		CheckEnable();
	}

	private void CheckEnable()
	{
		if (!_enableNextFrame) return;

		_enableNextFrame = false;

		canvasGroup.interactable = true;
		canvasGroup.alpha = 1;
	}

	public AppBehaviour GetAppBehaviour(string appName)
	{
		return _appInstances[appName];
	}
}