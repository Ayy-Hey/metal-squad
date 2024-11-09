using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rank.UI
{
	public class RankUI : MonoBehaviour
	{
		public void ShowInfor()
		{
			base.gameObject.SetActive(true);
			RankInfor rankCurrentByExp = RankManager.Instance.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
			RankInfor rankInfoByLevel = RankManager.Instance.GetRankInfoByLevel(rankCurrentByExp.Level + 1);
			this.ShowReward(rankInfoByLevel);
			this.imgIconMain.sprite = PopupManager.Instance.sprite_RankAccount[rankCurrentByExp.Level];
			this.txtName.text = PopupManager.Instance.GetText((Localization0)rankCurrentByExp.IdName, null).ToUpper();
			this.imgIconCurrent.sprite = PopupManager.Instance.sprite_RankAccount[rankCurrentByExp.Level];
			this.imgIconNext.sprite = PopupManager.Instance.sprite_RankAccount[rankInfoByLevel.Level];
			this.txtExpCurrent.text = RankManager.Instance.ExpCurrent();
			this.progress.value = RankManager.Instance.ExpCurrent01();
		}

		private void ShowReward(RankInfor rank)
		{
			int i = 0;
			if (rank.Gold > 0)
			{
				this.rankRewardItems[i].gameObject.SetActive(true);
				this.rankRewardItems[i].item = Item.Gold;
				this.rankRewardItems[i].img_Main.sprite = PopupManager.Instance.sprite_Item[1];
				this.rankRewardItems[i].txt_Amount.text = rank.Gold.ToString();
				this.rankRewardItems[i].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Gold)];
				this.rankRewardItems[i].ShowBorderEffect();
				i++;
			}
			if (rank.Gem > 0)
			{
				this.rankRewardItems[i].gameObject.SetActive(true);
				this.rankRewardItems[i].img_Main.sprite = PopupManager.Instance.sprite_Item[4];
				this.rankRewardItems[i].item = Item.Gem;
				this.rankRewardItems[i].txt_Amount.text = rank.Gem.ToString();
				this.rankRewardItems[i].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Gem)];
				this.rankRewardItems[i].ShowBorderEffect();
				i++;
			}
			if (rank.Gacha_C > 0)
			{
				this.rankRewardItems[i].gameObject.SetActive(true);
				this.rankRewardItems[i].img_Main.sprite = PopupManager.Instance.sprite_Item[38];
				this.rankRewardItems[i].item = Item.Common_Crate;
				this.rankRewardItems[i].txt_Amount.text = rank.Gacha_C.ToString();
				this.rankRewardItems[i].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Common_Crate)];
				this.rankRewardItems[i].ShowBorderEffect();
				i++;
			}
			if (rank.Gacha_B > 0)
			{
				this.rankRewardItems[i].gameObject.SetActive(true);
				this.rankRewardItems[i].img_Main.sprite = PopupManager.Instance.sprite_Item[39];
				this.rankRewardItems[i].item = Item.Epic_Crate;
				this.rankRewardItems[i].txt_Amount.text = rank.Gacha_B.ToString();
				this.rankRewardItems[i].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Epic_Crate)];
				this.rankRewardItems[i].ShowBorderEffect();
				i++;
			}
			while (i < this.rankRewardItems.Length)
			{
				this.rankRewardItems[i].gameObject.SetActive(false);
				i++;
			}
		}

		[SerializeField]
		private Image imgIconMain;

		[SerializeField]
		private Text txtName;

		[SerializeField]
		private Image imgIconCurrent;

		[SerializeField]
		private Image imgIconNext;

		[SerializeField]
		private Slider progress;

		[SerializeField]
		private Text txtExpCurrent;

		[SerializeField]
		private CardReward[] rankRewardItems;
	}
}
