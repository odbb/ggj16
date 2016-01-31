using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
	public float alpha = 1;
	public float minAlpha = 0.1f;
	public float lerpFactor = 0.05f;
	public float waitTime = 3.0f;

	private bool fading = false;
	// Use this for initialization
	public IEnumerator Start ()
	{
		yield return new WaitForSeconds(waitTime);

		fading = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (fading)
		{
			alpha = Mathf.Lerp(alpha, 0, lerpFactor);

			GetComponent<CanvasGroup>().alpha = alpha;

			if (alpha <= minAlpha)
			{
				Destroy(gameObject);
			}
		}
	}
}
