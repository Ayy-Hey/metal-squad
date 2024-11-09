using System;
using UnityEngine;

public class Vector4ProfileData : BaseProfileDataType<Vector4>
{
	public Vector4ProfileData(string tag, Vector4 defaultValue) : base(tag, defaultValue)
	{
	}

	public Vector4 Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator Vector4(Vector4ProfileData vector4ProfileData)
	{
		return vector4ProfileData.Data;
	}

	public override void setValue(Vector4 value)
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

	protected override void load(Vector4 defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(Vector4 value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(Vector4 defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override Vector4 getFromPlayerPrefs(Vector4 defaultValue)
	{
		string text = ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag));
		Vector4 result;
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
			result = new Vector4(x, y, z, w);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(Vector4 value)
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

	protected Vector4 data;
}
