﻿using UnityEngine;
using System.Collections;

public class NoopGame : MonoBehaviour {
	public void OnGUI()
	{
		if (GUILayout.Button("NOPE"))
		{
			AppManager.GetSingleton().GetAppBehaviour("noop").Kill();
		}
	}
}