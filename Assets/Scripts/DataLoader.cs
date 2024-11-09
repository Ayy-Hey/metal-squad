using System;
using Achievement;
using com.dev.util.SecurityHelper;
using LitJson;
using MyDataLoader;
using Newtonsoft.Json;
using Rank;
using StarMission;
using StarterPack;
using UnityEngine;

public class DataLoader
{
	public static void LoadData()
	{
		if (!DataLoader.isLoaded)
		{
			DataLoader.tanks = new EnemyCharactor[3];
			DataLoader.maybay = new EnemyCharactor[4];
			DataLoader.missionDataRoot_Normal = new MissionDataRoot[60];
			DataLoader.missionDataRoot_Hard = new MissionDataRoot[60];
			DataLoader.missionDataRoot_SuperHard = new MissionDataRoot[60];
			DataLoader.missionDataRoot_Normal_S = new MissionDataRoot[0];
			DataLoader.missionDataRoot_Hard_S = new MissionDataRoot[0];
			DataLoader.missionDataRoot_SuperHard_S = new MissionDataRoot[0];
			DataLoader.ramboDataLoader = new RamboDataLoader[3];
			DataLoader.weaponDataLoader1 = new WeaponDataLoader[6];
			DataLoader.weaponDataLoader2 = new WeaponDataLoader[6];
			DataLoader.grenadeDataLoader = new GrenadeDataLoader[4];
			DataLoader.knifeDataLoader = new KnifeDataLoader[3];
			DataLoader.rankInfor = new RankInfor[19];
			DataLoader.achievement = new AchievementData[29];
			DataLoader.vipData = new VipData();
			DataLoader.LoadJsonData();
			DataLoader.isLoaded = true;
		}
	}

	public static void ReloadProfileAchievement()
	{
		for (int i = 0; i < DataLoader.achievement.Length; i++)
		{
			DataLoader.achievement[i].LoadProfile();
		}
	}

