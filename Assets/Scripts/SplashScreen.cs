using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
	private void Start()
	{
		SplashScreen.isCheckFirstOpen = true;
		Screen.sleepTimeout = -1;
		this.timeLoading = Time.timeSinceLevelLoad;
		if (this.ClearData)
		{
			PlayerPrefs.DeleteAll();
		}
		this.txtVersion.text = ((!this.isTesting) ? this.version : "TEST");
		SplashScreen.ThisVersion = this.version;
		SplashScreen._CheatCoin = this.CheatCoin;
		SplashScreen._isBuildMarketing = this.isBuildMarketing;
		SplashScreen._isTesting = this.isTesting;
		SplashScreen._isLoadResourceDecrypt = this.isLoadResourceDecrypt;
		ProfileManager.unlockAll = this.unlockAll;
		ProfileManager.init(this.key, this.pass, this.xorkey);
		DataLoader.LoadData();
		base.StartCoroutine(this.LoadData2());
	}

	private IEnumerator LoadData2()
	{
		yield return new WaitForSeconds(2f);
		RemoteConfigFirebase.Instance.OnInit();
		DailyQuestManager.Instance.GetData();
		SaleManager.Instance.OnInit();
		ProfileManager.inAppProfile.vipProfile.GetVipLevel();
		ProfileManager.InitSetting();
		this.sync = SceneManager.LoadSceneAsync("Menu");
		this.sync.allowSceneActivation = false;
		Application.targetFrameRate = 60;
		this.coroutine1 = base.StartCoroutine(this.WaitingForNextScene());
		this.coroutine2 = base.StartCoroutine(this.WaitingForNextScene2());
		ProfileManager.spinProfile.Free = true;
		yield break;
	}

	private IEnumerator WaitingForNextScene2()
	{
		yield return new WaitForSeconds(4f);
		if (this.coroutine1 != null && !this.sync.allowSceneActivation)
		{
			base.StopCoroutine(this.coroutine1);
		}
		this.objLoading.SetActive(true);
		yield return new WaitForSeconds(3f);
		if (!PlayfabManager.Instance.isTimeReady)
		{
			PlayfabManager.Instance.isTimeReady = true;
			ProfileManager.dailyGiftProfile.CheckDay(DateTime.Now);
		}
		this.sync.allowSceneActivation = true;
		this.isDone = true;
		yield break;
	}

	private IEnumerator WaitingForNextScene()
	{
		yield return new WaitUntil(() => PlayfabManager.Instance.isTimeReady);
		if (this.coroutine2 != null)
		{
			base.StopCoroutine(this.coroutine2);
		}
		yield return new WaitForSeconds(4f);
		this.objLoading.SetActive(true);
		this.sync.allowSceneActivation = true;
		this.isDone = true;
		yield break;
	}

	private void Update()
	{
		if (this.sync == null || !this.objLoading.activeSelf)
		{
			return;
		}
		this.txtLoading.text = ((int)(this.sync.progress * 100f)).ToString() + "%";
	}

	public static bool _CheatCoin;

	public static bool _isBuildMarketing;

	public static bool _isTesting;

	public static string ThisVersion;

	public static bool _isLoadResourceDecrypt;

	public static bool isCheckFirstOpen;

	public string key;

	public string pass;

	public int xorkey;

	[Header("-------------------------------------")]
	public bool isLoadResourceDecrypt;

	public bool ClearData;

	public bool unlockAll;

	public bool CheatCoin;

	public bool isBuildMarketing;

	public bool isTesting;

	public string version;

	public Text txtVersion;

	public bool isDone;

	private AsyncOperation sync;

	public GameObject objLoading;

	public Text txtLoading;

	private float timeLoading;

	private Coroutine coroutine1;

	private Coroutine coroutine2;
}
