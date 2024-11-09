using System;

public class OptionProfile
{
	public OptionProfile(string data, float defValue, float[] AddNewValue)
	{
		this.optionProfile = new FloatProfileData(data, defValue);
		this.AddNewValue = AddNewValue;
	}

	public OptionProfile(string data, float defValue)
	{
		this.optionProfile = new FloatProfileData(data, defValue);
	}

	public float Option
	{
		get
		{
			return this.optionProfile.Data;
		}
		set
		{
			this.optionProfile.setValue(value);
		}
	}

	private FloatProfileData optionProfile;

	public float[] AddNewValue;
}
