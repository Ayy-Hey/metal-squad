using System;

public class ThunderShotGunProfile : WeaponProfile
{
	public ThunderShotGunProfile(string demo = "")
	{
		this.Total_Bullet_Profile = new IntProfileData("rambo.weapon.profile.totalbullet" + this.name + "1.0.4" + demo, 30);
		this.Radius_Damage_Profile = 0.5f;
		this.LevelUpgrade = new IntProfileData("rambo.upgrade.level" + this.name + "1.0.4" + demo, 0);
		this.RankUpped = new IntProfileData("rambo.upgrade.rank" + this.name + "1.0.4" + demo, 0);
		this.Gun_Buy = new BoolProfileData("rambo.weapon.buy" + this.name + "1.0.4" + demo, false);
		this.TimeToCompleteUpgrade = new IntProfileData("rambo.weapon.time.complete.upgrade" + this.name + "1.0.4" + demo, 0);
		this.Sale_Time = new IntProfileData("rambo.weapon.saletime" + this.name + "1.0.4" + demo, 0);
		this.Dont_Show_Sale = new BoolProfileData("rambo.weapon.dontshowsale" + this.name + "1.0.4" + demo, false);
		this.Gun_Value = new ThunderValue();
	}

	private string name = "ThunderShotGunProfile";
}
