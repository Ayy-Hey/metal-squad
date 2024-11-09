using System;
using System.Collections;
using CustomData;
using UnityEngine;

public class PopupDailyGift : PopupBase
{
	private void OnEnable()
	{
		if ((float)Screen.width < (float)Screen.height * 1.7f)
		{
			this.tableObj.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		else
		{
			this.tableObj.localScale = Vector3.one;
		}
	}

	public void Awake()
	{
		this.ClearAndNewStart();
		PopupManager.Instance.SetCanvas(MenuManager.Instance.popupCanvas);
		this.giftProfile = ProfileManager.dailyGiftProfile.giftToday;
		for (int i = 0; i < this.Gifts.Length; i++)
		{
			this.Gifts[i].imgBG.raycastTarget = true;
		}
		if (this.giftProfile != null && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None)
		{
			ProfileManager.inAppProfile.FirstTime = true;
			PopupDailyGift.isReadyShowDailySale = true;
			ProfileManager.countSpinByMS.setValue(0);
		}
	}

	public override void OnClose()
	{
		base.StopAllCoroutines();
		if (this.giftProfile != null && !this.giftProfile.Done)
		{
			this.ShowGiftToday(false);
			return;
		}
		base.OnClose();
	}

	public void ShowGiftToday(bool isDoubleReward)
	{
		if (object.ReferenceEquals(this.giftProfile, null) || this.giftProfile.Done)
		{
			return;
		}
		base.StopAllCoroutines();
		this.objButtonClaim[0].SetActive(false);
		this.objButtonClaim[1].SetActive(false);
		this.objBtnClose.SetActive(true);
		this.objText.SetActive(true);
		AudioClick.Instance.OnClick();
		isDoubleReward = false;
		if (isDoubleReward)
		{
			AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isClose)
			{
				if (isClose)
				{
					this.objBtnClose.SetActive(true);
					this.RewardSuccess(true);
				}
				else
				{
					this.objButtonClaim[0].SetActive(true);
					this.objButtonClaim[1].SetActive(true);
					this.objBtnClose.SetActive(false);
					this.objText.SetActive(false);
					this.objEffectVideo.SetActive(false);
					if (!AdmobManager.Instance.IsVideoReady())
					{
						base.StartCoroutine(this.IEWaitVideo());
					}
					else
					{
						this.objEffectVideo.SetActive(true);
					}
				}
			});
		}
		else
		{
			this.RewardSuccess(false);
		}
	}

	private void RewardSuccess(bool isDoubleReward)
	{
		
		CustomData.Gift gift = ProfileManager.dailyGiftProfile.GetGift(this.giftProfile.day);
		int level = (int)ProfileManager.inAppProfile.vipProfile.level;
		int num = 1;
		if (level >= 0)
		{
			num = 1 + DataLoader.vipData.Levels[level].dailyReward.items.Length;
		}
		InforReward[] array = new InforReward[num];
		array[0] = new InforReward();
		array[0].item = gift.gift;
		array[0].amount = ((!isDoubleReward) ? gift.giftAmount : (gift.giftAmount * 2));
		Item gift2 = gift.gift;
		if (gift2 != Item.Flame_Gun)
		{
			if (gift2 != Item.Silver_Axe)
			{
				if (gift2 == Item.Sword)
				{
					if (ProfileManager.melesProfile[2].Unlock)
					{
						array[0].item = Item.Gem;
						array[0].amount = ((!isDoubleReward) ? PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 2) : (PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 2) * 2));
					}
				}
			}
			else if (ProfileManager.melesProfile[1].Unlock)
			{
				array[0].item = Item.Gem;
				array[0].amount = ((!isDoubleReward) ? PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 1) : (PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 1) * 2));
			}
		}
		else if (ProfileManager.weaponsSpecial[1].GetGunBuy())
		{
			array[0].item = Item.Gem;
			array[0].amount = ((!isDoubleReward) ? PopupManager.Instance.GetAmountGemIsHad(ItemConvert.GunSpecial, 1) : (PopupManager.Instance.GetAmountGemIsHad(ItemConvert.GunSpecial, 1) * 2));
		}
		if (PopupManager.Instance.CheckAmountItem(array[0].item) && level >= 0 && DataLoader.vipData.Levels[level].dailyReward.isX2RewarDailyGift)
		{
			array[0].amount = array[0].amount * 2;
			array[0].vipLevel = level;
		}
		if (level >= 0)
		{
			for (int i = 0; i < DataLoader.vipData.Levels[level].dailyReward.items.Length; i++)
			{
				array[i + 1] = new InforReward();
				int item = DataLoader.vipData.Levels[level].dailyReward.items[i];
				array[i + 1].amount = DataLoader.vipData.Levels[level].dailyReward.amounts[i];
				array[i + 1].item = (Item)item;
				array[i + 1].vipLevel = level;
				PopupManager.Instance.SaveReward((Item)item, DataLoader.vipData.Levels[level].dailyReward.amounts[i], base.name + "Reward_Daily_Vip", null);
			}
		}
		this.OnClick(isDoubleReward);
		base.StartCoroutine(this.ShowListReward(array));
	}

	private IEnumerator ShowListReward(InforReward[] listRewards)
	{
		yield return new WaitForSeconds(0.5f);
		PopupManager.Instance.ShowCongratulation(listRewards, true, delegate
		{
			this.OnClose();
		});
		yield break;
	}

	private void OnClick(bool isDoubleReward)
	{
		if (this.giftProfile != null && !this.giftProfile.Done)
		{
			this.giftProfile.Done = true;
			this.giftProfile.dateTime = DateTime.Today;
			ProfileManager.dailyGiftProfile.ClampGift(this.giftProfile.day, isDoubleReward);
			MenuManager.Instance.topUI.ShowCoin();
			this.Gifts[this.giftProfile.day].obj_Done.gameObject.SetActive(true);
			this.Gifts[this.giftProfile.day].ani_Done.AnimationState.SetAnimation(0, "animation2", false);
			this.Gifts[this.giftProfile.day].ani_Done.AnimationState.SetAnimation(0, "animation", false);
			this.Gifts[this.giftProfile.day].SetTextToday(true);
			this.Gifts[this.giftProfile.day].imgBG.raycastTarget = false;
			ProfileManager.dailyGiftProfile.giftToday = null;
		}
	}

	private IEnumerator IEWaitVideo()
	{
		yield return new WaitUntil(() => AdmobManager.Instance.IsVideoReady());
		this.objEffectVideo.SetActive(true);
		yield break;
	}

	public override void Show()
	{
		base.Show();
		this.DelayShow();
		if (this.giftProfile == null || this.giftProfile.Done)
		{
			this.objButtonClaim[0].SetActive(false);
			this.objBtnClose.SetActive(true);
			this.objText.SetActive(true);
		}
		else
		{
			this.objButtonClaim[0].SetActive(true);
			this.objBtnClose.SetActive(false);
		}
		for (int i = 0; i < 7; i++)
		{
			CustomData.Gift gift = ProfileManager.dailyGiftProfile.GetGift(i);
			bool flag = gift.gift > Item.Gacha_T3;
			Item item = gift.gift;
			int amount = gift.giftAmount;
			Item gift2 = gift.gift;
			if (gift2 != Item.Flame_Gun)
			{
				if (gift2 != Item.Silver_Axe)
				{
					if (gift2 == Item.Sword)
					{
						if (ProfileManager.melesProfile[2].Unlock != ProfileManager.dailyGiftProfile.Gifts[6].Done)
						{
							item = Item.Gem;
							amount = PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 2);
						}
					}
				}
				else if (ProfileManager.melesProfile[1].Unlock != ProfileManager.dailyGiftProfile.Gifts[6].Done)
				{
					item = Item.Gem;
					amount = PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 1);
				}
			}
			else if (ProfileManager.weaponsSpecial[1].GetGunBuy() != ProfileManager.dailyGiftProfile.Gifts[6].Done)
			{
				item = Item.Gem;
				amount = PopupManager.Instance.GetAmountGemIsHad(ItemConvert.GunSpecial, 1);
			}
			this.Gifts[i].OnShowCardReward(amount, item);
		}
	}

	private void DelayShow()
	{
		for (int i = 0; i < ProfileManager.dailyGiftProfile.Gifts.Length; i++)
		{
			if (ProfileManager.dailyGiftProfile.Gifts[i].Done)
			{
				this.Gifts[i].Done();
			}
			else
			{
				this.Gifts[i].NotDone();
			}
			if (DateTime.Compare(DateTime.Today, ProfileManager.dailyGiftProfile.Gifts[i].dateTime) == 0)
			{
				this.Gifts[i].SetTextToday(ProfileManager.dailyGiftProfile.Gifts[i].Done);
			}
		}
		if (this.giftProfile != null)
		{
			this.Gifts[this.giftProfile.day].ToDay();
		}
	}

	private bool IsDoneAll()
	{
		for (int i = 0; i < ProfileManager.dailyGiftProfile.Gifts.Length; i++)
		{
			if (!ProfileManager.dailyGiftProfile.Gifts[i].Done)
			{
				return false;
			}
		}
		return true;
	}

	public void ClearAndNewStart()
	{
		if (!this.IsDoneAll())
		{
			return;
		}
		DateTime dateTime = ProfileManager.dailyGiftProfile.Gifts[ProfileManager.dailyGiftProfile.Gifts.Length - 1].dateTime;
		DateTime today = DateTime.Today;
		if (DateTime.Compare(dateTime, today) < 0)
		{
			ProfileManager.dailyGiftProfile.NewWeek();
			for (int i = 0; i < ProfileManager.dailyGiftProfile.Gifts.Length; i++)
			{
				ProfileManager.dailyGiftProfile.Gifts[i].dateTime = DateTime.MinValue;
				ProfileManager.dailyGiftProfile.Gifts[i].Done = false;
			}
		}
	}

	public TodayGift[] Gifts;

	private GiftProfile giftProfile;

	public static bool isReadyShowDailySale;

	[SerializeField]
	private RectTransform tableObj;

	public GameObject objEffectVideo;

	public GameObject[] objButtonClaim;

	public GameObject objBtnClose;

	public GameObject objText;
}
