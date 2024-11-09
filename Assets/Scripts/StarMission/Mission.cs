using System;
using System.Collections;
using System.Collections.Generic;
using PVPManager;
using UnityEngine;

namespace StarMission
{
	public class Mission
	{
		public Mission(MonoBehaviour monoBehaviour)
		{
			this.monoBehaviour = monoBehaviour;
			this.weaponsRigle = new Gun[6];
			this.weaponsSpecial = new Gun[6];
			for (int i = 0; i < 6; i++)
			{
				this.weaponsRigle[i] = new Gun();
				this.weaponsSpecial[i] = new Gun();
			}
			for (int j = 0; j < this.flagsInfor.Length; j++)
			{
				InforFlag inforFlag = new InforFlag();
				this.flagsInfor[j] = inforFlag;
			}
		}

		public float TimeGamePlay
		{
			get
			{
				return this.timeGamePlay;
			}
			set
			{
				this.timeGamePlay = value;
				if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode && (int)this.timeGamePlay != PvP_LocalPlayer.Instance.EnemyTurnTime)
				{
					PvP_LocalPlayer.Instance.EnemyTurnTime = (int)this.timeGamePlay;
				}
			}
		}

		public void StartCheck()
		{
			this.monoBehaviour.StartCoroutine(this.CheckMission());
		}

