using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class TodayGift : MonoBehaviour
{
	public void OnShowCardReward(int amount, Item item)
	{
		this.cardReward.item = item;
		this.cardReward.txt_Amount.text = amount.ToString();
		this.cardReward.img_Main.sprite = PopupManager.Instance.sprite_Item[(int)item];
		this.cardReward.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(item)];
		this.cardReward.ShowBorderEffect();
	}

	public void Done()
	{
		this.obj_Done.SetActive(true);
		this.ani_Done.AnimationState.SetAnimation(0, "animation2", false);
		this.ani_Done.AnimationState.SetAnimation(0, "animation", false);
		this.ObjBoder.SetActive(false);
		this.obj_Shadow.SetActive(true);
	}

	public void NotDone()
	{
		this.obj_Done.SetActive(false);
		this.ObjBoder.SetActive(false);
		this.obj_Shadow.SetActive(false);
	}

	public void ToDay()
	{
		if (!this.isClampedToDay)
		{
			this.ObjBoder.SetActive(true);
			base.GetComponent<Image>().sprite = this.sprite_BGToDay;
			this.obj_Shadow.SetActive(false);
		}
		else
		{
			this.ObjBoder.SetActive(false);
			base.GetComponent<Image>().sprite = this.sprite_BGNotToDay;
			this.obj_Shadow.SetActive(true);
		}
	}

	public void SetTextToday(bool isDoneToday)
	{
		this.ObjBoder.SetActive(false);
		base.GetComponent<Image>().sprite = this.sprite_BGNotToDay;
		this.obj_Shadow.SetActive(true);
		this.isClampedToDay = true;
	}

	public Button btnGift;

	public GameObject obj_Done;

	public Image imgBG;

	public GameObject ObjBoder;

	public GameObject obj_Shadow;

	public SkeletonGraphic ani_Done;

	private bool isClampedToDay;

	public Sprite sprite_BGToDay;

	public Sprite sprite_BGNotToDay;

	public Sprite sprite_AmountToDay;

	public Sprite sprite_AmountNotToDay;

	public CardReward cardReward;
}
