using UnityEngine;
using UnityEngine.UI;

public class TrayIcon : MonoBehaviour
{
	public void Initialize(AppBehaviour app)
	{
		GetComponent<Image>().sprite = app.iconTexture;
	}
}