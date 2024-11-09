using System;
using UnityEngine;
using UnityEngine.Events;

public class AdmobMediationController : AdsMediationController
{
	public override void Init()
	{
		base.Init();
	}

	public override void InitInterstitialAd(UnityAction adClosedCallback, UnityAction adLoadFailedCallback, UnityAction adLoadSuccessCallback)
	{
	}

	public override void RequestInterstitialAd()
	{
	}

	public override bool IsInterstitialLoaded()
	{
		return false;
	}

	public string GetInterstitialAdUnit()
	{
		return this.m_AndroidAdmobID_Intertitial;
	}

	public override void ShowInterstitialAd(UnityAction closeCallback)
	{
		base.ShowInterstitialAd(closeCallback);
	}

	public override void InitRewardVideoAd(UnityAction videoClosed, UnityAction videoLoadFailed)
	{
		base.InitRewardVideoAd(videoClosed, videoLoadFailed);
	}

	private void HandleRewardBaseVideoCompleted(object sender, EventArgs e)
	{
		UnityEngine.Debug.Log("Reward Video Complete");
		this.m_IsWatchSuccess = true;
	}

	public override void RequestRewardVideoAd()
	{
		base.RequestRewardVideoAd();
	}

	public override void ShowRewardVideoAd(UnityAction successCallback, UnityAction failedCallback)
	{
		base.ShowRewardVideoAd(successCallback, failedCallback);
	}

	public override bool IsRewardVideoLoaded()
	{
		return false;
	}

	private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs e)
	{
		UnityEngine.Debug.Log("Reward Left Application");
	}

	private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
	{
		string text = "not null";
		if (this.m_WatchVideoSuccessCallback == null)
		{
			text = "null";
		}
		text += " closed ";
		if (this.m_CloseRewardVideoCallback == null)
		{
			text = "null";
		}
		text = text + " " + this.m_IsWatchSuccess;
		UnityEngine.Debug.Log("Closed video " + text);
		if (Application.platform == RuntimePlatform.IPhonePlayer && this.m_IsWatchSuccess && this.m_WatchVideoSuccessCallback != null)
		{
			EventManager.AddEventNextFrame(this.m_WatchVideoSuccessCallback);
		}
		if (this.m_CloseRewardVideoCallback != null)
		{
			EventManager.AddEventNextFrame(this.m_CloseRewardVideoCallback);
		}
	}

	private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
	{
		UnityEngine.Debug.Log("Start video success");
	}

	private void HandleRewardBasedVideoOpened(object sender, EventArgs e)
	{
		UnityEngine.Debug.Log("Opened video success");
	}

	private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
	{
		UnityEngine.Debug.Log("Load video success");
	}

	private void OnCloseInterstitialAd(object sender, EventArgs e)
	{
		string str = "not null";
		if (this.m_InterstitialAdCloseCallback == null)
		{
			str = "null";
		}
		UnityEngine.Debug.Log("Close interstital " + str);
		if (this.m_InterstitialAdCloseCallback != null)
		{
			this.m_InterstitialAdCloseCallback();
		}
	}

	private void OnAdInterstitialSuccessToLoad(object sender, EventArgs e)
	{
		if (this.m_InterstitialAdLoadSuccessCallback != null)
		{
			this.m_InterstitialAdLoadSuccessCallback();
		}
		UnityEngine.Debug.Log("Load interstitial success");
	}

	private void OnInterstialAdOpening(object sender, EventArgs e)
	{
		UnityEngine.Debug.Log("Show Interstitial success");
	}

	private void OnApplicationQuit()
	{
	}

	public string m_AndroidAdmobID_Intertitial;

	public string m_AndroidAdmobID_Interstitial_NoCap;

	public string m_AndroidAdmobID_RewardVideo;

	public string m_IOSAdmobID_Interstitial;

	public string m_IOSAdmobID_Interstitial_NoCap;

	public string m_IOSAdmobID_RewardVideo;

	private bool m_IsWatchSuccess;
}
