using System;

namespace Util.Data
{
	[Serializable]
	public struct ShopItemInfo
	{
		public Item item;

		public int quantity;

		public int discountMin;

		public int discountMax;

		public int price;
	}
}
