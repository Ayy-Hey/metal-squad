using System;
using StarMission;

namespace Database.Backup
{
	[Serializable]
	public class CampaignData
	{
		public CampaignData(int hardLevel, int missionStar)
		{
			this.hardLevel = hardLevel;
			this.missionStar = missionStar;
			this.m_mapIsUnlocked = new bool[60 * hardLevel];
			this.mapNumberStar = new int[60 * hardLevel];
			this.LoadMapsProfile(this.m_mapIsUnlocked, this.mapNumberStar, ProfileManager._CampaignProfile.MapsProfile);
			this.mapIsUnlocked = FirebaseProfileManager.BoolArrayToIntArray(this.m_mapIsUnlocked);
			this.m_normalMission = new bool[60 * missionStar];
			this.m_hardMission = new bool[60 * missionStar];
			this.m_superHardMission = new bool[60 * missionStar];
			this.LoadMissionData(this.m_normalMission, DataLoader.missionDataRoot_Normal);
			this.LoadMissionData(this.m_hardMission, DataLoader.missionDataRoot_Hard);
			this.LoadMissionData(this.m_superHardMission, DataLoader.missionDataRoot_SuperHard);
			this.normalMission = FirebaseProfileManager.BoolArrayToIntArray(this.m_normalMission);
			this.hardMission = FirebaseProfileManager.BoolArrayToIntArray(this.m_hardMission);
			this.superHardMission = FirebaseProfileManager.BoolArrayToIntArray(this.m_superHardMission);
		}

		private void LoadMissionData(bool[] firebaseMission, MissionDataRoot[] localMission)
		{
			for (int i = 0; i < localMission.Length; i++)
			{
				for (int j = 0; j < this.missionStar; j++)
				{
					firebaseMission[i * this.missionStar + j] = localMission[i].missionDataLevel.missionData[j].IsCompleted;
				}
			}
		}

		private void UpdateLocalMissionData()
		{
			FirebaseProfileManager.IntArrayToBoolArray(this.m_normalMission, this.normalMission);
			FirebaseProfileManager.IntArrayToBoolArray(this.m_hardMission, this.hardMission);
			FirebaseProfileManager.IntArrayToBoolArray(this.m_superHardMission, this.superHardMission);
			this.UpdateMissionData(this.m_normalMission, DataLoader.missionDataRoot_Normal);
			this.UpdateMissionData(this.m_hardMission, DataLoader.missionDataRoot_Hard);
			this.UpdateMissionData(this.m_superHardMission, DataLoader.missionDataRoot_SuperHard);
		}

		private void UpdateMissionData(bool[] firebaseMission, MissionDataRoot[] localMission)
		{
			for (int i = 0; i < localMission.Length; i++)
			{
				for (int j = 0; j < this.missionStar; j++)
				{
					localMission[i].missionDataLevel.missionData[j].IsCompleted = firebaseMission[i * 3 + j];
				}
			}
		}

		public void UpdateLocalProfile()
		{
			FirebaseProfileManager.IntArrayToBoolArray(this.m_mapIsUnlocked, this.mapIsUnlocked);
			this.UpdateMapsProfile(this.m_mapIsUnlocked, this.mapNumberStar, ProfileManager._CampaignProfile.MapsProfile);
			this.UpdateLocalMissionData();
		}

		private void LoadMapsProfile(bool[] firebaseMapIsUnlocked, int[] firebaseMapNumberStar, MapProfile[] localMapProfile)
		{
			for (int i = 0; i < localMapProfile.Length; i++)
			{
				for (int j = 0; j < this.hardLevel; j++)
				{
					firebaseMapIsUnlocked[i * this.hardLevel + j] = localMapProfile[i].isUnLocked[j].Data;
					firebaseMapNumberStar[i * this.hardLevel + j] = localMapProfile[i].numberStar[j].Data;
				}
			}
		}

		private void UpdateMapsProfile(bool[] firebaseMapIsUnlocked, int[] firebaseMapNumberStar, MapProfile[] localMapProfile)
		{
			for (int i = 0; i < localMapProfile.Length; i++)
			{
				for (int j = 0; j < this.hardLevel; j++)
				{
					localMapProfile[i].isUnLocked[j].setValue(firebaseMapIsUnlocked[i * this.hardLevel + j]);
					localMapProfile[i].numberStar[j].setValue(firebaseMapNumberStar[i * this.hardLevel + j]);
				}
			}
		}

		private bool[] m_mapIsUnlocked;

		public int[] mapIsUnlocked;

		public int[] mapNumberStar;

		private int hardLevel;

		private int missionStar;

		private bool[] m_normalMission;

		private bool[] m_hardMission;

		private bool[] m_superHardMission;

		public int[] normalMission;

		public int[] hardMission;

		public int[] superHardMission;
	}
}
