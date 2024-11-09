using System;

public class GrenadeSmokeProfile : GrenadeProfile
{
	public GrenadeSmokeProfile()
	{
		this.Name = "Chemical Bombs";
		this.isVip = false;
		this.levelUpGrade = new IntProfileData("rambo.upgrade.level" + this.Name, 0);
		this.isBuy = new BoolProfileData("rambo.weapon.buy" + this.Name, true);
		this.totalBomb = new IntProfileData("rambo.weapon.totalbom" + this.Name, 0);
		this.options = new float[3][];
		this.options[0] = new float[]
		{
			230f,
			240f,
			250f,
			260f,
			270f
		};
		this.options[1] = new float[]
		{
			1.66f,
			1.95f,
			2.15f,
			2.35f,
			2.8f
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
