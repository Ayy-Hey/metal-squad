using System;

namespace ABI
{
	public class PlayFabStatConstants
	{
		public static string[] leaderboardNames = new string[]
		{
			"ProfileRank",
			"CampaignRank",
			"BossModeRank",
			"EndlessRank",
			"AchievementRank"
		};

		public static string[] permissions = new string[]
		{
			"public_profile",
			"email",
			"user_friends"
		};

		public const string firebaseUrl = "https://test-7cfbf.firebaseio.com/";
	}
}
