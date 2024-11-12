using System;
using UnityEngine;
using UnityEngine.UI;

public class VipPopup : PopupBase
{
	public override void OnClose()
	{
		try
		{
			this.Closed(true);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		base.OnClose();
	}

	public override void Show()
	{
		base.Show();
		this._vipLevel = (int)ProfileManager.inAppProfile.vipProfile.level;
		FireBaseManager.Instance.LogEvent(FireBaseManager.VIPGET_MARKETING + Math.Max(0, this._vipLevel));
		InAppManager.Instance.FlyerTrackingEvent(FireBaseManager.VIPGET_MARKETING, Math.Max(0, this._vipLevel) + string.Empty);
		this.CheckReward();
		this.ShowCurrentInfo();
		this.ShowVipPanel();
		string text = string.Empty;
		try
		{
			text = InAppManager.Instance.GetPrice(InAppManager.Instance.ListSkuCurrent[0]);
		}
		catch
		{
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0.99$";
		}
		string[] nameSpec = new string[]
		{
			text
		};
		this.txtNote.text = PopupManager.Instance.GetText(Localization0.You_will_get_40_vip_points_for_every_spent, nameSpec);
		if ((float)Screen.width < (float)Screen.height * 1.7f)
		{
			this.tableObj.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		else
		{
			this.tableObj.localScale = Vector3.one;
		}
		E_Vip level = ProfileManager.inAppProfile.vipProfile.level;
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_LevelUpVip)
		{
			MenuManager.Instance.tutorial.listTutorialUI[12].cardsTutorial[1].obj_Target = this.obj_ListTextDailyBenefits;
			MenuManager.Instance.tutorial.NextTutorial(1);
			MenuManager.Instance.tutorial.listTutorialUI[12].cardsTutorial[2].obj_Target = this.btn_GetReward.gameObject;
		}
		int num = Mathf.Max((int)level, 0);
		for (int i = 0; i < this.vipInfos.Length; i++)
		{
			this.vipInfos[i].SetActive(i == num);
		}
		try
		{
			this.vipDes.Show(DataLoader.vipData.Levels[num]);
		}
		catch (Exception ex)
		{
		}
	}

	public void ShowVip(int vipId)
	{
		AudioClick.Instance.OnClick();
		this._vipLevel = vipId;
		this.ShowVipPanel();
		try
		{
			this.vipDes.Show(DataLoader.vipData.Levels[vipId]);
		}
		catch (Exception ex)
		{
		}
	}

	public void GetVipReward()
	{
		ProfileManager.inAppProfile.vipProfile.ReceiveGift(E_VipGiftStyle.Rewards, this._vipLevel);
		this.ActiveThongBao(this._vipLevel);
		this.btn_GetReward.interactable = false;
		this.obj_EffectBtnGetReward.gameObject.SetActive(this.btn_GetReward.interactable);
		this.imgReceived.enabled = true;
		AudioClick.Instance.OnClick();
		try
		{
			InforReward[] array = new InforReward[DataLoader.vipData.Levels[this._vipLevel].nowReward.items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new InforReward();
				VipReward nowReward = DataLoader.vipData.Levels[this._vipLevel].nowReward;
				array[i].amount = nowReward.amounts[i];
				array[i].item = (Item)nowReward.items[i];
			}
			PopupManager.Instance.ShowCongratulation(array, true, null);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("____Error:" + ex.Message);
		}
		try
		{
		}
		catch
		{
		}
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_LevelUpVip)
		{
			MenuManager.Instance.tutorial.NextTutorial(3);
		}
		if (MenuManager.Instance.tutorial.listTutorialUI[12] != null)
		{
			PlayerPrefs.SetInt(MenuManager.Instance.tutorial.listTutorialUI[12].keyPlayerPrefs, 1);
		}
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
	
	}

	private void ShowCurrentInfo()
	{
		this.imgIconVipCurent.sprite = this.spriteVips[Mathf.Min(this._vipLevel + 1, this.spriteVips.Length - 1)];
		this.imgIconVipNext.sprite = this.spriteVips[Mathf.Min(this._vipLevel + 2, this.spriteVips.Length - 1)];
		int num = Mathf.Min(this._vipLevel + 1, Enum.GetValues(typeof(E_Vip)).Length - 2);
		float num2 = (float)ProfileManager.inAppProfile.vipProfile.Point;
		this.slider.value = (num2 - (float)((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0)) / (float)(DataLoader.vipData.Levels[num].point - ((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0));
		this.txtProgress.text = num2 - (float)((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0) + " / " + (DataLoader.vipData.Levels[num].point - ((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0));
	}

	private void ShowVipPanel()
	{
		this.ShowBtnVip();
		int num = Math.Max(0, this._vipLevel);
		for (int i = 0; i < this.vipInfos.Length; i++)
		{
			this.vipInfos[i].SetActive(i == num);
		}
		this.btn_GetReward.interactable = (num != -1 && this.imgThongBaos[num].enabled);
		this.obj_EffectBtnGetReward.gameObject.SetActive(this.btn_GetReward.interactable);
		this.imgReceived.enabled = (num != -1 && !ProfileManager.inAppProfile.vipProfile.HasGift((E_Vip)num));
	}

	private void ShowBtnVip()
	{
		if (this._vipLevel < 0)
		{
			for (int i = 0; i < this.imgBtnVips.Length; i++)
			{
				this.imgBtnVips[i].enabled = false;
			}
			this.imgBtnVips[0].enabled = true;
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
		
		}
		else
		{
			for (int j = 0; j < this.imgBtnVips.Length; j++)
			{
				this.imgBtnVips[j].enabled = (j == this._vipLevel);
			}
			string parameterValue2 = "GamePlay";
			try
			{
				parameterValue2 = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			
		}
	}

	private void CheckReward()
	{
		for (int i = 0; i < this.imgThongBaos.Length; i++)
		{
			this.ActiveThongBao(i);
		}
	}

	private void ActiveThongBao(int id)
	{
		this.imgThongBaos[id].enabled = (ProfileManager.inAppProfile.vipProfile.HasGift((E_Vip)id) && ProfileManager.inAppProfile.vipProfile.Point >= DataLoader.vipData.Levels[id].point);
	}

	[Header("Sprite VIP:")]
	[SerializeField]
	private Sprite[] spriteVips;

	[SerializeField]
	[Header("Vip UI:")]
	private Image imgIconVipCurent;

	[SerializeField]
	private Image imgIconVipNext;

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private Text txtProgress;

	[SerializeField]
	private Text txtNote;

	[SerializeField]
	private Image[] imgBtnVips;

	[SerializeField]
	private Image[] imgThongBaos;

	[SerializeField]
	private GameObject[] vipInfos;

	[SerializeField]
	private Image imgReceived;

	[SerializeField]
	private Button btn_GetReward;

	[SerializeField]
	private GameObject obj_ListTextDailyBenefits;

	public GameObject obj_EffectBtnGetReward;

	private int _vipLevel;

	[SerializeField]
	private RectTransform tableObj;

	[Header("---------------Show()---------------")]
	public Action<bool> Closed;

	public bool isInapp;

	public VipPopupDescript vipDes;
}
