using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
	public static CountdownTimer Instance
	{
		get
		{
			if (CountdownTimer.instance == null)
			{
				CountdownTimer.instance = UnityEngine.Object.FindObjectOfType<CountdownTimer>();
			}
			return CountdownTimer.instance;
		}
	}

	public void Start()
	{
		if (this.Text == null)
		{
			UnityEngine.Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
			return;
		}
	}

	public void Update()
	{
		if (!this.isTimerRunning)
		{
			return;
		}
		this.Countdown -= Time.deltaTime;
		this.Text.text = string.Empty + (int)this.Countdown;
		if (this.Countdown > 0f)
		{
			return;
		}
		this.isTimerRunning = false;
		this.Text.text = string.Empty;
		if (this.OnCountdownTimerHasExpired != null)
		{
			this.OnCountdownTimerHasExpired();
			this.OnCountdownTimerHasExpired = null;
		}
	}

	public void StartTimer(float countdown, Action timeoutFunc)
	{
		this.OnCountdownTimerHasExpired = timeoutFunc;
		this.Countdown = countdown;
		this.isTimerRunning = true;
	}

	public void CancelTimer()
	{
		this.OnCountdownTimerHasExpired = null;
		this.Countdown = 0f;
	}

	public void CancelNetworkTimer()
	{
		this.OnCountdownTimerHasExpired = null;
		this.Countdown = 0f;
	}

	private static CountdownTimer instance;

	public Action OnCountdownTimerHasExpired;

	private bool isTimerRunning;

	private float startTime;

	[Header("Reference to a Text component for visualizing the countdown")]
	public Text Text;

	private float Countdown = 5f;
}
