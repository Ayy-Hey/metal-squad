using System;

namespace Database.Backup
{
	[Serializable]
	public class EndlessData
	{
		public EndlessData(int hardLevel, int missionStar)
		{
		}

		private void UpdateLocalEndless()
		{
		}

		public void UpdateLocalProfile()
		{
		}

		private int hardLevel;

		private int missionStar;

		public int bestMeter;

		public int bestTime;

		public float bestEnemyDefeated;

		public int expRank;

		public int levelRank;

		public bool unlockEndless;
	}
}
