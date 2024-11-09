using System;

namespace Inapp.ShopItem
{
	public class ShopItemProfile
	{
		public ShopItemProfile()
		{
			this.timeReset = new LongProfileData("com.metal.squad.shopitem.timereset", 0L);
			this.itemVideoAds.id = new IntProfileData("com.metal.squad.shopitem.itemVideoAds.id", -1);
			this.itemVideoAds.price = new IntProfileData("com.metal.squad.shopitem.itemVideoAds.price", 0);
			this.itemVideoAds.buy = new BoolProfileData("com.metal.squad.shopitem.itemVideoAds.buy", false);
			for (int i = 0; i < 3; i++)
			{
				this.itemFragments[i].id = new IntProfileData("com.metal.squad.shopitem.itemFragments.id" + i, -1);
				this.itemFragments[i].price = new IntProfileData("com.metal.squad.shopitem.itemFragments.price" + i, 0);
				this.itemFragments[i].buy = new BoolProfileData("com.metal.squad.shopitem.itemFragments.buy" + i, false);
				if (i > 1)
				{
					break;
				}
				this.itemGolds[i].id = new IntProfileData("com.metal.squad.shopitem.itemGolds.id" + i, -1);
				this.itemGolds[i].price = new IntProfileData("com.metal.squad.shopitem.itemGolds.price" + i, 0);
				this.itemGolds[i].buy = new BoolProfileData("com.metal.squad.shopitem.itemGolds.buy" + i, false);
				this.itemBoosters[i].id = new IntProfileData("com.metal.squad.shopitem.itemBoosters.id" + i, -1);
				this.itemBoosters[i].price = new IntProfileData("com.metal.squad.shopitem.itemBoosters.price" + i, 0);
				this.itemBoosters[i].buy = new BoolProfileData("com.metal.squad.shopitem.itemBoosters.buy" + i, false);
				this.itemGrenades[i].id = new IntProfileData("com.metal.squad.shopitem.itemGrenades.id" + i, -1);
				this.itemGrenades[i].price = new IntProfileData("com.metal.squad.shopitem.itemGrenades.price" + i, 0);
				this.itemGrenades[i].buy = new BoolProfileData("com.metal.squad.shopitem.itemGrenades.buy" + i, false);
			}
		}

		public ItemInfo ItemVideoAds
		{
			get
			{
				return this.itemVideoAds;
			}
			set
			{
				this.itemVideoAds.id.setValue(value.id);
				this.itemVideoAds.price.setValue(value.price);
				this.itemVideoAds.buy.setValue(value.buy);
			}
		}

		public ItemInfo GetItemGold(int id)
		{
			return this.itemGolds[id];
		}

		public void SetItemGold(int id, ItemInfo info)
		{
			this.itemGolds[id].id.setValue(info.id);
			this.itemGolds[id].price.setValue(info.price);
			this.itemGolds[id].buy.setValue(info.buy);
		}

		public ItemInfo GetItemBooster(int id)
		{
			return this.itemBoosters[id];
		}

		public void SetItemBooster(int id, ItemInfo info)
		{
			this.itemBoosters[id].id.setValue(info.id);
			this.itemBoosters[id].price.setValue(info.price);
			this.itemBoosters[id].buy.setValue(info.buy);
		}

		public ItemInfo GetItemGrenade(int id)
		{
			return this.itemGrenades[id];
		}

		public void SetItemGrenade(int id, ItemInfo info)
		{
			this.itemGrenades[id].id.setValue(info.id);
			this.itemGrenades[id].price.setValue(info.price);
			this.itemGrenades[id].buy.setValue(info.buy);
		}

		public ItemInfo GetItemFragment(int id)
		{
			return this.itemFragments[id];
		}

		public void SetItemFragment(int id, ItemInfo info)
		{
			this.itemFragments[id].id.setValue(info.id);
			this.itemFragments[id].price.setValue(info.price);
			this.itemFragments[id].buy.setValue(info.buy);
		}

		public long TimeReset
		{
			get
			{
				return this.timeReset.Data.Value;
			}
			set
			{
				this.timeReset.setValue(value);
			}
		}

		private ShopItemProfile.ItemProfile itemVideoAds;

		private ShopItemProfile.ItemProfile[] itemGolds = new ShopItemProfile.ItemProfile[2];

		private ShopItemProfile.ItemProfile[] itemBoosters = new ShopItemProfile.ItemProfile[2];

		private ShopItemProfile.ItemProfile[] itemGrenades = new ShopItemProfile.ItemProfile[2];

		private ShopItemProfile.ItemProfile[] itemFragments = new ShopItemProfile.ItemProfile[3];

		private LongProfileData timeReset;

		private struct ItemProfile
		{
			public static implicit operator ItemInfo(ShopItemProfile.ItemProfile item)
			{
				return new ItemInfo(item.id.Data.Value, item.price.Data.Value, item.buy.Data);
			}

			public IntProfileData id;

			public IntProfileData price;

			public BoolProfileData buy;
		}
	}
}
