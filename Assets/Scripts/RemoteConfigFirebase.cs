using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteConfigFirebase : MonoBehaviour
{
	public static RemoteConfigFirebase Instance
	{
		get
		{
			if (RemoteConfigFirebase.instance == null)
			{
				RemoteConfigFirebase.instance = UnityEngine.Object.FindObjectOfType<RemoteConfigFirebase>();
			}
			return RemoteConfigFirebase.instance;
		}
	}

	public void OnInit()
	{
		this.defaults_notFetch = new Dictionary<string, object>();
		this.InitCampaign();
		this.isInit = true;
		
	}

	private void InitCampaign()
	{
		StringBuilder[] array = new StringBuilder[3];
		this.powerCampaign = new RemoteConfigFirebase.DataPowerDefault[3];
		for (int i = 0; i < this.powerCampaign.Length; i++)
		{
			string text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathPowerCampaign : SPaths.PathPowerCampaign_Decrypt) + "Campaign" + i).text;
			string text2 = ProfileManager.DataEncryption.decrypt2(text);
			this.powerCampaign[i] = new RemoteConfigFirebase.DataPowerDefault();
			this.powerCampaign[i].value_default = ((!SplashScreen._isLoadResourceDecrypt) ? text2 : text).Split(new string[]
			{
				"\n"
			}, StringSplitOptions.None);
			array[i] = new StringBuilder();
		}
		for (int j = 0; j < this.powerCampaign.Length; j++)
		{
			for (int k = 0; k < 60; k++)
			{
				string[] array2 = this.powerCampaign[j].value_default[k].Split(new char[]
				{
					' '
				});
				array[j].Append(array2[1].Trim());
				if (j < 59)
				{
					array[j].Append("@");
				}
			}
			this.defaults_notFetch.Add(RemoteConfigFirebase.POWER_CAMPAIGN + j, array[j].ToString());
		}
	}

	private int PowerCampaignFromResource(GameMode.Mode mode, int level)
	{
		int result = 1000;
		try
		{
			string[] value_default = this.powerCampaign[(int)mode].value_default;
			result = int.Parse(value_default[level].Split(new char[]
			{
				' '
			})[1]);
		}
		catch
		{
		}
		return result;
	}

	

	private void FetchComplete(Task fetchTask)
	{
		try
		{
			if (fetchTask.IsCanceled)
			{
				UnityEngine.Debug.Log("Fetch canceled.");
			}
			else if (fetchTask.IsFaulted)
			{
				UnityEngine.Debug.Log("Fetch encountered an error.");
			}
			else if (fetchTask.IsCompleted)
			{
				UnityEngine.Debug.Log("Fetch completed successfully!");
			}
			
		}
		catch
		{
		}
		this.isFetchDataAsync = true;
	}

	public int PowerCampaignValue(GameMode.Mode mode, int level)
	{
		return this.PowerCampaignFromResource(mode, level);
	}

	public double GetDoubleValue(string key, double valueDefault)
	{
		double result = 0.0;
		
		return result;
	}

	public long GetLongValue(string key, long valueDefault)
	{
		long result = valueDefault;
		
		return result;
	}

	public string GetStringValue(string key, string valueDefault)
	{
		string result = valueDefault;
		
		return result;
	}

	public bool GetBooleanValue(string key, bool valueDefault)
	{
		bool result = valueDefault;
		
		return result;
	}

	private static RemoteConfigFirebase instance;

	public static readonly string POWER_CAMPAIGN = "power_campaign_";

	public static readonly string SALE_TODAY = "sale_today";

	public static readonly string MULTIPLAYER_RUNNING = "multiplayer_running";

	public static readonly string MULTIPLAYER_FAKEPLAYER = "multiplayer_create_fakeplayer";

	public static readonly string RATE_SPAM_ADS = "rate_spam_ads";

	public static readonly string TIME_SHOW_INTERS = "time_show_inters";

	public static readonly string GEM_RANKUP = "gem_rankup";

	public static readonly string SCALE_PRICE_UPGRADE = "scale_price_upgrade";

	public static readonly string STARTER_PACK_TEST = "starter_pack_test";

	public static readonly string DROP_GOLD_MISSION = "drop_gold_mission";

	private RemoteConfigFirebase.DataPowerDefault[] powerCampaign;

	private Dictionary<string, object> defaults_notFetch;

	private bool isInit;

	public bool isFetchDataAsync;

	public class DataPowerDefault
	{
		public string[] value_default;
	}
}
