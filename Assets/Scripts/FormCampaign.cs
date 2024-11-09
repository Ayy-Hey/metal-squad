using System;
using StarMission;
using UnityEngine;
using UnityEngine.UI;

public class FormCampaign : FormBase
{
	private void OnValidate()
	{
		if (this.onValidate && !Application.isPlaying)
		{
			ProfileManager.init("thanhnh", "151756776", 983168606);
			SplashScreen._isLoadResourceDecrypt = true;
			DataLoader.LoadData();
			DataLoader.LoadDataCampaign();
			for (int i = 0; i < this.cardsCampaign.Length; i++)
			{
				this.cardsCampaign[i].idCard = i;
				for (int j = 0; j < this.cardsCampaign[i].levelWorldMap.Length; j++)
				{
					this.cardsCampaign[i].levelWorldMap[j].isStartLevelOfMap = (j == 0);
					this.cardsCampaign[i].levelWorldMap[j].Map = i;
					this.cardsCampaign[i].levelWorldMap[j].Level = j;
					this.cardsCampaign[i].levelWorldMap[j].eLevel = (ELevel)(i * 12 + j);
					this.cardsCampaign[i].levelWorldMap[j].idBoss = DataLoader.missionDataRoot_Normal[this.cardsCampaign[i].idCard * 12 + j].missionDataLevel.idBoss;
					this.cardsCampaign[i].levelWorldMap[j].gameObject.name = string.Concat(new object[]
					{
						"Level",
						this.cardsCampaign[i].idCard + 1,
						"-",
						j + 1
					});
					if (j + 1 < this.cardsCampaign[i].levelWorldMap.Length)
					{
						this.cardsCampaign[i].levelWorldMap[j].tran_LevelNext = this.cardsCampaign[i].levelWorldMap[j + 1].transform;
					}
					else
					{
						this.cardsCampaign[i].levelWorldMap[j].tran_Line.gameObject.SetActive(false);
					}
					if (this.cardsCampaign[i].levelWorldMap[j].idBoss >= 0)
					{
						GameObject gameObject = this.cardsCampaign[i].levelWorldMap[j].gameObject;
						gameObject.name += "Boss";
						this.cardsCampaign[i].levelWorldMap[j].mode = LevelMode.BOSS;
						this.cardsCampaign[i].levelWorldMap[j].imgLevelUnlock = this.sprite_LevelBossUnlock;
						this.cardsCampaign[i].levelWorldMap[j].imgLevelLocked = this.sprite_LevelBossLock;
					}
					else
					{
						this.cardsCampaign[i].levelWorldMap[j].mode = LevelMode.NORMAL;
						this.cardsCampaign[i].levelWorldMap[j].imgLevelUnlock = this.sprite_LevelNormalUnlock;
						this.cardsCampaign[i].levelWorldMap[j].imgLevelLocked = this.sprite_LevelNormalLock;
					}
					if (this.cardsCampaign[i].levelWorldMap[j].img_line != null && this.cardsCampaign[i].levelWorldMap[j].tran_LevelNext != null)
					{
						this.cardsCampaign[i].levelWorldMap[j].tran_Line.LookAt(this.cardsCampaign[i].levelWorldMap[j].tran_LevelNext);
						if (this.cardsCampaign[i].levelWorldMap[j].tran_Line.localEulerAngles.y > 1f)
						{
							this.cardsCampaign[i].levelWorldMap[j].tran_Line.Rotate(0f, 0f, 90f, Space.Self);
						}
						this.cardsCampaign[i].levelWorldMap[j].img_line.rectTransform.sizeDelta = new Vector2(Vector3.Distance(this.cardsCampaign[i].levelWorldMap[j].tran_LevelNext.position, this.cardsCampaign[i].levelWorldMap[j].img_line.transform.position) * 100f, 28f);
					}
				}
			}
		}
	}

