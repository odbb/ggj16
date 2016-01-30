using UnityEngine;
using System.Collections;

public class YoScript : MonoBehaviour {
	public void Clicked()
	{
		AppManager.GetSingleton().GetAppBehaviour("yo").Kill();
	}
}
