using System;
using System.Collections.Generic;
using DailyQuest;
using DailyQuest.Booster;
using DailyQuest.Boss;
using DailyQuest.Combo;
using DailyQuest.Enemy;
using DailyQuest.Level;
using DailyQuest.Transfer;
using DailyQuest.UpgradeWeapon;
using Newtonsoft.Json;
using UnityEngine;

public class DailyQuestManager
{
	public static DailyQuestManager Instance
	{
		get
		{
			if (DailyQuestManager.instance == null)
			{
				DailyQuestManager.instance = new DailyQuestManager();
			}
			return DailyQuestManager.instance;
		}
	}

	public void GetData()
	{
		if (this.isTest)
		{
			return;
		}
		int dayOfYear = DateTime.Now.DayOfYear;
		this.DailyQuests = new DailyQuestType1[this.maxQuest];
		this.IdMissionProfile = new IntProfileData[this.maxQuest];
		bool flag = false;
		for (int i = 0; i < this.MAX; i++)
		{
			this.listID.Add(i);
		}
		for (int j = 0; j < this.maxQuest; j++)
		{
			this.IdMissionProfile[j] = new IntProfileData("com.sora.metal.squad.DailyQuestManager.dailyquest.mission" + j, 0);
		}
		this.DayProfile = new IntProfileData("com.sora.metal.squad.DailyQuestManager.dailyquest3", -1);
		if (dayOfYear != this.DayProfile.Data.Value)
		{
			flag = true;
			this.DayProfile.setValue(dayOfYear);
			this.listID.Clear();
			for (int k = 0; k < this.MAX; k++)
			{
				this.listID.Add(k);
			}
			this.IdMissionProfile[0].setValue(this.GetRandomID1());
			this.IdMissionProfile[1].setValue(this.GetRandomID2());
			for (int l = 2; l < this.IdMissionProfile.Length; l++)
			{
				this.IdMissionProfile[l].setValue(this.GetRandomID());
			}
		}
		string text = string.Empty;
		for (int m = 0; m < this.maxQuest; m++)
		{
			string path = string.Concat(new object[]
			{
				"DailyQuest/",
				(!SplashScreen._isLoadResourceDecrypt) ? "Encrypt" : "Decrypt",
				"/Mission",
				this.IdMissionProfile[m].Data.Value
			});
			text = Resources.Load<TextAsset>(path).text;
			string text2 = ProfileManager.DataEncryption.decrypt2(text);
			this.DailyQuests[m] = JsonConvert.DeserializeObject<DailyQuestType1>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		}
		for (int n = 0; n < this.maxQuest; n++)
		{
			this.DailyQuests[n].LoadProfile(n, dayOfYear);
		}
		if (flag)
		{
			for (int num = 0; num < this.maxQuest; num++)
			{
				this.DailyQuests[num].Reset();
			}
		}
	}

	private int GetRandomID1()
	{
		int num = UnityEngine.Random.Range(0, 2);
		this.listID.RemoveAt(num);
		return num;
	}

	private int GetRandomID2()
	{
		int num = UnityEngine.Random.Range(3, 5);
		this.listID.RemoveAt(num);
		return num;
	}

	private int GetRandomID()
	{
		int index = UnityEngine.Random.Range(0, this.listID.Count - 1);
		int result = this.listID[index];
		this.listID.RemoveAt(index);
		return result;
	}

