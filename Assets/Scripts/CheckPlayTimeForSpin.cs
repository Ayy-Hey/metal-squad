using System;
using System.Collections;
using UnityEngine;

public class CheckPlayTimeForSpin : MonoBehaviour
{
	private void Awake()
	{
		this._oldTime = (this._nowTime = Time.time);
		CheckPlayTimeForSpin.SetTime(300f);
		base.StartCoroutine(this.CheckRun());
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
		if (CheckPlayTimeForSpin.time_video_reward > 0f)
		{
			CheckPlayTimeForSpin.time_video_reward -= Time.deltaTime;
		}
		if (!ProfileManager.spinProfile.Free && ProfileManager.spinProfile.WaitTime > 0)
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

	public static void SetTime(float time)
	{
		CheckPlayTimeForSpin.time_video_reward = time;
	}

	public static float GetTime()
	{
		return CheckPlayTimeForSpin.time_video_reward;
	}

	private float _oldTime;

	private float _nowTime;

	private float _delta;

	public static float time_video_reward;

	private bool run;
}
