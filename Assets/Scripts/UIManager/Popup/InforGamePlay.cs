using System;
using System.Collections;
using System.Text;
using Com.LuisPedroFonseca.ProCamera2D;
using StarMission;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager.Popup
{
	public class InforGamePlay : MonoBehaviour
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

		public void Init()
		{
			this.strBuilderTime = new StringBuilder();
			GameMode.GameStyle style = GameMode.Instance.Style;
			if (style != GameMode.GameStyle.SinglPlayer)
			{
				if (style != GameMode.GameStyle.MultiPlayer)
				{
				}
			}
			else
			{
				switch (GameMode.Instance.modePlay)
				{
				case GameMode.ModePlay.Campaign:
				case GameMode.ModePlay.Special_Campaign:
					this.txtWorldMap.text = PopupManager.Instance.GetText(Localization0.Campaign, null);
					if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
					{
						for (int i = 0; i < DataLoader.LevelDataCurrent.points.Count; i++)
						{
							this.TotalEnemy += DataLoader.LevelDataCurrent.points[i].totalEnemy;
						}
					}
					break;
				case GameMode.ModePlay.Boss_Mode:
					this.txtWorldMap.text = PopupManager.Instance.GetText(Localization0.Boss_Mode, null);
					break;
				}
			}
		}

		public void ShowSettingController()
		{
			AudioClick.Instance.OnClick();
			if (this.objPause == null)
			{
				return;
			}
			this.control.Show(delegate
			{
				this.objPause.SetActive(true);
			});
			this.objPause.SetActive(false);
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			
		}

		public void Hide()
		{
			AudioClick.Instance.OnClick();
			base.gameObject.SetActive(false);
		}

		public void Show()
		{
			if (!GameManager.Instance.player.gameObject.activeSelf)
			{
				return;
			}
			this.objPause.SetActive(true);
			try
			{
				ProCamera2D.Instance.OnClearThisScene();
			}
			catch
			{
			}
			if (AutoCheckLevel.isAutoCheck && AutoCheckLevel.typeCheck == 0)
			{
				base.Invoke("StartAutoNextLevel", 1f);
			}
			this.txtTime.text = string.Empty;
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
			}
			if (GameManager.Instance.player != null && GameManager.Instance.player.gameObject.activeSelf)
			{
				GameManager.Instance.player._PlayerInput.IsShooting = false;
				GameManager.Instance.player._PlayerInput.isPressJump = false;
				GameManager.Instance.player._PlayerInput.isPressThrowGrenade = false;
			}
			GameManager.Instance.bombManager.SetPause();
			GameManager.Instance.bulletManager.Pause();
			GameManager.Instance.hudManager.HideControl();
			EnemyManager.Instance.OnPause();
			GameManager.Instance.audioManager.StopAll();
			GameManager.Instance.skillManager.OnPause();
			if (GameManager.Instance.isJetpackMode)
			{
				JetpackManager.Instance.OnPause();
			}
			int level = (int)GameManager.Instance.Level;
			if (this.Levels.Length > 0)
			{
				this.Levels[level].SetActive(true);
			}
			GameMode.GameStyle style = GameMode.Instance.Style;
			if (style != GameMode.GameStyle.SinglPlayer)
			{
				if (style != GameMode.GameStyle.MultiPlayer)
				{
				}
			}
			else
			{
				GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
				if (modePlay != GameMode.ModePlay.Boss_Mode)
				{
					if (modePlay == GameMode.ModePlay.Campaign || modePlay == GameMode.ModePlay.Special_Campaign)
					{
						if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
						{
							this.obj_BossMode.SetActive(false);
							this.obj_Campaign.SetActive(true);
							MissionDataLevel missionDataLevel = null;
							GameMode.Mode mode = GameMode.Instance.MODE;
							if (mode != GameMode.Mode.NORMAL)
							{
								if (mode != GameMode.Mode.HARD)
								{
									if (mode == GameMode.Mode.SUPER_HARD)
									{
										missionDataLevel = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_SuperHard_S[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_SuperHard[(int)GameManager.Instance.Level].missionDataLevel);
									}
								}
								else
								{
									missionDataLevel = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_Hard_S[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_Hard[(int)GameManager.Instance.Level].missionDataLevel);
								}
							}
							else
							{
								missionDataLevel = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_Normal[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_Normal[(int)GameManager.Instance.Level].missionDataLevel);
							}
							for (int i = 0; i < this.campaignResult.Length; i++)
							{
								this.campaignResult[i].txt_Description.text = PopupManager.Instance.GetText((Localization0)missionDataLevel.missionData[i].idDesc, missionDataLevel.missionData[i].valueDesc);
								if (missionDataLevel.missionData[i].IsCompleted)
								{
									this.campaignResult[i].img_Star.sprite = this.sprite_StarOn;
									this.campaignResult[i].obj_Completed.SetActive(true);
									this.campaignResult[i].txt_Gold.gameObject.SetActive(false);
									this.campaignResult[i].txt_Exp.gameObject.SetActive(false);
								}
								else
								{
									this.campaignResult[i].img_Star.sprite = this.sprite_StarOff;
									this.campaignResult[i].obj_Completed.SetActive(false);
									this.campaignResult[i].txt_Gold.text = missionDataLevel.missionData[i].gold_security.Value + string.Empty;
									this.campaignResult[i].txt_Exp.text = ((!GameManager.Instance.isDoubleExp) ? (missionDataLevel.missionData[i].exp_security.Value + string.Empty) : (missionDataLevel.missionData[i].exp_security.Value * 2 + string.Empty));
									this.campaignResult[i].txt_Gold.gameObject.SetActive(true);
									this.campaignResult[i].txt_Exp.gameObject.SetActive(true);
								}
							}
							this.txt_ModePlay.text = string.Concat(new object[]
							{
								PopupManager.Instance.GetText(Localization0.Campaign, null),
								" ",
								(int)((int)GameManager.Instance.Level / (int)ELevel.LEVEL_13 + 1),
								"-",
								(int)((int)GameManager.Instance.Level % (int)ELevel.LEVEL_13 + 1)
							});
							Text text = this.txt_ModePlay;
							text.text += ((missionDataLevel.idBoss < 0) ? " " : string.Concat(new string[]
							{
								" (",
								PopupManager.Instance.GetText(Localization0.Boss, null),
								": ",
								((EBoss)missionDataLevel.idBoss).ToString().Replace('_', ' '),
								")"
							}));
							this.TotalEnemy -= GameManager.Instance.CountEnemyDie;
							GameManager.Instance.CountEnemyDie = 0;
						}
						else
						{
							this.obj_BossMode.SetActive(false);
							this.obj_Campaign.SetActive(false);
							this.txt_ModePlay.text = PopupManager.Instance.GetText(Localization0.Tutorial, null).ToUpper();
						}
					}
				}
				else
				{
					this.obj_BossMode.SetActive(true);
					this.obj_Campaign.SetActive(false);
					this.txt_ModePlay.text = PopupManager.Instance.GetText(Localization0.Boss_Mode, null);
					this.nameBoss.text = ProfileManager.bossCurrent.ToString().Replace("_", " ");
					Sprite x = PopupManager.Instance.spriteAvatarBoss[(int)ProfileManager.bossCurrent];
					if (x != null)
					{
						this.img_Boss.sprite = PopupManager.Instance.spriteAvatarBoss[(int)ProfileManager.bossCurrent];
					}
					else
					{
						this.obj_ImgBoss.SetActive(false);
					}
				}
			}
		}

		private void StartAutoNextLevel()
		{
			this.OnWorldMap();
		}

		public void OnMenu()
		{
			AudioClick.Instance.OnClick();
			GameManager.Instance.hudManager.OnLoadScene("Menu", false);
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				GameMode.Instance.MODE = GameMode.Mode.NORMAL;
			}
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			
			base.gameObject.SetActive(false);
		}

		public void OnContinue()
		{
			if (this._Coroutine != null)
			{
				base.StopCoroutine(this._Coroutine);
			}
			this._Coroutine = base.StartCoroutine(this.IEWaitGameRunning());
			string parameterValue = "GamePlay";
			
		}

		private IEnumerator IEWaitGameRunning()
		{
			this.objPause.SetActive(false);
			this.txtTime.text = "3";
			yield return new WaitForSeconds(1f);
			this.txtTime.text = "2";
			yield return new WaitForSeconds(1f);
			this.txtTime.text = "1";
			GameManager.Instance.hudManager.ShowControl(1.1f);
			yield return new WaitForSeconds(1f);
			this.txtTime.text = string.Empty;
			GameManager.Instance.hudManager.OnResumeGame();
			yield break;
		}

		public void OnWorldMap()
		{
			GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
			if (modePlay != GameMode.ModePlay.Boss_Mode)
			{
				if (modePlay == GameMode.ModePlay.Campaign)
				{
					AutoCheckLevel.LevelCampaign++;
					GameManager.Instance.hudManager.OnLoadScene("UICampaign", false);
				}
			}
			else
			{
				AutoCheckLevel.LevelBossMode++;
				GameManager.Instance.hudManager.OnLoadScene("UIBossMode", false);
			}
		}

		public TextLocalization[] listTextLocalization;

		public GameObject[] Levels;

		public Text txt_ModePlay;

		public int TotalEnemy;

		private StringBuilder strBuilderTime;

		public ControllerSetting control;

		public RectTransform rectTransformPause;

		public GameObject objPause;

		public Text txtTime;

		[Header("---------------Campaign---------------")]
		public Sprite sprite_StarOn;

		public Sprite sprite_StarOff;

		public CardInforResult[] campaignResult;

		[Header("---------------BossMode---------------")]
		public Text nameBoss;

		public Image img_Boss;

		public GameObject obj_ImgBoss;

		public GameObject obj_Campaign;

		public GameObject obj_BossMode;

		private float veloY;

		public Text txtWorldMap;

		private Coroutine _Coroutine;
	}
}
