using System;
using UnityEngine;
using UnityEngine.UI;

public class CardAttributeWeapon : CardBase
{
	[Header("---------------CardAttributeWeapon---------------")]
	public float valueCurrent;

	public float valueNext;

	public float valueMax;

	public float valueMineMax;

	public float valueNextRank;

	public Text txt_ValueNext;

	public GameObject obj_Arrow;

	public Image img_CoreMax;

	public RectTransform rect_ValueBar;
}
