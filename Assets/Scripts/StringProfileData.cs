using System;
using UnityEngine;

public class StringProfileData : BaseProfileDataType<string>
{
	public StringProfileData(string tag, string defaultValue) : base(tag, defaultValue)
	{
	}

	public string Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator string(StringProfileData stringProfileData)
	{
		return stringProfileData.Data;
	}

	public override void setValue(string value)
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

	protected override void load(string defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(string value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override string getFromPlayerPrefs(string defaultValue)
	{
		string result;
		try
		{
			result = ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag));
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(string value)
	{
		if (value != null)
		{
			PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value));
		}
	}

	protected string data;
}
