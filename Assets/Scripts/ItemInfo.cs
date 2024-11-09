using System;

namespace Inapp.ShopItem
{
	[Serializable]
	public struct ItemInfo
	{
		public ItemInfo(int id, int price, bool buy)
		{
			this.id = id;
			this.price = price;
			this.buy = buy;
		}

		public int id;

		public int price;

		public bool buy;
	}
}
