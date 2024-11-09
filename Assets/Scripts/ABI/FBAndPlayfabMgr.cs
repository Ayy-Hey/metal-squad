using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Networking;

namespace ABI
{
	public class FBAndPlayfabMgr : MonoSingleton<FBAndPlayfabMgr>
	{
		public string PlayFabUserId { get; private set; }

		public bool IsLoggedOnPlayFab { get; private set; }

		public string UserCountryCode { get; private set; }

		public Sprite UserCountryFlagSprite { get; private set; }

		public bool IsLoggedOnFacebook { get; private set; }

		public string FacebookUserId { get; private set; }

		public string FacebookUserName { get; private set; }

		public string currentAvatarUrl { get; private set; }

		public Sprite FacebookUserPictureSprite { get; private set; }

		protected override void Awake()
		{
			base.Awake();
		
			
		}

		

		private void SetLoggedInfo()
		{
			this.IsLoggedOnFacebook = true;
			
			FirebaseDatabaseManager.Instance.isLoginFB = true;
		}

	
		private void GetRequiredDataFB()
		{
			
		}

		private void UpdatePlayFabData()
		{
			if (!this.IsLoggedOnPlayFab)
			{
				UnityEngine.Debug.Log("FacebookAndPlayFabManager.UpdatePlayFabDisplayUserName => Not logged on PlayFab!");
				return;
			}
			UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest
			{
				DisplayName = this.FacebookUserName
			};
			PlayFabClientAPI.UpdateUserTitleDisplayName(request, null, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
			UnityEngine.Debug.Log(this.currentAvatarUrl);
			UpdateAvatarUrlRequest request2 = new UpdateAvatarUrlRequest
			{
				ImageUrl = this.currentAvatarUrl
			};
			PlayFabClientAPI.UpdateAvatarUrl(request2, null, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
		}

		

		public IEnumerator GetProfilePic(string url, Action<Texture2D> callback)
		{
			if (string.IsNullOrEmpty(url))
			{
				yield break;
			}
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
			yield return www.SendWebRequest();
			if (www.isNetworkError)
			{
				UnityEngine.Debug.Log(www.error);
			}
			else if (www.downloadHandler.isDone)
			{
				callback(((DownloadHandlerTexture)www.downloadHandler).texture);
			}
			yield break;
		}

		
		
		public void LogOutFacebook()
		{
			
		}

		private void LogOnPlayFab()
		{
			
		}

		private void PlayFabLoginSuccessCallback(LoginResult result)
		{
			
		}

		private void PlayFabErrorCallback(PlayFabError error)
		{
			string message = string.Format("FacebookAndPlayFabManager.PlayFabErrorCallback => {0}", (!string.IsNullOrEmpty(error.ErrorMessage)) ? error.ErrorMessage : "null");
			UnityEngine.Debug.LogError(message);
		}

		public void UpdatePlayerLeaderboard(string statisticName, int value, Action<UpdatePlayerStatisticsResult> successCallback = null)
		{
			if (!this.IsLoggedOnPlayFab)
			{
				UnityEngine.Debug.Log("FacebookAndPlayFabManager.UpdateStat => Not logged on PlayFab!");
				return;
			}
			UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest
			{
				Statistics = new List<StatisticUpdate>
				{
					new StatisticUpdate
					{
						StatisticName = statisticName,
						Value = value
					}
				}
			};
			successCallback = (Action<UpdatePlayerStatisticsResult>)Delegate.Combine(successCallback, new Action<UpdatePlayerStatisticsResult>(delegate(UpdatePlayerStatisticsResult result)
			{
				UnityEngine.Debug.Log("FacebookAndPlayFabManager.UpdateStat => Success!");
			}));
			PlayFabClientAPI.UpdatePlayerStatistics(request, successCallback, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
		}

		public void UpdatePlayerLeaderboard(LeaderboardType leaderboardType, int value, Action<UpdatePlayerStatisticsResult> successCallback = null)
		{
			this.UpdatePlayerLeaderboard(PlayFabStatConstants.leaderboardNames[(int)leaderboardType], value, successCallback);
		}

		public void GetLeaderboardPlayFab(string statisticName, bool friendsOnly, int maxResultsCount, Action<GetLeaderboardResult> successCallback, int startPosition = 0)
		{
			if (!this.IsLoggedOnPlayFab)
			{
				UnityEngine.Debug.Log("FacebookAndPlayFabManager.GetFriendLeaderboard => Not logged on PlayFab!");
				return;
			}
			PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints();
			playerProfileViewConstraints.ShowAvatarUrl = true;
			playerProfileViewConstraints.ShowDisplayName = true;
			playerProfileViewConstraints.ShowLocations = true;
			if (friendsOnly)
			{
				GetFriendLeaderboardRequest request = new GetFriendLeaderboardRequest
				{
					StatisticName = statisticName,
					MaxResultsCount = new int?(maxResultsCount),
					IncludeFacebookFriends = new bool?(true),
					StartPosition = startPosition,
					ProfileConstraints = playerProfileViewConstraints
				};
				PlayFabClientAPI.GetFriendLeaderboard(request, successCallback, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
			}
			else
			{
				GetLeaderboardRequest request2 = new GetLeaderboardRequest
				{
					StatisticName = statisticName,
					MaxResultsCount = new int?(maxResultsCount),
					StartPosition = startPosition,
					ProfileConstraints = playerProfileViewConstraints
				};
				PlayFabClientAPI.GetLeaderboard(request2, successCallback, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
			}
		}

		public void GetPlayerRankPlayFab(string statisticName, bool friendsOnly, Action<GetLeaderboardAroundPlayerResult> successCallback, Action<GetFriendLeaderboardAroundPlayerResult> successCallbackFriend)
		{
			if (!this.IsLoggedOnPlayFab)
			{
				UnityEngine.Debug.Log("FacebookAndPlayFabManager.GetFriendLeaderboard => Not logged on PlayFab!");
				return;
			}
			PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints();
			playerProfileViewConstraints.ShowAvatarUrl = true;
			playerProfileViewConstraints.ShowDisplayName = true;
			playerProfileViewConstraints.ShowLocations = true;
			if (friendsOnly)
			{
				GetFriendLeaderboardAroundPlayerRequest request = new GetFriendLeaderboardAroundPlayerRequest
				{
					MaxResultsCount = new int?(1),
					StatisticName = statisticName,
					ProfileConstraints = playerProfileViewConstraints
				};
				PlayFabClientAPI.GetFriendLeaderboardAroundPlayer(request, successCallbackFriend, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
			}
			else
			{
				GetLeaderboardAroundPlayerRequest request2 = new GetLeaderboardAroundPlayerRequest
				{
					MaxResultsCount = new int?(1),
					StatisticName = statisticName,
					ProfileConstraints = playerProfileViewConstraints
				};
				PlayFabClientAPI.GetLeaderboardAroundPlayer(request2, successCallback, new Action<PlayFabError>(this.PlayFabErrorCallback), null, null);
			}
		}

		
		public void PostScoreInfo(Action<UpdatePlayerStatisticsResult> successCallback)
		{
			this.PostScoreProfiler(successCallback);
		}

		public void PostScoreProfiler(Action<UpdatePlayerStatisticsResult> successCallback)
		{
			int num = RankManager.Instance.GetTotalExp();
			num = 10000;
			UnityEngine.Debug.Log("post-rank" + num);
			this.UpdatePlayerLeaderboard(PlayFabStatConstants.leaderboardNames[0], num, null);
		}

		public void PostScoreCampaign(Action<UpdatePlayerStatisticsResult> successCallback)
		{
		}

		public void PostScoreBossMode(Action<UpdatePlayerStatisticsResult> successCallback)
		{
			int num = 100;
			int num2 = 5;
			int winCount = ProfileManager.bossModeProfile.GetWinCount(GameMode.Mode.NORMAL);
			int playCount = ProfileManager.bossModeProfile.GetPlayCount(GameMode.Mode.NORMAL);
			int winCount2 = ProfileManager.bossModeProfile.GetWinCount(GameMode.Mode.HARD);
			int playCount2 = ProfileManager.bossModeProfile.GetPlayCount(GameMode.Mode.HARD);
			int winCount3 = ProfileManager.bossModeProfile.GetWinCount(GameMode.Mode.SUPER_HARD);
			int playCount3 = ProfileManager.bossModeProfile.GetPlayCount(GameMode.Mode.SUPER_HARD);
			float num3 = (playCount > 0) ? (((float)num * Mathf.Pow((float)winCount, 2f) - (float)num2 * Mathf.Pow((float)winCount, 2f) + (float)(num2 * winCount * playCount)) / (float)playCount) : 0f;
			float num4 = (playCount2 > 0) ? (((float)(3 * num) * Mathf.Pow((float)winCount2, 2f) - (float)(2 * num2) * Mathf.Pow((float)winCount2, 2f) + (float)(2 * num2 * winCount2 * playCount2)) / (float)playCount2) : 0f;
			float num5 = (playCount3 > 0) ? (((float)(9 * num) * Mathf.Pow((float)winCount3, 2f) - (float)(3 * num2) * Mathf.Pow((float)winCount3, 2f) + (float)(3 * num2 * winCount3 * playCount3)) / (float)playCount3) : 0f;
			float num6 = num3 + num4 + num5;
			UnityEngine.Debug.Log((int)num6 + "----------------------bosss mode score");
			this.UpdatePlayerLeaderboard(PlayFabStatConstants.leaderboardNames[2], (int)num6, null);
		}

		public void PostScoreEndless(Action<UpdatePlayerStatisticsResult> successCallback)
		{
		}

		public void PostScoreEchievement(Action<UpdatePlayerStatisticsResult> successCallback)
		{
			int achivementCompleted = AchievementManager.Instance.GetAchivementCompleted();
			this.UpdatePlayerLeaderboard(PlayFabStatConstants.leaderboardNames[4], achivementCompleted, successCallback);
		}

		private const int PictureWidth = 140;

		private const int PictureHeight = 140;

		private string getUserPicString = "me?fields=id,name,picture.height(256)";
	}
}
