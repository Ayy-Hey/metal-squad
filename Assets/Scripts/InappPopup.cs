using System;
using System.Collections.Generic;
using com.dev.util.SecurityHelper;
using UnityEngine;
using UnityEngine.UI;

public class InappPopup : PopupBase
{
	private void OnEnable()
	{
		if (ThisPlatform.IsIphoneX)
		{
			this.rect_InforVip.localPosition = new Vector3(-40f, this.rect_InforVip.localPosition.y, 0f);
		}
		else
		{
			this.rect_InforVip.localPosition = new Vector3(0f, this.rect_InforVip.localPosition.y, 0f);
		}
		this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.y * (float)Screen.width / (float)Screen.height, this.rectTransform.sizeDelta.y);
	}

	public void Show(INAPP_TYPE type, Action<bool> isClosed)
	{
		base.Show();
		this.SecuredGold = new SecuredInt[6];
		this.SecuredGold[0] = new SecuredInt(5000);
		this.SecuredGold[1] = new SecuredInt(10000);
		this.SecuredGold[2] = new SecuredInt(25000);
		this.SecuredGold[3] = new SecuredInt(50000);
		this.SecuredGold[4] = new SecuredInt(100000);
		this.SecuredGold[5] = new SecuredInt(250000);
		this.SecuredGem = new SecuredInt[6];
		this.SecuredGem[0] = new SecuredInt(100);
		this.SecuredGem[1] = new SecuredInt(200);
		this.SecuredGem[2] = new SecuredInt(500);
		this.SecuredGem[3] = new SecuredInt(1000);
		this.SecuredGem[4] = new SecuredInt(2000);
		this.SecuredGem[5] = new SecuredInt(5000);
		this.isClosed = isClosed;
		this.XDefault = new List<float>();
		for (int i = 0; i < this.ListPacks.Count; i++)
		{
			this.XDefault.Add(this.ListPacks[i].anchoredPosition.x);
		}
		this.ShowInApp(type);
		this.ShowCoin();
		this.vipBar.Show();
		FireBaseManager.Instance.LogEvent("Show_Inapp");
		this.ReSortInappPacks();
		if (ProfileManager.inAppProfile.vipProfile.level >= E_Vip.Vip1)
		{
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
			this.txtNotf.text = PopupManager.Instance.GetText(Localization0.You_will_get_40_vip_points_for_every_spent, nameSpec);
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

	public void ShowCoin()
	{
		this.txtCoin.text = ProfileManager.userProfile.Coin.ToString();
		this.txtMS.text = ProfileManager.userProfile.Ms.ToString();
	}

	public void ShowInApp(int type)
	{
		this.ShowInApp((INAPP_TYPE)type);
	}

	private void ShowInApp(INAPP_TYPE type)
	{
		AudioClick.Instance.OnClick();
		for (int i = 0; i < this.objInapp.Length; i++)
		{
			this.objInapp[i].SetActive(i == (int)type);
			this.imgBtnInapp[i].enabled = (i == (int)type);
		}
		if (type == INAPP_TYPE.PACKS)
		{
			for (int j = 0; j < this.packSales.Length; j++)
			{
				this.packSales[j].OnShow(delegate
				{
					this.ShowCoin();
					this.vipBar.CheckVipLevel();
				});
			}
		}
	}

	public void Buy(InappPackage package)
	{
		AudioClick.Instance.OnClick();
		string sku = InAppManager.Instance.GetSku(package.SKU);
		InAppManager.Instance.PurchaseProduct(sku, delegate(InAppManager.InforCallback callback)
		{
			if (callback.isSuccess)
			{
				string parameterValue = "GamePlay";
				try
				{
					parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
				}
				catch
				{
				}
				
			}
			this.ShowCoin();
		});
	}

	public void BuyCoin(InappPackage package)
	{
		AudioClick.Instance.OnClick();
		//int sku = (int)package.SKU;
		// if (ProfileManager.userProfile.Ms >= this.SecuredGem[sku].Value)
		// {
		// 	InforReward[] array = new InforReward[]
		// 	{
		// 		new InforReward()
		// 	};
		// 	array[0].amount = this.SecuredGold[sku].Value;
		// 	array[0].item = Item.Gold;
		// 	PopupManager.Instance.ShowCongratulation(array, false, null);
		// 	PopupManager.Instance.SaveReward(Item.Gem, -this.SecuredGem[sku].Value, "IAP_BuyGold", null);
		// 	PopupManager.Instance.SaveReward(Item.Gold, this.SecuredGold[sku].Value, "IAP_BuyGold", null);
		// 	string parameterValue = "GamePlay";
		// 	try
		// 	{
		// 		parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		// 	}
		// 	catch
		// 	{
		// 	}
		// 	
		// 	DailyQuestManager.Instance.Transfer(null);
		// 	this.ShowCoin();
		// }
		// else
		// {
		// 	this.ShowInApp(INAPP_TYPE.MS);
		// }
	}

	public override void OnClose()
	{
		base.OnClose();
		this.isClosed(true);
		if (MenuManager.Instance != null)
		{
			if (MenuManager.Instance.formCurrent.idForm == FormUI.UIWeapon)
			{
				FormWeapon2 component = MenuManager.Instance.formCurrent.GetComponent<FormWeapon2>();
				if (component != null)
				{
					component.ChangeTab((int)FormWeapon2.indexTab);
					if (component.obj_PopUpRankUp.activeSelf)
					{
						component.BtnPopupRankUp();
					}
					if (component.obj_PopUpUpgrade.activeSelf)
					{
						component.BtnPopupUpgrade();
					}
					component.ReloadDataAll();
				}
			}
			else
			{
				FormUpgradeCharacter1 component2 = MenuManager.Instance.formCurrent.GetComponent<FormUpgradeCharacter1>();
				if (component2 != null)
				{
					component2.ClickToCharacter(new CardChar
					{
						idCard = component2.indexCharSelected
					});
					if (component2.obj_PopUpRankUp.activeSelf)
					{
						component2.BtnPopupRankUp();
					}
					if (component2.obj_PopUpUpgrade.activeSelf)
					{
						component2.BtnPopupUpgrade();
					}
				}
			}
		}
	}

	private void ReSortInappPacks()
	{
		if (SaleManager.Instance.ValueSale <= 0f || SaleManager.Instance.IDSale == -1)
		{
			return;
		}
		this.ListPacks[SaleManager.Instance.IDSale].SetSiblingIndex(0);
	}

	[SerializeField]
	public VipBar vipBar;

	[SerializeField]
	private Text txtCoin;

	[SerializeField]
	private Text txtMS;

	[SerializeField]
	private GameObject[] objInapp;

	[SerializeField]
	private Image[] imgBtnInapp;

	[SerializeField]
	private RectTransform rectTransform;

	private Action<bool> isClosed;

	private SecuredInt[] SecuredGold;

	private SecuredInt[] SecuredGem;

	[SerializeField]
	private List<RectTransform> ListPacks;

	private List<float> XDefault;

	[SerializeField]
	private PackInforDailySale[] packSales;

	public RectTransform rect_InforVip;

	public Text txtNotf;
}
