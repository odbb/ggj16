using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEmojiGenerator : MonoBehaviour {

	public List<Sprite> allEmojis = new List<Sprite>();
	private static RandomEmojiGenerator _staticInstance;

	public void Awake()
	{
		_staticInstance = this;
	}

	private Sprite GetRandomEmojiInternal()
	{
		return allEmojis[Random.Range(0, allEmojis.Count)];
	}

	public static Sprite GetRandomEmoji()
	{
		return _staticInstance.GetRandomEmojiInternal();
	}
}
