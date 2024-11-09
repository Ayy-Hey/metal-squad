using System;
using UnityEngine;

namespace Database.Backup
{
	[Serializable]
	public class WeaponData
	{
		public WeaponData()
		{
			this.weaponsRifleLevel = new int[ProfileManager.weaponsRifle.Length];
			this.weaponsRifleGunBuy = new bool[ProfileManager.weaponsRifle.Length];
			this.weaponRifleRankUpped = new int[ProfileManager.weaponsRifle.Length];
			this.LoadWeapon(this.weaponsRifleLevel, this.weaponsRifleGunBuy, this.weaponRifleRankUpped, ProfileManager.weaponsRifle);
			this.weaponsSpecialLevel = new int[ProfileManager.weaponsSpecial.Length];
			this.weaponsSpecialGunBuy = new bool[ProfileManager.weaponsSpecial.Length];
			this.weaponsSpecialRankUpped = new int[ProfileManager.weaponsRifle.Length];
			this.LoadWeapon(this.weaponsSpecialLevel, this.weaponsSpecialGunBuy, this.weaponsSpecialRankUpped, ProfileManager.weaponsSpecial);
			this.grenadeLevel = new int[ProfileManager.grenadesProfile.Length];
			this.LoadGrenadeLevel(this.grenadeLevel, ProfileManager.grenadesProfile);
			this.meleUnlock = new bool[ProfileManager.melesProfile.Length];
			this.meleLevel = new int[ProfileManager.melesProfile.Length];
			this.LoadMele(this.meleUnlock, this.meleLevel, ProfileManager.melesProfile);
			this.LoadGunFragment();
		}

		public void UpdateLocalProfile()
		{
			this.UpdateWeapon(this.weaponsRifleLevel, this.weaponsRifleGunBuy, this.weaponRifleRankUpped, ProfileManager.weaponsRifle);
			this.UpdateWeapon(this.weaponsSpecialLevel, this.weaponsSpecialGunBuy, this.weaponsSpecialRankUpped, ProfileManager.weaponsSpecial);
			this.UpdateGrenadeLevel(this.grenadeLevel, ProfileManager.grenadesProfile);
			this.UpdateMele(this.meleUnlock, this.meleLevel, ProfileManager.melesProfile);
			this.UpdateGunFragment();
		}

		private void LoadMele(bool[] unlock, int[] level, MeleProfile[] meleProfile)
		{
			for (int i = 0; i < meleProfile.Length; i++)
			{
				unlock[i] = meleProfile[i].Unlock;
				level[i] = meleProfile[i].LevelUpGradeMelee;
			}
		}

		private void UpdateMele(bool[] unlock, int[] level, MeleProfile[] meleProfile)
		{
			for (int i = 0; i < meleProfile.Length; i++)
			{
				meleProfile[i].Unlock = unlock[i];
				meleProfile[i].LevelUpGradeMelee = level[i];
			}
		}

		private void LoadGrenadeLevel(int[] grenadeLevel, GrenadeProfile[] grenadeProfile)
		{
			for (int i = 0; i < grenadeProfile.Length; i++)
			{
				grenadeLevel[i] = grenadeProfile[i].LevelUpGrade;
			}
		}

		private void UpdateGrenadeLevel(int[] grenadeLevel, GrenadeProfile[] grenadeProfile)
		{
			for (int i = 0; i < grenadeProfile.Length; i++)
			{
				grenadeProfile[i].LevelUpGrade = grenadeLevel[i];
			}
		}

		private void LoadWeapon(int[] level, bool[] gunBuy, int[] rankUpped, WeaponProfile[] weaponProfile)
		{
			for (int i = 0; i < weaponProfile.Length; i++)
			{
				level[i] = weaponProfile[i].GetLevelUpgrade();
				gunBuy[i] = weaponProfile[i].GetGunBuy();
				rankUpped[i] = weaponProfile[i].GetRankUpped();
			}
		}

		private void UpdateWeapon(int[] level, bool[] gunBuy, int[] rankUpped, WeaponProfile[] weaponProfile)
		{
			for (int i = 0; i < weaponProfile.Length; i++)
			{
				weaponProfile[i].SetLevelUpgrade(level[i]);
				weaponProfile[i].SetGunBuy(gunBuy[i]);
				try
				{
					weaponProfile[i].SetRankUpped(rankUpped[i]);
				}
				catch
				{
				}
			}
		}

		public void LoadGunFragment()
		{
			this.M4A1_Fragment = this.LoadFragment(Item.M4A1_Fragment);
			this.Machine_Gun_Fragment = this.LoadFragment(Item.Machine_Gun_Fragment);
			this.Ice_Gun_Fragment = this.LoadFragment(Item.Ice_Gun_Fragment);
			this.Sniper_Fragment = this.LoadFragment(Item.Sniper_Fragment);
			this.MGL_140_Fragment = this.LoadFragment(Item.MGL_140_Fragment);
			this.Spread_Gun_Fragment = this.LoadFragment(Item.Spread_Gun_Fragment);
			this.Flame_Gun_Fragment = this.LoadFragment(Item.Flame_Gun_Fragment);
			this.Thunder_Shot_Fragment = this.LoadFragment(Item.Thunder_Shot_Fragment);
			this.Laser_Gun_Fragment = this.LoadFragment(Item.Laser_Gun_Fragment);
			this.Rocket_Fragment = this.LoadFragment(Item.Rocket_Fragment);
		}

		public void UpdateGunFragment()
		{
			this.SaveFragment(Item.M4A1_Fragment, this.M4A1_Fragment);
			this.SaveFragment(Item.Machine_Gun_Fragment, this.Machine_Gun_Fragment);
			this.SaveFragment(Item.Ice_Gun_Fragment, this.Ice_Gun_Fragment);
			this.SaveFragment(Item.Sniper_Fragment, this.Sniper_Fragment);
			this.SaveFragment(Item.MGL_140_Fragment, this.MGL_140_Fragment);
			this.SaveFragment(Item.Spread_Gun_Fragment, this.Spread_Gun_Fragment);
			this.SaveFragment(Item.Flame_Gun_Fragment, this.Flame_Gun_Fragment);
			this.SaveFragment(Item.Thunder_Shot_Fragment, this.Thunder_Shot_Fragment);
			this.SaveFragment(Item.Laser_Gun_Fragment, this.Laser_Gun_Fragment);
			this.SaveFragment(Item.Rocket_Fragment, this.Rocket_Fragment);
		}

		public int LoadFragment(Item item)
		{
			return PlayerPrefs.GetInt("metal.squad.frag." + item.ToString(), 0);
		}

		public void SaveFragment(Item item, int value)
		{
			int num = this.LoadFragment(item);
			PlayerPrefs.SetInt("metal.squad.frag." + item.ToString(), (num <= value) ? value : num);
		}

		public int[] weaponsRifleLevel;

		public bool[] weaponsRifleGunBuy;

		public int[] weaponRifleRankUpped;

		public int[] weaponsSpecialLevel;

		public bool[] weaponsSpecialGunBuy;

		public int[] weaponsSpecialRankUpped;

		public int[] grenadeLevel;

		public bool[] meleUnlock;

		public int[] meleLevel;

		public int M4A1_Fragment;

		public int Machine_Gun_Fragment;

		public int Ice_Gun_Fragment;

		public int Sniper_Fragment;

		public int MGL_140_Fragment;

		public int Spread_Gun_Fragment;

		public int Flame_Gun_Fragment;

		public int Thunder_Shot_Fragment;

		public int Laser_Gun_Fragment;

		public int Rocket_Fragment;
	}
}
