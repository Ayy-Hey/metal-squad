using System;
using UnityEngine;

public class EnemyDataInfo
{
	public int type { get; set; }

	public bool created { get; set; }

	public bool ismove { get; set; }

	public int level { get; set; }

	public float pos_x { get; set; }

	public float pos_y { get; set; }

	public bool gift { get; set; }

	public int gift_value { get; set; }

	public Vector2 Vt2
	{
		get
		{
			return new Vector2(this.pos_x, this.pos_y);
		}
	}

	public GiftWeapon giftWeapon;

	public bool DropCoin;

	public int ValueCoin;
}
