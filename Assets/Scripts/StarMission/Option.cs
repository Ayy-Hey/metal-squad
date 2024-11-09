using System;

namespace StarMission
{
	public class Option
	{
		public Option()
		{
			this.level = new Level[5];
			for (int i = 0; i < this.level.Length; i++)
			{
				this.level[i] = new Level();
			}
		}

		public Level[] level;
	}
}
