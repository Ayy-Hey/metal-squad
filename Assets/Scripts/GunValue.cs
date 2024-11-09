using System;
using com.dev.util.SecurityHelper;

public class GunValue
{
	public GunValue()
	{
		this.properties = new float[10][];
		for (int i = 0; i < 10; i++)
		{
			this.properties[i] = new float[30];
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

	public float[][] properties;

	public int[] campaignUpgrade;

	public string[] PassiveDesc;

	public int GunUnlock;

	public string gunName;

	public bool isVip;

	public int RankBase;

	public int[] CostRankUp;

	public Item CurrencyUnlock;

	public Item CurrencyRankUp;

	public string[] PriceUpgrade;

	public SecuredInt SecuredGemPrice;

	public SecuredInt SecuredBulletPrice;
}
