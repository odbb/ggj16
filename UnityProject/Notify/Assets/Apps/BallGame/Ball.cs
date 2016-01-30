using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

class Ball : MonoBehaviour
{
	private BallGameController _ballGameController = null;

	[SerializeField] private Sprite[] _ballColors;

	public int BallColorIndex {
		get;
		private set;
	}

	public void Setup (BallGameController ballGameController)
	{
		_ballGameController = ballGameController;

		BallColorIndex = Random.Range (0, _ballColors.Length);
		//GetComponent<SpriteRenderer> ().sprite = _ballColors [BallColorIndex];
		GetComponentInChildren<SpriteRenderer> ().sprite = _ballColors [BallColorIndex];

		ballGameController.RegisterBall (this);
	}

	private void OnMouseUp ()
	{
		_ballGameController.DestroyAdjacentMatchingBalls (this);
	}

	private void OnDestroy ()
	{
		if (_ballGameController != null)
			_ballGameController.BallCollected (this);
	}
}
