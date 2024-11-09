using System;
using UnityEngine;
using UnityEngine.UI;

public class FormBossMode : FormBase
{
	private void OnValidate()
	{
		if (this.onValidate && !Application.isPlaying)
		{
			SplashScreen._isLoadResourceDecrypt = true;
			this.cardsBoss = this.obj_cardsBossFather.GetComponentsInChildren<CardBossMode>();
			for (int i = 0; i < this.cardsBoss.Length; i++)
			{
				if (this.cardsBoss[i].gameObject.activeSelf)
				{
					int boss = (int)this.cardsBoss[i].boss;
					this.cardsBoss[i].idCard = i;
					this.cardsBoss[i].nameDisplay = DataLoader.bossModeData.boss[boss].name;
					this.cardsBoss[i].unlockMap = DataLoader.bossModeData.boss[boss].unlockMap;
					this.cardsBoss[i].unlockLevel = DataLoader.bossModeData.boss[boss].unlockLevel;
					this.cardsBoss[i].power_0 = DataLoader.bossModeData.boss[boss].power[0];
					this.cardsBoss[i].power_1 = DataLoader.bossModeData.boss[boss].power[1];
					this.cardsBoss[i].power_2 = DataLoader.bossModeData.boss[boss].power[2];
					this.cardsBoss[i].txt_Lock.text = this.cardsBoss[i].unlockMap + "." + this.cardsBoss[i].unlockLevel;
					this.cardsBoss[i].sprite_Boss = this.tran_spritesBossFather.Find(this.cardsBoss[i].key.ToString()).gameObject.GetComponent<SpriteBoss>();
					if (this.cardsBoss[i].sprite_Boss == null)
					{
						UnityEngine.Debug.LogError("Lỗi: Sprite của boss bị null!");
					}
				}
			}
		}
	}

