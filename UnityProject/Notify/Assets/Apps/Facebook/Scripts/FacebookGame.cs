using UnityEngine;
using System.Collections;

public class FacebookGame : MonoBehaviour {
	private FacebookApp _app;

	// Use this for initialization
	public void Start () {
		var appManager = AppManager.GetSingleton();
		_app = (FacebookApp)appManager.GetAppBehaviour("facebook");
	}

	public void Liked()
	{
		_app.DismissNotification(new Notification("grandma"));
		_app.Kill();
	}
}
