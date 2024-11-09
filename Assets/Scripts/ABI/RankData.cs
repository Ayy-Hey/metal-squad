using System;

namespace ABI
{
	[Serializable]
	public class RankData
	{
		public RankData()
		{
			this.profileRank = new ProfileRank();
			this.bossModeRank = new BossModeRank();
			this.campaignRank = new CampaignRank();
			this.endlessRank = new EndlessRank();
			this.achievementRank = new AchievementRank();
		}

		public string userID;

		public string userName;

		public string countryID;

		public string urlAvatar;

		public int profileRankScore;

		public int bossModeRankScore;

		public int campaignRankScore;

		public int endlessRankScore;

		public int achievementRankScore;

		public ProfileRank profileRank;

		public BossModeRank bossModeRank;

		public CampaignRank campaignRank;

		public EndlessRank endlessRank;

		public AchievementRank achievementRank;
	}
}
