using System;

public class IntBitMaskProfileData : IntProfileData
{
	public IntBitMaskProfileData(string tag, int defaultValue) : base(tag, defaultValue)
	{
	}

	public void turnOn(int bit)
	{
		if (bit < 32)
		{
			this.save(this.data | 1 << bit);
		}
	}

	public void turnOff(int bit)
	{
		if (bit < 32)
		{
			this.save(this.data & ~(1 << bit));
		}
	}

	public bool isOn(int bit)
	{
		return bit < 32 && (this.data >> bit & 1) != 0;
	}
}
