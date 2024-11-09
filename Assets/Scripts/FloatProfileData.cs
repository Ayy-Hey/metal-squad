using System;
using System.Globalization;
using com.dev.util.SecurityHelper;
using UnityEngine;

public class FloatProfileData : BaseProfileDataType<float>
{
	public FloatProfileData(string tag, float defaultValue) : base(tag, defaultValue)
	{
	}

	public SecuredFloat Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator float(FloatProfileData floatProfileData)
	{
		return floatProfileData.Data;
	}

	public override void setValue(float value)
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

	protected override void load(float defaultValue)
	{
		this.data.Value = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(float value)
	{
		this.data.Value = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(float defaultValue)
	{
		this.data = new SecuredFloat(defaultValue);
		base.initData(defaultValue);
	}

	protected override float getFromPlayerPrefs(float defaultValue)
	{
		float result;
		try
		{
			result = float.Parse(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)), CultureInfo.InvariantCulture);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(float value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.ToString()));
	}

	protected SecuredFloat data;
}
