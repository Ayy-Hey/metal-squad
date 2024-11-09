using System;
using UnityEngine;

public class BoolProfileData : BaseProfileDataType<bool>
{
	public BoolProfileData(string tag, bool defaultValue) : base(tag, defaultValue)
	{
	}

	public bool Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator bool(BoolProfileData boolProfileData)
	{
		return boolProfileData.Data;
	}

	public override void setValue(bool value)
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

	protected override void load(bool defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(bool value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override bool getFromPlayerPrefs(bool value)
	{
		bool result;
		try
		{
			result = bool.Parse(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)));
		}
		catch
		{
			return value;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(bool value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.ToString()));
	}

	protected bool data;
}
