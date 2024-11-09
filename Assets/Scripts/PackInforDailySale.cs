using System;
using UnityEngine;
using UnityEngine.UI;

public class PackInforDailySale : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.listTextLocalization.Length; i++)
		{
			if (this.listTextLocalization[i].isUpcaseText)
			{
				this.listTextLocalization[i].txt_Text.text = (PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd).ToUpper();
			}
			else
			{
				this.listTextLocalization[i].txt_Text.text = PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd;
			}
		}
	}

	public void OnShow(Action OnPurchaseSuccess)
	{
		base.gameObject.SetActive(true);
		this.OnPurchaseSuccess = OnPurchaseSuccess;
		string text = string.Empty;
		string text2 = string.Empty;
		// if (!this.isTabInapp)
		// {
		// 	text = InAppManager.Instance.ListSkuCurrent[(int)this.sku_not_sale];
		// 	text2 = InAppManager.Instance.ListSkuCurrent[(int)this.sku_sale];
		// 	this.sku_Purchase_Current = text2;
		// 	if (this.objSale != null)
		// 	{
		// 		this.objSale.SetActive(true);
		// 	}
		// 	this.OnShowPrice(text, text2, true);
		// }
		// else if (this.IDPACK == ProfileManager.dailySaleProfile.ID)
		// {
		// 	text = InAppManager.Instance.ListSkuCurrent[(int)this.sku_not_sale];
		// 	text2 = InAppManager.Instance.ListSkuCurrent[(int)this.sku_sale];
		// 	this.sku_Purchase_Current = text2;
		// 	base.transform.SetSiblingIndex(0);
		// 	if (this.objOffer != null)
		// 	{
		// 		this.objOffer.SetActive(true);
		// 	}
		// 	this.OnShowPrice(text, this.sku_Purchase_Current, true);
		// }
		// else
		// {
		// 	text = InAppManager.Instance.ListSkuCurrent[(int)this.sku_not_sale];
		// 	this.sku_Purchase_Current = text;
		// 	this.OnShowPrice(text, this.sku_Purchase_Current, false);
		// }
	}

	public void OnShowRandom(Action OnPurchaseSuccess)
	{
		base.gameObject.SetActive(true);
		if (this.objSale != null)
		{
			this.objSale.SetActive(false);
		}
		// this.OnPurchaseSuccess = OnPurchaseSuccess;
		// string text = string.Empty;
		// text = InAppManager.Instance.ListSkuCurrent[(int)this.sku_not_sale];
		// this.sku_Purchase_Current = text;
		// string text2 = string.Empty;
		// text2 = InAppManager.Instance.GetPrice(this.sku_not_sale);
		// if (string.IsNullOrEmpty(text2) || ProfileManager.unlockAll)
		// {
		// 	text2 = InAppManager.Instance.usdList[(int)this.sku_not_sale];
		// }
		// this.txtPrice_sale.text = text2;
		// if (this.txtExpVip != null)
		// {
		// 	this.txtExpVip.text = DataLoader.dataPackInApp[this.sku_sale.ToString()]["amount"][DataLoader.dataPackInApp[this.sku_sale.ToString()]["amount"].Count - 1].ToString();
		// }
	}

	public void OnPurchase()
	{
		// InAppManager.Instance.PurchaseProduct(this.sku_Purchase_Current, delegate(InAppManager.InforCallback callback)
		// {
		// 	if (callback.isSuccess)
		// 	{
		// 		if (this.objOffer != null)
		// 		{
		// 			this.objOffer.SetActive(false);
		// 			ProfileManager.dailySaleProfile.ID = -1;
		// 		}
		// 		else if (this.OnPurchaseSuccess != null)
		// 		{
		// 			this.OnPurchaseSuccess();
		// 		}
		// 	}
		// });
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	private void OnShowPrice(string sku_not_sale, string sku_sale, bool isSaleThisPack = true)
	{
		string text = string.Empty;
		if (!this.isTabInapp || isSaleThisPack)
		{
			// text = InAppManager.Instance.GetPrice(sku_not_sale);
			// if (string.IsNullOrEmpty(text) || ProfileManager.unlockAll)
			// {
			// 	text = InAppManager.Instance.usdList[(int)this.sku_not_sale];
			// }
			// this.txtPrice_not_sale.text = text;
			// text = string.Empty;
			// text = InAppManager.Instance.GetPrice(sku_sale);
			// if (string.IsNullOrEmpty(text) || ProfileManager.unlockAll)
			// {
			// 	text = InAppManager.Instance.usdList[(int)this.sku_sale];
			// }
			// this.txtPrice_sale.text = text;
			// if (this.txtExpVip != null)
			// {
			// 	this.txtExpVip.text = DataLoader.dataPackInApp[this.sku_sale.ToString()]["amount"][DataLoader.dataPackInApp[this.sku_sale.ToString()]["amount"].Count - 1].ToString();
			// }
		}
		else
		{
			text = string.Empty;
			// text = InAppManager.Instance.GetPrice(sku_not_sale);
			// if (string.IsNullOrEmpty(text) || ProfileManager.unlockAll)
			// {
			// 	text = InAppManager.Instance.usdList[(int)this.sku_not_sale];
			// }
			// this.txtPrice_sale.text = text;
			// if (this.txtExpVip != null)
			// {
			// 	this.txtExpVip.text = DataLoader.dataPackInApp[this.sku_not_sale.ToString()]["amount"][DataLoader.dataPackInApp[this.sku_not_sale.ToString()]["amount"].Count - 1].ToString();
			// }
		}
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	public TextLocalization[] listTextLocalization;

	public bool isTabInapp;

	// public InAppManager.SKU sku_not_sale;
	//
	// public InAppManager.SKU sku_sale;

	public Text txtPrice_not_sale;

	public Text txtPrice_sale;

	private string sku_Purchase_Current;

	private Action OnPurchaseSuccess;

	public GameObject objOffer;

	public int IDPACK;

	public Text txtExpVip;

	public GameObject objSale;
}
