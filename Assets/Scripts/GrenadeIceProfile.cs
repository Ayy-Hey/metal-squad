using System;

public class GrenadeIceProfile : GrenadeProfile
{
	public GrenadeIceProfile()
	{
		this.Name = "ICE Grenades";
		this.isVip = false;
		this.levelUpGrade = new IntProfileData("rambo.upgrade.level" + this.Name, 0);
		this.isBuy = new BoolProfileData("rambo.weapon.buy" + this.Name, true);
		this.totalBomb = new IntProfileData("rambo.weapon.totalbom" + this.Name, 0);
		this.options = new float[3][];
		this.options[0] = new float[]
		{
			130f,
			140f,
			150f,
			160f,
			170f
		};
		this.options[1] = new float[]
		{
			1.5f,
			1.8f,
			1.9f,
			2.1f,
			2.4f
		};
		this.options[2] = new float[]
		{
			3f,
			3.5f,
			4f,
			4.5f,
			5f
		};
	}
}
