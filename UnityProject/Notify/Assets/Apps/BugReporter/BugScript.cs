using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using BugReporter;

public class BugScript : MonoBehaviour
{
	private BugReporterApp _app;

	public BugMessage yoMessagePrefab;

	public BugReporterApp appPrefab;

	public RectTransform yoContainer;

	private readonly Dictionary<string, BugMessage> _messages = new Dictionary<string, BugMessage> ();

	public void Start ()
	{
		var appManager = AppManager.GetSingleton ();
		if (appManager) {
			_app = (BugReporterApp)appManager.GetAppBehaviour ("BugReporter");
		}

		var waitingContacts = _app.contacts.Where (contact => contact.isWaiting).ToArray ();

		if (waitingContacts.Length == 0) {
			_app.Kill ();
			return;
		}

		foreach (var contact in waitingContacts) {
			CreateMessageForContact (contact);
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

		CreateMessageForContact (notificationData.contact);
	}

	private void OnDismissNotification (object data)
	{
		var notificationData = (BugNotification)data;

		var contactName = notificationData.contact.GetName ();

		Destroy (_messages [contactName].gameObject);

		_messages.Remove (contactName);

		if (_messages.Count == 0) {
			_app.Kill ();
		}
	}

	private void CreateMessageForContact (Bug contact)
	{
		var yoInstance = Instantiate (yoMessagePrefab);

		yoInstance.transform.SetParent (yoContainer, false);

		yoInstance.Initialize (_app, contact);

		_messages [contact.GetName ()] = yoInstance;
	}
}
