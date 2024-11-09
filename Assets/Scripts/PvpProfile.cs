using System;
using Rank;
using UnityEngine;

public class PvpProfile
{
	public PvpProfile()
	{
		this.Init();
	}

	private void Init()
	{
		this.userName = new StringProfileData("com.sora.metal.squad.profilemanager.pvpprofile.userName", string.Empty);
		this.avatarUrl = new StringProfileData("com.sora.metal.squad.profilemanager.pvpprofile.avatarUrl", string.Empty);
		this.countryCode = new IntProfileData("com.sora.metal.squad.profilemanager.pvpprofile.countrycode", -1);
		this.winRate = new IntProfileData("com.sora.metal.squad.profilemanager.pvpprofile.winRate", 0);
		this.score = new IntProfileData("com.sora.metal.squad.profilemanager.pvpprofile.score", 0);
		this.winStreak = new IntProfileData("com.sora.metal.squad.profilemanager.pvpprofile.winStreak", 0);
		this.totalWinMatch = new IntProfileData("com.sora.metal.squad.profilemanager.pvpprofile.totalWinMatch", 0);
		this.totalMatch = new IntProfileData("com.sora.metal.squad.profilemanager.pvpprofile.totalMatch", 0);
	}

	public string UserName
	{
		get
		{
			return this.userName.Data;
		}
		set
		{
			this.userName.setValue(value);
		}
	}

	public string AvatarUrl
	{
		get
		{
			return this.avatarUrl.Data;
		}
		set
		{
			this.avatarUrl.setValue(value);
		}
	}

	public int CountryCode
	{
		get
		{
			return this.countryCode.Data.Value;
		}
		set
		{
			if (value >= 0)
			{
				this.countryCode.setValue(value);
			}
		}
	}

	public int Power
	{
		get
		{
			return PowerManager.Instance.TotalPower();
		}
	}

	public int WinRate
	{
		get
		{
			return Mathf.Max(this.winRate.Data, 0);
		}
		set
		{
			this.winRate.setValue(value);
		}
	}

	public int WinStreak
	{
		get
		{
			return Mathf.Max(this.winStreak.Data, 0);
		}
		set
		{
			if (-9 <= value && value <= 11)
			{
				this.winStreak.setValue(value);
			}
		}
	}

	public int Score
	{
		get
		{
			return Mathf.Max(this.score.Data, 0);
		}
		set
		{
			this.score.setValue(Mathf.Max(value, 0));
		}
	}

	public int TotalWinMatch
	{
		get
		{
			return Mathf.Max(this.totalWinMatch.Data, 0);
		}
		set
		{
			this.totalWinMatch.setValue(Mathf.Max(value, 0));
		}
	}

	public int TotalMatch
	{
		get
		{
			return Mathf.Max(this.totalMatch.Data, 0);
		}
		set
		{
			this.totalMatch.setValue(Mathf.Max(value, 0));
		}
	}

	public int Vip
	{
		get
		{
			return Mathf.Max(VipManager.Instance.LevelCurrent(), 0);
		}
	}

	public int RankLevel
	{
		get
		{
			RankInfor rankCurrentByExp = RankManager.Instance.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
			return rankCurrentByExp.Level;
		}
	}

	private StringProfileData userName;

	private StringProfileData avatarUrl;

	private IntProfileData countryCode;

	private IntProfileData winRate;

	private IntProfileData score;

	private IntProfileData winStreak;

	private IntProfileData totalWinMatch;

	private IntProfileData totalMatch;

	public int basePvpScore = 15;
}
