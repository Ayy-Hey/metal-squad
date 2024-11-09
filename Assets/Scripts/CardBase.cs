using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
	public void ShowBorderEffect()
	{
		if (this.rectAmountBase != null)
		{
			float num = this.rectAmountBase.offsetMin.x;
			if (!string.IsNullOrEmpty(this.txt_Amount.text))
			{
				num = 80f;
				char[] array = this.txt_Amount.text.ToCharArray();
				num -= (float)((array.Length - 1) * 10);
			}
			this.rectAmountBase.offsetMin = new Vector2(num, this.rectAmountBase.offsetMin.y);
		}
		if (this.skeletonBorder == null)
		{
			return;
		}
		int num2 = PopupManager.Instance.GetRankItem(this.item);
		num2 -= 2;
		if (num2 < 0 || num2 > 2)
		{
			this.skeletonBorder.gameObject.SetActive(false);
			return;
		}
		this.skeletonBorder.gameObject.SetActive(true);
		this.skeletonBorder.Skeleton.SetSkin(this.skinRank[num2]);
		this.skeletonBorder.AnimationState.SetAnimation(0, this.animSkin[num2], true);
	}

	public int idCard;

	public string key;

	public int value;

	public Image img_BG;

	public Image img_Main;

	public Text txt_Name;

	public Text txt_Amount;

	public Text txt_Description;

	public Image img_Lock;

	public Image img_Core;

	public Text txt_Lock;

	public Button button;

	public Item item;

	[Header("---------------Obj---------------")]
	public bool isOn;

	public GameObject obj_Active;

	public GameObject obj_Lock;

	public GameObject obj_Open;

	public GameObject obj_Note;

	public SkeletonAnimation skeletonBorder;

	[SpineSkin("", "", true, false)]
	public string[] skinRank;

	[SpineAnimation("", "", true, false)]
	public string[] animSkin;

	public RectTransform rectAmountBase;
}
