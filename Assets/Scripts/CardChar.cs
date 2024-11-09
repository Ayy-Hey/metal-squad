using System;
using UnityEngine;
using UnityEngine.UI;

public class CardChar : CardBase
{
	[Header("---------------CardChar---------------")]
	public Image img_Boder;

	public Image img_IconRank;

	public int level;

	public int power;

	public int starUnlock;

	public int costUpgrade;

	public float costUnlockByGem;

	public float costUnlockByGold;

	public float[] valueParameters;

	public float[] valueParametersMax;
}
