using System;

namespace DailyQuest
{
	public class DailyQuestType1
	{
		public int Type { get; set; }

		public string Desc { get; set; }

		public int IdDesc { get; set; }

		public string[] ValueDesc { get; set; }

		public int Gold { get; set; }

		public int Gem { get; set; }

		public int IDPopup { get; set; }

		public MissionDailyQuest Mission { get; set; }

		public void LoadProfile(int numberMission, int day)
		{
			this.stateDailyQuest = new IntProfileData(string.Concat(new object[]
			{
				"com.sora.metal.squad.dailyquest.state.numbermission",
				numberMission,
				".day",
				day
			}), 0);
			this.totalMission = new IntProfileData(string.Concat(new object[]
			{
				"com.sora.metal.squad.dailyquest.total.numbermission",
				numberMission,
				".day",
				day
			}), 0);
			this.modeMission = new BoolProfileData[3];
			for (int i = 0; i < this.modeMission.Length; i++)
			{
				this.modeMission[i] = new BoolProfileData(string.Concat(new object[]
				{
					"com.sora.metal.squad.dailyquest.mode.numbermission",
					numberMission,
					".day",
					day,
					"id.",
					i
				}), false);
			}
			string datapath = string.Concat(new object[]
			{
				"com.sora.metal.squad.dailyquest.missionboss.number",
				numberMission,
				".day",
				day
			});
			if (this.Mission.MissionBoss != null && this.Mission.MissionBoss.Map != null)
			{
				this.Mission.MissionBoss.Map.LoadProfile(datapath);
			}
		}

		public void Reset()
		{
			this.State = 0;
			this.Total = 0;
			this.SetMode(new bool[3]);
			if (this.Mission.MissionBoss != null && this.Mission.MissionBoss.Map != null)
			{
				this.Mission.MissionBoss.Map.Reset();
			}
		}

		public void SetMode(bool[] modes)
		{
			for (int i = 0; i < modes.Length; i++)
			{
				this.modeMission[i].setValue(modes[i]);
			}
		}

		public bool GetMode(int index)
		{
			return this.modeMission[index].Data;
		}

		public int State
		{
			get
			{
				return this.stateDailyQuest.Data.Value;
			}
			set
			{
				this.stateDailyQuest.setValue(value);
			}
		}

		public int Total
		{
			get
			{
				return this.totalMission.Data.Value;
			}
			set
			{
				this.totalMission.setValue(value);
			}
		}

		private IntProfileData stateDailyQuest;

		private IntProfileData totalMission;

		private BoolProfileData[] modeMission;
	}
}
