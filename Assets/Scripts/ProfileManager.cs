using System;
using com.dev.util.SecurityHelper;
using StarterPack;
using UnityEngine;

public class ProfileManager
{
	public static DataEncryption DataEncryption
	{
		get
		{
			return ProfileManager.dataEncryption;
		}
	}

	public static void init(string password, string saltKey, int xorKey)
	{
		if (!ProfileManager.isInit)
		{
			ProfileManager.dataEncryption = new DataEncryption(password, saltKey, xorKey, DataEncryption.ENCRYPTION_MODE.MODE_2_BALANCE);
			ProfileManager.versionProfile = new IntProfileData("com.sora.metal.squad.versionprofile", 0);
			ProfileManager.userProfile = new UserProfile();
			ProfileManager.CountGamePlay = new IntProfileData("com.sora.metal.squad.CountGamePlay", 0);
			ProfileManager._CampaignProfile = new CampaignProfile();
			ProfileManager.worldMapProfile = new WorldMapProfile[5];
			for (int i = 0; i < 5; i++)
			{
				ProfileManager.worldMapProfile[i] = new WorldMapProfile(i);
			}
			ProfileManager.LevelRankClaimedProfile = new IntProfileData("com.sora.metal.squad.rank.level", 0);
			ProfileManager.ExpRankProfile = new IntProfileData("com.sora.metal.squad.rank.exp", 0);
			ProfileManager.rambos = new RamboProfile[3];
			ProfileManager.rambos[0] = new RamboBoyProfile();
			ProfileManager.rambos[1] = new RamboGirlProfile();
			ProfileManager.rambos[2] = new RamboManProfile();
			ProfileManager.weaponsRifle = new WeaponProfile[6];
			ProfileManager.weaponsRifle[0] = new M4a1GunProfile();
			ProfileManager.weaponsRifle[1] = new MachineGunProfile(string.Empty);
			ProfileManager.weaponsRifle[2] = new IceGunProfile(string.Empty);
			ProfileManager.weaponsRifle[3] = new ShotgunProfile(string.Empty);
			ProfileManager.weaponsRifle[4] = new Mgl140GunProfile(string.Empty);
			ProfileManager.weaponsRifle[5] = new Ct9GunProfile(string.Empty);
			ProfileManager.weaponsSpecial = new WeaponProfile[6];
			ProfileManager.weaponsSpecial[0] = new FlameGunProfile(string.Empty);
			ProfileManager.weaponsSpecial[1] = new SniperGunProfile(string.Empty);
			ProfileManager.weaponsSpecial[2] = new LaserGunProfile(string.Empty);
			ProfileManager.weaponsSpecial[3] = new Fc10GunProfile(string.Empty);
			ProfileManager.weaponsSpecial[4] = new CannonGunProfile(string.Empty);
			ProfileManager.weaponsSpecial[5] = new ThunderShotGunProfile(string.Empty);
			ProfileManager.weaponsRifleDemo = new WeaponProfile[6];
			ProfileManager.weaponsRifleDemo[0] = new M4a1GunProfile();
			ProfileManager.weaponsRifleDemo[1] = new MachineGunProfile("demo.machinegun");
			ProfileManager.weaponsRifleDemo[2] = new IceGunProfile("demo.icegun");
			ProfileManager.weaponsRifleDemo[3] = new SniperGunProfile("demo.sniper");
			ProfileManager.weaponsRifleDemo[4] = new Mgl140GunProfile("demo.mgl140");
			ProfileManager.weaponsRifleDemo[5] = new Ct9GunProfile("demo.ct9");
			ProfileManager.weaponsSpecialDemo = new WeaponProfile[6];
			ProfileManager.weaponsSpecialDemo[0] = new ShotgunProfile("demo.shotgun");
			ProfileManager.weaponsSpecialDemo[1] = new FlameGunProfile("demo.flame");
			ProfileManager.weaponsSpecialDemo[2] = new ThunderShotGunProfile("demo.thunder");
			ProfileManager.weaponsSpecialDemo[3] = new LaserGunProfile("demo.laser");
			ProfileManager.weaponsSpecialDemo[4] = new CannonGunProfile("demo.canon");
			ProfileManager.weaponsSpecialDemo[5] = new Fc10GunProfile("demo.fc10");
			ProfileManager.grenadesProfile = new GrenadeProfile[4];
			ProfileManager.grenadesProfile[0] = new GrenadeBasicProfile();
			ProfileManager.grenadesProfile[1] = new GrenadeIceProfile();
			ProfileManager.grenadesProfile[2] = new GrenadeFireProfile();
			ProfileManager.grenadesProfile[3] = new GrenadeSmokeProfile();
			ProfileManager.melesProfile = new MeleProfile[3];
			ProfileManager.melesProfile[0] = new BuaGoProfile();
			ProfileManager.melesProfile[1] = new RiuProfile();
			ProfileManager.melesProfile[2] = new KiemProfile();
			ProfileManager.boosterProfile = new BoosterProfile();
			ProfileManager.inAppProfile = new InAppProfile();
			ProfileManager.grenadeValue = new GrenadeValue();
			ProfileManager.rifleGunCurrentId = new IntProfileData("rambo.weapon.rigleid", 0);
			ProfileManager.grenadeCurrentId = new IntProfileData("rambo.weapon.grenadeid", 0);
			ProfileManager.meleCurrentId = new IntProfileData("rambo.weapon.meleid", 0);
			ProfileManager.partOfGunProfile = new PartOfGunProtile();
			ProfileManager.bonus = new FloatProfileData("rambo.play.bonus", 1f);
			ProfileManager.grenades = new GrenadesProfile("3D");
			ProfileManager.airplaneSkillProfile = new AirplaneSkillProfile();
			ProfileManager.bombAirplaneSkillProfile = new BombAirplaneSkillProfile();
			ProfileManager.charManSupporterProfile = new CharManSupporterProfile();
			ProfileManager.eyeBotProfile = new EyeBotProfile();
			ProfileManager.ramboTrack = new RamboTrackProfile();
			ProfileManager.dailyGiftProfile = new DailyGiftProfile();
			ProfileManager.spinProfile = new SpinProfile();
			ProfileManager.countSpinByMS = new IntProfileData("com.rambo.metal.squad.freespin", 0);
			ProfileManager.upgradeCardProfile = new UpgradeCardProfile();
			ProfileManager.InforChars[0] = new InforCharactor("John D.", true, "UnLocked");
			ProfileManager.InforChars[0].from = "U.S.A";
			ProfileManager.InforChars[0].weapon = "M4A1";
			ProfileManager.InforChars[0].Description = ((!ProfileManager.InforChars[0].IsUnLocked) ? string.Empty : "UnLocked");
			ProfileManager.InforChars[1] = new InforCharactor("Yoo-na", false, string.Empty);
			ProfileManager.InforChars[1].from = "Korea";
			ProfileManager.InforChars[1].weapon = "M4A1 Pink";
			ProfileManager.InforChars[1].Description = ((!ProfileManager.InforChars[1].IsUnLocked) ? "Complete Level 3 To Unlock" : "UnLocked");
			ProfileManager.InforChars[2] = new InforCharactor("Dvornikov", false, string.Empty);
			ProfileManager.InforChars[2].from = "IRAQ";
			ProfileManager.InforChars[2].weapon = "Gatling Gun";
			ProfileManager.InforChars[2].Description = ((!ProfileManager.InforChars[2].IsUnLocked) ? "Complete Level 8 To Unlock" : "UnLocked");
			for (int j = 0; j < ProfileManager.MapSpecialProfiles.Length; j++)
			{
				int level = 1000 + j;
				ProfileManager.MapSpecialProfiles[j] = new MapProfile((ELevel)level);
			}
			ProfileManager.starterPackProfile = new StarterPackProfile();
			ProfileManager.dailySaleProfile = new DailySaleProfile();
			ProfileManager.bossModeProfile = new BossModeProfile();
			ProfileManager.controlProfile = new ControlProfile();
			ProfileManager.pvpProfile = new PvpProfile();
			ProfileManager.isInit = true;
		}
	}

