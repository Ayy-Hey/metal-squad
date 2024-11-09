using System;
using System.Collections.Generic;
using Sale;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance
	{
		get
		{
			if (MenuManager.instance == null)
			{
				MenuManager.instance = UnityEngine.Object.FindObjectOfType<MenuManager>();
			}
			return MenuManager.instance;
		}
	}

	protected void Start()
	{
		PopupManager.isblockInput = false;
		if (!MenuManager.isFirstLoad)
		{
			MenuManager.isFirstLoad = true;
			MenuManager.backFormSpec = new FormUI[Enum.GetValues(typeof(FormUI)).Length];
			for (int i = 0; i < Enum.GetValues(typeof(FormUI)).Length; i++)
			{
				MenuManager.backFormSpec[i] = FormUI.None;
			}
		}
		this.popUpStack = new List<PopupBase>();
		ProfileManager.init(this.key, this.pass, this.xorkey);
		DataLoader.LoadData();
		if (ProfileManager.settingProfile.IsMusic)
		{
			SingletonGame<AudioController>.Instance.PlayMusic(this.bgMenu);
		}
		PopupManager.Instance.SetCanvas(this.popupCanvas);
		PopupManager.Instance.SetFormCanvas(this.formCanvas);
		if (SplashScreen._CheatCoin)
		{
			PopupManager.Instance.SaveReward(Item.Gold, 999999, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Gem, 999999, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.M4A1_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Machine_Gun_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Ice_Gun_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Sniper_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.MGL_140_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Spread_Gun_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Flame_Gun_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Thunder_Shot_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Laser_Gun_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Rocket_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.John_D_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Yoo_na_Fragment, 100, "_CheatCoin", null);
			PopupManager.Instance.SaveReward(Item.Dvornikov_Fragment, 100, "_CheatCoin", null);
		}
		this.topUI.ReloadMoney();
		if (this.formCurrent.idForm == FormUI.Menu)
		{
			TutorialUIManager.isFirstTutorial = false;
			for (int j = 0; j < Enum.GetValues(typeof(FormUI)).Length; j++)
			{
				MenuManager.backFormSpec[j] = FormUI.None;
			}
		}
		this.formCurrent.Show();
		PopupManager.Instance.ShowCongratulation(null, false, null);
	}

	private void OnDestroy()
	{
		MenuManager.instance = null;
	}

	private void Update()
	{
		if (this.async != null)
		{
			if (this.objLoading != null)
			{
				this.txtPercentLoading.text = ((int)(this.async.progress * 100f)).ToString() + "%";
			}
			return;
		}
		if ((this.objLoading != null && this.objLoading.activeSelf) || (PopupManager.Instance.miniLoading != null && PopupManager.Instance.miniLoading.gameObject.activeSelf))
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			PopupManager.Instance.ShowShopItem(delegate
			{
				this.topUI.ShowCoin();
			});
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL && GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer && !PopupManager.isblockInput)
		{
			if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None)
			{
				
				if (PopupManager.Instance.CloseAll())
				{
					return;
				}
				if (this.topUI.popupController != null && this.topUI.popupController.gameObject.activeSelf)
				{
					this.topUI.popupController.OnClose();
					return;
				}
				if (DemoSpecialSkillManager.instance != null && DemoSpecialSkillManager.instance.panelDemoSkill.activeSelf)
				{
					DemoSpecialSkillManager.instance.Close();
					return;
				}
				for (int i = this.listPopup.Length - 1; i >= 0; i--)
				{
					if (this.listPopup[i] != null && this.listPopup[i].activeSelf)
					{
						PopupBase component = this.listPopup[i].GetComponent<PopupBase>();
						if (!(component != null))
						{
							this.listPopup[i].SetActive(false);
							return;
						}
						PopupLuckySpin component2 = this.listPopup[i].GetComponent<PopupLuckySpin>();
						if (component.isActive && component2 == null)
						{
							component.OnClose();
							return;
						}
					}
				}
				if (this.popupLuckySpin != null && this.popupLuckySpin.isActive)
				{
					if (this.popupLuckySpin.addSpin.isActive)
					{
						this.popupLuckySpin.addSpin.OnClose();
						return;
					}
					this.popupLuckySpin.OnClose();
					return;
				}
				else
				{
					for (int j = 0; j < this.formCurrent.Obj_Popup.Length; j++)
					{
						if (this.formCurrent.Obj_Popup[j].activeSelf)
						{
							this.formCurrent.Obj_Popup[j].SetActive(false);
							return;
						}
					}
					if (SceneManager.GetActiveScene().name != "Menu")
					{
						this.BackForm();
						return;
					}
					PopupManager.Instance.ShowDialog(delegate(bool callback)
					{
						if (callback)
						{
							
							Application.Quit();
						}
					}, 1, PopupManager.Instance.GetText(Localization0.Do_you_want_to_exit_game, null), PopupManager.Instance.GetText(Localization0.Warning, null));
					return;
				}
			}
			else if (TutorialUIManager.tutorialIDCurrent != TutorialUIManager.TutorialID.Tut_FormMainMenu_1)
			{
				
				try
				{
					if (this.tutorial != null)
					{
						this.tutorial.CloseTutorial();
					}
				}
				catch
				{
					UnityEngine.Debug.LogError("Lỗi CloseTutorial");
				}
			}
		}
	}

	public void ChangeForm(FormUI formNext, FormUI backformSpec)
	{
		if (formNext == FormUI.None)
		{
			PopupManager.Instance.ShowDialog(delegate(bool callback)
			{
				if (callback)
				{
					if (this.OnChangeFormEnded != null)
					{
						this.OnChangeFormEnded();
					}
					
					Application.Quit();
				}
			}, 1, PopupManager.Instance.GetText(Localization0.Do_you_want_to_exit_game, null), PopupManager.Instance.GetText(Localization0.Quit_Game, null));
			return;
		}
		this.formCurrent.OnClose();
		MenuManager.formLast = this.formCurrent.idForm;
		if (backformSpec != FormUI.None)
		{
			MenuManager.backFormSpec[(int)formNext] = backformSpec;
		}
		this.objLoading.SetActive(true);
		this.async = SceneManager.LoadSceneAsync(formNext.ToString());
	}

	public void BackForm()
	{
		if (this.CloseLastPopUp())
		{
			return;
		}
		for (int i = 0; i < this.formCurrent.Obj_Popup.Length; i++)
		{
			if (this.formCurrent.Obj_Popup[i].activeSelf)
			{
				this.formCurrent.Obj_Popup[i].SetActive(false);
				return;
			}
		}
		if (this.ChangeTab(this.formCurrent.tabOpen - 1))
		{
			return;
		}
		if (PopupManager.Instance.CloseAll())
		{
			return;
		}
		this.ClearPopUpStack();
		if (MenuManager.backFormSpec[(int)this.formCurrent.idForm] != FormUI.None)
		{
			this.ChangeForm(MenuManager.backFormSpec[(int)this.formCurrent.idForm], FormUI.None);
			return;
		}
		if (this.formCurrent.backForm != FormUI.None)
		{
			this.ChangeForm(this.formCurrent.backForm, FormUI.Menu);
		}
	}

	public void HomeForm()
	{
		this.ClearPopUpStack();
		this.ChangeForm(FormUI.Menu, FormUI.Menu);
		this.topUI.obj_FormOther.SetActive(false);
		this.topUI.obj_FormMainMenu.SetActive(true);
	}

	public bool ChangeTab(int index)
	{
		if (this.formCurrent.tabObject.Length > 1 && this.formCurrent.tabObject.Length > index && index >= 0 && this.formCurrent.tabObject[index] != null)
		{
			AudioClick.Instance.OnClick();
			this.ClearPopUpStack();
			for (int i = 0; i < this.formCurrent.tabObject.Length; i++)
			{
				if (this.formCurrent.tabObject[i] != null)
				{
					this.formCurrent.tabObject[i].SetActive(false);
				}
			}
			this.formCurrent.tabObject[index].SetActive(true);
			this.formCurrent.OnTabClose();
			this.formCurrent.tabOpen = index;
			return true;
		}
		return false;
	}

	public void ClearPopUpStack()
	{
		PopupManager.Instance.CloseAll();
		if (this.topUI.popupController != null && this.topUI.popupController.gameObject.activeSelf)
		{
			this.topUI.popupController.OnClose();
		}
		try
		{
			if (DemoSpecialSkillManager.instance != null && DemoSpecialSkillManager.instance.panelDemoSkill.activeSelf)
			{
				DemoSpecialSkillManager.instance.Close();
			}
		}
		catch
		{
		}
		for (int i = this.listPopup.Length - 1; i >= 0; i--)
		{
			if (this.listPopup[i] != null)
			{
				PopupBase component = this.listPopup[i].GetComponent<PopupBase>();
				if (component != null)
				{
					component.OnClose();
				}
				else
				{
					this.listPopup[i].SetActive(false);
				}
			}
		}
		for (int j = 0; j < this.popUpStack.Count; j++)
		{
			this.popUpStack[j].gameObject.SetActive(false);
		}
		this.popUpStack.Clear();
	}

	public bool CloseLastPopUp()
	{
		AudioClick.Instance.OnClick();
		if (this.popUpStack.Count > 0 && this.popUpStack[this.popUpStack.Count - 1] != null)
		{
			
			this.popUpStack[this.popUpStack.Count - 1].gameObject.SetActive(false);
			return true;
		}
		return false;
	}

	public string GetNameForm(FormUI formUI)
	{
		switch (formUI)
		{
		case FormUI.Menu:
			return "FormMainMenu";
		case FormUI.UICampaign:
			return "FormCampaign";
		case FormUI.UIBossMode:
			return "FormBossMode";
		case FormUI.UICharacter:
			return "FormCharacter";
		case FormUI.UILoadOut:
			return "FormLoadOut";
		case FormUI.UIWeapon:
			return "FormWeapon";
		case FormUI.UIBlacksmith:
			return "FormBlacksmith";
		case FormUI.UIPvp:
			return "FormPVP";
		default:
			return "Error";
		}
	}

	private static MenuManager instance;

	public Action OnReturnHome;

	public string key;

	public string pass;

	public int xorkey;

	public Canvas popupCanvas;

	public Canvas formCanvas;

	public GameObject objLoading;

	public AsyncOperation async;

	[SerializeField]
	private Text txtPercentLoading;

	[Header("---------------Audio---------------")]
	public AudioClip bgMenu;

	[Header("---------------Top UI---------------")]
	public TopBar topUI;

	[Header("---------------Character---------------")]
	public NV_UI[] MainCharacters;

	public Transform obj_Characters;

	[Header("---------------Tutorial---------------")]
	public TutorialUIManager tutorial;

	[Header("---------------Form Special---------------")]
	public FormMainMenu formMainMenu;

	[Header("---------------Popup Special---------------")]
	public PopupInformation popupInformation;

	public PopupLuckySpin popupLuckySpin;

	public PopupDailyGift popupDailyGift;

	public PopupQuest popupQuest;

	public PopupBuy popupBuy;

	public PopupDailySale popupDailySale;

	public BestSaleCalculator popupSaleFromServer;

	public PopupStarterPack popupStarterPack;

	public PopupGift popupGift;

	public PopupLeaderboard popupLeaderboard;

	public PopupWarningUpgrade popupWarningUpgrade;

	[Header("---------------PopUp---------------")]
	public FormBase formCurrent;

	[SerializeField]
	public GameObject[] listPopup;

	public Text txt_NameForm;

	public List<PopupBase> popUpStack;

	public static FormUI formLast;

	public static FormUI[] backFormSpec;

	public static int LevelTotalCurrent;

	public static int IDMapSelect;

	public static int IDLevelSelect;

	public static int IDDifficultSelect;

	public static int BossCurrent;

	public static ETypeWeapon indexTabSpecial;

	public static bool isFirstLoad;

	private bool isLoaded;

	private Coroutine coroutineLoadForm;

	private Action OnChangeFormEnded;
}
