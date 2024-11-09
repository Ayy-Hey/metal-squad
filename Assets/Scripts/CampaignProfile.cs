using System;

public class CampaignProfile
{
	public CampaignProfile()
	{
		for (int i = 0; i < this.MapsProfile.Length; i++)
		{
			this.MapsProfile[i] = new MapProfile(i);
		}
		this.MapsProfile[0].SetUnlocked(GameMode.Mode.NORMAL, true);
		if (SplashScreen._isBuildMarketing)
		{
			for (int j = 0; j < this.MapsProfile.Length; j++)
			{
				this.MapsProfile[j].SetStar(GameMode.Mode.NORMAL, 3);
			}
		}
	}

	public int GetTotalStar
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.MapsProfile.Length; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					num += this.MapsProfile[i].GetStar((GameMode.Mode)j);
				}
			}
			return num;
		}
	}

	public int GetTotalStarInMap(int map)
	{
		int num = 0;
		for (int i = map * 12; i < (map + 1) * 12; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				num += this.MapsProfile[i].GetStar((GameMode.Mode)j);
			}
		}
		return num;
	}

	public int GetTotalStarInMapMode(int map, GameMode.Mode mode)
	{
		int num = 0;
		for (int i = map * 12; i < (map + 1) * 12; i++)
		{
			num += this.MapsProfile[i].GetStar(mode);
		}
		return num;
	}

	public MapProfile[] MapsProfile = new MapProfile[60];
}
