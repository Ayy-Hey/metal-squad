using System;

namespace DailyQuest.Level
{
	public class MapDailyQuest
	{
		public int ID { get; set; }

		public int TotalLevel { get; set; }

		public int TotalStar { get; set; }

		public bool NewLevel { get; set; }

		public bool UnlockMap { get; set; }
	}
}
