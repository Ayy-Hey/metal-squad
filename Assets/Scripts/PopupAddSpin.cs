using System;
using CustomData;
using UnityEngine;
using UnityEngine.UI;

public class PopupAddSpin : PopupBase
{
	public override void Show()
	{
		if (!this.dataSpinByGold)
		{
			this.dataSpinByGold = Resources.Load<DataArray>("Spin/SpinByCoin");
		}
		base.Show();
		this.numberSpin = 1;
		this.ShowTextSpin();
		
	}

	public void AddSpin(bool isUP)
	{
		this.numberSpin += ((!isUP) ? ((this.numberSpin <= 1) ? 0 : -1) : 1);
		this.ShowTextSpin();
		
	}

	public void OnBuy()
	{
		int num = this.numberSpin * this.dataSpinByGold.intDatas[0];
		if (ProfileManager.userProfile.Coin >= num)
		{
			AudioClick.Instance.OnClick();
			PopupManager.Instance.SaveReward(Item.Gold, -num, "BuySpin", null);
			PopupManager.Instance.SaveReward(Item.Ticket_Spin, this.numberSpin, "BuySpin", null);
			this.OnClose();
		}
		else
		{
			this.OnClose();
			MenuManager.Instance.popupInformation.ShowWarning(Item.Gold, num, string.Empty);
		}
		
	}

	private void ShowTextSpin()
	{
		float num = 1f;
		if (ProfileManager.inAppProfile.vipProfile.level >= E_Vip.Vip1)
		{
			num = Mathf.Clamp(1f - DataLoader.vipData.Levels[(int)ProfileManager.inAppProfile.vipProfile.level].dailyReward.ReducedPriceTicketSpin, 0f, 1f);
		}
		this.txtGoldBuy.text = ((float)(this.numberSpin * this.dataSpinByGold.intDatas[0]) * num).ToString();
		if ((float)ProfileManager.userProfile.Coin >= (float)(this.numberSpin * this.dataSpinByGold.intDatas[0]) * num)
		{
			this.txtGoldBuy.color = Color.white;
		}
		else
		{
			this.txtGoldBuy.color = Color.red;
		}
		this.txtTotalSpin.text = this.numberSpin.ToString();
	}

	public DataArray dataSpinByGold;

	public Text txtGoldBuy;

	public Text txtTotalSpin;

	public Animation animTxtTotal;

	private int numberSpin;
}
