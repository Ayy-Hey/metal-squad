using System;
using Rank;
using UnityEngine;
using UnityEngine.UI;

public class RankRewardPopup : PopupBase
{
	public override void Show()
	{
		base.Show();
		if (ProfileManager.settingProfile.IsSound)
		{
			this.mAudio.Play();
		}
		this.imgIconRank.sprite = PopupManager.Instance.sprite_RankAccount[this.infor.Level];
		int i = 0;
		if (this.infor.Gold > 0)
		{
			this.ShowReward(i, Item.Gold, this.infor.Gold);
			i++;
		}
		if (this.infor.Gem > 0)
		{
			this.ShowReward(i, Item.Gem, this.infor.Gem);
			i++;
		}
		if (this.infor.Gacha_C > 0)
		{
			this.ShowReward(i, Item.Common_Crate, this.infor.Gacha_C);
			i++;
		}
		if (this.infor.Gacha_B > 0)
		{
			this.ShowReward(i, Item.Epic_Crate, this.infor.Gacha_B);
			i++;
		}
		while (i < this.cardRewardItems.Length)
		{
			this.cardRewardItems[i].gameObject.SetActive(false);
			i++;
		}
	}

	private void ShowReward(int index, Item item, int amount)
	{
		this.cardRewardItems[index].gameObject.SetActive(true);
		this.cardRewardItems[index].img_Main.sprite = PopupManager.Instance.sprite_Item[(int)item];
		this.cardRewardItems[index].item = item;
		this.cardRewardItems[index].txt_Amount.text = amount.ToString();
		this.cardRewardItems[index].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(item)];
		this.cardRewardItems[index].ShowBorderEffect();
	}

	public override void OnClose()
	{
		PopupManager.Instance.SaveReward(Item.Gold, this.infor.Gold, "RankUpAccount:" + this.infor.Level, null);
		PopupManager.Instance.SaveReward(Item.Gem, this.infor.Gem, "RankUpAccount:" + this.infor.Level, null);
		PopupManager.Instance.SaveReward(Item.Common_Crate, this.infor.Gacha_C, "RankUpAccount:" + this.infor.Level, null);
		PopupManager.Instance.SaveReward(Item.Epic_Crate, this.infor.Gacha_B, "RankUpAccount:" + this.infor.Level, null);
		if (this.onClosed != null)
		{
			this.onClosed(true);
		}
		base.OnClose();
	}

	[SerializeField]
	private Image imgIconRank;

	[SerializeField]
	private AudioSource mAudio;

	[SerializeField]
	private CardBase[] cardRewardItems;

	[Header("---------------Show()---------------")]
	public RankInfor infor;

	public Action<bool> onClosed;
}