	public override void Show()
	{
		base.Show();
		if (ThisPlatform.IsIphoneX)
		{
			this.rect_Top.localPosition = new Vector3(70f, this.rect_Top.localPosition.y, 0f);
		}
		else
		{
			this.rect_Top.localPosition = new Vector3(0f, this.rect_Top.localPosition.y, 0f);
		}
		int num = 0;
		for (int i = 0; i < this.cardsBoss.Length; i++)
		{
			if (!ProfileManager.bossModeProfile.bossProfiles[(int)this.cardsBoss[i].boss].CheckUnlock(GameMode.Mode.NORMAL))
			{
				this.cardsBoss[i].obj_Lock.SetActive(true);
				for (int j = 0; j < this.cardsBoss[i].sprite_Boss.UISpriteBoss.Length; j++)
				{
					if (this.cardsBoss[i].sprite_Boss.UISpriteBoss[j].skeleton_Boss.AnimationState == null)
					{
						this.cardsBoss[i].sprite_Boss.UISpriteBoss[j].skeleton_Boss.Initialize(true);
					}
					this.cardsBoss[i].sprite_Boss.UISpriteBoss[j].skeleton_Boss.AnimationState.SetAnimation(0, this.cardsBoss[i].sprite_Boss.UISpriteBoss[j].str_nameAni_Off, true);
					if (this.cardsBoss[i].boss == EBoss.War_Bunker)
					{
						this.cardsBoss[i].sprite_Boss.UISpriteBoss[j].skeleton_Boss.color = new Color(0.129411772f, 0.129411772f, 0.129411772f);
					}
					else
					{
						this.cardsBoss[i].sprite_Boss.UISpriteBoss[j].skeleton_Boss.color = Color.black;
					}
				}
				if (this.cardsBoss[i].sprite_Boss.img_BossBase != null)
				{
					this.cardsBoss[i].sprite_Boss.img_BossBase.color = new Color(0.129411772f, 0.129411772f, 0.129411772f);
				}
			}
			else
			{
				this.cardsBoss[i].obj_Lock.SetActive(false);
				num = i;
				for (int k = 0; k < this.cardsBoss[i].sprite_Boss.UISpriteBoss.Length; k++)
				{
					if (this.cardsBoss[i].sprite_Boss.UISpriteBoss[k].skeleton_Boss.AnimationState == null)
					{
						this.cardsBoss[i].sprite_Boss.UISpriteBoss[k].skeleton_Boss.Initialize(true);
					}
					this.cardsBoss[i].sprite_Boss.UISpriteBoss[k].skeleton_Boss.AnimationState.SetAnimation(0, this.cardsBoss[i].sprite_Boss.UISpriteBoss[k].str_nameAni_On, true);
					this.cardsBoss[i].sprite_Boss.UISpriteBoss[k].skeleton_Boss.color = Color.white;
				}
				if (this.cardsBoss[i].sprite_Boss.img_BossBase != null)
				{
					this.cardsBoss[i].sprite_Boss.img_BossBase.color = Color.white;
				}
			}
		}
		if (AutoCheckLevel.isAutoCheck)
		{
			if (AutoCheckLevel.LevelBossMode < AutoCheckLevel.CheckToLevelBossMode)
			{
				num = AutoCheckLevel.LevelBossMode;
				this.ChangeBossActive(this.cardsBoss[num]);
				this.ChangeDifficultMode(0);
				this.OnBattle();
			}
			else
			{
				AutoCheckLevel.isAutoCheck = false;
				MenuManager.Instance.BackForm();
			}
			return;
		}
		if (FormBossMode.isRestartBoss)
		{
			FormBossMode.isRestartBoss = false;
			for (int l = 0; l < this.cardsBoss.Length; l++)
			{
				if (this.cardsBoss[l].boss == ProfileManager.bossCurrent)
				{
					if (FormBossMode.isNextBoss && l + 1 < this.cardsBoss.Length)
					{
						FormBossMode.isNextBoss = false;
						this.ChangeBossActive(this.cardsBoss[l + 1]);
					}
					else
					{
						this.ChangeBossActive(this.cardsBoss[l]);
					}
				}
			}
			this.ChangeDifficultMode((int)GameMode.Instance.modeBossMode);
		}
		else if (MenuManager.formLast == FormUI.UILoadOut)
		{
			this.ChangeBossActive(this.cardsBoss[FormBossMode.indexCardBossActive]);
			this.ChangeDifficultMode((int)GameMode.Instance.modeBossMode);
		}
		else
		{
			this.ChangeBossActive(this.cardsBoss[num]);
			this.ChangeDifficultMode(0);
		}
		if (FormBossMode.indexCardBossActive <= 3)
		{
			this.scrollbarBoss.horizontalNormalizedPosition = 0f;
		}
		else if (FormBossMode.indexCardBossActive >= this.cardsBoss.Length - 4)
		{
			this.scrollbarBoss.horizontalNormalizedPosition = 1f;
		}
		else
		{
			this.scrollbarBoss.horizontalNormalizedPosition = ((float)FormBossMode.indexCardBossActive - 3f) / (float)(this.cardsBoss.Length - 7);
		}
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormBossMode_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		else if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && MenuManager.Instance.tutorial.listTutorialUI[4] != null && PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[4].keyPlayerPrefs, 0) == 0)
		{
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_FormBossMode_1);
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		
	}

	public void ChangeBossActive(CardBossMode card)
	{
		AudioClick.Instance.OnClick();
		this.cardsBoss[FormBossMode.indexCardBossActive].obj_Active.SetActive(false);
		this.cardsBoss[FormBossMode.indexCardBossActive].sprite_Boss.gameObject.SetActive(false);
		FormBossMode.indexCardBossActive = card.idCard;
		card.obj_Active.SetActive(true);
		card.sprite_Boss.gameObject.SetActive(true);
		this.txt_NameBoss.text = card.nameDisplay.ToUpper();
		this.txt_LevelUnlock.text = card.txt_Lock.text;
		for (int i = 0; i < this.cardsDifficultMode.Length; i++)
		{
			if (!this.cardsBoss[FormBossMode.indexCardBossActive].obj_Lock.activeSelf && ProfileManager.bossModeProfile.bossProfiles[(int)((EBoss)Enum.Parse(typeof(EBoss), this.cardsBoss[FormBossMode.indexCardBossActive].key))].CheckUnlock(i))
			{
				this.cardsDifficultMode[i].obj_Lock.SetActive(false);
				this.cardsDifficultMode[i].img_BG.sprite = this.cardsDifficultMode[i].sprite_BtnBaseEnable;
			}
			else
			{
				this.cardsDifficultMode[i].obj_Lock.SetActive(true);
				this.cardsDifficultMode[i].img_BG.sprite = this.cardsDifficultMode[i].sprite_BtnBaseDisable;
			}
		}
		this.ChangeDifficultMode(0);
		this.LoadPowerBoss();
		this.CheckButtonStart();
		this.LoadDropList();
		
	}

	public void ChangeDifficultMode(int index)
	{
		AudioClick.Instance.OnClick();
		this.cardsDifficultMode[this.indexDifficultMode].obj_Active.SetActive(false);
		this.cardsDifficultMode[this.indexDifficultMode].obj_effectCamera.SetActive(false);
		this.cardsDifficultMode[index].obj_Active.SetActive(true);
		this.cardsDifficultMode[index].obj_effectCamera.SetActive(true);
		int emode = (int)GameMode.Instance.EMode;
		for (int i = 0; i < this.cardBonus.Length; i++)
		{
			this.cardBonus[i].img_Lock.enabled = (i != index);
		}
		if (this.cardsDifficultMode[index].obj_Lock.activeSelf && !this.cardsBoss[FormBossMode.indexCardBossActive].obj_Lock.activeSelf && index != 0)
		{
			PopupManager.Instance.ShowDialog(null, 0, this.ShowBossNote(index), PopupManager.Instance.GetText(Localization0.Locked, null));
		}
		this.indexDifficultMode = index;
		this.LoadPowerBoss();
		this.CheckButtonStart();
		
	}

	public void LoadPowerBoss()
	{
		if (this.indexDifficultMode == 0)
		{
			this.txt_BossPower.text = this.cardsBoss[FormBossMode.indexCardBossActive].power_0 + string.Empty;
		}
		else if (this.indexDifficultMode == 1)
		{
			this.txt_BossPower.text = this.cardsBoss[FormBossMode.indexCardBossActive].power_1 + string.Empty;
		}
		else if (this.indexDifficultMode == 2)
		{
			this.txt_BossPower.text = this.cardsBoss[FormBossMode.indexCardBossActive].power_2 + string.Empty;
		}
		else
		{
			UnityEngine.Debug.LogError("Lỗi độ khó!");
		}
	}

	private void CheckButtonStart()
	{
		if (!this.cardsBoss[FormBossMode.indexCardBossActive].obj_Lock.activeSelf && ProfileManager.bossModeProfile.bossProfiles[(int)((EBoss)Enum.Parse(typeof(EBoss), this.cardsBoss[FormBossMode.indexCardBossActive].key))].CheckUnlock(this.indexDifficultMode))
		{
			this.obj_BtnActive.SetActive(true);
		}
		else
		{
			this.obj_BtnActive.SetActive(false);
		}
	}

	private string ShowBossNote(int index)
	{
		string result = string.Empty;
		string[] array = new string[2];
		array[0] = this.cardsBoss[FormBossMode.indexCardBossActive].unlockMap + " - " + this.cardsBoss[FormBossMode.indexCardBossActive].unlockLevel;
		if (index != 0)
		{
			if (index != 1)
			{
				if (index == 2)
				{
					array[1] = PopupManager.Instance.GetText(Localization0.Crazy, null);
					result = PopupManager.Instance.GetText(Localization0.Challenge_Boss_in_level_to_unlock, array);
				}
			}
			else
			{
				array[1] = PopupManager.Instance.GetText(Localization0.Hard, null);
				result = PopupManager.Instance.GetText(Localization0.Challenge_Boss_in_level_to_unlock, array);
			}
		}
		return result;
	}

	public void OnBattle()
	{
		try
		{
			if (!ProfileManager.unlockAll)
			{
				if (this.cardsBoss[FormBossMode.indexCardBossActive].obj_Lock.activeSelf)
				{
					string[] nameSpec = new string[]
					{
						this.cardsBoss[FormBossMode.indexCardBossActive].unlockMap + " - " + this.cardsBoss[FormBossMode.indexCardBossActive].unlockLevel,
						PopupManager.Instance.GetText(Localization0.Normal, null)
					};
					PopupManager.Instance.ShowDialog(delegate(bool callback)
					{
					}, 0, PopupManager.Instance.GetText(Localization0.Challenge_Boss_in_level_to_unlock, nameSpec), PopupManager.Instance.GetText(Localization0.Warning, null));
					return;
				}
				if (!ProfileManager.bossModeProfile.bossProfiles[(int)((EBoss)Enum.Parse(typeof(EBoss), this.cardsBoss[FormBossMode.indexCardBossActive].key))].CheckUnlock(this.indexDifficultMode))
				{
					PopupManager.Instance.ShowDialog(null, 0, this.ShowBossNote(this.indexDifficultMode), PopupManager.Instance.GetText(Localization0.Locked, null));
					return;
				}
			}
			if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormBossMode_1)
			{
				MenuManager.Instance.tutorial.NextTutorial(5);
			}
			GameMode.Instance.Style = GameMode.GameStyle.SinglPlayer;
			
			AudioClick.Instance.OnClick();
			ProfileManager.bossCurrent = this.cardsBoss[FormBossMode.indexCardBossActive].boss;
			GameMode.Instance.modeBossMode = (GameMode.Mode)this.indexDifficultMode;
			FormLoadout.typeForm = FormLoadout.Type.BossMode;
			MenuManager.Instance.ChangeForm(FormUI.UILoadOut, FormUI.UIBossMode);
		}
		catch
		{
			UnityEngine.Debug.Log("loi roi");
		}
	}

	public void LoadDropList()
	{
		if (ProfileManager.bossModeProfile.bossReward[(int)this.cardsBoss[FormBossMode.indexCardBossActive].boss].StateReward < 2)
		{
			this.objGuarantee.SetActive(true);
			this.cardGuarantee.item = (Item)DataLoader.bossModeData.boss[(int)this.cardsBoss[FormBossMode.indexCardBossActive].boss].reward.id;
			this.cardGuarantee.img_Main.sprite = PopupManager.Instance.sprite_Item[(int)this.cardGuarantee.item];
			this.cardGuarantee.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(this.cardGuarantee.item)];
			this.cardGuarantee.txt_Amount.text = string.Empty + DataLoader.bossModeData.boss[(int)this.cardsBoss[FormBossMode.indexCardBossActive].boss].reward.value;
			this.cardGuarantee.ShowBorderEffect();
		}
		else if (ProfileManager.bossModeProfile.bossReward[(int)this.cardsBoss[FormBossMode.indexCardBossActive].boss].StateReward == 2)
		{
			this.objGuarantee.SetActive(false);
		}
		for (int i = 0; i < this.cardBonus.Length; i++)
		{
			if (i == 0)
			{
				this.cardBonus[i].txt_Description.text = PopupManager.Instance.GetText(Localization0.Normal, null) + " ";
			}
			else if (i == 1)
			{
				this.cardBonus[i].txt_Description.text = PopupManager.Instance.GetText(Localization0.Hard, null) + " ";
			}
			else if (i == 2)
			{
				this.cardBonus[i].txt_Description.text = PopupManager.Instance.GetText(Localization0.Crazy, null) + " ";
			}
			int timesGetGem = DataLoader.bossModeData.boss[(int)this.cardsBoss[FormBossMode.indexCardBossActive].boss].timesGetGem;
			this.cardBonus[i].item = Item.Gem;
			Text txt_Description = this.cardBonus[i].txt_Description;
			string text = txt_Description.text;
			txt_Description.text = string.Concat(new object[]
			{
				text,
				"(",
				ProfileManager.bossModeProfile.bossReward[(int)this.cardsBoss[FormBossMode.indexCardBossActive].boss].GetTimesGemReward(i),
				"/",
				timesGetGem,
				")"
			});
			this.cardBonus[i].txt_Amount.text = "10";
			this.cardBonus[i].ShowBorderEffect();
		}
	}

	public static int indexCardBossActive;

	public int indexDifficultMode;

	[Header("---------------TOP---------------")]
	public RectTransform rect_Top;

	public GameObject obj_cardsBossFather;

	public CardBossMode[] cardsBoss;

	public CardDifficultMode[] cardsDifficultMode;

	public ScrollRect scrollbarBoss;

	[Header("---------------MID---------------")]
	public Text txt_NameBoss;

	public Text txt_LevelUnlock;

	public Text txt_MyPower;

	public Text txt_BossPower;

	public Transform tran_spritesBossFather;

	[Header("---------------BOT---------------")]
	public Image img_Start;

	public Button btn_Start;

	public GameObject obj_BtnActive;

	[Header("---------------Drop---------------")]
	public CardBase cardGuarantee;

	public CardBase[] cardBonus;

	public GameObject objGuarantee;

	public GameObject objBonus;

	public bool onValidate;

	public static bool isRestartBoss;

	public static bool isNextBoss;
}
