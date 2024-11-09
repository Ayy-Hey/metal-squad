using System;
using ABI;
using Profile.UI;
using Rank;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private void OnDestroy()
	{
		base.StopAllCoroutines();
	}

	private void Awake()
	{
		float num = Mathf.Clamp(((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f, 0f, 1f);
		if (num < 0.9f)
		{
			this.obj_FormMainMenu.transform.localScale = Vector3.one * 0.9f;
			this.obj_FormOther.transform.localScale = Vector3.one * 0.9f;
			this.leftRect.localScale = Vector3.one * 0.9f;
			this.rightRect.localScale = Vector3.one * 0.9f;
			this.bgRect.sizeDelta = new Vector2(this.bgRect.sizeDelta.x, 67.5f);
		}
		else
		{
			this.obj_FormMainMenu.transform.localScale = Vector3.one;
			this.obj_FormOther.transform.localScale = Vector3.one;
			this.leftRect.localScale = Vector3.one;
			this.rightRect.localScale = Vector3.one;
			this.bgRect.sizeDelta = new Vector2(this.bgRect.sizeDelta.x, 75f);
		}
	}

	public void ReloadMoney()
	{
		this.amountGold = (float)ProfileManager.userProfile.Coin;
		this.amountGem = (float)ProfileManager.userProfile.Ms;
	}

	public void Show()
	{
		if (ThisPlatform.IsIphoneX)
		{
			this.leftRect.localPosition = new Vector3(60f, this.leftRect.localPosition.y, 0f);
		}
		else
		{
			this.leftRect.localPosition = new Vector3(0f, this.leftRect.localPosition.y, 0f);
		}
		try
		{
			this.avatarRambo.sprite = this.imagesRambo[ProfileManager.settingProfile.IDChar];
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.Log("Error: " + arg);
		}
		this.objOffer.SetActive(SaleManager.Instance.ValueSale > 0f);
		if (!string.IsNullOrEmpty(ProfileManager.pvpProfile.AvatarUrl))
		{
			base.StartCoroutine(MonoSingleton<FBAndPlayfabMgr>.Instance.GetProfilePic(ProfileManager.pvpProfile.AvatarUrl, delegate(Texture2D tex)
			{
				try
				{
					this.avatarRambo.sprite = Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), Vector2.zero);
				}
				catch
				{
				}
			}));
		}
	}

	public void ShowOffer()
	{
		this.objOffer.SetActive(SaleManager.Instance.ValueSale > 0f);
	}

	public void ShowCoin()
	{
		this.ReloadCoin();
		this.txtStar.text = string.Empty + ProfileManager._CampaignProfile.GetTotalStar;
		this.imgVip.sprite = this.spriteVips[(int)(ProfileManager.inAppProfile.vipProfile.level + 1)];
		int level = RankManager.Instance.GetRankCurrentByExp(RankManager.Instance.GetTotalExp()).Level;
		RankInfor rankInfoByLevel = RankManager.Instance.GetRankInfoByLevel(level);
		RankInfor rankInfoByLevel2 = RankManager.Instance.GetRankInfoByLevel(Mathf.Clamp(level + 1, 0, DataLoader.rankInfor.Length - 1));
		this.imgRank.sprite = this.spriteRanks[level];
		this.txt_ExpRank.text = RankManager.Instance.ExpCurrent(RankManager.Instance.GetTotalExp(), rankInfoByLevel, rankInfoByLevel2);
		this.rect_ExpRankCore.sizeDelta = new Vector2(156f * RankManager.Instance.ExpCurrent01(RankManager.Instance.GetTotalExp(), rankInfoByLevel, rankInfoByLevel2), this.rect_ExpRankCore.sizeDelta.y);
	}

	public void ReloadCoin()
	{
		this.txtCoin.text = ProfileManager.userProfile.Coin.ToString();
		this.txtMS.text = ProfileManager.userProfile.Ms.ToString();
	}

	public void ShowInapp(int type)
	{
		
		switch (MenuManager.Instance.formCurrent.idForm)
		{
		case FormUI.Menu:
			MenuManager.Instance.formMainMenu.gameObject.SetActive(false);
			MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].gameObject.SetActive(false);
			goto IL_129;
		case FormUI.UICharacter:
		case FormUI.UILoadOut:
			MenuManager.Instance.formCurrent.gameObject.SetActive(false);
			MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].gameObject.SetActive(false);
			goto IL_129;
		}
		MenuManager.Instance.formCurrent.gameObject.SetActive(false);
		IL_129:
		PopupManager.Instance.ShowInapp((INAPP_TYPE)type, delegate(bool isClosed)
		{
			if (isClosed)
			{
				this.ShowCoin();
			}
			switch (MenuManager.Instance.formCurrent.idForm)
			{
			case FormUI.Menu:
				MenuManager.Instance.formMainMenu.gameObject.SetActive(true);
				MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].gameObject.SetActive(true);
				MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].Show();
				return;
			case FormUI.UICharacter:
				MenuManager.Instance.formCurrent.GetComponent<FormUpgradeCharacter1>().CheckTips();
				MenuManager.Instance.formCurrent.gameObject.SetActive(true);
				MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].gameObject.SetActive(true);
				MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].Show();
				return;
			case FormUI.UILoadOut:
				MenuManager.Instance.formCurrent.gameObject.SetActive(true);
				MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].gameObject.SetActive(true);
				MenuManager.Instance.MainCharacters[ProfileManager.settingProfile.IDChar].Show();
				return;
			case FormUI.UIWeapon:
				MenuManager.Instance.formCurrent.GetComponent<FormWeapon2>().CheckTipsAll();
				MenuManager.Instance.formCurrent.gameObject.SetActive(true);
				return;
			}
			MenuManager.Instance.formCurrent.gameObject.SetActive(true);
		});
	}

	public void OnSetting()
	{
		
		this.popupController.gameObject.SetActive(true);
		this.popupController.Show();
	}

	public void OnCloseSetting()
	{
		AudioClick.Instance.OnClick();
		this.popupController.OnClose();
	}

	public void OnShowVip()
	{
		PopupManager.Instance.ShowVipPopup(delegate(bool closed)
		{
			this.ShowCoin();
		}, false);
	}

	public void ShowProfile()
	{
		
		PlayfabManager.Instance.GetLeaderBoard(false).MyProfile.CoppyDataProfile();
		this.profile.OnShow(PlayfabManager.Instance.GetLeaderBoard(false).MyProfile, null);
	}

	public void ShowNamePlayer()
	{
		this.txt_NamePlayer.text = ProfileManager.InforChars[ProfileManager.settingProfile.IDChar].Name;
		if (!string.IsNullOrEmpty(ProfileManager.pvpProfile.UserName))
		{
			this.txt_NamePlayer.text = ProfileManager.pvpProfile.UserName;
		}
	}

	private float amountGold;

	private float amountGem;

	[SerializeField]
	private Text txt_NamePlayer;

	[SerializeField]
	private Text txt_ExpRank;

	[SerializeField]
	private RectTransform rect_ExpRankCore;

	[SerializeField]
	private Text txtStar;

	[SerializeField]
	private Text txtCoin;

	[SerializeField]
	private Text txtMS;

	[SerializeField]
	private Image avatarRambo;

	public Sprite[] imagesRambo;

	public Sprite[] spriteVips;

	[SerializeField]
	private Image imgVip;

	[SerializeField]
	private Sprite[] spriteRanks;

	[SerializeField]
	private Image imgRank;

	public GameObject objOffer;

	public PopupSetting popupController;

	public PopupProfile profile;

	public GameObject obj_BtnBack;

	public GameObject obj_Setting;

	public GameObject obj_FormMainMenu;

	public GameObject obj_FormOther;

	[Header("---------------RESIZE---------------")]
	public RectTransform leftRect;

	public RectTransform rightRect;

	public RectTransform bgRect;

	private bool isLoadingFBInfor;
}
