using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CardWeaponLoadout : CardBase
{
	[Header("---------------CardWeaponLoadout---------------")]
	public Text txt_Power;

	public Image img_Rank;

	public SkeletonAnimation obj_EffectByRank;

	public GameObject buttonNext;

	public GameObject buttonBack;
}