	public EDailyQuest[] GetTypeMission()
	{
		EDailyQuest[] array = new EDailyQuest[this.maxQuest];
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest type = (EDailyQuest)this.DailyQuests[i].Type;
			array[i] = type;
		}
		return array;
	}

	public int[] GetTotal(int id)
	{
		int[] array = new int[2];
		array[0] = -1;
		switch (this.GetTypeMission()[id])
		{
		case EDailyQuest.ENEMY:
		{
			EnemyDailyQuest enemy = this.DailyQuests[id].Mission.Enemy;
			if (enemy != null)
			{
				array[0] = enemy.Total;
			}
			break;
		}
		case EDailyQuest.BOOSTER:
		{
			BoosterDailyQuest missionBooster = this.DailyQuests[id].Mission.MissionBooster;
			if (missionBooster != null)
			{
				array[0] = missionBooster.Total;
			}
			break;
		}
		case EDailyQuest.BOSS:
		{
			BossDailyQuest missionBoss = this.DailyQuests[id].Mission.MissionBoss;
			if (missionBoss != null)
			{
				array[0] = missionBoss.Total;
			}
			break;
		}
		case EDailyQuest.LEVEL:
		{
			LevelDailyQuest missionLevel = this.DailyQuests[id].Mission.MissionLevel;
			if (missionLevel.Map != null)
			{
				if (missionLevel.Map.UnlockMap)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = ((missionLevel.Map.TotalStar <= 0) ? missionLevel.Map.TotalLevel : 1);
				}
			}
			else
			{
				array[0] = 1;
			}
			break;
		}
		case EDailyQuest.WEAPON_SHOP:
		{
			UpgradeWeaponDailyQuest missionUpgradeWeapon = this.DailyQuests[id].Mission.MissionUpgradeWeapon;
			if (missionUpgradeWeapon.Upgrade != null)
			{
				array[0] = missionUpgradeWeapon.Upgrade.Total;
			}
			else
			{
				array[0] = 1;
			}
			break;
		}
		case EDailyQuest.COMBO:
		{
			ComboDailyQuest missionCombo = this.DailyQuests[id].Mission.MissionCombo;
			if (missionCombo != null)
			{
				array[0] = missionCombo.Total;
			}
			break;
		}
		case EDailyQuest.TRANSFER:
		{
			TransferDailyQuest missionTransfer = this.DailyQuests[id].Mission.MissionTransfer;
			if (missionTransfer != null)
			{
				array[0] = missionTransfer.Total;
			}
			break;
		}
		case EDailyQuest.CHARACTER_SHOP:
		{
			CharacterShop characterShop = this.DailyQuests[id].Mission.characterShop;
			if (characterShop != null)
			{
				array[0] = characterShop.Total;
			}
			break;
		}
		case EDailyQuest.PVP_FINISH:
			array[0] = this.DailyQuests[id].Mission.TotalRequirePvPFinish;
			break;
		case EDailyQuest.LUCKY_SPIN:
			array[0] = this.DailyQuests[id].Mission.TotalRequireLuckySpin;
			break;
		case EDailyQuest.LUCKY_GACHA:
			array[0] = this.DailyQuests[id].Mission.TotalRequireGacha;
			break;
		case EDailyQuest.PVP_WIN:
			array[0] = this.DailyQuests[id].Mission.TotalRequirePvPWin;
			break;
		case EDailyQuest.BOSS_MODE_FINISH:
			array[0] = this.DailyQuests[id].Mission.TotalRequireBossModeFinish;
			break;
		case EDailyQuest.BOSS_MODE_WIN:
			array[0] = this.DailyQuests[id].Mission.TotalRequireBossModeWin;
			break;
		}
		array[1] = this.DailyQuests[id].Total;
		return array;
	}

	public void Transfer(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.TRANSFER)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionTransfer.Total);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
			}
		}
	}

	public void MissionCharacterShop(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.CHARACTER_SHOP)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.characterShop.Total);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionWeaponShop(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.WEAPON_SHOP)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionUpgradeWeapon.Upgrade.Total);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionLuckySpin(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.LUCKY_SPIN)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.TotalRequireLuckySpin);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionGacha(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.LUCKY_GACHA)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.TotalRequireGacha);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionFinishPvP(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.PVP_FINISH)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.TotalRequirePvPFinish);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionWinPvP(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.PVP_WIN)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.TotalRequirePvPWin);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionBossModeFinish(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.BOSS_MODE_FINISH)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.TotalRequireBossModeFinish);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void MissionBossModeWin(Action<bool, DailyQuestManager.InforQuest> onCompleted = null)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.BOSS_MODE_WIN)
			{
				this.DailyQuests[i].Total++;
				bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.TotalRequireBossModeWin);
				inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
				inforQuest.Type = this.DailyQuests[i].Type;
				inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
				inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
				if (onCompleted != null)
				{
					onCompleted(arg, inforQuest);
				}
				return;
			}
		}
	}

	public void Combo(int maxCombo, int type, int id, Action<bool, DailyQuestManager.InforQuest> onCompleted)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.COMBO)
			{
				bool flag = this.DailyQuests[i].Mission.MissionCombo.TypeGun == -1 || this.DailyQuests[i].Mission.MissionCombo.TypeGun == type;
				bool flag2 = this.DailyQuests[i].Mission.MissionCombo.IDGun == -1 || this.DailyQuests[i].Mission.MissionCombo.IDGun == id;
				if (flag && flag2)
				{
					if (maxCombo > this.DailyQuests[i].Total)
					{
						this.DailyQuests[i].Total = maxCombo;
					}
					if (maxCombo >= this.DailyQuests[i].Mission.MissionCombo.Total)
					{
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
						inforQuest.Type = this.DailyQuests[i].Type;
						inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
						inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
						bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionCombo.Total);
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
				}
			}
		}
	}

	public void Level(int map, int level, int totalStar, bool isRevive, bool isUnLocked, bool isNewLevel, Action<bool, DailyQuestManager.InforQuest> onCompleted)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			bool[] array = new bool[]
			{
				GameMode.Instance.MODE == GameMode.Mode.NORMAL,
				GameMode.Instance.MODE == GameMode.Mode.HARD,
				GameMode.Instance.MODE == GameMode.Mode.SUPER_HARD
			};
			this.DailyQuests[i].SetMode(array);
			int mode = this.DailyQuests[i].Mission.Mode;
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.LEVEL)
			{
				if (this.DailyQuests[i].Mission.MissionLevel != null && (this.DailyQuests[i].Mission.Mode == -1 || array[mode]) && (this.DailyQuests[i].Mission.MissionLevel.Revive == null || this.DailyQuests[i].Mission.MissionLevel.Revive.Revive == isRevive))
				{
					if (this.DailyQuests[i].Mission.MissionLevel.Map == null)
					{
						this.DailyQuests[i].Total++;
						bool arg = this.CheckMission(i, 1);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
						inforQuest.Type = this.DailyQuests[i].Type;
						inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
						inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
					else if (this.DailyQuests[i].Mission.MissionLevel.Map.UnlockMap && isUnLocked)
					{
						this.DailyQuests[i].Total++;
						bool arg = this.CheckMission(i, 1);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
						inforQuest.Type = this.DailyQuests[i].Type;
						inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
						inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
					else if (this.DailyQuests[i].Mission.MissionLevel.Map.ID == map || this.DailyQuests[i].Mission.MissionLevel.Map.ID == -1)
					{
						if (this.DailyQuests[i].Mission.MissionLevel.Map.TotalStar == -1 && this.DailyQuests[i].Mission.MissionLevel.Map.NewLevel == isNewLevel)
						{
							this.DailyQuests[i].Total++;
							bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionLevel.Map.TotalLevel);
							inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
							inforQuest.Type = this.DailyQuests[i].Type;
							inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
							inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
							if (onCompleted != null)
							{
								onCompleted(arg, inforQuest);
							}
						}
						else if (this.DailyQuests[i].Mission.MissionLevel.Map.TotalStar != -1 && isNewLevel && totalStar >= this.DailyQuests[i].Mission.MissionLevel.Map.TotalStar)
						{
							this.DailyQuests[i].Total++;
							bool arg = this.CheckMission(i, 1);
							inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
							inforQuest.Type = this.DailyQuests[i].Type;
							inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
							inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
							if (onCompleted != null)
							{
								onCompleted(arg, inforQuest);
							}
						}
					}
				}
			}
		}
	}

	public void MissionBoss(int map, int id, Action<bool, DailyQuestManager.InforQuest> onCompleted)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			bool[] array = new bool[]
			{
				GameMode.Instance.MODE == GameMode.Mode.NORMAL,
				GameMode.Instance.MODE == GameMode.Mode.HARD,
				GameMode.Instance.MODE == GameMode.Mode.SUPER_HARD
			};
			this.DailyQuests[i].SetMode(array);
			int mode = this.DailyQuests[i].Mission.Mode;
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.BOSS)
			{
				if (this.DailyQuests[i].Mission.MissionBoss != null && (this.DailyQuests[i].Mission.Mode == -1 || array[mode]))
				{
					if (this.DailyQuests[i].Mission.MissionBoss.Map == null)
					{
						this.DailyQuests[i].Total++;
						bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionBoss.Total);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
						inforQuest.Type = this.DailyQuests[i].Type;
						inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
						inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
					else if (this.DailyQuests[i].Mission.MissionBoss.Map.ID == map || this.DailyQuests[i].Mission.MissionBoss.Map.ID == -1)
					{
						if (this.DailyQuests[i].Mission.MissionBoss.Map.FlagAny)
						{
							this.DailyQuests[i].Total++;
							bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionBoss.Total);
							inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
							inforQuest.Type = this.DailyQuests[i].Type;
							inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
							inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
							if (onCompleted != null)
							{
								onCompleted(arg, inforQuest);
							}
						}
						else if (this.DailyQuests[i].Mission.MissionBoss.Map.IDLevel[id] && !this.DailyQuests[i].Mission.MissionBoss.Map.GetIDlevelProfile(i))
						{
							this.DailyQuests[i].Total++;
							this.DailyQuests[i].Mission.MissionBoss.Map.IDLevel[id] = false;
							this.DailyQuests[i].Mission.MissionBoss.Map.SetIDLevelProfile(i, true);
							bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionBoss.Total);
							inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
							inforQuest.Type = this.DailyQuests[i].Type;
							inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
							inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
							if (onCompleted != null)
							{
								onCompleted(arg, inforQuest);
							}
						}
					}
				}
			}
		}
	}

	public void MisionBooster(EBooster booster, Action<bool, DailyQuestManager.InforQuest> onCompleted)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			bool[] array = new bool[]
			{
				GameMode.Instance.MODE == GameMode.Mode.NORMAL,
				GameMode.Instance.MODE == GameMode.Mode.HARD,
				GameMode.Instance.MODE == GameMode.Mode.SUPER_HARD
			};
			this.DailyQuests[i].SetMode(array);
			int mode = this.DailyQuests[i].Mission.Mode;
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.BOOSTER)
			{
				if (this.DailyQuests[i].Mission.MissionBooster != null && this.DailyQuests[i].Mission.MissionBooster.ID == (int)booster && (mode == -1 || array[mode]))
				{
					this.DailyQuests[i].Total++;
					bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.MissionBooster.Total);
					inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
					inforQuest.Type = this.DailyQuests[i].Type;
					inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
					inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
					if (onCompleted != null)
					{
						onCompleted(arg, inforQuest);
					}
				}
			}
		}
	}

	public void MissionEnemy(int idEnemy, int idWeapon, int typeWeapon, Action<bool, DailyQuestManager.InforQuest> onCompleted)
	{
		if (this.isTest)
		{
			return;
		}
		DailyQuestManager.InforQuest inforQuest = new DailyQuestManager.InforQuest();
		for (int i = 0; i < this.maxQuest; i++)
		{
			bool[] array = new bool[]
			{
				GameMode.Instance.MODE == GameMode.Mode.NORMAL,
				GameMode.Instance.MODE == GameMode.Mode.HARD,
				GameMode.Instance.MODE == GameMode.Mode.SUPER_HARD
			};
			this.DailyQuests[i].SetMode(array);
			int mode = this.DailyQuests[i].Mission.Mode;
			EDailyQuest edailyQuest = this.GetTypeMission()[i];
			if (edailyQuest == EDailyQuest.ENEMY)
			{
				if (this.DailyQuests[i].Mission.Enemy != null && (this.DailyQuests[i].Mission.Enemy.ID == -1 || this.DailyQuests[i].Mission.Enemy.ID == idEnemy))
				{
					if ((this.DailyQuests[i].Mission.Weapon == null || this.DailyQuests[i].Mission.Weapon.ID == -1) && (mode == -1 || array[mode]))
					{
						this.DailyQuests[i].Total++;
						bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.Enemy.Total);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
						inforQuest.Type = this.DailyQuests[i].Type;
						inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
						inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
					else if (this.DailyQuests[i].Mission.Weapon != null && this.DailyQuests[i].Mission.Weapon.ID == idWeapon && (this.DailyQuests[i].Mission.Weapon.Type == -1 || this.DailyQuests[i].Mission.Weapon.Type == typeWeapon) && (mode == -1 || array[mode]))
					{
						this.DailyQuests[i].Total++;
						bool arg = this.CheckMission(i, this.DailyQuests[i].Mission.Enemy.Total);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)this.DailyQuests[i].IdDesc, this.DailyQuests[i].ValueDesc);
						inforQuest.Type = this.DailyQuests[i].Type;
						inforQuest.amount = Mathf.Max(this.DailyQuests[i].Gold, this.DailyQuests[i].Gem);
						inforQuest.item = ((this.DailyQuests[i].Gold <= 0) ? Item.Gem : Item.Gold);
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
				}
			}
		}
	}

	private bool CheckMission(int index, int TotalRequire)
	{
		bool result = false;
		try
		{
			if (this.DailyQuests[index].Total >= TotalRequire && this.DailyQuests[index].State == 0)
			{
				result = true;
				ProfileManager.userProfile.TotalCompletedDailyQuest++;
				this.DailyQuests[index].State = 1;
				
			}
		}
		catch
		{
		}
		return result;
	}

	public int CheckDailyQuest()
	{
		int result = -1;
		for (int i = 0; i < this.DailyQuests.Length; i++)
		{
			if (this.DailyQuests[i].State == 1)
			{
				result = i;
			}
		}
		return result;
	}

	private static DailyQuestManager instance;

	private readonly int MAX = 31;

	private int maxQuest = 8;

	public DailyQuestType1[] DailyQuests;

	private IntProfileData DayProfile;

	public IntProfileData[] IdMissionProfile;

	private bool isTest;

	private List<int> listID = new List<int>();

	public class InforQuest
	{
		public int Type { get; set; }

		public string Desc { get; set; }

		public int amount { get; set; }

		public Item item { get; set; }
	}
}
