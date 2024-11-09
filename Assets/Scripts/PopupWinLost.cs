using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.dev.util.SecurityHelper;
using CrossAdPlugin;
using PVPManager;
using Rank;
using Sale;
using Spine.Unity;
using StarMission;
using UnityEngine;
using UnityEngine.UI;

public class PopupWinLost : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.listTextLocalization.Length; i++)
		{
			if (this.listTextLocalization[i].isUpcaseText)
			{
				this.listTextLocalization[i].txt_Text.text = (this.listTextLocalization[i].textStart + PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd).ToUpper();
			}
			else
			{
				this.listTextLocalization[i].txt_Text.text = this.listTextLocalization[i].textStart + PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd;
			}
		}
	}

	private void Update()
	{
		if (!this.isShowDone)
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !PopupManager.isblockInput)
		{
			if (PopupManager.Instance.CloseAll())
			{
				return;
			}
			if (this.popupStarterPack.isActive)
			{
				this.popupStarterPack.OnClose();
				return;
			}
			if (this.popupDailySale.isActive)
			{
				this.popupDailySale.OnClose();
				return;
			}
			if (this.saleFromServer != null && this.saleFromServer.isActive)
			{
				this.saleFromServer.OnClose();
				return;
			}
			if (this.isShowDone)
			{
				this.OnMenu();
				this.isShowDone = false;
				return;
			}
		}
	}

	public void OnShow()
	{
		PopupWinLost.CountCompleted++;
		if (this.isShowDone)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.obj_OpenPopup.gameObject.SetActive(false);
		this.objGroupButton.gameObject.SetActive(false);
		this.btn_Bonus.gameObject.SetActive(false);
		this.obj_NoItem.SetActive(false);
		this.listReward.CreateObj(0);
		this.listResult.CreateObj(0);
		this.animPingPong.enabled = false;
		this.isReceived = false;
		this.amountReward = 0;
		this.totalGold = 0;
		this.totalExp = 0;
		this.totalGem = 0;
		if (this.objTimePlay != null)
		{
			this.objTimePlay.SetActive(false);
		}
		this.btn_Menu.gameObject.SetActive(GameMode.Instance.EMode != GameMode.Mode.TUTORIAL);
		this.btn_Back.gameObject.SetActive(this.state != EndMission.CoOpWin && this.state != EndMission.CoOpLost && this.state != EndMission.PvPWin && this.state != EndMission.PvPLost && GameMode.Instance.EMode != GameMode.Mode.TUTORIAL);
		this.btn_Upgrade.gameObject.SetActive(this.state == EndMission.BossModeLost || this.state == EndMission.CampaignLost || this.state == EndMission.PvPLost || this.state == EndMission.CoOpLost);
		this.txt_BestResult.gameObject.SetActive(this.state != EndMission.BossModeLost && GameMode.Instance.EMode != GameMode.Mode.TUTORIAL);
		this.txt_BossModeLost.gameObject.SetActive(this.state == EndMission.BossModeLost);
		this.txt_Back.text = ((this.state != EndMission.BossModeWin && this.state != EndMission.BossModeLost) ? PopupManager.Instance.GetText(Localization0.Campaign, null) : PopupManager.Instance.GetText(Localization0.Boss_Mode, null));
		this.txt_Restart.text = ((this.state != EndMission.CampaignWin && this.state != EndMission.BossModeWin && this.state != EndMission.CoOpWin && this.state != EndMission.CoOpLost && this.state != EndMission.PvPWin && this.state != EndMission.PvPLost) ? PopupManager.Instance.GetText(Localization0.Restart, null) : PopupManager.Instance.GetText(Localization0.Continue, null));
		this.obj_Pvp.SetActive(this.state == EndMission.PvPLost || this.state == EndMission.PvPWin);
		this.obj_NonPvp.SetActive(!this.obj_Pvp.activeSelf);
		this.listResult.CreateObj(0);
		this.listReward.CreateObj(0);
		this.ShowRankCurrent(false);
		ProfileManager.userProfile.TotalGame++;
		if (GameManager.Instance.player != null)
		{
			GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
		}
		this.COIN_COLLECTED = GameManager.Instance.COIN_COLLECTED;
		ControlManager.Instance.OnHideControl();
		this.objEffectRestart.SetActive(!AdmobManager.Instance.IsVideoReady());
		this.objEffectBonus.SetActive(AdmobManager.Instance.IsVideoReady());
		string key = "save.game.campaign.lost." + (int)GameManager.Instance.Level;
		UnityEngine.Debug.Log("_____State_ " + this.state);
		if (this.state == EndMission.CampaignWin)
		{
			PlayerPrefs.SetInt(key, 0);
			if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
			{
				this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.Tutorial, null);
			}
			else
			{
				this.txt_ModeGame.text = string.Concat(new object[]
				{
					PopupManager.Instance.GetText(Localization0.Campaign, null),
					" ",
					(int)((int)GameManager.Instance.Level / (int)ELevel.LEVEL_13 + 1),
					"-",
					(int)((int)GameManager.Instance.Level % (int)ELevel.LEVEL_13 + 1)
				});
				if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && GameManager.Instance.bossManager.Boss != null && GameManager.Instance.bossManager.Boss != null)
				{
					Text text = this.txt_ModeGame;
					string text2 = text.text;
					text.text = string.Concat(new string[]
					{
						text2,
						" (",
						PopupManager.Instance.GetText(Localization0.Boss, null),
						": ",
						GameManager.Instance.bossManager.Boss.boss.ToString().Replace("_", " "),
						")"
					});
				}
			}
			try
			{
				if (!ProfileManager.settingProfile.IsSound)
				{
					for (int i = 0; i < 3; i++)
					{
						this.Obj_StarsOn[i].GetComponent<AudioSource>().playOnAwake = false;
						this.Obj_StarsOff[i].GetComponent<AudioSource>().playOnAwake = false;
					}
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			ProfileManager.userProfile.GameWon++;
			ProfileManager.userProfile.TotalEnemyKilled += GameManager.Instance.TotalEnemyKilled;
			UserManager.User.ResetCountRepeat();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.BG_Completed();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message2)
			{
				UnityEngine.Debug.LogError(message2);
			}
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				GameManager.Instance.mMission.StartCheck();
			}
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
			}
		}
		else if (this.state == EndMission.CampaignLost)
		{
			PopupWinLost.CountCompleted += 2;
			int @int = PlayerPrefs.GetInt(key, 0);
			PlayerPrefs.SetInt(key, @int + 1);
			this.txt_ModeGame.text = string.Concat(new object[]
			{
				PopupManager.Instance.GetText(Localization0.Campaign, null),
				" ",
				(int)((int)GameManager.Instance.Level / (int)ELevel.LEVEL_13 + 1),
				"-",
				(int)((int)GameManager.Instance.Level % (int)ELevel.LEVEL_13 + 1)
			});
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && GameManager.Instance.bossManager.Boss != null && GameManager.Instance.bossManager.Boss != null)
			{
				Text text3 = this.txt_ModeGame;
				string text2 = text3.text;
				text3.text = string.Concat(new string[]
				{
					text2,
					" (",
					PopupManager.Instance.GetText(Localization0.Boss, null),
					": ",
					GameManager.Instance.bossManager.Boss.boss.ToString().Replace("_", " "),
					")"
				});
			}
			ProfileManager.userProfile.TotalEnemyKilled += GameManager.Instance.TotalEnemyKilled;
			UserManager.User.YouLost();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.BG_Failed();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message3)
			{
				UnityEngine.Debug.LogError(message3);
			}
		}
		else if (this.state == EndMission.BossModeWin)
		{
			AchievementManager.Instance.MissionBossModeWin(delegate(bool isCompleted, AchievementManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowAchievement(infor.Desc, infor.pathIcon);
				}
			});
			DailyQuestManager.Instance.MissionBossModeWin(delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			DailyQuestManager.Instance.MissionBossModeFinish(delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.Boss, null) + ": " + ProfileManager.bossCurrent.ToString().Replace("_", " ");
			UserManager.User.ResetCountRepeat();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.StopAll();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message4)
			{
				UnityEngine.Debug.LogError(message4);
			}
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
			}
		}
		else if (this.state == EndMission.BossModeLost)
		{
			PopupWinLost.CountCompleted += 2;
			DailyQuestManager.Instance.MissionBossModeFinish(delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.Boss, null) + ": " + ProfileManager.bossCurrent.ToString().Replace("_", " ");
			UserManager.User.YouLost();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.StopAll();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message5)
			{
				UnityEngine.Debug.LogError(message5);
			}
		}
		else if (this.state == EndMission.PvPWin)
		{
			if (PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer <= 2)
			{
				this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.PVP, null) + " 1-1";
			}
			else
			{
				this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.PVP, null) + " 1-3";
			}
			DailyQuestManager.Instance.MissionFinishPvP(delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			DailyQuestManager.Instance.MissionWinPvP(delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			UserManager.User.ResetCountRepeat();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.StopAll();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message6)
			{
				UnityEngine.Debug.LogError(message6);
			}
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
			}
		}
		else if (this.state == EndMission.PvPLost)
		{
			PopupWinLost.CountCompleted += 2;
			DailyQuestManager.Instance.MissionFinishPvP(delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			if (PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer <= 2)
			{
				this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.PVP, null) + " 1-1";
			}
			else
			{
				this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.PVP, null) + " 1-3";
			}
			UserManager.User.YouLost();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.BG_Failed();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message7)
			{
				UnityEngine.Debug.LogError(message7);
			}
		}
		else if (this.state == EndMission.CoOpWin)
		{
			this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.CoOp, null);
			UserManager.User.ResetCountRepeat();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.StopAll();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message8)
			{
				UnityEngine.Debug.LogError(message8);
			}
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
			}
		}
		else if (this.state == EndMission.CoOpLost)
		{
			PopupWinLost.CountCompleted += 2;
			this.txt_ModeGame.text = PopupManager.Instance.GetText(Localization0.CoOp, null);
			UserManager.User.YouLost();
			try
			{
				GameManager.Instance.hudManager.HideLineBoss();
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.bulletManager.DestroyAll();
				GameManager.Instance.skillManager.DestroyAll();
				GameManager.Instance.coinManager.DestroyAll();
				GameManager.Instance.audioManager.BG_Failed();
				GameManager.Instance.bombManager.SetPause();
			}
			catch (Exception message9)
			{
				UnityEngine.Debug.LogError(message9);
			}
		}
		base.StartCoroutine(this.IEShow());
	}

	private IEnumerator IEShow()
	{
		yield return new WaitForSeconds(2f);
		this.obj_OpenPopup.gameObject.SetActive(true);
		if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
		{
			this.btn_Bonus.gameObject.SetActive(true);
		}
		this.SetGoldVideoBonus();
		yield return new WaitForSeconds(0.5f);
		if (SaleManager.Instance.HasSaleServerToday())
		{
			this.saleFromServer = (BestSaleCalculator)UnityEngine.Object.Instantiate(Resources.Load("Popup/SaleFromServer", typeof(BestSaleCalculator)), base.transform);
		}
		if (this.state == EndMission.CampaignWin)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Completed", false);
			base.StartCoroutine(this.IEShowCampaignWin());
		}
		else if (this.state == EndMission.CampaignLost)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Failed", false);
			base.StartCoroutine(this.IEShowCampaignLost());
		}
		else if (this.state == EndMission.BossModeWin)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Boss_Completed", false);
			base.StartCoroutine(this.IEShowBossmodeWin());
		}
		else if (this.state == EndMission.BossModeLost)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Boss_Failed", false);
			base.StartCoroutine(this.IEShowBossmodeLost());
		}
		else if (this.state == EndMission.PvPWin)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Completed", false);
			base.StartCoroutine(this.IEShowPvpWin());
		}
		else if (this.state == EndMission.PvPLost)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Failed", false);
			base.StartCoroutine(this.IEShowPvpLost());
		}
		else if (this.state == EndMission.CoOpWin)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Completed", false);
			base.StartCoroutine(this.IEShowCoopWin());
		}
		else if (this.state == EndMission.CoOpLost)
		{
			this.aniLogo.gameObject.SetActive(true);
			this.aniLogo.AnimationState.SetAnimation(0, "Failed", false);
			UnityEngine.Debug.Log("______________StartCoroutine(IEShowCoopLost())");
			base.StartCoroutine(this.IEShowCoopLost());
		}
		yield break;
	}

	private IEnumerator IEShowEnd()
	{
		try
		{
			this.OnSendEvent();
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		if (this.CheckDuplicateReward())
		{
			yield return new WaitForSeconds(0.5f);
			this.obj_EffectReward.SetActive(true);
			yield return new WaitForSeconds(0.2f);
			this.MergeReward();
		}
		if (this.totalExp > 0 || this.state == EndMission.PvPWin || this.state == EndMission.PvPLost)
		{
			yield return new WaitForSeconds(0.6f);
			this.ShowRankCurrent(true);
		}
		else
		{
			this.ShowRankCurrent(false);
		}
		yield return new WaitForSeconds(0.8f);
		this.objGroupButton.SetActive(true);
		this.isShowDone = true;
		if (AutoCheckLevel.isAutoCheck)
		{
			this.objGroupButton.SetActive(false);
			yield return new WaitForSeconds(1.5f);
			switch (this.state)
			{
			case EndMission.CampaignLost:
			case EndMission.CampaignWin:
				AutoCheckLevel.LevelCampaign++;
				GameManager.Instance.hudManager.OnLoadScene("UICampaign", this.isSkipInter);
				break;
			case EndMission.BossModeLost:
			case EndMission.BossModeWin:
				AutoCheckLevel.LevelBossMode++;
				this.OnBack();
				break;
			}
		}
		yield break;
	}

	private IEnumerator IEShowCampaignWin()
	{
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			yield return new WaitForSeconds(1f);
			if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
			{
				this.soundScore.Play();
			}
			this.totalGold = 500;
			SecuredInt goldTutorial = new SecuredInt(500);
			int indexCard = this.ShowReward(Item.Gold, goldTutorial.Value, false, -1);
			PopupManager.Instance.SaveReward(Item.Gold, goldTutorial.Value, base.name + "_Tutorial", null);
			LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)goldTutorial.Value, 0.4f).setOnUpdate(delegate(float val)
			{
				this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
			});
		}
		else
		{
			MissionDataLevel missionData = null;
			GameMode.Mode emode = GameMode.Instance.EMode;
			if (emode != GameMode.Mode.NORMAL)
			{
				if (emode != GameMode.Mode.HARD)
				{
					if (emode == GameMode.Mode.SUPER_HARD)
					{
						missionData = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_SuperHard_S[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_SuperHard[(int)GameManager.Instance.Level].missionDataLevel);
					}
				}
				else
				{
					missionData = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_Hard_S[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_Hard[(int)GameManager.Instance.Level].missionDataLevel);
				}
			}
			else
			{
				missionData = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_Normal[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_Normal[(int)GameManager.Instance.Level].missionDataLevel);
			}
			GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
			if (modePlay != GameMode.ModePlay.Campaign)
			{
				if (modePlay == GameMode.ModePlay.Special_Campaign)
				{
					GameMode.Mode mode = GameMode.Instance.MODE;
					if (mode != GameMode.Mode.NORMAL)
					{
						if (mode == GameMode.Mode.HARD)
						{
							ProfileManager.MapSpecialProfiles[ProfileManager.eLevelCurrent - ELevel.LEVEL_S0].SetUnlocked(GameMode.Mode.SUPER_HARD, true);
						}
					}
					else
					{
						ProfileManager.MapSpecialProfiles[ProfileManager.eLevelCurrent - ELevel.LEVEL_S0].SetUnlocked(GameMode.Mode.HARD, true);
					}
					Log.Info("________________________unlock special campaign here");
				}
			}
			else
			{
				bool isUnLocked = false;
				if (ProfileManager.LevelCurrent + ProfileManager.MapCurrent * 12 < 59 && (GameMode.Instance.EMode == GameMode.Mode.NORMAL || ProfileManager.LevelCurrent != 11))
				{
					ProfileManager.LevelCurrent++;
				}
				if (GameMode.Instance.EMode == GameMode.Mode.NORMAL && ProfileManager.LevelCurrent >= 12)
				{
					ProfileManager.MapCurrent++;
					ProfileManager.LevelCurrent = 0;
					isUnLocked = true;
					GameMode.Instance.CheckUnlockDifficulty();
				}
				ProfileManager._CampaignProfile.MapsProfile[ProfileManager.LevelCurrent + ProfileManager.MapCurrent * 12].SetUnlocked(GameMode.Instance.EMode, true);
				if (ProfileManager.LevelCurrent + ProfileManager.MapCurrent * 12 == 59)
				{
					GameMode.Instance.CheckUnlockDifficulty();
				}
				DailyQuestManager.Instance.Level(ProfileManager.MapCurrent, (int)((int)GameManager.Instance.Level % (int)ELevel.LEVEL_13), this.TotalStar, !GameManager.Instance.mMission.isRevive, isUnLocked, this.isLevelNew, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				AchievementManager.Instance.MissionUnlockMap(delegate(bool isCompleted, AchievementManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowAchievement(infor.Desc, infor.pathIcon);
					}
				});
			}
			this.StarActive = GameManager.Instance.mMission.Calculator();
			this.TotalStar = 0;
			for (int k = 0; k < this.StarActive.Length; k++)
			{
				if (this.StarActive[k])
				{
					this.TotalStar++;
				}
			}
			GameMode.ModePlay modePlay2 = GameMode.Instance.modePlay;
			if (modePlay2 != GameMode.ModePlay.Campaign)
			{
				if (modePlay2 == GameMode.ModePlay.Special_Campaign)
				{
					Log.Info("set star for level special campaign" + ProfileManager.eLevelCurrent);
				}
			}
			else
			{
				ProfileManager._CampaignProfile.MapsProfile[(int)GameManager.Instance.Level].SetStar(GameMode.Instance.MODE, this.TotalStar);
			}
			int totalStar = ProfileManager._CampaignProfile.GetTotalStar;
			AchievementManager.Instance.MissionEarnStar(totalStar, delegate(bool isCompleted, AchievementManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowAchievement(infor.Desc, infor.pathIcon);
				}
			});
			yield return new WaitForSeconds(0.5f);
			this.listResult.CreateObj(3);
			for (int l = 0; l < 3; l++)
			{
				CardInforResult component = this.listResult.listObjs[l].GetComponent<CardInforResult>();
				component.txt_Unit.gameObject.SetActive(false);
				component.txt_Value.gameObject.SetActive(false);
				component.obj_TabCampaign.SetActive(true);
				component.txt_Description.text = PopupManager.Instance.GetText((Localization0)missionData.missionData[l].idDesc, missionData.missionData[l].valueDesc);
				if (missionData.missionData[l].IsCompleted)
				{
					component.img_Star.sprite = this.sprite_StarOn;
					component.txt_Description.color = this.colorTextCompleted;
					component.img_Star.GetComponent<Image>().color = this.colorTextCompleted;
					component.obj_Completed.SetActive(true);
					component.txt_Gold.gameObject.SetActive(false);
					component.txt_Exp.gameObject.SetActive(false);
				}
				else
				{
					component.img_Star.sprite = this.sprite_StarOff;
				}
			}
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < 3; i++)
			{
				if (this.StarActive[i])
				{
					this.Obj_StarsOn[i].SetActive(true);
					CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
				}
				else
				{
					this.Obj_StarsOff[i].SetActive(true);
				}
				yield return new WaitForSeconds(0.6f);
			}
			this.starCompletedCurrent = 0;
			int starCompletedLast = 0;
			for (int j = 0; j < 3; j++)
			{
				yield return new WaitForSeconds(0.5f);
				CardInforResult card = this.listResult.listObjs[j].GetComponent<CardInforResult>();
				card.img_Star.sprite = ((!this.StarActive[j] && !missionData.missionData[j].IsCompleted) ? this.sprite_StarOff : this.sprite_StarOn);
				if (!missionData.missionData[j].IsCompleted && this.StarActive[j])
				{
					if (j == 0)
					{
						this.isLevelNew = true;
					}
					int value = missionData.missionData[j].gold_security.Value;
					if (!this.soundScore.isPlaying && value > 0 && ProfileManager.settingProfile.IsSound)
					{
						this.soundScore.Play();
					}
					LeanTween.value(card.txt_Gold.gameObject, 0f, (float)value, 0.4f).setOnUpdate(delegate(float val)
					{
						card.txt_Gold.text = ((int)val).ToString();
					});
					int indexCard = this.ShowReward(Item.Gold, value, false, -1);
					PopupManager.Instance.SaveReward(Item.Gold, value, base.name + "_CampainWin_Mission_" + j, null);
					LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)value, 0.4f).setOnUpdate(delegate(float val)
					{
						this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
					});
					int num = (!GameManager.Instance.isDoubleExp) ? missionData.missionData[j].exp_security.Value : (missionData.missionData[j].exp_security.Value * 2);
					LeanTween.value(card.txt_Exp.gameObject, 0f, (float)num, 0.4f).setOnUpdate(delegate(float val)
					{
						card.txt_Exp.text = ((int)val).ToString();
					});
					int indexCardExp = this.ShowReward(Item.Exp, num, false, -1);
					PopupManager.Instance.SaveReward(Item.Exp, num, base.name + "_CampainWin_Mission_" + j, null);
					LeanTween.value(this.listReward.listObjs[indexCardExp].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)num, 0.4f).setOnUpdate(delegate(float val)
					{
						this.listReward.listObjs[indexCardExp].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
					});
					this.totalGold += value;
					this.totalExp += num;
					this.starCompletedCurrent++;
				}
				if (missionData.missionData[j].IsCompleted)
				{
					starCompletedLast++;
				}
			}
			if (GameManager.Instance.Level == ELevel.LEVEL_13 && this.isLevelNew)
			{
				UIShowInforManager.Instance.ShowUnlock(PopupManager.Instance.GetText(Localization0.Pvp_Mode_is_unlocked, null));
			}
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign)
			{
				if (GameManager.Instance.bossManager.Boss != null && this.isLevelNew)
				{
					yield return new WaitForSeconds(0.5f);
					this.totalGem = 5;
					this.ShowReward(Item.Gem, this.totalGem, false, -1);
					PopupManager.Instance.SaveReward(Item.Gem, this.totalGem, base.name + "_CampaignWin", null);
					if (starCompletedLast == 0 && ProfileManager.inAppProfile.vipProfile.level >= E_Vip.Vip1)
					{
						yield return new WaitForSeconds(0.5f);
						int gemVip = DataLoader.vipData.Levels[(int)ProfileManager.inAppProfile.vipProfile.level].dailyReward.GemBonusBossInCampaign;
						this.ShowReward(Item.Gem, gemVip, false, (int)ProfileManager.inAppProfile.vipProfile.level);
						PopupManager.Instance.SaveReward(Item.Gem, gemVip, base.name + "_CampaignWin_VIP" + (int)ProfileManager.inAppProfile.vipProfile.level, null);
						this.totalGem += gemVip;
					}
					yield return new WaitForSeconds(0.5f);
				}
				if (GameManager.Instance.bossManager.Boss != null && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
				{
					ProfileManager.bossModeProfile.bossProfiles[(int)GameManager.Instance.bossManager.Boss.boss].SetUnlock(GameMode.Instance.MODE, true);
				}
			}
			if (this.COIN_COLLECTED > 0)
			{
				yield return new WaitForSeconds(0.5f);
				if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
				{
					this.soundScore.Play();
				}
				this.totalGold += this.COIN_COLLECTED;
				int indexCard = this.ShowReward(Item.Gold, this.COIN_COLLECTED, false, -1);
				PopupManager.Instance.SaveReward(Item.Gold, this.COIN_COLLECTED, base.name + "_CampainWin_Collected", null);
				LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)this.COIN_COLLECTED, 0.4f).setOnUpdate(delegate(float val)
				{
					this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
				});
			}
			if (!this.listReward.listObjs[0].activeSelf)
			{
				this.obj_NoItem.SetActive(true);
			}
			GameMode.Mode emode2 = GameMode.Instance.EMode;
			if (emode2 == GameMode.Mode.NORMAL)
			{
				FireBaseManager.Instance.LogEvent(FireBaseManager.LEVEL_MARKETING + (int)GameManager.Instance.Level);
			//	InAppManager.Instance.FlyerTrackingEvent(FireBaseManager.LEVEL_MARKETING, (int)GameManager.Instance.Level + string.Empty);
			}
			for (int m = 0; m < 3; m++)
			{
				if (!missionData.missionData[m].IsCompleted)
				{
					missionData.missionData[m].IsCompleted = this.StarActive[m];
				}
			}
			if (!this.ShowPopupRate())
			{
				this.isSkipInter = this.OnShowSale();
			}
			if (GameMode.Instance.modePlay != GameMode.ModePlay.Campaign || GameManager.Instance.Level != ELevel.LEVEL_1 || GameMode.Instance.modePlay != GameMode.ModePlay.Special_Campaign)
			{
				Singleton<CrossAdsManager>.Instance.ShowFloatAds();
			}
		}
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowCampaignLost()
	{
		yield return new WaitForSeconds(0.5f);
		this.listResult.CreateObj(3);
		string[] decInforResult = new string[]
		{
			PopupManager.Instance.GetText(Localization0.Time_Comsuming, null),
			PopupManager.Instance.GetText(Localization0.Enemies_Killed, null),
			PopupManager.Instance.GetText(Localization0.Percent_Completed, null)
		};
		for (int i = 0; i < 3; i++)
		{
			CardInforResult component = this.listResult.listObjs[i].GetComponent<CardInforResult>();
			component.txt_Unit.gameObject.SetActive(false);
			component.txt_Value.gameObject.SetActive(true);
			component.obj_TabCampaign.SetActive(false);
			component.txt_Value.text = string.Empty;
			component.txt_Description.text = decInforResult[i];
		}
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(this.listResult.listObjs[0].GetComponent<CardInforResult>().txt_Value.gameObject, 0f, (float)((int)GameManager.Instance.mMission.TimeGamePlay), 0.4f).setOnUpdate(delegate(float val)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)(val / 60f);
			int num2 = (int)(val - (float)(num * 60));
			stringBuilder.Length = 0;
			if (num < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num);
			stringBuilder.Append(":");
			if (num2 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num2);
			this.listResult.listObjs[0].GetComponent<CardInforResult>().txt_Value.text = stringBuilder.ToString();
		});
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && GameManager.Instance.TotalEnemyKilled > 0 && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(this.listResult.listObjs[1].GetComponent<CardInforResult>().txt_Value.gameObject, 0f, (float)GameManager.Instance.TotalEnemyKilled, 0.4f).setOnUpdate(delegate(float val)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Length = 0;
			int value = (int)val;
			int num = 4 - value.ToString().Length;
			for (int j = 0; j < num; j++)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(value);
			this.listResult.listObjs[1].GetComponent<CardInforResult>().txt_Value.text = stringBuilder.ToString();
		});
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && this.miniMap.percent_distance_completed > 0f && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(this.listResult.listObjs[2].GetComponent<CardInforResult>().txt_Value.gameObject, 0f, this.miniMap.percent_distance_completed, 0.4f).setOnUpdate(delegate(float val)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Length = 0;
			int value = (int)(val * 100f);
			stringBuilder.Append(value);
			stringBuilder.Append("%");
			this.listResult.listObjs[2].GetComponent<CardInforResult>().txt_Value.text = stringBuilder.ToString();
		});
		yield return new WaitForSeconds(0.5f);
		if (this.COIN_COLLECTED > 0)
		{
			if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
			{
				this.soundScore.Play();
			}
			this.totalGold += this.COIN_COLLECTED;
			int indexCard = this.ShowReward(Item.Gold, this.COIN_COLLECTED, false, -1);
			PopupManager.Instance.SaveReward(Item.Gold, this.COIN_COLLECTED, base.name + "_CampainLost_Collected", null);
			LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)this.COIN_COLLECTED, 0.4f).setOnUpdate(delegate(float val)
			{
				this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
			});
		}
		if (!this.listReward.listObjs[0].activeSelf)
		{
			this.obj_NoItem.SetActive(true);
		}
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowBossmodeWin()
	{
		int powerCurrent = 0;
		int idboss = (int)ProfileManager.bossCurrent;
		int mode = (int)GameMode.Instance.modeBossMode;
		try
		{
			powerCurrent = DataLoader.bossModeData.boss[idboss].power[mode];
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		this.totalGold = powerCurrent;
		this.totalGold /= 2;
		if (ProfileManager.bossModeProfile.bossReward[idboss].StateReward != 0)
		{
			this.totalGold /= 5;
		}
		else
		{
			ProfileManager.bossModeProfile.bossReward[idboss].StateReward++;
		}
		int gemBonus = 0;
		if (ProfileManager.bossModeProfile.bossReward[idboss].GetTimesGemReward(mode) < DataLoader.bossModeData.boss[idboss].timesGetGem)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			float num2 = 0.2f + (float)mode * 0.1f;
			if (num <= num2)
			{
				gemBonus = 10;
				ProfileManager.bossModeProfile.bossReward[idboss].AddTimesGemReward(mode);
			}
		}
		yield return new WaitForSeconds(0.5f);
		this.listResult.CreateObj(1);
		CardInforResult cardInforResult = this.listResult.listObjs[0].GetComponent<CardInforResult>();
		cardInforResult.txt_Unit.gameObject.SetActive(false);
		cardInforResult.txt_Value.gameObject.SetActive(true);
		cardInforResult.obj_TabCampaign.SetActive(false);
		cardInforResult.txt_Description.text = PopupManager.Instance.GetText(Localization0.Time_Comsuming, null);
		cardInforResult.txt_Value.text = string.Empty;
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(cardInforResult.txt_Value.gameObject, 0f, (float)((int)GameManager.Instance.mMission.TimeGamePlay), 0.4f).setOnUpdate(delegate(float val)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = (int)(val / 60f);
			int num4 = (int)(val - (float)(num3 * 60));
			stringBuilder.Length = 0;
			if (num3 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num3);
			stringBuilder.Append(":");
			if (num4 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num4);
			cardInforResult.txt_Value.text = stringBuilder.ToString();
		});
		yield return new WaitForSeconds(0.5f);
		if (ProfileManager.bossModeProfile.bossReward[idboss].StateReward == 1)
		{
			yield return new WaitForSeconds(0.5f);
			int vipLevel = (int)ProfileManager.inAppProfile.vipProfile.level;
			int bonusVip = ((DataLoader.bossModeData.boss[idboss].reward.id >= 17 && DataLoader.bossModeData.boss[idboss].reward.id <= 36) || ProfileManager.inAppProfile.vipProfile.level < E_Vip.Vip1 || !DataLoader.vipData.Levels[vipLevel].dailyReward.isX2RewardBossMode) ? 1 : 2;
			if (bonusVip == 1)
			{
				vipLevel = -1;
			}
			this.ShowReward((Item)DataLoader.bossModeData.boss[idboss].reward.id, DataLoader.bossModeData.boss[idboss].reward.value * bonusVip, false, vipLevel);
			ProfileManager.bossModeProfile.bossReward[idboss].StateReward = 2;
			PopupManager.Instance.SaveReward((Item)DataLoader.bossModeData.boss[idboss].reward.id, DataLoader.bossModeData.boss[idboss].reward.value * bonusVip, base.name + "_BossModeWin", null);
		}
		yield return new WaitForSeconds(0.5f);
		if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundScore.Play();
		}
		this.totalGold += this.COIN_COLLECTED;
		int indexCard = this.ShowReward(Item.Gold, this.totalGold, false, -1);
		PopupManager.Instance.SaveReward(Item.Gold, this.totalGold, base.name + "_BossModeWin", null);
		LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)this.totalGold, 0.4f).setOnUpdate(delegate(float val)
		{
			this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
		});
		if (gemBonus > 0)
		{
			if (this.listReward.listObjs[0].activeSelf)
			{
				yield return new WaitForSeconds(0.5f);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
			int vipLevel2 = (int)ProfileManager.inAppProfile.vipProfile.level;
			int bonusVip2 = (ProfileManager.inAppProfile.vipProfile.level < E_Vip.Vip1 || !DataLoader.vipData.Levels[vipLevel2].dailyReward.isX2RewardBossMode) ? 1 : 2;
			this.ShowReward(Item.Gem, gemBonus * bonusVip2, false, vipLevel2);
			PopupManager.Instance.SaveReward(Item.Gem, gemBonus * bonusVip2, base.name + "_BossModeWin", null);
		}
		yield return new WaitForSeconds(1f);
		if (!this.listReward.listObjs[0].activeSelf)
		{
			this.obj_NoItem.SetActive(true);
		}
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowBossmodeLost()
	{
		yield return new WaitForSeconds(0.5f);
		this.obj_NoItem.SetActive(true);
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowPvpWin()
	{
		yield return new WaitForSeconds(0.5f);
		this.listResult.CreateObj(1);
		CardInforResult cardInforResult = this.listResult.listObjs[0].GetComponent<CardInforResult>();
		cardInforResult.txt_Unit.gameObject.SetActive(false);
		cardInforResult.txt_Value.gameObject.SetActive(true);
		cardInforResult.obj_TabCampaign.SetActive(false);
		cardInforResult.txt_Description.text = PopupManager.Instance.GetText(Localization0.Time_Comsuming, null);
		cardInforResult.txt_Value.text = string.Empty;
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(cardInforResult.txt_Value.gameObject, 0f, (float)((int)PVPManager.PVPManager.Instance.totalPlayTime), 0.4f).setOnUpdate(delegate(float val)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)(val / 60f);
			int num2 = (int)(val - (float)(num * 60));
			stringBuilder.Length = 0;
			if (num < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num);
			stringBuilder.Append(":");
			if (num2 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num2);
			cardInforResult.txt_Value.text = stringBuilder.ToString();
		});
		yield return new WaitForSeconds(0.5f);
		this.listResult.CreateObj(2);
		CardInforResult cardInforResult2 = this.listResult.listObjs[1].GetComponent<CardInforResult>();
		cardInforResult2.txt_Unit.gameObject.SetActive(false);
		cardInforResult2.txt_Value.gameObject.SetActive(true);
		cardInforResult2.obj_TabCampaign.SetActive(false);
		cardInforResult2.txt_Description.text = PopupManager.Instance.GetText(Localization0.Wave, null);
		cardInforResult2.txt_Value.text = string.Empty;
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(cardInforResult2.txt_Value.gameObject, 0f, (float)(PvP_LocalPlayer.Instance.EnemyTurn + 1), 0.4f).setOnUpdate(delegate(float val)
		{
			cardInforResult2.txt_Value.text = ((int)val).ToString();
		});
		UnityEngine.Debug.LogWarning("????" + PvP_LocalPlayer.Instance.EnemyTurn + 1);
		if (PhotonSingleton<PvP_RoomProperty>.Instance.BettingType == PvP_RoomProperty.RoomBettingType.GOLD)
		{
			yield return new WaitForSeconds(0.5f);
			this.totalGold = (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1) * PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
			{
				this.soundScore.Play();
			}
			int indexCard = this.ShowReward(Item.Gold, this.totalGold, false, -1);
			PopupManager.Instance.SaveReward(Item.Gold, this.totalGold + PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount, base.name + "_PvpWin_1vs" + (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1), null);
			LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)this.totalGold, 0.4f).setOnUpdate(delegate(float val)
			{
				this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
			});
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
			this.totalGem = (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1) * PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			this.ShowReward(Item.Gem, this.totalGem, false, -1);
			PopupManager.Instance.SaveReward(Item.Gem, this.totalGem + PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount, base.name + "_PvpWin_1vs" + (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1), null);
		}
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowPvpLost()
	{
		yield return new WaitForSeconds(0.5f);
		this.listResult.CreateObj(1);
		CardInforResult cardInforResult = this.listResult.listObjs[0].GetComponent<CardInforResult>();
		cardInforResult.txt_Unit.gameObject.SetActive(false);
		cardInforResult.txt_Value.gameObject.SetActive(true);
		cardInforResult.obj_TabCampaign.SetActive(false);
		cardInforResult.txt_Description.text = PopupManager.Instance.GetText(Localization0.Time_Comsuming, null);
		cardInforResult.txt_Value.text = "00:00";
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(cardInforResult.txt_Value.gameObject, 0f, (float)((int)PVPManager.PVPManager.Instance.totalPlayTime), 0.4f).setOnUpdate(delegate(float val)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)(val / 60f);
			int num2 = (int)(val - (float)(num * 60));
			stringBuilder.Length = 0;
			if (num < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num);
			stringBuilder.Append(":");
			if (num2 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num2);
			cardInforResult.txt_Value.text = stringBuilder.ToString();
		});
		yield return new WaitForSeconds(0.5f);
		this.listResult.CreateObj(2);
		CardInforResult cardInforResult2 = this.listResult.listObjs[1].GetComponent<CardInforResult>();
		cardInforResult2.txt_Unit.gameObject.SetActive(false);
		cardInforResult2.txt_Value.gameObject.SetActive(true);
		cardInforResult2.obj_TabCampaign.SetActive(false);
		cardInforResult2.txt_Description.text = PopupManager.Instance.GetText(Localization0.Wave, null);
		cardInforResult2.txt_Value.text = "0";
		yield return new WaitForSeconds(0.5f);
		if (!this.soundTyping2.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundTyping2.Play();
		}
		LeanTween.value(cardInforResult2.txt_Value.gameObject, 0f, (float)(PvP_LocalPlayer.Instance.EnemyTurn + 1), 0.4f).setOnUpdate(delegate(float val)
		{
			cardInforResult2.txt_Value.text = ((int)val).ToString();
		});
		if (PhotonSingleton<PvP_RoomProperty>.Instance.BettingType == PvP_RoomProperty.RoomBettingType.GOLD)
		{
			yield return new WaitForSeconds(1f);
			this.totalGold = (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1) * PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
			{
				this.soundScore.Play();
			}
			int indexCard = this.ShowReward(Item.Gold, -this.totalGold, false, -1);
		}
		else
		{
			yield return new WaitForSeconds(1f);
			this.totalGem = (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1) * PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			this.ShowReward(Item.Gem, -this.totalGem, false, -1);
		}
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowCoopWin()
	{
		yield return new WaitForSeconds(0.5f);
		if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.soundScore.Play();
		}
		float rand = UnityEngine.Random.Range(0f, 1f);
		Item item = Item.Gold;
		int value = 3000;
		if (rand <= 0.6f)
		{
			value = 3000;
			item = Item.Gold;
		}
		else if (rand <= 0.9f)
		{
			value = 1;
			item = Item.Common_Crate;
		}
		else
		{
			value = 1;
			item = Item.Epic_Crate;
		}
		int indexCard = this.ShowReward(item, value, false, -1);
		PopupManager.Instance.SaveReward(item, value, "Coop", null);
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	private IEnumerator IEShowCoopLost()
	{
		yield return new WaitForSeconds(0.5f);
		this.obj_NoItem.SetActive(true);
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
		this.isSkipInter = this.OnShowSale();
		base.StartCoroutine(this.IEShowEnd());
		yield break;
	}

	public void OnMenu()
	{
		GameManager.Instance.audioManager.StopAll();
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
		GameManager.Instance.hudManager.OnLoadScene("Menu", this.isSkipInter);
		string parameterValue = "GamePlay";
		
	}

	public void OnBack()
	{
		GameManager.Instance.audioManager.StopAll();
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
		if (this.state == EndMission.CampaignWin || this.state == EndMission.CampaignLost)
		{
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				FormCampaign.isCampaignContinue = true;
			}
			GameManager.Instance.hudManager.OnLoadScene("UICampaign", this.isSkipInter);
			string parameterValue = "GamePlay";
			
		}
		else if (this.state == EndMission.BossModeWin || this.state == EndMission.BossModeLost)
		{
			GameManager.Instance.hudManager.OnLoadScene("UIBossMode", this.isSkipInter);
			string parameterValue2 = "GamePlay";
			
		}
	}

	public void OnUpgrade()
	{
		GameManager.Instance.audioManager.StopAll();
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
		GameManager.Instance.hudManager.OnLoadScene("UIWeapon", this.isSkipInter);
		string parameterValue = "GamePlay";
		
	}

	public void OnReStart()
	{
		GameManager.Instance.audioManager.StopAll();
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
		if (this.state == EndMission.CampaignWin)
		{
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				FormCampaign.isCampaignContinue = true;
			}
			else
			{
				GameMode.Instance.MODE = GameMode.Mode.NORMAL;
			}
			GameManager.Instance.hudManager.OnLoadScene("UICampaign", this.isSkipInter);
			string parameterValue = "GamePlay";
			
		}
		else if (this.state == EndMission.CampaignLost)
		{
			FormLoadout.typeForm = FormLoadout.Type.Campaign;
			FormCampaign.isCampaignContinue = true;
			GameManager.Instance.hudManager.OnLoadScene("UILoadout", this.isSkipInter);
			string parameterValue2 = "GamePlay";
			
		}
		else if (this.state == EndMission.BossModeWin)
		{
			FormLoadout.typeForm = FormLoadout.Type.BossMode;
			FormBossMode.isNextBoss = true;
			GameManager.Instance.hudManager.OnLoadScene("UIBossMode", this.isSkipInter);
			string parameterValue3 = "GamePlay";
			
		}
		else if (this.state == EndMission.BossModeLost)
		{
			FormLoadout.typeForm = FormLoadout.Type.BossMode;
			GameManager.Instance.hudManager.OnLoadScene("UILoadout", this.isSkipInter);
			string parameterValue4 = "GamePlay";
			
		}
		else if (this.state == EndMission.PvPWin || this.state == EndMission.PvPLost || this.state == EndMission.CoOpLost || this.state == EndMission.CoOpWin)
		{
			GameManager.Instance.hudManager.OnLoadScene("UIPvp", this.isSkipInter);
			string parameterValue5 = "GamePlay";
			
		}
	}

	public void OnDoubleGold()
	{
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
		if (this.isReceived)
		{
			PopupManager.Instance.mDialog.typeDialog = 0;
			PopupManager.Instance.mDialog.isOK = null;
			PopupManager.Instance.mDialog.txtTitle.text = PopupManager.Instance.GetText(Localization0.Alert, null);
			string[] nameSpec = new string[]
			{
				this.CoinVideoReward_Secured.ToString()
			};
			PopupManager.Instance.mDialog.txtContent.text = PopupManager.Instance.GetText(Localization0.You_have_already_received_golds, nameSpec);
			PopupManager.Instance.mDialog.Show();
			return;
		}
		this.btn_Bonus.gameObject.SetActive(false);
		AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isSuccess)
		{
			if (isSuccess)
			{
				this.isSkipInter = true;
				if (!this.isReceived)
				{
					this.isReceived = true;
					if (!this.soundScore.isPlaying && ProfileManager.settingProfile.IsSound)
					{
						this.soundScore.Play();
					}
					int indexCard = this.ShowReward(Item.Gold, this.CoinVideoReward_Secured.Value, true, -1);
					PopupManager.Instance.SaveReward(Item.Gold, this.CoinVideoReward_Secured.Value, base.name + "_BonusVideo_" + this.state.ToString(), null);
					LeanTween.value(this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.gameObject, 0f, (float)this.CoinVideoReward_Secured.Value, 0.4f).setOnUpdate(delegate(float val)
					{
						this.listReward.listObjs[indexCard].GetComponent<CardBase>().txt_Amount.text = ((int)val).ToString();
					});
					float num = UnityEngine.Random.Range(0f, 1f);
					if (num <= 0.2f && this.objDropGem.activeSelf)
					{
						this.ShowReward(Item.Gem, 5, true, -1);
						PopupManager.Instance.SaveReward(Item.Gem, 5, base.name + "_BonusVideo_" + this.state.ToString(), null);
					}
					try
					{
						
					}
					catch (Exception message)
					{
						UnityEngine.Debug.LogError(message);
					}
				}
				else
				{
					this.isReceived = false;
					this.btn_Bonus.gameObject.SetActive(true);
				}
			}
		});
	}

	private void ShowRankCurrent(bool isPingpong)
	{
		if (isPingpong)
		{
			this.animPingPong.enabled = true;
			this.animPingPong.Play(0);
			this.animPingPongPvp.enabled = true;
			this.animPingPongPvp.Play(0);
		}
		if (this.state == EndMission.PvPLost && isPingpong)
		{
			this.txt_PointPvp.text = ProfileManager.pvpProfile.Score + " Pts ";
			float num = (float)(ProfileManager.pvpProfile.Score + 5 * (ProfileManager.pvpProfile.WinStreak - 1));
			float num2 = 15f;
			float num3 = 0.002f;
			float num4 = num3 * num / (1f + num3 * Mathf.Abs(num));
			int num5 = (int)(num2 * (1f + num4));
			Text text = this.txt_PointPvp;
			string text2 = text.text;
			text.text = string.Concat(new object[]
			{
				text2,
				"<color=#FF4F4FFF>- ",
				num5,
				" Pts</color>"
			});
			ProfileManager.pvpProfile.Score -= num5;
		}
		else if (this.state == EndMission.PvPWin && isPingpong)
		{
			this.txt_PointPvp.text = ProfileManager.pvpProfile.Score + " Pts ";
			float num6 = (float)(ProfileManager.pvpProfile.Score + 5 * (ProfileManager.pvpProfile.WinStreak - 1));
			float num7 = 15f;
			float num8 = 0.002f;
			float num9 = num8 * num6 / (1f + num8 * Mathf.Abs(num6));
			int num10 = (int)(num7 * (1f + num9));
			Text text3 = this.txt_PointPvp;
			string text2 = text3.text;
			text3.text = string.Concat(new object[]
			{
				text2,
				"<color=#74FF4FFF>+ ",
				num10,
				" Pts</color>"
			});
			ProfileManager.pvpProfile.Score += num10;
		}
		else
		{
			this.txt_Exp.text = RankManager.Instance.ExpCurrent() + " " + PopupManager.Instance.GetText(Localization0.Exp, null);
			RankInfor rankCurrentByExp = RankManager.Instance.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
			this.img_Rank.sprite = PopupManager.Instance.sprite_RankAccount[rankCurrentByExp.Level];
			this.img_CoreExp.fillAmount = RankManager.Instance.ExpCurrent01();
			this.txt_PointPvp.text = ProfileManager.pvpProfile.Score + " Pts ";
		}
	}

	private void OnSendEvent()
	{
		if (this.state == EndMission.CampaignWin)
		{
			
		}
		else if (this.state == EndMission.CampaignLost)
		{
			
		}
		else if (this.state == EndMission.BossModeWin)
		{
			
		}
		else if (this.state == EndMission.BossModeLost)
		{
			
		}
		else if (this.state == EndMission.PvPWin)
		{
			
		}
		else if (this.state == EndMission.PvPLost)
		{
			
		}
	}

	private bool OnShowSale()
	{
		if (PopupWinLost.CountCompleted < 3)
		{
			return false;
		}
		PopupWinLost.CountCompleted = 0;
		if (this.saleFromServer != null)
		{
			this.saleFromServer.OnShow();
			return true;
		}
		if (SaleManager.Instance.CountSaleSesion < 1)
		{
			this.OnShowSalePack();
			SaleManager.Instance.CountSaleSesion++;
		}
		else
		{
			this.OnShowDailySaleRandom();
		}
		return true;
	}

	private void OnShowDailySaleRandom()
	{
		this.popupDailySale.OnShowDailySaleRandom();
	}

	private void OnShowSalePack()
	{
		SaleManager.TYPE_SALE type_SALE = SaleManager.Instance.OnShowSalePack();
		if (type_SALE != SaleManager.TYPE_SALE.Double_Gem)
		{
			if (type_SALE != SaleManager.TYPE_SALE.StarterPack_1)
			{
				if (type_SALE == SaleManager.TYPE_SALE.DailySale)
				{
					this.popupDailySale.OnShowDailySale();
				}
			}
			else
			{
				this.popupStarterPack.OnShowPack(true);
			}
		}
		else
		{
			this.popupDailySale.OnShowDoubleGem();
		}
	}

	private bool ShowPopupRate()
	{
		try
		{
			ELevel level = GameManager.Instance.Level;
			if (level == ELevel.LEVEL_3 || level == ELevel.LEVEL_12 || level == ELevel.LEVEL_16)
			{
				if (PlayerPrefs.GetInt("com.sora.metal.squad.rategame", 0) == 0 && PlayerPrefs.GetInt("com.sora.metal.squad.rategame" + GameManager.Instance.Level, 0) == 0)
				{
					PopupManager.Instance.ShowRatePopup(delegate(bool callback)
					{
						if (callback)
						{
							string url_RATE_ANDROID = GameConfig.URL_RATE_ANDROID;
							Application.OpenURL(url_RATE_ANDROID);
							PlayerPrefs.SetInt("com.sora.metal.squad.rategame", 1);
						}
						else
						{
							PlayerPrefs.SetInt("com.sora.metal.squad.rategame" + GameManager.Instance.Level, 1);
						}
					});
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exeption " + ex.Message);
		}
		return false;
	}

	private bool CheckDuplicateReward()
	{
		string text = "_";
		for (int i = 0; i < this.amountReward; i++)
		{
			string str = (int)this.listReward.listObjs[i].GetComponent<CardBase>().item + string.Empty;
			if (text.Contains("_" + str + "_"))
			{
				return true;
			}
			text = text + str + "_";
		}
		return false;
	}

	private void MergeReward()
	{
		for (int i = 0; i < this.amountReward - 1; i++)
		{
			for (int j = 1; j < this.amountReward; j++)
			{
				if (this.listReward.listObjs[j].GetComponent<CardBase>().item != Item.None)
				{
					if (i != j && this.listReward.listObjs[i].GetComponent<CardBase>().item == this.listReward.listObjs[j].GetComponent<CardBase>().item)
					{
						this.listReward.listObjs[i].GetComponent<CardBase>().value += this.listReward.listObjs[j].GetComponent<CardBase>().value;
						this.listReward.listObjs[j].GetComponent<CardBase>().item = Item.None;
					}
				}
			}
		}
		List<Item> list = new List<Item>();
		List<int> list2 = new List<int>();
		for (int k = 0; k < this.amountReward; k++)
		{
			if (this.listReward.listObjs[k].GetComponent<CardBase>().item != Item.None)
			{
				list.Add(this.listReward.listObjs[k].GetComponent<CardBase>().item);
				list2.Add(this.listReward.listObjs[k].GetComponent<CardBase>().value);
			}
		}
		this.amountReward = 0;
		for (int l = 0; l < list.Count; l++)
		{
			this.ShowReward(list[l], list2[l], false, -1);
		}
	}

	private int ShowReward(Item item, int amount, bool videoGold = false, int isVip = -1)
	{
		this.amountReward++;
		this.obj_NoItem.SetActive(false);
		this.listReward.CreateObj(this.amountReward);
		CardBase component = this.listReward.listObjs[this.amountReward - 1].GetComponent<CardBase>();
		this.listReward.listObjs[this.amountReward - 1].transform.localScale = Vector3.one;
		component.item = item;
		component.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(item)];
		component.img_Main.sprite = PopupManager.Instance.sprite_Item[(int)item];
		component.value = amount;
		component.txt_Amount.text = amount + string.Empty;
		component.obj_Note.SetActive(videoGold);
		component.img_Lock.gameObject.SetActive(isVip >= 0);
		component.img_Lock.sprite = PopupManager.Instance.spriteVip[isVip + 1];
		component.ShowBorderEffect();
		return this.amountReward - 1;
	}

	private void SetGoldVideoBonus()
	{
		this.objDropGem.SetActive(UnityEngine.Random.Range(0f, 1f) <= 0.5f);
		if (this.state == EndMission.CampaignWin)
		{
			int num = (int)(this.goldBase + (int)GameManager.Instance.Level * (int)(ELevel)this.goldBonusEachLevel);
			this.CoinVideoReward_Secured = new SecuredInt(UnityEngine.Random.Range(num, (int)((float)num * 2f)));
			this.txtCoinVideoReward.text = (num * 2).ToString();
		}
		else if (this.state == EndMission.CampaignLost)
		{
			if (this.miniMap.percent_distance_completed > 0.3f)
			{
				int num = (int)(this.goldBase + (int)GameManager.Instance.Level * (int)(ELevel)this.goldBonusEachLevel);
				this.CoinVideoReward_Secured = new SecuredInt(UnityEngine.Random.Range(num, (int)((float)num * 2f)));
				this.txtCoinVideoReward.text = (num * 2).ToString();
			}
			else
			{
				this.btn_Bonus.gameObject.SetActive(false);
			}
		}
		else if (this.state == EndMission.BossModeWin)
		{
			int num = (int)(this.goldBase + (int)ProfileManager.bossCurrent * (int)(EBoss)this.goldBonusEachLevel);
			this.CoinVideoReward_Secured = new SecuredInt(UnityEngine.Random.Range(num / 2, (int)((float)num * 1.5f)));
			this.txtCoinVideoReward.text = (num * 2).ToString();
		}
		else if (this.state == EndMission.BossModeLost)
		{
			int num = (int)(this.goldBase + (int)ProfileManager.bossCurrent * (int)(EBoss)this.goldBonusEachLevel);
			this.CoinVideoReward_Secured = new SecuredInt(UnityEngine.Random.Range(num / 2, (int)((float)num * 1.5f)));
			this.txtCoinVideoReward.text = (num * 2).ToString();
		}
		else
		{
			this.btn_Bonus.gameObject.SetActive(false);
		}
	}

	public TextLocalization[] listTextLocalization;

	public const string SAVE_GAME_CAMPAIGN_LOST = "save.game.campaign.lost.";

	public EndMission state;

	private int COIN_COLLECTED;

	private int amountReward;

	private bool isShowDone;

	private bool isSkipInter;

	private bool isLevelNew;

	private int TotalStar;

	private int starCompletedCurrent;

	private int totalGold;

	private int totalExp;

	private int totalGem;

	[Header("-----Rank Exp-----")]
	public Text txt_Exp;

	public Image img_CoreExp;

	public Image img_Rank;

	public Animator animPingPong;

	public Text txt_PointPvp;

	public Animator animPingPongPvp;

	public GameObject obj_NonPvp;

	public GameObject obj_Pvp;

	[Header("-----Gold-----")]
	public GameObject obj_EffectReward;

	public Text txt_Gold;

	[Header("-----Top-----")]
	public SkeletonGraphic aniLogo;

	public GameObject obj_ListStar;

	public GameObject[] Obj_StarsOn;

	public GameObject[] Obj_StarsOff;

	public Color colorTextCompleted;

	public Sprite sprite_StarOn;

	public Sprite sprite_StarOff;

	[Header("-----Video Reward-----")]
	private bool isReceived;

	public Text txtCoinVideoReward;

	private SecuredInt CoinVideoReward_Secured;

	[Header("-----Result-----")]
	public Text txt_ModeGame;

	public Text txt_BestResult;

	public Text txt_BossModeLost;

	[Header("-----Starter Pack-----")]
	public PopupStarterPack popupStarterPack;

	public PopupDailySale popupDailySale;

	private BestSaleCalculator saleFromServer;

	[Header("-----Button-----")]
	public Text txt_Restart;

	public Text txt_Back;

	public Button btn_Back;

	public Button btn_Menu;

	public Button btn_Upgrade;

	public Button btn_Restart;

	public Button btn_Bonus;

	public GameObject objEffectRestart;

	public GameObject objEffectBonus;

	public GameObject obj_NoItem;

	public GameObject obj_OpenPopup;

	public FactoryObject listReward;

	public FactoryObject listResult;

	[Header("-----Endless-----")]
	public GameObject objTimePlay;

	public AudioSource soundScore;

	public AudioSource soundTyping2;

	[Header("-----Campaign-----")]
	public UIMiniMap miniMap;

	public GameObject objGroupButton;

	public GameObject objDropGem;

	public static int CountCompleted;

	private bool[] StarActive;

	private int goldBase = 500;

	private int goldBonusEachLevel = 40;
}
