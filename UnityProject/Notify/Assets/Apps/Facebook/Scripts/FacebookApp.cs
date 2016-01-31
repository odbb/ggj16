using UnityEngine;
using System.Collections;

public class FacebookApp : AppBehaviour {
	private void Start()
	{
		StartCoroutine(Spam());
	}

	private IEnumerator Spam()
	{
		while (isActiveAndEnabled)
		{
			yield return new WaitForSeconds(2 + Random.value * 20);

			SendNotification(new Notification("grandma"));
		}
	}
}
