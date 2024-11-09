using System;

public class BossModeData
{
	public BossModeData.BossModeDataChild[] boss;

	public class BossModeDataChild
	{
		public string name;

		public int unlockMap;

		public int unlockLevel;

		public int[] power;

		public int timesGetGem;

		public BossModeData.Reward reward;
	}

	public class Reward
	{
		public int id;

		public int value;
	}
}
