using System;

public class GrenadeFireProfile : GrenadeProfile
{
	public GrenadeFireProfile()
	{
		this.Name = "Molotov Cocktail";
		this.isVip = false;
		this.levelUpGrade = new IntProfileData("rambo.upgrade.level" + this.Name, 0);
		this.isBuy = new BoolProfileData("rambo.weapon.buy" + this.Name, true);
		this.totalBomb = new IntProfileData("rambo.weapon.totalbom" + this.Name, 0);
		this.options = new float[3][];
		this.options[0] = new float[]
		{
			180f,
			190f,
			200f,
			210f,
			220f
		};
		this.options[1] = new float[]
		{
			1.6f,
			1.9f,
			2f,
			2.2f,
			2.5f
		};
		this.options[2] = new float[]
		{
			3.5f,
			4f,
			4.5f,
			5f,
			5.5f
		};
	}
}
