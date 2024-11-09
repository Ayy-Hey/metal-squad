using System;
using UnityEngine;

public class ShopTips
{
	public static ShopTips Instance
	{
		get
		{
			if (ShopTips.instance == null)
			{
				ShopTips.instance = new ShopTips();
			}
			return ShopTips.instance;
		}
	}

	public bool[] CheckTipAll()
	{
		ETypeWeapon[] array = new ETypeWeapon[]
		{
			ETypeWeapon.PRIMARY,
			ETypeWeapon.SPECIAL,
			ETypeWeapon.KNIFE,
			ETypeWeapon.GRENADE
		};
		bool[] array2 = new bool[array.Length + 1];
		for (int i = 0; i < array.Length; i++)
		{
			if (this.CheckTipTab(array[i])[0])
			{
				array2[0] = true;
				array2[i + 1] = true;
			}
		}
		return array2;
	}

	public bool[] CheckTipTab(ETypeWeapon indexTab)
	{
		int num = 0;
		switch (indexTab)
		{
		case ETypeWeapon.PRIMARY:
			num = ProfileManager.weaponsRifle.Length;
			break;
		case ETypeWeapon.SPECIAL:
			num = ProfileManager.weaponsSpecial.Length;
			break;
		case ETypeWeapon.KNIFE:
			num = ProfileManager.melesProfile.Length;
			break;
		case ETypeWeapon.GRENADE:
			num = ProfileManager.grenadesProfile.Length;
			break;
		default:
			UnityEngine.Debug.Log("Lỗi CheckTipTab() trong ShopTip");
			break;
		}
		bool[] array = new bool[num + 1];
		for (int i = 0; i < num; i++)
		{
			if (this.CheckTipWeapon(indexTab, i)[0])
			{
				array[0] = true;
				array[i + 1] = true;
			}
		}
		return array;
	}

