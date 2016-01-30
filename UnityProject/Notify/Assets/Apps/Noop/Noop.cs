using UnityEngine;
using System.Collections;

public class Noop : AppBehaviour
{
	private void Start()
	{
		StartCoroutine(Spam());
	}

	private IEnumerator Spam()
	{
		while (isActiveAndEnabled)
		{
			yield return new WaitForSeconds(Random.value);

			SendNotification(new Notification("asdf"));
		}
	}
}

