using System;
using System.Globalization;
using com.dev.util.SecurityHelper;
using UnityEngine;

public class DoubleProfileData : BaseProfileDataType<double>
{
	public DoubleProfileData(string tag, double defaultValue) : base(tag, defaultValue)
	{
	}

	public SecuredDouble Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator double(DoubleProfileData doubleProfileData)
	{
		return doubleProfileData.Data;
	}

	public override void setValue(double value)
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

	protected override void load(double defaultValue)
	{
		this.data.Value = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(double value)
	{
		this.data.Value = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(double defaultValue)
	{
		this.data = new SecuredDouble(defaultValue);
		base.initData(defaultValue);
	}

	protected override double getFromPlayerPrefs(double defaultValue)
	{
		double result;
		try
		{
			result = double.Parse(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)), CultureInfo.InvariantCulture);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(double value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.ToString()));
	}

	protected SecuredDouble data;
}
