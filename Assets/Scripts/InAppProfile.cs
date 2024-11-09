using System;
using System.Collections.Generic;
using Inapp.ShopItem;
using UnityEngine;

public class InAppProfile
{
	public InAppProfile()
	{
		this.first_time = new BoolProfileData("rambo.first.time", true);
		this.buySalePacks = new BoolProfileData[15];
		for (int i = 0; i < this.buySalePacks.Length; i++)
		{
			this.buySalePacks[i] = new BoolProfileData("rambo.sale.buy.pack" + i, false);
		}
		this.salePacks = new IntProfileData[3];
		for (int j = 0; j < this.salePacks.Length; j++)
		{
			this.salePacks[j] = new IntProfileData("rambo.sale.view.pack" + j, 0);
		}
		this.InitDiscountTime();
		this.vipProfile = new VipProfile();
		this.shopItemProfile = new ShopItemProfile();
	}

	public bool FirstTime
	{
		get
		{
			return this.first_time.Data;
		}
		set
		{
			this.first_time.setValue(value);
		}
	}

	public int DiscountTotalTime
	{
		get
		{
			return this.discount_total_time.Data;
		}
		set
		{
			this.discount_total_time.setValue(value);
		}
	}

	public void SetBuyPack(int pack, bool value = true)
	{
		this.buySalePacks[pack].setValue(value);
	}

	public bool GetBuyPack(int pack)
	{
		return this.buySalePacks[pack].Data;
	}

	public bool HasPackSale()
	{
		return this.salePacks[0].Data >= 0 || this.salePacks[1].Data >= 0 || this.salePacks[2].Data >= 0;
	}

	private void InitDiscountTime()
	{
		this.discount_total_time = new IntProfileData("rambo.discount.total.time", 0);
		int time = this.GetTime();
		if (this.discount_total_time.Data <= time)
		{
			this.discount_total_time.setValue(time + 4320);
			this.ResetSalePack();
		}
	}

	private int GetTime()
	{
		DateTime now = DateTime.Now;
		return (int)((float)now.Year * 365.25f * 24f * 60f) + now.DayOfYear * 24 * 60 + now.Hour * 60 + now.Minute;
	}

	private void ResetSalePack()
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		List<List<int>> list4 = new List<List<int>>
		{
			new List<int>(),
			new List<int>(),
			new List<int>()
		};
		for (int i = 0; i < this.buySalePacks.Length; i++)
		{
			if (!this.buySalePacks[i].Data)
			{
				list4[i / 5].Add(i);
			}
		}
		this.salePacks[0].setValue(list4[0][UnityEngine.Random.Range(0, list4[0].Count)]);
		if (list4[1].Count >= 1)
		{
			this.salePacks[1].setValue(list4[1][UnityEngine.Random.Range(0, list4[1].Count)]);
		}
		else
		{
			this.salePacks[1].setValue(-1);
		}
		if (list4[2].Count >= 1)
		{
			this.salePacks[2].setValue(list4[2][UnityEngine.Random.Range(0, list4[2].Count)]);
		}
		else
		{
			this.salePacks[2].setValue(-1);
		}
	}

	private const string str_first_time_game = "rambo.first.time";

	private const string str_discount_total_time = "rambo.discount.total.time";

	private const string str_starter_pack2_buy = "rambo.starter.pack1.buy";

	private BoolProfileData first_time;

	private IntProfileData discount_total_time;

	public IntProfileData[] salePacks;

	private BoolProfileData[] buySalePacks;

	internal VipProfile vipProfile;

	public ShopItemProfile shopItemProfile;
}
