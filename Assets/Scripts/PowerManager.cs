using System;
using UnityEngine;

public class PowerManager
{
	public static PowerManager Instance
	{
		get
		{
			if (PowerManager.instance == null)
			{
				PowerManager.instance = new PowerManager();
			}
			return PowerManager.instance;
		}
	}

	public int TotalPower()
	{
		int num = 0;
		for (int i = 0; i < ProfileManager.grenadesProfile.Length; i++)
		{
			num += this.Grenade(i);
		}
		for (int j = 0; j < ProfileManager.melesProfile.Length; j++)
		{
			if (ProfileManager.melesProfile[j].Unlock)
			{
				num += this.Knife(j);
			}
		}
		for (int k = 0; k < ProfileManager.weaponsRifle.Length; k++)
		{
			if (ProfileManager.weaponsRifle[k].GetGunBuy())
			{
				num += this.Gun1(k);
			}
		}
		for (int l = 0; l < ProfileManager.weaponsSpecial.Length; l++)
		{
			if (ProfileManager.weaponsSpecial[l].GetGunBuy())
			{
				num += this.Gun2(l);
			}
		}
		return num;
	}

	public int TotalPowerRequireCampaign(GameMode.Mode mode, int Level)
	{
		return RemoteConfigFirebase.Instance.PowerCampaignValue(mode, Level);
	}

	public float RatePower(float powerRequire)
	{
		float value = 0f;
		if (powerRequire > (float)this.TotalPower())
		{
			value = 0.5f;
		}
		return Mathf.Clamp01(value);
	}

	public int Grenade(int id)
	{
		return ProfileManager.grenadesProfile[id].Power();
	}

	public int Knife(int id)
	{
		return ProfileManager.melesProfile[id].Power();
	}

	public int Character(int id)
	{
		return ProfileManager.rambos[id].Power();
	}

	public int Gun1(int id)
	{
		return ProfileManager.weaponsRifle[id].Power();
	}

	public int Gun2(int id)
	{
		int result = 0;
		if (id >= 0)
		{
			result = ProfileManager.weaponsSpecial[id].Power();
		}
		return result;
	}

	private static PowerManager instance;
}
