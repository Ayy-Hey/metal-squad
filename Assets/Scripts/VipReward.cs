using System;

public class VipReward
{
	public int ReducedTimeSpin { get; set; }

	public int MedkitMax { get; set; }

	public int GemBonusBossInCampaign { get; set; }

	public float ReducedPriceTicketSpin { get; set; }

	public int AddReviveWithGem { get; set; }

	public int GemBonusAchievement { get; set; }

	public int GetAmountOfItem(Item gift)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i] == (int)gift)
			{
				return this.amounts[i];
			}
		}
		return -1;
	}

	public int[] items;

	public int[] amounts;

	public bool isX2RewarDailyGift;

	public bool isX2RewardDailyQuest;

	public bool isX2Achievement;

	public bool isX2RewardBossMode;
}
