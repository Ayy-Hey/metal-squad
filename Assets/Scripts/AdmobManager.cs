using System;
using CrossAdPlugin;
using UnityEngine;
using UnityEngine.Events;

public class AdmobManager : MonoBehaviour
{
	public static AdmobManager Instance
	{
		get
		{
			if (AdmobManager.instance == null)
			{
				AdmobManager.instance = UnityEngine.Object.FindObjectOfType<AdmobManager>();
			}
			return AdmobManager.instance;
		}
	}

	private void Start()
	{
		if (this.isInit)
		{
			return;
		}
		try
		{
			Singleton<CrossAdsManager>.Instance.Preload("http://bigmath.vn/CrossAdsConfigAndroid_metalsquad.txt", "http://bigmath.vn/CrossAdsConfigIOS_metalsquad.txt", null);
		}
		catch (Exception ex)
		{
		}
		this.isInit = true;
		this.m_ApplicationKey = this.Android_Key;
		UnityEngine.Debug.Log("unity-script: MyAppStart Start called");
		
		
		UnityEngine.Debug.Log("unity-script: IronSource.Agent.validateIntegration");
		
		UnityEngine.Debug.Log("unity-script: IronSource.Agent.init");
		string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		
		this.RequestRewardBasedVideo();
	}

	private void InterstitialAdReadyEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got InterstitialAdReadyEvent");
	}

	

	private void InterstitialAdShowSucceededEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
	}

	

	private void InterstitialAdClickedEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got InterstitialAdClickedEvent");
	}

	private void InterstitialAdOpenedEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
	}

	private void InterstitialAdClosedEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got InterstitialAdClosedEvent");
		if (this.OnCloseInter != null)
		{
			this.OnCloseInter();
		}
		this.m_AdsTime = (float)RemoteConfigFirebase.Instance.GetLongValue(RemoteConfigFirebase.TIME_SHOW_INTERS, 90L);
		ProfileManager.settingProfile.IsMusic = this.isCacheStateMusic;
		ProfileManager.settingProfile.IsSound = this.isCacheStateSound;
	}

	private void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
	{
		UnityEngine.Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
	}

	private void RewardedVideoAdOpenedEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
	}

	

	private void RewardedVideoAdClosedEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got RewardedVideoAdClosedEvent " + this.OnVideoClosed);
		if (this.OnVideoClosed != null)
		{
			this.OnVideoClosed(this.isRewarded);
		}
		this.RequestRewardBasedVideo();
	}

	private void RewardedVideoAdStartedEvent()
	{
		UnityEngine.Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
	}

	private void RewardedVideoAdEndedEvent()
	{
		this.isRewarded = true;
		UnityEngine.Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
	}

	

	
	private void LateUpdate()
	{
		if (!this.isInit || AdmobManager.Instance.isTest)
		{
			return;
		}
		if (this.m_AdsTime > 0f)
		{
			this.m_AdsTime -= Time.deltaTime;
		}
	}

	private void OnApplicationPause(bool hasFocus)
	{
		
	}

	public void RequestInterstitial()
	{
		this.OnCloseInter = null;
		
	}

	public void RequestRewardBasedVideo()
	{
		if (this.IsVideoReady())
		{
			return;
		}
		
	}

	public void ShowInterstitial(Action OnCloseInter, bool isGamePlay = false)
	{
		this.OnCloseInter = OnCloseInter;
		if (ProfileManager.inAppProfile.vipProfile.level >= E_Vip.Vip1)
		{
			if (OnCloseInter != null)
			{
				OnCloseInter();
			}
			return;
		}
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			if (OnCloseInter != null)
			{
				OnCloseInter();
			}
			return;
		}
		bool flag = (double)UnityEngine.Random.Range(0f, 1f) < RemoteConfigFirebase.Instance.GetDoubleValue(RemoteConfigFirebase.RATE_SPAM_ADS, 1.0);
		bool flag2 = this.m_AdsTime <= 0f;
		this.isCacheStateMusic = ProfileManager.settingProfile.IsMusic;
		this.isCacheStateSound = ProfileManager.settingProfile.IsSound;
		
	}

	public bool IsVideoReady()
	{
		return false;
	}

	public void ShowRewardBasedVideo(UnityAction<bool> OnVideoClosed)
	{
		this.isCloseVideo = false;
		this.isRewarded = false;
		bool flag = false;
		this.OnVideoClosed = OnVideoClosed;
		try
		{
			if (this.IsVideoReady())
			{
				FireBaseManager.Instance.LogEvent(FireBaseManager.Show_Video);
				
			}
			else
			{
				this.RequestRewardBasedVideo();
				flag = true;
			}
		}
		catch
		{
			this.RequestRewardBasedVideo();
			flag = true;
		}
		try
		{
			if (flag)
			{
				PopupManager.Instance.ShowDialog(delegate(bool ok)
				{
					if (OnVideoClosed != null)
					{
						OnVideoClosed(false);
					}
					this.RequestRewardBasedVideo();
				}, 0, PopupManager.Instance.GetText(Localization0.Video_ad_is_not_ready_yet, null), PopupManager.Instance.GetText(Localization0.Warning, null));
			}
		}
		catch (Exception ex)
		{
			if (OnVideoClosed != null)
			{
				OnVideoClosed(true);
			}
		}
	}

	public bool isCountryRick
	{
		get
		{
			return false;
		}
	}

	private static AdmobManager instance;

	private int[] ListCountryRick = new int[]
	{
		235,
		234,
		111,
		82,
		39,
		13,
		118
	};

	private bool isRewarded;

	private UnityAction<bool> OnVideoClosed;

	private float m_AdsTime;

	public bool isTest;

	private float timeRequestAds;

	private bool isInit;

	public string Android_Key;

	public string IOS_Key;

	private string m_ApplicationKey;

	private Action OnCloseInter;

	private bool isCacheStateMusic;

	private bool isCacheStateSound;

	private bool isCloseVideo;
}
