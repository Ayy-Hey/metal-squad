using System;

public class GiftProfile
{
	public GiftProfile(int day, int[] Gift, int[] value)
	{
		this.DoneProfile = new BoolProfileData("com.rambo.metal.squad.dailygift.isdone" + day, false);
		this.dateTimeProfile = new DateTimeProfileData("com.rambo.metal.squad.dailygift.datetime" + day, DateTime.MinValue);
		this.Gift = Gift;
		this.value = value;
		this.day = day;
	}

	public bool Done
	{
		get
		{
			return this.DoneProfile.Data;
		}
		set
		{
			this.DoneProfile.setValue(value);
		}
	}

	public DateTime dateTime
	{
		get
		{
			return this.dateTimeProfile.Data;
		}
		set
		{
			this.dateTimeProfile.setValue(value);
		}
	}

	private BoolProfileData DoneProfile;

	public int[] Gift;

	public int[] value;

	public int day;

	private DateTimeProfileData dateTimeProfile;
}
