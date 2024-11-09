using System;
using UnityEngine;

namespace Player
{
	public class PlayerBooster
	{
		public void LoadProfile()
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode || GameMode.Instance.modePlay == GameMode.ModePlay.Campaign)
			{
				this.ActiveSkillBooster();
				this.ActiveDamagedBooster();
				this.ActiveSpeedBooster();
				this.ActiveMagnetBooster();
				this.ActiveArmorBooster();
				this.ActiveCriticalBooster();
				this.ActiveExpBooster();
				this.ActiveCoinBooster();
				if (ProfileManager.inAppProfile.vipProfile.level < E_Vip.Vip1)
				{
					this.maxMedkit = 2;
				}
				else
				{
					this.maxMedkit = DataLoader.vipData.Levels[(int)ProfileManager.inAppProfile.vipProfile.level].dailyReward.MedkitMax;
				}
				if (GameManager.Instance.hudManager.Max_Hp_Booster == -1)
				{
					GameManager.Instance.hudManager.Max_Hp_Booster = Mathf.Min(this.maxMedkit, ProfileManager.boosterProfile.GetItem(EBooster.MEDKIT));
				}
			}
		}

		private void ActiveCoinBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.DOUBLE_COIN))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.DOUBLE_COIN, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				PopupManager.Instance.SaveReward(Item.Booster_X2Gold, -1, "Use", null);
				GameManager.Instance.isDoubleCoin = true;
			}
		}

		private void ActiveExpBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.DOUBLE_EXP))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.DOUBLE_EXP, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				PopupManager.Instance.SaveReward(Item.Booster_X2Exp, -1, "Use", null);
				GameManager.Instance.isDoubleExp = true;
			}
		}

		private void ActiveCriticalBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.CRITICAL_HIT))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.CRITICAL_HIT, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				this.IsCritical = true;
				PopupManager.Instance.SaveReward(Item.Booster_Crit, -1, "Use", null);
			}
		}

		private void ActiveArmorBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.ARMOR))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.ARMOR, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				this.IsArmor = true;
				PopupManager.Instance.SaveReward(Item.Booster_Amor, -1, "Use", null);
			}
		}

		private void ActiveMagnetBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.MAGNET))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.MAGNET, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				this.IsMagnet = true;
				PopupManager.Instance.SaveReward(Item.Booster_Maget, -1, "Use", null);
			}
		}

		private void ActiveSkillBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.SKILL))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.SKILL, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				this.skillBooster = 0.3f;
				PopupManager.Instance.SaveReward(Item.Booster_Skill, -1, "Use", null);
			}
		}

		private void ActiveDamagedBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.DAMAGE))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.DAMAGE, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				this.DamagedBooster = 0.3f;
				PopupManager.Instance.SaveReward(Item.Booster_Damage, -1, "Use", null);
			}
		}

		private void ActiveSpeedBooster()
		{
			if (ProfileManager.boosterProfile.IsUseItem(EBooster.SPEED))
			{
				DailyQuestManager.Instance.MisionBooster(EBooster.SPEED, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
				{
					if (isCompleted)
					{
						UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
					}
				});
				this.speedBooster = 1.3f;
				PopupManager.Instance.SaveReward(Item.Booster_Speed, -1, "Use", null);
			}
		}

		public bool IsCritical;

		public bool IsArmor;

		public bool IsMagnet;

		public float skillBooster;

		public float DamagedBooster;

		public float speedBooster = 1f;

		private int maxMedkit;
	}
}
