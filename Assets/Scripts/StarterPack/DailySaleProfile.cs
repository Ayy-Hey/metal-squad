using System;
using UnityEngine;

namespace StarterPack
{
	public class DailySaleProfile
	{
		public DailySaleProfile()
		{
			this.SaveDayProfile = new IntProfileData("com.sora.metal.squad.DailySaleProfile.SaveDayProfile2", -1);
			this.SaveDailySaleProfile = new IntProfileData("com.sora.metal.squad.DailySaleProfile.SaveDailySaleProfile", 0);
			this.packsProfile = new PackProfile[13];
			for (int i = 0; i < this.packsProfile.Length; i++)
			{
				this.packsProfile[i] = new PackProfile("com.sora.metal.squad.DailySaleProfile.UI" + i);
			}
			if (this.SaveDayProfile.Data.Value != DateTime.Now.DayOfYear)
			{
				this.Calculator();
				for (int j = 0; j < 5; j++)
				{
					this.ID = this.GetID();
					if (this.ID >= 0)
					{
						break;
					}
				}
				if (this.ID < 0)
				{
					this.ID = UnityEngine.Random.Range(0, 2);
				}
				this.SaveDayProfile.setValue(DateTime.Now.DayOfYear);
			}
		}

		private int GetID()
		{
			int result = -1;
			int num = 0;
			for (int i = 0; i < this.packsProfile.Length; i++)
			{
				num += this.packsProfile[i].Weight;
			}
			for (int j = 0; j < this.packsProfile.Length; j++)
			{
				float num2 = Mathf.Max(0f, (float)this.packsProfile[j].Weight) / (float)num;
				float num3 = UnityEngine.Random.Range(0f, 1f);
				if (num3 <= num2)
				{
					return j;
				}
			}
			return result;
		}

		public int ID
		{
			get
			{
				return this.SaveDailySaleProfile.Data.Value;
			}
			set
			{
				this.SaveDailySaleProfile.setValue(value);
			}
		}

		public int IDRandom
		{
			get
			{
				this.Calculator();
				return this.GetID();
			}
		}

		public void Calculator()
		{
			for (int i = 0; i < this.packsProfile.Length; i++)
			{
				int weight = 500;
				this.packsProfile[i].Weight = weight;
			}
			this.packsProfile[0].Weight = 0;
			this.packsProfile[1].Weight = 0;
			this.packsProfile[2].Weight = 0;
		}

		private IntProfileData SaveDayProfile;

		private IntProfileData SaveDailySaleProfile;

		public PackProfile[] packsProfile;
	}
}
