using System;
using com.dev.util.SecurityHelper;

public class MeleProfile
{
	public int LevelUpGradeMelee
	{
		get
		{
			return this.levelUpGradeMelee.Data;
		}
		set
		{
			this.levelUpGradeMelee.setValue(value);
		}
	}

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
            num2 = 1;

        }
		catch
		{
		}
		return (int)((float)num * num2);
	}

	public float Damage
	{
		get
		{
			return this.SecuredDamaged[ProfileManager.melesProfile[ProfileManager.meleCurrentId].LevelUpGradeMelee].Value;
		}
	}

	public bool Unlock
	{
		get
		{
			return this.unlock.Data;
		}
		set
		{
			this.unlock.setValue(value);
		}
	}

	public int Power()
	{
		float num = 1.5f * this.Damage;
		return (int)num;
	}

	public int PowerByLevel(int level)
	{
		float num = 1.5f * this.SecuredDamaged[level].Value;
		return (int)num;
	}

	protected string Name;

	public int[] range;

	public int[] speed;

	protected string STR_UnLock = "rambo.mele.unlock";

	protected string STR_Level = "rambo.mele.level";

	protected BoolProfileData unlock;

	public string NAME;

	public int RankBase;

	protected IntProfileData levelUpGradeMelee;

	public SecuredFloat[] SecuredDamaged;

	public string[] PriceUpgrade;
}
