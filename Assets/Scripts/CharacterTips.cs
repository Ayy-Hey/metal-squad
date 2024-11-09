using System;
using UnityEngine;

public class CharacterTips
{
	public static CharacterTips Instance
	{
		get
		{
			if (CharacterTips.instance == null)
			{
				CharacterTips.instance = new CharacterTips();
			}
			return CharacterTips.instance;
		}
	}

	public bool[] GetLisEnableTips()
	{
		int ms = ProfileManager.userProfile.Ms;
		int coin = ProfileManager.userProfile.Coin;
		bool[] array = new bool[ProfileManager.rambos.Length + 1];
		bool flag = false;
		Item itemID = ProfileManager.rambos[0].GetItemID(ProfileManager.rambos[0].LevelUpgrade + 1);
		int num = ProfileManager.rambos[0].ValueUpgrade(ProfileManager.rambos[0].LevelUpgrade + 1);
		if (itemID != Item.Gold)
		{
			if (itemID == Item.Gem)
			{
				flag = (ProfileManager.userProfile.Ms >= num);
			}
		}
		else
		{
			flag = (ProfileManager.userProfile.Coin >= num);
		}
		for (int i = 0; i < ProfileManager.InforChars.Length; i++)
		{
			if (ProfileManager.InforChars[i].IsUnLocked)
			{
				int num2 = ProfileManager.rambos[i].LevelUpgrade + 1;
				if (num2 >= 0 && num2 < (ProfileManager.rambos[i].RankUpped + 1) * 10 && flag)
				{
					array[0] = true;
					array[i + 1] = true;
				}
				if (ProfileManager.rambos[i].CostRankUp[Mathf.Min(1, ProfileManager.rambos[i].RankUpped)] > 0 && ProfileManager.rambos[i].RankUpped < 2 && ProfileManager.rambos[i].RankUpped == num2 / 10 && num2 % 10 == 9 && PlayerPrefs.GetInt("metal.squad.frag." + ProfileManager.rambos[i].CurrencyRankUp.ToString(), 0) / ProfileManager.rambos[i].CostRankUp[Mathf.Min(1, ProfileManager.rambos[i].RankUpped)] >= 1)
				{
					array[0] = true;
					array[i + 1] = true;
				}
			}
			else if ((ProfileManager.rambos[i].StarUnlock <= ProfileManager._CampaignProfile.GetTotalStar && ProfileManager.rambos[i].GoldUnlock.Value <= PlayerPrefs.GetInt("metal.squad.frag." + ProfileManager.rambos[i].CurrencyUnlock.ToString(), 0)) || ProfileManager.rambos[i].GemUnlock.Value <= ms)
			{
				array[0] = true;
				array[i + 1] = true;
			}
		}
		return array;
	}

	private static CharacterTips instance;
}