	public override void Show()
	{
		base.Show();
		if (AutoCheckLevel.isAutoCheck)
		{
			if (AutoCheckLevel.LevelCampaign < AutoCheckLevel.CheckToLevelCampaign - 1)
			{
				MenuManager.IDDifficultSelect = 0;
				int iddifficultSelect = MenuManager.IDDifficultSelect;
				MenuManager.IDLevelSelect = AutoCheckLevel.LevelCampaign % 12;
				MenuManager.IDMapSelect = AutoCheckLevel.LevelCampaign / 12;
				this.ChangeSelectMap(this.cardsCampaign[MenuManager.IDMapSelect]);
				this.ChangeDifficultMode(iddifficultSelect);
				this.ChangeSelectLevel(this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[MenuManager.IDLevelSelect]);
				this.OnStart();
			}
			else
			{
				AutoCheckLevel.LevelCampaign = AutoCheckLevel.CheckToLevelCampaign;
				MenuManager.Instance.BackForm();
			}
		}
		this.isFirst = true;
		float num = Mathf.Clamp(((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f, 0f, 1f);
		if (num > 0.9f)
		{
			this.obj_MapSelect.SetActive(true);
			this.rect_ListMap.localPosition = new Vector3(849f, this.rect_ListMap.localPosition.y, this.rect_ListMap.localPosition.z);
		}
		else
		{
			this.obj_MapSelect.SetActive(false);
			this.rect_ListMap.localPosition = new Vector3(612f, this.rect_ListMap.localPosition.y, this.rect_ListMap.localPosition.z);
		}
		this.twn_TableStar.to = new Vector3(-160f - 30f * num, this.twn_TableStar.to.y, 0f);
		if (ThisPlatform.IsIphoneX)
		{
			this.rect_Top.localPosition = new Vector3(70f, this.rect_Top.localPosition.y, 0f);
			this.popupDifficultMode.transform.localPosition = new Vector3(70f, this.popupDifficultMode.transform.localPosition.y, 0f);
			this.twn_Infor.to = new Vector3(this.twn_Infor.from.x - 500f, this.twn_Infor.from.y, 0f);
		}
		else
		{
			this.rect_Top.localPosition = new Vector3(0f, this.rect_Top.localPosition.y, 0f);
			this.popupDifficultMode.transform.localPosition = new Vector3(0f, this.popupDifficultMode.transform.localPosition.y, 0f);
			this.twn_Infor.to = new Vector3(this.twn_Infor.from.x - 450f, this.twn_Infor.from.y, 0f);
		}
		if (this.isChoosingDifficult)
		{
			this.ChangeDifficult();
		}
		this.effSelectLevel.Hide();
		int num2 = 0;
		for (int i = 0; i < this.cardsCampaign.Length; i++)
		{
			this.cardsCampaign[i]._mapProfile = ProfileManager.worldMapProfile[i];
			if (!ProfileManager._CampaignProfile.MapsProfile[i * 12].GetUnlocked(GameMode.Mode.NORMAL))
			{
				this.cardsCampaign[i].obj_Lock.SetActive(true);
				this.cardsCampaign[i].obj_starOfMap.SetActive(false);
			}
			else
			{
				this.cardsCampaign[i].obj_Lock.SetActive(false);
				this.cardsCampaign[i].obj_starOfMap.SetActive(true);
				num2 = i;
			}
		}
		GameMode.Instance.CheckUnlockDifficulty();
		if (FormCampaign.isCampaignContinue)
		{
			FormCampaign.isCampaignContinue = false;
			this.ChangeSelectMap(this.cardsCampaign[ProfileManager.MapCurrent]);
			this.ChangeDifficultMode((int)GameMode.Instance.MODE);
			this.ChangeSelectLevel(this.cardsCampaign[ProfileManager.MapCurrent].levelWorldMap[ProfileManager.LevelCurrent]);
		}
		else if (MenuManager.formLast != FormUI.None && (MenuManager.formLast == FormUI.UILoadOut || MenuManager.formLast == FormUI.UICharacter || MenuManager.formLast == FormUI.UIWeapon))
		{
			int iddifficultSelect2 = MenuManager.IDDifficultSelect;
			this.ChangeSelectMap(this.cardsCampaign[MenuManager.IDMapSelect]);
			this.ChangeDifficultMode(iddifficultSelect2);
			if (MenuManager.formLast == FormUI.UICharacter || MenuManager.formLast == FormUI.UIWeapon)
			{
				this.EffectSelectLevelLast(MenuManager.IDMapSelect);
				int totalStarInMapMode = ProfileManager._CampaignProfile.GetTotalStarInMapMode(MenuManager.IDMapSelect, (GameMode.Mode)iddifficultSelect2);
				this.OpenPopupStarGift(totalStarInMapMode / 12, iddifficultSelect2);
			}
			else
			{
				this.ChangeSelectLevel(this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[MenuManager.IDLevelSelect]);
			}
		}
		else
		{
			this.ChangeSelectMap(this.cardsCampaign[num2]);
			this.EffectSelectLevelLast(num2);
		}
		if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[2].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && ProfileManager._CampaignProfile.MapsProfile[0].GetStar(GameMode.Mode.NORMAL) == 0)
		{
			this.ClosePopupActiveLevel();
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_FormCampaign_1);
		}
		else if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[7].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && this.inforStarGift[0].obj_Open.activeSelf)
		{
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_RewardStar);
		}
		else if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[9].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && !this.cardsCampaign[1].obj_Lock.activeSelf && ProfileManager._CampaignProfile.MapsProfile[12].GetStar(GameMode.Mode.NORMAL) <= 0)
		{
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_UnlockMap);
		}
		
		this.isFirst = false;
		FormCampaign.isCampaignContinue = false;
	}

	private void ChangeDifficultMode(int index)
	{
		if (this.cardsDifficultMode[index].obj_Lock.activeSelf && !ProfileManager.unlockAll && index != 0)
		{
			PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.You_need_complete_previous_difficulties, null), PopupManager.Instance.GetText(Localization0.Locked, null));
			this.ChangeDifficult();
			return;
		}
		this.cardsDifficultMode[MenuManager.IDDifficultSelect].obj_effectCamera.SetActive(false);
		this.cardsDifficultMode[index].obj_effectCamera.SetActive(true);
		MenuManager.IDDifficultSelect = index;
		this.cardsCampaign[MenuManager.IDMapSelect].obj_Map.SetActive(true);
		for (int i = 0; i < this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap.Length; i++)
		{
			this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[i].OnShow((GameMode.Mode)MenuManager.IDDifficultSelect);
		}
		this.myCardDifficultMode.img_Main.sprite = this.cardsDifficultMode[MenuManager.IDDifficultSelect].img_Main.sprite;
		this.myCardDifficultMode.txt_Name.text = this.cardsDifficultMode[MenuManager.IDDifficultSelect].txt_Name.text;
		this.myCardDifficultMode.img_BG.sprite = this.cardsDifficultMode[MenuManager.IDDifficultSelect].img_BG.sprite;
		this.myCardDifficultMode.obj_Lock.SetActive(this.cardsDifficultMode[MenuManager.IDDifficultSelect].obj_Lock.activeSelf);
		this.ShowTableInfor(false);
		this.LoadStarGift();
		if (this.isChoosingDifficult)
		{
			this.ChangeDifficult();
		}
	}

	private void ChangeSelectMap(CardCampaign card)
	{
		if (MenuManager.IDMapSelect != card.idCard || this.isFirst)
		{
			if (MenuManager.IDMapSelect != -1)
			{
				this.cardsCampaign[MenuManager.IDMapSelect].rectTransform().localScale = Vector3.one;
				this.cardsCampaign[MenuManager.IDMapSelect].img_BG.color = new Color(0.4745098f, 0.498039216f, 0.5254902f);
			}
			this.cardsCampaign[MenuManager.IDMapSelect].obj_Map.SetActive(false);
			MenuManager.IDMapSelect = card.idCard;
			this.cardsCampaign[MenuManager.IDMapSelect].rectTransform().localScale = Vector3.one * 1.2f;
			this.cardsCampaign[MenuManager.IDMapSelect].img_BG.color = Color.white;
			if (this.cardsCampaign[MenuManager.IDMapSelect].obj_Lock.activeSelf)
			{
				if (!ProfileManager.unlockAll)
				{
					this.obj_Lock.SetActive(true);
				}
			}
			else
			{
				this.obj_Lock.SetActive(false);
			}
			for (int i = 0; i < this.cardsDifficultMode.Length; i++)
			{
				if (ProfileManager._CampaignProfile.MapsProfile[MenuManager.IDMapSelect * 12].GetUnlocked((GameMode.Mode)i))
				{
					this.cardsDifficultMode[i].obj_Lock.SetActive(false);
					this.cardsDifficultMode[i].img_BG.sprite = this.cardsDifficultMode[i].sprite_BtnBaseEnable;
				}
				else
				{
					this.cardsDifficultMode[i].obj_Lock.SetActive(true);
					this.cardsDifficultMode[i].img_BG.sprite = this.cardsDifficultMode[i].sprite_BtnBaseDisable;
				}
			}
			this.ChangeDifficultMode(0);
			this.cardsCampaign[MenuManager.IDMapSelect].obj_Map.SetActive(true);
			for (int j = 0; j < this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap.Length; j++)
			{
				this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[j].OnShow((GameMode.Mode)MenuManager.IDDifficultSelect);
			}
			this.ClosePopupActiveLevel();
			this.LoadStarGift();
			this.effSelectLevel.Hide();
		}
	}

	private void ChangeSelectLevel(LevelWorldMap levelWorldMap)
	{
		levelWorldMap.transform.SetAsLastSibling();
		this.popupActiveLevel.transform.SetParent(levelWorldMap.transform);
		this.popupActiveLevel.transform.SetAsFirstSibling();
		this.popupActiveLevel.transform.localPosition = Vector3.zero;
		ProfileManager.eLevelCurrent = levelWorldMap.eLevel;
		LevelMode mode = levelWorldMap.mode;
		if (mode != LevelMode.NORMAL && mode != LevelMode.BOSS)
		{
			if (mode == LevelMode.SPECIAL)
			{
				GameMode.Instance.modePlay = GameMode.ModePlay.Special_Campaign;
				MenuManager.IDLevelSelect = levelWorldMap.Level;
			}
		}
		else
		{
			GameMode.Instance.modePlay = GameMode.ModePlay.Campaign;
			MenuManager.IDLevelSelect = levelWorldMap.Level;
		}
		this.effSelectLevel.Show(levelWorldMap.transform, levelWorldMap.mode == LevelMode.BOSS);
		this.ShowTableInfor(true);
	}

	private void OnStart()
	{
		if (!ProfileManager.unlockAll)
		{
			if (this.cardsCampaign[MenuManager.IDMapSelect].obj_Lock.activeSelf)
			{
				PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.You_must_complete_the_previous_stages, null), PopupManager.Instance.GetText(Localization0.Locked, null));
				return;
			}
			if (MenuManager.IDLevelSelect != 0 && ProfileManager._CampaignProfile.MapsProfile[MenuManager.IDMapSelect * 12 + MenuManager.IDLevelSelect - 1].GetStar((GameMode.Mode)MenuManager.IDDifficultSelect) == 0)
			{
				PopupManager.Instance.ShowDialog(null, 0, PopupManager.Instance.GetText(Localization0.You_must_complete_the_previous_stages, null), PopupManager.Instance.GetText(Localization0.Locked, null));
				return;
			}
			if (this.cardsDifficultMode[MenuManager.IDDifficultSelect].obj_Lock.activeSelf)
			{
				return;
			}
		}
		this.GoLoadOut();
	}

	private void GoLoadOut()
	{
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay != GameMode.ModePlay.Campaign)
		{
			if (modePlay == GameMode.ModePlay.Special_Campaign)
			{
				FormLoadout.typeForm = FormLoadout.Type.Special_Campaign;
				DataLoader.LoadLevelJsonData("Map/LevelS" + (ProfileManager.eLevelCurrent - ELevel.LEVEL_S0));
			}
		}
		else
		{
			FormLoadout.typeForm = FormLoadout.Type.Campaign;
		}
		MenuManager.Instance.ChangeForm(FormUI.UILoadOut, FormUI.UICampaign);
	}

	private void ChangeDifficult()
	{
		if (!this.isChoosingDifficult)
		{
			if (!this.myCardDifficultMode.obj_Lock.activeSelf || ProfileManager.unlockAll)
			{
				this.isChoosingDifficult = true;
				this.myCardDifficultMode.obj_Active.SetActive(true);
				this.popupDifficultMode.SetActive(true);
				
			}
		}
		else
		{
			this.isChoosingDifficult = false;
			this.myCardDifficultMode.obj_Active.SetActive(false);
			this.popupDifficultMode.SetActive(false);
		}
	}

	private void LoadStarGift()
	{
		int num = 0;
		for (int i = 0; i < this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap.Length; i++)
		{
			try
			{
				num += this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[i].amountStarActive[MenuManager.IDDifficultSelect];
			}
			catch
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"????",
					MenuManager.IDMapSelect,
					"-",
					i
				}));
			}
		}
		this.txtStarGift.text = num + " / 36";
		this.inforStarGift[0].obj_Open.SetActive(false);
		this.inforStarGift[1].obj_Open.SetActive(false);
		this.inforStarGift[2].obj_Open.SetActive(false);
		this.inforStarGift[0].obj_Active.SetActive(false);
		this.inforStarGift[1].obj_Active.SetActive(false);
		this.inforStarGift[2].obj_Active.SetActive(false);
		this.inforStarGift[0].obj_Lock.SetActive(true);
		this.inforStarGift[1].obj_Lock.SetActive(true);
		this.inforStarGift[2].obj_Lock.SetActive(true);
		this.inforStarGift[0].img_Main.sprite = this.sprite_GiftNotDone;
		this.inforStarGift[1].img_Main.sprite = this.sprite_GiftNotDone;
		this.inforStarGift[2].img_Main.sprite = this.sprite_GiftNotDone;
		if (num < 12)
		{
			this.inforStarGift[0].img_Core.fillAmount = (float)num / 12f;
			this.inforStarGift[1].img_Core.fillAmount = 0f;
			this.inforStarGift[2].img_Core.fillAmount = 0f;
		}
		else if (num < 24)
		{
			this.CheckClaimStarReward(0);
			this.inforStarGift[0].img_Core.fillAmount = 1f;
			this.inforStarGift[1].img_Core.fillAmount = (float)(num - 12) / 12f;
			this.inforStarGift[2].img_Core.fillAmount = 0f;
		}
		else if (num < 36)
		{
			this.CheckClaimStarReward(0);
			this.CheckClaimStarReward(1);
			this.inforStarGift[0].img_Core.fillAmount = 1f;
			this.inforStarGift[1].img_Core.fillAmount = 1f;
			this.inforStarGift[2].img_Core.fillAmount = (float)(num - 24) / 12f;
		}
		else
		{
			this.CheckClaimStarReward(0);
			this.CheckClaimStarReward(1);
			this.CheckClaimStarReward(2);
			this.inforStarGift[0].img_Core.fillAmount = 1f;
			this.inforStarGift[1].img_Core.fillAmount = 1f;
			this.inforStarGift[2].img_Core.fillAmount = 1f;
		}
	}

	private void CheckClaimStarReward(int sttStarGift)
	{
		UnityEngine.Debug.Log("IDMAP:" + MenuManager.IDMapSelect);
		if ((MenuManager.IDDifficultSelect == 0 && ProfileManager.worldMapProfile[MenuManager.IDMapSelect].GetClaimedGiftNormal(sttStarGift)) || (MenuManager.IDDifficultSelect == 1 && ProfileManager.worldMapProfile[MenuManager.IDMapSelect].GetClaimedGiftHard(sttStarGift)) || (MenuManager.IDDifficultSelect == 2 && ProfileManager.worldMapProfile[MenuManager.IDMapSelect].GetClaimedGiftSuperHard(sttStarGift)))
		{
			this.inforStarGift[sttStarGift].obj_Open.SetActive(false);
			this.inforStarGift[sttStarGift].obj_Active.SetActive(false);
			this.inforStarGift[sttStarGift].obj_Lock.SetActive(false);
			this.inforStarGift[sttStarGift].img_Main.sprite = this.sprite_GiftDone;
		}
		else
		{
			this.inforStarGift[sttStarGift].obj_Open.SetActive(true);
			this.inforStarGift[sttStarGift].obj_Active.SetActive(true);
			this.inforStarGift[sttStarGift].obj_Lock.SetActive(false);
			this.inforStarGift[sttStarGift].img_Main.sprite = this.sprite_GiftClaim;
		}
	}

	private void ShowTableInfor(bool turnOnEffect)
	{
		if (turnOnEffect)
		{
			this.twn_Infor.PlayForward();
			this.twn_Infor2.PlayForward();
			this.twn_TableStar.PlayForward();
			this.popupActiveLevel.SetActive(true);
			this.cardsCampaign[MenuManager.IDMapSelect].twn_ListLevel.to = -this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[MenuManager.IDLevelSelect].transform.localPosition * 1.35f;
			this.cardsCampaign[MenuManager.IDMapSelect].twn_ListLevel.PlayForward();
			this.cardsCampaign[MenuManager.IDMapSelect].twn_ListLevelScale.PlayForward();
		}
		string text = (GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? PopupManager.Instance.GetText(Localization0.Complete_Stage, null) : PopupManager.Instance.GetText(Localization0.Stage, null);
		text += " ";
		text += (MenuManager.IDMapSelect + 1).ToString();
		text += " - ";
		text += (MenuManager.IDLevelSelect + 1).ToString();
		this.txtLevelTitle.text = text;
		this.listReward[0].img_Main.sprite = PopupManager.Instance.sprite_Item[1];
		this.listReward[0].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Gold)];
		this.listReward[0].item = Item.Gold;
		this.listReward[0].ShowBorderEffect();
		if (this.cardsCampaign[MenuManager.IDMapSelect].levelWorldMap[MenuManager.IDLevelSelect].mode == LevelMode.BOSS && ProfileManager._CampaignProfile.MapsProfile[MenuManager.IDLevelSelect + MenuManager.IDMapSelect * 12].GetStar((GameMode.Mode)MenuManager.IDDifficultSelect) < 3)
		{
			this.listReward[1].item = Item.Gem;
			this.listReward[1].img_Main.sprite = PopupManager.Instance.sprite_Item[4];
			this.listReward[1].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(Item.Gem)];
			this.listReward[1].gameObject.SetActive(true);
			this.listReward[1].ShowBorderEffect();
		}
		else
		{
			this.listReward[1].gameObject.SetActive(false);
		}
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay != GameMode.ModePlay.Campaign)
		{
			if (modePlay == GameMode.ModePlay.Special_Campaign)
			{
				int iddifficultSelect = MenuManager.IDDifficultSelect;
				if (iddifficultSelect != 0)
				{
					if (iddifficultSelect != 1)
					{
						if (iddifficultSelect == 2)
						{
							this.ShowStarInfor(DataLoader.missionDataRoot_SuperHard_S);
						}
					}
					else
					{
						this.ShowStarInfor(DataLoader.missionDataRoot_Hard_S);
					}
				}
				else
				{
					this.ShowStarInfor(DataLoader.missionDataRoot_Normal_S);
				}
				if (ProfileManager.MapSpecialProfiles[ProfileManager.eLevelCurrent - ELevel.LEVEL_S0].GetUnlocked((GameMode.Mode)MenuManager.IDDifficultSelect))
				{
					this.obj_BtnStartActive.SetActive(true);
				}
				else
				{
					this.obj_BtnStartActive.SetActive(false);
				}
			}
		}
		else
		{
			int iddifficultSelect2 = MenuManager.IDDifficultSelect;
			if (iddifficultSelect2 != 0)
			{
				if (iddifficultSelect2 != 1)
				{
					if (iddifficultSelect2 == 2)
					{
						this.ShowStarInfor(DataLoader.missionDataRoot_SuperHard);
					}
				}
				else
				{
					this.ShowStarInfor(DataLoader.missionDataRoot_Hard);
				}
			}
			else
			{
				this.ShowStarInfor(DataLoader.missionDataRoot_Normal);
			}
			if (ProfileManager._CampaignProfile.MapsProfile[(int)ProfileManager.eLevelCurrent].GetUnlocked((GameMode.Mode)MenuManager.IDDifficultSelect))
			{
				this.obj_BtnStartActive.SetActive(true);
			}
			else
			{
				this.obj_BtnStartActive.SetActive(false);
			}
		}
	}

	private void ShowStarInfor(MissionDataRoot[] missionDataRoot)
	{
		for (int i = 0; i < 3; i++)
		{
			MissionData missionData = missionDataRoot[MenuManager.IDLevelSelect + MenuManager.IDMapSelect * 12].missionDataLevel.missionData[i];
			this.starInfor[i].txt_Description.text = PopupManager.Instance.GetText((Localization0)missionData.idDesc, missionData.valueDesc);
			if (missionDataRoot[MenuManager.IDLevelSelect + MenuManager.IDMapSelect * 12].missionDataLevel.missionData[i].IsCompleted)
			{
				this.starInfor[i].img_Main.sprite = this.sprite_StarShow;
				this.starInfor[i].obj_Open.SetActive(true);
				this.starInfor[i].obj_Lock.SetActive(false);
				this.starInfor[i].txt_Amount.gameObject.SetActive(false);
				this.starInfor[i].txt_Lock.gameObject.SetActive(false);
				this.starInfor[i].img_Lock.gameObject.SetActive(false);
			}
			else
			{
				this.starInfor[i].img_Main.sprite = this.sprite_StarHide;
				this.starInfor[i].obj_Open.SetActive(false);
				this.starInfor[i].obj_Lock.SetActive(true);
				this.starInfor[i].txt_Amount.gameObject.SetActive(true);
				this.starInfor[i].txt_Lock.gameObject.SetActive(true);
				this.starInfor[i].img_Lock.gameObject.SetActive(true);
				string text = missionDataRoot[MenuManager.IDLevelSelect + MenuManager.IDMapSelect * 12].missionDataLevel.missionData[i].gold_security.Value.ToString();
				this.starInfor[i].txt_Amount.text = text;
				this.starInfor[i].txt_Lock.text = missionDataRoot[MenuManager.IDLevelSelect + MenuManager.IDMapSelect * 12].missionDataLevel.missionData[i].exp_security.Value.ToString();
			}
		}
	}

	public void ClosePopupActiveLevel()
	{
		this.popupActiveLevel.SetActive(false);
		this.twn_Infor.ResetToBeginning();
		this.twn_Infor2.ResetToBeginning();
		this.twn_TableStar.ResetToBeginning();
		this.cardsCampaign[MenuManager.IDMapSelect].twn_ListLevel.ResetToBeginning();
		this.cardsCampaign[MenuManager.IDMapSelect].twn_ListLevelScale.ResetToBeginning();
	}

	private void EffectSelectLevelLast(int idMapCur)
	{
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			if (ProfileManager._CampaignProfile.MapsProfile[idMapCur * 12 + i].GetStar((GameMode.Mode)MenuManager.IDDifficultSelect) > 0 && i + 1 < 12)
			{
				num = i + 1;
			}
		}
		this.effSelectLevel.Show(this.cardsCampaign[idMapCur].levelWorldMap[num].transform, this.cardsCampaign[idMapCur].levelWorldMap[num].mode == LevelMode.BOSS);
	}

	public void OpenPopupStarGift(int index, int indexDifficult)
	{
		float num = (float)(12 * (index + 1));
		int totalStarInMapMode = ProfileManager._CampaignProfile.GetTotalStarInMapMode(MenuManager.IDMapSelect, (GameMode.Mode)indexDifficult);
		this.txt_RewardStar.text = totalStarInMapMode + "/" + num;
		this.imgLineStarReward.fillAmount = (float)totalStarInMapMode / num;
		listRewardByAmountStar listRewardByAmountStar = DataLoader.starGift.listMap[MenuManager.IDMapSelect].listRewardByDifficult[indexDifficult].listRewardByAmountStar[index];
		this.obj_PopupRewardStar.SetActive(true);
		for (int i = 0; i < listRewardByAmountStar.id.Length; i++)
		{
			this.listRewards[i].txt_Amount.text = listRewardByAmountStar.value[i] + string.Empty;
			this.listRewards[i].item = (Item)listRewardByAmountStar.id[i];
			this.listRewards[i].img_Main.sprite = PopupManager.Instance.sprite_Item[listRewardByAmountStar.id[i]];
			if (this.listRewards[i].img_BG != null)
			{
				this.listRewards[i].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(this.listRewards[i].item)];
			}
			this.listRewards[i].ShowBorderEffect();
		}
	}

	public void BtnDifficultMode(int index)
	{
		AudioClick.Instance.OnClick();
		
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UnlockDifficult)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		this.ChangeDifficultMode(index);
		this.EffectSelectLevelLast(MenuManager.IDMapSelect);
	}

	public void BtnPopupDifficult()
	{
		AudioClick.Instance.OnClick();
		
		this.ChangeDifficult();
	}

	public void BtnMap(CardCampaign card)
	{
		AudioClick.Instance.OnClick();
		
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UnlockMap)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		this.ChangeSelectMap(card);
		this.EffectSelectLevelLast(card.idCard);
	}

	public void BtnLevel(LevelWorldMap levelWorldMap)
	{
		AudioClick.Instance.OnClick();
		
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormCampaign_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		this.ChangeSelectLevel(levelWorldMap);
	}

	public void BtnStart()
	{
		AudioClick.Instance.OnClick();
		
		this.OnStart();
	}

	public void BtnClaimStarGift(CardBase card)
	{
		AudioClick.Instance.OnClick();
		
		this.inforStarGift[card.idCard].obj_Open.SetActive(false);
		this.inforStarGift[card.idCard].obj_Active.SetActive(false);
		this.inforStarGift[card.idCard].img_Main.sprite = this.sprite_GiftDone;
		if (MenuManager.IDDifficultSelect == 0)
		{
			ProfileManager.worldMapProfile[MenuManager.IDMapSelect].SetClaimedGiftNormal(card.idCard, true);
		}
		else if (MenuManager.IDDifficultSelect == 1)
		{
			ProfileManager.worldMapProfile[MenuManager.IDMapSelect].SetClaimedGiftHard(card.idCard, true);
		}
		else if (MenuManager.IDDifficultSelect == 2)
		{
			ProfileManager.worldMapProfile[MenuManager.IDMapSelect].SetClaimedGiftSuperHard(card.idCard, true);
		}
		listRewardByAmountStar listRewardByAmountStar = DataLoader.starGift.listMap[MenuManager.IDMapSelect].listRewardByDifficult[MenuManager.IDDifficultSelect].listRewardByAmountStar[card.idCard];
		InforReward[] array = new InforReward[listRewardByAmountStar.id.Length];
		for (int i = 0; i < listRewardByAmountStar.id.Length; i++)
		{
			array[i] = new InforReward();
			array[i].amount = listRewardByAmountStar.value[i];
			array[i].item = (Item)listRewardByAmountStar.id[i];
			PopupManager.Instance.SaveReward(array[i].item, array[i].amount, string.Concat(new object[]
			{
				base.name,
				"_ClaimStarGift_Map:",
				MenuManager.IDMapSelect,
				"_Pack:",
				card.idCard,
				"_Id:",
				i
			}), null);
		}
		PopupManager.Instance.ShowCongratulation(array, true, null);
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_RewardStar)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		PlayerPrefs.SetInt(MenuManager.Instance.tutorial.listTutorialUI[7].keyPlayerPrefs, 1);
	}

	public void BtnRewardStar(CardBase card)
	{
		AudioClick.Instance.OnClick();
		
		this.OpenPopupStarGift(card.idCard, MenuManager.IDDifficultSelect);
	}

	public void BtnCloseRewardStar()
	{
		AudioClick.Instance.OnClick();
		
		this.obj_PopupRewardStar.SetActive(false);
	}

	private bool isFirst;

	private bool isChoosingDifficult;

	[Header("-----Top-----")]
	public CardCampaign[] cardsCampaign;

	public RectTransform rect_ListMap;

	public GameObject obj_MapSelect;

	public RectTransform rect_Top;

	[Header("-----Mid-----")]
	public EffSelectLevel effSelectLevel;

	public GameObject obj_Lock;

	public Text txt_StarUnlockInfor;

	public Sprite sprite_LevelNormalUnlock;

	public Sprite sprite_LevelNormalLock;

	public Sprite sprite_LevelBossUnlock;

	public Sprite sprite_LevelBossLock;

	[Header("-----Bot-----")]
	public Text txtStarGift;

	public CardBase[] inforStarGift;

	public Sprite sprite_GiftNotDone;

	public Sprite sprite_GiftClaim;

	public Sprite sprite_GiftDone;

	public TweenPosition twn_TableStar;

	[Header("-----Left-----")]
	public CardDifficultMode myCardDifficultMode;

	public CardDifficultMode[] cardsDifficultMode;

	public GameObject popupDifficultMode;

	public GameObject popupActiveLevel;

	[Header("-----Right-----")]
	public TweenPosition twn_Infor;

	public TweenAlpha twn_Infor2;

	public Text txtLevelTitle;

	public CardBase[] listReward;

	public Sprite[] spriteBonusCoin;

	public CardBase[] starInfor;

	public Sprite sprite_StarShow;

	public Sprite sprite_StarHide;

	[SerializeField]
	private Text txtPowerCurrent;

	[SerializeField]
	private Text txtPowerRequire;

	public GameObject obj_BtnStartActive;

	public Image img_Start;

	public static bool isCampaignContinue;

	[Header("-----PopupRewardStar-----")]
	public Text txt_RewardStar;

	public CardReward[] listRewards;

	public GameObject obj_PopupRewardStar;

	public Image imgLineStarReward;

	public bool onValidate;
}
