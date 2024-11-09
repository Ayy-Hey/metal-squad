using System;
using System.Collections.Generic;
using Inapp.ShopItem;
using UnityEngine;
using UnityEngine.UI;
using Util.Data;

public class ShopItem : PopupBase
{
	public void Show(Action hideCallback)
	{
		base.Show();
		this.hideCallback = hideCallback;
		this.typeShow = ShopItem.ETypeShow.ShopItem;
		this.OnBtnClick((int)this.typeShow);
		bool flag = (double)ProfileManager.inAppProfile.shopItemProfile.TimeReset <= TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
		if (flag)
		{
			this.ResetItem();
		}
		else
		{
			this._spanWait = TimeSpan.FromSeconds((double)ProfileManager.inAppProfile.shopItemProfile.TimeReset);
		}
		this.ShowItem();
		this._old = DateTime.Now;
		this.UpdateCoolDown();
		this.isShow = true;
		for (int i = 0; i < this.packSales.Length; i++)
		{
			this.packSales[i].OnShow(delegate
			{
				MenuManager.Instance.topUI.ReloadCoin();
			});
		}
	}

	public void Close()
	{
		this.isShow = false;
		if (this.hideCallback != null)
		{
			this.hideCallback();
		}
		base.OnClose();
	}

	public void OnBtnClick(int id)
	{
		this.typeShow = (ShopItem.ETypeShow)id;
		for (int i = 0; i < this.highLightBtns.Length; i++)
		{
			this.highLightBtns[i].SetActive(i == id);
		}
		for (int j = 0; j < this.objContents.Length; j++)
		{
			this.objContents[j].SetActive(j == id);
		}
		this.txtCoolDownTime.gameObject.SetActive(this.typeShow == ShopItem.ETypeShow.ShopItem);
	}

	private void LateUpdate()
	{
		if (this._reward)
		{
			this._reward = false;
			PopupManager.Instance.SaveReward(this.dataVideoAds.shopItem[this._itemRewardAds.id].item, this.dataVideoAds.shopItem[this._itemRewardAds.id].quantity, "ShopItem_VideoAds", null);
			if (this._itemRewardAds.id == ProfileManager.inAppProfile.shopItemProfile.ItemVideoAds.id)
			{
				this._itemRewardAds.buy = true;
				ProfileManager.inAppProfile.shopItemProfile.ItemVideoAds = this._itemRewardAds;
				this.items[0].obj_Lock.SetActive(true);
				this.items[0].button.interactable = false;
			}
			InforReward[] array = new InforReward[]
			{
				new InforReward()
			};
			array[0].amount = this.dataVideoAds.shopItem[this._itemRewardAds.id].quantity;
			array[0].item = this.dataVideoAds.shopItem[this._itemRewardAds.id].item;
			PopupManager.Instance.ShowCongratulation(array, false, delegate
			{
				if (MenuManager.Instance)
				{
					MenuManager.Instance.topUI.ShowCoin();
				}
			});
		}
		if (this.typeShow == ShopItem.ETypeShow.ShopItem)
		{
			DateTime now = DateTime.Now;
			if (now.Second != this._old.Second)
			{
				this._old = now;
				this.UpdateCoolDown();
			}
		}
	}

	private void UpdateCoolDown()
	{
		this._spanCoolDown = this._spanWait - TimeSpan.FromSeconds((double)(this._old.Ticks / 10000000L));
		if (this._spanCoolDown.TotalSeconds <= 0.0)
		{
			this.ResetItem();
			this.ShowItem();
		}
		string[] array = this._spanCoolDown.ToString().Split(new char[]
		{
			'.'
		});
		this.txtCoolDownTime.text = array[0];
	}

