using System;
using UnityEngine;

namespace Sale
{
	public class AIRecommendSale
	{
		public static AIRecommendSale Instance
		{
			get
			{
				if (AIRecommendSale.instance == null)
				{
					AIRecommendSale.instance = new AIRecommendSale();
				}
				return AIRecommendSale.instance;
			}
		}

		public int TypeGun(int powerGun1, int powerGun2)
		{
			this.scoreTypeGun += 0.1f * (float)((powerGun1 >= powerGun2) ? 1 : -1);
			if (this.isUnlockedAllGun(ProfileManager.weaponsRifle))
			{
				this.scoreTypeGun = 1f;
			}
			if (this.isUnlockedAllGun(ProfileManager.weaponsSpecial))
			{
				this.scoreTypeGun = 0f;
			}
			int num = (this.scoreTypeGun > 0.5f) ? 1 : 0;
			this.scoreTypeGun += 0.3f * (float)((num != 0) ? -1 : 1);
			return num;
		}

		public int IDGun1(int mapCurrent)
		{
			mapCurrent = Mathf.Min(1, mapCurrent);
			this.listScoreGun1[Mathf.Min(mapCurrent, this.listScoreGun1.Length - 1)] += 0.1f;
			for (int i = 0; i < ProfileManager.weaponsRifle.Length; i++)
			{
				if (ProfileManager.weaponsRifle[i].GetGunBuy())
				{
					this.listScoreGun1[i] -= 1f;
				}
			}
			int num = -1;
			float num2 = float.MinValue;
			for (int j = 0; j < this.listScoreGun1.Length; j++)
			{
				this.listScoreGun1[j] = Mathf.Clamp01(this.listScoreGun1[j]);
				if (this.listScoreGun1[j] > num2)
				{
					num2 = this.listScoreGun1[j];
					num = j;
				}
			}
			this.listScoreGun1[num] -= 0.2f;
			return num;
		}

		public int IDGun2(int mapCurrent)
		{
			this.listScoreGun2[Mathf.Min(mapCurrent, this.listScoreGun2.Length - 1)] += 0.1f;
			for (int i = 0; i < ProfileManager.weaponsSpecial.Length; i++)
			{
				if (ProfileManager.weaponsSpecial[i].GetGunBuy())
				{
					this.listScoreGun2[i] -= 1f;
				}
			}
			int num = -1;
			float num2 = float.MinValue;
			for (int j = 0; j < this.listScoreGun2.Length; j++)
			{
				this.listScoreGun2[j] = Mathf.Clamp01(this.listScoreGun2[j]);
				if (this.listScoreGun2[j] > num2)
				{
					num2 = this.listScoreGun2[j];
					num = j;
				}
			}
			this.listScoreGun2[num] -= 0.2f;
			return num;
		}

		private bool isUnlockedAllGun(WeaponProfile[] weaponProfiles)
		{
			foreach (WeaponProfile weaponProfile in weaponProfiles)
			{
				if (!weaponProfile.GetGunBuy())
				{
					return false;
				}
			}
			return true;
		}

		private static AIRecommendSale instance;

		private float scoreTypeGun = 0.5f;

		private float[] listScoreGun1 = new float[]
		{
			0f,
			1f,
			1f,
			1f,
			1f
		};

		private float[] listScoreGun2 = new float[]
		{
			1f,
			1f,
			1f,
			1f,
			1f
		};
	}
}
