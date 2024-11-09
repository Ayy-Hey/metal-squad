using System;
using Rank;
using UnityEngine;

public class RankManager
{
	public static RankManager Instance
	{
		get
		{
			if (RankManager.instance == null)
			{
				RankManager.instance = new RankManager();
			}
			return RankManager.instance;
		}
	}

	public RankInfor GetRankInfoByLevel(int level)
	{
		level = Mathf.Min(level, DataLoader.rankInfor.Length - 1);
		return DataLoader.rankInfor[level];
	}

	public string ExpCurrent()
	{
		RankInfor rankCurrentByExp = this.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
		RankInfor rankInfoByLevel = this.GetRankInfoByLevel(rankCurrentByExp.Level + 1);
		int num = ProfileManager.ExpRankProfile.Data.Value - rankCurrentByExp.Exp;
		int num2 = rankInfoByLevel.Exp - rankCurrentByExp.Exp;
		if (num2 == 0)
		{
			return "MAX";
		}
		return num + "/" + num2;
	}

	public float ExpCurrent01()
	{
		RankInfor rankCurrentByExp = this.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
		RankInfor rankInfoByLevel = this.GetRankInfoByLevel(rankCurrentByExp.Level + 1);
		int num = ProfileManager.ExpRankProfile.Data.Value - rankCurrentByExp.Exp;
		int num2 = rankInfoByLevel.Exp - rankCurrentByExp.Exp;
		return Mathf.Clamp01((float)num / (float)num2);
	}

	public string ExpCurrent(int exp, RankInfor rankCurrent, RankInfor ranknext)
	{
		int num = Mathf.Max(0, exp - rankCurrent.Exp);
		int num2 = ranknext.Exp - rankCurrent.Exp;
		if (num2 == 0)
		{
			return "MAX";
		}
		return num + "/" + num2;
	}

	public float ExpCurrent01(int exp, RankInfor rankCurrent, RankInfor ranknext)
	{
		if (rankCurrent.Level == ranknext.Level)
		{
			return 1f;
		}
		int num = exp - rankCurrent.Exp;
		int num2 = ranknext.Exp - rankCurrent.Exp;
		return Mathf.Clamp01((float)num / (float)num2);
	}

	public void AddExp(int exp, Action<bool> callback = null)
	{
		if (ProfileManager.unlockAll)
		{
			exp = 500;
		}
		ProfileManager.ExpRankProfile.setValue(ProfileManager.ExpRankProfile.Data.Value + exp);
		if (ProfileManager.LevelRankClaimedProfile.Data.Value > DataLoader.rankInfor.Length - 1)
		{
			return;
		}
		RankInfor rankCurrentByExp = this.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
		if (ProfileManager.LevelRankClaimedProfile.Data.Value < rankCurrentByExp.Level)
		{
			ProfileManager.LevelRankClaimedProfile.setValue(ProfileManager.LevelRankClaimedProfile.Data.Value + 1);
			PopupManager.Instance.ShowRankRewardPopup(rankCurrentByExp, delegate(bool cb)
			{
				if (callback != null && cb)
				{
					callback(true);
				}
			});
		}
	}

	public int GetTotalExp()
	{
		return ProfileManager.ExpRankProfile.Data.Value;
	}

	public RankInfor GetRankCurrentByExp(int totalExp)
	{
		int num = -1;
		for (int i = 0; i < DataLoader.rankInfor.Length; i++)
		{
			int num2 = totalExp - DataLoader.rankInfor[i].Exp;
			if (num2 < 0)
			{
				break;
			}
			num++;
		}
		return DataLoader.rankInfor[Mathf.Clamp(num, 0, DataLoader.rankInfor.Length - 1)];
	}

	private static RankManager instance;
}
