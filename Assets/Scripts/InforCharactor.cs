using System;

public class InforCharactor
{
	public InforCharactor(string name, bool isUnLocked, string des)
	{
		this.NameProfile = new StringProfileData("rambo.data.charactor.name" + name + "1.0.4", name);
		this.isLockedProfile = new BoolProfileData("rambo.data.charactor.islocked" + name + "1.0.4", isUnLocked);
		this.isUnlockedMapProfile = new BoolProfileData("rambo.data.charactor.islockedmap" + name + "1.0.4", false);
		this.Description = des;
	}

	public string Name
	{
		get
		{
			return this.NameProfile.Data;
		}
		set
		{
			this.NameProfile.setValue(value);
		}
	}

	public bool IsUnLocked
	{
		get
		{
			return this.isLockedProfile.Data;
		}
		set
		{
			this.isLockedProfile.setValue(value);
		}
	}

	public bool IsUnlockedMap
	{
		get
		{
			return this.isUnlockedMapProfile.Data;
		}
		set
		{
			this.isUnlockedMapProfile.setValue(value);
		}
	}

	private StringProfileData NameProfile;

	private BoolProfileData isLockedProfile;

	public string Description;

	public string from;

	public string weapon;

	private BoolProfileData isUnlockedMapProfile;
}
