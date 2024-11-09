using System;
using com.dev.util.SecurityHelper;
using UnityEngine;

public class LongProfileData : BaseProfileDataType<long>
{
	public LongProfileData(string tag, long defaultValue) : base(tag, defaultValue)
	{
	}

	public SecuredLong Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator long(LongProfileData longProfileData)
	{
		return longProfileData.Data.Value;
	}

	public override void setValue(long value)
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

	protected override void load(long defaultValue)
	{
		this.data.Value = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(long value)
	{
		this.data.Value = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(long defaultValue)
	{
		this.data = new SecuredLong(defaultValue);
		base.initData(defaultValue);
	}

	protected override long getFromPlayerPrefs(long defaultValue)
	{
		long result;
		try
		{
			result = long.Parse(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)));
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(long value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.ToString()));
	}

	protected SecuredLong data;
}