	public static void InitSetting()
	{
		if (ProfileManager.settingProfile == null)
		{
			ProfileManager.settingProfile = new SettingProfile();
		}
	}

	public static void saveAll()
	{
		PlayerPrefs.Save();
	}

	public static void deleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	private static DataEncryption dataEncryption;

	public static SettingProfile settingProfile;

	public static UserProfile userProfile;

	public static MapProfile[] MapSpecialProfiles = new MapProfile[0];

	public static WorldMapProfile[] worldMapProfile;

	public static RamboProfile[] rambos;

	public static RamboTrackProfile ramboTrack;

	public static InforCharactor[] InforChars = new InforCharactor[3];

	public static DailyGiftProfile dailyGiftProfile;

	public static SpinProfile spinProfile;

	public static IntProfileData countSpinByMS;

	public static UpgradeCardProfile upgradeCardProfile;

	public static WeaponProfile[] weaponsRifleDemo;

	public static WeaponProfile[] weaponsSpecialDemo;

	public static WeaponProfile[] weaponsRifle;

	public static WeaponProfile[] weaponsSpecial;

	public static BoosterProfile boosterProfile;

	public static GrenadeProfile[] grenadesProfile;

	public static MeleProfile[] melesProfile;

	public static GrenadeValue grenadeValue;

	public static IntProfileData rifleGunCurrentId;

	public static IntProfileData grenadeCurrentId;

	public static IntProfileData meleCurrentId;

	public static GrenadesProfile grenades;

	public static PartOfGunProtile partOfGunProfile;

	public static FloatProfileData bonus;

	public static InAppProfile inAppProfile;

	public static StarterPackProfile starterPackProfile;

	public static DailySaleProfile dailySaleProfile;

	public static AirplaneSkillProfile airplaneSkillProfile;

	public static BombAirplaneSkillProfile bombAirplaneSkillProfile;

	public static CharManSupporterProfile charManSupporterProfile;

	public static EyeBotProfile eyeBotProfile;

	public static BossModeProfile bossModeProfile;

	public static ControlProfile controlProfile;

	public static int REWARD_MAX_ONLINE_GIFT = 5;

	public static int CHARACTER_MAX = 3;

	public static int CHARACTER_MAX_SKILL = 3;

	public static int UPGRADE_MAX_SKILL = 30;

	public static CampaignProfile _CampaignProfile;

	public static int MapCurrent;

	public static int LevelCurrent;

	public static EBoss bossCurrent;

	public static ELevel eLevelCurrent;

	public static bool unlockAll;

	private const string Str_RigleGunCurrentId = "rambo.weapon.rigleid";

	private const string Str_GrenadeCurrentId = "rambo.weapon.grenadeid";

	private const string Str_MeleCurrentId = "rambo.weapon.meleid";

	private const string Str_Bonus = "rambo.play.bonus";

	public static PvpProfile pvpProfile;

	public static IntProfileData CountGamePlay;

	public static IntProfileData versionProfile;

	public static IntProfileData LevelRankClaimedProfile;

	public static IntProfileData ExpRankProfile;

	public static bool isInit;
}
