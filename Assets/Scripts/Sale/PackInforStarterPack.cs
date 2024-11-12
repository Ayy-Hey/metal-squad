using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
	public class PackInforStarterPack : MonoBehaviour
	{
		public void OnShow(Action OnPurchaseSuccess)
		{
			if (this.isPopupFull)
			{
				this.imgDamua.enabled = SaleManager.Instance.profileStarterPacks[this.IDPack].isBuy;
			}
			this.OnPurchaseSuccess = OnPurchaseSuccess;
			base.gameObject.SetActive(true);
			string text = string.Empty;
			string text2 = string.Empty;
			text = InAppManager.Instance.ListSkuCurrent[(int)this.sku_not_sale];
			text2 = InAppManager.Instance.ListSkuCurrent[(int)this.sku_sale];
			this.sku_Purchase_Current = text2;
			this.WaitingPrice(text, text2);
		}

		public void OnPurchase()
		{
			InAppManager.Instance.PurchaseProduct(this.sku_Purchase_Current, delegate(InAppManager.InforCallback callback)
			{
				if (callback.isSuccess)
				{
					SaleManager.Instance.profileStarterPacks[this.sku_sale - InAppManager.SKU.STARTERPACK_PACK_0_SALE].isShowed = true;
					SaleManager.Instance.profileStarterPacks[this.IDPack].isBuy = true;
					if (this.OnPurchaseSuccess != null)
					{
						this.OnPurchaseSuccess();
					}
					if (this.imgDamua != null)
					{
						this.imgDamua.enabled = true;
					}
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

		private void WaitingPrice(string sku_not_sale, string sku_sale)
		{
			this.txtPrice_sale.text = ((!sku_sale.Equals(string.Empty)) ? "Loading" : string.Empty);
			string text = InAppManager.Instance.GetPrice(sku_not_sale);
			if (string.IsNullOrEmpty(text) || ProfileManager.unlockAll)
			{
				text = InAppManager.Instance.usdList[(int)this.sku_not_sale];
			}
			this.txtPrice_not_sale.text = text;
			if (!sku_sale.Equals(string.Empty))
			{
				text = InAppManager.Instance.GetPrice(sku_sale);
				if (string.IsNullOrEmpty(text) || ProfileManager.unlockAll)
				{
					text = InAppManager.Instance.usdList[(int)this.sku_sale];
				}
				this.txtPrice_sale.text = text;
			}
		}

		private void OnDisable()
		{
			base.StopAllCoroutines();
		}

		public InAppManager.SKU sku_not_sale;
		
		public InAppManager.SKU sku_sale;

		public Text txtPrice_not_sale;

		public Text txtPrice_sale;

		private string sku_Purchase_Current;

		private Action OnPurchaseSuccess;

		public bool isPopupFull;

		public Image imgDamua;

		public int IDPack;
	}
}
