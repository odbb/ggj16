using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AppBehaviour : MonoBehaviour
{
	public Sprite iconTexture;

	public enum AppEvent
	{
		Done,
		Notification
	}

	public delegate void EventHandler(object data);

	private readonly Dictionary<AppEvent, HashSet<EventHandler>> _eventListeners = new Dictionary<AppEvent, HashSet<EventHandler>>();

	public void On(AppEvent evt, EventHandler listener)
	{
		if (!_eventListeners.ContainsKey(evt))
		{
			_eventListeners[evt] = new HashSet<EventHandler>();
		}

		_eventListeners[evt].Add(listener);
	}

	public void Kill()
	{
		DispatchEvent(AppEvent.Done);
	}

	private void DispatchEvent(AppEvent evt, object eventData = null)
	{
		if (!_eventListeners.ContainsKey(evt)) return;

		foreach (var action in _eventListeners[evt])
		{
			action(eventData);
		}
	}

	public virtual void Launch()
	{
		
	}

	public virtual void Cleanup()
	{
		
	}

	public void SendNotification(INotification notification)
	{
		DispatchEvent(AppEvent.Notification, notification);
	}
}