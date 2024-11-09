using System;
using Newtonsoft.Json;
using UnityEngine;

public class CustomProfileData : BaseProfileDataType<object>
{
	public CustomProfileData(string tag, object defaultValue) : base(tag, defaultValue)
	{
	}

	public object Data
	{
		get
		{
			return this.data;
		}
	}

	public override void setValue(object value)
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

	protected override void load(object defaultValue)
	{
		this.data = this.getFromPlayerPrefs(defaultValue);
	}

	protected override void save(object value)
	{
		this.data = value;
		this.saveToPlayerPrefs(value);
	}

	protected override void initData(object defaultValue)
	{
		this.data = defaultValue;
		base.initData(defaultValue);
	}

	protected override object getFromPlayerPrefs(object defaultValue)
	{
		object result;
		try
		{
			result = JsonConvert.DeserializeObject<object>(ProfileManager.DataEncryption.decrypt(PlayerPrefs.GetString(this.encryptedTag)));
		}
		catch
		{
			return defaultValue;
		}
		return result;
	}

	protected override void saveToPlayerPrefs(object value)
	{
		if (value != null)
		{
			PlayerPrefs.SetString(this.encryptedTag, ProfileManager.DataEncryption.encrypt(JsonConvert.SerializeObject(value)));
		}
	}

	protected object data;
}
