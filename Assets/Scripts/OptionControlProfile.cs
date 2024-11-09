using System;
using UnityEngine;

public class OptionControlProfile
{
	public OptionControlProfile(EControl control, int totalOption)
	{
		this.idControl = (int)control;
		this.optionPosX = new FloatProfileData[totalOption];
		this.optionPosY = new FloatProfileData[totalOption];
		string arg = "com.metal.squad.control." + control + ".optionX.";
		string arg2 = "com.metal.squad.control." + control + ".optionY.";
		float defaultValue = 0f;
		float defaultValue2 = 0f;
		for (int i = 0; i < totalOption; i++)
		{
			EOptionControl eoptionControl = (EOptionControl)i;
			if (eoptionControl != EOptionControl.Move)
			{
				if (eoptionControl == EOptionControl.Fire)
				{
					defaultValue = ((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.FireDefaultPos[this.idControl].x : ControlDefaultSpace.FireDefaultPosIPX[this.idControl].x);
					defaultValue2 = ((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.FireDefaultPos[this.idControl].y : ControlDefaultSpace.FireDefaultPosIPX[this.idControl].y);
				}
			}
			else
			{
				defaultValue = ((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.MoveDefaultPos[this.idControl].x : ControlDefaultSpace.MoveDefaultPosIPX[this.idControl].x);
				defaultValue2 = ((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.MoveDefaultPos[this.idControl].y : ControlDefaultSpace.MoveDefaultPosIPX[this.idControl].y);
			}
			this.optionPosX[i] = new FloatProfileData(arg + eoptionControl, defaultValue);
			this.optionPosY[i] = new FloatProfileData(arg2 + eoptionControl, defaultValue2);
		}
	}

	public Vector2 GetOptionPos(EOptionControl option)
	{
		Vector2 zero = Vector2.zero;
		zero.x = this.optionPosX[(int)option].Data.Value;
		zero.y = this.optionPosY[(int)option].Data.Value;
		return zero;
	}

	public void SetOptionPos(EOptionControl option, Vector2 val)
	{
		this.optionPosX[(int)option].setValue(val.x);
		this.optionPosY[(int)option].setValue(val.y);
	}

	public void Reset()
	{
		int length = Enum.GetValues(typeof(EOptionControl)).Length;
		for (int i = 0; i < length; i++)
		{
			EOptionControl eoptionControl = (EOptionControl)i;
			if (eoptionControl != EOptionControl.Move)
			{
				if (eoptionControl == EOptionControl.Fire)
				{
					this.optionPosX[i].setValue((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.FireDefaultPos[this.idControl].x : ControlDefaultSpace.FireDefaultPosIPX[this.idControl].x);
					this.optionPosY[i].setValue((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.FireDefaultPos[this.idControl].y : ControlDefaultSpace.FireDefaultPosIPX[this.idControl].y);
				}
			}
			else
			{
				this.optionPosX[i].setValue((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.MoveDefaultPos[this.idControl].x : ControlDefaultSpace.MoveDefaultPosIPX[this.idControl].x);
				this.optionPosY[i].setValue((!ThisPlatform.IsIphoneX) ? ControlDefaultSpace.MoveDefaultPos[this.idControl].y : ControlDefaultSpace.MoveDefaultPosIPX[this.idControl].y);
			}
		}
	}

	private FloatProfileData[] optionPosX;

	private FloatProfileData[] optionPosY;

	private int idControl;
}
