using System;
using System.Collections.Generic;
using Achievement;


public class AchievementManager
{
	public static AchievementManager Instance
	{
		get
		{
			if (AchievementManager.instance == null)
			{
				AchievementManager.instance = new AchievementManager();
				try
				{
					AchievementManager.ArrayAchivementCompleted = new StringProfileData("com.sora.metal.squad.AchievementManager.ArrayAchivementCompleted", string.Empty);
					AchievementManager.LastAchievement = new Queue<string>();
					if (!AchievementManager.ArrayAchivementCompleted.Data.Equals(string.Empty))
					{
						string[] array = AchievementManager.ArrayAchivementCompleted.Data.Split(new char[]
						{
							'@'
						});
						for (int i = 0; i < array.Length; i++)
						{
							AchievementManager.LastAchievement.Enqueue(array[i]);
							if (AchievementManager.LastAchievement.Count > 4)
							{
								AchievementManager.LastAchievement.Dequeue();
							}
						}
					}
				}
				catch
				{
				}
			}
			return AchievementManager.instance;
		}
	}

	public void AddAchievement(InforAchievement achivement)
	{
		if (AchievementManager.ArrayAchivementCompleted == null || AchievementManager.LastAchievement == null)
		{
			return;
		}
		string data = AchievementManager.ArrayAchivementCompleted.Data;
		AchievementManager.ArrayAchivementCompleted.setValue((!data.Equals(string.Empty)) ? (data + "@" + achivement.Image) : achivement.Image);
		string[] array = AchievementManager.ArrayAchivementCompleted.Data.Split(new char[]
		{
			'@'
		});
		AchievementManager.LastAchievement.Enqueue(achivement.Image);
		if (AchievementManager.LastAchievement.Count > 4)
		{
			AchievementManager.LastAchievement.Dequeue();
		}
	}

	public Queue<string> GetLastAchievement()
	{
		return AchievementManager.LastAchievement;
	}

