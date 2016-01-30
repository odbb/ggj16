using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
	private static AppManager _staticAppManager;

	private bool _enableNextFrame;
	public List<AppBehaviour> apps = new List<AppBehaviour>();
	public CanvasGroup canvasGroup;

	public RectTransform iconPanel;

	private readonly Dictionary<string, AppBehaviour> _appInstances = new Dictionary<string, AppBehaviour>(); 

	public static AppManager GetSingleton()
	{
		return _staticAppManager;
	}

	public void Start()
	{
		_staticAppManager = this;

		foreach (var app in apps)
		{
			var appName = app.name;

			var appIcon = new GameObject(appName, typeof (Image), typeof (Button), typeof (AppIcon)).GetComponent<AppIcon>();

			appIcon.transform.SetParent(iconPanel);

			appIcon.GetComponent<AppIcon>().Initialize(this, app);

			_appInstances.Add(appName, appIcon.GetAppBehaviour());
		}
	}

	public void AppLaunched(AppBehaviour app)
	{
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;
		SceneManager.LoadScene(app.name, LoadSceneMode.Additive);
	}

	public void AppDone(AppBehaviour app)
	{
		SceneManager.UnloadScene(app.name);

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