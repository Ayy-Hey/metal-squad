using System;

public class BossModeProfile
{
	public BossModeProfile()
	{
		int length = Enum.GetValues(typeof(EBoss)).Length;
		this.bossReward = new BossModeReward[length];
		for (int i = 0; i < this.bossReward.Length; i++)
		{
			this.bossReward[i] = new BossModeReward(i);
		}
		this.bossProfiles = new BossProfile[length];
		for (int j = 0; j < this.bossProfiles.Length; j++)
		{
			this.bossProfiles[j] = new BossProfile(j);
		}
		this.winCount = new IntProfileData[3];
		this.playCount = new IntProfileData[3];
		for (int k = 0; k < 3; k++)
		{
			this.winCount[k] = new IntProfileData("metal.squad.bossmode.win.count." + k, 0);
			this.playCount[k] = new IntProfileData("metal.squad.bossmode.play.count." + k, 0);
		}
	}

	internal int GetWinCount(GameMode.Mode mode)
	{
		return this.winCount[(int)mode].Data.Value;
	}

	internal void UpWinCount(GameMode.Mode mode)
	{
		this.winCount[(int)mode].setValue(this.GetWinCount(mode) + 1);
	}

	internal int GetPlayCount(GameMode.Mode mode)
	{
		return this.playCount[(int)mode].Data.Value;
	}

	internal void UpPlayCount(GameMode.Mode mode)
	{
		this.playCount[(int)mode].setValue(this.GetPlayCount(mode) + 1);
	}

	public BossProfile[] bossProfiles;

	public ETypeWeapon gunType;

	private IntProfileData[] winCount;

	private IntProfileData[] playCount;

	public BossModeReward[] bossReward;
}
