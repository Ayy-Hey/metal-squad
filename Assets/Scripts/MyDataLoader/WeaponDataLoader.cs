using System;

namespace MyDataLoader
{
	public class WeaponDataLoader
	{
		public string Name { get; set; }

		public int IdName { get; set; }

		public float[] DAMAGED { get; set; }

		public float[] FIRE_RATE { get; set; }

		public float[] CAPACITY { get; set; }

		public float[] TIME_RELOAD { get; set; }

		public float[] CRITICAL { get; set; }

		public string[] CoinUpgrade { get; set; }

		public int[] CampaignUpgrade { get; set; }

		public string[] PassiveDesc { get; set; }

		public int GunUnlock { get; set; }

		public int BulletPrice { get; set; }

		public int MSPrice { get; set; }

		public bool Vip { get; set; }

		public int RankBase { get; set; }

		public int[] CostRankUp { get; set; }

		public int CurrencyRankUp { get; set; }

		public int CurrencyUnlock { get; set; }
	}
}
