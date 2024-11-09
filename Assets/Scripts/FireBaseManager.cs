using System;
using System.Threading.Tasks;

using UnityEngine;

public class FireBaseManager : MonoBehaviour
{
	public static FireBaseManager Instance
	{
		get
		{
			if (FireBaseManager.instance == null)
			{
				FireBaseManager.instance = UnityEngine.Object.FindObjectOfType<FireBaseManager>();
			}
			return FireBaseManager.instance;
		}
	}

	private void Start()
	{
		
	}

	private void InitializeFirebase()
	{
		
		FirebaseDatabaseManager.Instance.Init();
		this.isInit = true;
	}

	public void OnSetUserProperty()
	{
		if (!this.isInit)
		{
			return;
		}
		this.OnSetProperty("profileLevel", RankManager.Instance.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value).Level);
		this.OnSetProperty("star", ProfileManager._CampaignProfile.GetTotalStar);
		int num = -1;
		int num2 = int.MinValue;
		int num3 = int.MinValue;
		int num4 = -1;
		for (int i = 0; i < ProfileManager.weaponsRifle.Length; i++)
		{
			if (ProfileManager.weaponsRifle[i].GetGunBuy())
			{
				if (ProfileManager.weaponsRifle[i].Power() > num2)
				{
					num2 = ProfileManager.weaponsRifle[i].Power();
					num = i;
				}
				if (ProfileManager.weaponsRifle[i].GetLevelUpgrade() > num3)
				{
					num3 = ProfileManager.weaponsRifle[i].GetLevelUpgrade();
				}
				num4++;
			}
		}
		this.OnSetProperty("primaryWeapon", num);
		this.OnSetProperty("primaryLevel", num3);
		this.OnSetProperty("totalPrimaryWeapon", num4);
		int num5 = -1;
		int num6 = int.MinValue;
		int num7 = int.MinValue;
		int num8 = -1;
		for (int j = 0; j < ProfileManager.weaponsSpecial.Length; j++)
		{
			if (ProfileManager.weaponsSpecial[j].GetGunBuy())
			{
				if (ProfileManager.weaponsSpecial[j].Power() > num6)
				{
					num6 = ProfileManager.weaponsSpecial[j].Power();
					num5 = j;
				}
				if (ProfileManager.weaponsSpecial[j].GetLevelUpgrade() > num7)
				{
					num7 = ProfileManager.weaponsSpecial[j].GetLevelUpgrade();
				}
				num8++;
			}
		}
		this.OnSetProperty("specialWeapon", num5);
		this.OnSetProperty("specialLevel", num7);
		this.OnSetProperty("totalSpecialWeapon", num8);
		int num9 = -1;
		int num10 = int.MinValue;
		int num11 = 0;
		for (int k = 0; k < ProfileManager.melesProfile.Length; k++)
		{
			if (ProfileManager.melesProfile[k].Unlock)
			{
				if (ProfileManager.melesProfile[k].Power() > num10)
				{
					num10 = ProfileManager.melesProfile[k].Power();
					num9 = k;
				}
			}
		}
		this.OnSetProperty("meleeWeapon", num9);
		this.OnSetProperty("meleeLevel", num11);
		int num12 = -1;
		int num13 = int.MinValue;
		int num14 = int.MinValue;
		int num15 = -1;
		for (int l = 0; l < ProfileManager.InforChars.Length; l++)
		{
			if (ProfileManager.InforChars[l].IsUnLocked)
			{
				if (ProfileManager.rambos[l].Power() > num13)
				{
					num13 = ProfileManager.rambos[l].Power();
					num12 = l;
				}
				if (ProfileManager.rambos[l].LevelUpgrade > num14)
				{
					num14 = ProfileManager.rambos[l].LevelUpgrade;
				}
				num15++;
			}
		}
		this.OnSetProperty("rambo", num12);
		this.OnSetProperty("ramboLevel", num14);
		this.OnSetProperty("totalRambo", num15);
		this.OnSetProperty("gold", ProfileManager.userProfile.Coin);
		this.OnSetProperty("gem", ProfileManager.userProfile.Ms);
		this.OnSetProperty("VIP", (int)ProfileManager.inAppProfile.vipProfile.level);
		this.OnSetProperty("playDayTotal", ProfileManager.dailyGiftProfile.CountLoginCurrent());
		this.OnSetProperty("lastNotPlayDayTotal", ProfileManager.dailyGiftProfile.lastNotPlayDayTotal);
		this.OnSetProperty("Control", ProfileManager.settingProfile.TypeControl);
	}

	private void OnSetProperty(string key, object value)
	{
		
	}

	public void LogEvent(string name)
	{
		if (!this.isInit)
		{
			return;
		}
		
	}

	
	

	public void LogEvent(string name, string parameterName, int parameterValue)
	{
		if (!this.isInit)
		{
			return;
		}
		
	}

	public void LogEvent(string name, string parameterName, long parameterValue)
	{
		if (!this.isInit)
		{
			return;
		}
		
	}

	public void LogEvent(string name, string parameterName, double parameterValue)
	{
		if (!this.isInit)
		{
			return;
		}
	
	}

	public void LogEvent(string name, string parameterName, string parameterValue)
	{
		if (!this.isInit)
		{
			return;
		}
		
	}

	public void DelGiftCode(string id)
	{
		string rawJsonValueAsync = null;
		
	}

	private static FireBaseManager instance;

	public static string LEVEL_MARKETING = "MARKETING_CAMPAIGN_NORMAL_LEVEL_";

	public static string VIPGET_MARKETING = "MARKETING_GET_VIP_";

	public static string DAILYGIFT_MARKETING = "MARKETING_DAILYGIFT_";

	public static string CLICK_BUTTON = "Click_Button_";

	public static string Show_Video = "Show_Video";

	public static readonly string PLAY_LEVEL = "Play_Level";

	public static readonly string INAPP = "IAP";

	public static readonly string POPUP = "Popup";

	public bool isInit;
}
