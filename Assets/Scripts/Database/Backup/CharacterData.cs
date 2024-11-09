using System;
using UnityEngine;

namespace Database.Backup
{
	[Serializable]
	public class CharacterData
	{
		public CharacterData()
		{
			this.levelUpgrade = new int[ProfileManager.rambos.Length];
			this.LoadLevelUpgrade(this.levelUpgrade, ProfileManager.rambos);
			this.rankUpped = new int[ProfileManager.rambos.Length];
			this.LoadRankUpped(this.rankUpped, ProfileManager.rambos);
			this.charactorIsUnLocked = new bool[ProfileManager.rambos.Length];
			this.LoadCharactorIsUnLocked(this.charactorIsUnLocked, ProfileManager.InforChars);
			this.LoadCharactorFragment();
		}

		public void UpdateLocalProfile()
		{
			this.UpdateLevelUpgrade(this.levelUpgrade, ProfileManager.rambos);
			this.UpdateRankUpped(this.rankUpped, ProfileManager.rambos);
			this.UpdateCharactorIsUnLocked(this.charactorIsUnLocked, ProfileManager.InforChars);
			this.UpdateCharactorFragment();
		}

		private void LoadCharactorIsUnLocked(bool[] firebaseCharactorIsUnLocked, InforCharactor[] localInforChars)
		{
			for (int i = 0; i < localInforChars.Length; i++)
			{
				firebaseCharactorIsUnLocked[i] = localInforChars[i].IsUnLocked;
			}
		}

		private void UpdateCharactorIsUnLocked(bool[] firebaseCharactorIsUnLocked, InforCharactor[] localInforChars)
		{
			for (int i = 0; i < localInforChars.Length; i++)
			{
				localInforChars[i].IsUnLocked = firebaseCharactorIsUnLocked[i];
			}
		}

		private void LoadLevelUpgrade(int[] firebaseLevelUpgrade, RamboProfile[] localRambos)
		{
			for (int i = 0; i < localRambos.Length; i++)
			{
				firebaseLevelUpgrade[i] = localRambos[i].LevelUpgrade;
			}
		}

		private void UpdateLevelUpgrade(int[] firebaseLevelUpgrade, RamboProfile[] localRambos)
		{
			for (int i = 0; i < localRambos.Length; i++)
			{
				localRambos[i].LevelUpgrade = firebaseLevelUpgrade[i];
			}
		}

		public void LoadRankUpped(int[] firebaseRankUpped, RamboProfile[] localRambos)
		{
			for (int i = 0; i < localRambos.Length; i++)
			{
				firebaseRankUpped[i] = localRambos[i].RankUpped;
			}
		}

		public void UpdateRankUpped(int[] firebaseRankUpped, RamboProfile[] localRambos)
		{
			try
			{
				for (int i = 0; i < localRambos.Length; i++)
				{
					localRambos[i].RankUpped = firebaseRankUpped[i];
				}
			}
			catch
			{
			}
		}

		public void LoadCharactorFragment()
		{
			this.John_D_Fragment = this.LoadFragment(Item.John_D_Fragment);
			this.Yoo_na_Fragment = this.LoadFragment(Item.Yoo_na_Fragment);
			this.Dvornikov_Fragment = this.LoadFragment(Item.Dvornikov_Fragment);
		}

		public void UpdateCharactorFragment()
		{
			this.SaveFragment(Item.John_D_Fragment, this.John_D_Fragment);
			this.SaveFragment(Item.Yoo_na_Fragment, this.Yoo_na_Fragment);
			this.SaveFragment(Item.Dvornikov_Fragment, this.Dvornikov_Fragment);
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

		public int[] levelSkill;

		public int[] levelUpgrade;

		public int[] rankUpped;

		public bool[] charactorIsUnLocked;

		public int John_D_Fragment;

		public int Yoo_na_Fragment;

		public int Dvornikov_Fragment;
	}
}
