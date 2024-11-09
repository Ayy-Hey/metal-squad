using System;
using UnityEngine;
using UnityEngine.UI;

namespace DailyQuest.UI
{
	public class MissionDailyQuest : MonoBehaviour
	{
		public void Show(string strMission, bool isGold, int valueReward, int total, int totalCurrent, int IDPopup)
		{
			this.IDPopup = (MissionDailyQuest.EIDPopup)IDPopup;
			this.txtMission.text = strMission;
			if (isGold)
			{
				this.itemReward.img_Main.sprite = PopupManager.Instance.sprite_Item[1];
				this.itemReward.item = Item.Gold;
				this.itemReward.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Gold)];
			}
			else
			{
				this.itemReward.img_Main.sprite = PopupManager.Instance.sprite_Item[4];
				this.itemReward.item = Item.Gem;
				this.itemReward.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Gem)];
			}
			this.itemReward.txt_Amount.text = valueReward.ToString();
			this.itemReward.ShowBorderEffect();
			this.txtTotal.text = totalCurrent + "/" + total;
			this.imgLine.fillAmount = (float)totalCurrent / (float)total;
		}

		public void SetState(EStateDailyQuest state)
		{
			for (int i = 0; i < this.objClaim.Length; i++)
			{
				this.objClaim[i].SetActive(i == (int)state);
			}
			if (state == EStateDailyQuest.COMPLETED)
			{
				this.objProgress.SetActive(false);
			}
		}

		public void OnGoPopup()
		{
			
			MenuManager.Instance.ClearPopUpStack();
			UnityEngine.Debug.Log("ID___" + this.IDPopup);
			switch (this.IDPopup)
			{
			case MissionDailyQuest.EIDPopup.CAMPAIGN:
				MenuManager.Instance.formMainMenu.OnCampaign();
				break;
			case MissionDailyQuest.EIDPopup.BOSSMODE:
				MenuManager.Instance.formMainMenu.OnBossMode();
				break;
			case MissionDailyQuest.EIDPopup.INAPP_GEM:
				PopupManager.Instance.ShowInapp(INAPP_TYPE.MS, delegate(bool closed)
				{
					if (closed)
					{
						MenuManager.Instance.topUI.ShowCoin();
					}
				});
				break;
			case MissionDailyQuest.EIDPopup.INAPP_GOLD:
				PopupManager.Instance.ShowInapp(INAPP_TYPE.COINS, delegate(bool closed)
				{
					if (closed)
					{
						MenuManager.Instance.topUI.ShowCoin();
					}
				});
				break;
			case MissionDailyQuest.EIDPopup.LUCKY_SPIN:
				MenuManager.Instance.formMainMenu.OnShowLuckySpin();
				break;
			case MissionDailyQuest.EIDPopup.ONLINE_REWARD:
				MenuManager.Instance.formMainMenu.OnShowGift();
				break;
			case MissionDailyQuest.EIDPopup.SHOP_WEAPON:
				MenuManager.Instance.formMainMenu.OnShowShop();
				break;
			case MissionDailyQuest.EIDPopup.SHOP_CHARACTER:
				MenuManager.Instance.formMainMenu.OnShowCharacter();
				break;
			case MissionDailyQuest.EIDPopup.LUCKY_GACHA:
				MenuManager.Instance.formMainMenu.OnShowGacha();
				break;
			case MissionDailyQuest.EIDPopup.PvP:
				MenuManager.Instance.formMainMenu.BtnPVP();
				break;
			}
		}

		[SerializeField]
		private Text txtMission;

		[SerializeField]
		private Text txtTotal;

		public GameObject[] objClaim;

		private MissionDailyQuest.EIDPopup IDPopup;

		[SerializeField]
		private Image imgLine;

		public GameObject objProgress;

		public CardReward itemReward;

		public enum EIDPopup
		{
			CAMPAIGN,
			BOSSMODE,
			ENDLESS,
			INAPP_GEM,
			INAPP_GOLD,
			LUCKY_SPIN,
			ONLINE_REWARD,
			SHOP_WEAPON,
			SHOP_CHARACTER,
			LUCKY_GACHA,
			PvP
		}
	}
}
