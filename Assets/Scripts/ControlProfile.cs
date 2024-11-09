using System;
using UnityEngine;

public class ControlProfile
{
	public ControlProfile()
	{
		int length = Enum.GetValues(typeof(EControl)).Length;
		int length2 = Enum.GetValues(typeof(EOptionControl)).Length;
		this.controls = new OptionControlProfile[length];
		for (int i = 0; i < this.controls.Length; i++)
		{
			this.controls[i] = new OptionControlProfile((EControl)i, length2);
		}
	}

	public Vector2 GetPosOption(EControl control, EOptionControl option)
	{
		return this.controls[(int)control].GetOptionPos(option);
	}

	public void SetOption(EControl control, EOptionControl option, Vector2 pos)
	{
		this.controls[(int)control].SetOptionPos(option, pos);
	}

	public void Reset(EControl control)
	{
		this.controls[(int)control].Reset();
	}

	private OptionControlProfile[] controls;
}