	public void MissionEnemy(int idEnemy, int idWeapon, int typeWeapon, Action<bool, AchievementManager.InforQuest> onCompleted)
	{
		AchievementManager.InforQuest inforQuest = new AchievementManager.InforQuest();
		try
		{
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				EDailyQuest type = (EDailyQuest)DataLoader.achievement[i].Type;
				if (type == EDailyQuest.ENEMY)
				{
					if ((DataLoader.achievement[i].achievement.Mission.Mode == -1 || DataLoader.achievement[i].achievement.Mission.Mode == (int)GameMode.Instance.EMode) && (DataLoader.achievement[i].achievement.Mission.Enemy.ID == -1 || DataLoader.achievement[i].achievement.Mission.Enemy.ID == idEnemy) && (DataLoader.achievement[i].achievement.Mission.Weapon == null || DataLoader.achievement[i].achievement.Mission.Weapon.ID == -1 || (DataLoader.achievement[i].achievement.Mission.Weapon.ID == idWeapon && (DataLoader.achievement[i].achievement.Mission.Weapon.Type == -1 || DataLoader.achievement[i].achievement.Mission.Weapon.Type == typeWeapon))))
					{
						DataLoader.achievement[i].achievement.Total++;
						bool arg = this.CheckAchievement(i);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)DataLoader.achievement[i].achievement.IdDesc, DataLoader.achievement[i].achievement.ValueDesc);
						inforQuest.Exp = DataLoader.achievement[i].achievement.Exp_Secured.Value;
						inforQuest.Coin = DataLoader.achievement[i].achievement.MS_Secured.Value;
						inforQuest.pathIcon = DataLoader.achievement[i].achievement.Image;
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	public void MissionUnlockMap(Action<bool, AchievementManager.InforQuest> onCompleted)
	{
		AchievementManager.InforQuest inforQuest = new AchievementManager.InforQuest();
		try
		{
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				EDailyQuest type = (EDailyQuest)DataLoader.achievement[i].Type;
				if (type == EDailyQuest.LEVEL)
				{
					if (DataLoader.achievement[i].achievement.Mission.MissionLevel.Map != null && DataLoader.achievement[i].achievement.Mission.MissionLevel.Map.UnlockMap && DataLoader.achievement[i].achievement.Mission.Mode != -1)
					{
						int num = 0;
						for (int j = 0; j < 12; j++)
						{
							if (ProfileManager._CampaignProfile.MapsProfile[DataLoader.achievement[i].achievement.Mission.MissionLevel.Map.ID * 12 + j].GetStar((GameMode.Mode)DataLoader.achievement[i].achievement.Mission.Mode) >= 1)
							{
								num++;
							}
						}
						DataLoader.achievement[i].achievement.Total = num;
						bool arg = this.CheckAchievement(i);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)DataLoader.achievement[i].achievement.IdDesc, DataLoader.achievement[i].achievement.ValueDesc);
						inforQuest.Exp = DataLoader.achievement[i].achievement.Exp_Secured.Value;
						inforQuest.Coin = DataLoader.achievement[i].achievement.MS_Secured.Value;
						inforQuest.pathIcon = DataLoader.achievement[i].achievement.Image;
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	public void MissionBossModeWin(Action<bool, AchievementManager.InforQuest> onCompleted)
	{
		AchievementManager.InforQuest inforQuest = new AchievementManager.InforQuest();
		try
		{
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				EDailyQuest type = (EDailyQuest)DataLoader.achievement[i].Type;
				if (type == EDailyQuest.BOSS_MODE)
				{
					if (DataLoader.achievement[i].achievement.Mission.bossModeWin != null && DataLoader.achievement[i].achievement.Mission.bossModeWin.Total > 0)
					{
						DataLoader.achievement[i].achievement.Total++;
						bool arg = this.CheckAchievement(i);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)DataLoader.achievement[i].achievement.IdDesc, DataLoader.achievement[i].achievement.ValueDesc);
						inforQuest.Exp = DataLoader.achievement[i].achievement.Exp_Secured.Value;
						inforQuest.Coin = DataLoader.achievement[i].achievement.MS_Secured.Value;
						inforQuest.pathIcon = DataLoader.achievement[i].achievement.Image;
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	public void MissionEarnCoin(int addCoin, Action<bool, AchievementManager.InforQuest> onCompleted = null)
	{
		AchievementManager.InforQuest inforQuest = new AchievementManager.InforQuest();
		try
		{
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				EDailyQuest type = (EDailyQuest)DataLoader.achievement[i].Type;
				if (type == EDailyQuest.EARN_MONEY)
				{
					if (DataLoader.achievement[i].achievement.Mission.earnCoin != null && DataLoader.achievement[i].achievement.Mission.earnCoin.TotalCoin > 0)
					{
						DataLoader.achievement[i].achievement.Total += addCoin;
						bool arg = this.CheckAchievement(i);
						inforQuest.Desc = PopupManager.Instance.GetText((Localization0)DataLoader.achievement[i].achievement.IdDesc, DataLoader.achievement[i].achievement.ValueDesc);
						inforQuest.Exp = DataLoader.achievement[i].achievement.Exp_Secured.Value;
						inforQuest.Coin = DataLoader.achievement[i].achievement.MS_Secured.Value;
						inforQuest.pathIcon = DataLoader.achievement[i].achievement.Image;
						if (onCompleted != null)
						{
							onCompleted(arg, inforQuest);
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	public void MissionEarnStar(int totalStar, Action<bool, AchievementManager.InforQuest> onCompleted)
	{
		AchievementManager.InforQuest inforQuest = new AchievementManager.InforQuest();
		try
		{
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				EDailyQuest type = (EDailyQuest)DataLoader.achievement[i].Type;
				if (type == EDailyQuest.EARN_STAR)
				{
					for (int j = 0; j < DataLoader.achievement[i].Achievements.Count; j++)
					{
						if (DataLoader.achievement[i].Achievements[j].Mission.earnStar != null && DataLoader.achievement[i].Achievements[j].Mission.earnStar.Total > 0)
						{
							DataLoader.achievement[i].Achievements[j].Total = totalStar;
							bool arg = this.CheckAchievement(i);
							inforQuest.Desc = PopupManager.Instance.GetText((Localization0)DataLoader.achievement[i].achievement.IdDesc, DataLoader.achievement[i].achievement.ValueDesc);
							inforQuest.Exp = DataLoader.achievement[i].achievement.Exp_Secured.Value;
							inforQuest.Coin = DataLoader.achievement[i].achievement.MS_Secured.Value;
							inforQuest.pathIcon = DataLoader.achievement[i].achievement.Image;
							if (onCompleted != null)
							{
								onCompleted(arg, inforQuest);
							}
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	public void MissionWeaponShop(Action<bool, AchievementManager.InforQuest> onCompleted = null)
	{
		AchievementManager.InforQuest inforQuest = new AchievementManager.InforQuest();
		try
		{
			int num = -1;
			for (int i = 0; i < ProfileManager.weaponsRifle.Length; i++)
			{
				if (ProfileManager.weaponsRifle[i].GetGunBuy())
				{
					num++;
				}
			}
			for (int j = 0; j < ProfileManager.weaponsSpecial.Length; j++)
			{
				if (ProfileManager.weaponsSpecial[j].GetGunBuy())
				{
					num++;
				}
			}
			for (int k = 0; k < DataLoader.achievement.Length; k++)
			{
				EDailyQuest type = (EDailyQuest)DataLoader.achievement[k].Type;
				if (type == EDailyQuest.WEAPON_SHOP)
				{
					for (int l = 0; l < DataLoader.achievement[k].Achievements.Count; l++)
					{
						if (DataLoader.achievement[k].Achievements[l].Mission.MissionUpgradeWeapon != null && DataLoader.achievement[k].Achievements[l].Mission.MissionUpgradeWeapon.UnLock != null)
						{
							DataLoader.achievement[k].Achievements[l].Total = num;
							bool arg = this.CheckAchievement(k);
							inforQuest.Desc = PopupManager.Instance.GetText((Localization0)DataLoader.achievement[k].achievement.IdDesc, DataLoader.achievement[k].achievement.ValueDesc);
							inforQuest.Exp = DataLoader.achievement[k].achievement.Exp_Secured.Value;
							inforQuest.Coin = DataLoader.achievement[k].achievement.MS_Secured.Value;
							inforQuest.pathIcon = DataLoader.achievement[k].achievement.Image;
							if (onCompleted != null)
							{
								onCompleted(arg, inforQuest);
							}
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	private bool CheckAchievement(int id)
	{
		bool result = false;
		try
		{
			if (DataLoader.achievement[id].achievement.State == EChievement.DOING && DataLoader.achievement[id].achievement.Total >= DataLoader.achievement[id].achievement.TotalRequirement)
			{
				result = true;
				DataLoader.achievement[id].achievement.State = EChievement.DONE;
				
			}
		}
		catch
		{
		}
		return result;
	}

	public string AchievementCompleted()
	{
		string result = "0/0";
		try
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				for (int j = 0; j < DataLoader.achievement[i].Achievements.Count; j++)
				{
					num2++;
					if (DataLoader.achievement[i].Achievements[j].State != EChievement.DOING)
					{
						num++;
					}
				}
			}
			result = num + "/" + num2;
		}
		catch
		{
		}
		return result;
	}

	public int GetAchivementCompleted()
	{
		int num = 0;
		try
		{
			int num2 = 0;
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				for (int j = 0; j < DataLoader.achievement[i].Achievements.Count; j++)
				{
					num2++;
					if (DataLoader.achievement[i].Achievements[j].State != EChievement.DOING)
					{
						num++;
					}
				}
			}
		}
		catch
		{
		}
		return num;
	}

	public bool hasCompleted
	{
		get
		{
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				for (int j = 0; j < DataLoader.achievement[i].Achievements.Count; j++)
				{
					if (DataLoader.achievement[i].Achievements[j].State == EChievement.DONE)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public int[] GemReward = new int[]
	{
		3,
		5,
		10,
		20,
		30
	};

	private static AchievementManager instance;

	private static StringProfileData ArrayAchivementCompleted;

	private static Queue<string> LastAchievement;

	public class InforQuest
	{
		public string Desc { get; set; }

		public int Coin { get; set; }

		public int Exp { get; set; }

		public string pathIcon { get; set; }
	}
}
