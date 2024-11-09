using System;

namespace MyDataLoader
{
	public class GrenadeDataLoader
	{
		public string Name { get; set; }

		public int IdName { get; set; }

		public float[] Damage { get; set; }

		public float[] Damage_Range { get; set; }

		public float[] Effect_Time { get; set; }

		public string[] CoinUpgrade { get; set; }

		public int RankBase { get; set; }

		public int Price { get; set; }

		public bool Vip { get; set; }
	}
}
