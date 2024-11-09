using System;
using UnityEngine;

namespace Database.Backup
{
	[Serializable]
	public class UserSaveToFirebase
	{
		public UserSaveToFirebase()
		{
			this.versionApp = SplashScreen.ThisVersion;
			this.idDevice = SystemInfo.deviceUniqueIdentifier;
			this.idFacebook = ((!FirebaseDatabaseManager.Instance.isLoginFB) ? "null" : FirebaseDatabaseManager.Instance.IDPlayFab);
			this.data = new FirebaseProfileManager();
			this.lastSave = DateTime.Now.ToString();
		}

		public void UpdateLocalProfile()
		{
			if (this.data != null)
			{
				this.data.UpdateProfile();
			}
		}

		public int versionBackup;

		public string versionApp;

		public string idDevice;

		public string idFacebook;

		public bool isLoginFB;

		public FirebaseProfileManager data;

		public string lastSave;
	}
}
