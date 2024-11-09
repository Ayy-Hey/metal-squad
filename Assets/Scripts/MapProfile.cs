using System;
using UnityEngine;

public class MapProfile
{
	public MapProfile(int Level)
	{
		this.Level = Level;
		this.isUnLocked = new BoolProfileData[3];
		this.numberStar = new IntProfileData[3];
		this.isUnLocked[0] = new BoolProfileData("rambo_locked" + this.Level + "1.0.4", false);
		this.numberStar[0] = new IntProfileData("rambo_star" + this.Level + "1.0.4", 0);
		this.isUnLocked[1] = new BoolProfileData("rambo_locked" + this.Level + "1.0.4HARD", false);
		this.numberStar[1] = new IntProfileData("rambo_star" + this.Level + "1.0.4HARD", 0);
		this.isUnLocked[2] = new BoolProfileData("rambo_locked" + this.Level + "1.0.4SUPER_HARD", false);
		this.numberStar[2] = new IntProfileData("rambo_star" + this.Level + "1.0.4SUPER_HARD", 0);
	}

	public MapProfile(ELevel level)
	{
		this.name = level.ToString();
		this.isUnLocked = new BoolProfileData[3];
		this.numberStar = new IntProfileData[3];
		this.isUnLocked[0] = new BoolProfileData("rambo_locked" + this.name + "1.0.4", false);
		this.numberStar[0] = new IntProfileData("rambo_star" + this.name + "1.0.4", 0);
		this.isUnLocked[1] = new BoolProfileData("rambo_locked" + this.name + "1.0.4HARD", false);
		this.numberStar[1] = new IntProfileData("rambo_star" + this.name + "1.0.4HARD", 0);
		this.isUnLocked[2] = new BoolProfileData("rambo_locked" + this.name + "1.0.4SUPER_HARD", false);
		this.numberStar[2] = new IntProfileData("rambo_star" + this.name + "1.0.4SUPER_HARD", 0);
	}

	public bool GetUnlocked(GameMode.Mode mode)
	{
		return this.isUnLocked[(int)mode].Data;
	}

	public void SetUnlocked(GameMode.Mode mode, bool unlock = true)
	{
		this.isUnLocked[(int)mode].setValue(unlock);
	}

	public int GetStar(GameMode.Mode mode)
	{
		return this.numberStar[(int)mode].Data;
	}

	public void SetStar(GameMode.Mode mode, int value)
	{
		int star = this.GetStar(mode);
		this.numberStar[(int)mode].setValue(Mathf.Max(value, star));
	}

	public BoolProfileData[] isUnLocked;

	public IntProfileData[] numberStar;

	public int Level;

	public string name;
}
