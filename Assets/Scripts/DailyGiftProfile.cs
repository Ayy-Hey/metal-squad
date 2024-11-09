using System;
using CustomData;
using UnityEngine;

public class DailyGiftProfile
{
	public DailyGiftProfile()
	{
		this.countLoginProfile = new IntProfileData("com.metal.squad.DailyGiftProfile.countLoginProfile", 0);
		this.saveLoginToday = new IntProfileData("com.metal.squad.DailyGiftProfile.saveLoginToday", -1);
		this.IsDoneTodayProfile = new BoolProfileData("com.rambo.metal.squad.dailygift.isdonetoday", false);
		this.Gifts = new GiftProfile[7];
		this.Gifts[0] = new GiftProfile(0, new int[]
		{
			1
		}, new int[]
		{
			3
		});
		this.Gifts[1] = new GiftProfile(1, new int[1], new int[]
		{
			500
		});
		this.Gifts[2] = new GiftProfile(2, new int[1], new int[]
		{
			800
		});
		this.Gifts[3] = new GiftProfile(3, new int[]
		{
			2
		}, new int[]
		{
			5
		});
		this.Gifts[4] = new GiftProfile(4, new int[]
		{
			1
		}, new int[]
		{
			5
		});
		this.Gifts[5] = new GiftProfile(5, new int[]
		{
			0,
			1
		}, new int[]
		{
			1000,
			3
		});
		this.Gifts[6] = new GiftProfile(6, new int[]
		{
			0,
			1,
			2
		}, new int[]
		{
			1000,
			3,
			5
		});
		this.week = new IntProfileData("com.metal.squad.dailygift.week", 0);
		this.gifts = Resources.Load<GiftArray>("DailyGift/DailyGift");
	}

	public void CheckDay(DateTime timeServer)
	{
		this.trueDateTime = DateTime.Now;
		int dayOfYear = DateTime.Now.DayOfYear;
		int num = dayOfYear;
		int num2 = Mathf.Abs(dayOfYear - timeServer.DayOfYear);
		UnityEngine.Debug.Log("ngaychenh: " + num2);
		if (num2 >= 2)
		{
			num = timeServer.DayOfYear;
			this.trueDateTime = timeServer;
		}
		if (this.saveLoginToday.Data.Value != num)
		{
			if (this.saveLoginToday.Data.Value != -1)
			{
				this.lastNotPlayDayTotal = num - this.saveLoginToday.Data.Value;
			}
			this.saveLoginToday.setValue(num);
			this.countLoginProfile.setValue(this.CountLoginCurrent() + 1);
			this.giftToday = this.GiftToday();
		}
	}

	public int CountLoginCurrent()
	{
		return this.countLoginProfile.Data.Value;
	}

	public void NewWeek()
	{
		if (this.week.Data.Value >= 3)
		{
			this.week.setValue(1);
		}
		else
		{
			this.week.setValue(this.week.Data.Value + 1);
		}
	}

	public int Week
	{
		get
		{
			return this.week.Data.Value;
		}
	}

	private GiftProfile GiftToday()
	{
		if (!this.Gifts[0].Done && this.Gifts[0].dateTime == DateTime.MinValue)
		{
			return this.Gifts[0];
		}
		int i = 1;
		while (i < this.Gifts.Length)
		{
			GiftProfile giftProfile = this.Gifts[i];
			GiftProfile giftProfile2 = this.Gifts[i - 1];
			if (!this.Gifts[i].Done)
			{
				if (DateTime.Compare(this.trueDateTime, giftProfile2.dateTime) > 0)
				{
					return this.Gifts[i];
				}
				break;
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	public bool IsDoneToday
	{
		get
		{
			return this.IsDoneTodayProfile.Data;
		}
		set
		{
			this.IsDoneTodayProfile.setValue(value);
		}
	}

	public CustomData.Gift GetGift(int day)
	{
		int num = day + 7 * this.Week;
		return this.gifts.gifts[num];
	}

	public void ClampGift(int day, bool isDoubleReward)
	{
		int num = day + 7 * this.Week;
		Item gift = this.gifts.gifts[num].gift;
		if (gift != Item.Silver_Axe)
		{
			if (gift != Item.Sword)
			{
				if (gift != Item.Flame_Gun)
				{
					PopupManager.Instance.SaveReward(this.gifts.gifts[num].gift, (!isDoubleReward) ? this.gifts.gifts[num].giftAmount : (this.gifts.gifts[num].giftAmount * 2), "DailyGift", null);
				}
				else if (ProfileManager.weaponsSpecial[1].GetGunBuy())
				{
					PopupManager.Instance.SaveReward(Item.Gem, (!isDoubleReward) ? PopupManager.Instance.GetAmountGemIsHad(ItemConvert.GunSpecial, 1) : (PopupManager.Instance.GetAmountGemIsHad(ItemConvert.GunSpecial, 1) * 2), "DailyGift_FlameGunPart", null);
				}
				else
				{
					PopupManager.Instance.SaveReward(this.gifts.gifts[num].gift, (!isDoubleReward) ? 1 : 2, "DailyGift_FlameGunPart", null);
				}
			}
			else if (ProfileManager.melesProfile[2].Unlock)
			{
				PopupManager.Instance.SaveReward(Item.Gem, (!isDoubleReward) ? PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 2) : (PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 2) * 2), "DailyGift_SwordPart", null);
			}
			else
			{
				PopupManager.Instance.SaveReward(this.gifts.gifts[num].gift, (!isDoubleReward) ? 1 : 2, "DailyGift_SwordPart", null);
			}
		}
		else if (ProfileManager.melesProfile[1].Unlock)
		{
			PopupManager.Instance.SaveReward(Item.Gem, (!isDoubleReward) ? PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 1) : (PopupManager.Instance.GetAmountGemIsHad(ItemConvert.Melee, 1) * 2), "DailyGift_SliverAxePart", null);
		}
		else
		{
			PopupManager.Instance.SaveReward(this.gifts.gifts[num].gift, (!isDoubleReward) ? 1 : 2, "DailyGift_SliverAxePart", null);
		}
	}

	private IntProfileData countLoginProfile;

	private IntProfileData saveLoginToday;

	public int lastNotPlayDayTotal;

	public GiftProfile[] Gifts;

	private BoolProfileData IsDoneTodayProfile;

	private DateTime dateTime;

	private GiftArray gifts;

	private IntProfileData week;

	private DateTime trueDateTime;

	public GiftProfile giftToday;
}
