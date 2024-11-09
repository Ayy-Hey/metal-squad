using System;

public class BuaGoProfile : MeleProfile
{
	public BuaGoProfile()
	{
		this.Name = "Hammer";
		this.range = new int[]
		{
			80,
			80,
			80,
			80,
			80,
			80,
			80,
			80,
			80,
			80,
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
			100
		};
		this.speed = new int[]
		{
			40,
			40,
			40,
			40,
			40,
			40,
			40,
			40,
			40,
			40,
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
			60
		};
		this.unlock = new BoolProfileData(this.STR_UnLock + this.Name, true);
		this.levelUpGradeMelee = new IntProfileData(this.STR_Level + this.Name, 0);
	}
}
