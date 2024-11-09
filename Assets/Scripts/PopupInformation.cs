using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupInformation : PopupBase
{
	public void Show(Item itemTarget, int amountNeed)
	{
		base.Show();
		this.txtNote.text = PopupManager.Instance.GetText(Localization0.Collect_Fragments_to_Unlock_RankUp, null);
		bool[] amountInformation = this.GetAmountInformation(itemTarget);
		for (int i = 0; i < this.card_ListInfor.Length; i++)
		{
			this.card_ListInfor[i].gameObject.SetActive(amountInformation[i]);
		}
		this.ReloadItem(this.card_Solo, itemTarget, amountNeed);
	}

	public void ShowWarning(Item itemTarget, int amountNeed, string note = "")
	{
		this.Show(itemTarget, amountNeed);
		if (itemTarget != Item.Gold)
		{
			if (itemTarget != Item.Gem)
			{
				this.txtNote.text = PopupManager.Instance.GetText(Localization0.Not_enough_Fragments_to_Unlock_RankUp, null);
			}
			else
			{
				this.txtNote.text = PopupManager.Instance.GetText(Localization0.Not_enough_Gems_to_Unlock, null);
			}
		}
		else
		{
			this.txtNote.text = PopupManager.Instance.GetText(Localization0.Not_enough_Golds_to_Upgrade, null);
		}
		if (!string.IsNullOrEmpty(note))
		{
			this.txtNote.text = note;
		}
		this.txtNote.color = Color.yellow;
		base.transform.SetAsLastSibling();
	}

	private bool[] GetAmountInformation(Item itemTarget1)
	{
		bool[] array = new bool[this.card_ListInfor.Length];
		this.indexMap_StarGift = 0;
		this.indexDifficult_StarGift = 0;
		switch (itemTarget1)
		{
		case Item.M4A1_Fragment:
			this.indexMap_StarGift = 0;
			array[0] = true;
			array[1] = true;
			array[2] = true;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.Machine_Gun_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = true;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.Ice_Gun_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = false;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.Sniper_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = false;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.MGL_140_Fragment:
		case Item.Spread_Gun_Fragment:
		case Item.Rocket_Fragment:
		case Item.Fc10_Gun_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = false;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.Flame_Gun_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = false;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.Thunder_Shot_Fragment:
		case Item.Yoo_na_Fragment:
		case Item.Dvornikov_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = false;
			array[3] = false;
			array[4] = true;
			array[5] = false;
			break;
		case Item.Laser_Gun_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = false;
			array[3] = true;
			array[4] = true;
			array[5] = false;
			break;
		case Item.John_D_Fragment:
			array[0] = true;
			array[1] = true;
			array[2] = true;
			array[3] = false;
			array[4] = true;
			array[5] = false;
			break;
		default:
			switch (itemTarget1)
			{
			case Item.Gold:
			case Item.Gem:
				array[0] = false;
				array[1] = true;
				array[2] = true;
				array[3] = true;
				array[4] = true;
				array[5] = true;
				return array;
			}
			UnityEngine.Debug.LogError("Lỗi loại Item");
			break;
		case Item.Ct9_Gun_Fragment:
			array[0] = true;
			array[1] = true;
			break;
		}
		return array;
	}

	private void ReloadItem(CardReward card, Item item, int amountMax)
	{
		card.item = item;
		card.txt_Name.text = item.ToString().Replace('_', ' ');
		card.img_Main.sprite = PopupManager.Instance.sprite_Item[(int)item];
		card.txt_Amount.text = amountMax.ToString();
		if (item != Item.Gold)
		{
			if (item != Item.Gem)
			{
				card.txt_Description.text = PopupManager.Instance.GetText(Localization0.Available, null) + ": " + PlayerPrefs.GetInt("metal.squad.frag." + item.ToString(), 0);
			}
			else
			{
				card.txt_Description.text = PopupManager.Instance.GetText(Localization0.Available, null) + ": " + ProfileManager.userProfile.Ms;
			}
		}
		else
		{
			card.txt_Description.text = PopupManager.Instance.GetText(Localization0.Available, null) + ": " + ProfileManager.userProfile.Coin;
		}
		card.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(card.item)];
		card.ShowBorderEffect();
	}

	public void BtnInfor(CardBase card)
	{
		UnityEngine.Debug.Log("Card_____" + card.idCard);
		switch (card.idCard)
		{
		case 0:
			PopupManager.Instance.ShowGacha();
			break;
		case 1:
			PopupManager.Instance.ShowInapp(INAPP_TYPE.PACKS, delegate(bool closed)
			{
				if (closed)
				{
					MenuManager.Instance.topUI.ShowCoin();
				}
			});
			break;
		case 2:
			if (MenuManager.Instance.popupLuckySpin == null)
			{
				MenuManager.Instance.popupLuckySpin = UnityEngine.Object.Instantiate<PopupLuckySpin>(Resources.Load<PopupLuckySpin>("Popup/Popup_LucySpin"), base.transform.parent);
				MenuManager.Instance.popupLuckySpin.transform.SetAsLastSibling();
				MenuManager.Instance.listPopup[4] = MenuManager.Instance.popupLuckySpin.gameObject;
			}
			MenuManager.Instance.popupLuckySpin.Show();
			break;
		case 3:
			GameMode.Instance.modePlay = GameMode.ModePlay.Campaign;
			DataLoader.LoadDataCampaign();
			MenuManager.IDDifficultSelect = this.indexDifficult_StarGift;
			MenuManager.IDMapSelect = this.indexMap_StarGift;
			MenuManager.IDLevelSelect = 0;
			MenuManager.Instance.ChangeForm(FormUI.UICampaign, FormUI.Menu);
			break;
		case 4:
			PopupManager.Instance.ShowShopItem(delegate
			{
				MenuManager.Instance.topUI.ShowCoin();
			});
			break;
		case 5:
			if (MenuManager.Instance.popupGift == null)
			{
				MenuManager.Instance.popupGift = UnityEngine.Object.Instantiate<PopupGift>(Resources.Load<PopupGift>("Popup/Popup_Gift"), base.transform.parent);
				MenuManager.Instance.popupGift.transform.SetAsLastSibling();
				MenuManager.Instance.listPopup[5] = MenuManager.Instance.popupGift.gameObject;
			}
			MenuManager.Instance.popupGift.OnClosed = delegate()
			{
				MenuManager.Instance.topUI.ShowCoin();
			};
			MenuManager.Instance.popupGift.Show();
			break;
		}
		this.OnClose();
	}

	public bool isSolo;

	private int indexMap_StarGift;

	private int indexDifficult_StarGift;

	public CardBase[] card_ListInfor;

	public CardReward card_Solo;

	public Text txtNote;
}
