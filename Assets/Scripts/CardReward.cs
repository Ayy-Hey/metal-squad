using System;
using UnityEngine;
using UnityEngine.UI;

public class CardReward : CardBase
{
	[Header("---------------CardReward---------------")]
	public SpriteRenderer sprite_BG;

	public SpriteRenderer sprite_Item;

	public TextMesh txtMesh_Name;

	public Text txt_Note;

	public Image img_Vip;

	public TweenAlpha twn_AlphaAll;

	public TweenScale twn_ScaleAll;

	public TweenAlpha twn_AlphaGlow;

	public TweenScale twn_ScaleGlow;

	public int startWeight;

	public int endWeight;
}
