using System;

public class BossModeReward
{
	public BossModeReward(int id)
	{
		this.StateGetReward = new IntProfileData("com.sore.metal.squad.BossModeReward.StateGetReward." + id, 0);
		this.timesGetGem = new IntProfileData[3];
		for (int i = 0; i < this.timesGetGem.Length; i++)
		{
			this.timesGetGem[i] = new IntProfileData(string.Concat(new object[]
			{
				"com.sore.metal.squad.BossModeReward.timesGetGem.",
				id,
				".",
				i
			}), 0);
		}
	}

	public int StateReward
	{
		get
		{
			return this.StateGetReward.Data.Value;
		}
		set
		{
			this.StateGetReward.setValue(value);
		}
	}

	public int GetTimesGemReward(int mode)
	{
		return this.timesGetGem[mode].Data.Value;
	}

	public void AddTimesGemReward(int mode)
	{
		this.timesGetGem[mode].setValue(this.GetTimesGemReward(mode) + 1);
	}

	private IntProfileData StateGetReward;

	private IntProfileData[] timesGetGem;
}
