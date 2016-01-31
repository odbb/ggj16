using UnityEngine;
using System.Collections;

public class NoopGame : MonoBehaviour {
	public void OnGUI()
	{
		GUILayout.Space(16);
		if (GUILayout.Button("NOPE"))
		{
			AppManager.GetSingleton().GetAppBehaviour("noop").Kill();
		}
	}
}
