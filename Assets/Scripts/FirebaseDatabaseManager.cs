using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Database.Backup;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseDatabaseManager : MonoBehaviour
{
	public static FirebaseDatabaseManager Instance
	{
		get
		{
			if (FirebaseDatabaseManager.instance == null)
			{
				FirebaseDatabaseManager.instance = UnityEngine.Object.FindObjectOfType<FirebaseDatabaseManager>();
			}
			return FirebaseDatabaseManager.instance;
		}
	}

	public void Init()
	{
		if (this.isInit)
		{
			return;
		}
		this.InitializeFirebase(this.databaseURL);
		this.isInit = true;
	}

	protected virtual void InitializeFirebase(string databaseURL)
	{
		
	}

	private void PushJson(string jSondata, string TableChild, string id, Action<bool> OnCompleted)
	{
		if (!this.isInit || string.IsNullOrEmpty(id))
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
			return;
		}
		
	}

	public void Pull<T>(string table, string id, UnityAction<List<T>> OnComplete, int amount)
	{
		
	}

	public void Pull<T>(string table, string id, UnityAction<T> OnComplete)
	{
		
	}


	

	public void DoSaveData(string ID, UserSaveToFirebase userdata, Action<bool> OnCompleted)
	{
		if (!this.isInit || string.IsNullOrEmpty(ID))
		{
			if (OnCompleted != null)
			{
				OnCompleted(false);
			}
			return;
		}
		
	}

	public void DoRestoreDataWithID(string ID, Action<bool> OnCompleted)
	{
		this.OnCompletedRestore = OnCompleted;
		if (!this.isInit || string.IsNullOrEmpty(ID))
		{
			OnCompleted(false);
			return;
		}
		UserSaveToFirebase userSaveToFirebase = new UserSaveToFirebase();
		this.Pull<UserSaveToFirebase>("UserProfile", ID, delegate(UserSaveToFirebase user)
		{
			if (OnCompleted != null)
			{
				OnCompleted(user != null);
			}
			try
			{
				user.UpdateLocalProfile();
				ProfileManager.versionProfile.setValue(user.versionBackup);
			}
			catch
			{
			}
		});
	}

	public void AutoSaveData()
	{
		if (string.IsNullOrEmpty(this.IDPlayFab))
		{
			return;
		}
		
		
	}

	public void CheckVersionFB(Action<int> OnRestoreNow)
	{
		
	}

	public VersionUpdate _version { get; set; }

	private void GetVersionUpdate()
	{
		string table = "GameInforAndroid";
		this.Pull<VersionUpdate>(table, "VersionUpdate", delegate(VersionUpdate _version)
		{
			if (_version != null && this.GetVersion(SplashScreen.ThisVersion) < this.GetVersion(_version.VersionCurrent))
			{
				this._version = _version;
				this.isRequireUpdateGame = true;
			}
		});
	}

	private int GetVersion(string version)
	{
		int result = 0;
		string[] array = version.Split(new char[]
		{
			'.'
		});
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string value in array)
		{
			stringBuilder.Append(value);
		}
		try
		{
			result = int.Parse(stringBuilder.ToString());
		}
		catch (Exception ex)
		{
		}
		return result;
	}

	private static FirebaseDatabaseManager instance;

	public string databaseURL;

	private bool isInit;

	public bool isNeedRestoreNow;


	private UnityAction<FirebaseProfileManager> OnCompleted;

	private const string TableUserProfile = "UserProfile";

	private const string TableVersionUpdateAndroid = "GameInforAndroid";

	private const string TableVersionUpdateIOS = "GameInforIOS";

	public bool isLoginFB;

	public string IDPlayFab;

	private Action<bool> OnCompletedRestore;

	private static bool checkAutoSave;

	public bool isRequireUpdateGame;
}
