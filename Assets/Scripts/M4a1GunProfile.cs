using System;

public class M4a1GunProfile : WeaponProfile
{
	public M4a1GunProfile()
	{
		this.Total_Bullet_Profile = new IntProfileData("rambo.weapon.profile.totalbullet" + this.name + "1.0.4", 9999);
		this.Radius_Damage_Profile = 0.5f;
		this.LevelUpgrade = new IntProfileData("rambo.upgrade.level" + this.name + "1.0.4", 0);
		this.RankUpped = new IntProfileData("rambo.upgrade.rank" + this.name + "1.0.4", 0);
		this.Gun_Buy = new BoolProfileData("rambo.weapon.buy" + this.name + "1.0.4", true);
		this.TimeToCompleteUpgrade = new IntProfileData("rambo.weapon.time.complete.upgrade" + this.name + "1.0.4", 0);
		this.Sale_Time = new IntProfileData("rambo.weapon.saletime" + this.name + "1.0.4", 0);
		this.Dont_Show_Sale = new BoolProfileData("rambo.weapon.dontshowsale" + this.name + "1.0.4", false);
		this.Gun_Value = new M4a1Value();
	}

	private string name = "M4a1GunProfile";
}
