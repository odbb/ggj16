﻿using UnityEngine;
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
			yield return new WaitForSeconds(Random.value * 10);

			SendNotification(new Notification("asdf"));
		}
	}

	public override void Cleanup()
	{
		DismissNotification(new Notification("asdf"));
	}
}

