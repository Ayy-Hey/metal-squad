using System;

public class GrenadesProfile
{
	public GrenadesProfile(string sex)
	{
		this.Time_Reload_Bomb_Profile = new FloatProfileData("rambo_time_reload" + sex + "1.0.4", 0.5f);
		this.Force_Bomb_Profile = new IntProfileData("rambo_force_bomb" + sex + "1.0.4", 400);
		this.Damage_Bomb_Profile = new IntProfileData("rambo_damage_bomb" + sex + "1.0.4", 100);
		this.Total_Bomb_Profile = new IntProfileData("rambo_total_bomb" + sex + "1.0.4", 10);
		this.Radius_Bomb_Profile = new FloatProfileData("rambo_radius_bomb1.0.4", 1.2f);
	}

	public float Time_Raload_Bomb
	{
		get
		{
			return this.Time_Reload_Bomb_Profile.Data;
		}
	}

	public int Force_Bomb
	{
		get
		{
			return this.Force_Bomb_Profile.Data;
		}
	}

	public int Damage_Bomb
	{
		get
		{
			return this.Damage_Bomb_Profile.Data;
		}
		set
		{
			this.Damage_Bomb_Profile.setValue(value);
		}
	}

	public float Radius_Bomb
	{
		get
		{
			return this.Radius_Bomb_Profile.Data;
		}
	}

	public const string STR_Time_Reload_Bomb = "rambo_time_reload";

	public const string STR_Force_Bomb = "rambo_force_bomb";

	public const string STR_Damage_Bomb = "rambo_damage_bomb";

	public const string STR_Total_Bomb = "rambo_total_bomb";

	public const string STR_Radius_Bomb = "rambo_radius_bomb";

	private FloatProfileData Time_Reload_Bomb_Profile;

	private IntProfileData Force_Bomb_Profile;

	private IntProfileData Damage_Bomb_Profile;

	private IntProfileData Total_Bomb_Profile;

	private FloatProfileData Radius_Bomb_Profile;
}
