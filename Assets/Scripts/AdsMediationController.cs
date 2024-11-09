using System;
using UnityEngine;
using UnityEngine.Events;

public class AdsMediationController : MonoBehaviour
{
	public virtual void Init()
	{
		this.IsInited = true;
	}

	public virtual void InitInterstitialAd(UnityAction adClosedCallback, UnityAction adLoadFailedCallback, UnityAction adLoadSuccessCallback)
	{
		this.m_InterstitialAdCloseCallback = adClosedCallback;
		this.m_InterstitialAdLoadFailCallback = adLoadFailedCallback;
		this.m_InterstitialAdLoadSuccessCallback = adLoadSuccessCallback;
	}

	public virtual void ShowInterstitialAd(UnityAction closeCallback)
	{
	}

	public virtual void RequestInterstitialAd()
	{
	}

	public virtual bool IsInterstitialLoaded()
	{
		return false;
	}

	public virtual void InitRewardVideoAd(UnityAction videoClosed, UnityAction videoLoadFailed)
	{
		this.m_CloseRewardVideoCallback = videoClosed;
		this.m_LoadRewardVideoFailedCallback = videoLoadFailed;
	}

	public virtual void RequestRewardVideoAd()
	{
	}

	public virtual void ShowRewardVideoAd(UnityAction successCallback, UnityAction failedCallback)
	{
		this.m_WatchVideoSuccessCallback = successCallback;
		this.m_WatchVideoFailCallback = failedCallback;
	}

	public virtual bool IsRewardVideoLoaded()
	{
		return false;
	}

	public bool IsActive;

	protected UnityAction m_InterstitialAdCloseCallback;

	protected UnityAction m_InterstitialAdLoadFailCallback;

	protected UnityAction m_InterstitialAdLoadSuccessCallback;

	protected UnityAction m_WatchVideoSuccessCallback;

	protected UnityAction m_WatchVideoFailCallback;

	protected UnityAction m_CloseRewardVideoCallback;

	protected UnityAction m_LoadRewardVideoFailedCallback;

	public bool IsInited;
}
