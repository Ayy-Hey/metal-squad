using System;
using CustomData;
using UnityEngine;

public class BoosterProfile
{
	public BoosterProfile()
	{
		int length = Enum.GetValues(typeof(EBooster)).Length;
		this.items = new IntProfileData[length];
		for (int i = 0; i < length; i++)
		{
			this.items[i] = new IntProfileData("com.metal.squad.booster." + (EBooster)i, 0);
		}
		this.itemsUse = new bool[length];
		this.boosterPrices = Resources.Load<DataArray>("Booster/BoosterPrice");
		if (this.boosterPrices == null || this.boosterPrices.intDatas == null || this.boosterPrices.intDatas.Length < this.items.Length)
		{
			this.boosterPrices = new DataArray();
			this.boosterPrices.intDatas = this.itemsPrices;
		}
	}

	public int GetItem(EBooster booster)
	{
		return this.GetItem((int)booster);
	}

	internal int GetItem(int booster)
	{
		return this.items[booster].Data;
	}

	public void ItemAdd(EBooster booster, int num)
	{
		this.ItemAdd((int)booster, num);
	}

	internal void ItemAdd(int booster, int num)
	{
		this.items[booster].setValue(this.items[booster].Data + num);
		if (num < 0)
		{
			this.SetUseItem(booster, false);
		}
	}

	public int GetItemPrice(EBooster booster)
	{
		return this.GetItemPrice((int)booster);
	}

	internal int GetItemPrice(int booster)
	{
		return this.boosterPrices.intDatas[booster];
	}

	public bool IsUseItem(EBooster booster)
	{
		return this.IsUseItem((int)booster);
	}

	internal bool IsUseItem(int booster)
	{
		return this.itemsUse[booster];
	}

	public void SetUseItem(EBooster booster, bool isUse)
	{
		this.SetUseItem((int)booster, isUse);
	}

	internal void SetUseItem(int booster, bool isUse)
	{
		this.itemsUse[booster] = isUse;
	}

	public void ResetAllItem()
	{
		this.itemsUse = new bool[this.itemsUse.Length];
	}

	private IntProfileData[] items;

	private int[] itemsPrices = new int[]
	{
		1000,
		200,
		700,
		500,
		600,
		800,
		50,
		650,
		800,
		100
	};

	private bool[] itemsUse;

	private DataArray boosterPrices;
}
