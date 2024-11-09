using System;
using Sale;
using UnityEngine;

public class SaleManager
{
	public static SaleManager Instance
	{
		get
		{
			if (SaleManager.instance == null)
			{
				SaleManager.instance = new SaleManager();
			}
			return SaleManager.instance;
		}
	}

	public int IDSale
	{
		get
		{
			int result = 0;
			try
			{
				result = this.saveIDSaleFromServer.Data.Value;
			}
			catch (Exception ex)
			{
			}
			return result;
		}
		set
		{
			this.saveIDSaleFromServer.setValue(value);
		}
	}

	public float ValueSale
	{
		get
		{
			float result = 0f;
			try
			{
				result = this.saveValueSaleFromServer.Data.Value;
			}
			catch (Exception ex)
			{
			}
			return result;
		}
		set
		{
			this.saveValueSaleFromServer.setValue(value);
		}
	}

	public void OnInit()
	{
		this.profileStarterPacks = new ProfileStarterPack[3];
		for (int i = 0; i < this.profileStarterPacks.Length; i++)
		{
			this.profileStarterPacks[i] = new ProfileStarterPack(i);
		}
		this.saveToday = new IntProfileData("com.metal.squad.SaleManager.saveToday", -1);
		this.countDayLogin = new IntProfileData("com.metal.squad.SaleManager.countDayLogin", 0);
		this.saveStateSaleFromServer = new BoolProfileData("com.metal.squad.SaleManager.saveStateSaleFromServer", false);
		this.saveIDSaleFromServer = new IntProfileData("com.metal.squad.SaleManager.saveIDSaleFromServer", -1);
		this.saveValueSaleFromServer = new FloatProfileData("com.metal.squad.SaleManager.saveValueSaleFromServer", 0f);
		if (this.saveToday.Data.Value != DateTime.Now.DayOfYear)
		{
			this.saveToday.setValue(DateTime.Now.DayOfYear);
			this.countDayLogin.setValue(this.countDayLogin.Data.Value + 1);
			ProfileManager.userProfile.gachaFreeProfile.setValue(true);
			this.saveStateSaleFromServer.setValue(false);
			this.saveIDSaleFromServer.setValue(-1);
			this.saveValueSaleFromServer.setValue(0f);
			ProfileManager.spinProfile.OnRandomID();
		}
		this.OnCalculatorStarterPack();
		this.isInit = true;
	}

	public void CheckSale()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.HasSaleServerToday())
		{
			return;
		}
		if (this.HasSaleClientToday())
		{
			this.IDSale = -1;
			this.ValueSale = 1f;
		}
	}

	public bool HasSaleServerToday()
	{
		if (!this.saveStateSaleFromServer.Data && RemoteConfigFirebase.Instance.GetLongValue(RemoteConfigFirebase.SALE_TODAY, 0L) != 0L)
		{
			this.saveStateSaleFromServer.setValue(true);
		}
		if (SaleManager.CountSaleServer > 5)
		{
			this.saveStateSaleFromServer.setValue(false);
		}
		SaleManager.CountSaleServer++;
		return this.saveStateSaleFromServer.Data;
	}

	public void TurnOffSaleServer()
	{
		this.saveStateSaleFromServer.setValue(false);
	}

	private bool HasSaleClientToday()
	{
		bool flag = this.countDayLogin.Data.Value == 4;
		bool flag2 = (this.countDayLogin.Data.Value - 4) % 6 == 0;
		return flag || flag2;
	}

	public void OnCalculatorStarterPack()
	{
		this.isStarterPackReady = false;
		if (this.countDayLogin.Data.Value > 3)
		{
			this.isStarterPack3Day = true;
		}
		for (int i = 0; i < this.profileStarterPacks.Length; i++)
		{
			if (!this.profileStarterPacks[i].isBuy)
			{
				this.isStarterPackReady = true;
				break;
			}
		}
		if (!this.isStarterPackReady)
		{
			this.isStarterPack3Day = true;
			return;
		}
		try
		{
			for (int j = 0; j < this.countDayLogin.Data.Value; j++)
			{
				if (this.countDayLogin.Data.Value - 1 > j)
				{
					this.profileStarterPacks[j].isShowed = true;
				}
			}
		}
		catch (Exception ex)
		{
		}
		for (int k = 0; k < this.profileStarterPacks.Length; k++)
		{
			if (this.profileStarterPacks[k].CountDisplay >= 5)
			{
				this.profileStarterPacks[k].isShowed = true;
			}
		}
		for (int l = 0; l < this.profileStarterPacks.Length; l++)
		{
			if (!this.profileStarterPacks[l].isShowed)
			{
				this.IDStarterPack = l;
				return;
			}
		}
		this.isShowAll = true;
		this.IDStarterPack = UnityEngine.Random.Range(0, 2);
	}

	public SaleManager.TYPE_SALE OnShowSalePack()
	{
		if (this.HasSaleClientToday())
		{
			return SaleManager.TYPE_SALE.Double_Gem;
		}
		if (!this.isStarterPack3Day)
		{
			return SaleManager.TYPE_SALE.StarterPack_1;
		}
		return SaleManager.TYPE_SALE.DailySale;
	}

	private static SaleManager instance;

	public ProfileStarterPack[] profileStarterPacks;

	private IntProfileData saveToday;

	private IntProfileData countDayLogin;

	private BoolProfileData saveStateSaleFromServer;

	private IntProfileData saveIDSaleFromServer;

	private FloatProfileData saveValueSaleFromServer;

	public bool isInit;

	public bool isStarterPackReady;

	public bool isStarterPack3Day;

	public bool isShowAll;

	public int IDStarterPack;

	public int CountSaleSesion;

	public static int CountSaleServer;

	public enum TYPE_SALE
	{
		Double_Gem,
		StarterPack_1,
		DailySale
	}
}
