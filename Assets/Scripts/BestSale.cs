using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BestSale : MonoBehaviour
{
	public void OnShow(Action OnPurchaseSuccess)
	{
		base.gameObject.SetActive(true);
		this.OnPurchaseSuccess = OnPurchaseSuccess;
		string text = InAppManager.Instance.GetPrice(InAppManager.Instance.GetSku(this.sku_sale));
		string text2 = InAppManager.Instance.GetPrice(InAppManager.Instance.GetSku(this.sku_not_sale));
		if (string.IsNullOrEmpty(text) || ProfileManager.unlockAll)
		{
			text = InAppManager.Instance.usdList[(int)this.sku_sale];
			text2 = InAppManager.Instance.usdList[(int)this.sku_not_sale];
		}
		this.txtUSDSale.text = text;
		this.txtUSDNotSale.text = text2;
	}

	public void OnClose()
	{
		base.gameObject.SetActive(false);
	}

	public void OnPurchase()
	{
		InAppManager.Instance.PurchaseProduct(InAppManager.Instance.ListSkuCurrent[(int)this.sku_sale], delegate(InAppManager.InforCallback callback)
		{
			if (callback.isSuccess && this.OnPurchaseSuccess != null)
			{
				this.OnPurchaseSuccess();
			}
		});
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	private void LateUpdate()
	{
		if (this.builderTimeStarterPack != null)
		{
			this.builderTimeStarterPack.Length = 0;
			this.builderTimeStarterPack.Append("(");
			int value = 24 - DateTime.Now.Hour;
			this.builderTimeStarterPack.Append(value);
			this.builderTimeStarterPack.Append("h ");
			int value2 = 60 - DateTime.Now.Minute;
			this.builderTimeStarterPack.Append(value2);
			this.builderTimeStarterPack.Append("m ");
			int value3 = 60 - DateTime.Now.Second;
			this.builderTimeStarterPack.Append(value3);
			this.builderTimeStarterPack.Append("s");
			this.builderTimeStarterPack.Append(")");
			this.txtTimeCountdown.text = this.builderTimeStarterPack.ToString();
		}
	}

	public InAppManager.SKU sku_not_sale;
	
	public InAppManager.SKU sku_sale;

	public Text txtTimeCountdown;

	public Text txtUSDNotSale;

	public Text txtUSDSale;

	private StringBuilder builderTimeStarterPack = new StringBuilder();

	private Action OnPurchaseSuccess;
}
