using System;

namespace MyDataLoader
{
	public class RamboDataLoader
	{
		public string Name { get; set; }

		public int IdName { get; set; }

		public float[] HpLevel { get; set; }

		public float[] SpeedLevel { get; set; }

		public float[] JumpLevel { get; set; }

		public string[] CoinUpgrade { get; set; }

		public int RankBase { get; set; }

		public int[] CostRankUp { get; set; }

		public int CoinUnlock { get; set; }

		public int MsUnlock { get; set; }

		public int StarUnlock { get; set; }

		public int CurrencyUnlock { get; set; }

		public int CurrencyRankUp { get; set; }
	}
}
