using System;
using DailyQuest.Booster;
using DailyQuest.Boss;
using DailyQuest.BossMode;
using DailyQuest.Combo;
using DailyQuest.Enemy;
using DailyQuest.Level;
using DailyQuest.Transfer;
using DailyQuest.UpgradeWeapon;

namespace DailyQuest
{
	public class MissionDailyQuest
	{
		public EnemyDailyQuest Enemy { get; set; }

		public WeaponDailyQuest Weapon { get; set; }

		public BoosterDailyQuest MissionBooster { get; set; }

		public BossDailyQuest MissionBoss { get; set; }

		public LevelDailyQuest MissionLevel { get; set; }

		public UpgradeWeaponDailyQuest MissionUpgradeWeapon { get; set; }

		public ComboDailyQuest MissionCombo { get; set; }

		public TransferDailyQuest MissionTransfer { get; set; }

		public RescueYoona rescueYoona { get; set; }

		public EarnCoin earnCoin { get; set; }

		public EarnStar earnStar { get; set; }

		public BossModeWin bossModeWin { get; set; }

		public CharacterShop characterShop { get; set; }

		public int Mode { get; set; }

		public int TotalRequireLuckySpin { get; set; }

		public int TotalRequireGacha { get; set; }

		public int TotalRequirePvPFinish { get; set; }

		public int TotalRequirePvPWin { get; set; }

		public int TotalRequireBossModeFinish { get; set; }

		public int TotalRequireBossModeWin { get; set; }
	}
}
