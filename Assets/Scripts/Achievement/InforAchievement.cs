using System;
using com.dev.util.SecurityHelper;
using DailyQuest;
using UnityEngine;

namespace Achievement
{
	public class InforAchievement
	{
		public string Name { get; set; }

		public int IdName { get; set; }

		public string[] ValueName { get; set; }

		public string Desc { get; set; }

		public int IdDesc { get; set; }

		public string[] ValueDesc { get; set; }

		public int TotalRequirement { get; set; }

		public string ValueIcon { get; set; }

		public string Image { get; set; }

		public int Exp { get; set; }

		public int MS { get; set; }

		public MissionDailyQuest Mission { get; set; }

		public void LoadProfile(int number, int index)
		{
			this.index = index;
			this.TotalProfile = new IntProfileData(string.Concat(new object[]
			{
				"com.sora.metal.squad.Achievement.Number_14",
				number,
				".Index",
				index
			}), 0);
			this.StateProfile = new IntProfileData(string.Concat(new object[]
			{
				"com.sora.metal.squad.Achievement.State.Number_14",
				number,
				".Index",
				index
			}), 0);
			string datapath = string.Concat(new object[]
			{
				"com.sora.metal.squad.Achievement.Mode.Number_14",
				number,
				".Index",
				index
			});
			if (this.Mission.MissionBoss != null && this.Mission.MissionBoss.Map != null)
			{
				this.Mission.MissionBoss.Map.LoadProfile(datapath);
			}
			this.Exp_Secured = new SecuredInt(this.Exp);
			this.MS_Secured = new SecuredInt(this.MS);
		}

		public int Total
		{
			get
			{
				return Mathf.Max(this.TotalProfile.Data.Value, 0);
			}
			set
			{
				this.TotalProfile.setValue(value);
			}
		}

		public EChievement State
		{
			get
			{
				return (EChievement)this.StateProfile.Data.Value;
			}
			set
			{
				this.StateProfile.setValue((int)value);
			}
		}

		private IntProfileData TotalProfile;

		private IntProfileData StateProfile;

		public int index;

		public SecuredInt Exp_Secured;

		public SecuredInt MS_Secured;
	}
}
