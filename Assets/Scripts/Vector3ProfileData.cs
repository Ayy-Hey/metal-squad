using System;
using UnityEngine;

public class Vector3ProfileData : BaseProfileDataType<Vector3>
{
	public Vector3ProfileData(string tag, Vector3 defaultValue) : base(tag, defaultValue)
	{
	}

	public Vector3 Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator Vector3(Vector3ProfileData vector3ProfileData)
	{
		return vector3ProfileData.Data;
	}

	public override void setValue(Vector3 value)
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

	protected override void load(Vector3 defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(Vector3 value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(Vector3 defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override Vector3 getFromPlayerPrefs(Vector3 defaultValue)
	{
		string text = ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag));
		Vector3 result;
		try
		{
			int num = 0;
			int num2 = text.IndexOf(',');
			float x = float.Parse(text.Substring(num, num2 - num));
			num = num2;
			num2 = text.IndexOf(',', num + 1);
			float y = float.Parse(text.Substring(num + 1, num2 - num - 1));
			num = num2;
			num2 = text.Length;
			float z = float.Parse(text.Substring(num + 1, num2 - num - 1));
			result = new Vector3(x, y, z);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(Vector3 value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(string.Concat(new object[]
		{
			value.x,
			",",
			value.y,
			",",
			value.z
		})));
	}

	protected Vector3 data;
}
