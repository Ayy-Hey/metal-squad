using System;
using System.Collections;
using CustomControl;
using Rank;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public static PopupManager Instance
	{
		get
		{
			if (PopupManager.instance == null)
			{
				PopupManager.instance = UnityEngine.Object.FindObjectOfType<PopupManager>();
			}
			return PopupManager.instance;
		}
	}

	private void Start()
	{
		int @int = PlayerPrefs.GetInt(this.SaveLanguage, -1);
		if (@int >= 0)
		{
			PopupManager.language = (Language)@int;
		}
		else
		{
			PopupManager.language = Language.English;
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void SetCanvas(Canvas canvas)
	{
		this.mCanvas = canvas;
	}

	public void SetFormCanvas(Canvas canvas)
	{
		this.formCanvas = canvas;
	}

	public void ShowWarningUpdateGame()
	{
		if (this.mPopupWarningUpdateGame == null)
		{
			this.mPopupWarningUpdateGame = UnityEngine.Object.Instantiate<PopupWarningUpdateGame>(Resources.Load<PopupWarningUpdateGame>("Popup/Popup_Warning_UpdateGame"), this.mCanvas.transform);
			this.mPopupWarningUpdateGame.transform.localScale = Vector3.one;
			this.mPopupWarningUpdateGame.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		this.mPopupWarningUpdateGame.Show();
		this.mPopupWarningUpdateGame.transform.SetAsLastSibling();
	}

	public void ShowPopupCustomControl(int idControl, Action callback = null)
	{
		if (!this.popupCustomControl)
		{
			this.popupCustomControl = UnityEngine.Object.Instantiate<PopupCustomControl>(this.prefabPopupCustomControl);
			this.popupCustomControl.transform.position = Vector3.zero;
			this.popupCustomControl.transform.parent = base.transform;
		}
		this.popupCustomControl.Show(idControl, callback);
	}

	public void ShowMiniLoading()
	{
		if (this.miniLoading == null)
		{
			this.miniLoading = (UnityEngine.Object.Instantiate(Resources.Load(this.paths[9], typeof(MiniLoading)), this.mCanvas.transform) as MiniLoading);
		}
		this.miniLoading.Show();
		this.miniLoading.transform.localScale = Vector3.one;
		this.miniLoading.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		this.miniLoading.transform.SetAsLastSibling();
	}

	public void CloseMiniLoading()
	{
		if (this.coroutineMiniLoading != null)
		{
			base.StopCoroutine(this.coroutineMiniLoading);
		}
		this.coroutineMiniLoading = base.StartCoroutine(this.waitMiniLoading());
	}

	private IEnumerator waitMiniLoading()
	{
		yield return new WaitUntil(() => this.miniLoading != null && this.miniLoading.gameObject.activeSelf);
		this.miniLoading.OnClose();
		yield break;
	}

	public void ShowDialog(Action<bool> isOK, int typeDialog, string content, string title)
	{
		if (this.mDialog == null)
		{
			this.mDialog = (UnityEngine.Object.Instantiate(Resources.Load(this.paths[6], typeof(Dialog2)), this.mCanvas.transform) as Dialog2);
		}
		AudioClick.Instance.OnClick();
		this.mDialog.typeDialog = typeDialog;
		this.mDialog.isOK = isOK;
		this.mDialog.txtTitle.text = title;
		this.mDialog.txtContent.text = content;
		this.mDialog.Show();
		this.mDialog.transform.localScale = Vector3.one;
		this.mDialog.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		this.mDialog.transform.SetAsLastSibling();
	}

	public void ShowInapp(INAPP_TYPE type, Action<bool> isClosed)
	{
		if (this.mInappPopup == null)
		{
			this.mInappPopup = (UnityEngine.Object.Instantiate(Resources.Load(this.paths[5], typeof(InappPopup)), this.mCanvas.transform) as InappPopup);
		}
		this.mInappPopup.transform.localScale = Vector3.one;
		this.mInappPopup.Show(type, isClosed);
		this.mInappPopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		this.mInappPopup.transform.SetAsLastSibling();
	}

	public void ShowVipPopup(Action<bool> isClosed, bool isInapp)
	{
		if (this.mVipPopup == null)
		{
			this.mVipPopup = UnityEngine.Object.Instantiate<VipPopup>(Resources.Load<VipPopup>("Popup/VipPopup"), this.mCanvas.transform);
			this.mVipPopup.transform.localScale = Vector3.one;
			this.mVipPopup.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		this.mVipPopup.Closed = isClosed;
		this.mVipPopup.isInapp = isInapp;
		this.mVipPopup.Show();
		this.mVipPopup.transform.SetAsLastSibling();
	}

	public void ShowCongratulation(InforReward[] list, bool isDoubleReward, Action actionClose = null)
	{
		if (this.mpopupCongratulation == null)
		{
			this.mpopupCongratulation = UnityEngine.Object.Instantiate<PopupCongratulation>(Resources.Load<PopupCongratulation>("Popup/Popup_Congratulation"), this.mCanvas.transform);
		}
		this.mpopupCongratulation.ShowListReward(list, actionClose, isDoubleReward);
		this.mpopupCongratulation.transform.localScale = Vector3.one;
		this.mpopupCongratulation.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		this.mpopupCongratulation.transform.SetAsLastSibling();
	}

	public void ShowGacha()
	{
		if (this.mPopupGacha == null)
		{
			if (this.formCanvas != null)
			{
				this.mPopupGacha = UnityEngine.Object.Instantiate<PopupGacha>(Resources.Load<PopupGacha>("Popup/Popup_Gacha"), this.formCanvas.transform);
				this.mPopupGacha.transform.SetAsLastSibling();
			}
			else
			{
				this.mPopupGacha = UnityEngine.Object.Instantiate<PopupGacha>(Resources.Load<PopupGacha>("Popup/Popup_Gacha"), this.mCanvas.transform);
				this.mPopupGacha.transform.SetAsFirstSibling();
			}
		}
		this.mPopupGacha.transform.localScale = Vector3.one;
		this.mPopupGacha.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		this.mPopupGacha.Show();
	}

	public void ShowRankRewardPopup(RankInfor infor, Action<bool> closed)
	{
		if (this.mRankRewardPopup == null)
		{
			this.mRankRewardPopup = UnityEngine.Object.Instantiate<RankRewardPopup>(Resources.Load<RankRewardPopup>("Popup/RankRewardPopup"), this.mCanvas.transform);
			this.mRankRewardPopup.transform.localScale = Vector3.one;
			this.mRankRewardPopup.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		AudioClick.Instance.OnClick();
		this.mRankRewardPopup.infor = infor;
		this.mRankRewardPopup.onClosed = closed;
		this.mRankRewardPopup.Show();
		this.mRankRewardPopup.transform.SetAsLastSibling();
	}

	public void ShowRatePopup(Action<bool> isOk)
	{
		if (this.mPopupRate == null)
		{
			this.mPopupRate = UnityEngine.Object.Instantiate<PopupRate>(Resources.Load<PopupRate>("Popup/Popup_RateUs"), this.mCanvas.transform);
			this.mPopupRate.transform.localScale = Vector3.one;
			this.mPopupRate.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		AudioClick.Instance.OnClick();
		this.mPopupRate.Show();
		this.mPopupRate.isOk = isOk;
		this.mPopupRate.transform.SetAsLastSibling();
	}

	public void ShowShopItem(Action hideCallback)
	{
		if (!this.mPopupShopItem)
		{
			this.mPopupShopItem = UnityEngine.Object.Instantiate<ShopItem>(this.popupShopItem, this.mCanvas.transform);
			this.mPopupShopItem.transform.localScale = Vector3.one;
			this.mPopupShopItem.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		this.mPopupShopItem.Show(hideCallback);
	}

	public bool CloseAll()
	{
		if (this.mPopupWarningUpdateGame != null && this.mPopupWarningUpdateGame.gameObject.activeSelf)
		{
			this.mPopupWarningUpdateGame.OnClose();
			return true;
		}
		if (this.mDialog != null && this.mDialog.gameObject.activeSelf)
		{
			this.mDialog.OnClose();
			return true;
		}
		if (this.mpopupCongratulation != null && this.mpopupCongratulation.gameObject.activeSelf)
		{
			this.mpopupCongratulation.OnClose();
			return true;
		}
		if (this.mVipPopup != null && this.mVipPopup.gameObject.activeSelf && this.mVipPopup.transform.localScale.x > 0f)
		{
			this.mVipPopup.OnClose();
			return true;
		}
		if (this.mInappPopup != null && this.mInappPopup.gameObject.activeSelf && this.mInappPopup.transform.localScale.x > 0f)
		{
			this.mInappPopup.OnClose();
			return true;
		}
		if (this.mPopupRate != null && this.mPopupRate.gameObject.activeSelf && this.mPopupRate.transform.localScale.x > 0f)
		{
			this.mPopupRate.OnClose();
			return true;
		}
		if (this.mRankRewardPopup != null && this.mRankRewardPopup.gameObject.activeSelf && this.mRankRewardPopup.transform.localScale.x > 0f)
		{
			this.mRankRewardPopup.OnClose();
			return true;
		}
		if (this.mPopupGacha != null && this.mPopupGacha.gameObject.activeSelf && this.mPopupGacha.transform.localScale.x > 0f)
		{
			if (this.mPopupGacha.objInfor.activeSelf)
			{
				this.mPopupGacha.DisableInforBox();
				return true;
			}
			this.mPopupGacha.OnClose();
			return true;
		}
		else
		{
			if (this.popupCustomControl && this.popupCustomControl.gameObject.activeSelf)
			{
				this.popupCustomControl.Off();
				return true;
			}
			if (this.mPopupShopItem && this.mPopupShopItem.isShow)
			{
				this.mPopupShopItem.Close();
				return true;
			}
			return false;
		}
	}

	public int GetAmountGemIsHad(ItemConvert type, int index)
	{
		int result = 1;
		switch (type)
		{
		case ItemConvert.Character:
			switch (index)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 100;
				break;
			case 2:
				result = 200;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/GetAmountGemIsHad/Character");
				break;
			}
			break;
		case ItemConvert.GunMain:
			switch (index)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 50;
				break;
			case 2:
				result = 100;
				break;
			case 3:
				result = 150;
				break;
			case 4:
				result = 200;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/GetAmountGemIsHad/GunMain");
				break;
			}
			break;
		case ItemConvert.GunSpecial:
			switch (index)
			{
			case 0:
				result = 20;
				break;
			case 1:
				result = 100;
				break;
			case 2:
				result = 150;
				break;
			case 3:
				result = 200;
				break;
			case 4:
				result = 250;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/GetAmountGemIsHad/GunSpecial");
				break;
			}
			break;
		case ItemConvert.Melee:
			switch (index)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 10;
				break;
			case 2:
				result = 20;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/GetAmountGemIsHad/Melee");
				break;
			}
			break;
		default:
			UnityEngine.Debug.LogError("Lỗi MenuManager/GetAmountGemIsHad");
			break;
		}
		return result;
	}

	public bool CheckAmountItem(Item item)
	{
		switch (item)
		{
		case Item.Hammer:
		case Item.Silver_Axe:
		case Item.Sword:
		case Item.M4A1:
		case Item.Machine_Gun:
		case Item.Ice_Gun:
		case Item.Sniper:
		case Item.MGL_140:
		case Item.Spread_Gun:
		case Item.Flame_Gun:
		case Item.Thunder_Shot:
		case Item.Laser_Gun:
		case Item.Rocket:
		case Item.John_D:
		case Item.Yoo_na:
		case Item.Dvornikov:
			return false;
		}
		return true;
	}

	public int ConvertToIndexItem(ItemConvert type, int index)
	{
		int result = 1;
		switch (type)
		{
		case ItemConvert.Character:
			switch (index)
			{
			case 0:
				result = 34;
				break;
			case 1:
				result = 35;
				break;
			case 2:
				result = 36;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem/Character");
				break;
			}
			break;
		case ItemConvert.GunMain:
			switch (index)
			{
			case 0:
				result = 24;
				break;
			case 1:
				result = 25;
				break;
			case 2:
				result = 26;
				break;
			case 3:
				result = 29;
				break;
			case 4:
				result = 28;
				break;
			case 5:
				result = 54;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem/GunMain");
				break;
			}
			break;
		case ItemConvert.GunSpecial:
			switch (index)
			{
			case 0:
				result = 30;
				break;
			case 1:
				result = 27;
				break;
			case 2:
				result = 32;
				break;
			case 3:
				result = 55;
				break;
			case 4:
				result = 33;
				break;
			case 5:
				result = 31;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem/GunSpecial");
				break;
			}
			break;
		case ItemConvert.Melee:
			switch (index)
			{
			case 0:
				result = 17;
				break;
			case 1:
				result = 18;
				break;
			case 2:
				result = 19;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem/Melee");
				break;
			}
			break;
		case ItemConvert.Bomb:
			switch (index)
			{
			case 0:
				result = 20;
				break;
			case 1:
				result = 21;
				break;
			case 2:
				result = 22;
				break;
			case 3:
				result = 23;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem/Bomb");
				break;
			}
			break;
		case ItemConvert.Gacha:
			switch (index)
			{
			case 0:
				result = 38;
				break;
			case 1:
				result = 39;
				break;
			case 2:
				result = 40;
				break;
			default:
				UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem/Gacha");
				break;
			}
			break;
		default:
			UnityEngine.Debug.LogError("Lỗi MenuManager/ConvertToIndexItem");
			break;
		}
		return result;
	}

	public int GetRankItem(Item item)
	{
		switch (item)
		{
		case Item.Gold:
		case Item.Gold2:
		case Item.Gold3:
		case Item.Booster_X2Exp:
		case Item.Booster_X2Gold:
		case Item.Sword:
		case Item.M61_Grenades:
		case Item.M4A1:
		case Item.Flame_Gun:
		case Item.John_D:
		case Item.Common_Crate:
		case Item.M4A1_Fragment:
		case Item.Flame_Gun_Fragment:
		case Item.John_D_Fragment:
			return 2;
		case Item.Gem:
		case Item.Chemical:
		case Item.MGL_140:
		case Item.Spread_Gun:
		case Item.Thunder_Shot:
		case Item.Rocket:
		case Item.Dvornikov:
		case Item.Exp_Vip:
		case Item.Epic_Crate:
		case Item.MGL_140_Fragment:
		case Item.Spread_Gun_Fragment:
		case Item.Thunder_Shot_Fragment:
		case Item.Rocket_Fragment:
		case Item.Dvornikov_Fragment:
		case Item.Ct9_Gun:
		case Item.Ct9_Gun_Fragment:
			return 4;
		case Item.Medkit:
		case Item.Exp:
		case Item.Ice_Grenades:
		case Item.Molotov:
		case Item.Machine_Gun:
		case Item.Ice_Gun:
		case Item.Sniper:
		case Item.Laser_Gun:
		case Item.Yoo_na:
		case Item.Machine_Gun_Fragment:
		case Item.Ice_Gun_Fragment:
		case Item.Sniper_Fragment:
		case Item.Laser_Gun_Fragment:
		case Item.Yoo_na_Fragment:
		case Item.Fc10_Gun:
		case Item.Fc10_Gun_Fragment:
			return 3;
		case Item.Silver_Axe:
			return 1;
		}
		return 0;
	}

	public int GetRankItemStart(Item item)
	{
		switch (item)
		{
		case Item.Hammer:
		case Item.M61_Grenades:
		case Item.M4A1:
		case Item.Flame_Gun:
		case Item.John_D:
		case Item.M4A1_Fragment:
		case Item.Flame_Gun_Fragment:
		case Item.John_D_Fragment:
			return 0;
		case Item.Silver_Axe:
		case Item.Ice_Grenades:
		case Item.Molotov:
		case Item.Machine_Gun:
		case Item.Ice_Gun:
		case Item.Sniper:
		case Item.Laser_Gun:
		case Item.Yoo_na:
		case Item.Common_Crate:
		case Item.Machine_Gun_Fragment:
		case Item.Ice_Gun_Fragment:
		case Item.Sniper_Fragment:
		case Item.Laser_Gun_Fragment:
		case Item.Yoo_na_Fragment:
		case Item.Fc10_Gun:
		case Item.Fc10_Gun_Fragment:
			return 1;
		case Item.Sword:
		case Item.Chemical:
		case Item.MGL_140:
		case Item.Spread_Gun:
		case Item.Thunder_Shot:
		case Item.Rocket:
		case Item.Dvornikov:
		case Item.Epic_Crate:
		case Item.MGL_140_Fragment:
		case Item.Spread_Gun_Fragment:
		case Item.Thunder_Shot_Fragment:
		case Item.Rocket_Fragment:
		case Item.Dvornikov_Fragment:
		case Item.Ct9_Gun:
		case Item.Ct9_Gun_Fragment:
			return 2;
		}
		return 0;
	}

	public void SaveReward(Item item, int amount, string location, Action<bool> callback = null)
	{
		if (amount == 0)
		{
			return;
		}
		if (location == string.Empty && MenuManager.Instance != null && MenuManager.Instance.formCurrent != null)
		{
			location = "?" + MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		location = ((!(MenuManager.Instance != null)) ? ("InGame_" + location) : ("GUI_" + location));
		switch (item)
		{
		case Item.Gold:
		case Item.Gold2:
		case Item.Gold3:
			ProfileManager.userProfile.Coin += amount;
			if (MenuManager.Instance != null)
			{
				MenuManager.Instance.topUI.ShowCoin();
			}
			if (this.mInappPopup != null)
			{
				this.mInappPopup.ShowCoin();
			}
			
			break;
		case Item.Gem:
			ProfileManager.userProfile.Ms += amount;
			if (MenuManager.Instance != null)
			{
				MenuManager.Instance.topUI.ShowCoin();
			}
			if (this.mInappPopup != null)
			{
				this.mInappPopup.ShowCoin();
			}
			
			break;
		case Item.Medkit:
			ProfileManager.boosterProfile.ItemAdd(EBooster.MEDKIT, amount);
			
			break;
		case Item.Booster_Amor:
			ProfileManager.boosterProfile.ItemAdd(EBooster.ARMOR, amount);
			
			break;
		case Item.Booster_Crit:
			ProfileManager.boosterProfile.ItemAdd(EBooster.CRITICAL_HIT, amount);
			
			break;
		case Item.Booster_Damage:
			ProfileManager.boosterProfile.ItemAdd(EBooster.DAMAGE, amount);
			
			break;
		case Item.Booster_Maget:
			ProfileManager.boosterProfile.ItemAdd(EBooster.MAGNET, amount);
			
			break;
		case Item.Booster_Skill:
			ProfileManager.boosterProfile.ItemAdd(EBooster.SKILL, amount);
			
			break;
		case Item.Booster_Speed:
			ProfileManager.boosterProfile.ItemAdd(EBooster.SPEED, amount);
			
			break;
		case Item.Booster_X2Exp:
			ProfileManager.boosterProfile.ItemAdd(EBooster.DOUBLE_EXP, amount);
			
			break;
		case Item.Booster_X2Gold:
			ProfileManager.boosterProfile.ItemAdd(EBooster.DOUBLE_COIN, amount);
			
			break;
		case Item.Ticket_Spin:
			ProfileManager.spinProfile.TotalSpin += amount;
			
			break;
		case Item.Ticket_BossMode:
			break;
		case Item.Exp:
			RankManager.Instance.AddExp(amount, callback);
			break;
		case Item.Hammer:
			break;
		case Item.Silver_Axe:
			ProfileManager.melesProfile[1].Unlock = true;
			
			break;
		case Item.Sword:
			ProfileManager.melesProfile[2].Unlock = true;
			
			break;
		case Item.M61_Grenades:
			ProfileManager.grenadesProfile[0].TotalBomb += amount;
			
			break;
		case Item.Ice_Grenades:
			ProfileManager.grenadesProfile[1].TotalBomb += amount;
			
			break;
		case Item.Molotov:
			ProfileManager.grenadesProfile[2].TotalBomb += amount;
			
			break;
		case Item.Chemical:
			ProfileManager.grenadesProfile[3].TotalBomb += amount;
			
			break;
		case Item.M4A1:
			break;
		case Item.Machine_Gun:
			ProfileManager.weaponsRifle[1].SetGunBuy(true);
			
			break;
		case Item.Ice_Gun:
			ProfileManager.weaponsRifle[2].SetGunBuy(true);
			
			break;
		case Item.Sniper:
			ProfileManager.weaponsSpecial[1].SetGunBuy(true);
			
			break;
		case Item.MGL_140:
			ProfileManager.weaponsRifle[4].SetGunBuy(true);
			
			break;
		case Item.Spread_Gun:
			ProfileManager.weaponsRifle[3].SetGunBuy(true);
			
			break;
		case Item.Flame_Gun:
			ProfileManager.weaponsSpecial[0].SetGunBuy(true);
			
			break;
		case Item.Thunder_Shot:
			ProfileManager.weaponsSpecial[5].SetGunBuy(true);
			
			break;
		case Item.Laser_Gun:
			ProfileManager.weaponsSpecial[2].SetGunBuy(true);
			
			break;
		case Item.Rocket:
			ProfileManager.weaponsSpecial[4].SetGunBuy(true);
			
			break;
		case Item.John_D:
			break;
		case Item.Yoo_na:
			ProfileManager.InforChars[1].IsUnLocked = true;
			
			break;
		case Item.Dvornikov:
			ProfileManager.InforChars[2].IsUnLocked = true;
			
			break;
		case Item.Exp_Vip:
			ProfileManager.inAppProfile.vipProfile.Point += amount;
			if (this.mInappPopup && this.mInappPopup.gameObject.activeSelf)
			{
				this.mInappPopup.vipBar.CheckVipLevel();
			}
			break;
		case Item.Common_Crate:
			ProfileManager.userProfile.SetGachaValue(0, amount);
			
			break;
		case Item.Epic_Crate:
			ProfileManager.userProfile.SetGachaValue(1, amount);
			
			break;
		case Item.Gacha_T3:
			ProfileManager.userProfile.SetGachaValue(2, amount);
			
			break;
		case Item.M4A1_Fragment:
		case Item.Machine_Gun_Fragment:
		case Item.Ice_Gun_Fragment:
		case Item.Sniper_Fragment:
		case Item.MGL_140_Fragment:
		case Item.Spread_Gun_Fragment:
		case Item.Flame_Gun_Fragment:
		case Item.Thunder_Shot_Fragment:
		case Item.Laser_Gun_Fragment:
		case Item.Rocket_Fragment:
		case Item.John_D_Fragment:
		case Item.Yoo_na_Fragment:
		case Item.Dvornikov_Fragment:
		case Item.Ct9_Gun_Fragment:
		case Item.Fc10_Gun_Fragment:
			PlayerPrefs.SetInt("metal.squad.frag." + item.ToString(), PlayerPrefs.GetInt("metal.squad.frag." + item.ToString(), 0) + amount);
			
			break;
		case Item.Ct9_Gun:
			ProfileManager.weaponsRifle[5].SetGunBuy(true);
			
			break;
		case Item.Fc10_Gun:
			ProfileManager.weaponsSpecial[3].SetGunBuy(true);
			
			break;
		default:
			UnityEngine.Debug.LogError("Lỗi MenuManager/SaveReward");
			break;
		}
	}

	public string GetText(Localization0 key, string[] nameSpec = null)
	{
		if (DataLoader.dataLocalization != null && DataLoader.dataLocalization.Count > (int)key)
		{
			string text = DataLoader.dataLocalization[(int)key][(int)PopupManager.language].ToString();
			if (text.Contains("{1}") && nameSpec != null && nameSpec.Length > 0)
			{
				string text2 = nameSpec[0];
				if (nameSpec[0].Contains("id:"))
				{
					text2 = text2.Substring(3);
					int num = -1;
					try
					{
						num = int.Parse(text2);
					}
					catch
					{
						UnityEngine.Debug.LogError("Lỗi int Parse PopupManager/GetText");
					}
					if (num >= 0)
					{
						text = text.Replace("{1}", DataLoader.dataLocalization[num][(int)PopupManager.language].ToString());
					}
				}
				else
				{
					text = text.Replace("{1}", text2);
				}
			}
			if (text.Contains("{2}") && nameSpec != null && nameSpec.Length > 1)
			{
				string text3 = nameSpec[1];
				if (nameSpec[1].Contains("id:"))
				{
					text3 = text3.Substring(3);
					int num2 = -1;
					try
					{
						num2 = int.Parse(text3);
					}
					catch
					{
						UnityEngine.Debug.LogError("Lỗi int Parse PopupManager/GetText");
					}
					if (num2 >= 0)
					{
						text = text.Replace("{2}", DataLoader.dataLocalization[num2][(int)PopupManager.language].ToString());
					}
				}
				else
				{
					text = text.Replace("{2}", text3);
				}
			}
			return text;
		}
		UnityEngine.Debug.LogWarning("Lỗi Localization: GetIndex " + key);
		return string.Empty;
	}

	private static PopupManager instance;

	public Canvas mCanvas;

	public Canvas formCanvas;

	private EPopup ePopup;

	private string[] paths = new string[]
	{
		"1",
		"2",
		"3",
		"4",
		"Popup/Shop",
		"Popup/Inapp",
		"Popup/Dialog",
		"Popup/SalePack",
		"Popup/PopupStarterPack",
		"Popup/MiniLoading"
	};

	[NonSerialized]
	public MiniLoading miniLoading;

	[NonSerialized]
	public Dialog2 mDialog;

	[NonSerialized]
	public InappPopup mInappPopup;

	[NonSerialized]
	public VipPopup mVipPopup;

	[NonSerialized]
	public PopupCongratulation mpopupCongratulation;

	[NonSerialized]
	public RankRewardPopup mRankRewardPopup;

	[NonSerialized]
	public PopupGacha mPopupGacha;

	[NonSerialized]
	public PopupRate mPopupRate;

	[NonSerialized]
	public PopupWarningUpdateGame mPopupWarningUpdateGame;

	[NonSerialized]
	public ShopItem mPopupShopItem;

	[Header("---------------Library Popup---------------")]
	public PopupBase[] listPrefabPopup;

	public PopupCustomControl prefabPopupCustomControl;

	public ShopItem popupShopItem;

	[Header("---------------Library Sprite---------------")]
	public SpriteWeapon[] sprite_GunMain;

	public SpriteWeapon[] sprite_GunSpecial;

	public Sprite[] sprite_Melee;

	public SpriteWeapon[] sprite_Grenade;

	public Sprite sprite_Health;

	public Sprite[] sprite_Item;

	public Sprite[] sprite_BGRankItem;

	public Sprite[] sprite_IconRankItem;

	public Sprite[] sprite_IconRankItem_NoName;

	public Sprite[] sprite_BGCardRank;

	public Sprite[] sprite_RankAccount;

	public Sprite[] spriteAvatarBoss;

	public Sprite[] spriteVip;

	public Sprite[] spriteIconProperty;

	public Color[] color_Rank;

	public static Language language;

	public static bool isblockInput;

	[HideInInspector]
	public PopupCustomControl popupCustomControl;

	public string SaveLanguage = "language_default";

	private Coroutine coroutineMiniLoading;
}
