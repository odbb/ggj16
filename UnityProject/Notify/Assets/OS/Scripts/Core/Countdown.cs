using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
	public Sprite[] countdownSprites;

	// Use this for initialization
	public IEnumerator Start ()
	{
		var image = GetComponent<Image>();

		image.sprite = countdownSprites[5];
		yield return new WaitForSeconds(1);
		image.sprite = countdownSprites[4];
		yield return new WaitForSeconds(1);
		image.sprite = countdownSprites[3];
		yield return new WaitForSeconds(1);
		image.sprite = countdownSprites[2];
		yield return new WaitForSeconds(1);
		image.sprite = countdownSprites[1];
		yield return new WaitForSeconds(1);
		image.sprite = countdownSprites[0];
		yield return new WaitForSeconds(1);

		Application.Quit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
