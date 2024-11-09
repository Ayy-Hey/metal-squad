using System;

namespace Rank
{
	public class RankInfor
	{
		public string Name { get; set; }

		public int IdName { get; set; }

		public int Exp { get; set; }

		public void LoadProfile(int index)
		{
			this.Level = index;
		}

		public int Gold { get; set; }

		public int Gem { get; set; }

		public int Gacha_C { get; set; }

		public int Gacha_B { get; set; }

		public int Level;
	}
}
