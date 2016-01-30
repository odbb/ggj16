using UnityEngine;

static class BallGameSaveSystem
{
	public static int BallsCollected { get; set; }

	public static int MaxLevelReached { get; set; }

	private static void Load ()
	{
		BallsCollected = PlayerPrefs.GetInt ("BallGame.BallsCollected", 0);
		MaxLevelReached = PlayerPrefs.GetInt ("BallGame.MaxLevelReached", 0);
	}

	public static void Save ()
	{
		PlayerPrefs.SetInt ("BallGame.BallsCollected", BallsCollected);
		PlayerPrefs.SetInt ("BallGame.MaxLevelReached", MaxLevelReached);
	}

	public static string AsString ()
	{
		return "Balls Collected: " + BallsCollected + "\n" +
		"Max Level Reached: " + MaxLevelReached;
	}
}
