using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour {
	public List<App> apps = new List<App>();
	public List<App> installedApps = new List<App>();

	public RectTransform iconPanel;
	private static AppManager _staticAppManager;

	public static AppManager GetSingleton()
	{
		return _staticAppManager;
	}

	public void Start ()
	{
		_staticAppManager = this;

		foreach (var installedApp in installedApps)
		{
			var appIcon = new GameObject(installedApp.appName, typeof(Image), typeof(Button), typeof(AppIcon));
			appIcon.transform.SetParent(iconPanel);

			appIcon.GetComponent<AppIcon>().Initialize(this, installedApp);
		}
	}

	public void AppLaunched(App app)
	{
		SceneManager.LoadScene(app.sceneName, LoadSceneMode.Additive);
	}

	public void AppDone(App app)
	{
		SceneManager.UnloadScene(app.sceneName);
	}
}
