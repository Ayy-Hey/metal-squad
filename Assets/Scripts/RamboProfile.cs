using System;
using com.dev.util.SecurityHelper;
using UnityEngine;

public class RamboProfile
{
	public RamboProfile(string sex, float GunDefaultDamage, float GunDefaultSpeed)
	{
		this.LevelUpgradeProfile = new IntProfileData("rambo.profile.data.level.upgrade2" + sex, -1);
		this.RankUppedProfile = new IntProfileData("rambo.profile.data.rankupped." + sex, 0);
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
        Debug.Log("this.PriceUpgrade " + this.PriceUpgrade[5]);
		int num = 0;
		float num2 = 1f;
		try
		{
			string s = this.PriceUpgrade[index].Split(new char[]
			{
				','
			})[1];
            Debug.Log("this.PriceUpgrade " + s);
            num = int.Parse(s);
            //num2 = (float)RemoteConfigFirebase.Instance.GetDoubleValue(RemoteConfigFirebase.SCALE_PRICE_UPGRADE, 1.0);
            num2 = 1;

        }
		catch
		{
		}
		return (int)((float)num * num2);
	}

	public int LevelUpgrade
	{
		get
		{
			SecuredInt data = this.LevelUpgradeProfile.Data;
			return Mathf.Max(0, data);
		}
		set
		{
			this.LevelUpgradeProfile.setValue(value);
		}
	}

	public int RankUpped
	{
		get
		{
			SecuredInt data = this.RankUppedProfile.Data;
			if (data < 2 && this.LevelUpgrade >= 20)
			{
				this.RankUppedProfile.setValue(2);
			}
			else if (data < 1 && this.LevelUpgrade >= 10)
			{
				this.RankUppedProfile.setValue(1);
			}
			return Mathf.Max(0, data);
		}
		set
		{
			this.RankUppedProfile.setValue(value);
		}
	}

	public float GetOption(int id)
	{
		return this.optionProfile[id][this.LevelUpgrade];
	}

	public float GetMaxOption(int id)
	{
		return this.optionProfile[id][29];
	}

	public float GetOptionByLevel(int id, int level)
	{
		return this.optionProfile[id][level];
	}

	public float HP
	{
		get
		{
			return this.optionProfile[0][this.LevelUpgrade];
		}
		set
		{
			this.optionProfile[0][this.LevelUpgrade] = value;
		}
	}

	public float Speed_Normal
	{
		get
		{
			return this.optionProfile[1][this.LevelUpgrade];
		}
		set
		{
			this.optionProfile[1][this.LevelUpgrade] = value;
		}
	}

	public int Speed_Sit
	{
		get
		{
			return this.Speed_Sit_Profile.Data;
		}
		set
		{
			this.Speed_Sit_Profile.setValue(value);
		}
	}

	public float Force_Jump
	{
		get
		{
			return this.optionProfile[2][this.LevelUpgrade];
		}
		set
		{
			this.optionProfile[2][this.LevelUpgrade] = value;
		}
	}

	public float Knife_Damage
	{
		get
		{
			return this.Knife_Damage_Profile.Data;
		}
		set
		{
			this.Knife_Damage_Profile.setValue(value);
		}
	}

	public int Power()
	{
		int num = this.LevelUpgrade;
		if (this.LevelUpgrade < 0)
		{
			num = 0;
		}
		float num2 = this.optionProfile[0][num];
		float num3 = this.optionProfile[2][num];
		float num4 = this.optionProfile[1][num];
		float num5 = 0.5f * num2 + 0.25f * num3 + 0.125f * num4;
		return (int)num5;
	}

	public int Power(int level)
	{
		int result;
		try
		{
			if (level < 0)
			{
				level = 0;
			}
			float num = this.optionProfile[0][level];
			float num2 = this.optionProfile[2][level];
			float num3 = this.optionProfile[1][level];
			int num4 = (int)(0.5f * num + 0.25f * num2 + 0.125f * num3);
			result = num4;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			result = 0;
		}
		return result;
	}

	public string name;

	public const string STR_HP_Profile = "rambo_hp";

	public const string STR_Speed_Normal = "rambo_speed_normal";

	public const string STR_Speed_Sit = "rambo_speed_sit";

	public const string STR_Force_Jump = "rambo_force_jump";

	public const string STR_Knife_Damage = "rambo_knife_damage";

	protected IntProfileData LevelUpgradeProfile;

	protected IntProfileData RankUppedProfile;

	public string[] PriceUpgrade;

	public float[][] optionProfile;

	public int[] CostRankUp;

	public SecuredInt RankBase;

	public SecuredInt CurrencyUnlock;

	public SecuredInt CurrencyRankUp;

	public SecuredInt GoldUnlock;

	public SecuredInt GemUnlock;

	public int StarUnlock;

	protected IntProfileData Speed_Sit_Profile;

	protected FloatProfileData Knife_Damage_Profile;

	public GrenadesProfile grenades;
}
