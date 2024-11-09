using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CardWeapon : CardBase
{
	[Header("---------------CardWeapon---------------")]
	public int gunUnlock;

	public Image img_IconRank;

	public GameObject obj_Tip;

	public GameObject obj_Equiped;

	public SkeletonAnimation obj_EffectByRank;

	public Text txtFragment;

	public Image imageProgressFragment;

	public GameObject objProgressFragment;
}
