using System;
using System.Collections.Generic;
using System.Text;
using ABI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class UserLB
{
	public UserLB()
	{
		this._character = new StringBuilder();
		this._Weapon1 = new StringBuilder();
		this._Weapon2 = new StringBuilder();
		this._WolrdMap = new StringBuilder();
		this.data = new Dictionary<string, string>();
	}

	public void UpdateData(MonoBehaviour _MonoBehaviour, Action callbackDone)
	{
		this.callbackDone = callbackDone;
		if (!PlayfabManager.Instance.isLoggedIn)
		{
			return;
		}
		this._MonoBehaviour = _MonoBehaviour;
		PlayFabClientAPI.GetUserData(new GetUserDataRequest
		{
			PlayFabId = this.ID
		}, delegate(GetUserDataResult result)
		{
			this.CoppyDataPlayFab(result.Data);
			this.LoadSprite();
		}, delegate(PlayFabError error)
		{
			this.CoppyDataProfile();
			if (callbackDone != null)
			{
				this.callbackDone();
			}
		}, null, null);
	}

	private void LoadSprite()
	{
		string url = string.Format("http://www.countryflags.io/{0}/shiny/64.png", this.entry.Profile.Locations[0].CountryCode);
		CountryCode countryCode = (CountryCode)this.cachingFlag;
		CountryCode? countryCode2 = this.entry.Profile.Locations[0].CountryCode;
		if (countryCode == countryCode2.Value && this.spriteFlag != null)
		{
			if (this.callbackDone != null)
			{
				this.callbackDone();
			}
		}
		else
		{
			CountryCode? countryCode3 = this.entry.Profile.Locations[0].CountryCode;
			this.cachingFlag = (int)countryCode3.Value;
			this._MonoBehaviour.StartCoroutine(MonoSingleton<FBAndPlayfabMgr>.Instance.GetProfilePic(url, delegate(Texture2D tex)
			{
				this.spriteFlag = Sprite.Create(tex, new Rect(0f, 0f, 64f, 64f), Vector2.zero);
				if (this.callbackDone != null)
				{
					this.callbackDone();
				}
			}));
		}
		string avatarUrl = this.entry.Profile.AvatarUrl;
		if (!string.IsNullOrEmpty(this.cachingAvatar) && this.cachingAvatar.Equals(avatarUrl) && this.spriteAvatar != null)
		{
			return;
		}
		this.cachingAvatar = avatarUrl;
		this._MonoBehaviour.StartCoroutine(MonoSingleton<FBAndPlayfabMgr>.Instance.GetProfilePic(this.cachingAvatar, delegate(Texture2D tex)
		{
			try
			{
				this.spriteAvatar = Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), Vector2.zero);
			}
			catch
			{
			}
		}));
	}

	public void CoppyDataPlayFab(Dictionary<string, UserDataRecord> Data)
	{
		this.data.Clear();
		for (int i = 0; i < PlayfabManager.ListKey.Length; i++)
		{
			string value = string.Empty;
			if (Data.ContainsKey(PlayfabManager.ListKey[i]))
			{
				value = Data[PlayfabManager.ListKey[i]].Value;
			}
			this.data.Add(PlayfabManager.ListKey[i], value);
		}
	}

	public void CoppyDataProfile()
	{
		this._character.Length = 0;
		for (int i = 0; i < ProfileManager.InforChars.Length; i++)
		{
			this._character.Append((!ProfileManager.InforChars[i].IsUnLocked) ? "-1" : ProfileManager.rambos[i].RankUpped.ToString());
			if (i < ProfileManager.InforChars.Length - 1)
			{
				this._character.Append("|");
			}
		}
		this._Weapon1.Length = 0;
		for (int j = 0; j < ProfileManager.weaponsRifle.Length; j++)
		{
			this._Weapon1.Append((!ProfileManager.weaponsRifle[j].GetGunBuy()) ? "-1" : ProfileManager.weaponsRifle[j].GetRankUpped().ToString());
			if (j < ProfileManager.weaponsRifle.Length - 1)
			{
				this._Weapon1.Append("|");
			}
		}
		this._Weapon2.Length = 0;
		for (int k = 0; k < ProfileManager.weaponsSpecial.Length; k++)
		{
			this._Weapon2.Append((!ProfileManager.weaponsSpecial[k].GetGunBuy()) ? "-1" : ProfileManager.weaponsSpecial[k].GetRankUpped().ToString());
			if (k < ProfileManager.weaponsSpecial.Length - 1)
			{
				this._Weapon2.Append("|");
			}
		}
		this._WolrdMap.Length = 0;
		for (int l = 0; l < 5; l++)
		{
			this._WolrdMap.Append(ProfileManager._CampaignProfile.GetTotalStarInMap(l));
			if (l < ProfileManager.worldMapProfile.Length - 1)
			{
				this._WolrdMap.Append("|");
			}
		}
		this.data.Clear();
		this.data.Add(PlayfabManager.ListKey[0], PowerManager.Instance.TotalPower().ToString());
		this.data.Add(PlayfabManager.ListKey[1], VipManager.Instance.Exp.ToString());
		this.data.Add(PlayfabManager.ListKey[2], this._character.ToString());
		this.data.Add(PlayfabManager.ListKey[3], this._Weapon1.ToString());
		this.data.Add(PlayfabManager.ListKey[4], this._Weapon2.ToString());
		this.data.Add(PlayfabManager.ListKey[5], this._WolrdMap.ToString());
		this.data.Add(PlayfabManager.ListKey[6], ProfileManager.LevelRankClaimedProfile.Data.Value.ToString());
		this.data.Add(PlayfabManager.ListKey[7], ProfileManager.ExpRankProfile.Data.Value.ToString());
	}

	public Dictionary<string, string> Data
	{
		get
		{
			return this.data;
		}
	}

	public string GetValue(PlayfabManager.EKey key)
	{
		string result = "0";
		try
		{
			if (this.data.ContainsKey(PlayfabManager.ListKey[(int)key]))
			{
				result = this.data[PlayfabManager.ListKey[(int)key]];
			}
		}
		catch (Exception ex)
		{
		}
		return result;
	}

	public string UserName
	{
		get
		{
			string result = string.Empty;
			try
			{
				result = this.entry.DisplayName;
			}
			catch
			{
				result = ProfileManager.InforChars[ProfileManager.settingProfile.IDChar].Name;
				if (this.isPlayer && !string.IsNullOrEmpty(ProfileManager.pvpProfile.UserName))
				{
					result = ProfileManager.pvpProfile.UserName;
				}
			}
			return result;
		}
	}

	public int TotalStar
	{
		get
		{
			int num = 0;
			try
			{
				num = this.entry.StatValue;
			}
			catch
			{
				num = ProfileManager._CampaignProfile.GetTotalStar;
			}
			if (this.isPlayer)
			{
				num = Mathf.Max(num, ProfileManager._CampaignProfile.GetTotalStar);
			}
			return num;
		}
	}

	public int TotalExpRank
	{
		get
		{
			int result = 0;
			try
			{
				result = int.Parse(this.GetValue(PlayfabManager.EKey.Rank));
			}
			catch
			{
				result = ProfileManager.ExpRankProfile.Data.Value;
			}
			if (this.isPlayer)
			{
				result = ProfileManager.ExpRankProfile.Data.Value;
			}
			return result;
		}
	}

	public string[] GetCharacters
	{
		get
		{
			string[] result = new string[0];
			if (this.data.ContainsKey(PlayfabManager.ListKey[2]))
			{
				result = this.data[PlayfabManager.ListKey[2]].Split(new char[]
				{
					'|'
				});
			}
			return result;
		}
	}

	public string[] GetWeapon1
	{
		get
		{
			string[] result = new string[0];
			if (this.data.ContainsKey(PlayfabManager.ListKey[3]))
			{
				result = this.data[PlayfabManager.ListKey[3]].Split(new char[]
				{
					'|'
				});
			}
			return result;
		}
	}

	public string[] GetWeapon2
	{
		get
		{
			string[] result = new string[0];
			if (this.data.ContainsKey(PlayfabManager.ListKey[4]))
			{
				result = this.data[PlayfabManager.ListKey[4]].Split(new char[]
				{
					'|'
				});
			}
			return result;
		}
	}

	public string[] GetWorldMap
	{
		get
		{
			string[] result = new string[0];
			if (this.data.ContainsKey(PlayfabManager.ListKey[5]))
			{
				result = this.data[PlayfabManager.ListKey[5]].Split(new char[]
				{
					'|'
				});
			}
			return result;
		}
	}

	private StringBuilder _character;

	private StringBuilder _Weapon1;

	private StringBuilder _Weapon2;

	private StringBuilder _WolrdMap;

	private Dictionary<string, string> data;

	public PlayerLeaderboardEntry entry;

	public string ID;

	public bool isPlayer;

	public Sprite spriteFlag;

	public Sprite spriteAvatar;

	private int cachingFlag = -1;

	private string cachingAvatar;

	private MonoBehaviour _MonoBehaviour;

	private Action callbackDone;
}
