using System;
using System.Collections.Generic;

namespace Achievement
{
	public class AchievementData
	{
		public int Type { get; set; }

		public List<InforAchievement> Achievements { get; set; }

		public int LevelCurrent { get; set; }

		public void LoadProfile(int number)
		{
			for (int i = 0; i < this.Achievements.Count; i++)
			{
				this.Achievements[i].LoadProfile(number, i);
			}
			this.LoadProfile();
			this.ResidueNumberProfile = new IntProfileData("com.sora.metal.squad.Achievement.ResidueNumberProfile." + number, 0);
		}

		public void LoadProfile()
		{
			int num = this.Achievements.Count - 1;
			for (int i = 0; i < this.Achievements.Count; i++)
			{
				EChievement state = this.Achievements[i].State;
				if (state != EChievement.CLEAR)
				{
					num = i;
					break;
				}
			}
			this.LevelCurrent = num;
			this.achievement = this.Achievements[num];
		}

		public int ResidueNumber
		{
			get
			{
				return this.ResidueNumberProfile.Data.Value;
			}
			set
			{
				this.ResidueNumberProfile.setValue(value);
			}
		}

		public InforAchievement achievement;

		private IntProfileData ResidueNumberProfile;
	}
}
