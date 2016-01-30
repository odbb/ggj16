using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Yo;

public class YoScript : MonoBehaviour
{
	private App _app;

	[FormerlySerializedAs("yoPrefab")]
	public Message yoMessagePrefab;

	public App appPrefab;

	public RectTransform yoContainer;

	private readonly Dictionary<string, Message> _messages = new Dictionary<string, Message>(); 

	public void Start()
	{
		var appManager = AppManager.GetSingleton();
		if (appManager)
		{
			_app = (App) appManager.GetAppBehaviour("yo");
		}

		var waitingContacts = _app.contacts.Where(contact => contact.isWaiting).ToArray();

		if (waitingContacts.Length == 0)
		{
			_app.Kill();
			return;
		}

		foreach (var contact in waitingContacts)
		{
			CreateMessageForContact(contact);
		}

		_app.On(AppBehaviour.AppEvent.DismissNotification, OnDismissNotification);
		_app.On(AppBehaviour.AppEvent.Notification, OnNotification);
	}

	public void OnDestroy()
	{
		_app.Off(AppBehaviour.AppEvent.DismissNotification, OnDismissNotification);
		_app.Off(AppBehaviour.AppEvent.Notification, OnNotification);
	}

	private void OnNotification(object data)
	{
		var notificationData = (YoNotification) data;

		CreateMessageForContact(notificationData.contact);
	}

	private void OnDismissNotification(object data)
	{
		var notificationData = (YoNotification) data;

		var contactName = notificationData.contact.GetName();

		Destroy(_messages[contactName].gameObject);

		_messages.Remove(contactName);

		if (_messages.Count == 0)
		{
			_app.Kill();
		}
	}

	private void CreateMessageForContact(Contact contact)
	{
		var yoInstance = Instantiate(yoMessagePrefab);

		yoInstance.transform.SetParent(yoContainer);

		yoInstance.Initialize(_app, contact);

		_messages[contact.GetName()] = yoInstance;
	}
}
