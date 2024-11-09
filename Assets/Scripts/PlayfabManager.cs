using System;
using System.Collections;
using ABI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabManager : MonoBehaviour
{
	public static PlayfabManager Instance
	{
		get
		{
			if (PlayfabManager.instance == null)
			{
				PlayfabManager.instance = UnityEngine.Object.FindObjectOfType<PlayfabManager>();
			}
			return PlayfabManager.instance;
		}
	}

	private IEnumerator Start()
	{
		UnityEngine.Debug.Log("Start Playfab!");
		yield return new WaitUntil(() => ProfileManager.isInit);
		this.leaderBoard = new LeaderboardRank[2];
		this.leaderBoard[0] = new LeaderboardRank(this, "LeaderboardStar");
		this.leaderBoard[1] = new LeaderboardRank(this, "LeaderboardPvp");
		if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
		{
			PlayFabSettings.TitleId = "ED2B";
		}
		float timeCurrnet = Time.timeSinceLevelLoad;
		this.LoginToPlayfab(null);
		yield break;
	}

	public void LoginToPlayfab(Action callbackCustomSuccess = null)
	{
		this.callbackCustomSuccess = callbackCustomSuccess;
		
	}

	private void OnLoginSuccess(LoginResult result)
	{
		this.isLoggedIn = true;
		FirebaseDatabaseManager.Instance.IDPlayFab = result.PlayFabId;
		if (this.callbackCustomSuccess != null)
		{
			this.callbackCustomSuccess();
		}
		this.GetLeaderBoard(false).GetTopPlayer(null);
		base.StartCoroutine(this.GetProfileUser());
	}

	private IEnumerator GetProfileUser()
	{
		yield return new WaitUntil(() => ProfileManager.isInit);
		this.GetTimeOnServer();
		PlayerProfileViewConstraints ProfileConstraints = new PlayerProfileViewConstraints();
		ProfileConstraints.ShowLocations = true;
		PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
		{
			ProfileConstraints = ProfileConstraints
		}, delegate(GetPlayerProfileResult result)
		{
			PvpProfile pvpProfile = ProfileManager.pvpProfile;
			CountryCode? countryCode = result.PlayerProfile.Locations[0].CountryCode;
			pvpProfile.CountryCode = (int)countryCode.Value;
			if (PlayerPrefs.GetInt(PopupManager.Instance.SaveLanguage, -1) == -1)
			{
				CountryCode? countryCode2 = result.PlayerProfile.Locations[0].CountryCode;
				if (countryCode2 != null)
				{
					CountryCode value = countryCode2.Value;
					if (value != CountryCode.ID)
					{
						if (value != CountryCode.PT)
						{
							if (value != CountryCode.RU)
							{
								if (value != CountryCode.ES)
								{
									if (value != CountryCode.TH)
									{
										if (value == CountryCode.VN)
										{
											PopupManager.language = Language.TiengViet;
											PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, 1);
										}
									}
									else
									{
										PopupManager.language = Language.ThaiLan;
										PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, 5);
									}
								}
								else
								{
									PopupManager.language = Language.TayBanNha;
									PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, 4);
								}
							}
							else
							{
								PopupManager.language = Language.Nga;
								PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, 6);
							}
						}
						else
						{
							PopupManager.language = Language.BoDaoNha;
							PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, 3);
						}
					}
					else
					{
						PopupManager.language = Language.Indonesia;
						PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, 2);
					}
				}
			}
		}, delegate(PlayFabError error)
		{
		}, null, null);
		yield break;
	}

	private void GetTimeOnServer()
	{
		GetTimeRequest request = new GetTimeRequest();
		PlayFabClientAPI.GetTime(request, delegate(GetTimeResult result)
		{
			if (result != null)
			{
				this.isTimeReady = true;
				ProfileManager.dailyGiftProfile.CheckDay(result.Time);
			}
		}, delegate(PlayFabError error)
		{
			this.isTimeReady = true;
			ProfileManager.dailyGiftProfile.CheckDay(DateTime.Now);
		}, null, null);
	}

	private void OnLoginError(PlayFabError error)
	{
	}

	private IEnumerator WaitForPlayFabLoginCoroutine()
	{
		yield return new WaitUntil(() => MonoSingleton<FBAndPlayfabMgr>.Instance.IsLoggedOnPlayFab);
		FirebaseDatabaseManager.Instance.IDPlayFab = MonoSingleton<FBAndPlayfabMgr>.Instance.PlayFabUserId;
		this.isLoggedIn = true;
		this.GetLeaderBoard(false).GetTopPlayer(null);
		try
		{
			this.GetLeaderBoard(false).GetMyProfile(null);
		}
		catch (Exception ex)
		{
		}
		base.StartCoroutine(this.GetProfileUser());
		yield break;
	}

	public LeaderboardRank GetLeaderBoard(bool isPvp)
	{
		return this.leaderBoard[(!isPvp) ? 0 : 1];
	}

	public void OnStop()
	{
		base.StopAllCoroutines();
	}

	public static readonly string[] ListKey = new string[]
	{
		"Power",
		"Vip",
		"Character",
		"Weapon1",
		"Weapon2",
		"WorldMap",
		"LevelRank",
		"Rank"
	};

	private static PlayfabManager instance = null;

	public const int MAX_RESULT = 100;

	public bool isLoggedIn;

	public bool isTimeReady;

	public LeaderboardRank[] leaderBoard;

	private Action callbackCustomSuccess;

	public enum TypeLeaderBoard
	{
		RANK,
		PVP
	}

	public enum EKey
	{
		Power,
		Vip,
		Character,
		Weapon1,
		Weapon2,
		WorldMap,
		LevelRank,
		Rank
	}
}
