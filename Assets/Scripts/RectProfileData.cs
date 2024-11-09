using System;
using UnityEngine;

public class RectProfileData : BaseProfileDataType<Rect>
{
	public RectProfileData(string tag, Rect defaultValue) : base(tag, defaultValue)
	{
	}

	public Rect Data
	{
		get
		{
			return this.data;
		}
	}

	public static implicit operator Rect(RectProfileData rectProfileData)
	{
		return rectProfileData.Data;
	}

	public override void setValue(Rect value)
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

	protected override void load(Rect defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(Rect value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(Rect defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override Rect getFromPlayerPrefs(Rect defaultValue)
	{
		string text = ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag));
		Rect result;
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
			float width = float.Parse(text.Substring(num + 1, num2 - num - 1));
			num = num2;
			num2 = text.Length;
			float height = float.Parse(text.Substring(num + 1, num2 - num - 1));
			result = new Rect(x, y, width, height);
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(Rect value)
	{
		PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(string.Concat(new object[]
		{
			value.x,
			",",
			value.y,
			",",
			value.width,
			",",
			value.height
		})));
	}

	protected Rect data;
}
