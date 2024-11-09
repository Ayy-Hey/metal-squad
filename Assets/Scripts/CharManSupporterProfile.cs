using System;
using UnityEngine;

public class CharManSupporterProfile
{
	public CharManSupporterProfile()
	{
		this.LevelProfile = new IntProfileData("com.sora.metal.squad.CharManSupporterProfile.levelProfile", 0);
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

	public float Time_Attack
	{
		get
		{
			return this.Time_Attacks[this.Level];
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

	public int HP
	{
		get
		{
			return this.HPs[this.Level];
		}
	}

	private float[] Time_Attacks = new float[]
	{
		0.3f,
		0.3f,
		0.3f,
		0.25f,
		0.25f,
		0.25f,
		0.2f,
		0.2f,
		0.2f,
		0.2f
	};

	private float[] Damages = new float[]
	{
		1f,
		2f,
		3f,
		4f,
		5f,
		6f,
		7f,
		8f,
		9f,
		10f
	};

	private float[] Speed_Bullets = new float[]
	{
		7f,
		7f,
		7f,
		8f,
		9f,
		10f,
		11f,
		12f,
		12f,
		12f
	};

	private int[] HPs = new int[]
	{
		250,
		300,
		350,
		400,
		450,
		500,
		550,
		600,
		650,
		700
	};

	private IntProfileData LevelProfile;
}
