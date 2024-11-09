using System;

namespace Database.Backup
{
	[Serializable]
	public class GamePlayData
	{
		public GamePlayData(int hardLevel, int missionStar)
		{
			this.bossModeData = new BossModeData(hardLevel, missionStar);
			this.campaignData = new CampaignData(hardLevel, missionStar);
			this.endlessData = new EndlessData(hardLevel, missionStar);
		}

		public void UpdateLocalProfile()
		{
			this.bossModeData.UpdateLocalProfile();
			this.campaignData.UpdateLocalProfile();
			this.endlessData.UpdateLocalProfile();
		}

		public BossModeData bossModeData;

		public CampaignData campaignData;

		public EndlessData endlessData;

		private int hardLevel;

		private int missionStar;
	}
}