	public bool[] CheckTipWeapon(ETypeWeapon indexTab, int indexWeapon)
	{
		bool[] array = new bool[5];
		bool flag = false;
		Item itemID = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.GetItemID(ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() + 1);
		int num = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.ValueUpgrade(ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() + 1);
		try
		{
			switch (indexTab)
			{
			case ETypeWeapon.PRIMARY:
				flag = false;
				itemID = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.GetItemID(ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() + 1);
				num = ProfileManager.weaponsRifle[indexWeapon].Gun_Value.ValueUpgrade(ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() + 1);
				if (itemID != Item.Gold)
				{
					if (itemID == Item.Gem)
					{
						flag = (ProfileManager.userProfile.Ms >= num);
					}
				}
				else
				{
					flag = (ProfileManager.userProfile.Coin >= num);
				}
				if (!ProfileManager.weaponsRifle[indexWeapon].GetGunBuy())
				{
					if (ProfileManager.weaponsRifle[indexWeapon].Gun_Value.ValueUpgrade(0) > 0 && 80 <= PlayerPrefs.GetInt("metal.squad.frag." + ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CurrencyUnlock.ToString(), 0))
					{
						array[0] = true;
						array[1] = true;
					}
					if (ProfileManager.weaponsRifle[indexWeapon].Gun_Value.SecuredGemPrice.Value > 0 && ProfileManager.weaponsRifle[indexWeapon].Gun_Value.SecuredGemPrice.Value <= ProfileManager.userProfile.Ms)
					{
						array[0] = true;
						array[2] = true;
					}
				}
				else
				{
					if (ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() + 1 < (ProfileManager.weaponsRifle[indexWeapon].GetRankUpped() + 1) * 10 && ProfileManager.weaponsRifle[indexWeapon].Gun_Value.ValueUpgrade(ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() + 1) > 0 && flag)
					{
						array[0] = true;
						array[3] = true;
					}
					if (ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, ProfileManager.weaponsRifle[indexWeapon].GetRankUpped())] > 0 && ProfileManager.weaponsRifle[indexWeapon].GetRankUpped() < 2 && ProfileManager.weaponsRifle[indexWeapon].GetRankUpped() == ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() / 10 && ProfileManager.weaponsRifle[indexWeapon].GetLevelUpgrade() % 10 == 9 && ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, ProfileManager.weaponsRifle[indexWeapon].GetRankUpped())] <= PlayerPrefs.GetInt("metal.squad.frag." + ProfileManager.weaponsRifle[indexWeapon].Gun_Value.CurrencyRankUp.ToString(), 0))
					{
						array[0] = true;
						array[4] = true;
					}
				}
				break;
			case ETypeWeapon.SPECIAL:
				flag = false;
				itemID = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.GetItemID(ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() + 1);
				num = ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() + 1);
				if (itemID != Item.Gold)
				{
					if (itemID == Item.Gem)
					{
						flag = (ProfileManager.userProfile.Ms >= num);
					}
				}
				else
				{
					flag = (ProfileManager.userProfile.Coin >= num);
				}
				if (!ProfileManager.weaponsSpecial[indexWeapon].GetGunBuy())
				{
					if (ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(0) > 0 && ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(0) <= PlayerPrefs.GetInt("metal.squad.frag." + ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CurrencyUnlock.ToString(), 0))
					{
						array[0] = true;
						array[1] = true;
					}
					if (ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.SecuredGemPrice.Value > 0 && ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.SecuredGemPrice.Value <= ProfileManager.userProfile.Ms)
					{
						array[0] = true;
						array[2] = true;
					}
				}
				else
				{
					if (ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() + 1 < (ProfileManager.weaponsSpecial[indexWeapon].GetRankUpped() + 1) * 10 && ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() + 1) > 0 && ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.ValueUpgrade(ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() + 1) <= ProfileManager.userProfile.Coin)
					{
						array[0] = true;
						array[3] = true;
					}
					if (ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, ProfileManager.weaponsSpecial[indexWeapon].GetRankUpped())] > 0 && ProfileManager.weaponsSpecial[indexWeapon].GetRankUpped() < 2 && ProfileManager.weaponsSpecial[indexWeapon].GetRankUpped() == ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() / 10 && ProfileManager.weaponsSpecial[indexWeapon].GetLevelUpgrade() % 10 == 9 && ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CostRankUp[Mathf.Min(1, ProfileManager.weaponsSpecial[indexWeapon].GetRankUpped())] <= PlayerPrefs.GetInt("metal.squad.frag." + ProfileManager.weaponsSpecial[indexWeapon].Gun_Value.CurrencyRankUp.ToString(), 0))
					{
						array[0] = true;
						array[4] = true;
					}
				}
				break;
			case ETypeWeapon.KNIFE:
				flag = false;
				itemID = ProfileManager.melesProfile[indexWeapon].GetItemID(ProfileManager.melesProfile[indexWeapon].LevelUpGradeMelee + 1);
				num = ProfileManager.melesProfile[indexWeapon].ValueUpgrade(ProfileManager.melesProfile[indexWeapon].LevelUpGradeMelee + 1);
				if (itemID != Item.Gold)
				{
					if (itemID == Item.Gem)
					{
						flag = (ProfileManager.userProfile.Ms >= num);
					}
				}
				else
				{
					flag = (ProfileManager.userProfile.Coin >= num);
				}
				if (!ProfileManager.melesProfile[indexWeapon].Unlock)
				{
					if (ProfileManager.melesProfile[indexWeapon].ValueUpgrade(0) > 0 && ProfileManager.melesProfile[indexWeapon].ValueUpgrade(0) <= ProfileManager.userProfile.Ms)
					{
						array[0] = true;
						array[2] = true;
					}
				}
				else if (ProfileManager.melesProfile[indexWeapon].LevelUpGradeMelee < 29 && ProfileManager.melesProfile[indexWeapon].ValueUpgrade(ProfileManager.melesProfile[indexWeapon].LevelUpGradeMelee + 1) > 0 && flag)
				{
					array[0] = true;
					array[3] = true;
				}
				break;
			case ETypeWeapon.GRENADE:
				flag = false;
				itemID = ProfileManager.grenadesProfile[indexWeapon].GetItemID(ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade + 1);
				num = ProfileManager.grenadesProfile[indexWeapon].ValueUpgrade(ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade + 1);
				if (itemID != Item.Gold)
				{
					if (itemID == Item.Gem)
					{
						flag = (ProfileManager.userProfile.Ms >= num);
					}
				}
				else
				{
					flag = (ProfileManager.userProfile.Coin >= num);
				}
				if (ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade < 29 && ProfileManager.grenadesProfile[indexWeapon].ValueUpgrade(ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade + 1) > 0 && ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade % 10 != 9 && flag)
				{
					array[0] = true;
					array[3] = true;
				}
				if (ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade < 29 && ProfileManager.grenadesProfile[indexWeapon].ValueUpgrade(ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade + 1) > 0 && ProfileManager.grenadesProfile[indexWeapon].LevelUpGrade % 10 == 9 && flag)
				{
					array[0] = true;
					array[4] = true;
				}
				break;
			default:
				UnityEngine.Debug.Log("Lỗi CheckTipWeapon() trong ShopTip");
				break;
			}
		}
		catch (Exception ex)
		{
		}
		return array;
	}

	private static ShopTips instance;
}
