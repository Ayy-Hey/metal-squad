using System;
using System.Collections.Generic;

namespace StarterPack
{
	public class PackInforData
	{
		public string Name { get; set; }

		public int ID_Inapp { get; set; }

		public string SKU { get; set; }

		public ItemGunData Gun { get; set; }

		public ItemGunData Gun2 { get; set; }

		public List<int> Coin { get; set; }

		public List<ItemBoosterData> Booster { get; set; }

		public CharacterPackData Character { get; set; }

		public float PriceOrigin { get; set; }

		public float PriceSale { get; set; }

		public int Weight { get; set; }
	}
}
