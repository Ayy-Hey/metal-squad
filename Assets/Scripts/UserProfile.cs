using System;
using UnityEngine;

public class UserProfile
{
	public UserProfile()
	{
		this.gachaProfile = new IntProfileData[3];
		for (int i = 0; i < this.gachaProfile.Length; i++)
		{
			this.gachaProfile[i] = new IntProfileData("sora.metal.squad.gacha.id_" + i, 0);
		}
		this.gachaFreeProfile = new BoolProfileData("com.metal.squad.gacha.id_0.free", false);
		this.CoinProfile = new IntProfileData("rambo.user.profile.coind", 0);
		this.MsProfile = new IntProfileData("rambo.user.profile.ms", 0);
		this.CoinBonusProfile = new FloatProfileData("rambo.user.profile.CoinBonus", 1f);
		this.TotalGameProfile = new IntProfileData("com.sora.metal.squad.userprofile.TotalGameProfile", 0);
		this.GameWonProfile = new IntProfileData("com.sora.metal.squad.userprofile.GameWonProfile", 0);
		this.TotalCompeletedDailyQuestProfile = new IntProfileData("com.sora.metal.squad.userprofile.TotalCompeletedDailyQuestProfile", 0);
		this.TotalEnemyKilledProfile = new IntProfileData("com.sora.metal.squad.userprofile.TotalEnemyKilled", 0);
	}

	public int GetTotalGacha(int id)
	{
		int result = 0;
		try
		{
			result = Mathf.Max(this.gachaProfile[id].Data.Value, 0);
		}
		catch
		{
		}
		return result;
	}

	public void SetGachaValue(int id, int value)
	{
		int a = 0;
		try
		{
			a = this.GetTotalGacha(id) + value;
		}
		catch
		{
		}
		this.gachaProfile[id].setValue(Mathf.Max(a, 0));
	}

	public int GameWon
	{
		get
		{
			return this.GameWonProfile.Data.Value;
		}
		set
		{
			this.GameWonProfile.setValue(value);
		}
	}

	public int TotalGame
	{
		get
		{
			return this.TotalGameProfile.Data.Value;
		}
		set
		{
			this.TotalGameProfile.setValue(value);
		}
	}

	public int TotalCompletedDailyQuest
	{
		get
		{
			return this.TotalCompeletedDailyQuestProfile.Data.Value;
		}
		set
		{
			this.TotalCompeletedDailyQuestProfile.setValue(value);
		}
	}

	public int TotalEnemyKilled
	{
		get
		{
			return this.TotalEnemyKilledProfile.Data.Value;
		}
		set
		{
			this.TotalEnemyKilledProfile.setValue(value);
		}
	}

	public int Coin
	{
		get
		{
			return Mathf.Max(0, this.CoinProfile.Data);
		}
		set
		{
			int num = value - this.CoinProfile.Data;
			if (num > 0)
			{
				AchievementManager.Instance.MissionEarnCoin(num, null);
			}
			this.CoinProfile.setValue(value);
		}
	}

	public int Ms
	{
		get
		{
			return Mathf.Max(this.MsProfile.Data, 0);
		}
		set
		{
			this.MsProfile.setValue(value);
		}
	}

	public float CoinBonus
	{
		get
		{
			return this.CoinBonusProfile.Data.Value;
		}
		set
		{
			this.CoinBonusProfile.setValue(value);
		}
	}

	public void ResetCoin()
	{
		this.Ms = 0;
		this.Coin = 0;
	}

	private IntProfileData CoinProfile;

	private IntProfileData MsProfile;

	private FloatProfileData CoinBonusProfile;

	private IntProfileData[] gachaProfile;

	public BoolProfileData gachaFreeProfile;

	private IntProfileData TotalGameProfile;

	private IntProfileData GameWonProfile;

	private IntProfileData TotalCompeletedDailyQuestProfile;

	private IntProfileData TotalEnemyKilledProfile;
}
