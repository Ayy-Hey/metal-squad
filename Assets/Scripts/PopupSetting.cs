using System;
using System.Collections;
using System.Net.Mail;
using ABI;
using Database.Backup;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
	public override void Show()
	{
		base.Show();
		this.OpenTab(this.indexTabSelected);
		this.SetMusic();
		this.SetSound();
		this.SetControl();
		this.saveToday = new IntProfileData(this.dayResetCountBackupData, DateTime.Now.DayOfYear);
		if (this.saveToday.Data.Value != DateTime.Now.DayOfYear)
		{
			this.saveToday.setValue(DateTime.Now.DayOfYear);
			PlayerPrefs.SetInt(this.countBackupData, 3);
		}
		this.txtVersion.text = "v " + SplashScreen.ThisVersion;
	}

	private void ShowIDUser()
	{
		if (string.IsNullOrEmpty(FirebaseDatabaseManager.Instance.IDPlayFab))
		{
			this.txtIDUser.text = "No Internet Connection";
		}
		else
		{
			this.txtIDUser.text = "ID: " + FirebaseDatabaseManager.Instance.IDPlayFab;
		}
	}

	public void OpenTab(int indexTab)
	{
		AudioClick.Instance.OnClick();
		for (int i = 0; i < this.obj_TabObj.Length; i++)
		{
			this.obj_TabObj[i].SetActive(false);
			this.obj_TabActive[i].SetActive(false);
		}
		this.objLangSetting.SetActive(false);
		this.obj_TabObj[indexTab].SetActive(true);
		this.obj_TabActive[indexTab].SetActive(true);
		if (indexTab != 0)
		{
			if (indexTab != 1)
			{
				if (indexTab != 2)
				{
				}
			}
			else
			{
				if (FirebaseDatabaseManager.Instance.isLoginFB)
				{
					this.OnFBLoginSuccess();
				}
				else
				{
					this.OnFBLogoutSuccess();
				}
				this.ShowIDUser();
			}
		}
		else
		{
			switch (PopupManager.language)
			{
			case Language.English:
				this.txtLangauge.text = "English";
				break;
			case Language.TiengViet:
				this.txtLangauge.text = "Tiếng Việt";
				break;
			case Language.Indonesia:
				this.txtLangauge.text = "Bahasa Indonesia";
				break;
			case Language.BoDaoNha:
				this.txtLangauge.text = "Português";
				break;
			case Language.TayBanNha:
				this.txtLangauge.text = "Español";
				break;
			case Language.ThaiLan:
				this.txtLangauge.text = "ภาษาไทย";
				break;
			case Language.Nga:
				this.txtLangauge.text = "Русский";
				break;
			}
		}
	}

	private void OnFBLoginSuccess()
	{
		base.StartCoroutine(this.OnLoadUserAvatar());
		base.StartCoroutine(this.OnLoadUserName());
		this.buttonsFB[1].SetActive(true);
		this.buttonsFB[0].SetActive(false);
	}

	private IEnumerator OnLoadUserAvatar()
	{
		yield return new WaitUntil(() => MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserPictureSprite != null);
		this.imgAvatarFB.sprite = MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserPictureSprite;
		yield break;
	}

	private IEnumerator OnLoadUserName()
	{
		yield return new WaitUntil(() => !string.IsNullOrEmpty(MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserName));
		this.txtNameFB.text = MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserName;
		yield break;
	}

	private void OnFBLogoutSuccess()
	{
		this.imgAvatarFB.sprite = this.spriteIconDefault[ProfileManager.settingProfile.IDChar];
		this.txtNameFB.text = "Anonymous User";
		this.buttonsFB[0].SetActive(true);
		this.buttonsFB[1].SetActive(false);
	}

	public void OnLoginFB()
	{
		AudioClick.Instance.OnClick();
		
	}

	private IEnumerator WaitForPlayFabLoginCoroutine()
	{
		PopupManager.Instance.ShowMiniLoading();
		yield return new WaitUntil(() => MonoSingleton<FBAndPlayfabMgr>.Instance.IsLoggedOnPlayFab);
		this.OnFBLoginSuccess();
		this.ShowIDUser();
		PopupManager.Instance.CloseMiniLoading();
		FirebaseDatabaseManager.Instance.CheckVersionFB(delegate(int version)
		{
			DialogWarningOldData dialogWarningOldData = UnityEngine.Object.Instantiate(Resources.Load("Popup/DialogWarningOldData", typeof(DialogWarningOldData)), MenuManager.Instance.formCanvas.transform) as DialogWarningOldData;
			dialogWarningOldData.version = version;
			dialogWarningOldData.OnClosed = delegate()
			{
			};
			dialogWarningOldData.Show();
		});
		yield break;
	}

	public void OnLogoutFB()
	{
		AudioClick.Instance.OnClick();
		PopupManager.Instance.ShowDialog(delegate(bool callback)
		{
			if (callback)
			{
				MonoSingleton<FBAndPlayfabMgr>.Instance.LogOutFacebook();
				FirebaseDatabaseManager.Instance.isLoginFB = false;
				this.OnFBLogoutSuccess();
				PlayfabManager.Instance.LoginToPlayfab(delegate
				{
					this.ShowIDUser();
				});
			}
		}, 1, PopupManager.Instance.GetText(Localization0.Do_you_want_to_logout_facebook, null), PopupManager.Instance.GetText(Localization0.Logout, null));
	}

	public void OnRestoreData()
	{
	}

	private IEnumerator checkPingToGoogle()
	{
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error == null && www.isDone && www.bytesDownloaded > 0)
		{
			PopupManager.Instance.ShowDialog(delegate(bool yes)
			{
				if (yes)
				{
					DataLoader.LoadDataBossMode();
					DataLoader.LoadDataCampaign();
					DataLoader.LoadDataEndlessMode();
					PopupManager.Instance.ShowMiniLoading();
					UserSaveToFirebase userdata = new UserSaveToFirebase();
					userdata.isLoginFB = FirebaseDatabaseManager.Instance.isLoginFB;
					FirebaseDatabaseManager.Instance.DoSaveData(FirebaseDatabaseManager.Instance.IDPlayFab, userdata, delegate(bool isSuccess)
					{
						if (isSuccess)
						{
							PlayerPrefs.SetInt(this.countBackupData, PlayerPrefs.GetInt(this.countBackupData, 3) - 1);
							PopupManager.Instance.CloseMiniLoading();
							PopupManager.Instance.ShowDialog(delegate(bool ok)
							{
							}, 0, PopupManager.Instance.GetText(Localization0.Save_data_successfully, null), PopupManager.Instance.GetText(Localization0.Congratulations, null));
							if (userdata.isLoginFB)
							{
								userdata.isLoginFB = false;
								FirebaseDatabaseManager.Instance.DoSaveData(userdata.idDevice, userdata, null);
							}
						}
						else
						{
							PopupManager.Instance.CloseMiniLoading();
							PopupManager.Instance.ShowDialog(delegate(bool ok)
							{
							}, 0, PopupManager.Instance.GetText(Localization0.Cannot_connect_to_Server, null), PopupManager.Instance.GetText(Localization0.Error, null));
						}
					});
				}
			}, 1, "This action helps you save your game progress on the server. Do you want to continue?", "Save Data " + PlayerPrefs.GetInt(this.countBackupData, 3) + "/3");
		}
		else
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.Please_check_your_network_connection_and_try_again, null), PopupManager.Instance.GetText(Localization0.Warning, null));
		}
		yield break;
	}

	public void OnSaveData()
	{
		AudioClick.Instance.OnClick();
		
		if (PlayerPrefs.GetInt(this.countBackupData, 3) <= 0)
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.You_can_only_save_data_3_times_per_day, null), PopupManager.Instance.GetText(Localization0.Save_Data, null) + PlayerPrefs.GetInt(this.countBackupData, 3) + "/3");
			return;
		}
		if (this._Coroutine != null)
		{
			base.StopCoroutine(this._Coroutine);
		}
		this._Coroutine = base.StartCoroutine(this.checkPingToGoogle());
	}

	public void OnRestorePurchase()
	{
		AudioClick.Instance.OnClick();
		
		try
		{
			PopupManager.Instance.ShowDialog(delegate(bool callback)
			{
				if (callback)
				{
	//				InAppManager.Instance.RestorePurchases();
				}
			}, 1, PopupManager.Instance.GetText(Localization0.Do_you_agree_with_the_restoration, null), PopupManager.Instance.GetText(Localization0.Restore_purchased_items, null));
		}
		catch
		{
		}
	}

	public void OnEmailUs()
	{
		AudioClick.Instance.OnClick();
		
		string text = "sora.gamestudio@gmail.com";
		string text2 = this.MyEscapeURL("FEEDBACK/SUGGESTION");
		string text3 = this.MyEscapeURL(string.Concat(new string[]
		{
			"Please Enter your message here\n\n\n\n________\n\nPlease Do Not Modify This\n\nVersion: ",
			SplashScreen.ThisVersion,
			"\n\nModel: ",
			SystemInfo.deviceModel,
			"\n\nID: ",
			this.txtIDUser.text,
			"\n\nOS: ",
			SystemInfo.operatingSystem,
			"\n\n________"
		}));
		Application.OpenURL(string.Concat(new string[]
		{
			"mailto:",
			text,
			"?subject=",
			text2,
			"&body=",
			text3
		}));
	}

	private string MyEscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}

	public void OnJoinGroup()
	{
		AudioClick.Instance.OnClick();
		
		if (this.checkPackageAppIsPresent("com.facebook.katana"))
		{
			Application.OpenURL("fb://group/300915017178311");
		}
		else
		{
			Application.OpenURL("https://www.facebook.com/groups/300915017178311");
		}
	}

	public void OnFBPage()
	{
		AudioClick.Instance.OnClick();
		
		if (this.checkPackageAppIsPresent("com.facebook.katana"))
		{
			Application.OpenURL("fb://page/2062967430599368");
		}
		else
		{
			Application.OpenURL("https://www.facebook.com/MetalSquadGame/");
		}
	}

	private bool checkPackageAppIsPresent(string package)
	{
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getInstalledPackages", new object[]
			{
				0
			});
			int num = androidJavaObject2.Call<int>("size", new object[0]);
			for (int i = 0; i < num; i++)
			{
				AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("get", new object[]
				{
					i
				});
				string text = androidJavaObject3.Get<string>("packageName");
				if (text.CompareTo(package) == 0)
				{
					return true;
				}
			}
		}
		catch (Exception ex)
		{
		}
		return false;
	}

	private void ShowTick(int id)
	{
		for (int i = 0; i < this.objTickLangauge.Length; i++)
		{
			this.objTickLangauge[i].SetActive(i == id);
		}
	}

	public void ShowLangSetting()
	{
		int language = (int)PopupManager.language;
		this.ShowTickLang(language);
		this.objLangSetting.SetActive(true);
	}

	public void ShowTickLang(int id)
	{
		this.ShowTick(id);
	}

	public void OnComfirmLang()
	{
		for (int i = 0; i < this.objTickLangauge.Length; i++)
		{
			if (this.objTickLangauge[i].activeSelf)
			{
				PopupManager.language = (Language)i;
				PlayerPrefs.SetInt(PopupManager.Instance.SaveLanguage, i);
			}
		}
		MenuManager.Instance.objLoading.SetActive(true);
		SceneManager.LoadSceneAsync("Menu");
	}

	public override void OnClose()
	{
		AudioClick.Instance.OnClick();
		base.StopAllCoroutines();
		if (this.settingControl.gameObject.activeSelf)
		{
			
			this.settingControl.Hide();
			return;
		}
		base.OnClose();
		try
		{
			MenuManager.Instance.topUI.ShowCoin();
		}
		catch
		{
		}
	}

	public void OnClickMusic()
	{
		AudioClick.Instance.OnClick();
		ProfileManager.settingProfile.IsMusic = !ProfileManager.settingProfile.IsMusic;
		this.SetMusic();
		if (ProfileManager.settingProfile.IsMusic)
		{
			SingletonGame<AudioController>.Instance.PlayMusic(MenuManager.Instance.bgMenu);
		}
		else
		{
			SingletonGame<AudioController>.Instance.StopMusic();
		}
		
	}

	public void OnClickSound()
	{
		AudioClick.Instance.OnClick();
		ProfileManager.settingProfile.IsSound = !ProfileManager.settingProfile.IsSound;
		this.SetSound();
		
	}

	private void SetMusic()
	{
		if (ProfileManager.settingProfile.IsMusic)
		{
			this.toggle_Music.image.color = new Color(0.372549027f, 0.807843149f, 0.5411765f);
			this.img_Music.sprite = this.sprite_MusicOn;
			this.txt_Music.text = PopupManager.Instance.GetText(Localization0.On, null).ToUpper();
		}
		else
		{
			this.toggle_Music.image.color = new Color(0.5882353f, 0.5882353f, 0.5882353f);
			this.img_Music.sprite = this.sprite_MusicOff;
			this.txt_Music.text = PopupManager.Instance.GetText(Localization0.Off, null).ToUpper();
		}
	}

	private void SetSound()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.toggle_Sound.image.color = new Color(0.372549027f, 0.807843149f, 0.5411765f);
			this.img_Sound.sprite = this.sprite_SoundOn;
			this.txt_Sound.text = PopupManager.Instance.GetText(Localization0.On, null).ToUpper();
		}
		else
		{
			this.toggle_Sound.image.color = new Color(0.5882353f, 0.5882353f, 0.5882353f);
			this.img_Sound.sprite = this.sprite_SoundOff;
			this.txt_Sound.text = PopupManager.Instance.GetText(Localization0.Off, null).ToUpper();
		}
	}

	public void SetControl()
	{
		string str = "A";
		int typeControl = ProfileManager.settingProfile.TypeControl;
		if (typeControl != 0)
		{
			if (typeControl != 1)
			{
				if (typeControl == 2)
				{
					str = "C";
				}
			}
			else
			{
				str = "B";
			}
		}
		else
		{
			str = "A";
		}
		this.txt_Control.text = PopupManager.Instance.GetText(Localization0.Control, null) + " " + str;
	}

	private string countBackupData = "com.sora.metal.squad.setting.countsavedata";

	private string dayResetCountBackupData = "com.sora.metal.squad.setting.checkday";

	public int indexTabSelected;

	public ControllerSetting settingControl;

	public Sprite sprite_MusicOn;

	public Sprite sprite_MusicOff;

	public Sprite sprite_SoundOn;

	public Sprite sprite_SoundOff;

	public GameObject[] obj_TabObj;

	public GameObject[] obj_TabActive;

	public Image img_Music;

	public Image img_Sound;

	public Text txt_Music;

	public Text txt_Sound;

	public Text txt_Control;

	public Toggle toggle_Music;

	public Toggle toggle_Sound;

	public Button btn_Control;

	public Sprite[] spriteIconDefault;

	[Header("Account Settings")]
	public Image imgAvatarFB;

	public Text txtNameFB;

	public GameObject[] buttonsFB;

	public GameObject objResoreIOS;

	private IntProfileData saveToday;

	public Text txtIDUser;

	public Text txtVersion;

	[Header("Choose langauge")]
	public GameObject[] objTickLangauge;

	public GameObject objLangSetting;

	public Text txtLangauge;

	private Coroutine _Coroutine;

	private MailMessage mail = new MailMessage();

	public enum ETabSetting
	{
		Settings,
		Account,
		Support
	}
}
