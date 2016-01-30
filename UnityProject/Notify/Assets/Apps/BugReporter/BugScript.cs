using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using BugReporter;

public class BugScript : MonoBehaviour
{
	private BugReporterApp _app;

	public BugMessage bugMessagePrefab;

	public BugReporterApp appPrefab;

	public RectTransform BugsContainer;

	private readonly Dictionary<string, BugMessage> _messages = new Dictionary<string, BugMessage> ();

	public void Start ()
	{
		var appManager = AppManager.GetSingleton ();
		if (appManager) {
			_app = (BugReporterApp)appManager.GetAppBehaviour ("BugReporter");
		}

		var waitingBugs = _app.bugs.Where (bug => bug.isWaiting).ToArray ();

		if (waitingBugs.Length == 0) {
			_app.Kill ();
			return;
		}

		foreach (var bug in waitingBugs) {
			CreateMessageForContact (bug);
		}

		_app.On (AppBehaviour.AppEvent.DismissNotification, OnDismissNotification);
		_app.On (AppBehaviour.AppEvent.Notification, OnNotification);
	}

	public void OnDestroy ()
	{
		_app.Off (AppBehaviour.AppEvent.DismissNotification, OnDismissNotification);
		_app.Off (AppBehaviour.AppEvent.Notification, OnNotification);
	}

	private void OnNotification (object data)
	{
		var notificationData = (BugNotification)data;

		CreateMessageForContact (notificationData.bug);
	}

	private void OnDismissNotification (object data)
	{
		var notificationData = (BugNotification)data;

		var contactName = notificationData.bug.GetName ();

		Destroy (_messages [contactName].gameObject);

		_messages.Remove (contactName);

		if (_messages.Count == 0) {
			_app.Kill ();
		}
	}

	private void CreateMessageForContact (Bug bug)
	{
		var bugInstance = Instantiate (bugMessagePrefab);

		bugInstance.transform.SetParent (BugsContainer, false);

		bugInstance.Initialize (_app, bug);

		_messages [bug.GetName ()] = bugInstance;
	}
}
