using System;

public class RamboBoyProfile : RamboProfile
{
	public RamboBoyProfile() : base("John D", 5f, 12f)
	{
		this.optionProfile = new float[3][];
		this.Speed_Sit_Profile = new IntProfileData("rambo_speed_sit" + this.sex + "1.0.4", 200);
		this.Knife_Damage_Profile = new FloatProfileData("rambo_knife_damage" + this.sex + "1.0.4", 50f);
		this.grenades = new GrenadesProfile(this.sex);
	}

	private string sex = "John D";
}
