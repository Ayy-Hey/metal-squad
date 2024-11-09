using System;
using UnityEngine;

public class DateTimeProfileData : BaseProfileDataType<DateTime>
{
	public DateTimeProfileData(string tag, DateTime defaultValue) : base(tag, defaultValue)
	{
	}

	public DateTime Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator DateTime(DateTimeProfileData dateTimeProfileData)
	{
		return dateTimeProfileData.Data;
	}

	public override void setValue(DateTime value)
	{
		if (this.data != value)
		{
			this.save(value);
		}
	}

	public override string ToString()
	{
		return this.data.ToString();
	}

	protected override void load(DateTime defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(DateTime value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(DateTime defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override DateTime getFromPlayerPrefs(DateTime defaultValue)
	{
		DateTime result;
		try
		{
			result = DateTime.Parse(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)));
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(DateTime value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.ToString()));
	}

	protected DateTime data;
}
