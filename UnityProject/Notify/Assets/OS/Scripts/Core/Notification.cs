using System.Collections.Generic;
using UnityEngine;

public class Notification
{
	public Notification(string id)
	{
		this.id = id;
	}

	public string id;
	public List<Sprite> text = null;
}