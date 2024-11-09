using System;
using UnityEngine;

public class BestSaleCalculator : PopupBase
{
	public void OnShow()
	{
		int dayOfYear = DateTime.Now.DayOfYear;
		this.Show();
		if (PlayerPrefs.GetInt(this.KeySaveToday, -1) != dayOfYear)
		{
			PlayerPrefs.SetInt(this.KeySaveToday, dayOfYear);
			PlayerPrefs.SetInt(this.KeyIDShow, PlayerPrefs.GetInt(this.KeyIDShow, -1) + 1);
		}
		int @int = PlayerPrefs.GetInt(this.KeyIDShow, -1);
		if (@int > 2)
		{
			PlayerPrefs.SetInt(this.KeyIDShow, 0);
		}
		switch (3)
		{
		case 0:
			if (!this.SaleWeapon())
			{
				this.SaleGem();
			}
			break;
		case 1:
			if (!this.SaleWeapon())
			{
				this.SaleGem();
			}
			break;
		case 2:
			this.SaleGem();
			break;
		default:
			this.SaleGem();
			break;
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		try
		{
			MenuManager.Instance.topUI.Show();
			MenuManager.Instance.topUI.ShowCoin();
			for (int i = 0; i < this.ListWeaponPopup.Length; i++)
			{
				if (this.ListWeaponPopup[i] != null)
				{
					this.ListWeaponPopup[i].OnClose();
				}
			}
		}
		catch (Exception ex)
		{
		}
		PlayerPrefs.SetInt(this.KeyTestIOSReview, PlayerPrefs.GetInt(this.KeyTestIOSReview, 0) + 1);
		if (PlayerPrefs.GetInt(this.KeyTestIOSReview, 0) > 4)
		{
			PlayerPrefs.SetInt(this.KeyTestIOSReview, 0);
		}
	}

	public bool SaleWeapon()
	{
		int num = -1;
		for (int i = ProfileManager.weaponsRifle.Length - 1; i >= 0; i--)
		{
			WeaponProfile weaponProfile = ProfileManager.weaponsRifle[i];
			if (weaponProfile.GetGunBuy())
			{
				int rankUpped = weaponProfile.GetRankUpped();
				if (rankUpped < 2)
				{
					num = i;
				}
			}
		}
		if (num < 0)
		{
			for (int j = 0; j < ProfileManager.weaponsRifle.Length; j++)
			{
				WeaponProfile weaponProfile2 = ProfileManager.weaponsRifle[j];
				if (!weaponProfile2.GetGunBuy())
				{
					num = j;
				}
			}
		}
		if (num >= 0)
		{
			this.ListWeaponPopup[num].OnShow(delegate
			{
				this.OnClose();
			});
			return true;
		}
		return false;
	}

	public bool SaleCharacter()
	{
		if (!ProfileManager.InforChars[1].IsUnLocked)
		{
			this.ListCharPopup[0].OnShow(delegate
			{
				SaleManager.Instance.TurnOffSaleServer();
				this.OnClose();
			});
			return true;
		}
		if (!ProfileManager.InforChars[2].IsUnLocked)
		{
			this.ListCharPopup[1].OnShow(delegate
			{
				SaleManager.Instance.TurnOffSaleServer();
				this.OnClose();
			});
			return true;
		}
		return false;
	}

	public void SaleGem()
	{
		this.saleGem.OnShow(delegate
		{
			this.OnClose();
		});
	}

	public BestSale[] ListWeaponPopup;

	public BestSale[] ListCharPopup;

	public BestSale saleGem;

	private string KeySaveToday = "com.sora.metal.squad.BestSaleCalculator.SaveToday";

	private string KeyIDShow = "com.sora.metal.squad.BestSaleCalculator.IDShow";

	private string KeyTestIOSReview = "com.metal.squad.test.iap";

	public enum ETypeSale
	{
		Gem,
		Weapon,
		Character
	}
}
