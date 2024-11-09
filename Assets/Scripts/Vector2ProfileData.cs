using System;
using UnityEngine;

public class Vector2ProfileData : BaseProfileDataType<Vector2>
{
	public Vector2ProfileData(string tag, Vector2 defaultValue) : base(tag, defaultValue)
	{
	}

	public Vector2 Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator Vector2(Vector2ProfileData vector2ProfileData)
	{
		return vector2ProfileData.Data;
	}

	public override void setValue(Vector2 value)
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

	protected override void load(Vector2 defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(Vector2 value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(Vector2 defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override Vector2 getFromPlayerPrefs(Vector2 defaultValue)
	{
		string text = ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag));
		Vector2 result;
		try
		{
			int num = 0;
			int num2 = text.IndexOf(',');
			float x = float.Parse(text.Substring(num, num2 - num));
			num = num2;
			num2 = text.Length;
			float y = float.Parse(text.Substring(num + 1, num2 - num - 1));
			result = new Vector2(x, y);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(Vector2 value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(value.x + "," + value.y));
	}

	protected Vector2 data;
}
