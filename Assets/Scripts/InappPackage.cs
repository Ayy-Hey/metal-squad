using System;
using UnityEngine;
using UnityEngine.UI;

public class InappPackage : MonoBehaviour
{
	private void OnEnable()
	{
		this.OnShow();
	}

	private void OnShow()
	{
		this.objTextBonus.SetActive(this.percentBonusCurrent > 0f);
		this.txtBonus.text = "+" + this.percentBonusCurrent + "%";
		// string price = InAppManager.Instance.GetPrice(this.SKU);
		// if (!string.IsNullOrEmpty(price) && !ProfileManager.unlockAll)
		// {
		// 	this.txtPrice.text = price;
		// }
		// else
		// {
		// 	this.txtPrice.text = InAppManager.Instance.usdList[(int)this.SKU];
		// }
		// if (SaleManager.Instance.ValueSale == 0f)
		// {
		// 	this.txtGem.text = InAppManager.Instance.Ms[(int)this.SKU].ToString();
		// 	this.objSuperOffer.SetActive(false);
		// 	this.txtBonus.text = "+" + this.percentBonusCurrent + "%";
		// }
		// else if (SaleManager.Instance.IDSale == -1 || SaleManager.Instance.IDSale == (int)this.SKU)
		// {
		// 	this.objSuperOffer.SetActive(true);
		// 	//int num = InAppManager.Instance.Ms[(int)this.SKU] + (int)(SaleManager.Instance.ValueSale * (float)InAppManager.Instance.Ms[(int)this.SKU]);
		// 	//this.txtGem.text = num.ToString();
		// 	float num2 = this.percentBonusCurrent + SaleManager.Instance.ValueSale * 100f;
		// 	this.txtBonus.text = "+" + num2 + "%";
		// }
		// else
		// {
		// 	//this.txtGem.text = InAppManager.Instance.Ms[(int)this.SKU].ToString();
		// 	this.objSuperOffer.SetActive(false);
		// 	this.txtBonus.text = "+" + this.percentBonusCurrent + "%";
		// }
	}

	public Text txtPrice;

	//public InAppManager.SKU SKU;

	public Text txtGem;

	public GameObject objSuperOffer;

	public GameObject objTextBonus;

	public Text txtBonus;

	public float percentBonusCurrent;
}
