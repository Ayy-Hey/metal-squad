using System;
using UnityEngine;

public class PartOfGunProtile
{
	public PartOfGunProtile()
	{
		this.partOfPrimarys = new IntProfileData[5];
		this.partOfSpecials = new IntProfileData[5];
		this.partOfKnifes = new IntProfileData[2];
		for (int i = 0; i < 5; i++)
		{
			this.partOfPrimarys[i] = new IntProfileData("com.metal.squad.part.primary.gun" + i, 0);
			this.partOfSpecials[i] = new IntProfileData("com.metal.squad.part.special.gun" + i, 0);
		}
		for (int j = 0; j < 2; j++)
		{
			this.partOfKnifes[j] = new IntProfileData("com.metal.squad.part.knife" + j, 0);
		}
	}

	public int GetPartOfPrimary(int primaryId)
	{
		return this.partOfPrimarys[primaryId].Data.Value;
	}

	public void SetPartOfPrimary(int primaryId, int part)
	{
		part += this.partOfPrimarys[primaryId].Data.Value;
		if (ProfileManager.weaponsRifle[primaryId].GetGunBuy())
		{
			int num = ProfileManager.weaponsRifle[primaryId].GetLevelUpgrade() + part;
			num = Mathf.Min(num, 29);
			ProfileManager.weaponsRifle[primaryId].SetLevelUpgrade(num);
			this.partOfPrimarys[primaryId].setValue(0);
		}
		else
		{
			this.partOfPrimarys[primaryId].setValue(part);
		}
	}

	public int GetPartOfSpecial(int specialId)
	{
		return this.partOfSpecials[specialId].Data.Value;
	}

	public void SetPartOfSpecial(int specialId, int part)
	{
		part += this.partOfSpecials[specialId].Data.Value;
		if (ProfileManager.weaponsSpecial[specialId].GetGunBuy())
		{
			int num = ProfileManager.weaponsSpecial[specialId].GetLevelUpgrade() + part;
			num = Mathf.Min(num, 29);
			ProfileManager.weaponsSpecial[specialId].SetLevelUpgrade(num);
			this.partOfSpecials[specialId].setValue(0);
		}
		else
		{
			this.partOfSpecials[specialId].setValue(part);
		}
	}

	private IntProfileData[] partOfPrimarys;

	private IntProfileData[] partOfSpecials;

	private IntProfileData[] partOfKnifes;
}
