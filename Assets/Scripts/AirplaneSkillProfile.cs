using System;
using UnityEngine;

public class AirplaneSkillProfile
{
	public AirplaneSkillProfile()
	{
		this.levelProfile = new IntProfileData("com.sora.metal.squad.AirplaneSkillProfile.levelProfile", 0);
	}

	public int NumberBullet
	{
		get
		{
			return this.NumberBullets[this.Level];
		}
	}

	public float Damaged
	{
		get
		{
			return this.Damageds[this.Level];
		}
	}

	public int Level
	{
		get
		{
			return Mathf.Clamp(this.levelProfile.Data.Value, 0, 9);
		}
		set
		{
			this.levelProfile.setValue(Mathf.Min(value, 9));
		}
	}

	private int[] NumberBullets = new int[]
	{
		3,
		3,
		3,
		4,
		4,
		4,
		5,
		5,
		5,
		5
	};

	private float[] Damageds = new float[]
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

	private IntProfileData levelProfile;
}
