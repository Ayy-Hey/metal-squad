using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PVPManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormLoadout : FormBase
{
	public void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEventReceived;
	}

	public override void Show()
	{
		base.Show();
		float num = Mathf.Clamp(((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f, 0f, 1f);
		this.rect_Left.localScale = Vector3.one * (0.8f + 0.2f * num);
		this.rect_Right.localScale = Vector3.one * (0.8f + 0.2f * num);
		if (ThisPlatform.IsIphoneX)
		{
			this.rect_Left.localPosition = new Vector3(60f, this.rect_Left.localPosition.y, 0f);
			this.rect_Right.localPosition = new Vector3(-60f, this.rect_Left.localPosition.y, 0f);
		}
		else
		{
			this.rect_Left.localPosition = new Vector3(0f, this.rect_Left.localPosition.y, 0f);
			this.rect_Right.localPosition = new Vector3(0f, this.rect_Left.localPosition.y, 0f);
		}
		this.obj_TooltipBooster.SetActive(false);
		this.indexUnlockMin = new int[]
		{
			-1,
			-1,
			-1
		};
		this.indexUnlockMax = new int[]
		{
			-1,
			-1,
			-1
		};
		switch (FormLoadout.typeForm)
		{
		case FormLoadout.Type.Shop:
			MenuManager.Instance.txt_NameForm.text = PopupManager.Instance.GetText(Localization0.Loadout, null).ToUpper();
			this.obj_ButtonStart.SetActive(false);
			break;
		case FormLoadout.Type.Campaign:
			MenuManager.Instance.txt_NameForm.text = string.Concat(new object[]
			{
				PopupManager.Instance.GetText(Localization0.Campaign_Map, null).ToUpper(),
				" ",
				MenuManager.IDMapSelect + 1,
				"-",
				MenuManager.IDLevelSelect + 1
			});
			this.obj_ButtonStart.SetActive(true);
			if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormCampaign_1)
			{
				MenuManager.Instance.tutorial.NextTutorial(4);
			}
			break;
		case FormLoadout.Type.BossMode:
		{
			string text = ProfileManager.bossCurrent.ToString().ToUpper().Replace("_", " ");
			if (((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f < 1.1f && text.Length > 16)
			{
				text = text.Substring(0, 14) + "...";
			}
			if (num < 0.6f && text.Length > 6)
			{
				text = text.Substring(0, 5) + "...";
			}
			MenuManager.Instance.txt_NameForm.text = PopupManager.Instance.GetText(Localization0.Boss, null).ToUpper() + ": " + text;
			this.obj_ButtonStart.SetActive(true);
			break;
		}
		case FormLoadout.Type.Special_Campaign:
			MenuManager.Instance.txt_NameForm.text = string.Concat(new object[]
			{
				PopupManager.Instance.GetText(Localization0.Campaign_Map_S, null).ToUpper(),
				MenuManager.IDMapSelect + 1,
				"-",
				MenuManager.IDLevelSelect + 1
			});
			this.obj_ButtonStart.SetActive(true);
			break;
		}
		if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[3].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None)
		{
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_FormLoadout_1);
			ProfileManager.grenadeCurrentId.setValue(0);
		}
		this.txt_NameCharacter.text = ProfileManager.rambos[ProfileManager.settingProfile.IDChar].name;
		this.txt_PowerCharacter.text = ProfileManager.rambos[ProfileManager.settingProfile.IDChar].Power() + string.Empty;
		this.txt_Health.text = ProfileManager.rambos[ProfileManager.settingProfile.IDChar].GetOption(0) + string.Empty;
		this.txt_Speed.text = ProfileManager.rambos[ProfileManager.settingProfile.IDChar].GetOption(1) + string.Empty;
		this.txt_Stamina.text = ProfileManager.rambos[ProfileManager.settingProfile.IDChar].GetOption(2) + string.Empty;
		this.img_Character.sprite = MenuManager.Instance.topUI.imagesRambo[ProfileManager.settingProfile.IDChar];
		if (ProfileManager.rambos[ProfileManager.settingProfile.IDChar].GetOption(0) >= ProfileManager.rambos[ProfileManager.settingProfile.IDChar].GetMaxOption(0))
		{
			this.txt_BtnUpgradeCharacter.text = PopupManager.Instance.GetText(Localization0.Max_Upgrade, null);
		}
		else
		{
			this.txt_BtnUpgradeCharacter.text = PopupManager.Instance.GetText(Localization0.Upgrade_Character, null);
		}
		MenuManager.Instance.obj_Characters.localPosition = new Vector3(0.6f, 0f, 0f);
		for (int i = 0; i < MenuManager.Instance.MainCharacters.Length; i++)
		{
			MenuManager.Instance.MainCharacters[i].gameObject.SetActive(i == ProfileManager.settingProfile.IDChar);
		}
		MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].Show();
		this.ReloadTotalPower();
		FormLoadout.indexBoostersSelected = new List<int>();
		ProfileManager.boosterProfile.ResetAllItem();
		for (int j = 0; j < this.listBooster.Length; j++)
		{
			this.listBooster[j].idCard = (int)this.listBooster[j].booster;
			this.listBooster[j].indexCard = j;
			this.listBooster[j].txt_Amount.text = string.Empty + ProfileManager.boosterProfile.GetItem(this.listBooster[j].booster);
			this.listBooster[j].txt_Cost.text = ProfileManager.boosterProfile.GetItemPrice(this.listBooster[j].booster).ToString();
			if (ProfileManager.boosterProfile.GetItem(this.listBooster[j].booster) > 0)
			{
				this.listBooster[j].obj_Active.SetActive(PlayerPrefs.GetInt("gui.campaign.active.booster." + this.listBooster[j].booster.ToString()) == 1);
			}
			else
			{
				this.listBooster[j].obj_Active.SetActive(false);
			}
			if (this.listBooster[j].obj_Active.activeSelf)
			{
				if (!FormLoadout.indexBoostersSelected.Contains((int)this.listBooster[j].booster))
				{
					FormLoadout.indexBoostersSelected.Add((int)this.listBooster[j].booster);
					ProfileManager.boosterProfile.SetUseItem(this.listBooster[j].booster, true);
				}
			}
			else
			{
				FormLoadout.indexBoostersSelected.Remove((int)this.listBooster[j].booster);
				ProfileManager.boosterProfile.SetUseItem(this.listBooster[j].booster, false);
			}
		}
		this.dataGunMain = new InforWeaponLoadout[6];
		for (int k = 0; k < this.dataGunMain.Length; k++)
		{
			this.dataGunMain[k] = new InforWeaponLoadout();
			this.dataGunMain[k].id = k;
			this.dataGunMain[k].nameWeapon = ProfileManager.weaponsRifle[k].Gun_Value.gunName;
			this.dataGunMain[k].power = ProfileManager.weaponsRifle[k].Power();
			this.dataGunMain[k].rankBase = ProfileManager.weaponsRifle[k].GetRankBase();
			this.dataGunMain[k].rankUpped = ProfileManager.weaponsRifle[k].GetRankUpped();
			this.dataGunMain[k].spriteWeapon = PopupManager.Instance.sprite_GunMain[this.dataGunMain[k].rankUpped].Sprites[k];
			this.dataGunMain[k].isUnlock = ProfileManager.weaponsRifle[k].GetGunBuy();
			if (this.dataGunMain[k].isUnlock)
			{
				if (this.indexUnlockMin[0] == -1 || k < this.indexUnlockMin[0])
				{
					this.indexUnlockMin[0] = k;
				}
				if (this.indexUnlockMax[0] == -1 || k > this.indexUnlockMin[0])
				{
					this.indexUnlockMax[0] = k;
				}
			}
		}
		this.dataGunSpecial = new InforWeaponLoadout[6];
		for (int l = 0; l < this.dataGunSpecial.Length; l++)
		{
			this.dataGunSpecial[l] = new InforWeaponLoadout();
			this.dataGunSpecial[l].id = l;
			this.dataGunSpecial[l].nameWeapon = ProfileManager.weaponsSpecial[l].Gun_Value.gunName;
			this.dataGunSpecial[l].power = ProfileManager.weaponsSpecial[l].Power();
			this.dataGunSpecial[l].rankBase = ProfileManager.weaponsSpecial[l].GetRankBase();
			this.dataGunSpecial[l].rankUpped = ProfileManager.weaponsSpecial[l].GetRankUpped();
			this.dataGunSpecial[l].spriteWeapon = PopupManager.Instance.sprite_GunSpecial[this.dataGunSpecial[l].rankUpped].Sprites[l];
			this.dataGunSpecial[l].isUnlock = ProfileManager.weaponsSpecial[l].GetGunBuy();
			if (this.dataGunSpecial[l].isUnlock)
			{
			}
			if (this.dataGunSpecial[l].isUnlock)
			{
				if (this.indexUnlockMin[1] == -1 || l < this.indexUnlockMin[1])
				{
					this.indexUnlockMin[1] = l;
				}
				if (this.indexUnlockMax[1] == -1 || l > this.indexUnlockMin[1])
				{
					this.indexUnlockMax[1] = l;
				}
			}
		}
		this.dataMelee = new InforWeaponLoadout[ProfileManager.melesProfile.Length];
		for (int m = 0; m < this.dataMelee.Length; m++)
		{
			this.dataMelee[m] = new InforWeaponLoadout();
			this.dataMelee[m].id = m;
			this.dataMelee[m].nameWeapon = ProfileManager.melesProfile[m].NAME;
			this.dataMelee[m].rankBase = ProfileManager.melesProfile[m].RankBase;
			this.dataMelee[m].power = ProfileManager.melesProfile[m].Power();
			this.dataMelee[m].spriteWeapon = PopupManager.Instance.sprite_Melee[m];
			this.dataMelee[m].isUnlock = ProfileManager.melesProfile[m].Unlock;
			if (this.dataMelee[m].isUnlock)
			{
				if (this.indexUnlockMin[2] == -1 || m < this.indexUnlockMin[2])
				{
					this.indexUnlockMin[2] = m;
				}
				if (this.indexUnlockMax[2] == -1 || m > this.indexUnlockMin[2])
				{
					this.indexUnlockMax[2] = m;
				}
			}
		}
		this.dataBomb = new InforWeaponLoadout[ProfileManager.grenadesProfile.Length];
		for (int n = 0; n < this.dataBomb.Length; n++)
		{
			this.dataBomb[n] = new InforWeaponLoadout();
			this.dataBomb[n].id = n;
			this.dataBomb[n].nameWeapon = ProfileManager.grenadesProfile[n].NAME_GRENADE;
			this.dataBomb[n].power = ProfileManager.grenadesProfile[n].Power();
			this.dataBomb[n].rankBase = ProfileManager.grenadesProfile[n].RankBase;
			this.dataBomb[n].rankUpped = ProfileManager.grenadesProfile[n].LevelUpGrade / 10;
			this.dataBomb[n].spriteWeapon = PopupManager.Instance.sprite_Grenade[this.dataBomb[n].rankUpped].Sprites[n];
			if (ProfileManager.grenadesProfile[n].TotalBomb < 0)
			{
				ProfileManager.grenadesProfile[n].TotalBomb = 0;
			}
			this.dataBomb[n].amountCurrent = ProfileManager.grenadesProfile[n].TotalBomb;
			this.dataBomb[n].cost = ProfileManager.grenadesProfile[n].SecuredPrice.Value;
		}
		this.datarHealth = new InforWeaponLoadout();
		this.datarHealth.id = 0;
		this.datarHealth.nameWeapon = PopupManager.Instance.GetText(Localization0.Medkit, null);
		this.datarHealth.spriteWeapon = PopupManager.Instance.sprite_Health;
		this.datarHealth.amountCurrent = ProfileManager.boosterProfile.GetItem(EBooster.MEDKIT);
		this.datarHealth.cost = ProfileManager.boosterProfile.GetItemPrice(EBooster.MEDKIT);
		this.indexGunMain = ProfileManager.rifleGunCurrentId.Data.Value;
		this.indexMelee = ProfileManager.meleCurrentId.Data.Value;
		this.indexBomb = ProfileManager.grenadeCurrentId.Data.Value;
		this.ReloadGunMain();
		this.ReloadGunSpecial();
		this.ReloadMelee();
		this.ReloadBomb();
		this.ReloadHP();
		this.ReloadAmount();
		if (AutoCheckLevel.isAutoCheck)
		{
			this.StartGame(false);
			return;
		}
		
	}

	private void OpenFormCharacter()
	{
		MenuManager.Instance.ChangeForm(FormUI.UICharacter, FormUI.UILoadOut);
	}

	private void OpenFormWeapon(int tabOpen)
	{
		switch (tabOpen)
		{
		case 0:
			MenuManager.indexTabSpecial = ETypeWeapon.PRIMARY;
			break;
		case 1:
			MenuManager.indexTabSpecial = ETypeWeapon.SPECIAL;
			break;
		case 2:
			MenuManager.indexTabSpecial = ETypeWeapon.KNIFE;
			break;
		case 3:
			MenuManager.indexTabSpecial = ETypeWeapon.GRENADE;
			break;
		}
		MenuManager.Instance.ChangeForm(FormUI.UIWeapon, FormUI.UILoadOut);
	}

	private void StartGame(bool isFirebase)
	{
		switch (FormLoadout.typeForm)
		{
		case FormLoadout.Type.Shop:
			UnityEngine.Debug.LogError("Lỗi Type của FormLoadout");
			break;
		case FormLoadout.Type.Campaign:
			GameMode.Instance.Style = GameMode.GameStyle.SinglPlayer;
			if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormCampaign_1)
			{
				MenuManager.Instance.tutorial.NextTutorial(5);
			}
			MenuManager.Instance.objLoading.SetActive(true);
			GameMode.Instance.MODE = (GameMode.Mode)MenuManager.IDDifficultSelect;
			GameMode.Instance.modePlay = GameMode.ModePlay.Campaign;
			ProfileManager.LevelCurrent = MenuManager.IDLevelSelect;
			ProfileManager.MapCurrent = MenuManager.IDMapSelect;
			FireBaseManager.Instance.LogEvent(FireBaseManager.PLAY_LEVEL + (int)ProfileManager.eLevelCurrent);
			if (isFirebase)
			{
				
			}
			DataLoader.LoadLevelJsonData(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathMapCampaign : SPaths.PathMapCampaign_Decrypt) + "Level" + (int)ProfileManager.eLevelCurrent);
			MenuManager.Instance.async = SceneManager.LoadSceneAsync("Level " + (int)ProfileManager.eLevelCurrent);
			break;
		case FormLoadout.Type.BossMode:
			GameMode.Instance.Style = GameMode.GameStyle.SinglPlayer;
			MenuManager.Instance.objLoading.SetActive(true);
			GameMode.Instance.modePlay = GameMode.ModePlay.Boss_Mode;
			FormBossMode.isRestartBoss = true;
			Log.Info(ProfileManager.bossCurrent);
			if (isFirebase)
			{
				
			}
			if (MenuManager.Instance.tutorial.listTutorialUI[4] != null)
			{
				PlayerPrefs.SetInt(MenuManager.Instance.tutorial.listTutorialUI[4].keyPlayerPrefs, 1);
			}
			MenuManager.Instance.async = SceneManager.LoadSceneAsync("BossModePlay");
			break;
		case FormLoadout.Type.Special_Campaign:
			GameMode.Instance.Style = GameMode.GameStyle.SinglPlayer;
			MenuManager.Instance.objLoading.SetActive(true);
			GameMode.Instance.MODE = (GameMode.Mode)MenuManager.IDDifficultSelect;
			GameMode.Instance.modePlay = GameMode.ModePlay.Special_Campaign;
			FireBaseManager.Instance.LogEvent(FireBaseManager.PLAY_LEVEL + "S" + (ProfileManager.eLevelCurrent - ELevel.LEVEL_S0));
			if (isFirebase)
			{
			
			}
			MenuManager.Instance.async = SceneManager.LoadSceneAsync("Level S" + (ProfileManager.eLevelCurrent - ELevel.LEVEL_S0));
			break;
		case FormLoadout.Type.PVP:
			if (isFirebase)
			{
				
			}
                PVPManager.PVPManager.RaiseEvent(0, null, ReceiverGroup.All);
			break;
		}
	}

	public void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEventReceived;
	}

	private void OnPhotonEventReceived(EventData photonEvent)
	{
		if (photonEvent.Code == 0)
		{
			UnityEngine.Debug.Log("++++++++++++++ receive event");
			GameMode.Instance.Style = GameMode.GameStyle.MultiPlayer;
			GameMode.Instance.modePlay = GameMode.ModePlay.PvpMode;
			MenuManager.Instance.objLoading.SetActive(true);
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.CurrentRoom.IsVisible = false;
			MenuManager.Instance.async = PhotonNetwork.LoadLevel("PvpPlay");
		}
	}

	private void SelectBooster(CardBooster cardBooster)
	{
		cardBooster = this.listBooster[cardBooster.indexCard];
		if (int.Parse(cardBooster.txt_Amount.text) == 0)
		{
			this.BuyBooster(cardBooster);
			return;
		}
		cardBooster.obj_Active.SetActive(!cardBooster.obj_Active.activeSelf);
		if (cardBooster.obj_Active.activeSelf)
		{
			if (!FormLoadout.indexBoostersSelected.Contains((int)cardBooster.booster))
			{
				FormLoadout.indexBoostersSelected.Add((int)cardBooster.booster);
				ProfileManager.boosterProfile.SetUseItem(cardBooster.booster, true);
			}
			if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormLoadout_1)
			{
				MenuManager.Instance.tutorial.NextTutorial(5);
			}
			PlayerPrefs.SetInt("gui.campaign.active.booster." + cardBooster.booster.ToString(), 1);
		}
		else
		{
			FormLoadout.indexBoostersSelected.Remove((int)cardBooster.booster);
			ProfileManager.boosterProfile.SetUseItem(cardBooster.booster, false);
			PlayerPrefs.SetInt("gui.campaign.active.booster." + cardBooster.booster.ToString(), 0);
		}
		this.obj_TooltipBooster.SetActive(true);
		this.obj_TooltipBooster.transform.position = cardBooster.transform.position;
		base.StopAllCoroutines();
		base.StartCoroutine(this.WaitCloseTooltip());
		switch (cardBooster.booster)
		{
		case EBooster.MEDKIT:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Restore_the_health_fully, null);
			break;
		case EBooster.SPEED:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Increase_movement_speed, null);
			break;
		case EBooster.DAMAGE:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Increase_30_damage, null);
			break;
		case EBooster.SKILL:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Decrease_Special_Skill_cooldown, null);
			break;
		case EBooster.CRITICAL_HIT:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Have_20_chance_to_deal_x3_Crit_Damage, null);
			break;
		case EBooster.ALLIED_HP_BOOST:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Increase_50_hostage_s_health, null);
			break;
		case EBooster.MAGNET:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Auto_collect_Golds, null);
			break;
		case EBooster.ARMOR:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Decrease_30_enemies_damage, null);
			break;
		case EBooster.DOUBLE_EXP:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Increase_Rank_experiences_for_completed_mission_objectives, null);
			break;
		case EBooster.DOUBLE_COIN:
			this.txt_TooltipBooster.text = PopupManager.Instance.GetText(Localization0.Increase_looted_gold_during_mission, null);
			break;
		}
	}

	private IEnumerator WaitCloseTooltip()
	{
		yield return new WaitForSeconds(5f);
		this.obj_TooltipBooster.SetActive(false);
		yield break;
	}

	private void BuyBooster(CardBooster cardBooster)
	{
		int amountBeforeBuy = int.Parse(cardBooster.txt_Amount.text);
		MenuManager.Instance.popupBuy.Show(PopupBuy.ItemType.Booster, cardBooster.idCard, cardBooster.item, PopupManager.Instance.GetText(cardBooster.idName, null), cardBooster.img_Main.sprite, int.Parse(cardBooster.txt_Cost.text), int.Parse(cardBooster.txt_Amount.text), -1, false, null);
		MenuManager.Instance.popupBuy.OnClosed = delegate()
		{
			this.ReloadAmount();
			if (amountBeforeBuy <= 0 && int.Parse(cardBooster.txt_Amount.text) > 0 && !cardBooster.obj_Active.activeSelf)
			{
				this.SelectBooster(cardBooster);
			}
		};
	}

	private void OpenPopupBuyBomb()
	{
		MenuManager.Instance.popupBuy.Show(PopupBuy.ItemType.Bomb, this.indexBomb, (Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Bomb, this.indexBomb), string.Empty + this.dataBomb[this.indexBomb].nameWeapon, this.dataBomb[this.indexBomb].spriteWeapon, this.dataBomb[this.indexBomb].cost, this.dataBomb[this.indexBomb].amountCurrent, -1, false, null);
		MenuManager.Instance.popupBuy.OnClosed = delegate()
		{
			this.ReloadAmount();
		};
	}

	public void OpenPopupBuyHp()
	{
		MenuManager.Instance.popupBuy.Show(PopupBuy.ItemType.Booster, 0, Item.Medkit, PopupManager.Instance.GetText(Localization0.Medkit, null), this.datarHealth.spriteWeapon, this.datarHealth.cost, this.datarHealth.amountCurrent, -1, false, null);
		MenuManager.Instance.popupBuy.OnClosed = delegate()
		{
			this.ReloadAmount();
		};
	}

	public void NextGunMain()
	{
		int num = Mathf.Clamp(this.indexGunMain + 1, 0, this.dataGunMain.Length - 1);
		while (!ProfileManager.weaponsRifle[num].GetGunBuy())
		{
			if (num >= this.dataGunMain.Length - 1)
			{
				return;
			}
			num++;
		}
		AudioClick.Instance.OnClick();
		this.indexGunMain = num;
		this.ReloadGunMain();
		this.ReloadTotalPower();
		MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].ShowGun(this.indexGunMain);
	}

	public void BackGunMain()
	{
		int num = Mathf.Clamp(this.indexGunMain - 1, 0, this.dataGunMain.Length - 1);
		while (!ProfileManager.weaponsRifle[num].GetGunBuy())
		{
			if (num <= 0)
			{
				return;
			}
			num--;
		}
		AudioClick.Instance.OnClick();
		this.indexGunMain = num;
		this.ReloadGunMain();
		this.ReloadTotalPower();
		MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].ShowGun(this.indexGunMain);
	}

	private void ReloadGunMain()
	{
		ProfileManager.rifleGunCurrentId.setValue(this.indexGunMain);
		this.cardsWeaponLoadout[0].img_Main.sprite = this.dataGunMain[this.indexGunMain].spriteWeapon;
		this.cardsWeaponLoadout[0].txt_Name.text = this.dataGunMain[this.indexGunMain].nameWeapon;
		this.cardsWeaponLoadout[0].txt_Power.text = this.dataGunMain[this.indexGunMain].power + string.Empty;
		this.cardsWeaponLoadout[0].img_Rank.sprite = PopupManager.Instance.sprite_IconRankItem[this.dataGunMain[this.indexGunMain].rankBase + this.dataGunMain[this.indexGunMain].rankUpped];
		this.cardsWeaponLoadout[0].buttonBack.SetActive(this.indexUnlockMin[0] != -1 && this.indexGunMain > this.indexUnlockMin[0]);
		this.cardsWeaponLoadout[0].buttonNext.SetActive(this.indexUnlockMin[0] != -1 && this.indexGunMain < this.indexUnlockMax[0]);
		if (!string.IsNullOrEmpty(this.aniRank[this.dataGunMain[this.indexGunMain].rankBase + this.dataGunMain[this.indexGunMain].rankUpped]))
		{
			this.cardsWeaponLoadout[0].obj_EffectByRank.gameObject.SetActive(true);
			if (this.cardsWeaponLoadout[0].obj_EffectByRank.AnimationState == null)
			{
				this.cardsWeaponLoadout[0].obj_EffectByRank.Initialize(true);
			}
			this.cardsWeaponLoadout[0].obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[this.dataGunMain[this.indexGunMain].rankBase + this.dataGunMain[this.indexGunMain].rankUpped], true);
		}
		else
		{
			this.cardsWeaponLoadout[0].obj_EffectByRank.gameObject.SetActive(false);
		}
	}

	public void NextGunSpecial()
	{
	}

	public void BackGunSpecial()
	{
	}

	private void ReloadGunSpecial()
	{
	}

	public void NextMelee()
	{
		int num = Mathf.Clamp(this.indexMelee + 1, 0, this.dataMelee.Length - 1);
		while (!ProfileManager.melesProfile[num].Unlock)
		{
			if (num >= this.dataMelee.Length - 1)
			{
				return;
			}
			num++;
		}
		AudioClick.Instance.OnClick();
		this.indexMelee = num;
		this.ReloadMelee();
		this.ReloadTotalPower();
	}

	public void BackMelee()
	{
		int num = Mathf.Clamp(this.indexMelee - 1, 0, this.dataMelee.Length - 1);
		while (!ProfileManager.melesProfile[num].Unlock)
		{
			if (num <= 0)
			{
				return;
			}
			num--;
		}
		AudioClick.Instance.OnClick();
		this.indexMelee = num;
		this.ReloadMelee();
		this.ReloadTotalPower();
	}

	private void ReloadMelee()
	{
		ProfileManager.meleCurrentId.setValue(this.indexMelee);
		this.cardsWeaponLoadout[2].img_Main.sprite = this.dataMelee[this.indexMelee].spriteWeapon;
		this.cardsWeaponLoadout[2].txt_Name.text = this.dataMelee[this.indexMelee].nameWeapon;
		this.cardsWeaponLoadout[2].txt_Power.text = this.dataMelee[this.indexMelee].power + string.Empty;
		this.cardsWeaponLoadout[2].img_Rank.sprite = PopupManager.Instance.sprite_IconRankItem[this.dataMelee[this.indexMelee].rankBase + this.dataMelee[this.indexMelee].rankUpped];
		this.cardsWeaponLoadout[2].buttonBack.SetActive(this.indexUnlockMin[2] != -1 && this.indexMelee > this.indexUnlockMin[2]);
		this.cardsWeaponLoadout[2].buttonNext.SetActive(this.indexUnlockMin[2] != -1 && this.indexMelee < this.indexUnlockMax[2]);
	}

	public void NextBomb()
	{
		int num = Mathf.Clamp(this.indexBomb + 1, 0, this.dataBomb.Length - 1);
		if (num > this.dataBomb.Length - 1)
		{
			return;
		}
		AudioClick.Instance.OnClick();
		this.indexBomb = num;
		this.ReloadBomb();
		this.ReloadTotalPower();
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormLoadout_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
	}

	public void BackBomb()
	{
		int num = Mathf.Clamp(this.indexBomb - 1, 0, this.dataBomb.Length - 1);
		if (num < 0)
		{
			return;
		}
		AudioClick.Instance.OnClick();
		this.indexBomb = num;
		this.ReloadBomb();
		this.ReloadTotalPower();
	}

	private void ReloadBomb()
	{
		ProfileManager.grenadeCurrentId.setValue(this.indexBomb);
		this.cardsWeaponLoadout[3].img_Main.sprite = this.dataBomb[this.indexBomb].spriteWeapon;
		this.cardsWeaponLoadout[3].txt_Name.text = this.dataBomb[this.indexBomb].nameWeapon;
		this.cardsWeaponLoadout[3].txt_Power.text = this.dataBomb[this.indexBomb].power + string.Empty;
		this.cardsWeaponLoadout[3].txt_Amount.text = this.dataBomb[this.indexBomb].amountCurrent + string.Empty;
		this.cardsWeaponLoadout[3].img_Rank.sprite = PopupManager.Instance.sprite_IconRankItem[this.dataBomb[this.indexBomb].rankBase + this.dataBomb[this.indexBomb].rankUpped];
		this.cardsWeaponLoadout[3].buttonBack.SetActive(this.indexBomb > 0);
		this.cardsWeaponLoadout[3].buttonNext.SetActive(this.indexBomb < this.dataBomb.Length - 1);
	}

	private void ReloadHP()
	{
		this.cardsWeaponLoadout[4].img_Main.sprite = this.datarHealth.spriteWeapon;
		this.cardsWeaponLoadout[4].txt_Name.text = this.datarHealth.nameWeapon;
		this.cardsWeaponLoadout[4].txt_Amount.text = this.datarHealth.amountCurrent + string.Empty;
	}

	private void ReloadAmount()
	{
		this.dataBomb[this.indexBomb].amountCurrent = ProfileManager.grenadesProfile[this.indexBomb].TotalBomb;
		this.cardsWeaponLoadout[3].txt_Amount.text = this.dataBomb[this.indexBomb].amountCurrent + string.Empty;
		this.datarHealth.amountCurrent = ProfileManager.boosterProfile.GetItem(EBooster.MEDKIT);
		this.cardsWeaponLoadout[4].txt_Amount.text = this.datarHealth.amountCurrent + string.Empty;
		for (int i = 0; i < this.listBooster.Length; i++)
		{
			this.listBooster[i].txt_Amount.text = string.Empty + ProfileManager.boosterProfile.GetItem(this.listBooster[i].booster);
		}
	}

	private void ReloadTotalPower()
	{
		this.txt_Power.text = string.Empty + PowerManager.Instance.TotalPower().ToString();
	}

	public void BtnChangeWeapon(int tabOpen)
	{
		AudioClick.Instance.OnClick();
		
		this.OpenFormWeapon(tabOpen);
	}

	public void BtnChangeChar()
	{
		AudioClick.Instance.OnClick();
		
		this.OpenFormCharacter();
	}

	public void BtnNextWeapon(int index)
	{
		AudioClick.Instance.OnClick();
		
		switch (index)
		{
		case 0:
			this.NextGunMain();
			break;
		case 1:
			this.NextGunSpecial();
			break;
		case 2:
			this.NextMelee();
			break;
		case 3:
			this.NextBomb();
			break;
		default:
			UnityEngine.Debug.LogError("Lỗi FormLoadout/NextWeapon");
			break;
		}
	}

	public void BtnBackWeapon(int index)
	{
		AudioClick.Instance.OnClick();
		
		switch (index)
		{
		case 0:
			this.BackGunMain();
			break;
		case 1:
			this.BackGunSpecial();
			break;
		case 2:
			this.BackMelee();
			break;
		case 3:
			this.BackBomb();
			break;
		default:
			UnityEngine.Debug.LogError("Lỗi FormLoadout/BackWeapon");
			break;
		}
	}

	public void BtnSelectBooster(CardBooster cardBooster)
	{
		AudioClick.Instance.OnClick();
		
		this.SelectBooster(cardBooster);
	}

	public void BtnBuyBooster(CardBooster cardBooster)
	{
		AudioClick.Instance.OnClick();
		
		this.BuyBooster(cardBooster);
	}

	public void BtnBuyBomb()
	{
		AudioClick.Instance.OnClick();
		
		this.OpenPopupBuyBomb();
	}

	public void BtnBuyHP()
	{
		AudioClick.Instance.OnClick();
		
		this.OpenPopupBuyHp();
	}

	public void BtnStart()
	{
		AudioClick.Instance.OnClick();
		if (MenuManager.Instance.async != null)
		{
			return;
		}
		SingletonGame<AudioController>.Instance.StopMusic();
        this.StartGame(true);
        //AdmobManager.Instance.ShowInterstitial(delegate
        //{
        //    this.StartGame(true);
        //}, false);
    }

	public void BtnCloseTooltipBooster()
	{
		AudioClick.Instance.OnClick();
		
		this.obj_TooltipBooster.SetActive(false);
		base.StopAllCoroutines();
	}

	public int indexGunMain;

	public int indexMelee;

	public int indexBomb;

	public static List<int> indexBoostersSelected;

	private InforWeaponLoadout[] dataGunMain;

	private InforWeaponLoadout[] dataGunSpecial;

	private InforWeaponLoadout[] dataMelee;

	private InforWeaponLoadout[] dataBomb;

	private InforWeaponLoadout datarHealth;

	public int[] indexUnlockMin;

	public int[] indexUnlockMax;

	public CardWeaponLoadout[] cardsWeaponLoadout;

	public CardBooster[] listBooster;

	public Text txt_Power;

	public Text txt_NameCharacter;

	public Text txt_PowerCharacter;

	public Text txt_Health;

	public Text txt_Speed;

	public Text txt_Stamina;

	public Text txt_BtnUpgradeCharacter;

	public Image img_Character;

	public GameObject obj_ButtonStart;

	public Text txt_TooltipBooster;

	public GameObject obj_TooltipBooster;

	public string[] aniRank = new string[]
	{
		string.Empty,
		string.Empty,
		"capa",
		"caps",
		"capss"
	};

	public bool isAutoResupply;

	public RectTransform rect_Left;

	public RectTransform rect_Right;

	public static FormLoadout.Type typeForm;

	public enum Type
	{
		Shop,
		Campaign,
		BossMode,
		Endless,
		Special_Campaign,
		PVP,
		CoOp
	}
}
