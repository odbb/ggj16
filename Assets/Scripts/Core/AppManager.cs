using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class AppManager : MonoBehaviour {
    public List<App> apps = new List<App>();
    public List<App> installedApps = new List<App>();

    public RectTransform iconPanel;

	// Use this for initialization
	void Start () {
	    foreach (var installedApp in installedApps)
	    {
	        var appIcon = new GameObject(installedApp.appName, typeof(Image), typeof(Button));
	        appIcon.GetComponent<Image>().sprite = installedApp.iconTexture;
            appIcon.GetComponent<Button>().onClick.AddListener(CreateLaunchDelegate(installedApp));

	        appIcon.transform.parent = iconPanel;
	    }
	}

    private UnityAction CreateLaunchDelegate(App installedApp)
    {
        return delegate
        {
            var appBehaviourInstance = Instantiate(installedApp.appBehaviourPrefab);

            iconPanel.gameObject.SetActive(false);

            appBehaviourInstance.Launch();
            appBehaviourInstance.On(AppBehaviour.AppEvent.Done, () =>
            {
                appBehaviourInstance.Cleanup();

                Destroy(appBehaviourInstance.gameObject);

                iconPanel.gameObject.SetActive(true);
            });
        };
    }

    // Update is called once per frame
	void Update () {
	
	}
}
