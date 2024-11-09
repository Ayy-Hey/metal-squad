using System;
using UnityEngine;

public class InforWeapon
{
	public void SetCostUpgrade(Item item, int value)
	{
		if (this.costUpgrade == null)
		{
			this.costUpgrade = new MoneyUpgrade();
		}
		this.costUpgrade.item = item;
		this.costUpgrade.value = value;
	}

	public int id;

	public string nameWeapon;

	public int power;

	public int powerNext;

	public int powerMax;

	public int levelWeapon;

	public Sprite spriteWeapon;

	public Sprite spriteWeaponNext;

	public Vector2 sizeImage;

	public bool isUnlock;

	public bool isVip;

	public bool isEquiped;

	public Item CurrencyUnlock;

	public Item CurrencyRankUp;

	public int rankBase;

	public int rankUpped;

	public int costRankUp;

	public string[] passiveDesc;

	public int amountCurrent;

	public int costUnlockByGold;

	public int costUnlockByGem;

	public MoneyUpgrade costUpgrade;

	public int costBullet;

	public int campaignUpgrade;

	public PropertiesWeapon[] properties;
}
