using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RandomPhoto : MonoBehaviour {
	public List<Sprite> photos = new List<Sprite>();

	public void Start()
	{
		GetComponent<Image>().sprite = photos[Random.Range(0, photos.Count - 1)];
	}
}
