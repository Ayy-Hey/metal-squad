using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class LeaderboardRank
{
	public LeaderboardRank(MonoBehaviour _MonoBehaviour, string StatisticName)
	{
		this._MonoBehaviour = _MonoBehaviour;
		this.StatisticName = StatisticName;
		this.MyProfile = new UserLB();
		this.MyProfile.isPlayer = true;
		this.MyProfile.CoppyDataProfile();
		this.ListTopPlayer = new List<UserLB>();
		this.ListTopFriends = new List<UserLB>();
	}

	public void GetTopPlayer(Action<bool> OnGlobalCompleted = null)
	{
		this.OnGlobalCompleted = OnGlobalCompleted;
		if (!PlayfabManager.Instance.isLoggedIn)
		{
			return;
		}
		PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints();
		playerProfileViewConstraints.ShowAvatarUrl = true;
		playerProfileViewConstraints.ShowDisplayName = true;
		playerProfileViewConstraints.ShowLocations = true;
		GetLeaderboardRequest request = new GetLeaderboardRequest
		{
			StatisticName = this.StatisticName,
			MaxResultsCount = new int?(100),
			StartPosition = 0,
			ProfileConstraints = playerProfileViewConstraints
		};
		PlayFabClientAPI.GetLeaderboard(request, new Action<GetLeaderboardResult>(this.GetLeaderboardCallback), new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
	}

	private void GetTopFriends()
	{
		if (!PlayfabManager.Instance.isLoggedIn)
		{
			return;
		}
		PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints();
		playerProfileViewConstraints.ShowAvatarUrl = true;
		playerProfileViewConstraints.ShowDisplayName = true;
		playerProfileViewConstraints.ShowLocations = true;
		GetFriendLeaderboardRequest request = new GetFriendLeaderboardRequest
		{
			MaxResultsCount = new int?(100),
			StatisticName = this.StatisticName,
			IncludeFacebookFriends = new bool?(true),
			StartPosition = 0,
			ProfileConstraints = playerProfileViewConstraints
		};
		PlayFabClientAPI.GetFriendLeaderboard(request, delegate(GetLeaderboardResult success)
		{
			this.CountUserLoadedFriends = 0;
			for (int i = 0; i < success.Leaderboard.Count; i++)
			{
				PlayerLeaderboardEntry playerLeaderboardEntry = success.Leaderboard[i];
				UserLB userLB = new UserLB();
				userLB.ID = playerLeaderboardEntry.PlayFabId;
				userLB.entry = playerLeaderboardEntry;
				if (i < this.ListTopFriends.Count)
				{
					this.ListTopFriends[i] = userLB;
				}
				else
				{
					this.ListTopFriends.Add(userLB);
				}
				this.ListTopFriends[i].UpdateData(this._MonoBehaviour, delegate
				{
					this.CountUserLoadedFriends++;
				});
			}
		}, delegate(PlayFabError error)
		{
		}, null, null);
	}

	public void GetMyProfile(Action<bool> OnCompleted)
	{
		if (!PlayfabManager.Instance.isLoggedIn || !FirebaseDatabaseManager.Instance.isLoginFB)
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
			return;
		}
		PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints();
		playerProfileViewConstraints.ShowAvatarUrl = true;
		playerProfileViewConstraints.ShowDisplayName = true;
		playerProfileViewConstraints.ShowLocations = true;
		GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest
		{
			MaxResultsCount = new int?(1),
			StatisticName = this.StatisticName,
			ProfileConstraints = playerProfileViewConstraints
		};
		PlayFabClientAPI.GetLeaderboardAroundPlayer(request, delegate(GetLeaderboardAroundPlayerResult result)
		{
			foreach (PlayerLeaderboardEntry playerLeaderboardEntry in result.Leaderboard)
			{
				if (playerLeaderboardEntry.PlayFabId.Equals(FirebaseDatabaseManager.Instance.IDPlayFab))
				{
					PvpProfile pvpProfile = ProfileManager.pvpProfile;
					CountryCode? countryCode = playerLeaderboardEntry.Profile.Locations[0].CountryCode;
					pvpProfile.CountryCode = (int)countryCode.Value;
					ProfileManager.pvpProfile.AvatarUrl = playerLeaderboardEntry.Profile.AvatarUrl;
					this.MyProfile.entry = playerLeaderboardEntry;
					this.MyProfile.ID = FirebaseDatabaseManager.Instance.IDPlayFab;
					if (OnCompleted != null)
					{
						OnCompleted(true);
					}
					this.MyProfile.UpdateData(this._MonoBehaviour, delegate
					{
						if (OnCompleted != null)
						{
							OnCompleted(true);
						}
					});
				}
			}
		}, delegate(PlayFabError error)
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
		}, null, null);
	}

	public void PostMyScore(int score, Action<bool> OnCompleted)
	{
		if (!PlayfabManager.Instance.isLoggedIn || !FirebaseDatabaseManager.Instance.isLoginFB)
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
			return;
		}
		UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate
				{
					StatisticName = this.StatisticName,
					Value = score
				}
			}
		};
		PlayFabClientAPI.UpdatePlayerStatistics(request, delegate(UpdatePlayerStatisticsResult result)
		{
			this.PostMyData(OnCompleted);
		}, delegate(PlayFabError error)
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
		}, null, null);
	}

	private void PostMyData(Action<bool> OnCompleted)
	{
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
		{
			Data = this.MyProfile.Data,
			Permission = new UserDataPermission?(UserDataPermission.Public)
		}, delegate(UpdateUserDataResult result)
		{
			if (OnCompleted != null)
			{
				OnCompleted(true);
			}
		}, delegate(PlayFabError error)
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
		}, null, null);
	}

	public void GetUserProfile(UserLB user, Action<bool> OnCompleted)
	{
		if (!PlayfabManager.Instance.isLoggedIn)
		{
			return;
		}
		PlayFabClientAPI.GetUserData(new GetUserDataRequest
		{
			PlayFabId = user.ID
		}, delegate(GetUserDataResult result)
		{
			user.CoppyDataPlayFab(result.Data);
			user.UpdateData(this._MonoBehaviour, delegate
			{
				if (OnCompleted != null)
				{
					OnCompleted(true);
				}
			});
		}, delegate(PlayFabError error)
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
		}, null, null);
	}

	public void GetLeaderboardCallback(GetLeaderboardResult result)
	{
		this.CountUserLoaded = 0;
		for (int i = 0; i < result.Leaderboard.Count; i++)
		{
			PlayerLeaderboardEntry playerLeaderboardEntry = result.Leaderboard[i];
			UserLB userLB = new UserLB();
			userLB.ID = playerLeaderboardEntry.PlayFabId;
			userLB.entry = playerLeaderboardEntry;
			if (i < this.ListTopPlayer.Count)
			{
				this.ListTopPlayer[i] = userLB;
			}
			else
			{
				this.ListTopPlayer.Add(userLB);
			}
			this.ListTopPlayer[i].UpdateData(this._MonoBehaviour, delegate
			{
				this.CountUserLoaded++;
				if (this.CountUserLoaded >= this.ListTopPlayer.Count)
				{
					if (this.OnGlobalCompleted != null)
					{
						this.OnGlobalCompleted(true);
					}
					if (this.OnGlobalEnded != null)
					{
						this.OnGlobalEnded();
					}
					this.GetTopFriends();
				}
			});
		}
	}

	private void PlayFabErrorCallback(PlayFabError error)
	{
		string str = string.Format("FacebookAndPlayFabManager.PlayFabErrorCallback => {0}", (!string.IsNullOrEmpty(error.ErrorMessage)) ? error.ErrorMessage : "null");
		UnityEngine.Debug.Log("PlayFabErrorCallback_" + str);
	}

	private string StatisticName = "LeaderboardStar";

	private MonoBehaviour _MonoBehaviour;

	public bool isCachedFriend;

	public List<UserLB> ListTopPlayer;

	public List<UserLB> ListTopFriends;

	public UserLB MyProfile;

	private Coroutine _CoroutineFriends;

	private int CountUserLoaded;

	private int CountUserLoadedFriends;

	private Action<bool> OnGlobalCompleted;

	public Action OnGlobalEnded;
}
