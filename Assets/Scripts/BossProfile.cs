using System;

public class BossProfile
{
	public BossProfile(int id)
	{
		this.unlockModes = new BoolProfileData[3];
		this.unlockModes[0] = new BoolProfileData("metal.squad.boss_mode.unlock_normal.boss" + id, false);
		this.unlockModes[1] = new BoolProfileData("metal.squad.boss_mode.unlock_hard.boss" + id, false);
		this.unlockModes[2] = new BoolProfileData("metal.squad.boss_mode.unlock_supper_hard.boss" + id, false);
		this.receivedLuckys = new BoolProfileData[3];
		this.receivedLuckys[0] = new BoolProfileData("metal.squad.boss.normal.received.lucky" + id, false);
		this.receivedLuckys[1] = new BoolProfileData("metal.squad.boss.hard.received.lucky" + id, false);
		this.receivedLuckys[2] = new BoolProfileData("metal.squad.boss.supper_hard.received.lucky" + id, false);
		this.timesPlay = new IntProfileData("metal.squad.boss_mode.times_play.boss" + id, 0);
	}

	internal bool CheckUnlock(GameMode.Mode mode)
	{
		return this.unlockModes[(int)mode].Data;
	}

	internal bool CheckUnlock(int mode)
	{
		return this.unlockModes[mode].Data;
	}

	internal void SetUnlock(GameMode.Mode mode, bool unlocked = true)
	{
		this.unlockModes[(int)mode].setValue(unlocked);
	}

	internal bool HasLucky(GameMode.Mode mode)
	{
		return !this.receivedLuckys[(int)mode].Data;
	}

	internal void ReceivedLucky(GameMode.Mode mode)
	{
		this.receivedLuckys[(int)mode].setValue(true);
	}

	private bool CheckUnlockBoss(int bossId)
	{
		switch (bossId)
		{
		case 0:
			return ProfileManager._CampaignProfile.MapsProfile[3].GetStar(GameMode.Mode.NORMAL) > 0;
		case 1:
			return ProfileManager._CampaignProfile.MapsProfile[1].GetStar(GameMode.Mode.NORMAL) > 0;
		case 2:
			return ProfileManager._CampaignProfile.MapsProfile[5].GetStar(GameMode.Mode.NORMAL) > 0;
		case 3:
			return ProfileManager._CampaignProfile.MapsProfile[7].GetStar(GameMode.Mode.NORMAL) > 0;
		case 4:
			return ProfileManager._CampaignProfile.MapsProfile[10].GetStar(GameMode.Mode.NORMAL) > 0;
		case 5:
			return ProfileManager._CampaignProfile.MapsProfile[11].GetStar(GameMode.Mode.NORMAL) > 0;
		case 8:
			return ProfileManager._CampaignProfile.MapsProfile[14].GetStar(GameMode.Mode.NORMAL) > 0;
		case 9:
			return ProfileManager._CampaignProfile.MapsProfile[16].GetStar(GameMode.Mode.NORMAL) > 0;
		case 11:
			return ProfileManager._CampaignProfile.MapsProfile[19].GetStar(GameMode.Mode.NORMAL) > 0;
		case 12:
			return ProfileManager._CampaignProfile.MapsProfile[21].GetStar(GameMode.Mode.NORMAL) > 0;
		case 13:
			return ProfileManager._CampaignProfile.MapsProfile[23].GetStar(GameMode.Mode.NORMAL) > 0;
		case 14:
			return ProfileManager._CampaignProfile.MapsProfile[25].GetStar(GameMode.Mode.NORMAL) > 0;
		case 15:
			return ProfileManager._CampaignProfile.MapsProfile[29].GetStar(GameMode.Mode.NORMAL) > 0;
		case 16:
			return ProfileManager._CampaignProfile.MapsProfile[13].GetStar(GameMode.Mode.NORMAL) > 0;
		case 17:
			return ProfileManager._CampaignProfile.MapsProfile[17].GetStar(GameMode.Mode.NORMAL) > 0;
		}
		return false;
	}

	public BoolProfileData[] unlockModes;

	private IntProfileData timesPlay;

	private BoolProfileData[] receivedLuckys;
}
