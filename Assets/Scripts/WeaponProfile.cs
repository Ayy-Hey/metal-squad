using System;
using UnityEngine;

public class WeaponProfile
{
	public void SetGunBuy(bool buy)
	{
		this.Gun_Buy.setValue(buy);
	}

	public bool GetGunBuy()
	{
		return this.Gun_Buy.Data;
	}

	public int TimeCompleteUpgrade
	{
		get
		{
			return this.TimeToCompleteUpgrade.Data;
		}
		set
		{
			this.TimeToCompleteUpgrade.setValue(value);
		}
	}

	public float GetOptionByLevel(int id, int level)
	{
		return this.Gun_Value.properties[id][level];
	}

	public float GetOption(int id)
	{
		int value = this.LevelUpgrade.Data.Value;
		return this.Gun_Value.properties[id][value];
	}

	public float GetMaxOption(int id)
	{
		return this.Gun_Value.properties[id][14];
	}

	public float GetMinOption(int id)
	{
		return this.Gun_Value.properties[id][0];
	}

	public int GetRank()
	{
		return this.RankUpped.Data.Value + this.Gun_Value.RankBase;
	}

	public int GetRankBase()
	{
		return this.Gun_Value.RankBase;
	}

	public int GetRankUpped()
	{
		if (this.RankUpped.Data.Value < 2 && this.LevelUpgrade >= 20)
		{
			this.RankUpped.setValue(2);
		}
		else if (this.RankUpped.Data.Value < 1 && this.LevelUpgrade >= 10)
		{
			this.RankUpped.setValue(1);
		}
		return this.RankUpped.Data.Value;
	}

	public void SetRankUpped(int value)
	{
		int value2 = Mathf.Clamp(value - this.Gun_Value.RankBase, 0, 2);
		this.RankUpped.setValue(value2);
	}

	public int GetLevelUpgrade()
	{
		return this.LevelUpgrade.Data.Value;
	}

	public void SetLevelUpgrade(int value)
	{
		this.LevelUpgrade.setValue(value);
	}

	public int TotalBullet
	{
		get
		{
			return this.Total_Bullet_Profile.Data;
		}
		set
		{
			this.Total_Bullet_Profile.setValue(value);
		}
	}

	public float Radius_Damage
	{
		get
		{
			return this.Radius_Damage_Profile;
		}
		set
		{
			this.Radius_Damage_Profile = value;
		}
	}

	public int SaleTime
	{
		get
		{
			return this.Sale_Time.Data;
		}
		set
		{
			this.Sale_Time.setValue(value);
		}
	}

	public bool DontShowSale
	{
		get
		{
			return this.Dont_Show_Sale.Data;
		}
		set
		{
			this.Dont_Show_Sale.setValue(value);
		}
	}

	public float Get_Time_Reload
	{
		get
		{
			return this.Gun_Value.properties[3][this.GetLevelUpgrade()];
		}
	}

	public float Get_Critical
	{
		get
		{
			return this.Gun_Value.properties[4][this.GetLevelUpgrade()];
		}
	}

	public int Power()
	{
		int num = this.GetLevelUpgrade();
		if (!this.GetGunBuy())
		{
			num = 0;
		}
		float num2 = this.Gun_Value.properties[2][num];
		float num3 = this.Gun_Value.properties[3][num];
		float num4 = this.Gun_Value.properties[4][num];
		float num5 = num2 / num3 * (1f + num4) * 10f;
		return (int)num5;
	}

	public int PowerByLevel(int level)
	{
		float num = this.Gun_Value.properties[2][level];
		float num2 = this.Gun_Value.properties[3][level];
		float num3 = this.Gun_Value.properties[4][level];
		float num4 = num / num2 * (1f + num3) * 10f;
		return (int)num4;
	}

	protected const string STR_Total_Bullet_Profile = "rambo.weapon.profile.totalbullet";

	protected const string STR_LEVELUPGRADE = "rambo.upgrade.level";

	protected const string STR_RANKUPPEDGRADE = "rambo.upgrade.rank";

	protected const string STR_GUNBUY = "rambo.weapon.buy";

	protected const string STR_TimeToCompleteTheUpgrade = "rambo.weapon.time.complete.upgrade";

	protected const string STR_SaleTime = "rambo.weapon.saletime";

	protected const string STR_Dont_Show_Sale = "rambo.weapon.dontshowsale";

	protected IntProfileData LevelUpgrade;

	protected IntProfileData RankUpped;

	protected IntProfileData Total_Bullet_Profile;

	protected float Radius_Damage_Profile;

	protected BoolProfileData Gun_Buy;

	protected IntProfileData TimeToCompleteUpgrade;

	protected IntProfileData Sale_Time;

	protected BoolProfileData Dont_Show_Sale;

	public GunValue Gun_Value;
}
