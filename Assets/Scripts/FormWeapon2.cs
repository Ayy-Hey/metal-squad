using System;
using System.Collections;
using com.dev.util.SecurityHelper;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class FormWeapon2 : FormBase
{
	private void Awake()
	{
	}

	public override void Show()
	{
		base.Show();
		int num = (int)RemoteConfigFirebase.Instance.GetLongValue(RemoteConfigFirebase.GEM_RANKUP, 100L);
		this.GemRankUp = new SecuredInt[2];
		this.GemRankUp[0] = new SecuredInt(num);
		this.GemRankUp[1] = new SecuredInt(num * 2);
		this.isFirst = true;
		this.nameWeaponSelected = string.Empty;
		float num2 = ((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f;
		if (num2 >= 0.9f)
		{
			this.preview.transform.localPosition = new Vector3(0f, this.preview.transform.localPosition.y, 0f);
			if (ThisPlatform.IsIphoneX)
			{
				this.rect_ListWeapon.localPosition = new Vector3(-5f, this.rect_ListWeapon.localPosition.y, 0f);
			}
			else
			{
				this.rect_ListWeapon.localPosition = new Vector3(-60f, this.rect_ListWeapon.localPosition.y, 0f);
			}
		}
		else
		{
			this.preview.transform.localPosition = new Vector3(-136f * num2 + 136f, this.preview.transform.localPosition.y, 0f);
		}
		this.obj_PopUpUpgrade.SetActive(false);
		bool flag = false;
		Item itemID = ProfileManager.weaponsRifle[0].Gun_Value.GetItemID(ProfileManager.weaponsRifle[0].GetLevelUpgrade() + 1);
		int num3 = ProfileManager.weaponsRifle[0].Gun_Value.ValueUpgrade(ProfileManager.weaponsRifle[0].GetLevelUpgrade() + 1);
		if (itemID != Item.Gold)
		{
			if (itemID == Item.Gem)
			{
				flag = (ProfileManager.userProfile.Ms >= num3);
			}
		}
		else
		{
			flag = (ProfileManager.userProfile.Coin >= num3);
		}
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UpgradeWeapon)
		{
			MenuManager.indexTabSpecial = ETypeWeapon.PRIMARY;
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		else if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.None && PlayerPrefs.GetInt(MenuManager.Instance.tutorial.listTutorialUI[5].keyPlayerPrefs, 0) == 0 && ProfileManager.weaponsRifle[0].GetLevelUpgrade() < 29 && flag)
		{
			MenuManager.Instance.tutorial.StartTutorial(TutorialUIManager.TutorialID.Tut_UpgradeWeapon);
			MenuManager.indexTabSpecial = ETypeWeapon.PRIMARY;
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		this.listMaxPrimary = new float[3];
		this.listMaxSpecial = new float[4];
		this.listMaxKnife = new float[3];
		this.listMaxBomb = new float[3];
		this.GetMaxProperty();
		this.dataGunMain = new InforWeapon[ProfileManager.weaponsRifle.Length];
		this.dataGunSpecial = new InforWeapon[ProfileManager.weaponsSpecial.Length];
		this.dataMelee = new InforWeapon[ProfileManager.melesProfile.Length];
		this.dataBomb = new InforWeapon[ProfileManager.grenadesProfile.Length];
		this.ReloadDataAll();
		for (int i = 0; i < this.cardsTab.Length; i++)
		{
			this.cardsTab[i].obj_Active.SetActive(false);
		}
		for (int j = 0; j < this.cardsWeapon.Length; j++)
		{
			this.cardsWeapon[j].obj_Active.SetActive(false);
		}
		FormWeapon2.indexTab = MenuManager.indexTabSpecial;
		this.ChangeTab((int)FormWeapon2.indexTab);
		this.obj_GroupButton.SetActive(true);
		
		this.isFirst = false;
	}

	public override void OnClose()
	{
		MenuManager.indexTabSpecial = ETypeWeapon.PRIMARY;
		for (int i = 0; i < this.listEffUnlock.Length; i++)
		{
			this.listEffUnlock[i].EndEffectUnlock();
		}
		base.OnClose();
	}

	public void ReloadDataAll()
	{
		for (int i = 0; i < this.dataGunMain.Length; i++)
		{
			this.ReloadData(0, i);
		}
		for (int j = 0; j < this.dataGunSpecial.Length; j++)
		{
			this.ReloadData(1, j);
		}
		for (int k = 0; k < this.dataMelee.Length; k++)
		{
			this.ReloadData(2, k);
		}
		for (int l = 0; l < this.dataBomb.Length; l++)
		{
			this.ReloadData(3, l);
		}
	}

	public void ChangeTab(int index)
	{
		this.cardsTab[(int)FormWeapon2.indexTab].obj_Active.SetActive(false);
		FormWeapon2.indexTab = (ETypeWeapon)index;
		this.cardsTab[(int)FormWeapon2.indexTab].obj_Active.SetActive(true);
		this.cardsWeapon[FormWeapon2.indexWeapon].obj_Active.SetActive(false);
		FormWeapon2.indexWeapon = 0;
		InforWeapon[] arrayData = this.GetArrayData((int)FormWeapon2.indexTab);
		for (int i = 0; i < this.cardsWeapon.Length; i++)
		{
			if (i < arrayData.Length)
			{
				this.cardsWeapon[i].gameObject.SetActive(true);
				this.cardsWeapon[i].txt_Name.text = string.Concat(new object[]
				{
					arrayData[i].nameWeapon,
					" ",
					PopupManager.Instance.GetText(Localization0.Lv, null),
					" ",
					arrayData[i].levelWeapon + 1
				});
				if (FormWeapon2.indexTab == ETypeWeapon.PRIMARY || FormWeapon2.indexTab == ETypeWeapon.SPECIAL)
				{
					this.cardsWeapon[i].objProgressFragment.SetActive(arrayData[i].levelWeapon < 29);
					if (arrayData[i].levelWeapon < 29)
					{
						this.cardsWeapon[i].txtFragment.text = PlayerPrefs.GetInt("metal.squad.frag." + arrayData[i].CurrencyUnlock.ToString(), 0) + "/" + arrayData[i].costRankUp;
						this.cardsWeapon[i].imageProgressFragment.fillAmount = (float)PlayerPrefs.GetInt("metal.squad.frag." + arrayData[i].CurrencyUnlock.ToString(), 0) / (float)arrayData[i].costRankUp;
					}
					else
					{
						this.cardsWeapon[i].txtFragment.text = PlayerPrefs.GetInt("metal.squad.frag." + arrayData[i].CurrencyUnlock.ToString(), 0) + string.Empty;
						this.cardsWeapon[i].imageProgressFragment.fillAmount = 1f;
					}
				}
				else
				{
					this.cardsWeapon[i].objProgressFragment.SetActive(false);
				}
				this.cardsWeapon[i].img_Main.sprite = arrayData[i].spriteWeapon;
				this.cardsWeapon[i].img_Main.rectTransform.sizeDelta = arrayData[i].sizeImage;
				int num = arrayData[i].rankBase + arrayData[i].rankUpped;
				this.cardsWeapon[i].img_BG.sprite = PopupManager.Instance.sprite_BGCardRank[num];
				this.cardsWeapon[i].txt_Name.color = PopupManager.Instance.color_Rank[num];
				this.cardsWeapon[i].img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
				if (index != 3 && index != 2)
				{
					if (!string.IsNullOrEmpty(this.aniRank[arrayData[i].rankBase + arrayData[i].rankUpped]))
					{
						this.cardsWeapon[i].obj_EffectByRank.gameObject.SetActive(true);
						if (this.cardsWeapon[i].obj_EffectByRank.AnimationState == null)
						{
							this.cardsWeapon[i].obj_EffectByRank.Initialize(true);
						}
						this.cardsWeapon[i].obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[arrayData[i].rankBase + arrayData[i].rankUpped], true);
					}
					else
					{
						this.cardsWeapon[i].obj_EffectByRank.gameObject.SetActive(false);
					}
				}
				else
				{
					this.cardsWeapon[i].obj_EffectByRank.gameObject.SetActive(false);
				}
				this.cardsWeapon[i].obj_Lock.SetActive(!arrayData[i].isUnlock);
				this.cardsWeapon[i].obj_Equiped.SetActive(arrayData[i].isEquiped && FormWeapon2.indexTab != ETypeWeapon.SPECIAL);
				if (arrayData[i].isEquiped)
				{
					FormWeapon2.indexWeapon = i;
				}
			}
			else
			{
				this.cardsWeapon[i].gameObject.SetActive(false);
			}
		}
		this.ClickToWeapon(this.cardsWeapon[FormWeapon2.indexWeapon]);
		this.obj_InforUnlockFrag.SetActive(FormWeapon2.indexTab != ETypeWeapon.KNIFE && FormWeapon2.indexTab != ETypeWeapon.GRENADE);
		this.obj_InforRankUp.SetActive(FormWeapon2.indexTab != ETypeWeapon.KNIFE && FormWeapon2.indexTab != ETypeWeapon.GRENADE);
		this.scrollbarWepon.value = (float)(1 - FormWeapon2.indexWeapon / (this.GetArrayData((int)FormWeapon2.indexTab).Length - 1));
		base.StartCoroutine(this.ScrollListCardWeapon());
	}

	private void ClickToWeapon(CardWeapon card)
	{
		int num = FormWeapon2.indexWeapon;
		if (!this.cardsWeapon[card.idCard].obj_Lock.activeSelf)
		{
			this.EquipWeapon(FormWeapon2.indexWeapon, card.idCard);
		}
		this.cardsWeapon[FormWeapon2.indexWeapon].obj_Active.SetActive(false);
		FormWeapon2.indexWeapon = card.idCard;
		this.cardsWeapon[FormWeapon2.indexWeapon].obj_Active.SetActive(true);
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		if (this.nameWeaponSelected != this.cardsWeapon[FormWeapon2.indexWeapon].img_Main.sprite.name)
		{
			this.preview.rambo[ProfileManager.settingProfile.IDChar].OnSetSkinByRank(FormWeapon2.indexTab, FormWeapon2.indexWeapon, data.rankUpped, data.isUnlock);
			this.preview.OnShow(FormWeapon2.indexTab, Mathf.Max(FormWeapon2.indexWeapon, 0), data.levelWeapon, data.rankUpped, data.isUnlock);
			this.nameWeaponSelected = this.cardsWeapon[FormWeapon2.indexWeapon].img_Main.sprite.name;
		}
		this.LoadTableInfor();
		this.OpenUnlockOrUpgrade();
		this.CheckTipsAll();
	}

	private void ReloadData(int indexTab, int indexWeapon)
	{
		float num = 90f;
		float num2 = 220f;
		switch (indexTab)
		{
		case 0:
		{
			this.dataGunMain[indexWeapon] = new InforWeapon();
			this.dataGunMain[indexWeapon].id = indexWeapon;
			this.dataGunMain[indexWeapon].nameWeapon = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.gunName;
			this.dataGunMain[indexWeapon].levelWeapon = ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade();
			this.dataGunMain[indexWeapon].power = ProfileManager.weaponsRifle[indexWeapon].Power();
			if (this.dataGunMain[indexWeapon].levelWeapon < 29)
			{
				this.dataGunMain[indexWeapon].powerNext = ProfileManager.weaponsRifle[indexWeapon].PowerByLevel(this.dataGunMain[indexWeapon].levelWeapon + 1);
			}
			this.dataGunMain[indexWeapon].powerMax = ProfileManager.weaponsRifle[indexWeapon].PowerByLevel(29);
			this.dataGunMain[indexWeapon].rankBase = ProfileManager.weaponsRifle[indexWeapon].GetRankBase();
			this.dataGunMain[indexWeapon].rankUpped = ProfileManager.weaponsRifle[indexWeapon].GetRankUpped();
			this.dataGunMain[indexWeapon].passiveDesc = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.PassiveDesc;
			this.dataGunMain[indexWeapon].isUnlock = ProfileManager.weaponsRifle[indexWeapon].GetGunBuy();
			if (this.dataGunMain[indexWeapon].isUnlock)
			{
				this.dataGunMain[indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunMain[this.dataGunMain[indexWeapon].rankUpped].Sprites[indexWeapon];
				if (this.dataGunMain[indexWeapon].rankUpped + 1 <= 2)
				{
					this.dataGunMain[indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_GunMain[this.dataGunMain[indexWeapon].rankUpped + 1].Sprites[indexWeapon];
				}
			}
			else
			{
				this.dataGunMain[indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunMain[2].Sprites[indexWeapon];
				this.dataGunMain[indexWeapon].spriteWeaponNext = this.dataGunMain[indexWeapon].spriteWeapon;
			}
			float num3 = this.dataGunMain[indexWeapon].spriteWeapon.bounds.size.x / this.dataGunMain[indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num3 <= num2)
			{
				this.dataGunMain[indexWeapon].sizeImage = new Vector2(num3, num);
			}
			else
			{
				this.dataGunMain[indexWeapon].sizeImage = new Vector2(num2, this.dataGunMain[indexWeapon].spriteWeapon.bounds.size.y / this.dataGunMain[indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			this.dataGunMain[indexWeapon].isVip = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.isVip;
			this.dataGunMain[indexWeapon].isEquiped = (indexWeapon == ProfileManager.rifleGunCurrentId.Data.Value);
			this.dataGunMain[indexWeapon].costRankUp = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, this.dataGunMain[indexWeapon].rankUpped)];
			this.dataGunMain[indexWeapon].CurrencyUnlock = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CurrencyUnlock;
			this.dataGunMain[indexWeapon].CurrencyRankUp = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CurrencyRankUp;
			this.dataGunMain[indexWeapon].costUnlockByGold = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.ValueUpgrade(0);
			this.dataGunMain[indexWeapon].costUnlockByGem = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.SecuredGemPrice.Value;
			if (this.dataGunMain[indexWeapon].levelWeapon < 29)
			{
				this.dataGunMain[indexWeapon].SetCostUpgrade(ProfileManager.weaponsRifle[indexWeapon].Gun_Value.GetItemID(this.dataGunMain[indexWeapon].levelWeapon + 1), ProfileManager.weaponsRifle[indexWeapon].Gun_Value.ValueUpgrade(this.dataGunMain[indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataGunMain[indexWeapon].SetCostUpgrade(ProfileManager.weaponsRifle[indexWeapon].Gun_Value.GetItemID(this.dataGunMain[indexWeapon].levelWeapon + 1), -1);
			}
			this.dataGunMain[indexWeapon].campaignUpgrade = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.campaignUpgrade[this.dataGunMain[indexWeapon].levelWeapon];
			this.dataGunMain[indexWeapon].properties = new PropertiesWeapon[3];
			this.dataGunMain[indexWeapon].properties[0] = new PropertiesWeapon();
			this.dataGunMain[indexWeapon].properties[0].title = PopupManager.Instance.GetText(Localization0.Damage, null);
			this.dataGunMain[indexWeapon].properties[0].valueCurrent = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[2][this.dataGunMain[indexWeapon].levelWeapon];
			this.dataGunMain[indexWeapon].properties[0].valueNextRank = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[2][Mathf.Min(this.dataGunMain[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunMain[indexWeapon].properties[0].valueMineMax = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[2][29];
			this.dataGunMain[indexWeapon].properties[0].valueMax = this.listMaxPrimary[0];
			this.dataGunMain[indexWeapon].properties[1] = new PropertiesWeapon();
			this.dataGunMain[indexWeapon].properties[1].title = PopupManager.Instance.GetText(Localization0.Firerate, null);
			this.dataGunMain[indexWeapon].properties[1].valueCurrent = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[3][this.dataGunMain[indexWeapon].levelWeapon];
			this.dataGunMain[indexWeapon].properties[1].valueNextRank = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[3][Mathf.Min(this.dataGunMain[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunMain[indexWeapon].properties[1].valueMineMax = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[3][29];
			this.dataGunMain[indexWeapon].properties[1].valueMax = this.listMaxPrimary[1];
			this.dataGunMain[indexWeapon].properties[2] = new PropertiesWeapon();
			this.dataGunMain[indexWeapon].properties[2].title = PopupManager.Instance.GetText(Localization0.Critical, null);
			this.dataGunMain[indexWeapon].properties[2].valueCurrent = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[4][this.dataGunMain[indexWeapon].levelWeapon];
			this.dataGunMain[indexWeapon].properties[2].valueNextRank = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[4][Mathf.Min(this.dataGunMain[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunMain[indexWeapon].properties[2].valueMineMax = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[4][29];
			this.dataGunMain[indexWeapon].properties[2].valueMax = this.listMaxPrimary[2];
			if (this.dataGunMain[indexWeapon].levelWeapon < 29)
			{
				this.dataGunMain[indexWeapon].properties[0].valueNext = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[2][this.dataGunMain[indexWeapon].levelWeapon + 1];
				this.dataGunMain[indexWeapon].properties[1].valueNext = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[3][this.dataGunMain[indexWeapon].levelWeapon + 1];
				this.dataGunMain[indexWeapon].properties[2].valueNext = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.properties[4][this.dataGunMain[indexWeapon].levelWeapon + 1];
			}
			break;
		}
		case 1:
		{
			this.dataGunSpecial[indexWeapon] = new InforWeapon();
			this.dataGunSpecial[indexWeapon].id = indexWeapon;
			this.dataGunSpecial[indexWeapon].nameWeapon = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.gunName;
			this.dataGunSpecial[indexWeapon].levelWeapon = ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade();
			this.dataGunSpecial[indexWeapon].power = ProfileManager.weaponsSpecial[indexWeapon].Power();
			if (this.dataGunSpecial[indexWeapon].levelWeapon < 29)
			{
				this.dataGunSpecial[indexWeapon].powerNext = ProfileManager.weaponsSpecial[indexWeapon].PowerByLevel(this.dataGunSpecial[indexWeapon].levelWeapon + 1);
			}
			this.dataGunSpecial[indexWeapon].powerMax = ProfileManager.weaponsSpecial[indexWeapon].PowerByLevel(29);
			this.dataGunSpecial[indexWeapon].rankBase = ProfileManager.weaponsSpecial[indexWeapon].GetRankBase();
			this.dataGunSpecial[indexWeapon].rankUpped = ProfileManager.weaponsSpecial[indexWeapon].GetRankUpped();
			this.dataGunSpecial[indexWeapon].passiveDesc = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.PassiveDesc;
			this.dataGunSpecial[indexWeapon].isUnlock = ProfileManager.weaponsSpecial[indexWeapon].GetGunBuy();
			if (this.dataGunSpecial[indexWeapon].isUnlock)
			{
				this.dataGunSpecial[indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunSpecial[this.dataGunSpecial[indexWeapon].rankUpped].Sprites[indexWeapon];
				if (this.dataGunSpecial[indexWeapon].rankUpped + 1 <= 2)
				{
					this.dataGunSpecial[indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_GunSpecial[this.dataGunSpecial[indexWeapon].rankUpped + 1].Sprites[indexWeapon];
				}
			}
			else
			{
				this.dataGunSpecial[indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunSpecial[2].Sprites[indexWeapon];
				this.dataGunSpecial[indexWeapon].spriteWeaponNext = this.dataGunSpecial[indexWeapon].spriteWeapon;
			}
			float num4 = this.dataGunSpecial[indexWeapon].spriteWeapon.bounds.size.x / this.dataGunSpecial[indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num4 <= num2)
			{
				this.dataGunSpecial[indexWeapon].sizeImage = new Vector2(num4, num);
			}
			else
			{
				this.dataGunSpecial[indexWeapon].sizeImage = new Vector2(num2, this.dataGunSpecial[indexWeapon].spriteWeapon.bounds.size.y / this.dataGunSpecial[indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			this.dataGunSpecial[indexWeapon].isVip = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.isVip;
			this.dataGunSpecial[indexWeapon].costRankUp = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, this.dataGunMain[indexWeapon].rankUpped)];
			this.dataGunSpecial[indexWeapon].CurrencyUnlock = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CurrencyUnlock;
			this.dataGunSpecial[indexWeapon].CurrencyRankUp = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CurrencyRankUp;
			this.dataGunSpecial[indexWeapon].costUnlockByGold = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(0);
			this.dataGunSpecial[indexWeapon].costUnlockByGem = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.SecuredGemPrice.Value;
			if (this.dataGunSpecial[indexWeapon].levelWeapon < 29)
			{
				this.dataGunSpecial[indexWeapon].SetCostUpgrade(ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.GetItemID(this.dataGunSpecial[indexWeapon].levelWeapon + 1), ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(this.dataGunSpecial[indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataGunSpecial[indexWeapon].SetCostUpgrade(ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.GetItemID(this.dataGunSpecial[indexWeapon].levelWeapon + 1), -1);
			}
			this.dataGunSpecial[indexWeapon].campaignUpgrade = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.campaignUpgrade[this.dataGunSpecial[indexWeapon].levelWeapon];
			this.dataGunSpecial[indexWeapon].properties = new PropertiesWeapon[4];
			this.dataGunSpecial[indexWeapon].properties[0] = new PropertiesWeapon();
			this.dataGunSpecial[indexWeapon].properties[0].title = PopupManager.Instance.GetText(Localization0.Damage, null);
			this.dataGunSpecial[indexWeapon].properties[0].valueCurrent = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[2][this.dataGunSpecial[indexWeapon].levelWeapon];
			this.dataGunSpecial[indexWeapon].properties[0].valueNextRank = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[2][Mathf.Min(this.dataGunSpecial[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[indexWeapon].properties[0].valueMineMax = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[2][29];
			this.dataGunSpecial[indexWeapon].properties[0].valueMax = this.listMaxSpecial[0];
			this.dataGunSpecial[indexWeapon].properties[1] = new PropertiesWeapon();
			this.dataGunSpecial[indexWeapon].properties[1].title = PopupManager.Instance.GetText(Localization0.Firerate, null);
			this.dataGunSpecial[indexWeapon].properties[1].valueCurrent = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[3][this.dataGunSpecial[indexWeapon].levelWeapon];
			this.dataGunSpecial[indexWeapon].properties[1].valueNextRank = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[3][Mathf.Min(this.dataGunSpecial[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[indexWeapon].properties[1].valueMineMax = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[3][29];
			this.dataGunSpecial[indexWeapon].properties[1].valueMax = this.listMaxSpecial[1];
			this.dataGunSpecial[indexWeapon].properties[2] = new PropertiesWeapon();
			this.dataGunSpecial[indexWeapon].properties[2].title = PopupManager.Instance.GetText(Localization0.Critical, null);
			this.dataGunSpecial[indexWeapon].properties[2].valueCurrent = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[4][this.dataGunSpecial[indexWeapon].levelWeapon];
			this.dataGunSpecial[indexWeapon].properties[2].valueNextRank = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[4][Mathf.Min(this.dataGunSpecial[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[indexWeapon].properties[2].valueMineMax = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[4][29];
			this.dataGunSpecial[indexWeapon].properties[2].valueMax = this.listMaxSpecial[2];
			this.dataGunSpecial[indexWeapon].properties[3] = new PropertiesWeapon();
			this.dataGunSpecial[indexWeapon].properties[3].title = PopupManager.Instance.GetText(Localization0.Ammo, null);
			this.dataGunSpecial[indexWeapon].properties[3].valueCurrent = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[1][this.dataGunSpecial[indexWeapon].levelWeapon];
			this.dataGunSpecial[indexWeapon].properties[3].valueNextRank = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[1][Mathf.Min(this.dataGunSpecial[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[indexWeapon].properties[3].valueMineMax = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[1][29];
			this.dataGunSpecial[indexWeapon].properties[3].valueMax = this.listMaxSpecial[3];
			if (this.dataGunSpecial[indexWeapon].levelWeapon < 29)
			{
				this.dataGunSpecial[indexWeapon].properties[0].valueNext = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[2][this.dataGunSpecial[indexWeapon].levelWeapon + 1];
				this.dataGunSpecial[indexWeapon].properties[1].valueNext = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[3][this.dataGunSpecial[indexWeapon].levelWeapon + 1];
				this.dataGunSpecial[indexWeapon].properties[2].valueNext = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[4][this.dataGunSpecial[indexWeapon].levelWeapon + 1];
				this.dataGunSpecial[indexWeapon].properties[3].valueNext = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.properties[1][this.dataGunSpecial[indexWeapon].levelWeapon + 1];
			}
			break;
		}
		case 2:
		{
			this.dataMelee[indexWeapon] = new InforWeapon();
			this.dataMelee[indexWeapon].id = indexWeapon;
			this.dataMelee[indexWeapon].nameWeapon = ProfileManager.melesProfile[indexWeapon].NAME;
			this.dataMelee[indexWeapon].levelWeapon = ProfileManager.melesProfile[indexWeapon].LevelUpGradeMelee;
			this.dataMelee[indexWeapon].power = ProfileManager.melesProfile[indexWeapon].Power();
			if (this.dataMelee[indexWeapon].levelWeapon < 29)
			{
				this.dataMelee[indexWeapon].powerNext = ProfileManager.melesProfile[indexWeapon].PowerByLevel(this.dataMelee[indexWeapon].levelWeapon + 1);
			}
			this.dataMelee[indexWeapon].powerMax = ProfileManager.melesProfile[indexWeapon].PowerByLevel(29);
			this.dataMelee[indexWeapon].rankBase = ProfileManager.melesProfile[indexWeapon].RankBase;
			this.dataMelee[indexWeapon].rankUpped = 0;
			this.dataMelee[indexWeapon].spriteWeapon = PopupManager.Instance.sprite_Melee[indexWeapon];
			this.dataMelee[indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_Melee[indexWeapon];
			float num5 = this.dataMelee[indexWeapon].spriteWeapon.bounds.size.x / this.dataMelee[indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num5 <= num2)
			{
				this.dataMelee[indexWeapon].sizeImage = new Vector2(num5, num);
			}
			else
			{
				this.dataMelee[indexWeapon].sizeImage = new Vector2(num2, this.dataMelee[indexWeapon].spriteWeapon.bounds.size.y / this.dataMelee[indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			this.dataMelee[indexWeapon].isUnlock = ProfileManager.melesProfile[indexWeapon].Unlock;
			this.dataMelee[indexWeapon].isEquiped = (indexWeapon == ProfileManager.meleCurrentId.Data.Value);
			this.dataMelee[indexWeapon].costUnlockByGem = ProfileManager.melesProfile[indexWeapon].ValueUpgrade(0);
			if (this.dataMelee[indexWeapon].levelWeapon < 29)
			{
				this.dataMelee[indexWeapon].SetCostUpgrade(ProfileManager.melesProfile[indexWeapon].GetItemID(this.dataMelee[indexWeapon].levelWeapon + 1), ProfileManager.melesProfile[indexWeapon].ValueUpgrade(this.dataMelee[indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataMelee[indexWeapon].SetCostUpgrade(ProfileManager.melesProfile[indexWeapon].GetItemID(this.dataMelee[indexWeapon].levelWeapon + 1), -1);
			}
			this.dataMelee[indexWeapon].properties = new PropertiesWeapon[3];
			this.dataMelee[indexWeapon].properties[0] = new PropertiesWeapon();
			this.dataMelee[indexWeapon].properties[0].title = PopupManager.Instance.GetText(Localization0.Damage, null);
			this.dataMelee[indexWeapon].properties[0].valueCurrent = ProfileManager.melesProfile[indexWeapon].SecuredDamaged[this.dataMelee[indexWeapon].levelWeapon].Value;
			this.dataMelee[indexWeapon].properties[0].valueNextRank = ProfileManager.melesProfile[indexWeapon].SecuredDamaged[Mathf.Min(this.dataMelee[indexWeapon].rankUpped + 1, 2) * 10 + 9].Value;
			this.dataMelee[indexWeapon].properties[0].valueMineMax = ProfileManager.melesProfile[indexWeapon].SecuredDamaged[29].Value;
			this.dataMelee[indexWeapon].properties[0].valueMax = this.listMaxKnife[0];
			this.dataMelee[indexWeapon].properties[1] = new PropertiesWeapon();
			this.dataMelee[indexWeapon].properties[1].title = PopupManager.Instance.GetText(Localization0.Range, null);
			this.dataMelee[indexWeapon].properties[1].valueCurrent = (float)ProfileManager.melesProfile[indexWeapon].range[this.dataMelee[indexWeapon].levelWeapon];
			this.dataMelee[indexWeapon].properties[1].valueNextRank = (float)ProfileManager.melesProfile[indexWeapon].range[Mathf.Min(this.dataMelee[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataMelee[indexWeapon].properties[1].valueMineMax = (float)ProfileManager.melesProfile[indexWeapon].range[29];
			this.dataMelee[indexWeapon].properties[1].valueMax = this.listMaxKnife[1];
			this.dataMelee[indexWeapon].properties[2] = new PropertiesWeapon();
			this.dataMelee[indexWeapon].properties[2].title = PopupManager.Instance.GetText(Localization0.Speed, null);
			this.dataMelee[indexWeapon].properties[2].valueCurrent = (float)ProfileManager.melesProfile[indexWeapon].speed[this.dataMelee[indexWeapon].levelWeapon];
			this.dataMelee[indexWeapon].properties[2].valueNextRank = (float)ProfileManager.melesProfile[indexWeapon].speed[Mathf.Min(this.dataMelee[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataMelee[indexWeapon].properties[2].valueMineMax = (float)ProfileManager.melesProfile[indexWeapon].speed[29];
			this.dataMelee[indexWeapon].properties[2].valueMax = this.listMaxKnife[2];
			if (this.dataMelee[indexWeapon].levelWeapon < 29)
			{
				this.dataMelee[indexWeapon].properties[0].valueNext = ProfileManager.melesProfile[indexWeapon].SecuredDamaged[this.dataMelee[indexWeapon].levelWeapon + 1].Value;
				this.dataMelee[indexWeapon].properties[1].valueNext = (float)ProfileManager.melesProfile[indexWeapon].range[this.dataMelee[indexWeapon].levelWeapon + 1];
				this.dataMelee[indexWeapon].properties[2].valueNext = (float)ProfileManager.melesProfile[indexWeapon].speed[this.dataMelee[indexWeapon].levelWeapon + 1];
			}
			break;
		}
		case 3:
		{
			this.dataBomb[indexWeapon] = new InforWeapon();
			this.dataBomb[indexWeapon].id = indexWeapon;
			this.dataBomb[indexWeapon].nameWeapon = ProfileManager.grenadesProfile[indexWeapon].NAME_GRENADE;
			this.dataBomb[indexWeapon].levelWeapon = ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade;
			this.dataBomb[indexWeapon].power = ProfileManager.grenadesProfile[indexWeapon].Power();
			if (this.dataBomb[indexWeapon].levelWeapon < 29)
			{
				this.dataBomb[indexWeapon].powerNext = ProfileManager.grenadesProfile[indexWeapon].PowerByLevel(this.dataBomb[indexWeapon].levelWeapon + 1);
			}
			this.dataBomb[indexWeapon].powerMax = ProfileManager.grenadesProfile[indexWeapon].PowerByLevel(29);
			this.dataBomb[indexWeapon].rankBase = ProfileManager.grenadesProfile[indexWeapon].RankBase;
			this.dataBomb[indexWeapon].rankUpped = this.dataBomb[indexWeapon].levelWeapon / 10;
			this.dataBomb[indexWeapon].spriteWeapon = PopupManager.Instance.sprite_Grenade[this.dataBomb[indexWeapon].rankUpped].Sprites[indexWeapon];
			this.dataBomb[indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_Grenade[Mathf.Min(this.dataBomb[indexWeapon].rankUpped + 1, 2)].Sprites[indexWeapon];
			float num6 = this.dataBomb[indexWeapon].spriteWeapon.bounds.size.x / this.dataBomb[indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num6 <= num2)
			{
				this.dataBomb[indexWeapon].sizeImage = new Vector2(num6, num);
			}
			else
			{
				this.dataBomb[indexWeapon].sizeImage = new Vector2(num2, this.dataBomb[indexWeapon].spriteWeapon.bounds.size.y / this.dataBomb[indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			this.dataBomb[indexWeapon].isUnlock = true;
			this.dataBomb[indexWeapon].isEquiped = (indexWeapon == ProfileManager.grenadeCurrentId.Data.Value);
			if (ProfileManager.grenadesProfile[indexWeapon].TotalBomb < 0)
			{
				ProfileManager.grenadesProfile[indexWeapon].TotalBomb = 0;
			}
			this.dataBomb[indexWeapon].amountCurrent = ProfileManager.grenadesProfile[indexWeapon].TotalBomb;
			if (this.dataBomb[indexWeapon].levelWeapon < 29)
			{
				this.dataBomb[indexWeapon].SetCostUpgrade(ProfileManager.grenadesProfile[indexWeapon].GetItemID(this.dataBomb[indexWeapon].levelWeapon + 1), ProfileManager.grenadesProfile[indexWeapon].ValueUpgrade(this.dataBomb[indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataBomb[indexWeapon].SetCostUpgrade(ProfileManager.grenadesProfile[indexWeapon].GetItemID(this.dataBomb[indexWeapon].levelWeapon + 1), -1);
			}
			this.dataBomb[indexWeapon].costBullet = ProfileManager.grenadesProfile[indexWeapon].SecuredPrice;
			this.dataBomb[indexWeapon].properties = new PropertiesWeapon[3];
			this.dataBomb[indexWeapon].properties[0] = new PropertiesWeapon();
			this.dataBomb[indexWeapon].properties[0].title = PopupManager.Instance.GetText(Localization0.Damage, null);
			this.dataBomb[indexWeapon].properties[0].valueCurrent = ProfileManager.grenadesProfile[indexWeapon].options[0][this.dataBomb[indexWeapon].levelWeapon];
			this.dataBomb[indexWeapon].properties[0].valueNextRank = ProfileManager.grenadesProfile[indexWeapon].options[0][Mathf.Min(this.dataBomb[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataBomb[indexWeapon].properties[0].valueMineMax = ProfileManager.grenadesProfile[indexWeapon].options[0][29];
			this.dataBomb[indexWeapon].properties[0].valueMax = this.listMaxBomb[0];
			this.dataBomb[indexWeapon].properties[1] = new PropertiesWeapon();
			this.dataBomb[indexWeapon].properties[1].title = PopupManager.Instance.GetText(Localization0.Range, null);
			this.dataBomb[indexWeapon].properties[1].valueCurrent = ProfileManager.grenadesProfile[indexWeapon].options[1][this.dataBomb[indexWeapon].levelWeapon];
			this.dataBomb[indexWeapon].properties[1].valueNextRank = ProfileManager.grenadesProfile[indexWeapon].options[1][Mathf.Min(this.dataBomb[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataBomb[indexWeapon].properties[1].valueMineMax = ProfileManager.grenadesProfile[indexWeapon].options[1][29];
			this.dataBomb[indexWeapon].properties[1].valueMax = this.listMaxBomb[1];
			this.dataBomb[indexWeapon].properties[2] = new PropertiesWeapon();
			this.dataBomb[indexWeapon].properties[2].title = PopupManager.Instance.GetText(Localization0.Time, null);
			this.dataBomb[indexWeapon].properties[2].valueCurrent = ProfileManager.grenadesProfile[indexWeapon].options[2][this.dataBomb[indexWeapon].levelWeapon];
			this.dataBomb[indexWeapon].properties[2].valueNextRank = ProfileManager.grenadesProfile[indexWeapon].options[2][Mathf.Min(this.dataBomb[indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataBomb[indexWeapon].properties[2].valueMineMax = ProfileManager.grenadesProfile[indexWeapon].options[2][29];
			this.dataBomb[indexWeapon].properties[2].valueMax = this.listMaxBomb[2];
			if (this.dataBomb[indexWeapon].levelWeapon < 29)
			{
				this.dataBomb[indexWeapon].properties[0].valueNext = ProfileManager.grenadesProfile[indexWeapon].options[0][this.dataBomb[indexWeapon].levelWeapon + 1];
				this.dataBomb[indexWeapon].properties[1].valueNext = ProfileManager.grenadesProfile[indexWeapon].options[1][this.dataBomb[indexWeapon].levelWeapon + 1];
				this.dataBomb[indexWeapon].properties[2].valueNext = ProfileManager.grenadesProfile[indexWeapon].options[2][this.dataBomb[indexWeapon].levelWeapon + 1];
			}
			break;
		}
		default:
			UnityEngine.Debug.Log("Lỗi ReloadData() trong FormWeapon");
			break;
		}
	}

	private void SetData(bool isUnlock, int levelUpgrade, int rankUpgrade, int amountBullet)
	{
		float num = 90f;
		float num2 = 220f;
		switch (FormWeapon2.indexTab)
		{
		case ETypeWeapon.PRIMARY:
		{
			ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].SetLevelUpgrade(levelUpgrade);
			if (isUnlock)
			{
				PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.GunMain, FormWeapon2.indexWeapon), 1, base.name + "_Unlock", null);
			}
			this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon = levelUpgrade;
			this.dataGunMain[FormWeapon2.indexWeapon].power = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Power();
			if (this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataGunMain[FormWeapon2.indexWeapon].powerNext = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].PowerByLevel(this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1);
			}
			ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].SetRankUpped(rankUpgrade);
			this.dataGunMain[FormWeapon2.indexWeapon].rankUpped = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].GetRankUpped();
			this.dataGunMain[FormWeapon2.indexWeapon].isUnlock = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].GetGunBuy();
			if (this.dataGunMain[FormWeapon2.indexWeapon].isUnlock)
			{
				this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunMain[this.dataGunMain[FormWeapon2.indexWeapon].rankUpped].Sprites[FormWeapon2.indexWeapon];
				if (this.dataGunMain[FormWeapon2.indexWeapon].rankUpped + 1 <= 2)
				{
					this.dataGunMain[FormWeapon2.indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_GunMain[this.dataGunMain[FormWeapon2.indexWeapon].rankUpped + 1].Sprites[FormWeapon2.indexWeapon];
				}
			}
			else
			{
				this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunMain[2].Sprites[FormWeapon2.indexWeapon];
				this.dataGunMain[FormWeapon2.indexWeapon].spriteWeaponNext = this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon;
			}
			float num3 = this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x / this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num3 <= num2)
			{
				this.dataGunMain[FormWeapon2.indexWeapon].sizeImage = new Vector2(num3, num);
			}
			else
			{
				this.dataGunMain[FormWeapon2.indexWeapon].sizeImage = new Vector2(num2, this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y / this.dataGunMain[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			if (this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataGunMain[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.GetItemID(this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1), ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.ValueUpgrade(this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataGunMain[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.GetItemID(this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1), -1);
			}
			this.dataGunMain[FormWeapon2.indexWeapon].campaignUpgrade = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.campaignUpgrade[this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunMain[FormWeapon2.indexWeapon].properties[0].valueCurrent = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[2][this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunMain[FormWeapon2.indexWeapon].properties[0].valueNextRank = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[2][Mathf.Min(this.dataGunMain[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunMain[FormWeapon2.indexWeapon].properties[1].valueCurrent = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[3][this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunMain[FormWeapon2.indexWeapon].properties[1].valueNextRank = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[3][Mathf.Min(this.dataGunMain[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunMain[FormWeapon2.indexWeapon].properties[2].valueCurrent = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[4][this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunMain[FormWeapon2.indexWeapon].properties[2].valueNextRank = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[4][Mathf.Min(this.dataGunMain[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			if (this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataGunMain[FormWeapon2.indexWeapon].properties[0].valueNext = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[2][this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataGunMain[FormWeapon2.indexWeapon].properties[1].valueNext = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[3][this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataGunMain[FormWeapon2.indexWeapon].properties[2].valueNext = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.properties[4][this.dataGunMain[FormWeapon2.indexWeapon].levelWeapon + 1];
			}
			break;
		}
		case ETypeWeapon.SPECIAL:
		{
			ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].SetLevelUpgrade(levelUpgrade);
			if (isUnlock)
			{
				PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.GunSpecial, FormWeapon2.indexWeapon), 1, base.name + "_Unlock", null);
			}
			this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon = levelUpgrade;
			this.dataGunSpecial[FormWeapon2.indexWeapon].power = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Power();
			if (this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].powerNext = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].PowerByLevel(this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1);
			}
			ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].SetRankUpped(rankUpgrade);
			this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].GetRankUpped();
			this.dataGunSpecial[FormWeapon2.indexWeapon].isUnlock = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].GetGunBuy();
			if (this.dataGunSpecial[FormWeapon2.indexWeapon].isUnlock)
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunSpecial[this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped].Sprites[FormWeapon2.indexWeapon];
				if (this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped + 1 <= 2)
				{
					this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_GunSpecial[this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped + 1].Sprites[FormWeapon2.indexWeapon];
				}
			}
			else
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon = PopupManager.Instance.sprite_GunSpecial[2].Sprites[FormWeapon2.indexWeapon];
				this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeaponNext = this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon;
			}
			float num4 = this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x / this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num4 <= num2)
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].sizeImage = new Vector2(num4, num);
			}
			else
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].sizeImage = new Vector2(num2, this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y / this.dataGunSpecial[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			if (this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.GetItemID(this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1), ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.ValueUpgrade(this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.GetItemID(this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1), -1);
			}
			this.dataGunSpecial[FormWeapon2.indexWeapon].campaignUpgrade = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.campaignUpgrade[this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[0].valueCurrent = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[2][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[0].valueNextRank = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[2][Mathf.Min(this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[1].valueCurrent = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[3][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[1].valueNextRank = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[3][Mathf.Min(this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[2].valueCurrent = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[4][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[2].valueNextRank = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[4][Mathf.Min(this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[3].valueCurrent = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[1][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon];
			this.dataGunSpecial[FormWeapon2.indexWeapon].properties[3].valueNextRank = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[1][Mathf.Min(this.dataGunSpecial[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			if (this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataGunSpecial[FormWeapon2.indexWeapon].properties[0].valueNext = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[2][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataGunSpecial[FormWeapon2.indexWeapon].properties[1].valueNext = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[3][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataGunSpecial[FormWeapon2.indexWeapon].properties[2].valueNext = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[4][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataGunSpecial[FormWeapon2.indexWeapon].properties[3].valueNext = ProfileManager.weaponsSpecial[FormWeapon2.indexWeapon].Gun_Value.properties[1][this.dataGunSpecial[FormWeapon2.indexWeapon].levelWeapon + 1];
			}
			break;
		}
		case ETypeWeapon.KNIFE:
		{
			ProfileManager.melesProfile[FormWeapon2.indexWeapon].LevelUpGradeMelee = levelUpgrade;
			PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Melee, FormWeapon2.indexWeapon), 1, base.name + "_Unlock", null);
			this.dataMelee[FormWeapon2.indexWeapon].spriteWeapon = PopupManager.Instance.sprite_Melee[FormWeapon2.indexWeapon];
			float num5 = this.dataMelee[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x / this.dataMelee[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num5 <= num2)
			{
				this.dataMelee[FormWeapon2.indexWeapon].sizeImage = new Vector2(num5, num);
			}
			else
			{
				this.dataMelee[FormWeapon2.indexWeapon].sizeImage = new Vector2(num2, this.dataMelee[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y / this.dataMelee[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			this.dataMelee[FormWeapon2.indexWeapon].levelWeapon = levelUpgrade;
			this.dataMelee[FormWeapon2.indexWeapon].power = ProfileManager.melesProfile[FormWeapon2.indexWeapon].Power();
			if (this.dataMelee[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataMelee[FormWeapon2.indexWeapon].powerNext = ProfileManager.melesProfile[FormWeapon2.indexWeapon].PowerByLevel(this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1);
			}
			this.dataMelee[FormWeapon2.indexWeapon].isUnlock = ProfileManager.melesProfile[FormWeapon2.indexWeapon].Unlock;
			if (this.dataMelee[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataMelee[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.melesProfile[FormWeapon2.indexWeapon].GetItemID(this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1), ProfileManager.melesProfile[FormWeapon2.indexWeapon].ValueUpgrade(this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataMelee[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.melesProfile[FormWeapon2.indexWeapon].GetItemID(this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1), -1);
			}
			this.dataMelee[FormWeapon2.indexWeapon].properties[0].valueCurrent = ProfileManager.melesProfile[FormWeapon2.indexWeapon].SecuredDamaged[this.dataMelee[FormWeapon2.indexWeapon].levelWeapon].Value;
			this.dataMelee[FormWeapon2.indexWeapon].properties[0].valueNextRank = ProfileManager.melesProfile[FormWeapon2.indexWeapon].SecuredDamaged[Mathf.Min(this.dataMelee[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9].Value;
			this.dataMelee[FormWeapon2.indexWeapon].properties[1].valueCurrent = (float)ProfileManager.melesProfile[FormWeapon2.indexWeapon].range[this.dataMelee[FormWeapon2.indexWeapon].levelWeapon];
			this.dataMelee[FormWeapon2.indexWeapon].properties[1].valueNextRank = (float)ProfileManager.melesProfile[FormWeapon2.indexWeapon].range[Mathf.Min(this.dataMelee[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataMelee[FormWeapon2.indexWeapon].properties[2].valueCurrent = (float)ProfileManager.melesProfile[FormWeapon2.indexWeapon].speed[this.dataMelee[FormWeapon2.indexWeapon].levelWeapon];
			this.dataMelee[FormWeapon2.indexWeapon].properties[2].valueNextRank = (float)ProfileManager.melesProfile[FormWeapon2.indexWeapon].speed[Mathf.Min(this.dataMelee[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			if (this.dataMelee[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataMelee[FormWeapon2.indexWeapon].properties[0].valueNext = ProfileManager.melesProfile[FormWeapon2.indexWeapon].SecuredDamaged[this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1].Value;
				this.dataMelee[FormWeapon2.indexWeapon].properties[1].valueNext = (float)ProfileManager.melesProfile[FormWeapon2.indexWeapon].range[this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataMelee[FormWeapon2.indexWeapon].properties[2].valueNext = (float)ProfileManager.melesProfile[FormWeapon2.indexWeapon].speed[this.dataMelee[FormWeapon2.indexWeapon].levelWeapon + 1];
			}
			break;
		}
		case ETypeWeapon.GRENADE:
		{
			ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].LevelUpGrade = levelUpgrade;
			if (amountBullet < 0)
			{
				amountBullet = 0;
			}
			ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].TotalBomb = amountBullet;
			this.dataBomb[FormWeapon2.indexWeapon].levelWeapon = levelUpgrade;
			this.dataBomb[FormWeapon2.indexWeapon].power = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].Power();
			if (this.dataBomb[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataBomb[FormWeapon2.indexWeapon].powerNext = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].PowerByLevel(this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1);
			}
			this.dataBomb[FormWeapon2.indexWeapon].rankUpped = this.dataBomb[FormWeapon2.indexWeapon].levelWeapon / 10;
			this.dataBomb[FormWeapon2.indexWeapon].spriteWeapon = PopupManager.Instance.sprite_Grenade[this.dataBomb[FormWeapon2.indexWeapon].rankUpped].Sprites[FormWeapon2.indexWeapon];
			this.dataBomb[FormWeapon2.indexWeapon].spriteWeaponNext = PopupManager.Instance.sprite_Grenade[Mathf.Min(this.dataBomb[FormWeapon2.indexWeapon].rankUpped + 1, 2)].Sprites[FormWeapon2.indexWeapon];
			float num6 = this.dataBomb[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x / this.dataBomb[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y * num;
			if (num6 <= num2)
			{
				this.dataBomb[FormWeapon2.indexWeapon].sizeImage = new Vector2(num6, num);
			}
			else
			{
				this.dataBomb[FormWeapon2.indexWeapon].sizeImage = new Vector2(num2, this.dataBomb[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.y / this.dataBomb[FormWeapon2.indexWeapon].spriteWeapon.bounds.size.x * num2);
			}
			this.dataBomb[FormWeapon2.indexWeapon].amountCurrent = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].TotalBomb;
			if (this.dataBomb[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataBomb[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].GetItemID(this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1), ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].ValueUpgrade(this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1));
			}
			else
			{
				this.dataBomb[FormWeapon2.indexWeapon].SetCostUpgrade(ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].GetItemID(this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1), -1);
			}
			this.dataBomb[FormWeapon2.indexWeapon].costBullet = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].SecuredPrice;
			this.dataBomb[FormWeapon2.indexWeapon].properties[0].valueCurrent = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[0][this.dataBomb[FormWeapon2.indexWeapon].levelWeapon];
			this.dataBomb[FormWeapon2.indexWeapon].properties[0].valueNextRank = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[0][Mathf.Min(this.dataBomb[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataBomb[FormWeapon2.indexWeapon].properties[1].valueCurrent = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[1][this.dataBomb[FormWeapon2.indexWeapon].levelWeapon];
			this.dataBomb[FormWeapon2.indexWeapon].properties[1].valueNextRank = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[1][Mathf.Min(this.dataBomb[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			this.dataBomb[FormWeapon2.indexWeapon].properties[2].valueCurrent = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[2][this.dataBomb[FormWeapon2.indexWeapon].levelWeapon];
			this.dataBomb[FormWeapon2.indexWeapon].properties[2].valueNextRank = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[2][Mathf.Min(this.dataBomb[FormWeapon2.indexWeapon].rankUpped + 1, 2) * 10 + 9];
			if (this.dataBomb[FormWeapon2.indexWeapon].levelWeapon < 29)
			{
				this.dataBomb[FormWeapon2.indexWeapon].properties[0].valueNext = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[0][this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataBomb[FormWeapon2.indexWeapon].properties[1].valueNext = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[1][this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1];
				this.dataBomb[FormWeapon2.indexWeapon].properties[2].valueNext = ProfileManager.grenadesProfile[FormWeapon2.indexWeapon].options[2][this.dataBomb[FormWeapon2.indexWeapon].levelWeapon + 1];
			}
			break;
		}
		default:
			UnityEngine.Debug.Log("Lỗi SetData() trong FormWeapon");
			break;
		}
	}

	private void LoadTableInfor()
	{
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		for (int i = 0; i < this.attributesWeapon.Length; i++)
		{
			if (data.properties != null && i < data.properties.Length)
			{
				this.attributesWeapon[i].gameObject.SetActive(true);
				this.attributesWeapon[i].txt_Name.text = data.properties[i].title.ToUpper() + string.Empty;
				if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Range, null) && FormWeapon2.indexTab == ETypeWeapon.GRENADE)
				{
					this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[4];
					this.attributesWeapon[i].valueCurrent = (float)((int)(100f * data.properties[i].valueCurrent));
					this.attributesWeapon[i].valueNext = (float)((int)(100f * data.properties[i].valueNext));
					this.attributesWeapon[i].valueNextRank = (float)((int)(100f * data.properties[i].valueNextRank));
					this.attributesWeapon[i].valueMineMax = (float)((int)(100f * data.properties[i].valueMineMax));
					this.attributesWeapon[i].valueMax = (float)((int)(100f * data.properties[i].valueMax));
				}
				else if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Firerate, null))
				{
					this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[1];
					this.attributesWeapon[i].valueCurrent = (float)((data.properties[i].valueCurrent <= 0f) ? 100 : ((int)(10f / data.properties[i].valueCurrent)));
					this.attributesWeapon[i].valueNext = (float)((data.properties[i].valueNext <= 0f) ? 100 : ((int)(10f / data.properties[i].valueNext)));
					this.attributesWeapon[i].valueNextRank = (float)((data.properties[i].valueNextRank <= 0f) ? 100 : ((int)(10f / data.properties[i].valueNextRank)));
					this.attributesWeapon[i].valueMineMax = (float)((data.properties[i].valueMineMax <= 0f) ? 100 : ((int)(10f / data.properties[i].valueMineMax)));
					this.attributesWeapon[i].valueMax = (float)((data.properties[i].valueMax <= 0f) ? 100 : ((int)(10f / data.properties[i].valueMax)));
				}
				else if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Damage, null))
				{
					this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[0];
					this.attributesWeapon[i].valueCurrent = (float)((int)(10f * data.properties[i].valueCurrent));
					this.attributesWeapon[i].valueNext = (float)((int)(10f * data.properties[i].valueNext));
					this.attributesWeapon[i].valueNextRank = (float)((int)(10f * data.properties[i].valueNextRank));
					this.attributesWeapon[i].valueMineMax = (float)((int)(10f * data.properties[i].valueMineMax));
					this.attributesWeapon[i].valueMax = (float)((int)(10f * data.properties[i].valueMax));
				}
				else if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Critical, null))
				{
					this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[2];
					this.attributesWeapon[i].valueCurrent = 100f * data.properties[i].valueCurrent;
					this.attributesWeapon[i].valueNext = 100f * data.properties[i].valueNext;
					this.attributesWeapon[i].valueNextRank = 100f * data.properties[i].valueNextRank;
					this.attributesWeapon[i].valueMineMax = 100f * data.properties[i].valueMineMax;
					this.attributesWeapon[i].valueMax = 100f;
				}
				else
				{
					if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Range, null))
					{
						this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[4];
					}
					else if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Ammo, null))
					{
						this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[3];
					}
					else if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Time, null))
					{
						this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[5];
					}
					else if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Speed, null))
					{
						this.attributesWeapon[i].img_Main.sprite = PopupManager.Instance.spriteIconProperty[6];
					}
					else
					{
						UnityEngine.Debug.LogError("FormWeapon2: Lỗi Property mới");
					}
					this.attributesWeapon[i].valueCurrent = data.properties[i].valueCurrent;
					this.attributesWeapon[i].valueNext = data.properties[i].valueNext;
					this.attributesWeapon[i].valueNextRank = data.properties[i].valueNextRank;
					this.attributesWeapon[i].valueMineMax = data.properties[i].valueMineMax;
					this.attributesWeapon[i].valueMax = data.properties[i].valueMax;
				}
				if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Critical, null))
				{
					this.attributesWeapon[i].txt_Amount.text = string.Concat(new object[]
					{
						this.attributesWeapon[i].valueCurrent,
						"%/<color=#8D8C8CFF>",
						this.attributesWeapon[i].valueMax,
						"%</color>"
					});
				}
				else
				{
					this.attributesWeapon[i].txt_Amount.text = string.Concat(new object[]
					{
						this.attributesWeapon[i].valueCurrent,
						"/<color=#8D8C8CFF>",
						this.attributesWeapon[i].valueMax,
						"</color>"
					});
				}
				this.attributesWeapon[i].img_Core.fillAmount = this.attributesWeapon[i].valueCurrent / this.attributesWeapon[i].valueMax;
				this.attributesWeapon[i].img_CoreMax.fillAmount = this.attributesWeapon[i].valueMineMax / this.attributesWeapon[i].valueMax;
			}
			else
			{
				this.attributesWeapon[i].gameObject.SetActive(false);
			}
		}
		this.txt_Name.text = string.Concat(new object[]
		{
			data.nameWeapon,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.txt_Power.text = data.power + string.Empty;
		int num = data.rankBase + data.rankUpped;
		this.txt_Name.color = PopupManager.Instance.color_Rank[num];
		Color color = PopupManager.Instance.color_Rank[num];
		this.img_BGName.color = new Color(color.r, color.g, color.b, 0.4f);
		this.img_Rank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
	}

	private InforWeapon[] GetArrayData(int indexTab)
	{
		switch (indexTab)
		{
		case 0:
			return this.dataGunMain;
		case 1:
			return this.dataGunSpecial;
		case 2:
			return this.dataMelee;
		case 3:
			return this.dataBomb;
		default:
			UnityEngine.Debug.Log("Lỗi GetArrayData() trong FormWeapon");
			return null;
		}
	}

	private InforWeapon GetData(int indexTab, int indexWeapon)
	{
		if (this.GetArrayData(indexTab) != null)
		{
			return this.GetArrayData(indexTab)[indexWeapon];
		}
		UnityEngine.Debug.Log("Lỗi GetArrayData() trong FormWeapon");
		return null;
	}

	private void OpenUnlockOrUpgrade()
	{
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		this.obj_Maximum.SetActive(false);
		if (data.isUnlock)
		{
			this.obj_BtnUpgrade.SetActive(true);
			this.obj_Upgrade_RankUp.SetActive(true);
			this.obj_Unlock.SetActive(false);
			this.obj_RankUpFrag.SetActive(FormWeapon2.indexTab != ETypeWeapon.GRENADE);
			this.obj_RankUpGold.SetActive(FormWeapon2.indexTab == ETypeWeapon.GRENADE);
			if (FormWeapon2.indexTab != ETypeWeapon.KNIFE && FormWeapon2.indexTab != ETypeWeapon.GRENADE)
			{
				if (data.rankUpped < 2 && data.rankUpped == data.levelWeapon / 10 && data.costRankUp > 0 && data.levelWeapon % 10 == 9)
				{
					this.txt_RankUpFrag.text = data.costRankUp + string.Empty;
					int num = this.GemRankUp[data.rankUpped].Value;
					if (FormWeapon2.indexWeapon == 0)
					{
						num = 10;
					}
					Text text = this.popUpRankUp_txtRankUpFrag;
					string text2 = num.ToString();
					this.txt_RankUpGem.text = text2;
					text.text = text2;
					Text text3 = this.popUpRankUp_txtRankUpFrag;
					text2 = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, this.dataGunMain[FormWeapon2.indexWeapon].rankUpped)].ToString();
					this.txt_RankUpFrag.text = text2;
					text3.text = text2;
					this.obj_BtnRankUp.SetActive(true);
				}
				else
				{
                    Debug.Log("1");
					this.obj_BtnRankUp.SetActive(false);
				}
				if (data.costUpgrade.value > 0)
				{
					this.txt_UpgradeGold.text = data.costUpgrade.value + string.Empty;
					this.imgIconMoney1.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
					this.imgIconMoney2.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
				}
				else
				{
                    Debug.Log("2");
                    this.obj_BtnUpgrade.SetActive(false);
				}
				if (data.levelWeapon + 1 >= (data.rankUpped + 1) * 10)
				{
                    Debug.Log("3");
                    this.obj_BtnUpgrade.SetActive(false);
					if (data.levelWeapon >= 29)
					{
						this.obj_BtnRankUp.SetActive(false);
						this.obj_Maximum.SetActive(true);
					}
					return;
				}
			}
			else if (FormWeapon2.indexTab == ETypeWeapon.KNIFE)
			{
				this.obj_BtnRankUp.SetActive(false);
				if (data.levelWeapon >= 29)
				{
					this.obj_BtnUpgrade.SetActive(false);
					this.obj_Maximum.SetActive(true);
					return;
				}
				if (data.costUpgrade.value > 0)
				{
					this.txt_UpgradeGold.text = data.costUpgrade.value + string.Empty;
					this.imgIconMoney1.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
					this.imgIconMoney2.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
				}
				else
				{
					this.obj_BtnUpgrade.SetActive(false);
				}
			}
			else
			{
				if (data.rankUpped < 2 && data.costUpgrade.value > 0 && data.rankUpped == data.levelWeapon / 10 && data.levelWeapon % 10 == 9)
				{
                    Debug.Log("4");
                    this.txt_RankUpGold.text = data.costUpgrade.value + string.Empty;
					this.imgIconMoney1.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
					this.imgIconMoney2.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
					this.obj_BtnRankUp.SetActive(true);
					this.obj_BtnUpgrade.SetActive(false);
					return;
				}
				this.obj_BtnRankUp.SetActive(false);
				if (data.costUpgrade.value > 0)
				{
					this.txt_UpgradeGold.text = data.costUpgrade.value + string.Empty;
					this.imgIconMoney1.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
					this.imgIconMoney2.sprite = PopupManager.Instance.sprite_Item[(int)data.costUpgrade.item];
				}
				else
				{
                    Debug.Log("5");
                    this.obj_BtnUpgrade.SetActive(false);
				}
				if (data.levelWeapon >= 29)
				{
					this.obj_BtnUpgrade.SetActive(false);
					this.obj_Maximum.SetActive(true);
					return;
				}
			}
		}
		else
		{
			this.obj_Upgrade_RankUp.SetActive(false);
			this.obj_Unlock.SetActive(true);
			this.obj_BtnUnlockByGem.SetActive(true);
			this.obj_BtnUnlockByFrag.SetActive(true);
			if (data.costUnlockByGold > 0)
			{
				int num2 = (data.CurrencyUnlock != Item.Gold) ? PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0) : ProfileManager.userProfile.Coin;
				this.txt_UnlockFrag.text = 80 + string.Empty;
				this.SecuredGoldUnlock = new SecuredInt(data.costUnlockByGold);
			}
			else
			{
				this.obj_BtnUnlockByFrag.SetActive(false);
			}
			if (data.costUnlockByGem > 0)
			{
				this.txt_UnlockGem.text = data.costUnlockByGem + string.Empty;
				this.SecuredGemUnlock = new SecuredInt(data.costUnlockByGem);
			}
			else
			{
				this.obj_BtnUnlockByGem.SetActive(false);
			}
		}
	}

	public void CheckTipsAll()
	{
		for (int i = 0; i < this.cardsTab.Length; i++)
		{
			this.cardsTab[i].obj_Open.SetActive(this.CheckTipsListWeapon(i));
		}
	}

	private bool CheckTipsListWeapon(int indexTabCheck)
	{
		bool result = false;
		InforWeapon[] arrayData = this.GetArrayData(indexTabCheck);
		for (int i = 0; i < arrayData.Length; i++)
		{
			bool flag = this.CheckTipsUnlockOrUpgrade(indexTabCheck, i);
			if (FormWeapon2.indexTab == (ETypeWeapon)indexTabCheck)
			{
				this.cardsWeapon[i].obj_Tip.SetActive(flag);
			}
			if (flag)
			{
				result = true;
			}
		}
		return result;
	}

	private bool CheckTipsUnlockOrUpgrade(int indexTabCheck, int indexWeaponCheck)
	{
		bool[] array = new bool[5];
		InforWeapon data = this.GetData(indexTabCheck, indexWeaponCheck);
		try
		{
			bool flag = FormWeapon2.indexTab == (ETypeWeapon)indexTabCheck && FormWeapon2.indexWeapon == indexWeaponCheck;
			array = ShopTips.Instance.CheckTipWeapon((ETypeWeapon)indexTabCheck, indexWeaponCheck);
			if (!data.isUnlock)
			{
				if (flag)
				{
					this.obj_TipUnlockFrag.SetActive(array[1]);
				}
				if (flag)
				{
					this.obj_TipUnlockGem.SetActive(array[2]);
				}
				SkeletonGraphic component = this.obj_TipUnlockFrag.GetComponent<SkeletonGraphic>();
				SkeletonGraphic component2 = this.obj_TipUnlockGem.GetComponent<SkeletonGraphic>();
				if (this.obj_TipUnlockFrag.activeSelf && this.obj_TipUnlockGem.activeSelf && component2 != null && component != null)
				{
					component.AnimationState.SetAnimation(0, "animation", true);
					component2.AnimationState.SetAnimation(0, "animation", true);
				}
			}
			else
			{
				if (flag)
				{
					this.obj_TipUpgrade.SetActive(array[3]);
				}
				if (flag)
				{
					this.obj_TipRankUp.SetActive(array[4]);
				}
			}
		}
		catch (Exception ex)
		{
			return false;
		}
		return array[0];
	}

	private IEnumerator ShowEffGun(float time, GameObject offButton)
	{
		offButton.SetActive(false);
		yield return new WaitForSeconds(time);
		offButton.SetActive(true);
		yield break;
	}

	private void EquipWeapon(int indexWeaponOld, int indexWeaponNew)
	{
		switch (FormWeapon2.indexTab)
		{
		case ETypeWeapon.PRIMARY:
			this.dataGunMain[indexWeaponOld].isEquiped = false;
			this.dataGunMain[indexWeaponNew].isEquiped = true;
			for (int i = 0; i < this.cardsWeapon.Length; i++)
			{
				this.cardsWeapon[i].obj_Equiped.SetActive(false);
			}
			this.cardsWeapon[indexWeaponNew].obj_Equiped.SetActive(true);
			ProfileManager.rifleGunCurrentId.setValue(indexWeaponNew);
			break;
		case ETypeWeapon.SPECIAL:
			this.dataGunSpecial[indexWeaponOld].isEquiped = false;
			this.dataGunSpecial[indexWeaponNew].isEquiped = true;
			break;
		case ETypeWeapon.KNIFE:
			this.dataMelee[indexWeaponOld].isEquiped = false;
			this.dataMelee[indexWeaponNew].isEquiped = true;
			for (int j = 0; j < this.cardsWeapon.Length; j++)
			{
				this.cardsWeapon[j].obj_Equiped.SetActive(false);
			}
			this.cardsWeapon[indexWeaponNew].obj_Equiped.SetActive(true);
			ProfileManager.meleCurrentId.setValue(indexWeaponNew);
			break;
		case ETypeWeapon.GRENADE:
			this.dataBomb[indexWeaponOld].isEquiped = false;
			this.dataBomb[indexWeaponNew].isEquiped = true;
			for (int k = 0; k < this.cardsWeapon.Length; k++)
			{
				this.cardsWeapon[k].obj_Equiped.SetActive(false);
			}
			this.cardsWeapon[indexWeaponNew].obj_Equiped.SetActive(true);
			ProfileManager.grenadeCurrentId.setValue(indexWeaponNew);
			break;
		default:
			UnityEngine.Debug.Log("Lỗi EquipWeapon() trong FormWeapon");
			break;
		}
	}

	private IEnumerator ScrollListCardWeapon()
	{
		yield return new WaitForSeconds(0.04f);
		this.scrollbarWepon.value = (float)(1 - FormWeapon2.indexWeapon / (this.GetArrayData((int)FormWeapon2.indexTab).Length - 1));
		yield break;
	}

	private void GetMaxProperty()
	{
		for (int i = 0; i < ProfileManager.weaponsRifle.Length; i++)
		{
			if (this.listMaxPrimary[0] < ProfileManager.weaponsRifle[i].Gun_Value.properties[2][29])
			{
				this.listMaxPrimary[0] = ProfileManager.weaponsRifle[i].Gun_Value.properties[2][29];
			}
		}
		for (int j = 0; j < ProfileManager.weaponsRifle.Length; j++)
		{
			if (this.listMaxPrimary[1] > ProfileManager.weaponsRifle[j].Gun_Value.properties[3][29])
			{
				this.listMaxPrimary[1] = ProfileManager.weaponsRifle[j].Gun_Value.properties[3][29];
			}
		}
		for (int k = 0; k < ProfileManager.weaponsRifle.Length; k++)
		{
			if (this.listMaxPrimary[2] < ProfileManager.weaponsRifle[k].Gun_Value.properties[4][29])
			{
				this.listMaxPrimary[2] = ProfileManager.weaponsRifle[k].Gun_Value.properties[4][29];
			}
		}
		for (int l = 0; l < ProfileManager.weaponsSpecial.Length; l++)
		{
			if (this.listMaxSpecial[0] < ProfileManager.weaponsSpecial[l].Gun_Value.properties[2][29])
			{
				this.listMaxSpecial[0] = ProfileManager.weaponsSpecial[l].Gun_Value.properties[2][29];
			}
		}
		for (int m = 0; m < ProfileManager.weaponsSpecial.Length; m++)
		{
			if (this.listMaxSpecial[1] > ProfileManager.weaponsSpecial[m].Gun_Value.properties[3][29])
			{
				this.listMaxSpecial[1] = ProfileManager.weaponsSpecial[m].Gun_Value.properties[3][29];
			}
		}
		for (int n = 0; n < ProfileManager.weaponsSpecial.Length; n++)
		{
			if (this.listMaxSpecial[2] < ProfileManager.weaponsSpecial[n].Gun_Value.properties[4][29])
			{
				this.listMaxSpecial[2] = ProfileManager.weaponsSpecial[n].Gun_Value.properties[4][29];
			}
		}
		for (int num = 0; num < ProfileManager.weaponsSpecial.Length; num++)
		{
			if (this.listMaxSpecial[3] < ProfileManager.weaponsSpecial[num].Gun_Value.properties[1][29])
			{
				this.listMaxSpecial[3] = ProfileManager.weaponsSpecial[num].Gun_Value.properties[1][29];
			}
		}
		for (int num2 = 0; num2 < ProfileManager.melesProfile.Length; num2++)
		{
			if (this.listMaxKnife[0] < ProfileManager.melesProfile[num2].SecuredDamaged[29].Value)
			{
				this.listMaxKnife[0] = ProfileManager.melesProfile[num2].SecuredDamaged[29].Value;
			}
		}
		for (int num3 = 0; num3 < ProfileManager.melesProfile.Length; num3++)
		{
			if (this.listMaxKnife[1] < (float)ProfileManager.melesProfile[num3].range[29])
			{
				this.listMaxKnife[1] = (float)ProfileManager.melesProfile[num3].range[29];
			}
		}
		for (int num4 = 0; num4 < ProfileManager.melesProfile.Length; num4++)
		{
			if (this.listMaxKnife[2] < (float)ProfileManager.melesProfile[num4].speed[29])
			{
				this.listMaxKnife[2] = (float)ProfileManager.melesProfile[num4].speed[29];
			}
		}
		for (int num5 = 0; num5 < ProfileManager.grenadesProfile.Length; num5++)
		{
			if (this.listMaxBomb[0] < ProfileManager.grenadesProfile[num5].options[0][29])
			{
				this.listMaxBomb[0] = ProfileManager.grenadesProfile[num5].options[0][29];
			}
		}
		for (int num6 = 0; num6 < ProfileManager.grenadesProfile.Length; num6++)
		{
			if (this.listMaxBomb[1] < ProfileManager.grenadesProfile[num6].options[1][29])
			{
				this.listMaxBomb[1] = ProfileManager.grenadesProfile[num6].options[1][29];
			}
		}
		for (int num7 = 0; num7 < ProfileManager.grenadesProfile.Length; num7++)
		{
			if (this.listMaxBomb[2] < ProfileManager.grenadesProfile[num7].options[2][29])
			{
				this.listMaxBomb[2] = ProfileManager.grenadesProfile[num7].options[2][29];
			}
		}
	}

	public void BtnTab(int index)
	{
		AudioClick.Instance.OnClick();
		
		this.ChangeTab(index);
	}

	public void BtnWeapon(CardWeapon card)
	{
		AudioClick.Instance.OnClick();
		
		this.ClickToWeapon(card);
	}

	public void BtnUnlock(bool isGem)
	{
		
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		if (isGem)
		{
			if (this.SecuredGemUnlock.Value > ProfileManager.userProfile.Ms)
			{
				MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, this.SecuredGemUnlock.Value, string.Empty);
				return;
			}
			PopupManager.Instance.SaveReward(Item.Gem, -this.SecuredGemUnlock.Value, base.name + "_Unlock", null);
			MenuManager.Instance.topUI.ReloadCoin();
		}
		else if (data.CurrencyUnlock == Item.Gold)
		{
			if (this.SecuredGoldUnlock.Value > ProfileManager.userProfile.Coin)
			{
				MenuManager.Instance.popupInformation.ShowWarning(Item.Gold, this.SecuredGoldUnlock.Value, string.Empty);
				return;
			}
			PopupManager.Instance.SaveReward(Item.Gold, -this.SecuredGoldUnlock.Value, base.name + "_Unlock", null);
			MenuManager.Instance.topUI.ReloadCoin();
		}
		else
		{
			if (80 > PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0))
			{
				MenuManager.Instance.popupInformation.ShowWarning(data.CurrencyUnlock, 80, string.Empty);
				return;
			}
			PopupManager.Instance.SaveReward(data.CurrencyUnlock, -80, base.name + "_Unlock", null);
		}
		this.SetData(true, data.levelWeapon, data.rankBase + data.rankUpped, data.amountCurrent);
		this.cardsWeapon[FormWeapon2.indexWeapon].img_Main.sprite = data.spriteWeapon;
		this.cardsWeapon[FormWeapon2.indexWeapon].img_Main.rectTransform.sizeDelta = data.sizeImage;
		this.cardsWeapon[FormWeapon2.indexWeapon].obj_Lock.SetActive(false);
		if (FormWeapon2.indexTab == ETypeWeapon.PRIMARY || FormWeapon2.indexTab == ETypeWeapon.SPECIAL)
		{
			this.cardsWeapon[FormWeapon2.indexWeapon].txtFragment.text = PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0) + "/" + 80;
			this.cardsWeapon[FormWeapon2.indexWeapon].imageProgressFragment.fillAmount = (float)PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0) / 80f;
		}
		if (!this.cardsWeapon[FormWeapon2.indexWeapon].obj_Lock.activeSelf)
		{
			this.EquipWeapon(FormWeapon2.indexWeapon, FormWeapon2.indexWeapon);
		}
		this.LoadTableInfor();
		this.OpenUnlockOrUpgrade();
		this.preview.rambo[ProfileManager.settingProfile.IDChar].OnSetSkinByRank(FormWeapon2.indexTab, FormWeapon2.indexWeapon, data.rankUpped, data.isUnlock);
		this.preview.OnShow(FormWeapon2.indexTab, Mathf.Max(FormWeapon2.indexWeapon, 0), data.levelWeapon, data.rankUpped, data.isUnlock);
		this.CheckTipsAll();
		this.effUpgrade.ShowUpgrade(base.transform, 1f);
		this.listEffUnlock[(int)FormWeapon2.indexTab].UnLock(FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		base.StartCoroutine(this.ShowEffGun(0.5f, this.obj_GroupButton));
	}

	public void BtnUpgrade()
	{
		
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		if (!ProfileManager.unlockAll)
		{
			Item item = data.costUpgrade.item;
			if (item != Item.Gold)
			{
				if (item == Item.Gem)
				{
					if (data.costUpgrade.value > ProfileManager.userProfile.Ms)
					{
						MenuManager.Instance.popupInformation.ShowWarning(data.costUpgrade.item, data.costUpgrade.value, string.Empty);
						return;
					}
					PopupManager.Instance.SaveReward(data.costUpgrade.item, -data.costUpgrade.value, base.name + "_Upgrade", null);
					DailyQuestManager.Instance.MissionWeaponShop(null);
				}
			}
			else
			{
				if (data.costUpgrade.value > ProfileManager.userProfile.Coin)
				{
					MenuManager.Instance.popupInformation.ShowWarning(data.costUpgrade.item, data.costUpgrade.value, string.Empty);
					return;
				}
				PopupManager.Instance.SaveReward(data.costUpgrade.item, -data.costUpgrade.value, base.name + "_Upgrade", null);
				DailyQuestManager.Instance.MissionWeaponShop(null);
			}
		}
		this.SetData(data.isUnlock, data.levelWeapon + 1, data.rankBase + data.rankUpped, data.amountCurrent);
		
		this.LoadTableInfor();
		this.OpenUnlockOrUpgrade();
		this.cardsWeapon[FormWeapon2.indexWeapon].txt_Name.text = string.Concat(new object[]
		{
			data.nameWeapon,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.cardsWeapon[FormWeapon2.indexWeapon].objProgressFragment.SetActive(FormWeapon2.indexTab != ETypeWeapon.KNIFE && FormWeapon2.indexTab != ETypeWeapon.GRENADE && data.levelWeapon < 29);
		this.CheckTipsAll();
		if (FormWeapon2.indexTab == ETypeWeapon.KNIFE)
		{
			if (data.levelWeapon < 29)
			{
				base.StartCoroutine(this.ShowEffGun(0.5f, this.popUpUpgrade_BtnUpgrade));
				this.effUpgrade.ShowUpgrade(this.popUpUpgrade_CardsWeapon.transform, 0.5f);
			}
			else
			{
				this.obj_PopUpUpgrade.SetActive(false);
			}
		}
		else if (data.levelWeapon % 10 == 9)
		{
			if (this.obj_BtnRankUp.activeSelf)
			{
				this.effUpgrade.ShowUpgrade(this.popUpUpgrade_CardsWeapon.transform, 0.5f);
				this.obj_PopUpUpgrade.SetActive(false);
				this.BtnPopupRankUp();
			}
			else
			{
				this.obj_PopUpUpgrade.SetActive(false);
			}
		}
		else
		{
			base.StartCoroutine(this.ShowEffGun(0.5f, this.popUpUpgrade_BtnUpgrade));
			this.effUpgrade.ShowUpgrade(this.popUpUpgrade_CardsWeapon.transform, 0.5f);
		}
		this.popUpUpgrade_txtLevel.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.popUpUpgrade_txtLevelNext.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 2
		});
		this.popUpUpgrade_txtPower.text = data.power + string.Empty;
		this.popUpUpgrade_txtPowerNext.text = data.powerNext + string.Empty;
		for (int i = 0; i < this.attributesWeapon.Length; i++)
		{
			this.popUpUpgrade_AttributesWeapon[i].gameObject.SetActive(this.attributesWeapon[i].gameObject.activeSelf);
			this.popUpUpgrade_AttributesWeapon[i].txt_Name.text = this.attributesWeapon[i].txt_Name.text;
			this.popUpUpgrade_AttributesWeapon[i].img_Main.sprite = this.attributesWeapon[i].img_Main.sprite;
			this.popUpUpgrade_AttributesWeapon[i].txt_Amount.text = this.attributesWeapon[i].valueCurrent + string.Empty;
			if (i < data.properties.Length)
			{
				if (this.attributesWeapon[i].valueNext > this.attributesWeapon[i].valueCurrent)
				{
					this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext.gameObject.SetActive(true);
					this.popUpUpgrade_AttributesWeapon[i].obj_Arrow.SetActive(true);
					this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext.text = this.attributesWeapon[i].valueNext + string.Empty;
				}
				else
				{
					this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext.gameObject.SetActive(false);
					this.popUpUpgrade_AttributesWeapon[i].obj_Arrow.SetActive(false);
				}
				if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Critical, null))
				{
					Text txt_Amount = this.popUpUpgrade_AttributesWeapon[i].txt_Amount;
					txt_Amount.text += "%";
					Text txt_ValueNext = this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext;
					txt_ValueNext.text += "%";
				}
				this.popUpUpgrade_AttributesWeapon[i].img_Core.fillAmount = this.attributesWeapon[i].valueCurrent / this.attributesWeapon[i].valueMax;
				this.popUpUpgrade_AttributesWeapon[i].img_CoreMax.fillAmount = this.attributesWeapon[i].valueNext / this.attributesWeapon[i].valueMax;
			}
		}
		this.popUpUpgrade_BtnUpgradeTip.SetActive(this.obj_TipUpgrade.activeSelf);
		this.popUpUpgrade_CardsWeapon.txt_Name.text = string.Concat(new object[]
		{
			data.nameWeapon,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.popUpUpgrade_CardsWeapon.img_Main.sprite = data.spriteWeapon;
		this.popUpUpgrade_txtUpgradeGold.text = this.txt_UpgradeGold.text;
		int num = data.rankBase + data.rankUpped;
		this.popUpUpgrade_CardsWeapon.img_BG.sprite = PopupManager.Instance.sprite_BGCardRank[num];
		this.popUpUpgrade_CardsWeapon.txt_Name.color = PopupManager.Instance.color_Rank[num];
		this.popUpUpgrade_CardsWeapon.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		if (FormWeapon2.indexTab != ETypeWeapon.GRENADE && FormWeapon2.indexTab != ETypeWeapon.KNIFE)
		{
			if (!string.IsNullOrEmpty(this.aniRank[data.rankBase + data.rankUpped]))
			{
				this.popUpUpgrade_CardsWeapon.obj_EffectByRank.gameObject.SetActive(true);
				if (this.popUpUpgrade_CardsWeapon.obj_EffectByRank.AnimationState == null)
				{
					this.popUpUpgrade_CardsWeapon.obj_EffectByRank.Initialize(true);
				}
				this.popUpUpgrade_CardsWeapon.obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[data.rankBase + data.rankUpped], true);
			}
			else
			{
				this.popUpUpgrade_CardsWeapon.obj_EffectByRank.gameObject.SetActive(false);
			}
		}
		else
		{
			this.popUpUpgrade_CardsWeapon.obj_EffectByRank.gameObject.SetActive(false);
		}
		int num2 = this.GemRankUp[Mathf.Min(1, data.rankUpped)].Value;
		if (FormWeapon2.indexWeapon == 0)
		{
			num2 = 10;
		}
		Text text = this.popUpRankUp_txtRankUpFrag;
		string text2 = num2.ToString();
		this.txt_RankUpGem.text = text2;
		text.text = text2;
		Text text3 = this.popUpRankUp_txtRankUpFrag;
		text2 = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, this.dataGunMain[FormWeapon2.indexWeapon].rankUpped)].ToString();
		this.txt_RankUpFrag.text = text2;
		text3.text = text2;
	}

	public void BtnRankUp()
	{
		
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		if (!ProfileManager.unlockAll)
		{
			if (FormWeapon2.indexTab != ETypeWeapon.GRENADE)
			{
				int num = this.GemRankUp[data.rankUpped].Value;
				if (FormWeapon2.indexWeapon == 0)
				{
					num = 10;
				}
				if (ProfileManager.userProfile.Ms < num)
				{
					MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, num, string.Empty);
					return;
				}
				if (data.costRankUp > PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyRankUp.ToString(), 0))
				{
					MenuManager.Instance.popupInformation.ShowWarning(data.CurrencyUnlock, data.costRankUp, string.Empty);
					return;
				}
				PopupManager.Instance.SaveReward(data.CurrencyRankUp, -data.costRankUp, base.name + "_RankUp", null);
				PopupManager.Instance.SaveReward(Item.Gem, -num, base.name + "_RankUp", null);
			}
			else
			{
				Item item = data.costUpgrade.item;
				if (item != Item.Gold)
				{
					if (item == Item.Gem)
					{
						if (data.costUpgrade.value > ProfileManager.userProfile.Ms)
						{
							MenuManager.Instance.popupInformation.ShowWarning(data.costUpgrade.item, data.costUpgrade.value, string.Empty);
							return;
						}
						PopupManager.Instance.SaveReward(data.costUpgrade.item, -data.costUpgrade.value, base.name + "_RankUp", null);
					}
				}
				else
				{
					if (data.costUpgrade.value > ProfileManager.userProfile.Coin)
					{
						MenuManager.Instance.popupInformation.ShowWarning(data.costUpgrade.item, data.costUpgrade.value, string.Empty);
						return;
					}
					PopupManager.Instance.SaveReward(data.costUpgrade.item, -data.costUpgrade.value, base.name + "_RankUp", null);
					DailyQuestManager.Instance.MissionWeaponShop(null);
				}
			}
		}
		if (FormWeapon2.indexTab != ETypeWeapon.GRENADE)
		{
			this.SetData(data.isUnlock, data.levelWeapon, data.rankBase + data.rankUpped + 1, data.amountCurrent);
			this.listEffUnlock[(int)FormWeapon2.indexTab].RankupGun(FormWeapon2.indexTab, FormWeapon2.indexWeapon, data.rankUpped - 1, data.rankUpped);
			this.listEffUnlock[(int)FormWeapon2.indexTab].OnShowRankOldEnd = delegate()
			{
				this.effUpgrade.ShowUpgrade(base.transform, 1f);
			};
		}
		else
		{
			this.SetData(data.isUnlock, data.levelWeapon + 1, data.rankBase + data.rankUpped, data.amountCurrent);
			this.listEffUnlock[(int)FormWeapon2.indexTab].RankupGun(FormWeapon2.indexTab, FormWeapon2.indexWeapon, data.levelWeapon / 10 - 1, data.levelWeapon / 10);
			this.listEffUnlock[(int)FormWeapon2.indexTab].OnShowRankOldEnd = delegate()
			{
				this.effUpgrade.ShowUpgrade(base.transform, 1f);
			};
		}
		base.StartCoroutine(this.ShowEffGun(0.65f, this.obj_GroupButton));
		
		this.cardsWeapon[FormWeapon2.indexWeapon].img_Main.sprite = data.spriteWeapon;
		this.cardsWeapon[FormWeapon2.indexWeapon].img_Main.rectTransform.sizeDelta = data.sizeImage;
		this.cardsWeapon[FormWeapon2.indexWeapon].txt_Name.text = string.Concat(new object[]
		{
			data.nameWeapon,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		int num2 = data.rankBase + data.rankUpped;
		this.cardsWeapon[FormWeapon2.indexWeapon].img_BG.sprite = PopupManager.Instance.sprite_BGCardRank[num2];
		this.cardsWeapon[FormWeapon2.indexWeapon].txt_Name.color = PopupManager.Instance.color_Rank[num2];
		this.cardsWeapon[FormWeapon2.indexWeapon].img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num2];
		if (FormWeapon2.indexTab == ETypeWeapon.PRIMARY || FormWeapon2.indexTab == ETypeWeapon.SPECIAL)
		{
			if (data.levelWeapon < 29)
			{
				this.cardsWeapon[FormWeapon2.indexWeapon].txtFragment.text = PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0) + "/" + data.costRankUp;
				this.cardsWeapon[FormWeapon2.indexWeapon].imageProgressFragment.fillAmount = (float)PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0) / (float)data.costRankUp;
			}
			else
			{
				this.cardsWeapon[FormWeapon2.indexWeapon].txtFragment.text = PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyUnlock.ToString(), 0) + string.Empty;
				this.cardsWeapon[FormWeapon2.indexWeapon].imageProgressFragment.fillAmount = 1f;
			}
		}
		if (FormWeapon2.indexTab != ETypeWeapon.GRENADE && FormWeapon2.indexTab != ETypeWeapon.KNIFE)
		{
			if (!string.IsNullOrEmpty(this.aniRank[data.rankBase + data.rankUpped]))
			{
				this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.gameObject.SetActive(true);
				if (this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.AnimationState == null)
				{
					this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.Initialize(true);
				}
				this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[data.rankBase + data.rankUpped], true);
			}
			else
			{
				this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.gameObject.SetActive(false);
			}
		}
		else
		{
			this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.gameObject.SetActive(false);
		}
		if (FormWeapon2.indexTab == ETypeWeapon.PRIMARY || FormWeapon2.indexTab == ETypeWeapon.SPECIAL)
		{
		}
		this.LoadTableInfor();
		this.OpenUnlockOrUpgrade();
		this.preview.rambo[ProfileManager.settingProfile.IDChar].OnSetSkinByRank(FormWeapon2.indexTab, FormWeapon2.indexWeapon, data.rankUpped, data.isUnlock);
		this.preview.OnShow(FormWeapon2.indexTab, Mathf.Max(FormWeapon2.indexWeapon, 0), data.levelWeapon, data.rankUpped, data.isUnlock);
		this.CheckTipsAll();
		this.obj_PopUpRankUp.SetActive(false);
	}

	public void BtnPopupUpgrade()
	{
		
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		this.popUpUpgrade_txtLevel.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.popUpUpgrade_txtLevelNext.text = string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 2
		});
		this.popUpUpgrade_txtPower.text = data.power + string.Empty;
		this.popUpUpgrade_txtPowerNext.text = data.powerNext + string.Empty;
		for (int i = 0; i < this.attributesWeapon.Length; i++)
		{
			this.popUpUpgrade_AttributesWeapon[i].gameObject.SetActive(this.attributesWeapon[i].gameObject.activeSelf);
			this.popUpUpgrade_AttributesWeapon[i].txt_Name.text = this.attributesWeapon[i].txt_Name.text;
			this.popUpUpgrade_AttributesWeapon[i].img_Main.sprite = this.attributesWeapon[i].img_Main.sprite;
			this.popUpUpgrade_AttributesWeapon[i].txt_Amount.text = this.attributesWeapon[i].valueCurrent + string.Empty;
			if (i < data.properties.Length)
			{
				if (this.attributesWeapon[i].valueNext > this.attributesWeapon[i].valueCurrent)
				{
					this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext.gameObject.SetActive(true);
					this.popUpUpgrade_AttributesWeapon[i].obj_Arrow.SetActive(true);
					this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext.text = this.attributesWeapon[i].valueNext + string.Empty;
				}
				else
				{
					this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext.gameObject.SetActive(false);
					this.popUpUpgrade_AttributesWeapon[i].obj_Arrow.SetActive(false);
				}
				if (data.properties[i].title == PopupManager.Instance.GetText(Localization0.Critical, null))
				{
					Text txt_Amount = this.popUpUpgrade_AttributesWeapon[i].txt_Amount;
					txt_Amount.text += "%";
					Text txt_ValueNext = this.popUpUpgrade_AttributesWeapon[i].txt_ValueNext;
					txt_ValueNext.text += "%";
				}
				this.popUpUpgrade_AttributesWeapon[i].img_Core.fillAmount = this.attributesWeapon[i].valueCurrent / this.attributesWeapon[i].valueMax;
				this.popUpUpgrade_AttributesWeapon[i].img_CoreMax.fillAmount = this.attributesWeapon[i].valueNext / this.attributesWeapon[i].valueMax;
			}
		}
		this.popUpUpgrade_BtnUpgradeTip.SetActive(this.obj_TipUpgrade.activeSelf);
		this.popUpUpgrade_CardsWeapon.txt_Name.text = string.Concat(new object[]
		{
			data.nameWeapon,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.popUpUpgrade_CardsWeapon.img_Main.sprite = data.spriteWeapon;
		this.popUpUpgrade_txtUpgradeGold.text = this.txt_UpgradeGold.text;
		int num = data.rankBase + data.rankUpped;
		this.popUpUpgrade_CardsWeapon.img_BG.sprite = PopupManager.Instance.sprite_BGCardRank[num];
		this.popUpUpgrade_CardsWeapon.txt_Name.color = PopupManager.Instance.color_Rank[num];
		this.popUpUpgrade_CardsWeapon.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		if (FormWeapon2.indexTab != ETypeWeapon.GRENADE && FormWeapon2.indexTab != ETypeWeapon.KNIFE)
		{
			if (!string.IsNullOrEmpty(this.aniRank[data.rankBase + data.rankUpped]))
			{
				this.popUpUpgrade_CardsWeapon.obj_EffectByRank.gameObject.SetActive(true);
				if (this.popUpUpgrade_CardsWeapon.obj_EffectByRank.AnimationState == null)
				{
					this.popUpUpgrade_CardsWeapon.obj_EffectByRank.Initialize(true);
				}
				this.popUpUpgrade_CardsWeapon.obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[data.rankBase + data.rankUpped], true);
			}
			else
			{
				this.popUpUpgrade_CardsWeapon.obj_EffectByRank.gameObject.SetActive(false);
			}
		}
		else
		{
			this.cardsWeapon[FormWeapon2.indexWeapon].obj_EffectByRank.gameObject.SetActive(false);
		}
		this.obj_PopUpUpgrade.SetActive(true);
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_UpgradeWeapon)
		{
			MenuManager.Instance.tutorial.NextTutorial(2);
		}
	}

	public void BtnPopupRankUp()
	{
		
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		int num = data.rankBase + data.rankUpped;
		this.popUpRankUp_ImgRankOld.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		this.popUpRankUp_ImgRankNew.sprite = PopupManager.Instance.sprite_IconRankItem[Mathf.Min(num + 1, PopupManager.Instance.sprite_IconRankItem.Length - 1)];
		this.popUpRankUp_CardsWeaponOld.txt_Name.text = string.Concat(new object[]
		{
			data.nameWeapon,
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		});
		this.popUpRankUp_CardsWeaponNew.txt_Name.text = data.nameWeapon + ((FormWeapon2.indexTab != ETypeWeapon.GRENADE) ? string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 1
		}) : string.Concat(new object[]
		{
			" ",
			PopupManager.Instance.GetText(Localization0.Lv, null),
			" ",
			data.levelWeapon + 2
		}));
		this.popUpRankUp_CardsWeaponOld.img_Main.sprite = data.spriteWeapon;
		this.popUpRankUp_CardsWeaponNew.img_Main.sprite = data.spriteWeaponNext;
		this.popUpRankUp_CardsWeaponOld.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
		this.popUpRankUp_CardsWeaponNew.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[Mathf.Min(num + 1, PopupManager.Instance.sprite_IconRankItem.Length - 1)];
		this.popUpRankUp_CardsWeaponOld.img_BG.sprite = PopupManager.Instance.sprite_BGCardRank[num];
		this.popUpRankUp_CardsWeaponNew.img_BG.sprite = PopupManager.Instance.sprite_BGCardRank[Mathf.Min(num + 1, PopupManager.Instance.sprite_BGCardRank.Length - 1)];
		this.popUpRankUp_CardsWeaponOld.txt_Name.color = PopupManager.Instance.color_Rank[num];
		this.popUpRankUp_CardsWeaponNew.txt_Name.color = PopupManager.Instance.color_Rank[Mathf.Min(num + 1, PopupManager.Instance.color_Rank.Length - 1)];
		if (FormWeapon2.indexTab != ETypeWeapon.GRENADE)
		{
			if (!string.IsNullOrEmpty(this.aniRank[data.rankBase + data.rankUpped]))
			{
				this.popUpRankUp_CardsWeaponOld.obj_EffectByRank.gameObject.SetActive(true);
				if (this.popUpRankUp_CardsWeaponOld.obj_EffectByRank.AnimationState == null)
				{
					this.popUpRankUp_CardsWeaponOld.obj_EffectByRank.Initialize(true);
				}
				this.popUpRankUp_CardsWeaponOld.obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[data.rankBase + data.rankUpped], true);
			}
			else
			{
				this.popUpRankUp_CardsWeaponOld.obj_EffectByRank.gameObject.SetActive(false);
			}
			if (!string.IsNullOrEmpty(this.aniRank[data.rankBase + data.rankUpped + 1]))
			{
				this.popUpRankUp_CardsWeaponNew.obj_EffectByRank.gameObject.SetActive(true);
				if (this.popUpRankUp_CardsWeaponNew.obj_EffectByRank.AnimationState == null)
				{
					this.popUpRankUp_CardsWeaponNew.obj_EffectByRank.Initialize(true);
				}
				this.popUpRankUp_CardsWeaponNew.obj_EffectByRank.AnimationState.SetAnimation(0, this.aniRank[data.rankBase + data.rankUpped + 1], true);
			}
			else
			{
				this.popUpRankUp_CardsWeaponNew.obj_EffectByRank.gameObject.SetActive(false);
			}
			this.popUpRankUp_CardsWeaponOld.objProgressFragment.SetActive(true);
			this.popUpRankUp_CardsWeaponNew.objProgressFragment.SetActive(true);
			this.popUpRankUp_CardsWeaponOld.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num];
			this.popUpRankUp_CardsWeaponNew.img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num + 1];
			this.popUpRankUp_CardsWeaponNew.objProgressFragment.SetActive(true);
			this.popUpRankUp_CardsWeaponOld.txtFragment.text = PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyRankUp.ToString(), 0) + "/" + data.costRankUp;
			this.popUpRankUp_CardsWeaponNew.txtFragment.text = Math.Max(PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyRankUp.ToString(), 0) - data.costRankUp, 0) + "/" + data.costRankUp;
			this.popUpRankUp_CardsWeaponOld.imageProgressFragment.fillAmount = (float)PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyRankUp.ToString(), 0) / (float)data.costRankUp;
			this.popUpRankUp_CardsWeaponNew.imageProgressFragment.fillAmount = (float)Math.Max(PlayerPrefs.GetInt("metal.squad.frag." + data.CurrencyRankUp.ToString(), 0) - data.costRankUp, 0) / (float)data.costRankUp;
		}
		else
		{
			this.popUpRankUp_CardsWeaponOld.obj_EffectByRank.gameObject.SetActive(false);
			this.popUpRankUp_CardsWeaponNew.obj_EffectByRank.gameObject.SetActive(false);
			this.popUpRankUp_CardsWeaponOld.objProgressFragment.SetActive(false);
			this.popUpRankUp_CardsWeaponNew.objProgressFragment.SetActive(false);
		}
		for (int i = 0; i < this.attributesWeapon.Length; i++)
		{
			this.popUpRankUp_AttributesWeaponOld[i].gameObject.SetActive(this.attributesWeapon[i].gameObject.activeSelf);
			this.popUpRankUp_AttributesWeaponNew[i].gameObject.SetActive(this.attributesWeapon[i].gameObject.activeSelf);
			this.popUpRankUp_AttributesWeaponOld[i].txt_Name.text = this.attributesWeapon[i].txt_Name.text;
			this.popUpRankUp_AttributesWeaponNew[i].txt_Name.text = this.attributesWeapon[i].txt_Name.text;
			this.popUpRankUp_AttributesWeaponOld[i].img_Main.sprite = this.attributesWeapon[i].img_Main.sprite;
			this.popUpRankUp_AttributesWeaponNew[i].img_Main.sprite = this.attributesWeapon[i].img_Main.sprite;
			if (i < data.properties.Length && data.properties[i].title == PopupManager.Instance.GetText(Localization0.Critical, null))
			{
				this.popUpRankUp_AttributesWeaponOld[i].txt_Amount.text = string.Concat(new object[]
				{
					this.attributesWeapon[i].valueCurrent,
					"%<color=#A0A0A0FF>/",
					this.attributesWeapon[i].valueCurrent,
					"%</color>"
				});
			}
			else
			{
				this.popUpRankUp_AttributesWeaponOld[i].txt_Amount.text = string.Concat(new object[]
				{
					this.attributesWeapon[i].valueCurrent,
					"<color=#A0A0A0FF>/",
					this.attributesWeapon[i].valueCurrent,
					"</color>"
				});
			}
			float num2;
			if (FormWeapon2.indexTab == ETypeWeapon.GRENADE)
			{
				num2 = this.attributesWeapon[i].valueNext;
			}
			else
			{
				num2 = this.attributesWeapon[i].valueCurrent;
			}
			if (i < data.properties.Length && data.properties[i].title == PopupManager.Instance.GetText(Localization0.Critical, null))
			{
				this.popUpRankUp_AttributesWeaponNew[i].txt_Amount.text = string.Concat(new object[]
				{
					num2,
					"%<color=#A0A0A0FF>/",
					this.attributesWeapon[i].valueNextRank,
					"%</color>"
				});
			}
			else
			{
				this.popUpRankUp_AttributesWeaponNew[i].txt_Amount.text = string.Concat(new object[]
				{
					num2,
					"<color=#A0A0A0FF>/",
					this.attributesWeapon[i].valueNextRank,
					"</color>"
				});
			}
			this.popUpRankUp_AttributesWeaponOld[i].img_Core.fillAmount = this.attributesWeapon[i].valueCurrent / this.attributesWeapon[i].valueMax;
			this.popUpRankUp_AttributesWeaponNew[i].img_Core.fillAmount = num2 / this.attributesWeapon[i].valueMax;
			this.popUpRankUp_AttributesWeaponOld[i].img_CoreMax.fillAmount = 0f;
			this.popUpRankUp_AttributesWeaponNew[i].img_CoreMax.fillAmount = this.attributesWeapon[i].valueNextRank / this.attributesWeapon[i].valueMax;
		}
		this.popUpRankUp_BtnRankUpFrag.SetActive(this.obj_RankUpFrag.activeSelf);
		this.popUpRankUp_BtnRankUpGold.SetActive(this.obj_RankUpGold.activeSelf);
		this.popUpRankUp_BtnRankUpTip.SetActive(this.obj_TipRankUp.activeSelf);
		this.popUpRankUp_txtRankUpGold.text = this.txt_RankUpGold.text;
		int num3 = this.GemRankUp[Mathf.Min(1, this.dataGunMain[FormWeapon2.indexWeapon].rankUpped)].Value;
		if (FormWeapon2.indexWeapon == 0)
		{
			num3 = 10;
		}
		this.popUpRankUp_txtRankUpGem.text = num3.ToString();
		Text text = this.popUpRankUp_txtRankUpFrag;
		string text2 = ProfileManager.weaponsRifle[FormWeapon2.indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, this.dataGunMain[FormWeapon2.indexWeapon].rankUpped)].ToString();
		this.txt_RankUpFrag.text = text2;
		text.text = text2;
		this.obj_PopUpRankUp.SetActive(true);
	}

	public void ClosePopupUpgrade()
	{
		
		this.obj_PopUpUpgrade.SetActive(false);
	}

	public void ClosePopupRankUp()
	{
		
		this.obj_PopUpRankUp.SetActive(false);
	}

	public void BtnInforFrag()
	{
		UnityEngine.Debug.Log("indexWeapon____________" + FormWeapon2.indexWeapon);
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		if (this.obj_Upgrade_RankUp.activeSelf)
		{
			MenuManager.Instance.popupInformation.Show(data.CurrencyRankUp, data.costRankUp);
		}
		else if (this.obj_Unlock.activeSelf)
		{
			MenuManager.Instance.popupInformation.Show(data.CurrencyUnlock, data.costUnlockByGold);
		}
	}

	public void BtnInforPassive(int index)
	{
		InforWeapon data = this.GetData((int)FormWeapon2.indexTab, FormWeapon2.indexWeapon);
		PopupManager.Instance.ShowDialog(null, 0, data.passiveDesc[index], string.Concat(new object[]
		{
			data.nameWeapon,
			"(Passive ",
			index + 1,
			")"
		}));
	}

	private SecuredInt[] GemRankUp;

	public static ETypeWeapon indexTab;

	private static int indexWeapon;

	private string nameWeaponSelected;

	private InforWeapon[] dataGunMain;

	private InforWeapon[] dataGunSpecial;

	private InforWeapon[] dataMelee;

	private InforWeapon[] dataBomb;

	private float[] listMaxPrimary;

	private float[] listMaxSpecial;

	private float[] listMaxKnife;

	private float[] listMaxBomb;

	[Header("---------------Tab Bar---------------")]
	public RectTransform rect_TabBar;

	public CardBase[] cardsTab;

	[Header("---------------List Weapon---------------")]
	public CardWeapon[] cardsWeapon;

	public Scrollbar scrollbarWepon;

	public RectTransform rect_ListWeapon;

	public string[] aniRank = new string[]
	{
		string.Empty,
		string.Empty,
		"capa",
		"caps",
		"capss"
	};

	[Header("---------------Table Info---------------")]
	public Text txt_Name;

	public Text txt_Power;

	public Image img_Rank;

	public Image img_BGName;

	public CardAttributeWeapon[] attributesWeapon;

	public PreviewWeapon preview;

	[Header("---------------Unlock---------------")]
	public GameObject obj_Unlock;

	public GameObject obj_BtnUnlockByFrag;

	public GameObject obj_BtnUnlockByGem;

	public GameObject obj_TipUnlockFrag;

	public GameObject obj_TipUnlockGem;

	public GameObject obj_InforUnlockFrag;

	public Text txt_UnlockFrag;

	public Text txt_UnlockGem;

	[Header("---------------Upgrade---------------")]
	public GameObject obj_BtnUpgrade;

	public GameObject obj_TipUpgrade;

	public Text txt_UpgradeGold;

	[Header("---------------RankUp---------------")]
	public GameObject obj_BtnRankUp;

	public GameObject obj_TipRankUp;

	public GameObject obj_InforRankUp;

	public GameObject obj_RankUpFrag;

	public GameObject obj_RankUpGold;

	public Text txt_RankUpFrag;

	public Text txt_RankUpGem;

	public Text txt_RankUpGold;

	public GameObject obj_Upgrade_RankUp;

	public GameObject obj_GroupButton;

	public GameObject obj_Maximum;

	[Header("---------------Effect---------------")]
	public EffectUpgrade effUpgrade;

	public EffectUnlock[] listEffUnlock;

	[Header("---------------PopUpUpgrade---------------")]
	public CardWeapon popUpUpgrade_CardsWeapon;

	public CardAttributeWeapon[] popUpUpgrade_AttributesWeapon;

	public Text popUpUpgrade_txtLevel;

	public Text popUpUpgrade_txtLevelNext;

	public Text popUpUpgrade_txtPower;

	public Text popUpUpgrade_txtPowerNext;

	public Text popUpUpgrade_txtUpgradeGold;

	public GameObject popUpUpgrade_BtnUpgrade;

	public GameObject popUpUpgrade_BtnUpgradeTip;

	public GameObject obj_PopUpUpgrade;

	[Header("---------------PopUpRankUp---------------")]
	public CardWeapon popUpRankUp_CardsWeaponOld;

	public CardWeapon popUpRankUp_CardsWeaponNew;

	public CardAttributeWeapon[] popUpRankUp_AttributesWeaponOld;

	public CardAttributeWeapon[] popUpRankUp_AttributesWeaponNew;

	public Image popUpRankUp_ImgRankOld;

	public Image popUpRankUp_ImgRankNew;

	public Text popUpRankUp_txtRankUpFrag;

	public Text popUpRankUp_txtRankUpGem;

	public Text popUpRankUp_txtRankUpGold;

	public GameObject popUpRankUp_BtnRankUp;

	public GameObject popUpRankUp_BtnRankUpFrag;

	public GameObject popUpRankUp_BtnRankUpGold;

	public GameObject popUpRankUp_BtnRankUpTip;

	public GameObject obj_PopUpRankUp;

	private bool isFirst;

	private SecuredInt SecuredGoldUnlock;

	private SecuredInt SecuredGemUnlock;

	public Image imgIconMoney1;

	public Image imgIconMoney2;
}
