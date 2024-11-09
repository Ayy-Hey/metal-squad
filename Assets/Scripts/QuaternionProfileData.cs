using System;
using UnityEngine;

public class QuaternionProfileData : BaseProfileDataType<Quaternion>
{
	public QuaternionProfileData(string tag, Quaternion defaultValue) : base(tag, defaultValue)
	{
	}

	public Quaternion Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator Quaternion(QuaternionProfileData quaternionProfileData)
	{
		return quaternionProfileData.Data;
	}

	public override void setValue(Quaternion value)
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

	protected override void load(Quaternion defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(Quaternion value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(Quaternion defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override Quaternion getFromPlayerPrefs(Quaternion defaultValue)
	{
		string text = ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag));
		Quaternion result;
		try
		{
			int num = 0;
			int num2 = text.IndexOf(',');
			float x = float.Parse(text.Substring(num, num2 - num));
			num = num2;
			num2 = text.IndexOf(',', num + 1);
			float y = float.Parse(text.Substring(num + 1, num2 - num - 1));
			num = num2;
			num2 = text.IndexOf(',', num + 1);
			float z = float.Parse(text.Substring(num + 1, num2 - num - 1));
			num = num2;
			num2 = text.Length;
			float w = float.Parse(text.Substring(num + 1, num2 - num - 1));
			result = new Quaternion(x, y, z, w);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(Quaternion value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(string.Concat(new object[]
		{
			value.x,
			",",
			value.y,
			",",
			value.z,
			",",
			value.w
		})));
	}

	protected Quaternion data;
}
