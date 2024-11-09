using System;

namespace Database.Backup
{
	[Serializable]
	public class UserData
	{
		public UserData()
		{
			this.vipPoint = ProfileManager.inAppProfile.vipProfile.Point;
			this.gold = ProfileManager.userProfile.Coin;
			this.gem = ProfileManager.userProfile.Ms;
		}

		public void UpdateLocalProfile()
		{
			ProfileManager.inAppProfile.vipProfile.Point = this.vipPoint;
			ProfileManager.userProfile.Coin = this.gold;
			ProfileManager.userProfile.Ms = this.gem;
		}

		public int vipPoint;

		public int gold;

		public int gem;
	}
}
