using System;
using System.Collections;
using UnityEngine;

public class TimePlay : MonoBehaviour
{
	private void Awake()
	{
		this.seconds = new WaitForSeconds(1f);
		this._oldTime = (this._nowTime = Time.time);
		base.StartCoroutine(this.CheckRun());
		if (PlayerPrefs.HasKey("metal.squad.day.of.year") && PlayerPrefs.GetInt("metal.squad.day.of.year") == DateTime.Now.DayOfYear)
		{
			TimePlay.timeRunGame = PlayerPrefs.GetInt("metal.squad.time.play.of.day");
			TimePlay.timeGachaFree = PlayerPrefs.GetInt("metal.squad.time.play.of.day.gacha");
			TimePlay.timeVideoReward = PlayerPrefs.GetInt("metal.squad.time.play.of.day.timeVideoReward");
		}
		else
		{
			PlayerPrefs.SetInt("metal.squad.day.of.year", DateTime.Now.DayOfYear);
			TimePlay.timeRunGame = 0;
			PlayerPrefs.SetInt("metal.squad.time.play.of.day", TimePlay.timeRunGame);
			TimePlay.timeGachaFree = 0;
			PlayerPrefs.SetInt("metal.squad.time.play.of.day.gacha", TimePlay.timeGachaFree);
			TimePlay.timeVideoReward = 0;
			PlayerPrefs.SetInt("metal.squad.time.play.of.day.timeVideoReward", TimePlay.timeVideoReward);
		}
		base.StartCoroutine(this.WaitOneSecond());
	}

	private IEnumerator WaitOneSecond()
	{
		TimePlay.timeRunGame++;
		TimePlay.timeGachaFree++;
		TimePlay.timeVideoReward++;
		yield return this.seconds;
		base.StartCoroutine(this.WaitOneSecond());
		yield break;
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("metal.squad.time.play.of.day", TimePlay.timeRunGame);
		PlayerPrefs.SetInt("metal.squad.time.play.of.day.gacha", TimePlay.timeGachaFree);
		PlayerPrefs.SetInt("metal.squad.time.play.of.day.timeVideoReward", TimePlay.timeVideoReward);
	}

	private IEnumerator CheckRun()
	{
		yield return new WaitUntil(() => ProfileManager.spinProfile != null);
		this.run = true;
		yield break;
	}

	private void Update()
	{
		if (!this.run)
		{
			return;
		}
		if (ProfileManager.isInit && ProfileManager.spinProfile != null && !ProfileManager.spinProfile.Free && ProfileManager.spinProfile.WaitTime > 0)
		{
			this._nowTime = Time.time;
			this._delta = this._nowTime - this._oldTime;
			if (this._delta > 0.99f)
			{
				this._oldTime = this._nowTime;
				ProfileManager.spinProfile.WaitTime--;
				if (ProfileManager.spinProfile.WaitTime <= 0)
				{
					ProfileManager.spinProfile.Free = true;
				}
			}
		}
	}

	private float _oldTime;

	private float _nowTime;

	private float _delta;

	private bool run;

	public static int timeRunGame;

	public static int timeGachaFree;

	public static int timeVideoReward;

	private WaitForSeconds seconds;
}
