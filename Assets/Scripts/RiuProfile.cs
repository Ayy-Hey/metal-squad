using System;

public class RiuProfile : MeleProfile
{
	public RiuProfile()
	{
		this.Name = "Silver Axe";
		this.range = new int[]
		{
			90,
			90,
			90,
			90,
			90,
			90,
			90,
			90,
			90,
			90,
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
			110
		};
		this.speed = new int[]
		{
			50,
			50,
			50,
			50,
			50,
			50,
			50,
			50,
			50,
			50,
			60,
			60,
			60,
			60,
			60,
			60,
			60,
			60,
			60,
			60,
			70,
			70,
			70,
			70,
			70,
			70,
			70,
			70,
			70,
			70
		};
		this.unlock = new BoolProfileData(this.STR_UnLock + this.Name, false);
		this.levelUpGradeMelee = new IntProfileData(this.STR_Level + this.Name, 0);
	}
}
