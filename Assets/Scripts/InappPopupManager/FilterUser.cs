using System;

namespace InappPopupManager
{
	public class FilterUser
	{
		public int PlayDays { get; set; }

		public int notPlayDays { get; set; }

		public string Country { get; set; }

		public int ownMS { get; set; }

		public NotOwn notOwn { get; set; }

		public int ownCoin { get; set; }

		public bool OK(int playDaysCurrent, int notPlayDaysCurrent, string CountryCurrent)
		{
			bool flag = this.PlayDays == -1 || playDaysCurrent >= this.PlayDays;
			bool flag2 = this.notPlayDays == -1 || notPlayDaysCurrent >= this.PlayDays;
			bool flag3 = string.IsNullOrEmpty(this.Country) || CountryCurrent.Equals(this.Country);
			bool flag4 = this.ownMS == -1 || ProfileManager.userProfile.Ms >= this.ownMS;
			bool flag5 = this.ownCoin == -1 || ProfileManager.userProfile.Coin >= this.ownCoin;
			bool flag6 = this.notOwn == null || this.notOwn.OK();
			return flag && flag2 && flag3 && flag4 && flag5 && flag6;
		}
	}
}
