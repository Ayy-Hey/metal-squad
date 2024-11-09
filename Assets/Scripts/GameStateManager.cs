using System;
using System.Collections;
using UIManager.Popup;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	public void InitObject()
	{
		GameManager.Instance.bombManager.SetResume();
		GameManager.Instance.bulletManager.Resume();
		GameManager.Instance.hudManager.txtStart.gameObject.SetActive(true);
		GameManager.Instance.hudManager.HideControl();
		this.inforGamePlayPause.Init();
	}

	public void SetPreview()
	{
		if (this.EState == EGamePlay.PREVIEW)
		{
			return;
		}
		this.EState = EGamePlay.PREVIEW;
		GameManager.Instance.bulletManager.DestroyAll();
	}

	public void SetBegin()
	{
		this.EState = EGamePlay.BEGIN;
		if (GameManager.Instance.OnGameBegin != null)
		{
			GameManager.Instance.OnGameBegin();
		}
		if (!object.ReferenceEquals(GameManager.Instance.bossManager.Boss32, null))
		{
			GameManager.Instance.bossManager.Boss32.InitEnemy(4, DataLoader.boss);
		}
	}

	public void SetGameRescue()
	{
		if (this.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.EState = EGamePlay.PAUSE;
		this.popupRescue.Show();
		if (GameManager.Instance.player != null && GameManager.Instance.player.gameObject.activeSelf)
		{
			GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
		}
		GameManager.Instance.bombManager.SetPause();
		GameManager.Instance.bulletManager.DestroyAll();
		EnemyManager.Instance.OnPause();
		GameManager.Instance.skillManager.OnPause();
		if (GameManager.Instance.isJetpackMode)
		{
			JetpackManager.Instance.OnPause();
		}
		if (this.descr != null)
		{
			this.descr.pause();
		}
	}

	public void SetGameRuning()
	{
		if (this.EState == EGamePlay.RUNNING)
		{
			return;
		}
		if (this.EState == EGamePlay.BEGIN)
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
			{
				ProfileManager.bossModeProfile.UpPlayCount(GameMode.Instance.modeBossMode);
				MonoBehaviour.print("day thong tin so lan choi boss mode");
			}
			GameManager.Instance.hudManager.ShowControlStartGame();
		}
		if (this.EState == EGamePlay.PAUSE && this.inforGamePlayPause.control.gameObject.activeSelf)
		{
			this.inforGamePlayPause.control.Back();
			return;
		}
		GameManager.Instance.bombManager.SetResume();
		GameManager.Instance.bulletManager.Resume();
		GameManager.Instance.skillManager.OnResume();
		EnemyManager.Instance.OnResume();
		if (GameManager.Instance.isJetpackMode)
		{
			JetpackManager.Instance.OnResume();
		}
		this.popupRescue.gameObject.SetActive(false);
		if (this.descr != null)
		{
			this.descr.resume();
		}
		this.EState = EGamePlay.RUNNING;
		this.ShowPopup();
		if (AutoCheckLevel.isAutoCheck)
		{
			AutoCheckLevelEditor.TypeCheck typeCheck = (AutoCheckLevelEditor.TypeCheck)AutoCheckLevel.typeCheck;
			if (typeCheck != AutoCheckLevelEditor.TypeCheck.Pause)
			{
				if (typeCheck != AutoCheckLevelEditor.TypeCheck.Win)
				{
					if (typeCheck == AutoCheckLevelEditor.TypeCheck.Lost)
					{
						EventDispatcher.PostEvent("LostGame");
					}
				}
				else
				{
					EventDispatcher.PostEvent("CompletedGame");
				}
			}
			else
			{
				EventDispatcher.PostEvent("PauseGame");
			}
		}
	}

	public void CloseTutorial()
	{
		GameManager.Instance.bombManager.SetResume();
		GameManager.Instance.bulletManager.Resume();
		GameManager.Instance.hudManager.ShowControl(1.1f);
		GameManager.Instance.skillManager.OnResume();
		EnemyManager.Instance.OnResume();
		this.popupRescue.gameObject.SetActive(false);
		if (this.descr != null)
		{
			this.descr.resume();
		}
		this.ShowPopup();
	}

	public void SetGamePause()
	{
		if (this.EState == EGamePlay.PAUSE)
		{
			return;
		}
		this.EState = EGamePlay.PAUSE;
		this.inforGamePlayPause.Show();
		this.ShowPopup();
		if (this.descr != null)
		{
			this.descr.pause();
		}
	}

	public void GameOver()
	{
		GameManager.Instance.bulletManager.DestroyAll();
		GameManager.Instance.hudManager.HideControl();
		base.StartCoroutine(this.IEGameOver());
	}

	private IEnumerator IEGameOver()
	{
		yield return new WaitForSeconds(1f);
		GameManager.Instance.audioManager.StopAll();
		if (!GameManager.Instance.isRescue)
		{
			this.SetGameRescue();
			GameManager.Instance.isRescue = true;
		}
		else
		{
			this.SetGameLost();
		}
		yield break;
	}

	public void GameOverNow()
	{
		GameManager.Instance.bulletManager.DestroyAll();
		GameManager.Instance.hudManager.HideControl();
		GameManager.Instance.audioManager.StopAll();
		this.SetGameLost();
	}

	public void SetGameLost()
	{
		if (this.EState == EGamePlay.LOST)
		{
			return;
		}
		this.EState = EGamePlay.LOST;
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.MultiPlayer)
		{
			if (style == GameMode.GameStyle.SinglPlayer)
			{
				switch (GameMode.Instance.modePlay)
				{
				case GameMode.ModePlay.Campaign:
				case GameMode.ModePlay.Special_Campaign:
					this.inforGamePlayPause.gameObject.SetActive(false);
					this.popupEndMission.state = EndMission.CampaignLost;
					this.popupEndMission.OnShow();
					break;
				case GameMode.ModePlay.Boss_Mode:
					this.popupEndMission.state = EndMission.BossModeLost;
					this.popupEndMission.OnShow();
					break;
				case GameMode.ModePlay.PvpMode:
					this.popupEndMission.state = EndMission.PvPLost;
					this.popupEndMission.OnShow();
					break;
				}
			}
		}
		else
		{
			Log.Info("__________________ PVP GAME_OVER");
			this.inforGamePlayPause.gameObject.SetActive(false);
		}
		if (this.descr != null)
		{
			this.descr.pause();
		}
	}

	public void SetGameWin()
	{
		if (this.EState == EGamePlay.WIN)
		{
			return;
		}
		this.EState = EGamePlay.WIN;
		switch (GameMode.Instance.modePlay)
		{
		case GameMode.ModePlay.Campaign:
		case GameMode.ModePlay.Special_Campaign:
			this.inforGamePlayPause.gameObject.SetActive(false);
			this.popupEndMission.state = EndMission.CampaignWin;
			this.popupEndMission.OnShow();
			break;
		case GameMode.ModePlay.Boss_Mode:
			ProfileManager.bossModeProfile.UpWinCount(GameMode.Instance.modeBossMode);
			if (GameManager.Instance.player != null && GameManager.Instance.player.gameObject.activeSelf)
			{
				GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
			}
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
			}
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, 1000f);
			this.popupEndMission.state = EndMission.BossModeWin;
			this.popupEndMission.OnShow();
			UnityEngine.Debug.Log("đẩy thông tin win boss mode");
			break;
		case GameMode.ModePlay.PvpMode:
			this.popupEndMission.state = EndMission.PvPWin;
			this.popupEndMission.OnShow();
			break;
		}
		if (this.descr != null)
		{
			this.descr.pause();
		}
	}

	public void SetWaitingBoss()
	{
		if (this.EState == EGamePlay.WIN || this.EState == EGamePlay.LOST || this.EState == EGamePlay.WAITING_BOSS)
		{
			return;
		}
		this.EState = EGamePlay.WAITING_BOSS;
		GameManager.Instance.bulletManager.Pause();
	}

	private void ShowPopup()
	{
		this.inforGamePlayPause.gameObject.SetActive(this.EState == EGamePlay.PAUSE);
	}

	public EGamePlay EState;

	[Header("Popup Game Play")]
	public PopupRescue popupRescue;

	public InforGamePlay inforGamePlayPause;

	[SerializeField]
	public PopupWinLost popupEndMission;

	public GameObject Rambo_Stop;

	[NonSerialized]
	public LTDescr descr;

	public AirplaneRambo airplaneRambo;
}
