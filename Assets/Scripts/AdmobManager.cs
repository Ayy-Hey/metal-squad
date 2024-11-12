using System;
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
        Advertisements.Instance.Initialize();
        Advertisements.Instance.ShowBanner(BannerPosition.TOP, BannerType.Banner);

    }
    


	
    //callback called each time an interstitial is closed
    private void InterstitialClosed(string advertiser)
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
            GleyMobileAds.ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
        }
    }

    //callback called each time a rewarded video is closed
    //if completed = true, rewarded video was seen until the end
    private void CompleteMethod(bool completed, string advertiser)
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
            GleyMobileAds.ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);
            if (completed == true)
            {
                //give the reward
            }
            else
            {
                //no reward
            }
        }
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
        
        Advertisements.Instance.ShowInterstitial(()=>
        {
            OnCloseInter?.Invoke();   
        });
		
    }

    public bool IsVideoReady()
    {
        return Advertisements.Instance.IsRewardVideoAvailable();
    }

    public void ShowRewardBasedVideo(UnityAction<bool> OnVideoClosed)
    {
	
        if (this.IsVideoReady())
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.Show_Video);
            
            Advertisements.Instance.ShowRewardedVideo(given=>
            {
                OnVideoClosed?.Invoke(given);
            });
				
        }
        else
        {
            PopupManager.Instance.ShowDialog(delegate(bool ok)
            {
                OnVideoClosed?.Invoke(false);
            }, 0, PopupManager.Instance.GetText(Localization0.Video_ad_is_not_ready_yet, null), PopupManager.Instance.GetText(Localization0.Warning, null));
        };

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