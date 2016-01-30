using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AppManager : MonoBehaviour {
    public List<App> apps = new List<App>();
    public List<App> installedApps = new List<App>();

    public RectTransform iconPanel;

	public void Start () {
	    foreach (var installedApp in installedApps)
	    {
	        var appIcon = new GameObject(installedApp.appName, typeof(Image), typeof(Button), typeof(AppIcon));
            appIcon.transform.SetParent(iconPanel);

            appIcon.GetComponent<AppIcon>().Initialize(this, installedApp);
	    }
	}

    public void AppLaunched(App app)
    {
        iconPanel.gameObject.SetActive(false);
    }

    public void AppDone(App app)
    {
        iconPanel.gameObject.SetActive(true);
    }
}
