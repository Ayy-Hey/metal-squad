using System;

public class SettingProfile
{
	public SettingProfile()
	{
		this.IdCharProfile = new IntProfileData("id.char.rambo.data", 0);
		this.IsMusicProfile = new BoolProfileData("music.rambo.data", true);
		this.IsSoundProfile = new BoolProfileData("sound.rambo.data", true);
		this.hasSkillBoost = new BoolProfileData("rambo.booster.skillboost", false);
		this.hasSpeedBoost = new BoolProfileData("rambo.booster.speedboost", false);
		this.hasHealthBoost = new BoolProfileData("rambo.booster.healthboost", false);
		this.hasDamageBoost = new BoolProfileData("rambo.booster.damageboost", false);
		this.TypeControlProfile = new IntProfileData("metal.squad.typecontrol.2", 2);
	}

	public int TypeControl
	{
		get
		{
			return this.TypeControlProfile.Data.Value;
		}
		set
		{
			this.TypeControlProfile.setValue(value);
		}
	}

	public int IDChar
	{
		get
		{
			return this.IdCharProfile.Data;
		}
		set
		{
			this.IdCharProfile.setValue(value);
		}
	}

	public bool IsMusic
	{
		get
		{
			return this.IsMusicProfile.Data;
		}
		set
		{
			this.IsMusicProfile.setValue(value);
		}
	}

	public bool IsSound
	{
		get
		{
			return this.IsSoundProfile.Data;
		}
		set
		{
			this.IsSoundProfile.setValue(value);
		}
	}

	public bool HasSkillBoost
	{
		get
		{
			return this.hasSkillBoost.Data;
		}
		set
		{
			this.hasSkillBoost.setValue(value);
		}
	}

	public bool HasSpeedBoost
	{
		get
		{
			return this.hasSpeedBoost.Data;
		}
		set
		{
			this.hasSpeedBoost.setValue(value);
		}
	}

	public bool HasHealthBoost
	{
		get
		{
			return this.hasHealthBoost.Data;
		}
		set
		{
			this.hasHealthBoost.setValue(value);
		}
	}

	public bool HasDamageBoost
	{
		get
		{
			return this.hasDamageBoost;
		}
		set
		{
			this.hasDamageBoost.setValue(value);
		}
	}

	private IntProfileData IdCharProfile;

	private BoolProfileData IsMusicProfile;

	private BoolProfileData IsSoundProfile;

	private BoolProfileData hasSkillBoost;

	private BoolProfileData hasSpeedBoost;

	private BoolProfileData hasHealthBoost;

	private BoolProfileData hasDamageBoost;

	private IntProfileData TypeControlProfile;

	public int bournMS = 9;
}
