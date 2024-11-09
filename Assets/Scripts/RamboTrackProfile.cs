using System;

public class RamboTrackProfile : RamboProfile
{
	public RamboTrackProfile() : base("RamboTrack5", 5f, 12f)
	{
		this.optionProfile = new float[2][];
		this.optionProfile[0] = this.NewAddHP;
		this.optionProfile[1] = this.NewAddSpeed;
		this.DamageProfile = new FloatProfileData("metal.squad.rambotrack.damage", 26f);
		this.SpeedBulletProfile = new FloatProfileData("metal.squad.rambotrack.speedbullet", 12f);
		this.TimeFireBulletProfile = new FloatProfileData("metal.squad.rambotrack.timefire", 0.15f);
	}

	public float Damage
	{
		get
		{
			return this.DamageProfile.Data;
		}
		set
		{
			this.DamageProfile.setValue(value);
		}
	}

	public float SpeedBullet
	{
		get
		{
			return this.SpeedBulletProfile.Data;
		}
		set
		{
			this.SpeedBulletProfile.setValue(value);
		}
	}

	public float TimeFireBullet
	{
		get
		{
			return this.TimeFireBulletProfile.Data;
		}
		set
		{
			this.TimeFireBulletProfile.setValue(value);
		}
	}

	private string sex = "RamboTrack6";

	private FloatProfileData DamageProfile;

	private FloatProfileData SpeedBulletProfile;

	private FloatProfileData TimeFireBulletProfile;

	private float[] NewAddHP = new float[]
	{
		3000f
	};

	private float[] NewAddSpeed = new float[]
	{
		3f
	};
}
