﻿using UnityEngine;
using UnityEngine.UI;


class BallGameHUD : MonoBehaviour
{
	[SerializeField] private Text _levelLabel;
	[SerializeField] private Text _turnsLabel;
	[SerializeField] private Text _targetLabel;

	private BallGameController _ballGameController;

	public void Setup (BallGameController ballGameController)
	{
		_ballGameController = ballGameController;
	}

	private void Update ()
	{
		if (_ballGameController == null)
			return;

		_turnsLabel.text = string.Format ("{0}", _ballGameController.TurnsLeft);
		_levelLabel.text = string.Format ("{0}", _ballGameController.Level);
		_targetLabel.text = string.Format ("{0}/{1}", _ballGameController.BallsDestroyedThisLevel, _ballGameController.BallTargetThisLevel);
	}
}
