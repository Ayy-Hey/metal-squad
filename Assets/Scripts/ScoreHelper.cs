using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class ScoreHelper : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (PhotonNetwork.LocalPlayer != null && this.Score != this._currentScore)
		{
			this._currentScore = this.Score;
			PhotonNetwork.LocalPlayer.SetScore(this.Score);
		}
	}

	public int Score;

	private int _currentScore;
}
