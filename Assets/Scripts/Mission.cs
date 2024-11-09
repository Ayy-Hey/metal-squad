using System;
using System.Collections.Generic;

namespace CreateMissionTool
{
	[Serializable]
	public struct Mission
	{
		public int Level;

		public string Name_Boss;

		public int Coin_Earn;

		public List<MissionDataTest> missionData;
	}
}
