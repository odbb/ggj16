using UnityEngine;
using UnityEngine.UI;

public class RandomEmoji : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<Image>().sprite = RandomEmojiGenerator.GetRandomEmoji();
	}
}
