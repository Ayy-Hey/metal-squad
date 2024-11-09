using System;

public class KiemProfile : MeleProfile
{
	public KiemProfile()
	{
		this.Name = "Sword";
		this.range = new int[]
		{
			100,
			100,
			100,
			100,
			100,
			100,
			100,
			100,
			100,
			100,
			110,
			110,
			110,
			110,
			110,
			110,
			110,
			110,
			110,
			110,
			120,
			120,
			120,
			120,
			120,
			120,
			120,
			120,
			120,
			120
		};
		this.speed = new int[]
		{
			55,
			55,
			55,
			55,
			55,
			55,
			55,
			55,
			55,
			55,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			75,
			75,
			75,
			75,
			75,
			75,
			75,
			75,
			75,
			75
		};
		this.unlock = new BoolProfileData(this.STR_UnLock + this.Name, false);
		this.levelUpGradeMelee = new IntProfileData(this.STR_Level + this.Name, 0);
	}
}
