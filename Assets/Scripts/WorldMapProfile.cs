using System;

public class WorldMapProfile
{
	public WorldMapProfile(int LevelMap)
	{
		this.LevelMap = LevelMap;
		this.Name = this.Name;
		this.IsClaimGiftNormal = new BoolProfileData[3];
		this.IsClaimGiftNormal[0] = new BoolProfileData("com.metal.squad.WorldMapProfile.Normal.0.levelmap." + LevelMap, false);
		this.IsClaimGiftNormal[1] = new BoolProfileData("com.metal.squad.WorldMapProfile.Normal.1.levelmap." + LevelMap, false);
		this.IsClaimGiftNormal[2] = new BoolProfileData("com.metal.squad.WorldMapProfile.Normal.2.levelmap." + LevelMap, false);
		this.IsClaimGiftHard = new BoolProfileData[3];
		this.IsClaimGiftHard[0] = new BoolProfileData("com.metal.squad.WorldMapProfile.Hard.0.levelmap." + LevelMap, false);
		this.IsClaimGiftHard[1] = new BoolProfileData("com.metal.squad.WorldMapProfile.Hard.1.levelmap." + LevelMap, false);
		this.IsClaimGiftHard[2] = new BoolProfileData("com.metal.squad.WorldMapProfile.Hard.2.levelmap." + LevelMap, false);
		this.IsClaimGiftSuperHard = new BoolProfileData[3];
		this.IsClaimGiftSuperHard[0] = new BoolProfileData("com.metal.squad.WorldMapProfile.SuperHard.0.levelmap." + LevelMap, false);
		this.IsClaimGiftSuperHard[1] = new BoolProfileData("com.metal.squad.WorldMapProfile.SuperHard.1.levelmap." + LevelMap, false);
		this.IsClaimGiftSuperHard[2] = new BoolProfileData("com.metal.squad.WorldMapProfile.SuperHard.2.levelmap." + LevelMap, false);
	}

	public bool GetClaimedGiftNormal(int id)
	{
		return this.IsClaimGiftNormal[id].Data;
	}

	public void SetClaimedGiftNormal(int id, bool unlock = true)
	{
		this.IsClaimGiftNormal[id].setValue(unlock);
	}

	public bool GetClaimedGiftHard(int id)
	{
		return this.IsClaimGiftHard[id].Data;
	}

	public void SetClaimedGiftHard(int id, bool unlock = true)
	{
		this.IsClaimGiftHard[id].setValue(unlock);
	}

	public bool GetClaimedGiftSuperHard(int id)
	{
		return this.IsClaimGiftSuperHard[id].Data;
	}

	public void SetClaimedGiftSuperHard(int id, bool unlock = true)
	{
		this.IsClaimGiftSuperHard[id].setValue(unlock);
	}

	public int LevelMap;

	public string Name;

	private BoolProfileData[] IsClaimGiftNormal;

	private BoolProfileData[] IsClaimGiftHard;

	private BoolProfileData[] IsClaimGiftSuperHard;

	public int RequireStar;
}
