using System;
using com.dev.util.SecurityHelper;
using UnityEngine;

public class IntProfileData : BaseProfileDataType<int>
{
	public IntProfileData(string tag, int defaultValue) : base(tag, defaultValue)
	{
	}

	public SecuredInt Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator int(IntProfileData intProfileData)
	{
		return intProfileData.Data.Value;
	}

	public override void setValue(int value)
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

	protected override void load(int defaultValue)
	{
		this.data.Value = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(int value)
	{
		this.data.Value = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(int defaultValue)
	{
		this.data = new SecuredInt(defaultValue);
		base.initData(defaultValue);
	}

	protected override int getFromPlayerPrefs(int defaultValue)
	{
		int result;
		try
		{
			result = int.Parse(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)));
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(int value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.ToString()));
	}

	protected SecuredInt data;
}
