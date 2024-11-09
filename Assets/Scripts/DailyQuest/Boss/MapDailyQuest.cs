using System;

namespace DailyQuest.Boss
{
	public class MapDailyQuest
	{
		public int ID { get; set; }

		public bool FlagAny { get; set; }

		public bool[] IDLevel { get; set; }

		public void LoadProfile(string datapath)
		{
			this.IDLevelProfile = new BoolProfileData[4];
			for (int i = 0; i < 4; i++)
			{
				this.IDLevelProfile[i] = new BoolProfileData(datapath + "Boss.Map.IDBoss" + i, false);
			}
		}

		public void Reset()
		{
			for (int i = 0; i < 4; i++)
			{
				this.IDLevelProfile[i].setValue(false);
			}
		}

		public void SetIDLevelProfile(int index, bool value)
		{
			this.IDLevelProfile[index].setValue(value);
		}

		public bool GetIDlevelProfile(int index)
		{
			return this.IDLevelProfile[index].Data;
		}

		private BoolProfileData[] IDLevelProfile;
	}
}