	public static void LoadGacha()
	{
		string text = Resources.Load((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathGUIGacha : SPaths.PathGUIGacha_Decrypt).ToString();
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.dataGacha = JsonMapper.ToObject((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
	}

	public static void LoadLocalization()
	{
		DataLoader.dataLocalization = JsonMapper.ToObject(Resources.Load("GUI/Localization").ToString());
	}

	public static void LoadPackInApp()
	{
		string text = Resources.Load((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathGUIPackInapp : SPaths.PathGUIPackInapp_Decrypt).ToString();
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.dataPackInApp = JsonMapper.ToObject((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
	}

	public static void LoadCharacterData()
	{
		string text = Resources.Load((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathGUIUpgradeCharacter : SPaths.PathGUIUpgradeCharacter_Decrypt).ToString();
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.characterData = new global::CharacterData[ProfileManager.CHARACTER_MAX];
		JsonData jsonData = JsonMapper.ToObject((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		for (int i = 0; i < ProfileManager.CHARACTER_MAX; i++)
		{
			DataLoader.characterData[i] = new global::CharacterData();
			for (int j = 0; j < ProfileManager.CHARACTER_MAX_SKILL; j++)
			{
				JsonData jsonData2 = jsonData[i]["Skill"][j];
				DataLoader.characterData[i].skills[j].idName = jsonData2["idName"].ToInt();
				DataLoader.characterData[i].skills[j].isActive = jsonData2["isActive"].ToBool();
				if (jsonData2.ContainsKey("idDesc"))
				{
					DataLoader.characterData[i].skills[j].idDesc = jsonData2["idDesc"].ToInt();
				}
				DataLoader.characterData[i].skills[j].HP = ((!jsonData2.ContainsKey("HP")) ? null : new int[ProfileManager.UPGRADE_MAX_SKILL]);
				DataLoader.characterData[i].skills[j].ShellPerShot = ((!jsonData2.ContainsKey("ShellPerShot")) ? null : new int[ProfileManager.UPGRADE_MAX_SKILL]);
				DataLoader.characterData[i].skills[j].Damage = ((!jsonData2.ContainsKey("Damage")) ? null : new float[ProfileManager.UPGRADE_MAX_SKILL]);
				DataLoader.characterData[i].skills[j].TimeReload = ((!jsonData2.ContainsKey("TimeReload")) ? null : new float[ProfileManager.UPGRADE_MAX_SKILL]);
				DataLoader.characterData[i].skills[j].ActiveDuration = ((!jsonData2.ContainsKey("ActiveDuration")) ? null : new float[ProfileManager.UPGRADE_MAX_SKILL]);
				DataLoader.characterData[i].skills[j].Cooldown = ((!jsonData2.ContainsKey("Cooldown")) ? null : new float[ProfileManager.UPGRADE_MAX_SKILL]);
				DataLoader.characterData[i].skills[j].ShootingRound = ((!jsonData2.ContainsKey("ShootingRound")) ? null : new int[ProfileManager.UPGRADE_MAX_SKILL]);
				for (int k = 0; k < ProfileManager.UPGRADE_MAX_SKILL; k++)
				{
					if (DataLoader.characterData[i].skills[j].HP != null)
					{
						DataLoader.characterData[i].skills[j].HP[k] = jsonData2["HP"][k].ToInt();
					}
					if (DataLoader.characterData[i].skills[j].ShellPerShot != null)
					{
						DataLoader.characterData[i].skills[j].ShellPerShot[k] = jsonData2["ShellPerShot"][k].ToInt();
					}
					if (DataLoader.characterData[i].skills[j].Damage != null)
					{
						DataLoader.characterData[i].skills[j].Damage[k] = jsonData2["Damage"][k].ToFloat();
					}
					if (DataLoader.characterData[i].skills[j].TimeReload != null)
					{
						DataLoader.characterData[i].skills[j].TimeReload[k] = jsonData2["TimeReload"][k].ToFloat();
					}
					if (DataLoader.characterData[i].skills[j].ActiveDuration != null)
					{
						DataLoader.characterData[i].skills[j].ActiveDuration[k] = jsonData2["ActiveDuration"][k].ToFloat();
					}
					if (DataLoader.characterData[i].skills[j].Cooldown != null)
					{
						DataLoader.characterData[i].skills[j].Cooldown[k] = jsonData2["Cooldown"][k].ToFloat();
					}
					if (DataLoader.characterData[i].skills[j].ShootingRound != null)
					{
						DataLoader.characterData[i].skills[j].ShootingRound[k] = jsonData2["ShootingRound"][k].ToInt();
					}
				}
			}
		}
	}

	public static void LoadStarGift()
	{
		if (DataLoader.starGift != null)
		{
			return;
		}
		string text = Resources.Load((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathGUIPackStarGift : SPaths.PathGUIPackStarGift_Decrypt).ToString();
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.starGift = new StarGift();
		DataLoader.starGift = JsonConvert.DeserializeObject<StarGift>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
	}

	public static void LoadLevelJsonData(string path)
	{
		string text = string.Empty;
		DataLoader.LevelDataCurrent = null;
		text = Resources.Load<TextAsset>(path).text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.LevelDataCurrent = JsonConvert.DeserializeObject<LevelData>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		for (int i = 0; i < DataLoader.LevelDataCurrent.points.Count; i++)
		{
			DataLoader.LevelDataCurrent.points[i].totalEnemy = DataLoader.LevelDataCurrent.points[i].enemyData.enemyDataInfo.Count;
		}
	}

	public static void LoadStarterPack()
	{
		string text = string.Empty;
		DataLoader.StarterPackData = new PackInforData[3];
		for (int i = 0; i < DataLoader.StarterPackData.Length; i++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathStarterPack : SPaths.PathStarterPack_Decrypt) + "Pack" + i).text;
			string text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.StarterPackData[i] = JsonConvert.DeserializeObject<PackInforData>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		}
	}

	public static void LoadDataEndlessMode()
	{
		if (DataLoader.isLoadDataEndlessMode)
		{
			return;
		}
		DataLoader.isLoadDataEndlessMode = true;
		string encrypted = string.Empty;
		string text = string.Empty;
		encrypted = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathMapEndless : SPaths.PathMapEndless_Decrypt) + "Endless").text;
		text = ProfileManager.DataEncryption.decrypt2(encrypted);
	}

	public static void LoadDataCampaign()
	{
		if (DataLoader.isLoadDataCampaign)
		{
			return;
		}
		DataLoader.isLoadDataCampaign = true;
		string text = string.Empty;
		string text2 = string.Empty;
		text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathSkillEYEBOT : SPaths.PathSkillEYEBOT_Decrypt) + "EyeBot").text;
        text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.eyeBotDataLoader = JsonConvert.DeserializeObject<EyeBotDataLoader>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		ProfileManager.eyeBotProfile.Damages = DataLoader.eyeBotDataLoader.Damages;
		ProfileManager.eyeBotProfile.HPs = DataLoader.eyeBotDataLoader.HPs;
		ProfileManager.eyeBotProfile.Max_Bullets = DataLoader.eyeBotDataLoader.Max_Bullets;
		ProfileManager.eyeBotProfile.Speed_Bullets = DataLoader.eyeBotDataLoader.Speed_Bullets;
		ProfileManager.eyeBotProfile.Time_Delays = DataLoader.eyeBotDataLoader.Time_Delays;
		text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathBossDefault : SPaths.PathBossDefault_Decrypt) + "Boss").text;
		text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.boss = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		for (int i = 0; i < 3; i++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathEnemyTank : SPaths.PathEnemyTank_Decrypt) + "Tank" + i).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.tanks[i] = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		}
		for (int j = 0; j < DataLoader.maybay.Length; j++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathMaybay : SPaths.PathMaybay_Decrypt) + "Maybay" + j).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.maybay[j] = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		}
		for (int k = 0; k < DataLoader.missionDataRoot_Normal.Length; k++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathMissionNormal : SPaths.PathMissionNormal_Decrypt) + "Level" + k).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.missionDataRoot_Normal[k] = JsonConvert.DeserializeObject<MissionDataRoot>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			DataLoader.missionDataRoot_Normal[k].missionDataLevel.InitData(k, string.Empty);
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathMissionHard : SPaths.PathMissionHard_Decrypt) + "Level" + k).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.missionDataRoot_Hard[k] = JsonConvert.DeserializeObject<MissionDataRoot>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			DataLoader.missionDataRoot_Hard[k].missionDataLevel.InitData(k, "hard");
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathMissionCrazy : SPaths.PathMissionCrazy_Decrypt) + "Level" + k).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.missionDataRoot_SuperHard[k] = JsonConvert.DeserializeObject<MissionDataRoot>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			DataLoader.missionDataRoot_SuperHard[k].missionDataLevel.InitData(k, "super_hard");
		}
		for (int l = 0; l < 0; l++)
		{
			text = Resources.Load<TextAsset>(SPaths.MISSION_DATA_NORMAL_S + l).text;
			DataLoader.missionDataRoot_Normal_S[l] = JsonConvert.DeserializeObject<MissionDataRoot>(text);
			DataLoader.missionDataRoot_Normal_S[l].missionDataLevel.InitData(l, "nomarl_s");
			text = Resources.Load<TextAsset>(SPaths.MISSION_DATA_HARD_S + l).text;
			DataLoader.missionDataRoot_Hard_S[l] = JsonConvert.DeserializeObject<MissionDataRoot>(text);
			DataLoader.missionDataRoot_Hard_S[l].missionDataLevel.InitData(l, "hard_s");
			text = Resources.Load<TextAsset>(SPaths.MISSION_DATA_SUPER_HARD_S + l).text;
			DataLoader.missionDataRoot_SuperHard_S[l] = JsonConvert.DeserializeObject<MissionDataRoot>(text);
			DataLoader.missionDataRoot_SuperHard_S[l].missionDataLevel.InitData(l, "super_hard_s");
		}
	}

	public static void LoadDataBossMode()
	{
		if (DataLoader.isLoadBossModeData)
		{
			return;
		}
		DataLoader.isLoadBossModeData = true;
		string text = Resources.Load<TextAsset>((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathGUIBossmode : SPaths.PathGUIBossmode_Decrypt).ToString();
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		JsonData jsonData = JsonMapper.ToObject((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		DataLoader.bossModeData = JsonConvert.DeserializeObject<BossModeData>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
	}

	public static void LoadJsonData()
	{
		DataLoader.LoadLocalization();
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < DataLoader.achievement.Length; i++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathAchievement : SPaths.PathAchievement_Decrypt) + "Achievement" + i).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.achievement[i] = JsonConvert.DeserializeObject<AchievementData>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			DataLoader.achievement[i].LoadProfile(i);
		}
		for (int j = 0; j < DataLoader.rankInfor.Length; j++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathRank : SPaths.PathRank_Decrypt) + "Rank" + j).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.rankInfor[j] = JsonConvert.DeserializeObject<RankInfor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			DataLoader.rankInfor[j].LoadProfile(j);
		}
		for (int k = 0; k < DataLoader.knifeDataLoader.Length; k++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathWeaponKnife : SPaths.PathWeaponKnife_Decrypt) + "Knife" + k).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.knifeDataLoader[k] = JsonConvert.DeserializeObject<KnifeDataLoader>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			ProfileManager.melesProfile[k].NAME = PopupManager.Instance.GetText((Localization0)DataLoader.knifeDataLoader[k].IdName, null);
			ProfileManager.melesProfile[k].RankBase = DataLoader.knifeDataLoader[k].RankBase;
			ProfileManager.melesProfile[k].SecuredDamaged = new SecuredFloat[DataLoader.knifeDataLoader[k].DAMAGED.Length];
			for (int l = 0; l < ProfileManager.melesProfile[k].SecuredDamaged.Length; l++)
			{
				ProfileManager.melesProfile[k].SecuredDamaged[l] = new SecuredFloat(DataLoader.knifeDataLoader[k].DAMAGED[l]);
			}
			ProfileManager.melesProfile[k].PriceUpgrade = DataLoader.knifeDataLoader[k].CoinUpgrade;
		}
		for (int m = 0; m < DataLoader.grenadeDataLoader.Length; m++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathWeaponGrenade : SPaths.PathWeaponGrenade_Decrypt) + "Grenade" + m).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.grenadeDataLoader[m] = JsonConvert.DeserializeObject<GrenadeDataLoader>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			ProfileManager.grenadesProfile[m].options[0] = DataLoader.grenadeDataLoader[m].Damage;
			ProfileManager.grenadesProfile[m].options[1] = DataLoader.grenadeDataLoader[m].Damage_Range;
			ProfileManager.grenadesProfile[m].options[2] = DataLoader.grenadeDataLoader[m].Effect_Time;
			ProfileManager.grenadesProfile[m].isVip = DataLoader.grenadeDataLoader[m].Vip;
			ProfileManager.grenadesProfile[m].RankBase = DataLoader.grenadeDataLoader[m].RankBase;
			ProfileManager.grenadesProfile[m].NAME_GRENADE = PopupManager.Instance.GetText((Localization0)DataLoader.grenadeDataLoader[m].IdName, null);
			ProfileManager.grenadesProfile[m].PriceUpgrade = DataLoader.grenadeDataLoader[m].CoinUpgrade;
			ProfileManager.grenadesProfile[m].SecuredPrice = new SecuredInt(DataLoader.grenadeDataLoader[m].Price);
		}
		for (int n = 0; n < DataLoader.weaponDataLoader1.Length; n++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathWeaponWeapon1 : SPaths.PathWeaponWeapon1_Decrypt) + "Weapon" + n).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.weaponDataLoader1[n] = JsonConvert.DeserializeObject<WeaponDataLoader>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			ProfileManager.weaponsRifle[n].Gun_Value.gunName = PopupManager.Instance.GetText((Localization0)DataLoader.weaponDataLoader1[n].IdName, null);
			ProfileManager.weaponsRifle[n].Gun_Value.properties[2] = DataLoader.weaponDataLoader1[n].DAMAGED;
			ProfileManager.weaponsRifle[n].Gun_Value.properties[0] = DataLoader.weaponDataLoader1[n].FIRE_RATE;
			ProfileManager.weaponsRifle[n].Gun_Value.properties[1] = DataLoader.weaponDataLoader1[n].CAPACITY;
			ProfileManager.weaponsRifle[n].Gun_Value.properties[3] = DataLoader.weaponDataLoader1[n].TIME_RELOAD;
			ProfileManager.weaponsRifle[n].Gun_Value.properties[4] = DataLoader.weaponDataLoader1[n].CRITICAL;
			ProfileManager.weaponsRifle[n].Gun_Value.PriceUpgrade = DataLoader.weaponDataLoader1[n].CoinUpgrade;
			ProfileManager.weaponsRifle[n].Gun_Value.SecuredBulletPrice = new SecuredInt(DataLoader.weaponDataLoader1[n].BulletPrice);
			ProfileManager.weaponsRifle[n].Gun_Value.SecuredGemPrice = new SecuredInt(DataLoader.weaponDataLoader1[n].MSPrice);
			ProfileManager.weaponsRifle[n].Gun_Value.campaignUpgrade = DataLoader.weaponDataLoader1[n].CampaignUpgrade;
			ProfileManager.weaponsRifle[n].Gun_Value.PassiveDesc = DataLoader.weaponDataLoader1[n].PassiveDesc;
			ProfileManager.weaponsRifle[n].Gun_Value.isVip = DataLoader.weaponDataLoader1[n].Vip;
			ProfileManager.weaponsRifle[n].Gun_Value.RankBase = DataLoader.weaponDataLoader1[n].RankBase;
			ProfileManager.weaponsRifle[n].Gun_Value.CostRankUp = DataLoader.weaponDataLoader1[n].CostRankUp;
			ProfileManager.weaponsRifle[n].Gun_Value.CurrencyUnlock = (Item)DataLoader.weaponDataLoader1[n].CurrencyUnlock;
			ProfileManager.weaponsRifle[n].Gun_Value.CurrencyRankUp = (Item)DataLoader.weaponDataLoader1[n].CurrencyRankUp;
            UnityEngine.Debug.Log("ProfileManager.weaponsRifle[n].Gun_Value.PriceUpgrade" + ProfileManager.weaponsRifle[n].Gun_Value.PriceUpgrade[2]);
            UnityEngine.Debug.Log("weaponDataLoader1______________" + n);
		}
		for (int num = 0; num < DataLoader.weaponDataLoader2.Length; num++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathWeaponWeapon2 : SPaths.PathWeaponWeapon2_Decrypt) + "Weapon" + num).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.weaponDataLoader2[num] = JsonConvert.DeserializeObject<WeaponDataLoader>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			ProfileManager.weaponsSpecial[num].Gun_Value.gunName = PopupManager.Instance.GetText((Localization0)DataLoader.weaponDataLoader2[num].IdName, null);
			ProfileManager.weaponsSpecial[num].Gun_Value.properties[2] = DataLoader.weaponDataLoader2[num].DAMAGED;
			ProfileManager.weaponsSpecial[num].Gun_Value.properties[0] = DataLoader.weaponDataLoader2[num].FIRE_RATE;
			ProfileManager.weaponsSpecial[num].Gun_Value.properties[1] = DataLoader.weaponDataLoader2[num].CAPACITY;
			ProfileManager.weaponsSpecial[num].Gun_Value.properties[3] = DataLoader.weaponDataLoader2[num].TIME_RELOAD;
			ProfileManager.weaponsSpecial[num].Gun_Value.properties[4] = DataLoader.weaponDataLoader2[num].CRITICAL;
			ProfileManager.weaponsSpecial[num].Gun_Value.PriceUpgrade = DataLoader.weaponDataLoader2[num].CoinUpgrade;
			ProfileManager.weaponsSpecial[num].Gun_Value.SecuredBulletPrice = new SecuredInt(DataLoader.weaponDataLoader2[num].BulletPrice);
			ProfileManager.weaponsSpecial[num].Gun_Value.SecuredGemPrice = new SecuredInt(DataLoader.weaponDataLoader2[num].MSPrice);
			ProfileManager.weaponsSpecial[num].Gun_Value.campaignUpgrade = DataLoader.weaponDataLoader2[num].CampaignUpgrade;
			ProfileManager.weaponsSpecial[num].Gun_Value.PassiveDesc = DataLoader.weaponDataLoader2[num].PassiveDesc;
			ProfileManager.weaponsSpecial[num].Gun_Value.isVip = DataLoader.weaponDataLoader2[num].Vip;
			ProfileManager.weaponsSpecial[num].Gun_Value.RankBase = DataLoader.weaponDataLoader2[num].RankBase;
			ProfileManager.weaponsSpecial[num].Gun_Value.CostRankUp = DataLoader.weaponDataLoader2[num].CostRankUp;
			ProfileManager.weaponsSpecial[num].Gun_Value.CurrencyUnlock = (Item)DataLoader.weaponDataLoader2[num].CurrencyUnlock;
			ProfileManager.weaponsSpecial[num].Gun_Value.CurrencyRankUp = (Item)DataLoader.weaponDataLoader2[num].CurrencyRankUp;
			UnityEngine.Debug.Log("weaponsSpecial______________" + num);
		}
		for (int num2 = 0; num2 < DataLoader.ramboDataLoader.Length; num2++)
		{
			text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathRambo : SPaths.PathRambo_Decrypt) + "Infor" + num2).text;
			text2 = ProfileManager.DataEncryption.decrypt2(text);
			DataLoader.ramboDataLoader[num2] = JsonConvert.DeserializeObject<RamboDataLoader>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			ProfileManager.rambos[num2].name = PopupManager.Instance.GetText((Localization0)DataLoader.ramboDataLoader[num2].IdName, null);
			ProfileManager.rambos[num2].optionProfile[0] = DataLoader.ramboDataLoader[num2].HpLevel;
			ProfileManager.rambos[num2].optionProfile[1] = DataLoader.ramboDataLoader[num2].SpeedLevel;
			ProfileManager.rambos[num2].optionProfile[2] = DataLoader.ramboDataLoader[num2].JumpLevel;
			ProfileManager.rambos[num2].PriceUpgrade = DataLoader.ramboDataLoader[num2].CoinUpgrade;
			ProfileManager.rambos[num2].CostRankUp = DataLoader.ramboDataLoader[num2].CostRankUp;
			ProfileManager.rambos[num2].RankBase = new SecuredInt(DataLoader.ramboDataLoader[num2].RankBase);
			ProfileManager.rambos[num2].CurrencyUnlock = new SecuredInt(DataLoader.ramboDataLoader[num2].CurrencyUnlock);
			ProfileManager.rambos[num2].CurrencyRankUp = new SecuredInt(DataLoader.ramboDataLoader[num2].CurrencyRankUp);
			ProfileManager.rambos[num2].GoldUnlock = new SecuredInt(DataLoader.ramboDataLoader[num2].CoinUnlock);
			ProfileManager.rambos[num2].GemUnlock = new SecuredInt(DataLoader.ramboDataLoader[num2].MsUnlock);
			ProfileManager.rambos[num2].StarUnlock = DataLoader.ramboDataLoader[num2].StarUnlock;
		}
		text = Resources.Load<TextAsset>(((!SplashScreen._isLoadResourceDecrypt) ? SPaths.PathVip : SPaths.PathVip_Decrypt) + "VipData").text;
		text2 = ProfileManager.DataEncryption.decrypt2(text);
		DataLoader.vipData = JsonConvert.DeserializeObject<VipData>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		DataLoader.LoadCharacterData();
		DataLoader.LoadStarGift();
		DataLoader.LoadPackInApp();
		DataLoader.LoadGacha();
	}

	private static void Log(string log)
	{
	}

	public static bool isLoaded;

	public static LevelData LevelDataCurrent;

	public static EnemyCharactor boss;

	public static EnemyCharactor[] tanks;

	public static EnemyCharactor[] maybay;

	public static MissionDataRoot[] missionDataRoot_Normal;

	public static MissionDataRoot[] missionDataRoot_Hard;

	public static MissionDataRoot[] missionDataRoot_SuperHard;

	public static MissionDataRoot[] missionDataRoot_Normal_S;

	public static MissionDataRoot[] missionDataRoot_Hard_S;

	public static MissionDataRoot[] missionDataRoot_SuperHard_S;

	private static RamboDataLoader[] ramboDataLoader;

	private static WeaponDataLoader[] weaponDataLoader1;

	private static WeaponDataLoader[] weaponDataLoader2;

	private static GrenadeDataLoader[] grenadeDataLoader;

	private static KnifeDataLoader[] knifeDataLoader;

	public static RankInfor[] rankInfor;

	public static AchievementData[] achievement;

	public static PackInforData[] StarterPackData;

	public static VipData vipData;

	public static global::CharacterData[] characterData;

	public static BossModeData bossModeData;

	public static StarGift starGift;

	public static JsonData dataPackInApp;

	public static JsonData dataGacha;

	public static JsonData dataLocalization;

	public static EyeBotDataLoader eyeBotDataLoader;

	public static bool isLoadDataEndlessMode;

	public static bool isLoadDataCampaign;

	public static bool isLoadBossModeData;
}
