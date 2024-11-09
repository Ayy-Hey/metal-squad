using System;
using System.Collections;
using System.Text;
using CrossAdPlugin;
using Photon.Pun;
using Sale;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormMainMenu : FormBase
{
	private void Start()
	{
		AdmobManager.Instance.RequestInterstitial();
		if (AdmobManager.Instance.isTest && ProfileManager.unlockAll)
		{
			return;
		}
		if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[1].keyPlayerPrefs, 0) == 0 && !ProfileManager.unlockAll)
		{
			if (ProfileManager._CampaignProfile.MapsProfile[1].GetUnlocked(GameMode.Mode.NORMAL))
			{
				if (MenuManager.Instance.tutorial.listTutorialUI[1] != null)
				{
					PlayerPrefs.SetInt(MenuManager.Instance.tutorial.listTutorialUI[1].keyPlayerPrefs, 1);
				}
			}
			else
			{
				this.isFirtOpen = true;
				MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_FormMainMenu_1);
			}
			return;
		}
		if (this.isFirtOpen)
		{
			return;
		}
		this.objLockEndless.SetActive(ProfileManager._CampaignProfile.MapsProfile[12].GetStar(GameMode.Mode.NORMAL) <= 0);
		try
		{
			if (PhotonNetwork.IsConnectedAndReady)
			{
				PhotonNetwork.Disconnect();
			}
		}
		catch
		{
		}
		this.strbTimeSaleOff = new StringBuilder();
		this.strbTimeSaleOff.Append("Starter Pack");
		if (DateTime.Now.DayOfYear != PlayerPrefs.GetInt("metal.squad.savedailylogin", -1))
		{
			PlayerPrefs.SetInt("metal.squad.savedailylogin", DateTime.Now.DayOfYear);
			PlayerPrefs.SetInt("metal.squad.savedailylogin2", PlayerPrefs.GetInt("metal.squad.savedailylogin2", 0) + 1);
			
			FireBaseManager.Instance.LogEvent(FireBaseManager.DAILYGIFT_MARKETING + PlayerPrefs.GetInt("metal.squad.savedailylogin2", 0));
		//	InAppManager.Instance.FlyerTrackingEvent(FireBaseManager.DAILYGIFT_MARKETING, PlayerPrefs.GetInt("metal.squad.savedailylogin2", 0) + string.Empty);
		}
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
		AchievementManager.Instance.MissionWeaponShop(null);
		ProfileManager.starterPackProfile.Calculator();
		ProfileManager.starterPackProfile.Check();
		SaleManager.Instance.CheckSale();
		if (!PlayerPrefs.HasKey("gui.campaign.active.booster." + EBooster.SPEED.ToString()))
		{
			string[] names = Enum.GetNames(typeof(EBooster));
			for (int i = 0; i < names.Length; i++)
			{
				PlayerPrefs.SetInt("gui.campaign.active.booster." + names, 1);
			}
		}
		PlayfabManager.Instance.GetLeaderBoard(false).PostMyScore(ProfileManager._CampaignProfile.GetTotalStar, delegate(bool ok)
		{
			if (ok)
			{
				PlayfabManager.Instance.GetLeaderBoard(false).GetMyProfile(null);
				PlayfabManager.Instance.GetLeaderBoard(false).GetTopPlayer(null);
			}
		});
	}

	private void LateUpdate()
	{
		if (this.objDailySale == null || !this.objDailySale.activeSelf || this.strbTimeSaleOff == null)
		{
			return;
		}
		this.strbTimeSaleOff.Length = 0;
		this.strbTimeSaleOff.Append("(");
		int value = 24 - DateTime.Now.Hour;
		this.strbTimeSaleOff.Append(value);
		this.strbTimeSaleOff.Append("h ");
		int value2 = 60 - DateTime.Now.Minute;
		this.strbTimeSaleOff.Append(value2);
		this.strbTimeSaleOff.Append("m ");
		int value3 = 60 - DateTime.Now.Second;
		this.strbTimeSaleOff.Append(value3);
		this.strbTimeSaleOff.Append("s");
		this.strbTimeSaleOff.Append(")");
		this.txtTimeSaleOff.text = this.strbTimeSaleOff.ToString();
	}

	public override void Show()
	{
		base.Show();
		try
		{
			this.ConvertFrag("Weapon_Fragment_T1", Item.Machine_Gun_Fragment);
			this.ConvertFrag("Weapon_Fragment_T2", Item.Ice_Gun_Fragment);
			this.ConvertFrag("Weapon_Fragment_T3", Item.Spread_Gun_Fragment);
			this.ConvertFrag("Weapon_Fragment_T4", Item.MGL_140_Fragment);
			this.ConvertFrag("Character_Fragment_T1", Item.John_D_Fragment);
			this.ConvertFrag("Character_Fragment_T2", Item.Yoo_na_Fragment);
			this.ConvertFrag("Character_Fragment_T3", Item.Dvornikov_Fragment);
			if (ProfileManager.userProfile.GetTotalGacha(2) > 0)
			{
				ProfileManager.userProfile.SetGachaValue(1, ProfileManager.userProfile.GetTotalGacha(2));
				ProfileManager.userProfile.SetGachaValue(2, -ProfileManager.userProfile.GetTotalGacha(2));
			}
		}
		catch
		{
			UnityEngine.Debug.LogError("Chuyển đổi mảnh súng bị lỗi");
		}
		
		try
		{
			FireBaseManager.Instance.OnSetUserProperty();
		}
		catch
		{
		}
		float num = Mathf.Clamp(((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f, 0f, 1f);
		this.rect_Right.localPosition = new Vector3(this.rect_Right.localPosition.x, -num * 55f, 0f);
		this.rect_Right.localScale = new Vector3(0.9f + num * 0.1f, 0.9f + num * 0.1f, 1f);
		this.rect_Weapon.localScale = new Vector3(0.9f + num * 0.1f, 0.9f + num * 0.1f, 1f);
		MenuManager.Instance.obj_Characters.localScale = new Vector3(0.8f + num * 0.2f, 0.8f + num * 0.2f, 1f);
		if (ThisPlatform.IsIphoneX)
		{
			this.rect_Left.localPosition = new Vector3(50f, this.rect_Left.transform.localPosition.y, 0f);
			this.rect_Right.localPosition = new Vector3(-50f, this.rect_Right.transform.localPosition.y, 0f);
		}
		else
		{
			this.rect_Left.localPosition = new Vector3(0f, this.rect_Left.transform.localPosition.y, 0f);
			this.rect_Right.localPosition = new Vector3(0f, this.rect_Right.transform.localPosition.y, 0f);
		}
		MenuManager.Instance.topUI.Show();
		MenuManager.Instance.topUI.ShowCoin();
		for (int i = 0; i < MenuManager.Instance.MainCharacters.Length; i++)
		{
			MenuManager.Instance.MainCharacters[i].gameObject.SetActive(i == ProfileManager.settingProfile.IDChar);
		}
		MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].Show();
		this.objLockBossMode.SetActive(!ProfileManager.bossModeProfile.bossProfiles[1].CheckUnlock(GameMode.Mode.NORMAL));
		if (this.isFirtOpen)
		{
			return;
		}
		TutorialUIManager.tutorialIDCurrent = TutorialUIManager.TutorialID.None;
		MenuManager.Instance.obj_Characters.localPosition = Vector3.zero;
		this.OnShowButtonSale();
		MenuManager.Instance.topUI.ShowNamePlayer();
		this.objTipWeapon.SetActive(ShopTips.Instance.CheckTipAll()[0]);
		this.objTipSelectChar.SetActive(CharacterTips.Instance.GetLisEnableTips()[0]);
		this.objAlertGift.SetActive(this.CheckTipGift());
		this.objAlertQuest.SetActive(DailyQuestManager.Instance.CheckDailyQuest() != -1 || AchievementManager.Instance.hasCompleted);
		this.objAlertLucky.SetActive(ProfileManager.spinProfile.Free || ProfileManager.spinProfile.TotalSpin > 0);
		if (AutoCheckLevel.isAutoCheck)
		{
			if (AutoCheckLevel.LevelCampaign < AutoCheckLevel.CheckToLevelCampaign)
			{
				this.OnCampaign();
			}
			else
			{
				this.OnBossMode();
			}
			return;
		}
		FormMainMenu.timesVisitMenu++;
		if (FormMainMenu.timesVisitMenu >= 5)
		{
			FormMainMenu.timesVisitMenu = 0;
			this.OnShowSalePack(null);
		}
		else
		{
			this.isPopupClosed = !SplashScreen.isCheckFirstOpen;
		}
		this.isShowDailyGift = false;
		MenuManager.Instance.popupDailyGift.ClearAndNewStart();
		if (ProfileManager.dailyGiftProfile.giftToday != null)
		{
			MenuManager.Instance.popupDailyGift.Show();
			this.isShowDailyGift = true;
		}
		else if (SplashScreen.isCheckFirstOpen)
		{
			if (FirebaseDatabaseManager.Instance.isNeedRestoreNow)
			{
				FirebaseDatabaseManager.Instance.isNeedRestoreNow = false;
				DialogWarningOldData dialogWarningOldData = UnityEngine.Object.Instantiate(Resources.Load("Popup/DialogWarningOldData", typeof(DialogWarningOldData)), MenuManager.Instance.formCanvas.transform) as DialogWarningOldData;
				dialogWarningOldData.version = 0;
				dialogWarningOldData.OnClosed = delegate()
				{
					if (this.objDailySale.activeInHierarchy || (SaleManager.Instance.OnShowSalePack() == SaleManager.TYPE_SALE.StarterPack_1 && this.objStarterPack.activeInHierarchy))
					{
						this.OnShowSalePack(delegate
						{
							this.OnRequireUpdateGame();
						});
					}
				};
				dialogWarningOldData.Show();
			}
			else
			{
				this.OnShowSalePack(delegate
				{
					this.OnRequireUpdateGame();
				});
			}
			SplashScreen.isCheckFirstOpen = false;
		}
		MenuManager.Instance.popupDailyGift.OnClosed = delegate()
		{
			this.objTipWeapon.SetActive(ShopTips.Instance.CheckTipAll()[0]);
			this.objTipSelectChar.SetActive(CharacterTips.Instance.GetLisEnableTips()[0]);
			MenuManager.Instance.topUI.ShowCoin();
			if (PopupDailyGift.isReadyShowDailySale)
			{
				if (FirebaseDatabaseManager.Instance.isNeedRestoreNow)
				{
					FirebaseDatabaseManager.Instance.isNeedRestoreNow = false;
					DialogWarningOldData dialogWarningOldData2 = UnityEngine.Object.Instantiate(Resources.Load("Popup/DialogWarningOldData", typeof(DialogWarningOldData)), MenuManager.Instance.formCanvas.transform) as DialogWarningOldData;
					dialogWarningOldData2.version = 0;
					dialogWarningOldData2.OnClosed = delegate()
					{
						if (this.objDailySale.activeInHierarchy || (SaleManager.Instance.OnShowSalePack() == SaleManager.TYPE_SALE.StarterPack_1 && this.objStarterPack.activeInHierarchy))
						{
							this.OnShowSalePack(delegate
							{
								this.OnRequireUpdateGame();
							});
						}
					};
					dialogWarningOldData2.Show();
				}
				else if (this.objDailySale.activeInHierarchy || (SaleManager.Instance.OnShowSalePack() == SaleManager.TYPE_SALE.StarterPack_1 && this.objStarterPack.activeInHierarchy))
				{
					this.OnShowSalePack(delegate
					{
						this.OnRequireUpdateGame();
					});
				}
				PopupDailyGift.isReadyShowDailySale = false;
			}
		};
		base.StartCoroutine(this.ShowTutorial());
	}

	private IEnumerator ShowTutorial()
	{
		yield return new WaitUntil(() => this.isPopupClosed);
		if (!TutorialUIManager.isFirstTutorial && !this.isShowDailyGift)
		{
			bool flag = false;
			Item itemID = ProfileManager.weaponsRifle[0].Gun_Value.GetItemID(ProfileManager.weaponsRifle[0].GetLevelUpgrade() + 1);
			int num = ProfileManager.weaponsRifle[0].Gun_Value.ValueUpgrade(ProfileManager.weaponsRifle[0].GetLevelUpgrade() + 1);
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
			bool flag2 = false;
			Item itemID2 = ProfileManager.rambos[0].GetItemID(ProfileManager.rambos[0].LevelUpgrade + 1);
			int num2 = ProfileManager.rambos[0].ValueUpgrade(ProfileManager.rambos[0].LevelUpgrade + 1);
			if (itemID2 != Item.Gold)
			{
				if (itemID2 == Item.Gem)
				{
					flag2 = (ProfileManager.userProfile.Ms >= num2);
				}
			}
			else
			{
				flag2 = (ProfileManager.userProfile.Coin >= num2);
			}
			if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[5].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && ProfileManager.weaponsRifle[0].GetLevelUpgrade() < 29 && flag)
			{
				MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_UpgradeWeapon);
			}
			else if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[6].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && ProfileManager.rambos[0].LevelUpgrade < 29 && flag2)
			{
				MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_UpgradeCharacter);
			}
			else if (PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[12].keyPlayerPrefs, 0) == 0 && TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && ProfileManager.inAppProfile.vipProfile.level != E_Vip.Vip0)
			{
				MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_LevelUpVip);
			}
		}
		yield break;
	}

	private void OnRequireUpdateGame()
	{
		if (FirebaseDatabaseManager.Instance.isRequireUpdateGame)
		{
			PopupManager.Instance.ShowWarningUpdateGame();
			FirebaseDatabaseManager.Instance.isRequireUpdateGame = false;
		}
	}

	public void OnCampaign()
	{
		GameMode.Instance.modePlay = GameMode.ModePlay.Campaign;
		DataLoader.LoadDataCampaign();
        MenuManager.Instance.ChangeForm(FormUI.UICampaign, FormUI.Menu);
        //AdmobManager.Instance.ShowInterstitial(delegate
        //{
        //	MenuManager.Instance.ChangeForm(FormUI.UICampaign, FormUI.Menu);
        //}, false);
    }

	public void OnEndless()
	{
	}

	public void OnBossMode()
	{
		if (!ProfileManager.bossModeProfile.bossProfiles[1].CheckUnlock(GameMode.Mode.NORMAL) && !ProfileManager.unlockAll)
		{
			string[] nameSpec = new string[]
			{
				"1.3"
			};
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.Boss_mode_is_only_available_after_completing_Stage, nameSpec), PopupManager.Instance.GetText(Localization0.Boss_Mode, null));
			return;
		}
		DataLoader.LoadDataBossMode();
		GameMode.Instance.modePlay = GameMode.ModePlay.Boss_Mode;
        MenuManager.Instance.ChangeForm(FormUI.UIBossMode, FormUI.Menu);
  //      AdmobManager.Instance.ShowInterstitial(delegate
		//{
		//	MenuManager.Instance.ChangeForm(FormUI.UIBossMode, FormUI.Menu);
		//}, false);
	}

	public void OnShowShop()
	{
		MenuManager.Instance.ChangeForm(FormUI.UIWeapon, FormUI.Menu);
	}

	public void OnShowCharacter()
	{
		MenuManager.Instance.ChangeForm(FormUI.UICharacter, FormUI.Menu);
	}

	public void OnShowGacha()
	{
		PopupManager.Instance.ShowGacha();
		PopupManager.Instance.mPopupGacha.OnClosed = new Action(this.CheckAlertGacha);
	}

	private void CheckAlertGacha()
	{
		bool active = ProfileManager.userProfile.gachaFreeProfile.Data;
		for (int i = 0; i < 3; i++)
		{
			if (ProfileManager.userProfile.GetTotalGacha(i) > 0)
			{
				active = true;
				break;
			}
		}
		this.objAlertGacha.SetActive(active);
	}

	public void OnShowLuckySpin()
	{
		MenuManager.Instance.popupLuckySpin.Show();
	}

	public void OnShowLeaderboard()
	{
		MenuManager.Instance.popupLeaderboard.Show();
	}

	public void OnShowButtonSale()
	{
		SaleManager.Instance.OnCalculatorStarterPack();
		this.objStarterPack.SetActive(SaleManager.Instance.isStarterPackReady);
		this.objDailySale.SetActive(ProfileManager.dailySaleProfile.ID >= 0);
	}

	public void OnShowSalePack(Action OnClose = null)
	{
		if (SaleManager.Instance.HasSaleServerToday())
		{
			MenuManager.Instance.popupSaleFromServer.OnShow();
			MenuManager.Instance.popupSaleFromServer.OnClosed = delegate()
			{
				if (OnClose != null)
				{
					OnClose();
				}
				this.isPopupClosed = true;
			};
			return;
		}
		switch (SaleManager.Instance.OnShowSalePack())
		{
		case SaleManager.TYPE_SALE.Double_Gem:
			MenuManager.Instance.popupDailySale.OnShowDoubleGem();
			MenuManager.Instance.popupDailySale.OnClosed = delegate()
			{
				if (OnClose != null)
				{
					OnClose();
				}
				this.isPopupClosed = true;
			};
			break;
		case SaleManager.TYPE_SALE.StarterPack_1:
		{
			MenuManager.Instance.popupStarterPack.OnShowPack(false);
			PopupStarterPack popupStarterPack = MenuManager.Instance.popupStarterPack;
			popupStarterPack.OnPurchaseSuccessed = (Action)Delegate.Combine(popupStarterPack.OnPurchaseSuccessed, new Action(delegate()
			{
				this.objStarterPack.SetActive(false);
			}));
			MenuManager.Instance.popupStarterPack.OnClosed = delegate()
			{
				bool flag = true;
				for (int i = 0; i < ProfileManager.starterPackProfile.packsProfile.Length; i++)
				{
					if (!ProfileManager.starterPackProfile.packsProfile[i].IsBuy)
					{
						flag = false;
						break;
					}
				}
				this.objStarterPack.SetActive(!flag);
				this.isPopupClosed = true;
				if (OnClose != null)
				{
					OnClose();
				}
			};
			break;
		}
		case SaleManager.TYPE_SALE.DailySale:
			if (ProfileManager.dailySaleProfile.ID < 0)
			{
				MenuManager.Instance.popupDailySale.OnShowDailySaleRandom();
			}
			else
			{
				MenuManager.Instance.popupDailySale.OnShowDailySale();
				PopupDailySale popupDailySale = MenuManager.Instance.popupDailySale;
				popupDailySale.OnPurchaseSuccessed = (Action)Delegate.Combine(popupDailySale.OnPurchaseSuccessed, new Action(delegate()
				{
					this.objDailySale.SetActive(false);
				}));
			}
			MenuManager.Instance.popupDailySale.OnClosed = delegate()
			{
				if (OnClose != null)
				{
					OnClose();
				}
				this.isPopupClosed = true;
			};
			break;
		default:
			MenuManager.Instance.popupDailySale.OnShowDailySaleRandom();
			MenuManager.Instance.popupDailySale.OnClosed = delegate()
			{
				if (OnClose != null)
				{
					OnClose();
				}
				this.isPopupClosed = true;
			};
			break;
		}
	}

	public void OnShowGift()
	{
		MenuManager.Instance.popupGift.Show();
	}

	public void OnShowDailyGift()
	{
		MenuManager.Instance.popupDailyGift.Show();
	}

	public void OnShowQuest()
	{
		MenuManager.Instance.popupQuest.ShowDailyQuest();
	}

	public bool CheckTipGift()
	{
		try
		{
			return MenuManager.Instance.popupGift.RewardCurrent.Data.Value < 5;
		}
		catch (Exception ex)
		{
		}
		return true;
	}

	private void ConvertFrag(string idFragOld, Item idFragNew)
	{
		try
		{
			if (PlayerPrefs.GetInt("metal.squad." + idFragOld) > 0)
			{
				PopupManager.Instance.SaveReward(idFragNew, PlayerPrefs.GetInt("metal.squad." + idFragOld), "ConvertFragOldVersion", null);
				PlayerPrefs.SetInt("metal.squad." + idFragOld, 0);
			}
		}
		catch
		{
			UnityEngine.Debug.LogError("Chuyển đổi mảnh súng bị lỗi: " + idFragOld);
		}
	}

	public void BtnCampaign()
	{
    
		DataLoader.LoadDataCampaign();
   
        if (MenuManager.Instance.async != null)
		{
			return;
		}
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormMainMenu_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
			MenuManager.Instance.objLoading.SetActive(true);
			MenuManager.Instance.async = SceneManager.LoadSceneAsync("Tutorial");
			ProfileManager.LevelCurrent = 0;
			ProfileManager.MapCurrent = 0;
			GameMode.Instance.MODE = GameMode.Mode.TUTORIAL;
			return;
		}
        this.OnCampaign();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "Campaign");
	}

	public void BtnBossMode()
	{
		
		this.OnBossMode();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "BossMode");
	}

	public void BtnPVP()
	{
		if (ProfileManager._CampaignProfile.MapsProfile[12].GetStar(GameMode.Mode.NORMAL) <= 0 && !ProfileManager.unlockAll)
		{
			string[] nameSpec = new string[]
			{
				"2.1"
			};
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.PvP_mode_is_only_available_after_completing_Stage, nameSpec), PopupManager.Instance.GetText(Localization0.PVP, null));
			return;
		}
		if (!ProfileManager.unlockAll && RemoteConfigFirebase.Instance.GetLongValue(RemoteConfigFirebase.MULTIPLAYER_RUNNING, 0L) == 1L)
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.PvP_mode_is_under_maintenance, null), PopupManager.Instance.GetText(Localization0.Maintenance, null));
			return;
		}
		
		FormLoadout.typeForm = FormLoadout.Type.PVP;
		GameMode.Instance.Style = GameMode.GameStyle.MultiPlayer;
		GameMode.Instance.modePlay = GameMode.ModePlay.PvpMode;
		DataLoader.LoadDataCampaign();
        MenuManager.Instance.ChangeForm(FormUI.UIPvp, FormUI.Menu);
  //      AdmobManager.Instance.ShowInterstitial(delegate
		//{
		//	MenuManager.Instance.ChangeForm(FormUI.UIPvp, FormUI.Menu);
		//}, false);
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "PvP");
	}

	public void BtnCoOp()
	{
		FormLoadout.typeForm = FormLoadout.Type.CoOp;
		GameMode.Instance.Style = GameMode.GameStyle.MultiPlayer;
		GameMode.Instance.modePlay = GameMode.ModePlay.CoOpMode;
		DataLoader.LoadDataCampaign();
        MenuManager.Instance.ChangeForm(FormUI.UICoOp, FormUI.Menu);
  //      AdmobManager.Instance.ShowInterstitial(delegate
		//{
		//	MenuManager.Instance.ChangeForm(FormUI.UICoOp, FormUI.Menu);
		//}, false);
	}

	public void BtnCharacter()
	{
		
		this.OnShowCharacter();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "Character");
	}

	public void BtnWeapon()
	{
		
		this.OnShowShop();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "Weapon");
	}

	public void BtnShopItem()
	{
		
		PopupManager.Instance.ShowShopItem(delegate
		{
			MenuManager.Instance.topUI.ShowCoin();
		});
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "ShopItem");
	}

	public void BtnGift()
	{
		MenuManager.Instance.popupGift.OnClosed = delegate()
		{
			this.objAlertGacha.SetActive(false);
			for (int i = 0; i < 3; i++)
			{
				if (ProfileManager.userProfile.GetTotalGacha(i) > 0)
				{
					this.objAlertGacha.SetActive(true);
				}
			}
			this.objAlertGift.SetActive(this.CheckTipGift());
			this.objTipWeapon.SetActive(ShopTips.Instance.CheckTipAll()[0]);
			this.objTipSelectChar.SetActive(CharacterTips.Instance.GetLisEnableTips()[0]);
			MenuManager.Instance.topUI.ShowCoin();
		};
		
		this.OnShowGift();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "OnlineGift");
	}

	public void BtnLucky()
	{
		MenuManager.Instance.popupLuckySpin.OnClosed = delegate()
		{
			this.objAlertLucky.SetActive(ProfileManager.spinProfile.Free || ProfileManager.spinProfile.TotalSpin > 0);
			this.objTipWeapon.SetActive(ShopTips.Instance.CheckTipAll()[0]);
			this.objTipSelectChar.SetActive(CharacterTips.Instance.GetLisEnableTips()[0]);
			MenuManager.Instance.topUI.ShowCoin();
		};
		
		this.OnShowLuckySpin();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "LuckySpin");
	}

	public void BtnGacha()
	{
		
		this.OnShowGacha();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "Gacha");
	}

	public void BtnDailyGift()
	{
		
		this.OnShowDailyGift();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "DailyGift");
	}

	public void BtnQuest()
	{
		MenuManager.Instance.popupQuest.OnClosed = delegate()
		{
			this.objAlertQuest.SetActive(DailyQuestManager.Instance.CheckDailyQuest() != -1 || AchievementManager.Instance.hasCompleted);
			this.objTipWeapon.SetActive(ShopTips.Instance.CheckTipAll()[0]);
			this.objTipSelectChar.SetActive(CharacterTips.Instance.GetLisEnableTips()[0]);
			MenuManager.Instance.topUI.ShowCoin();
		};
		
		this.OnShowQuest();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "Quest");
	}

	public void BtnLeaderboard()
	{
		
		this.OnShowLeaderboard();
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "Leaderboard");
	}

	public void BtnStarterPack()
	{
		AudioClick.Instance.OnClick();
		MenuManager.Instance.popupStarterPack.OnShowPack(false);
		PopupStarterPack popupStarterPack = MenuManager.Instance.popupStarterPack;
		popupStarterPack.OnPurchaseSuccessed = (Action)Delegate.Combine(popupStarterPack.OnPurchaseSuccessed, new Action(delegate()
		{
			this.objStarterPack.SetActive(false);
		}));
		MenuManager.Instance.popupStarterPack.OnClosed = delegate()
		{
			bool flag = true;
			for (int i = 0; i < SaleManager.Instance.profileStarterPacks.Length; i++)
			{
				if (!SaleManager.Instance.profileStarterPacks[i].isBuy)
				{
					flag = false;
					break;
				}
			}
			this.objStarterPack.SetActive(!flag);
		};
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "StarterPack");
	}

	public void BtnDailySale()
	{
		AudioClick.Instance.OnClick();
		if (SaleManager.Instance.HasSaleServerToday())
		{
			MenuManager.Instance.popupSaleFromServer.OnShow();
			return;
		}
		SaleManager.TYPE_SALE type_SALE = SaleManager.Instance.OnShowSalePack();
		if (type_SALE != SaleManager.TYPE_SALE.Double_Gem)
		{
			if (ProfileManager.dailySaleProfile.ID < 0)
			{
				MenuManager.Instance.popupDailySale.OnShowDailySaleRandom();
			}
			else
			{
				MenuManager.Instance.popupDailySale.OnShowDailySale();
				PopupDailySale popupDailySale = MenuManager.Instance.popupDailySale;
				popupDailySale.OnPurchaseSuccessed = (Action)Delegate.Combine(popupDailySale.OnPurchaseSuccessed, new Action(delegate()
				{
					this.objDailySale.SetActive(false);
				}));
			}
		}
		else
		{
			MenuManager.Instance.popupDailySale.OnShowDoubleGem();
		}
		FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BUTTON + "DailySale");
	}

	[SerializeField]
	[Header("---------------Left UI---------------")]
	private RectTransform rect_Left;

	[SerializeField]
	private GameObject objAlertGift;

	[SerializeField]
	private GameObject objAlertLucky;

	[SerializeField]
	private GameObject objAlertGacha;

	[SerializeField]
	private GameObject objAlertQuest;

	[SerializeField]
	[Header("---------------Right UI---------------")]
	private RectTransform rect_Right;

	public GameObject objLockEndless;

	public GameObject objLockBossMode;

	[Header("---------------Weapon & Upgrade---------------")]
	[SerializeField]
	private RectTransform rect_Weapon;

	[SerializeField]
	private GameObject objTipSelectChar;

	[SerializeField]
	private GameObject objTipWeapon;

	[Header("---------------SaleOff---------------")]
	public GameObject objStarterPack;

	public GameObject objDailySale;

	public Text txtTimeSaleOff;

	private StringBuilder strbTimeSaleOff;

	private bool isFirtOpen;

	public bool isShowDailyGift;

	public static int timesVisitMenu;

	private bool isPopupClosed;
}
