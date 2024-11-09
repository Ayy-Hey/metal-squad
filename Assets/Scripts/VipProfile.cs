using System;
using UnityEngine;

public class VipProfile
{
	public VipProfile()
	{
		this.point = new IntProfileData("rambo.inapp.ms.total", 0);
		int length = Enum.GetValues(typeof(E_Vip)).Length;
		this.hasGift = new BoolProfileData[length];
		for (int i = 0; i < length; i++)
		{
			this.hasGift[i] = new BoolProfileData("rambo.inapp.vip.hasgif" + i, true);
		}
	}

	internal int Point
	{
		get
		{
			return this.point.Data.Value;
		}
		set
		{
			this.point.setValue(value);
			this.GetVipLevel();
		}
	}

	internal void GetVipLevel()
	{
		try
		{
			E_Vip e_Vip = this.level;
			this.level = E_Vip.Vip0;
			for (int i = DataLoader.vipData.Levels.Length - 1; i >= 0; i--)
			{
				if (this.point.Data.Value >= DataLoader.vipData.Levels[i].point)
				{
					this.level = (E_Vip)i;
					break;
				}
			}
			if (e_Vip != this.level && this.level >= E_Vip.Vip1)
			{
				
				int num = VipManager.Instance.GetVipCurrent().dailyReward.ReducedTimeSpin * 60;
				if (ProfileManager.spinProfile.WaitTime > num)
				{
					ProfileManager.spinProfile.WaitTime -= num;
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	internal bool HasGift(E_Vip vip)
	{
		return this.hasGift[(int)vip].Data;
	}

	internal bool HasAnyGift()
	{
		for (int i = 0; i < DataLoader.vipData.Levels.Length; i++)
		{
			if (this.Point >= DataLoader.vipData.Levels[i].point && this.HasGift((E_Vip)i))
			{
				return true;
			}
		}
		return false;
	}

	internal void ReceiveGift(E_VipGiftStyle giftStyle, int vipLevel)
	{
		if (giftStyle != E_VipGiftStyle.Benefits)
		{
			if (giftStyle == E_VipGiftStyle.Rewards)
			{
				try
				{
					for (int i = 0; i < DataLoader.vipData.Levels[vipLevel].nowReward.items.Length; i++)
					{
						PopupManager.Instance.SaveReward((Item)DataLoader.vipData.Levels[vipLevel].nowReward.items[i], DataLoader.vipData.Levels[vipLevel].nowReward.amounts[i], "Vip:" + vipLevel + "_GiftStyle:Rewards", null);
					}
					this.ReceivedGift(vipLevel);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log("____Error:" + ex.Message);
				}
			}
		}
		else
		{
			try
			{
				for (int j = 0; j < DataLoader.vipData.Levels[vipLevel].dailyReward.items.Length; j++)
				{
					PopupManager.Instance.SaveReward((Item)DataLoader.vipData.Levels[vipLevel].dailyReward.items[j], DataLoader.vipData.Levels[vipLevel].dailyReward.amounts[j], "Vip:" + vipLevel + "_GiftStyle:Benefits", null);
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.Log("____Error:" + ex2.Message);
			}
		}
	}

	private void ReceivedGift(int vipId)
	{
		this.hasGift[vipId].setValue(false);
	}

	private IntProfileData point;

	internal E_Vip level;

	private BoolProfileData[] hasGift;
}
