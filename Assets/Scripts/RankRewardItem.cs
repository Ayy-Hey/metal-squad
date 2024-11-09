using System;
using UnityEngine;
using UnityEngine.UI;

public class RankRewardItem : MonoBehaviour
{
	public void Show(int value, Item item)
	{
		base.gameObject.SetActive(true);
		this.imgItem.sprite = PopupManager.Instance.sprite_Item[(int)item];
		this.txtValue.text = value.ToString();
		this.imgBG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(item)];
	}

	public void Off()
	{
		base.gameObject.SetActive(false);
	}

	public Image imgItem;

	public Text txtValue;

	public Image imgBG;
}
