using System;
using UnityEngine;

public class CardSkill : CardBase
{
	[Header("---------------CardSkill---------------")]
	public int level;

	public int levelCharUpgrade;

	public int costUpgrade;

	public bool isActive;

	public string nameSkill;

	public string infoSkill;

	public string[] nameParameters;

	public float[] valueParameters;

	public float[] valueParametersNext;

	public Sprite sprite_Lock;

	public Sprite sprite_Unlock;
}
