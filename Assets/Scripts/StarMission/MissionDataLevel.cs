using System;
using System.Collections.Generic;
using com.dev.util.SecurityHelper;

namespace StarMission
{
	public class MissionDataLevel
	{
		public int Level { get; set; }

		public int idBoss { get; set; }

		public int Coin_Earn { get; set; }

		public List<MissionData> missionData { get; set; }

		public void InitData(int level, string mode)
		{
			for (int i = 0; i < this.missionData.Count; i++)
			{
				this.missionData[i].InitData(level * 3 + i, mode);
			}
			this.Gold_Earn = new SecuredInt(this.Coin_Earn);
		}

		public SecuredInt Gold_Earn;
	}
}
