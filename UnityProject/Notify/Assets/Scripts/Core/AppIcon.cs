using System;
using UnityEngine;
using UnityEngine.UI;

public class AppIcon : MonoBehaviour
{
	public AppManager manager;
	public AppBehaviour appBehaviourPrefab;
	private AppBehaviour _appBehaviourInstance;

	public void Start()
	{
	}

	private void LaunchApp()
	{
		manager.AppLaunched(appBehaviourPrefab);

		_appBehaviourInstance.Launch();
	}

	public void Initialize(AppManager appManager, AppBehaviour installedApp)
	{
		manager = appManager;
		appBehaviourPrefab = installedApp;

		GetComponent<Image>().sprite = appBehaviourPrefab.iconTexture;
		GetComponent<Button>().onClick.AddListener(LaunchApp);

		_appBehaviourInstance = Instantiate(appBehaviourPrefab);

		_appBehaviourInstance.On(AppBehaviour.AppEvent.Done, () =>
		{
			_appBehaviourInstance.Cleanup();

			manager.AppDone(appBehaviourPrefab);
		});
	}

	public AppBehaviour GetAppBehaviour()
	{
		return _appBehaviourInstance;
	}
}