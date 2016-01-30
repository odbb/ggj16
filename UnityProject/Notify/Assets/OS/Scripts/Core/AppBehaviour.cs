using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AppBehaviour : MonoBehaviour
{
	public Sprite iconTexture;

	public enum AppEvent
	{
		Done
	}

	private readonly Dictionary<AppEvent, HashSet<Action>> eventListeners = new Dictionary<AppEvent, HashSet<Action>>();

	public void On(AppEvent evt, Action listener)
	{
		if (!eventListeners.ContainsKey(evt))
		{
			eventListeners[evt] = new HashSet<Action>();
		}

		eventListeners[evt].Add(listener);
	}

	public void Kill()
	{
		DispatchEvent(AppEvent.Done);
	}

	private void DispatchEvent(AppEvent evt)
	{
		if (!eventListeners.ContainsKey(evt)) return;

		foreach (var action in eventListeners[evt])
		{
			action();
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
		
	}
}