		private IEnumerator CheckMission()
		{
			switch (GameMode.Instance.modePlay)
			{
			case GameMode.ModePlay.Campaign:
			case GameMode.ModePlay.Special_Campaign:
			{
				MissionDataLevel missionData = null;
				GameMode.Mode mode = GameMode.Instance.MODE;
				if (mode != GameMode.Mode.NORMAL)
				{
					if (mode != GameMode.Mode.HARD)
					{
						if (mode == GameMode.Mode.SUPER_HARD)
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
					missionData = ((GameMode.Instance.modePlay != GameMode.ModePlay.Campaign) ? DataLoader.missionDataRoot_Normal_S[GameManager.Instance.Level - ELevel.LEVEL_S0].missionDataLevel : DataLoader.missionDataRoot_Normal[(int)GameManager.Instance.Level].missionDataLevel);
				}
				List<MissionData> msData = missionData.missionData;
				for (int j = 0; j < this.flagsInfor.Length; j++)
				{
					if (msData[j].IsCompleted)
					{
						this.flagsInfor[j].flag = 2;
					}
				}
				for (int k = 0; k < msData.Count; k++)
				{
					EStarGame type = (EStarGame)msData[k].type;
					string[] array = msData[k].require.Split(new char[]
					{
						'|'
					});
					float num = float.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					switch (type)
					{
					case EStarGame.CLEAR:
						if (GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.COMBO:
						if ((float)this.CountCombo >= num && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.TIME:
						if (this.TimeGamePlay <= num && GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.HP_MAIN:
						if (GameManager.Instance.hudManager.slideHp.value >= num && GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.REVIVE:
						if (!this.isRevive && GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.LAST_KILL_BOSS_BY:
						switch (num2)
						{
						case 2:
							if (this.isLaserBoss && this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
							break;
						case 4:
							if (this.isKnifeBoss && this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security.Value;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
							break;
						}
						break;
					case EStarGame.KILL_ENEMY_BY:
						if (num2 != 4)
						{
							if (num2 != 5)
							{
								if (num2 == -1)
								{
									if ((float)GameManager.Instance.TotalEnemyKilled >= num && this.flagsInfor[k].flag == 0)
									{
										this.flagsInfor[k].flag = 1;
										this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
										this.flagsInfor[k].Coin = msData[k].gold_security.Value;
										this.flagsInfor[k].Ms = msData[k].gem_security.Value;
										this.flagsInfor[k].Exp = msData[k].exp_security.Value;
									}
								}
							}
							else if ((float)this.CountKillByGrenades >= num && this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security.Value;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
						}
						else if ((float)this.KillEnemyByKnife >= num && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.KILL_ENEMY_TRUCK:
						if ((float)this.CountKillEnemyTruck >= num && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.DONT_USE_WEAPON:
						if (num2 != 5)
						{
							if (num2 != 6)
							{
							}
						}
						else if (this.isDontUseGrenades && GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.USE_SKILL_BY:
						if ((float)this.CountUseSkills >= num && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.DONT_USE_SKILL:
						if (this.CountUseSkills <= 0 && GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.DONT_STOP_JETPACK:
						if (!this.isJetpackTouchGround && GameManager.Instance.StateManager.EState == EGamePlay.WIN && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.DESTROY_CH_47:
						if (this.isDestroyCH_47 && this.TimeGamePlay <= num && this.flagsInfor[k].flag == 0)
						{
							this.flagsInfor[k].flag = 1;
							this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
							this.flagsInfor[k].Coin = msData[k].gold_security.Value;
							this.flagsInfor[k].Ms = msData[k].gem_security.Value;
							this.flagsInfor[k].Exp = msData[k].exp_security.Value;
						}
						break;
					case EStarGame.KILL_ENEMY_WITH_GUN:
					{
						int num3 = int.Parse(array[2]);
						if (num3 == 0 && (float)this.weaponsRigle[num2].GetTotalEnemy() >= num)
						{
							if (this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security.Value;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
						}
						else if (num3 == 1 && (float)this.weaponsSpecial[num2].GetTotalEnemy() >= num)
						{
							if (this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security.Value;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
						}
						break;
					}
					case EStarGame.REACH_COMBO_WITH_GUN:
					{
						int num3 = int.Parse(array[2]);
						if (num3 == 0 && (float)this.weaponsRigle[num2].TotalCombo >= num)
						{
							if (this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security.Value;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
						}
						else if (num3 == 1 && (float)this.weaponsSpecial[num2].TotalCombo >= num)
						{
							if (this.flagsInfor[k].flag == 0)
							{
								this.flagsInfor[k].flag = 1;
								this.flagsInfor[k].Content = PopupManager.Instance.GetText((Localization0)msData[k].idDesc, msData[k].valueDesc);
								this.flagsInfor[k].Coin = msData[k].gold_security.Value;
								this.flagsInfor[k].Ms = msData[k].gem_security.Value;
								this.flagsInfor[k].Exp = msData[k].exp_security.Value;
							}
						}
						break;
					}
					}
				}
				for (int i = 0; i < this.flagsInfor.Length; i++)
				{
					if (this.flagsInfor[i].flag == 1)
					{
						if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
						{
							UIShowInforManager.Instance.ShowMission(this.flagsInfor[i].Coin, this.flagsInfor[i].Ms, this.flagsInfor[i].Exp, this.flagsInfor[i].Content);
						}
						this.flagsInfor[i].flag = 2;
						yield return new WaitForSeconds(0.5f);
					}
				}
				break;
			}
			}
			yield break;
		}

		public bool[] Calculator()
		{
			bool[] array = new bool[3];
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
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				return array;
			}
			List<MissionData> missionData = missionDataLevel.missionData;
			for (int i = 0; i < missionData.Count; i++)
			{
				EStarGame type = (EStarGame)missionData[i].type;
				string[] array2 = missionData[i].require.Split(new char[]
				{
					'|'
				});
				float num = float.Parse(array2[0]);
				int num2 = int.Parse(array2[1]);
				switch (type)
				{
				case EStarGame.CLEAR:
					array[i] = true;
					break;
				case EStarGame.COMBO:
					if ((float)this.CountCombo >= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.TIME:
					if (this.TimeGamePlay <= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.HP_MAIN:
					if (GameManager.Instance.hudManager.slideHp.value >= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.REVIVE:
					if (!this.isRevive)
					{
						array[i] = true;
					}
					break;
				case EStarGame.LAST_KILL_BOSS_BY:
					switch (num2)
					{
					case 2:
						if (this.isLaserBoss)
						{
							array[i] = true;
						}
						break;
					case 4:
						if (this.isKnifeBoss)
						{
							array[i] = true;
						}
						break;
					}
					break;
				case EStarGame.KILL_ENEMY_BY:
					if (num2 != 4)
					{
						if (num2 != 5)
						{
							if (num2 == -1)
							{
								if ((float)GameManager.Instance.TotalEnemyKilled >= num)
								{
									array[i] = true;
								}
							}
						}
						else if ((float)this.CountKillByGrenades >= num)
						{
							array[i] = true;
						}
					}
					else if ((float)this.KillEnemyByKnife >= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.KILL_ENEMY_TRUCK:
					if ((float)this.CountKillEnemyTruck >= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.DONT_USE_WEAPON:
					if (num2 != 5)
					{
						if (num2 != 6)
						{
						}
					}
					else if (this.isDontUseGrenades)
					{
						array[i] = true;
					}
					break;
				case EStarGame.USE_SKILL_BY:
					if ((float)this.CountUseSkills >= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.DONT_USE_SKILL:
					if (this.CountUseSkills <= 0)
					{
						array[i] = true;
					}
					break;
				case EStarGame.DONT_STOP_JETPACK:
					if (!this.isJetpackTouchGround)
					{
						array[i] = true;
					}
					break;
				case EStarGame.DESTROY_CH_47:
					if (this.isDestroyCH_47 && this.TimeGamePlay <= num)
					{
						array[i] = true;
					}
					break;
				case EStarGame.KILL_ENEMY_WITH_GUN:
				{
					int num3 = int.Parse(array2[2]);
					if (num3 == 0 && (float)this.weaponsRigle[num2].GetTotalEnemy() >= num)
					{
						array[i] = true;
					}
					else if (num3 == 1 && (float)this.weaponsSpecial[num2].GetTotalEnemy() >= num)
					{
						array[i] = true;
					}
					break;
				}
				case EStarGame.REACH_COMBO_WITH_GUN:
				{
					int num3 = int.Parse(array2[2]);
					if (num3 == 0 && (float)this.weaponsRigle[num2].TotalCombo >= num)
					{
						array[i] = true;
					}
					else if (num3 == 1 && (float)this.weaponsSpecial[num2].TotalCombo >= num)
					{
						array[i] = true;
					}
					break;
				}
				}
			}
			return array;
		}

		public void AddEnemyToGun(int idEnemy)
		{
			if (GameManager.Instance.player == null)
			{
				UnityEngine.Debug.Log("Rambo Null");
				return;
			}
			if (GameManager.Instance.player.isGunDefault)
			{
				this.count++;
				UnityEngine.Debug.Log(this.count);
				this.weaponsRigle[GameManager.Instance.player._PlayerData.IDGUN1].AddEnemy(idEnemy);
			}
			else
			{
				this.weaponsSpecial[GameManager.Instance.player._PlayerData.IDGUN2].AddEnemy(idEnemy);
			}
		}

		public void AddComboToGun()
		{
			if (GameManager.Instance.player == null)
			{
				UnityEngine.Debug.Log("Rambo Null");
				return;
			}
			if (GameManager.Instance.player.isGunDefault)
			{
				this.weaponsRigle[GameManager.Instance.player._PlayerData.IDGUN1].TotalCombo++;
			}
			else
			{
				this.weaponsSpecial[GameManager.Instance.player._PlayerData.IDGUN2].TotalCombo++;
			}
		}

		public int CountCombo;

		public bool isKnifeBoss;

		public bool isLaserBoss;

		public bool isRevive;

		private float timeGamePlay;

		public int CountKillByGrenades;

		public int KillEnemyByKnife;

		public int CountKillEnemyTruck;

		public bool isDontUseGrenades = true;

		public int CountUseSkills;

		public bool isJetpackTouchGround;

		public bool isDestroyCH_47;

		public Gun[] weaponsRigle;

		public Gun[] weaponsSpecial;

		private InforFlag[] flagsInfor = new InforFlag[3];

		private MonoBehaviour monoBehaviour;

		private int count;
	}
}
