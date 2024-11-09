using System;
using System.Collections;

namespace Database.Backup
{
	[Serializable]
	public class FirebaseProfileManager
	{
		public FirebaseProfileManager()
		{
			this.hardLevel = 3;
			this.missionStar = 3;
			this.gamePlayData = new GamePlayData(this.hardLevel, this.missionStar);
			this.characterData = new CharacterData();
			this.weaponData = new WeaponData();
			this.userData = new UserData();
		}

		public static int[] BoolArrayToIntArray(bool[] boolArr)
		{
			int[] array = new int[boolArr.Length / 32 + 1];
			for (int i = 0; i < array.Length; i++)
			{
				bool[] array2 = new bool[32];
				int num = boolArr.Length - i * 32;
				Array.Copy(boolArr, i * 32, array2, 0, (num <= 32) ? num : 32);
				BitArray bitArray = new BitArray(array2);
				bitArray.CopyTo(array, i);
			}
			return array;
		}

		public static void IntArrayToBoolArray(bool[] boolArr, int[] intArr)
		{
			BitArray bitArray = new BitArray(intArr);
			for (int i = 0; i < boolArr.Length; i++)
			{
				boolArr[i] = bitArray[i];
			}
		}

		public void UpdateProfile()
		{
			this.gamePlayData.UpdateLocalProfile();
			this.characterData.UpdateLocalProfile();
			this.weaponData.UpdateLocalProfile();
			this.userData.UpdateLocalProfile();
		}

		private int hardLevel;

		private int missionStar;

		public GamePlayData gamePlayData;

		public CharacterData characterData;

		public WeaponData weaponData;

		public UserData userData;
	}
}