	public void OnBuy(int id)
	{
		switch (this.items[id].value)
		{
		case 0:
			this._idClickItem = id;
			this._itemRewardAds = ProfileManager.inAppProfile.shopItemProfile.ItemVideoAds;
			AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isSuccess)
			{
				this._reward = isSuccess;
			});
			break;
		case 1:
		{
			ItemInfo info = ProfileManager.inAppProfile.shopItemProfile.GetItemGold(this.items[id].idCard);
			if (info.buy)
			{
				return;
			}
			bool flag = ProfileManager.userProfile.Ms >= info.price;
			if (flag)
			{
				PopupManager.Instance.SaveReward(Item.Gem, -info.price, "ShopItem_BuyGold_Amount:" + this.dataGolds.shopItem[info.id].quantity, null);
				info.buy = true;
				ProfileManager.inAppProfile.shopItemProfile.SetItemGold(this.items[id].idCard, info);
				PopupManager.Instance.SaveReward(this.dataGolds.shopItem[info.id].item, this.dataGolds.shopItem[info.id].quantity, "ShopItem_Buy", null);
				this.items[id].obj_Lock.SetActive(true);
				this.items[id].button.interactable = false;
				InforReward[] array = new InforReward[]
				{
					new InforReward()
				};
				array[0].amount = this.dataGolds.shopItem[info.id].quantity;
				array[0].item = this.dataGolds.shopItem[info.id].item;
				PopupManager.Instance.ShowCongratulation(array, true, delegate
				{
					if (MenuManager.Instance)
					{
						MenuManager.Instance.topUI.ShowCoin();
					}
				});
			}
			else
			{
				string note = PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText(Localization0.Gem, null);
				MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, info.price, note);
				base.transform.localScale = Vector3.zero;
				MenuManager.Instance.popupInformation.OnClosed = delegate()
				{
					base.transform.localScale = Vector3.one;
				};
			}
			break;
		}
		case 2:
		{
			ItemInfo info = ProfileManager.inAppProfile.shopItemProfile.GetItemBooster(this.items[id].idCard);
			if (info.buy)
			{
				return;
			}
			bool flag = ProfileManager.userProfile.Coin >= info.price;
			if (flag)
			{
				PopupManager.Instance.SaveReward(Item.Gold, -info.price, string.Concat(new object[]
				{
					"ShopItem_Buy",
					this.dataBoosters.shopItem[info.id].item,
					"_Amount:",
					this.dataBoosters.shopItem[info.id].quantity
				}), null);
				info.buy = true;
				ProfileManager.inAppProfile.shopItemProfile.SetItemBooster(this.items[id].idCard, info);
				PopupManager.Instance.SaveReward(this.dataBoosters.shopItem[info.id].item, this.dataBoosters.shopItem[info.id].quantity, "ShopItem_Buy", null);
				this.items[id].obj_Lock.SetActive(true);
				this.items[id].button.interactable = false;
				InforReward[] array2 = new InforReward[]
				{
					new InforReward()
				};
				array2[0].amount = this.dataBoosters.shopItem[info.id].quantity;
				array2[0].item = this.dataBoosters.shopItem[info.id].item;
				PopupManager.Instance.ShowCongratulation(array2, true, delegate
				{
					if (MenuManager.Instance)
					{
						MenuManager.Instance.topUI.ShowCoin();
					}
				});
			}
			else
			{
				string note2 = PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText(Localization0.Gold, null);
				MenuManager.Instance.popupInformation.ShowWarning(Item.Gold, info.price, note2);
				base.transform.localScale = Vector3.zero;
				MenuManager.Instance.popupInformation.OnClosed = delegate()
				{
					base.transform.localScale = Vector3.one;
				};
			}
			break;
		}
		case 3:
		{
			ItemInfo info = ProfileManager.inAppProfile.shopItemProfile.GetItemGrenade(this.items[id].idCard);
			if (info.buy)
			{
				return;
			}
			bool flag = ProfileManager.userProfile.Coin >= info.price;
			if (flag)
			{
				PopupManager.Instance.SaveReward(Item.Gold, -info.price, string.Concat(new object[]
				{
					"ShopItem_Buy",
					this.dataGrenades.shopItem[info.id].item,
					"_Amount:",
					this.dataGrenades.shopItem[info.id].quantity
				}), null);
				info.buy = true;
				ProfileManager.inAppProfile.shopItemProfile.SetItemGrenade(this.items[id].idCard, info);
				PopupManager.Instance.SaveReward(this.dataGrenades.shopItem[info.id].item, this.dataGrenades.shopItem[info.id].quantity, "ShopItem_Buy", null);
				this.items[id].obj_Lock.SetActive(true);
				this.items[id].button.interactable = false;
				InforReward[] array3 = new InforReward[]
				{
					new InforReward()
				};
				array3[0].amount = this.dataGrenades.shopItem[info.id].quantity;
				array3[0].item = this.dataGrenades.shopItem[info.id].item;
				PopupManager.Instance.ShowCongratulation(array3, true, delegate
				{
					if (MenuManager.Instance)
					{
						MenuManager.Instance.topUI.ShowCoin();
					}
				});
			}
			else
			{
				string note3 = PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText(Localization0.Gold, null);
				MenuManager.Instance.popupInformation.ShowWarning(Item.Gold, info.price, note3);
				base.transform.localScale = Vector3.zero;
				MenuManager.Instance.popupInformation.OnClosed = delegate()
				{
					base.transform.localScale = Vector3.one;
				};
			}
			break;
		}
		case 4:
		{
			ItemInfo info = ProfileManager.inAppProfile.shopItemProfile.GetItemFragment(this.items[id].idCard);
			if (info.buy)
			{
				return;
			}
			bool flag = ProfileManager.userProfile.Ms >= info.price;
			if (flag)
			{
				PopupManager.Instance.SaveReward(Item.Gem, -info.price, string.Concat(new object[]
				{
					"ShopItem_Buy",
					this.dataFragments.shopItem[info.id].item,
					"_Amount:",
					this.dataFragments.shopItem[info.id].quantity
				}), null);
				info.buy = true;
				ProfileManager.inAppProfile.shopItemProfile.SetItemFragment(this.items[id].idCard, info);
				PopupManager.Instance.SaveReward(this.dataFragments.shopItem[info.id].item, this.dataFragments.shopItem[info.id].quantity, "ShopItem_Buy", null);
				this.items[id].obj_Lock.SetActive(true);
				this.items[id].button.interactable = false;
				InforReward[] array4 = new InforReward[]
				{
					new InforReward()
				};
				array4[0].amount = this.dataFragments.shopItem[info.id].quantity;
				array4[0].item = this.dataFragments.shopItem[info.id].item;
				PopupManager.Instance.ShowCongratulation(array4, true, delegate
				{
					if (MenuManager.Instance)
					{
						MenuManager.Instance.topUI.ShowCoin();
					}
				});
			}
			else
			{
				string note4 = PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText(Localization0.Gem, null);
				MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, info.price, note4);
				base.transform.localScale = Vector3.zero;
				MenuManager.Instance.popupInformation.OnClosed = delegate()
				{
					base.transform.localScale = Vector3.one;
				};
			}
			break;
		}
		}
	}

	private void ShowItem()
	{
		this.ShowItem(0, 0, ShopItem.ETypeItem.Video_Ads, ProfileManager.inAppProfile.shopItemProfile.ItemVideoAds, this.dataVideoAds);
		for (int i = 0; i < 3; i++)
		{
			int idItem = i + 7;
			this.ShowItem(idItem, i, ShopItem.ETypeItem.Fragment, ProfileManager.inAppProfile.shopItemProfile.GetItemFragment(i), this.dataFragments);
			if (i > 1)
			{
				break;
			}
			idItem = i + 1;
			this.ShowItem(idItem, i, ShopItem.ETypeItem.Gold, ProfileManager.inAppProfile.shopItemProfile.GetItemGold(i), this.dataGolds);
			idItem = i + 3;
			this.ShowItem(idItem, i, ShopItem.ETypeItem.Booster, ProfileManager.inAppProfile.shopItemProfile.GetItemBooster(i), this.dataBoosters);
			idItem = i + 5;
			this.ShowItem(idItem, i, ShopItem.ETypeItem.Grenade, ProfileManager.inAppProfile.shopItemProfile.GetItemGrenade(i), this.dataGrenades);
		}
	}

	private void ShowItem(int idItem, int idProfile, ShopItem.ETypeItem type, ItemInfo info, DataShopItem data)
	{
		this.items[idItem].idCard = idProfile;
		this.items[idItem].value = (int)type;
		this.items[idItem].img_Main.sprite = PopupManager.Instance.sprite_Item[(int)data.shopItem[info.id].item];
		this.items[idItem].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(data.shopItem[info.id].item)];
		this.items[idItem].txt_Amount.text = data.shopItem[info.id].quantity.ToString();
		this.items[idItem].obj_Lock.gameObject.SetActive(info.buy);
		this.items[idItem].button.interactable = !info.buy;
		this.items[idItem].item = data.shopItem[info.id].item;
		if (type != ShopItem.ETypeItem.Video_Ads)
		{
			this.items[idItem].txt_Description.text = info.price.ToString();
			bool flag = type == ShopItem.ETypeItem.Booster || type == ShopItem.ETypeItem.Grenade;
			this.items[idItem].img_Core.sprite = ((!flag) ? PopupManager.Instance.sprite_Item[4] : PopupManager.Instance.sprite_Item[1]);
			float num = (float)(data.shopItem[info.id].price - info.price);
			if (num > 0f)
			{
				num = Mathf.Round(num / (float)data.shopItem[info.id].price * 100f);
				this.items[idItem].txt_Name.text = "-" + num + "%";
				this.items[idItem].obj_Note.gameObject.SetActive(true);
			}
			else
			{
				this.items[idItem].obj_Note.gameObject.SetActive(false);
			}
		}
		else
		{
			this.items[idItem].obj_Note.gameObject.SetActive(false);
		}
		this.items[idItem].ShowBorderEffect();
	}

	private void ResetItem()
	{
		TimeSpan timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks) + TimeSpan.FromSeconds(14400.0);
		ProfileManager.inAppProfile.shopItemProfile.TimeReset = (long)timeSpan.TotalSeconds;
		this._spanWait = TimeSpan.FromSeconds((double)ProfileManager.inAppProfile.shopItemProfile.TimeReset);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		list.Add(ProfileManager.inAppProfile.shopItemProfile.ItemVideoAds.id);
		for (int i = 0; i < 3; i++)
		{
			list5.Add(ProfileManager.inAppProfile.shopItemProfile.GetItemFragment(i).id);
			if (i > 1)
			{
				break;
			}
			list2.Add(ProfileManager.inAppProfile.shopItemProfile.GetItemGold(i).id);
			list3.Add(ProfileManager.inAppProfile.shopItemProfile.GetItemBooster(i).id);
			list4.Add(ProfileManager.inAppProfile.shopItemProfile.GetItemGrenade(i).id);
		}
		ItemInfo itemInfo = this.ResetItem(ref list, this.dataVideoAds);
		ProfileManager.inAppProfile.shopItemProfile.ItemVideoAds = itemInfo;
		for (int j = 0; j < 3; j++)
		{
			itemInfo = this.ResetItem(ref list5, this.dataFragments);
			ProfileManager.inAppProfile.shopItemProfile.SetItemFragment(j, itemInfo);
			if (j > 1)
			{
				break;
			}
			itemInfo = this.ResetItem(ref list2, this.dataGolds);
			ProfileManager.inAppProfile.shopItemProfile.SetItemGold(j, itemInfo);
			itemInfo = this.ResetItem(ref list3, this.dataBoosters);
			ProfileManager.inAppProfile.shopItemProfile.SetItemBooster(j, itemInfo);
			itemInfo = this.ResetItem(ref list4, this.dataGrenades);
			ProfileManager.inAppProfile.shopItemProfile.SetItemGrenade(j, itemInfo);
		}
	}

	private ItemInfo ResetItem(ref List<int> listOldId, DataShopItem data)
	{
		int num = listOldId[0];
		int max = data.shopItem.Length;
		while (listOldId.Contains(num))
		{
			num = UnityEngine.Random.Range(0, max);
		}
		listOldId.Add(num);
		int num2;
		if (data.shopItem[num].discountMax == data.shopItem[num].discountMin)
		{
			num2 = data.shopItem[num].discountMax;
		}
		else
		{
			int num3 = data.shopItem[num].discountMin - data.shopItem[num].discountMax;
			int max2 = num3 / 5 + 1;
			num2 = data.shopItem[num].discountMax + UnityEngine.Random.Range(0, max2) * 5;
			num2 = Mathf.Min(num2, data.shopItem[num].discountMin);
		}
		ItemInfo result = new ItemInfo(num, num2, false);
		return result;
	}

	[HideInInspector]
	public bool isShow;

	[SerializeField]
	private CardBase[] items;

	[SerializeField]
	private DataShopItem dataVideoAds;

	[SerializeField]
	private DataShopItem dataGolds;

	[SerializeField]
	private DataShopItem dataBoosters;

	[SerializeField]
	private DataShopItem dataGrenades;

	[SerializeField]
	private DataShopItem dataFragments;

	[SerializeField]
	private Text txtCoolDownTime;

	[SerializeField]
	private GameObject[] highLightBtns;

	[SerializeField]
	private GameObject[] objContents;

	private TimeSpan _spanWait;

	private TimeSpan _spanCoolDown;

	private DateTime _old;

	private int _idClickItem;

	private bool _reward;

	private ItemInfo _itemRewardAds;

	private Action hideCallback;

	private ShopItem.ETypeShow typeShow;

	[SerializeField]
	private PackInforDailySale[] packSales;

	private enum ETypeItem
	{
		Video_Ads,
		Gold,
		Booster,
		Grenade,
		Fragment
	}

	private enum ETypeShow
	{
		ShopItem,
		ShopBundle
	}
}
