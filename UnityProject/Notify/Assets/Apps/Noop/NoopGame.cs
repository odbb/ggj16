using UnityEngine;
using System.Collections;

public class NoopGame : MonoBehaviour {
	public void Clicked()
	{
		AppManager.GetSingleton().GetAppBehaviour("noop").Kill();
	}
}
