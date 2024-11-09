using System;
using UnityEngine;

public abstract class BaseProfileDataType<T>
{
	public BaseProfileDataType(string tag, T defaultValue)
	{
		this.encryptedTag = ProfileManager.DataEncryption.encrypt(tag);
		this.initData(defaultValue);
	}

	protected bool isHasValue()
	{
		return PlayerPrefs.HasKey(this.encryptedTag);
	}

	public abstract void setValue(T value);

	protected abstract void load(T defaultValue);

	protected abstract void save(T value);

	protected virtual void initData(T defaultValue)
	{
		if (!this.isHasValue())
		{
			this.save(defaultValue);
		}
		else
		{
			this.load(defaultValue);
		}
	}

	protected abstract T getFromPlayerPrefs(T defaultValue);

	protected abstract void saveToPlayerPrefs(T value);

	protected string encryptedTag;
}
