using System;
using com.dev.util.SecurityHelper;

public class GrenadeProfile
{
	public Item GetItemID(int index)
	{
		Item result = Item.Gold;
		try
		{
			string s = this.PriceUpgrade[index].Split(new char[]
			{
				','
			})[0];
			result = (Item)int.Parse(s);
		}
		catch
		{
		}
		return result;
	}

	public int ValueUpgrade(int index)
	{
		int num = 0;
		float num2 = 1f;
		try
		{
			string s = this.PriceUpgrade[index].Split(new char[]
			{
				','
			})[1];
			num = int.Parse(s);
            //num2 = (float)RemoteConfigFirebase.Instance.GetDoubleValue(RemoteConfigFirebase.SCALE_PRICE_UPGRADE, 1.0);
            num2=1;

        }
		catch
		{
		}
		return (int)((float)num * num2);
	}

	public int LevelUpGrade
	{
		get
		{
			return this.levelUpGrade.Data;
		}
		set
		{
			this.levelUpGrade.setValue(value);
		}
	}

	public bool IsBuy
	{
		get
		{
			return this.isBuy.Data;
		}
		set
		{
			this.isBuy.setValue(value);
		}
	}

	public int TotalBomb
	{
		get
		{
			return this.totalBomb.Data.Value;
		}
		set
		{
			this.totalBomb.setValue(value);
		}
	}

	public float GetOption(EGrenadeOption option)
	{
		return this.options[(int)option][this.levelUpGrade.Data];
	}

	public float GetMaxOption(EGrenadeOption option)
	{
		return this.options[(int)option][29];
	}

	public int Power()
	{
		float num = this.GetOption(EGrenadeOption.Damage) * 10f / 3f;
		return (int)num;
	}

	public int PowerByLevel(int level)
	{
		float num = this.options[0][level] * 10f / 3f;
		return (int)num;
	}

	public SecuredInt SecuredPrice;

	protected string Name;

	public int RankBase;

	public bool isVip;

	public float[][] options;

	protected const string STR_TOTALBOMB = "rambo.weapon.totalbom";

	protected const string STR_LEVELUPGRADE = "rambo.upgrade.level";

	protected const string STR_GUNBUY = "rambo.weapon.buy";

	protected IntProfileData levelUpGrade;

	protected BoolProfileData isBuy;

	protected IntProfileData totalBomb;

	public string[] PriceUpgrade;

	public string NAME_GRENADE;
}
