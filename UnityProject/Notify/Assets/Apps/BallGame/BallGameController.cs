using System;
using System.Collections.Generic;
using UnityEngine;

class BallGameController : MonoBehaviour
{
	public int Level { get; private set; }

	public int BallsDestroyedThisLevel { get; private set; }

	private int _turnsLeft;

	public int TurnsLeft {
		get { return _turnsLeft; }
		private set {
			if (value > TotalTurns)
				throw new InvalidOperationException ("TurnsLeft can't be greater than TotalTurns");
			_turnsLeft = value;
		}
	}

	public int InitialBalls { get; private set; }

	public int TotalTurns { get; private set; }

	public int BallTargetThisLevel { get; private set; }

	public bool GameIsOver { get; private set; }

	[SerializeField] private GameObject _ballPrefab;
	[SerializeField] private BallSpawner[] _ballSpawners;
	[SerializeField] private BallGameGameOverUI _gameOverUI;
	[SerializeField] private BallGameHUD _hud;

	private readonly HashSet<Ball> _balls = new HashSet<Ball> ();

	private void Start ()
	{
		InitialBalls = 70;

		_gameOverUI.Setup (this);
		_hud.Setup (this);

		_ballPrefab.SetActive (false);

		foreach (var ballSpawner in _ballSpawners) {
			ballSpawner.Setup (this, _ballPrefab);
		}

		BeginGame ();
	}

	private void BeginGame ()
	{
		GameIsOver = false;
		Level = 6;
		SpawnBalls (InitialBalls);
		BeginRound ();
	}

	public void RegisterBall (Ball demoBall)
	{
		_balls.Add (demoBall);
	}

	public void BallCollected (Ball ball)
	{
		_balls.Remove (ball);
	}

	private void BeginRound ()
	{
		TotalTurns = 2 + (10 - Level);
		TurnsLeft = TotalTurns;
		BallsDestroyedThisLevel = 0;
		BallTargetThisLevel = 2 + Level * 7;
	}

	private void EndOfRound ()
	{
		Debug.Log ("Round " + Level + " over");
		Level++;
		BallGameSaveSystem.MaxLevelReached = Mathf.Max (BallGameSaveSystem.MaxLevelReached, Level);
		int ballsToSpawn = 18 + Mathf.Clamp ((8 - Level) * 2, 0, 20);
		SpawnBalls (ballsToSpawn);
		BeginRound ();
	}

	private void SpawnBalls (int ballsToSpawn)
	{
		Debug.Log (string.Format ("Spawning {0} balls", ballsToSpawn));

		foreach (var ballSpawner in _ballSpawners) {
			ballSpawner.SpawnBalls (ballsToSpawn / _ballSpawners.Length);
		}
	}

	public void DestroyAdjacentMatchingBalls (Ball demoBall)
	{

		if (GameIsOver)
			return;

		int colorIndex = demoBall.BallColorIndex;

		Debug.Log ("Clicked on ball with color " + colorIndex);

		float limitSquared = Mathf.Pow (1.1f, 2);
		HashSet<Ball> matchedBalls = new HashSet<Ball> { demoBall };

		int matchedBallCount;
		do {
			matchedBallCount = matchedBalls.Count;
			FindAdjacentMatchingBalls (matchedBalls, colorIndex, limitSquared);
		} while (matchedBallCount != matchedBalls.Count);

		Debug.Log (string.Format ("Destroying {0} balls", matchedBalls.Count));

		foreach (var ball in matchedBalls) {
			Destroy (ball.gameObject);
			BallsDestroyedThisLevel++;
			BallGameSaveSystem.BallsCollected++;
		}

		if (BallsDestroyedThisLevel >= BallTargetThisLevel) {
			EndOfRound ();
		} else {
			TurnsLeft--;
			if (TurnsLeft == 0) {
				EndGame ();
			}
		}
	}

	private void FindAdjacentMatchingBalls (HashSet<Ball> matchedBalls, int colorIndex, float limitSquared)
	{
		foreach (var ball in _balls) {
			if (matchedBalls.Contains (ball))
				continue;

			if (ball.BallColorIndex != colorIndex)
				continue;

			bool adjacentToMatched = false;
			foreach (var matchedBall in matchedBalls) {
				if (Vector3.SqrMagnitude (matchedBall.transform.position - ball.transform.position) < limitSquared) {
					adjacentToMatched = true;
					break;
				}
			}

			if (adjacentToMatched) {
				matchedBalls.Add (ball);
			}
		}
	}

	private void EndGame ()
	{
		_gameOverUI.Show ();
		GameIsOver = true;
		BallGameSaveSystem.Save ();
	}

	public void Restart ()
	{
		Debug.Log ("Restarting game");
		foreach (var ball in _balls) {
			Destroy (ball.gameObject);
		}

		BeginGame ();
	}
}
