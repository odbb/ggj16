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
			yield return new WaitForSeconds(Random.value * 10);

			SendNotification(new Notification("grandma"));
		}
	}
}
