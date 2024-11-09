using System;
using UnityEngine;

public class InfoGunBonus
{
	public static float damage
	{
		get
		{
			return PlayerPrefs.GetFloat("DamageBonus", 0f);
		}
		set
		{
			PlayerPrefs.SetFloat("DamageBonus", value);
		}
	}

	private static float _damage;

	private static float _speed;

	private static float _health;

	public static float speed;

	public static float health;
}
