using System;

namespace Database.Backup
{
	[Serializable]
	public class BossModeData
	{
		public BossModeData(int hardLevel, int missionStar)
		{
			this.hardLevel = hardLevel;
			this.missionStar = missionStar;
			this.m_bossModeProfile = new bool[Enum.GetValues(typeof(EBoss)).Length * hardLevel];
			this.LoadBossModeData(this.m_bossModeProfile, ProfileManager.bossModeProfile);
			this.bossModeProfile = FirebaseProfileManager.BoolArrayToIntArray(this.m_bossModeProfile);
		}

		public void UpdateLocalProfile()
		{
			FirebaseProfileManager.IntArrayToBoolArray(this.m_bossModeProfile, this.bossModeProfile);
			this.UpdateBossModeData(this.m_bossModeProfile, ProfileManager.bossModeProfile);
		}

		private void LoadBossModeData(bool[] firebaseBossMode, BossModeProfile localBossMode)
		{
			for (int i = 0; i < localBossMode.bossProfiles.Length; i++)
			{
				for (int j = 0; j < this.hardLevel; j++)
				{
					firebaseBossMode[i * this.hardLevel + j] = localBossMode.bossProfiles[i].unlockModes[j].Data;
				}
			}
		}

		private void UpdateBossModeData(bool[] firebaseBossMode, BossModeProfile localBossMode)
		{
			for (int i = 0; i < localBossMode.bossProfiles.Length; i++)
			{
				for (int j = 0; j < this.hardLevel; j++)
				{
					localBossMode.bossProfiles[i].unlockModes[j].setValue(firebaseBossMode[i * this.hardLevel + j]);
				}
			}
		}

		private bool[] m_bossModeProfile;

		public int[] bossModeProfile;

		private int hardLevel;

		private int missionStar;
	}
}
