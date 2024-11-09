using System;
using UnityEngine;

public class EyeBotProfile
{
	public EyeBotProfile()
	{
		this.LevelProfile = new IntProfileData("com.sora.metal.squad.EyeBotProfile.levelProfile", 0);
	}

	public int Level
	{
		get
		{
			return Mathf.Clamp(this.LevelProfile.Data.Value, 0, 9);
		}
		set
		{
			this.LevelProfile.setValue(Mathf.Min(9, value));
		}
	}

	public float TIME_DELAY
	{
		get
		{
			return this.Time_Delays[this.Level];
		}
	}

	public float Damage
	{
		get
		{
			return this.Damages[this.Level];
		}
	}

	public float Speed_Bullet
	{
		get
		{
			return this.Speed_Bullets[this.Level];
		}
	}

	public float HP
	{
		get
		{
			return this.HPs[this.Level];
		}
	}

	public int Max_Bullet
	{
		get
		{
			return this.Max_Bullets[this.Level];
		}
	}

	public float[] Time_Delays;

	public float[] Damages;

	public float[] Speed_Bullets;

	public float[] HPs;

	public int[] Max_Bullets;

	private IntProfileData LevelProfile;
}
