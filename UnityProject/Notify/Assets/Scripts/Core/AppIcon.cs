using System;
using UnityEngine;
using UnityEngine.UI;

public class AppIcon : MonoBehaviour
{
    public AppManager manager;
    public App app;
    private AppBehaviourBase _appBehaviourInstance;

    public void Start()
    {
    }

    private void LaunchApp()
    {
        manager.AppLaunched(app);

        _appBehaviourInstance.Launch();
    }

    public void Initialize(AppManager appManager, App installedApp)
    {
        manager = appManager;
        app = installedApp;

        GetComponent<Image>().sprite = app.iconTexture;
        GetComponent<Button>().onClick.AddListener(LaunchApp);

        _appBehaviourInstance = Instantiate(app.appBehaviourPrefab);

        _appBehaviourInstance.On(AppBehaviourBase.AppEvent.Done, () =>
        {
            _appBehaviourInstance.Cleanup();

            manager.AppDone(app);
        });
    }
}