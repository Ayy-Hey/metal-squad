using System;
using UnityEngine;

public class BombAirplaneSkillProfile
{
	public BombAirplaneSkillProfile()
	{
		this.LevelProfile = new IntProfileData("com.sora.metal.squad.BombAirplaneSkillProfile.levelProfile", 0);
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

	public int NumberTurn
	{
		get
		{
			return this.NumberTurns[this.Level];
		}
	}

	public float Damaged
	{
		get
		{
			return this.Damageds[this.Level];
		}
	}

	private int[] NumberTurns = new int[]
	{
		1,
		1,
		2,
		2,
		3,
		3,
		4,
		4,
		5,
		5
	};

	private float[] Damageds = new float[]
	{
		10f,
		20f,
		30f,
		40f,
		50f,
		60f,
		70f,
		80f,
		90f,
		100f
	};

	private IntProfileData LevelProfile;
}
