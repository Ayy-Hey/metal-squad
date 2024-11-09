using System;
using System.Collections;
using com.dev.util.SecurityHelper;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class FormUpgradeCharacter1 : FormBase
{
	public override void Show()
	{
		base.Show();
		int num = (int)RemoteConfigFirebase.Instance.GetLongValue(RemoteConfigFirebase.GEM_RANKUP, 100L);
		this.GemRankUp = new SecuredInt[2];
		this.GemRankUp[0] = new SecuredInt(num);
		this.GemRankUp[1] = new SecuredInt(num * 2);
		float num2 = Mathf.Clamp(((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f, 0f, 1f);
		this.rect_Left.localScale = new Vector3(0.78f + num2 * 0.22f, 0.78f + num2 * 0.22f, 1f);
		this.rect_Right.localScale = new Vector3(0.78f + num2 * 0.22f, 0.78f + num2 * 0.22f, 1f);
		if (ThisPlatform.IsIphoneX)
		{
			this.rect_Left.localPosition = new Vector3(70f, this.rect_Left.localPosition.y, 0f);
		}
		else
		{
			this.rect_Left.localPosition = new Vector3(0f, this.rect_Left.localPosition.y, 0f);
		}
		this.listMaxChar = new float[3];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < ProfileManager.rambos.Length; j++)
			{
				if (this.listMaxChar[i] < ProfileManager.rambos[j].GetMaxOption(i))
				{
					this.listMaxChar[i] = ProfileManager.rambos[j].GetMaxOption(i);
				}
			}
		}
		this.indexCharSelected = ProfileManager.settingProfile.IDChar;
		MenuManager.Instance.obj_Characters.localPosition = new Vector3(-0.8f, 0f, 0f);
		this.ClickToCharacter(this.listCardChar[this.indexCharSelected]);
		bool flag = false;
		Item itemID = ProfileManager.rambos[0].GetItemID(ProfileManager.rambos[0].LevelUpgrade + 1);
		int num3 = ProfileManager.rambos[0].ValueUpgrade(ProfileManager.rambos[0].LevelUpgrade + 1);
		if (itemID != Item.Gold)
		{
			if (itemID == Item.Gem)
			{
				flag = (ProfileManager.userProfile.Ms >= num3);
			}
		}
		else
		{
			flag = (ProfileManager.userProfile.Coin >= num3);
		}
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UpgradeCharacter)
		{
			this.indexCharSelected = 0;
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		else if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[6].keyPlayerPrefs, 0) == 0 && ProfileManager.rambos[0].LevelUpgrade < 29 && flag)
		{
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_UpgradeCharacter);
			this.indexCharSelected = 0;
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		
	}

	public void ClickToCharacter(CardChar card)
	{
		this.indexCharSelected = card.idCard;
		if (ProfileManager.InforChars[this.indexCharSelected].IsUnLocked)
		{
			ProfileManager.settingProfile.IDChar = this.indexCharSelected;
		}
		this.ShowAvatarCharacters();
		for (int i = 0; i < MenuManager.Instance.MainCharacters.Length; i++)
		{
			MenuManager.Instance.MainCharacters[i].gameObject.SetActive(i == this.indexCharSelected);
		}
		MenuManager.Instance.MainCharacters[this.indexCharSelected].Show();
		this.ShowInforChar();
		this.CheckTips();
	}

	private void ShowInforChar()
	{
		this.img_Skill.sprite = this.sprite_Skill[this.indexCharSelected];
		this.txt_NameChar.text = string.Concat(new string[]
		{
			ProfileManager.rambos[this.indexCharSelected].name,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			(!ProfileManager.InforChars[this.indexCharSelected].IsUnLocked) ? "1" : (ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1 + string.Empty)
		});
		int num = ProfileManager.rambos[this.indexCharSelected].RankBase + ProfileManager.rambos[this.indexCharSelected].RankUpped;
		this.txt_NameChar.color = PopupManager.Instance.color_Rank[num];
		this.img_RankChar.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		Color color = PopupManager.Instance.color_Rank[num];
		this.img_BGNameChar.color = new Color(color.r, color.g, color.b, 0.4f);
		for (int i = 0; i < this.listAttributeChar.Length; i++)
		{
			this.listAttributeChar[i].valueCurrent = ProfileManager.rambos[this.indexCharSelected].GetOption(i);
			this.listAttributeChar[i].valueNext = ProfileManager.rambos[this.indexCharSelected].GetOptionByLevel(i, Mathf.Min(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1, 29));
			this.listAttributeChar[i].valueNextRank = ProfileManager.rambos[this.indexCharSelected].GetOptionByLevel(i, Mathf.Min(ProfileManager.rambos[this.indexCharSelected].RankUpped + 1, 2) * 10 + 9);
			this.listAttributeChar[i].valueMineMax = ProfileManager.rambos[this.indexCharSelected].GetMaxOption(i);
			this.listAttributeChar[i].valueMax = this.listMaxChar[i];
			this.listAttributeChar[i].txt_Amount.text = string.Concat(new object[]
			{
				ProfileManager.rambos[this.indexCharSelected].GetOption(i),
				"/<color=#898989FF>",
				this.listMaxChar[i],
				"</color>"
			});
			this.listAttributeChar[i].img_Core.fillAmount = ProfileManager.rambos[this.indexCharSelected].GetOption(i) / this.listMaxChar[i];
			this.listAttributeChar[i].img_CoreMax.fillAmount = ProfileManager.rambos[this.indexCharSelected].GetMaxOption(i) / this.listMaxChar[i];
		}
		if (ProfileManager.InforChars[this.indexCharSelected].IsUnLocked)
		{
			this.img_Skill.color = Color.white;
			this.obj_Unlock.SetActive(false);
			this.txt_ConditionUnlock.gameObject.SetActive(false);
			if (ProfileManager.rambos[this.indexCharSelected].LevelUpgrade >= ProfileManager.rambos[this.indexCharSelected].PriceUpgrade.Length - 1)
			{
				this.obj_Upgrade_RankUp.SetActive(false);
				this.obj_MaximumChar.SetActive(true);
			}
			else
			{
				this.obj_Upgrade_RankUp.SetActive(true);
				this.obj_MaximumChar.SetActive(false);
				if (ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1 < (ProfileManager.rambos[this.indexCharSelected].RankUpped + 1) * 10)
				{
					this.obj_BtnUpgrade.SetActive(true);
					this.obj_BtnRankUp.SetActive(false);
                    Debug.Log("Cost " + ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade).ToString());
					this.txt_CostUpgradeChar.text = ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade).ToString();
					this.imgMoney1.sprite = PopupManager.Instance.sprite_Item[(int)ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade)];
					this.imgMoney2.sprite = PopupManager.Instance.sprite_Item[(int)ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade)];
				}
				else
				{
					this.obj_BtnUpgrade.SetActive(false);
					if (ProfileManager.rambos[this.indexCharSelected].RankUpped < 2)
					{
						this.obj_BtnRankUp.SetActive(true);
						this.txt_CostRankUpChar.text = ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)] + string.Empty;
						this.txt_CostRankUpCharGem.text = this.GemRankUp[ProfileManager.rambos[this.indexCharSelected].RankUpped].ToString();
					}
					else
					{
						this.obj_BtnRankUp.SetActive(false);
					}
				}
			}
			this.img_ProcessFrag.fillAmount = (float)PlayerPrefs.GetInt("metal.squad.frag." + ((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyRankUp.Value).ToString(), 0) / (float)ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)];
			this.txt_CostFrag.text = PlayerPrefs.GetInt("metal.squad.frag." + ((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyRankUp.Value).ToString(), 0) + "/" + ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)];
		}
		else
		{
			this.img_Skill.color = Color.gray;
			this.obj_Unlock.SetActive(true);
			this.obj_Upgrade_RankUp.SetActive(false);
			this.obj_MaximumChar.SetActive(false);
			int value = ProfileManager.rambos[this.indexCharSelected].GemUnlock.Value;
			int value2 = ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value;
			int starUnlock = ProfileManager.rambos[this.indexCharSelected].StarUnlock;
			bool flag = starUnlock <= ProfileManager._CampaignProfile.GetTotalStar;
			this.txt_ConditionUnlock.gameObject.SetActive(!flag);
			this.txt_ConditionUnlock.text = string.Empty + ProfileManager.rambos[this.indexCharSelected].StarUnlock;
			this.txt_CostUnlockByGem.text = value.ToString();
			this.txt_CostUnlockByFrag.text = ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value + string.Empty;
			this.txt_CostFrag.text = PlayerPrefs.GetInt("metal.squad.frag." + (Item)ProfileManager.rambos[this.indexCharSelected].CurrencyUnlock.Value, 0) + "/" + ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value;
			this.img_ProcessFrag.fillAmount = (float)PlayerPrefs.GetInt("metal.squad.frag." + ((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyUnlock.Value).ToString(), 0) / (float)ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value;
			if (flag)
			{
				this.txt_ConditionUnlock.color = Color.white;
			}
			else
			{
				this.txt_ConditionUnlock.color = Color.gray;
			}
		}
		this.obj_Frag.SetActive(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade < 29);
		this.txt_Passive1.color = ((ProfileManager.rambos[this.indexCharSelected].RankUpped < 1) ? Color.gray : this.color_Enable);
		this.txt_Passive2.color = ((ProfileManager.rambos[this.indexCharSelected].RankUpped < 2) ? Color.gray : this.color_Enable);
		this.txt_PowerChar.text = ProfileManager.rambos[this.indexCharSelected].Power() + string.Empty;
	}

	public void CheckTips()
	{
		bool[] lisEnableTips = CharacterTips.Instance.GetLisEnableTips();
		for (int i = 0; i < this.listCardChar.Length; i++)
		{
			this.listCardChar[i].obj_Note.SetActive(lisEnableTips[i + 1]);
		}
		bool flag = false;
		Item itemID = ProfileManager.rambos[0].GetItemID(ProfileManager.rambos[0].LevelUpgrade + 1);
		int num = ProfileManager.rambos[0].ValueUpgrade(ProfileManager.rambos[0].LevelUpgrade + 1);
		if (itemID != Item.Gold)
		{
			if (itemID == Item.Gem)
			{
				flag = (ProfileManager.userProfile.Ms >= num);
			}
		}
		else
		{
			flag = (ProfileManager.userProfile.Coin >= num);
		}
		if (this.obj_Upgrade_RankUp.activeSelf)
		{
			this.obj_TipUpgrade.SetActive(this.obj_BtnUpgrade.activeSelf && flag);
			this.obj_TipRankUp.SetActive(this.obj_BtnRankUp.activeSelf && PlayerPrefs.GetInt("metal.squad.frag." + ((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyRankUp.Value).ToString(), 0) / ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)] >= 1);
		}
		else if (this.obj_Unlock.activeSelf)
		{
			this.obj_TipUnlockByGem.SetActive(ProfileManager.rambos[this.indexCharSelected].GemUnlock.Value <= ProfileManager.userProfile.Ms);
			if (PlayerPrefs.GetInt("metal.squad.frag." + ((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyUnlock.Value).ToString(), 0) < ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value && ProfileManager.rambos[this.indexCharSelected].StarUnlock <= ProfileManager._CampaignProfile.GetTotalStar)
			{
				this.obj_TipUnlockByGold.SetActive(false);
			}
			else
			{
				this.obj_TipUnlockByGold.SetActive(true);
			}
			SkeletonGraphic component = this.obj_TipUnlockByGold.GetComponent<SkeletonGraphic>();
			SkeletonGraphic component2 = this.obj_TipUnlockByGem.GetComponent<SkeletonGraphic>();
			if (this.obj_TipUnlockByGem.activeSelf && this.obj_TipUnlockByGold.activeSelf && component2 != null && component != null)
			{
				component.AnimationState.SetAnimation(0, "animation", true);
				component2.AnimationState.SetAnimation(0, "animation", true);
			}
		}
	}

	private void ShowAvatarCharacters()
	{
		for (int i = 0; i < this.listCardChar.Length; i++)
		{
			this.listCardChar[i].img_Main.sprite = PopupManager.Instance.sprite_Item[PopupManager.Instance.ConvertToIndexItem(ItemConvert.Character, i)];
			this.listCardChar[i].obj_Lock.SetActive(!ProfileManager.InforChars[i].IsUnLocked);
			this.listCardChar[i].obj_Open.SetActive(this.indexCharSelected == i);
			this.listCardChar[i].obj_Active.gameObject.SetActive(ProfileManager.settingProfile.IDChar == i);
			int num = ProfileManager.rambos[i].RankBase + ProfileManager.rambos[i].RankUpped;
			this.listCardChar[i].img_Boder.color = PopupManager.Instance.color_Rank[num];
			this.listCardChar[i].img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		}
	}

	private IEnumerator EffectButtonUpgrade(float time, GameObject offButton)
	{
		offButton.SetActive(false);
		yield return new WaitForSeconds(time);
		offButton.SetActive(true);
		yield break;
	}

	public void BtnCharacter(CardChar card)
	{
		AudioClick.Instance.OnClick();
		
		this.ClickToCharacter(card);
	}

	public void BtnPopupUpgrade()
	{
		
		int num = ProfileManager.rambos[this.indexCharSelected].RankBase + ProfileManager.rambos[this.indexCharSelected].RankUpped;
		this.popUpUpgrade_txtName.text = ProfileManager.rambos[this.indexCharSelected].name;
		this.popUpUpgrade_txtName.color = PopupManager.Instance.color_Rank[num];
		this.popUpUpgrade_txtLevel.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1
		});
		this.popUpUpgrade_txtLevelNext.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 2
		});
		this.popUpUpgrade_txtPower.text = ProfileManager.rambos[this.indexCharSelected].Power() + string.Empty;
		this.popUpUpgrade_txtPowerNext.text = ProfileManager.rambos[this.indexCharSelected].Power(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1) + string.Empty;
		for (int i = 0; i < 3; i++)
		{
			this.popUpUpgrade_AttributesChar[i].txt_Amount.text = this.listAttributeChar[i].valueCurrent + string.Empty;
			if (this.listAttributeChar[i].valueNext > this.listAttributeChar[i].valueCurrent)
			{
				this.popUpUpgrade_AttributesChar[i].txt_ValueNext.text = this.listAttributeChar[i].valueNext + string.Empty;
			}
			this.popUpUpgrade_AttributesChar[i].img_Core.fillAmount = this.listAttributeChar[i].valueCurrent / this.listAttributeChar[i].valueMax;
			this.popUpUpgrade_AttributesChar[i].img_CoreMax.fillAmount = this.listAttributeChar[i].valueNext / this.listAttributeChar[i].valueMax;
		}
		this.popUpUpgrade_BtnUpgradeTip.SetActive(this.obj_TipUpgrade.activeSelf);
		this.popUpUpgrade_txtUpgradeGold.text = this.txt_CostUpgradeChar.text;
		this.popUpUpgrade_CardsChar.img_Main.sprite = this.listCardChar[this.indexCharSelected].img_Main.sprite;
		this.popUpUpgrade_CardsChar.img_Boder.color = PopupManager.Instance.color_Rank[num];
		this.popUpUpgrade_CardsChar.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		this.obj_PopUpUpgrade.SetActive(true);
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UpgradeCharacter)
		{
			MenuManager.Instance.tutorial.NextTutorial(2);
		}
	}

	public void BtnPopupRankUp()
	{
		
		int num = ProfileManager.rambos[this.indexCharSelected].RankBase + ProfileManager.rambos[this.indexCharSelected].RankUpped;
		this.popUpRankUp_txtNameRankOld.text = ProfileManager.rambos[this.indexCharSelected].name + string.Empty;
		this.popUpRankUp_txtNameRankNew.text = ProfileManager.rambos[this.indexCharSelected].name + string.Empty;
		this.popUpRankUp_txtNameRankOld.color = PopupManager.Instance.color_Rank[num];
		this.popUpRankUp_txtNameRankNew.color = PopupManager.Instance.color_Rank[num + 1];
		this.popUpRankUp_ImgRankOld.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		this.popUpRankUp_ImgRankNew.sprite = PopupManager.Instance.sprite_IconRankItem[num + 1];
		this.popUpRankUp_CardsCharOld.img_Main.sprite = this.listCardChar[this.indexCharSelected].img_Main.sprite;
		this.popUpRankUp_CardsCharNew.img_Main.sprite = this.listCardChar[this.indexCharSelected].img_Main.sprite;
		this.popUpRankUp_CardsCharOld.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		this.popUpRankUp_CardsCharNew.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num + 1];
		this.popUpRankUp_CardsCharOld.img_Boder.color = PopupManager.Instance.color_Rank[num];
		this.popUpRankUp_CardsCharNew.img_Boder.color = PopupManager.Instance.color_Rank[num + 1];
		for (int i = 0; i < this.listAttributeChar.Length; i++)
		{
			this.popUpRankUp_AttributesCharOld[i].gameObject.SetActive(this.listAttributeChar[i].gameObject.activeSelf);
			this.popUpRankUp_AttributesCharNew[i].gameObject.SetActive(this.listAttributeChar[i].gameObject.activeSelf);
			this.popUpRankUp_AttributesCharOld[i].txt_Amount.text = string.Concat(new object[]
			{
				this.listAttributeChar[i].valueCurrent,
				"<color=#7E7E7EFF>/",
				this.listAttributeChar[i].valueCurrent,
				"</color>"
			});
			this.popUpRankUp_AttributesCharNew[i].txt_Amount.text = string.Concat(new object[]
			{
				this.listAttributeChar[i].valueCurrent,
				"<color=#7E7E7EFF>/",
				this.listAttributeChar[i].valueNextRank,
				"</color>"
			});
			this.popUpRankUp_AttributesCharOld[i].img_Core.fillAmount = this.listAttributeChar[i].valueCurrent / this.listAttributeChar[i].valueMax;
			this.popUpRankUp_AttributesCharNew[i].img_Core.fillAmount = this.listAttributeChar[i].valueCurrent / this.listAttributeChar[i].valueMax;
			this.popUpRankUp_AttributesCharOld[i].img_CoreMax.fillAmount = 0f;
			this.popUpRankUp_AttributesCharNew[i].img_CoreMax.fillAmount = this.listAttributeChar[i].valueNextRank / this.listAttributeChar[i].valueMax;
		}
		this.popUpRankUp_BtnRankUpTip.SetActive(this.obj_TipRankUp.activeSelf);
		this.popUpRankUp_txtRankUpFrag.text = ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)].ToString();
		this.popUpRankUp_txtRankUpGem.text = this.GemRankUp[ProfileManager.rambos[this.indexCharSelected].RankUpped].ToString();
		this.obj_PopUpRankUp.SetActive(true);
	}

	public void ClosePopupUpgrade()
	{
		
		this.obj_PopUpUpgrade.SetActive(false);
	}

	public void ClosePopupRankUp()
	{
		
		this.obj_PopUpRankUp.SetActive(false);
	}

	public void BtnUpgradeCharacter()
	{
		AudioClick.Instance.OnClick();
		
		Item itemID = ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade);
		if (itemID != Item.Gold)
		{
			if (itemID == Item.Gem)
			{
				if (ProfileManager.userProfile.Ms < ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade))
				{
					MenuManager.Instance.popupInformation.ShowWarning(ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), string.Empty);
					return;
				}
				PopupManager.Instance.SaveReward(ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), -ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), "UpgradeChar", null);
			}
		}
		else
		{
			if (ProfileManager.userProfile.Coin < ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade))
			{
				MenuManager.Instance.popupInformation.ShowWarning(ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), string.Empty);
				return;
			}
			PopupManager.Instance.SaveReward(ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), -ProfileManager.rambos[this.indexCharSelected].ValueUpgrade(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade), "UpgradeChar", null);
		}
		AudioClick.Instance.OnUpgrade();
		ProfileManager.rambos[this.indexCharSelected].LevelUpgrade++;
		this.ShowInforChar();
		this.CheckTips();
		if (ProfileManager.rambos[this.indexCharSelected].LevelUpgrade % 10 == 9)
		{
			this.obj_PopUpUpgrade.SetActive(false);
			if (this.obj_BtnRankUp.activeSelf)
			{
				this.BtnPopupRankUp();
			}
		}
		else
		{
			this.effUpgrade.ShowUpgrade(this.popUpUpgrade_CardsChar.transform, 0.5f);
			base.StartCoroutine(this.EffectButtonUpgrade(0.65f, this.popUpUpgrade_BtnUpgrade));
		}
		this.popUpUpgrade_txtName.text = ProfileManager.rambos[this.indexCharSelected].name;
		this.popUpUpgrade_txtLevel.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1
		});
		this.popUpUpgrade_txtLevelNext.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 2
		});
		this.popUpUpgrade_txtPower.text = ProfileManager.rambos[this.indexCharSelected].Power() + string.Empty;
		this.popUpUpgrade_txtPowerNext.text = ProfileManager.rambos[this.indexCharSelected].Power(Mathf.Min(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade + 1, 29)) + string.Empty;
		for (int i = 0; i < 3; i++)
		{
			this.popUpUpgrade_AttributesChar[i].txt_Amount.text = this.listAttributeChar[i].valueCurrent + string.Empty;
			if (this.listAttributeChar[i].valueNext > this.listAttributeChar[i].valueCurrent)
			{
				this.popUpUpgrade_AttributesChar[i].txt_ValueNext.text = this.listAttributeChar[i].valueNext + string.Empty;
			}
			this.popUpUpgrade_AttributesChar[i].img_Core.fillAmount = this.listAttributeChar[i].valueCurrent / this.listAttributeChar[i].valueMax;
			this.popUpUpgrade_AttributesChar[i].img_CoreMax.fillAmount = this.listAttributeChar[i].valueNext / this.listAttributeChar[i].valueMax;
		}
		this.popUpUpgrade_BtnUpgradeTip.SetActive(this.obj_TipUpgrade.activeSelf);
		this.popUpUpgrade_CardsChar.img_Main.sprite = this.listCardChar[this.indexCharSelected].img_Main.sprite;
		this.popUpUpgrade_txtUpgradeGold.text = this.txt_CostUpgradeChar.text;
		int num = ProfileManager.rambos[this.indexCharSelected].RankBase + ProfileManager.rambos[this.indexCharSelected].RankUpped;
		this.popUpUpgrade_CardsChar.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UpgradeCharacter)
		{
			MenuManager.Instance.tutorial.NextTutorial(2);
		}
		
		DailyQuestManager.Instance.MissionCharacterShop(null);
		this.imgMoney1.sprite = PopupManager.Instance.sprite_Item[(int)ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade)];
		this.imgMoney2.sprite = PopupManager.Instance.sprite_Item[(int)ProfileManager.rambos[this.indexCharSelected].GetItemID(ProfileManager.rambos[this.indexCharSelected].LevelUpgrade)];
	}

	public void BtnRankUpCharacter()
	{
		AudioClick.Instance.OnClick();
		
		if (ProfileManager.userProfile.Ms < this.GemRankUp[ProfileManager.rambos[this.indexCharSelected].RankUpped].Value)
		{
			MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, this.GemRankUp[ProfileManager.rambos[this.indexCharSelected].RankUpped].Value, string.Empty);
			return;
		}
		int value = ProfileManager.rambos[this.indexCharSelected].CurrencyRankUp.Value;
		string str = "metal.squad.frag.";
		Item item = (Item)value;
		if (PlayerPrefs.GetInt(str + item.ToString(), 0) < ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)])
		{
			MenuManager.Instance.popupInformation.ShowWarning((Item)value, ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)], string.Empty);
			return;
		}
		PopupManager.Instance.SaveReward(Item.Gem, -this.GemRankUp[ProfileManager.rambos[this.indexCharSelected].RankUpped].Value, base.name + "_RankUpFrag", null);
		PopupManager.Instance.SaveReward((Item)value, -ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)], base.name + "_RankUpFrag", null);
		AudioClick.Instance.OnBumb();
		ProfileManager.rambos[this.indexCharSelected].RankUpped++;
		this.ShowAvatarCharacters();
		this.ShowInforChar();
		this.CheckTips();
		this.obj_EffectRankUpChar.AnimationState.SetAnimation(0, "update", false);
		this.obj_PopUpRankUp.SetActive(false);
		base.StartCoroutine(this.EffectButtonUpgrade(0.6f, this.obj_Upgrade_RankUp));
		
	}

	public void BtnUnlockCharByGem()
	{
		AudioClick.Instance.OnClick();
		
		if (ProfileManager.userProfile.Ms < ProfileManager.rambos[this.indexCharSelected].GemUnlock.Value)
		{
			MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, ProfileManager.rambos[this.indexCharSelected].GemUnlock.Value, string.Empty);
			return;
		}
		AudioClick.Instance.OnBumb();
		PopupManager.Instance.SaveReward(Item.Gem, -ProfileManager.rambos[this.indexCharSelected].GemUnlock.Value, "UnlockCharacter:" + this.indexCharSelected, null);
		PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Character, this.indexCharSelected), 1, base.name + "UnlockByGem", null);
		ProfileManager.rambos[this.indexCharSelected].LevelUpgrade = 0;
		if (ProfileManager.InforChars[this.indexCharSelected].IsUnLocked)
		{
			ProfileManager.settingProfile.IDChar = this.indexCharSelected;
		}
		this.ShowAvatarCharacters();
		this.ShowInforChar();
		this.CheckTips();
		this.effUpgrade.ShowUpgrade(this.popUpUpgrade_CardsChar.transform, 0.5f);
		
		
		DailyQuestManager.Instance.MissionCharacterShop(null);
	}

	public void BtnUnlockCharByGold()
	{
		AudioClick.Instance.OnClick();
		
		if (ProfileManager.rambos[this.indexCharSelected].StarUnlock > ProfileManager._CampaignProfile.GetTotalStar)
		{
			PopupManager.Instance.ShowDialog(delegate(bool callback)
			{
			}, 0, "Require " + this.txt_ConditionUnlock.text + " star.", "Locked");
			return;
		}
		int value = ProfileManager.rambos[this.indexCharSelected].CurrencyUnlock.Value;
		string str = "metal.squad.frag.";
		Item item = (Item)value;
		if (PlayerPrefs.GetInt(str + item.ToString(), 0) < ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value)
		{
			MenuManager.Instance.popupInformation.ShowWarning((Item)value, ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value, string.Empty);
			return;
		}
		PopupManager.Instance.SaveReward((Item)value, -ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value, base.name + "_UnlockByFrag:" + this.indexCharSelected, null);
		AudioClick.Instance.OnBumb();
		PopupManager instance = PopupManager.Instance;
		Item item2 = (Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Character, this.indexCharSelected);
		int amount = 1;
		string name = base.name;
		string str2 = "_UnlockBy";
		Item item3 = (Item)value;
		instance.SaveReward(item2, amount, name + str2 + item3.ToString(), null);
		ProfileManager.rambos[this.indexCharSelected].LevelUpgrade = 0;
		if (ProfileManager.InforChars[this.indexCharSelected].IsUnLocked)
		{
			ProfileManager.settingProfile.IDChar = this.indexCharSelected;
		}
		this.ShowAvatarCharacters();
		this.ShowInforChar();
		this.CheckTips();
		this.effUpgrade.ShowUpgrade(this.popUpUpgrade_CardsChar.transform, 0.5f);
		
		DailyQuestManager.Instance.MissionCharacterShop(null);
	}

	public void BtnInforFrag()
	{
		if (this.obj_Upgrade_RankUp.activeSelf)
		{
			MenuManager.Instance.popupInformation.Show((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyRankUp.Value, ProfileManager.rambos[this.indexCharSelected].CostRankUp[Mathf.Min(1, ProfileManager.rambos[this.indexCharSelected].RankUpped)]);
		}
		else if (this.obj_Unlock.activeSelf)
		{
			MenuManager.Instance.popupInformation.Show((Item)ProfileManager.rambos[this.indexCharSelected].CurrencyUnlock.Value, ProfileManager.rambos[this.indexCharSelected].GoldUnlock.Value);
		}
	}

	public void BtnPassive(int index)
	{
	}

	private SecuredInt[] GemRankUp;

	public int indexCharSelected;

	[Header("---------------Top UI---------------")]
	public Color color_Enable;

	public Image img_Skill;

	public Sprite[] sprite_Skill;

	public Text txt_Passive1;

	public Text txt_Passive2;

	[Header("---------------Left UI---------------")]
	public RectTransform rect_Left;

	public CardChar[] listCardChar;

	public RectTransform rect_Right;

	[Header("---------------Tab:Infor Character---------------")]
	public Text txt_NameChar;

	public Image img_RankChar;

	public Image img_BGNameChar;

	public Text txt_PowerChar;

	public Text txt_CostUpgradeChar;

	public Text txt_CostRankUpChar;

	public Text txt_CostRankUpCharGem;

	public Text txt_CostFrag;

	public Text txt_CostUnlockByGem;

	public Text txt_CostUnlockByFrag;

	public Text txt_ConditionUnlock;

	public Image img_ProcessFrag;

	public GameObject obj_Unlock;

	public GameObject obj_Upgrade_RankUp;

	public GameObject obj_BtnUpgrade;

	public GameObject obj_BtnRankUp;

	public GameObject obj_MaximumChar;

	public GameObject obj_Frag;

	public GameObject obj_TipUnlockByGem;

	public GameObject obj_TipUnlockByGold;

	public GameObject obj_TipUpgrade;

	public GameObject obj_TipRankUp;

	public CardAttributeWeapon[] listAttributeChar;

	public float[] listMaxChar;

	public EffectUpgrade effUpgrade;

	public SkeletonGraphic obj_EffectRankUpChar;

	private int indexCharSelectedTutorial;

	public GameObject obj_PopupTutorial;

	public CardChar[] listCardCharTut;

	[Header("---------------PopUpUpgrade---------------")]
	public CardChar popUpUpgrade_CardsChar;

	public CardAttributeWeapon[] popUpUpgrade_AttributesChar;

	public Text popUpUpgrade_txtName;

	public Text popUpUpgrade_txtLevel;

	public Text popUpUpgrade_txtLevelNext;

	public Text popUpUpgrade_txtPower;

	public Text popUpUpgrade_txtPowerNext;

	public Text popUpUpgrade_txtUpgradeGold;

	public GameObject popUpUpgrade_BtnUpgrade;

	public GameObject popUpUpgrade_BtnUpgradeTip;

	public GameObject obj_PopUpUpgrade;

	[Header("---------------PopUpRankUp---------------")]
	public CardChar popUpRankUp_CardsCharOld;

	public CardChar popUpRankUp_CardsCharNew;

	public CardAttributeWeapon[] popUpRankUp_AttributesCharOld;

	public CardAttributeWeapon[] popUpRankUp_AttributesCharNew;

	public Text popUpRankUp_txtNameRankOld;

	public Text popUpRankUp_txtNameRankNew;

	public Image popUpRankUp_ImgRankOld;

	public Image popUpRankUp_ImgRankNew;

	public Text popUpRankUp_txtRankUpFrag;

	public Text popUpRankUp_txtRankUpGem;

	public GameObject popUpRankUp_BtnRankUp;

	public GameObject popUpRankUp_BtnRankUpTip;

	public GameObject obj_PopUpRankUp;

	public Image imgMoney1;

	public Image imgMoney2;
}
