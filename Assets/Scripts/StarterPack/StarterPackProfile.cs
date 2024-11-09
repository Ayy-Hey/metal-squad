using System;
using UnityEngine;

namespace StarterPack
{
	public class StarterPackProfile
	{
		public StarterPackProfile()
		{
			DataLoader.LoadStarterPack();
			this.DayProfile = new IntProfileData("com.sora.metal.squad.StarterPack.UI.StarterPackManager14", 0);
			this.SaveDayProfile = new IntProfileData("com.sora.metal.squad.StarterPack.UI.StarterPackManager14.SaveDay", -1);
			this.SaveStarterPack = new IntProfileData("com.sora.metal.squad.StarterPack.UI.StarterPackManager14.SaveStarterPack", 0);
			this.packsProfile = new PackProfile[3];
			for (int i = 0; i < this.packsProfile.Length; i++)
			{
				this.packsProfile[i] = new PackProfile("com.sora.metal.squad.StarterPack.UI" + i);
			}
			if (this.SaveDayProfile.Data.Value != DateTime.Now.DayOfYear)
			{
				if (this.SaveDayProfile.Data.Value == -1 || this.DayProfile.Data.Value >= 2)
				{
					this.Calculator();
					this.ID = this.GetID();
					bool flag = true;
					while (this.ID < 0 && flag)
					{
						this.ID = this.GetID();
						int num = 0;
						for (int j = 0; j < this.packsProfile.Length; j++)
						{
							if (this.packsProfile[j].Weight <= 0)
							{
								num++;
							}
						}
						if (num >= this.packsProfile.Length)
						{
							this.ID = -1;
							break;
						}
					}
					this.DayProfile.setValue(1);
				}
				else
				{
					this.DayProfile.setValue(this.DayProfile.Data.Value + 1);
				}
				this.SaveDayProfile.setValue(DateTime.Now.DayOfYear);
			}
			UnityEngine.Debug.Log("Starter Pack: " + this.ID);
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

		public void Calculator()
		{
			for (int i = 0; i < this.packsProfile.Length; i++)
			{
				int weight = 500;
				try
				{
					weight = DataLoader.StarterPackData[i].Weight;
				}
				catch
				{
				}
				this.packsProfile[i].Weight = weight;
			}
			if (ProfileManager.weaponsRifle[1].GetGunBuy())
			{
				this.packsProfile[2].Weight = 0;
			}
			if (ProfileManager.weaponsSpecial[0].GetGunBuy())
			{
				this.packsProfile[1].Weight = 0;
			}
		}

		public void Check()
		{
			if (this.ID < 0)
			{
				return;
			}
			if (this.packsProfile[this.ID].Weight <= 0)
			{
				this.ID = -1;
			}
		}

		public int ID
		{
			get
			{
				return this.SaveStarterPack.Data.Value;
			}
			set
			{
				this.SaveStarterPack.setValue(value);
			}
		}

		public int DayCurrent
		{
			get
			{
				return this.DayProfile.Data.Value;
			}
		}

		private IntProfileData DayProfile;

		private IntProfileData SaveDayProfile;

		private IntProfileData SaveStarterPack;

		public PackProfile[] packsProfile;
	}
}
