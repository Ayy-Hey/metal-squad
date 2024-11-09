using System;
using UnityEngine;

public class VipManager
{
	public static VipManager Instance
	{
		get
		{
			if (VipManager.instance == null)
			{
				VipManager.instance = new VipManager();
			}
			return VipManager.instance;
		}
	}

	public int Exp
	{
		get
		{
			int result = 0;
			try
			{
				result = ProfileManager.inAppProfile.vipProfile.Point;
			}
			catch
			{
			}
			return result;
		}
	}

	public VipLevel GetVipCurrent()
	{
		int level = (int)ProfileManager.inAppProfile.vipProfile.level;
		if (level < 0)
		{
			return null;
		}
		return DataLoader.vipData.Levels[level];
	}

	public int LevelCurrent()
	{
		int num = 0;
		try
		{
			num = (int)ProfileManager.inAppProfile.vipProfile.level;
			num++;
		}
		catch
		{
		}
		return num;
	}

	public int LevelCurrent(int exp)
	{
		int result = -1;
		for (int i = DataLoader.vipData.Levels.Length - 1; i >= 0; i--)
		{
			if (exp >= DataLoader.vipData.Levels[i].point)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public float ExpCurrent01(int exp, int levelCurrent)
	{
		int num = levelCurrent - 1;
		float num2 = (float)exp;
		int b = DataLoader.vipData.Levels.Length - 1;
		float value = Mathf.Max(0f, num2 - (float)((num >= 0) ? DataLoader.vipData.Levels[num].point : 0)) / (float)(DataLoader.vipData.Levels[Mathf.Min(num + 1, b)].point - ((num >= 0) ? DataLoader.vipData.Levels[num].point : 0));
		return Mathf.Clamp01(value);
	}

	public string ExpCurrent(int exp, int levelCurrent)
	{
		int num = levelCurrent - 1;
		float num2 = (float)exp;
		int b = DataLoader.vipData.Levels.Length - 1;
		return Mathf.Max(0f, num2 - (float)((num >= 0) ? DataLoader.vipData.Levels[num].point : 0)) + "/" + (DataLoader.vipData.Levels[Mathf.Min(num + 1, b)].point - ((num >= 0) ? DataLoader.vipData.Levels[num].point : 0));
	}

	private static VipManager instance;
}
