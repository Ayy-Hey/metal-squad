using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGift : PopupBase
{
	public void Awake()
	{
		this.OnInit();
	}

	public override void Show()
	{
		base.Show();
		this.OpenTab(this.indexTabSelected);
		this.OpenTab(1);
		this.ShowRewardCurrent();
	}

	public void OpenTab(int indexTab)
	{
	}

	public void LoadOnlineGift()
	{
		this.indexOnlineGiftCompleted = 0;
		int num = TimePlay.timeRunGame / 60;
		for (int i = 0; i < ProfileManager.REWARD_MAX_ONLINE_GIFT; i++)
		{
			this.listOnlineGift[i].OnInit();
			this.listReward[i].obj_Active.SetActive(false);
			this.listReward[i].txt_Amount.text = PopupGift.TIME_ONLINEGIFT[i] + " min";
			if (num >= PopupGift.TIME_ONLINEGIFT[i])
			{
				this.indexOnlineGiftCompleted = i;
			}
		}
		this.indexOnlineGiftSelect = this.indexOnlineGiftCompleted;
		base.StartCoroutine(this.ReloadPopup());
		this.ClickToReward(this.listReward[this.indexOnlineGiftSelect]);
	}

	private IEnumerator ReloadPopup()
	{
		int minute = TimePlay.timeRunGame / 60;
		int second = TimePlay.timeRunGame % 60;
		for (int i = 0; i < ProfileManager.REWARD_MAX_ONLINE_GIFT; i++)
		{
			if (minute >= PopupGift.TIME_ONLINEGIFT[i])
			{
				this.indexOnlineGiftCompleted = i;
				this.listReward[i].img_Main.sprite = this.sprite_GiftClaim;
				this.listReward[i].obj_Open.SetActive(true);
				this.listReward[i].img_Core.fillAmount = 1f;
			}
			else
			{
				this.listReward[i].img_Main.sprite = this.sprite_GiftNotDone;
				this.listReward[i].obj_Open.SetActive(false);
				int num = (i != 0) ? PopupGift.TIME_ONLINEGIFT[i - 1] : 0;
				float num2 = ((float)minute + (float)second / 60f - (float)num <= 0f) ? 0f : ((float)minute + (float)second / 60f - (float)num);
				this.listReward[i].img_Core.fillAmount = num2 / (float)(PopupGift.TIME_ONLINEGIFT[i] - num);
			}
			if (PlayerPrefs.GetInt("gui.online.gift.reward." + i) == 1)
			{
				this.listReward[i].img_Main.sprite = this.sprite_GiftDone;
				this.listReward[i].obj_Open.SetActive(false);
			}
		}
		if (minute >= 45)
		{
			this.txt_TimeCurrent.text = "45m";
		}
		else if (minute > 0)
		{
			this.txt_TimeCurrent.text = string.Concat(new object[]
			{
				minute,
				"m ",
				second,
				"s"
			});
		}
		else
		{
			this.txt_TimeCurrent.text = second + "s";
		}
		this.CheckButtonClaim(this.listReward[this.indexOnlineGiftSelect]);
		yield return new WaitForSeconds(1f);
		base.StartCoroutine(this.ReloadPopup());
		yield break;
	}

	public void ClaimReward()
	{
		
		this.btn_Claim.gameObject.SetActive(false);
		this.obj_Claimed.SetActive(true);
		InforReward[] array = new InforReward[]
		{
			new InforReward()
		};
		array[0].amount = this.listOnlineGift[this.indexOnlineGiftSelect].AmountItem;
		array[0].item = this.listOnlineGift[this.indexOnlineGiftSelect].itemType;
		PopupManager.Instance.ShowCongratulation(array, true, null);
		PopupManager.Instance.SaveReward(array[0].item, array[0].amount, "OnlineGift_Index:" + this.indexOnlineGiftSelect, null);
		PlayerPrefs.SetInt("gui.online.gift.reward." + this.indexOnlineGiftSelect, 1);
	}

	public void ClickToReward(CardBase card)
	{
		
		this.listReward[this.indexOnlineGiftSelect].obj_Active.SetActive(false);
		this.indexOnlineGiftSelect = card.idCard;
		card.obj_Active.SetActive(true);
		this.txt_Infor.text = "online in " + PopupGift.TIME_ONLINEGIFT[this.indexOnlineGiftSelect] + " min";
		this.img_Item.sprite = PopupManager.Instance.sprite_Item[(int)this.listOnlineGift[this.indexOnlineGiftSelect].itemType];
		this.txt_NameItem.text = string.Concat(new object[]
		{
			"+",
			this.listOnlineGift[this.indexOnlineGiftSelect].AmountItem,
			" ",
			this.listOnlineGift[this.indexOnlineGiftSelect].itemType.ToString().Replace("_", " ")
		});
		this.CheckButtonClaim(card);
	}

	public void ClickToNextReward()
	{
		
		if (this.indexOnlineGiftSelect + 1 < ProfileManager.REWARD_MAX_ONLINE_GIFT)
		{
			this.ClickToReward(this.listReward[this.indexOnlineGiftSelect + 1]);
		}
	}

	public void ClickToBackReward()
	{
		
		if (this.indexOnlineGiftSelect - 1 >= 0)
		{
			this.ClickToReward(this.listReward[this.indexOnlineGiftSelect - 1]);
		}
	}

	private void CheckButtonClaim(CardBase card)
	{
		if (card.img_Main.sprite == this.sprite_GiftClaim)
		{
			this.btn_Claim.gameObject.SetActive(true);
			this.btn_Claim.interactable = true;
			this.obj_EffectBtnClaim.SetActive(this.btn_Claim.interactable);
			this.obj_Claimed.SetActive(false);
		}
		else if (card.img_Main.sprite == this.sprite_GiftDone)
		{
			this.btn_Claim.gameObject.SetActive(false);
			this.obj_Claimed.SetActive(true);
		}
		else
		{
			this.btn_Claim.gameObject.SetActive(true);
			this.btn_Claim.interactable = false;
			this.obj_EffectBtnClaim.SetActive(this.btn_Claim.interactable);
			this.obj_Claimed.SetActive(false);
		}
	}

	public void OnInit()
	{
		this.ListRewardVideo.Add(this.reward1);
		this.ListRewardVideo.Add(this.reward2);
		this.ListRewardVideo.Add(this.reward3);
		this.ListRewardVideo.Add(this.reward4);
		this.ListRewardVideo.Add(this.reward5);
		for (int i = 0; i < this.ListGiftVideoReward.Count; i++)
		{
			this.ListGiftVideoReward[i].Oninit(i);
		}
		this.DayCurrrent = new IntProfileData("com.sora.metal.squad.VideoRewardDaily.Day6", -1);
		this.RewardCurrent = new IntProfileData("com.sora.metal.squad.VideoRewardDaily.RewardCurrent6", 0);
		if (this.DayCurrrent != DateTime.Now.DayOfYear)
		{
			this.DayCurrrent.setValue(DateTime.Now.DayOfYear);
			this.RewardCurrent.setValue(0);
			for (int j = 0; j < this.ListGiftVideoReward.Count; j++)
			{
				this.ListGiftVideoReward[j].OnReset();
			}
			this.ListGiftVideoReward[0].SaveState(GiftVideoReward.State.READY_TO_COUNTDOWN);
		}
	}

	public void ShowRewardCurrent()
	{
		if (this.RewardCurrent.Data.Value >= 5)
		{
			for (int i = 0; i < this.ListGiftVideoReward.Count; i++)
			{
				this.ListGiftVideoReward[i].ReCheckButton();
			}
			return;
		}
		if (!PopupGift.isFirtSeasion)
		{
			PopupGift.isFirtSeasion = true;
		}
		this.ListGiftVideoReward[this.RewardCurrent.Data.Value].OnShow(this.ListRewardVideo[this.RewardCurrent.Data.Value], this);
		MenuManager.Instance.topUI.ReloadCoin();
		for (int j = 0; j < this.ListGiftVideoReward.Count; j++)
		{
			this.ListGiftVideoReward[j].ReCheckButton();
		}
	}

	private void Update()
	{
		if (this.RewardCurrent.Data.Value < 5)
		{
			this.ListGiftVideoReward[this.RewardCurrent.Data.Value].OnUpdate();
		}
	}

	public int indexTabSelected;

	private int indexOnlineGiftSelect;

	private int indexOnlineGiftCompleted;

	public CardBase[] listReward;

	public OnlineGift[] listOnlineGift;

	public Text txt_TimeCurrent;

	public Text txt_Infor;

	public Image img_Item;

	public Text txt_NameItem;

	public Button btn_Claim;

	public GameObject obj_EffectBtnClaim;

	public GameObject obj_Claimed;

	public Sprite sprite_GiftNotDone;

	public Sprite sprite_GiftClaim;

	public Sprite sprite_GiftDone;

	public static int[] TIME_ONLINEGIFT = new int[]
	{
		2,
		10,
		20,
		30,
		45
	};

	public List<GiftVideoReward> ListGiftVideoReward;

	private IntProfileData DayCurrrent;

	public IntProfileData RewardCurrent;

	private int[] reward1 = new int[]
	{
		5
	};

	private int[] reward2 = new int[]
	{
		10,
		3
	};

	private int[] reward3 = new int[]
	{
		15,
		3,
		3
	};

	private int[] reward4 = new int[]
	{
		20,
		3,
		3,
		1
	};

	private int[] reward5 = new int[]
	{
		30,
		5,
		5,
		2
	};

	private List<int[]> ListRewardVideo = new List<int[]>(5);

	private static bool isFirtSeasion;
}
