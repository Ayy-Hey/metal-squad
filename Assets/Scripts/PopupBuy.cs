using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupBuy : PopupBase
{
	public void Show(PopupBuy.ItemType type, int index, Item itemMain, string title, Sprite imageItem, int pricePerOne, int quantityCurrent, int quantityMax, bool isDiamond = false, Action OnEventBuy = null)
	{
		base.Show();
		this.itemType = type;
		this.indexItem = index;
		this.item = itemMain;
		this.costPerOne = pricePerOne;
		this.amountCurrent = quantityCurrent;
		this.amountMax = quantityMax;
		this.txt_Title.text = title;
		if (!this.item.ToString().Contains("Gacha"))
		{
			this.img_Item.sprite = imageItem;
			this.img_Item.gameObject.SetActive(true);
			this.aniGacha[0].SetActive(false);
			this.aniGacha[1].SetActive(false);
			this.aniGacha[2].SetActive(false);
		}
		else if (this.item == Item.Common_Crate)
		{
			this.img_Item.gameObject.SetActive(false);
			this.aniGacha[0].SetActive(true);
			this.aniGacha[1].SetActive(false);
			this.aniGacha[2].SetActive(false);
		}
		else if (this.item == Item.Epic_Crate)
		{
			this.img_Item.gameObject.SetActive(false);
			this.aniGacha[0].SetActive(false);
			this.aniGacha[1].SetActive(true);
			this.aniGacha[2].SetActive(false);
		}
		else if (this.item == Item.Gacha_T3)
		{
			this.img_Item.gameObject.SetActive(false);
			this.aniGacha[0].SetActive(false);
			this.aniGacha[1].SetActive(false);
			this.aniGacha[2].SetActive(true);
		}
		this.isGem = isDiamond;
		this.OnBuy = OnEventBuy;
		if (this.isGem)
		{
			this.img_Money.sprite = this.sprite_Gem;
		}
		else
		{
			this.img_Money.sprite = this.sprite_Gold;
		}
		PopupBuy.ItemType itemType = this.itemType;
		if (itemType != PopupBuy.ItemType.Bullet)
		{
			this.obj_Bullet.SetActive(false);
			this.txt_AmountCurrent.text = "x" + this.amountCurrent;
		}
		else
		{
			this.obj_Bullet.SetActive(true);
			this.txt_AmountCurrent.text = "x" + this.amountCurrent + "  ";
		}
		this.amountBuy = 1;
		int num;
		if (this.isGem)
		{
			num = ProfileManager.userProfile.Ms;
		}
		else
		{
			num = ProfileManager.userProfile.Coin;
		}
		int num2 = num / this.costPerOne;
		if (this.amountMax > 0)
		{
			int num3 = this.amountMax - this.amountCurrent;
			this.amountBuy = ((num3 >= num2) ? num2 : num3);
		}
		this.input_AmountBuy.text = this.amountBuy + string.Empty;
		this.txt_Cost.text = this.amountBuy * this.costPerOne + string.Empty;
		if (this.amountBuy * this.costPerOne <= num)
		{
			this.txt_Cost.color = Color.green;
		}
		else
		{
			this.txt_Cost.color = Color.red;
		}
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormLoadout_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(2);
		}
		string parameterValue = "Menu";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	public void IncreaseAmount()
	{
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormLoadout_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(3);
			this.amountBuy++;
			this.input_AmountBuy.text = this.amountBuy + string.Empty;
			this.txt_Cost.text = this.amountBuy * this.costPerOne + string.Empty;
			return;
		}
		this.CheckValueBuy(this.amountBuy + 1);
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	public void DecreaseAmount()
	{
		this.CheckValueBuy(this.amountBuy - 1);
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	public void ChangeValue()
	{
		this.CheckValueBuy(int.Parse(this.input_AmountBuy.text));
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	private void CheckValueBuy(int amountNew)
	{
		this.amountBuy = amountNew;
		int num;
		if (this.isGem)
		{
			num = ProfileManager.userProfile.Ms;
		}
		else
		{
			num = ProfileManager.userProfile.Coin;
		}
		int max = 999;
		if (this.amountMax > 0)
		{
			max = this.amountMax - this.amountCurrent;
		}
		this.amountBuy = Mathf.Clamp(this.amountBuy, 1, max);
		this.input_AmountBuy.text = this.amountBuy + string.Empty;
		this.txt_Cost.text = this.amountBuy * this.costPerOne + string.Empty;
		if (this.amountBuy * this.costPerOne <= num)
		{
			this.txt_Cost.color = Color.green;
		}
		else
		{
			this.txt_Cost.color = Color.red;
		}
	}

	public void BuyItem()
	{
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_FormLoadout_1)
		{
			MenuManager.Instance.tutorial.NextTutorial(4);
			if (MenuManager.Instance.tutorial.listTutorialUI[3] != null)
			{
				PlayerPrefs.SetInt(MenuManager.Instance.tutorial.listTutorialUI[3].keyPlayerPrefs, 1);
			}
			PopupManager.Instance.SaveReward(Item.Booster_Speed, 1, "TutorialLoadout", null);
		}
		else
		{
			if (this.txt_Cost.color == Color.red)
			{
				this.OnClose();
				MenuManager.Instance.popupInformation.ShowWarning((!this.isGem) ? Item.Gold : Item.Gem, this.amountBuy * this.costPerOne, PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText((!this.isGem) ? Localization0.Gold : Localization0.Gem, null));
				return;
			}
			if (this.isGem)
			{
				if (ProfileManager.userProfile.Ms < this.amountBuy * this.costPerOne)
				{
					this.OnClose();
					MenuManager.Instance.popupInformation.ShowWarning(Item.Gem, this.amountBuy * this.costPerOne, PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText(Localization0.Gem, null));
					return;
				}
				PopupManager.Instance.SaveReward(Item.Gem, -(this.amountBuy * this.costPerOne), base.name + "_Title:" + this.txt_Title.text, null);
			}
			else
			{
				if (ProfileManager.userProfile.Coin < this.amountBuy * this.costPerOne)
				{
					this.OnClose();
					MenuManager.Instance.popupInformation.ShowWarning(Item.Gold, this.amountBuy * this.costPerOne, PopupManager.Instance.GetText(Localization0.Not_enough, null) + " " + PopupManager.Instance.GetText(Localization0.Gold, null));
					return;
				}
				PopupManager.Instance.SaveReward(Item.Gold, -(this.amountBuy * this.costPerOne), base.name + "_Title:" + this.txt_Title.text, null);
			}
		}
		PopupBuy.ItemType itemType = this.itemType;
		if (itemType != PopupBuy.ItemType.Bullet)
		{
			if (itemType != PopupBuy.ItemType.Bomb)
			{
				PopupManager.Instance.SaveReward(this.item, this.amountBuy, MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm) + "_" + base.name, null);
			}
			else
			{
				PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Bomb, this.indexItem), this.amountBuy, MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm) + "_" + base.name, null);
			}
		}
		MenuManager.Instance.topUI.ShowCoin();
		if (this.OnBuy != null)
		{
			this.OnBuy();
		}
		base.OnClose();
	}

	public int indexItem;

	public Item item;

	public PopupBuy.ItemType itemType;

	public int costPerOne;

	public int amountCurrent;

	public int amountMax;

	public bool isGem;

	public int amountBuy;

	public Text txt_Title;

	public Text txt_AmountCurrent;

	public InputField input_AmountBuy;

	public Text txt_Cost;

	public Image img_Money;

	public Image img_Item;

	public Button btn_Buy;

	public GameObject obj_Bullet;

	public Sprite sprite_Gold;

	public Sprite sprite_Gem;

	public GameObject[] aniGacha;

	private Action OnBuy;

	public enum ItemType
	{
		Bullet,
		Bomb,
		Booster,
		Gacha
	}
}
