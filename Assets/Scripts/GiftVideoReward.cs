using System;
using System.Text;
using com.dev.util.SecurityHelper;
using UnityEngine;
using UnityEngine.UI;

public class GiftVideoReward : MonoBehaviour
{
	public void Oninit(int id)
	{
		this.txt_ReadyToClaim.text = PopupManager.Instance.GetText(Localization0.Get_reward, null).ToUpper();
		this.txt_NotReady.text = PopupManager.Instance.GetText(Localization0.Get_reward, null).ToUpper();
		this.saveState = new IntProfileData("com.sora.metal.squad.GiftVideoReward6" + id, 0);
		this.builderTimeVideo = new StringBuilder();
		this.isinit = true;
	}

	public void OnReset()
	{
		this.saveState.setValue(0);
		this.objButtonState[1].SetActive(false);
		this.objButtonState[2].SetActive(false);
		this.objButtonState[0].SetActive(true);
		this.objButtonState[3].SetActive(false);
	}

	public void SaveState(GiftVideoReward.State state)
	{
		this.saveState.setValue((int)state);
	}

	public void OnShow(int[] value, PopupGift videoReward)
	{
		this.isGetReward = false;
		this.videoReward = videoReward;
		if (this.reward_secured == null)
		{
			this.reward_secured = new SecuredInt[value.Length];
			for (int i = 0; i < this.reward_secured.Length; i++)
			{
				this.reward_secured[i] = new SecuredInt(value[i]);
			}
		}
		this.state = (GiftVideoReward.State)this.saveState.Data.Value;
		switch (this.state)
		{
		case GiftVideoReward.State.NOT_READY:
			this.objButtonState[1].SetActive(false);
			this.objButtonState[2].SetActive(false);
			this.objButtonState[0].SetActive(true);
			this.objButtonState[3].SetActive(false);
			break;
		case GiftVideoReward.State.READY_TO_COUNTDOWN:
			this.objButtonState[1].SetActive(true);
			this.objButtonState[2].SetActive(false);
			this.objButtonState[0].SetActive(false);
			this.objButtonState[3].SetActive(false);
			break;
		case GiftVideoReward.State.READY_TO_CLAIM:
			this.objButtonState[1].SetActive(false);
			this.objButtonState[2].SetActive(true);
			this.objButtonState[0].SetActive(false);
			this.objButtonState[3].SetActive(false);
			break;
		case GiftVideoReward.State.COMPLETED:
			this.objButtonState[1].SetActive(false);
			this.objButtonState[2].SetActive(false);
			this.objButtonState[0].SetActive(false);
			this.objButtonState[3].SetActive(true);
			break;
		}
	}

	public void ReCheckButton()
	{
		switch (this.saveState.Data.Value)
		{
		case 0:
			this.objButtonState[1].SetActive(false);
			this.objButtonState[2].SetActive(false);
			this.objButtonState[0].SetActive(true);
			this.objButtonState[3].SetActive(false);
			break;
		case 1:
			this.objButtonState[1].SetActive(true);
			this.objButtonState[2].SetActive(false);
			this.objButtonState[0].SetActive(false);
			this.objButtonState[3].SetActive(false);
			break;
		case 2:
			this.objButtonState[1].SetActive(false);
			this.objButtonState[2].SetActive(true);
			this.objButtonState[0].SetActive(false);
			this.objButtonState[3].SetActive(false);
			break;
		case 3:
			this.objButtonState[1].SetActive(false);
			this.objButtonState[2].SetActive(false);
			this.objButtonState[0].SetActive(false);
			this.objButtonState[3].SetActive(true);
			break;
		}
	}

	public void OnUpdate()
	{
		if (this.state == GiftVideoReward.State.READY_TO_COUNTDOWN && this.isinit)
		{
			if (TimePlay.timeVideoReward < this.TimeGetReward)
			{
				this.builderTimeVideo.Length = 0;
				int num = (this.TimeGetReward - TimePlay.timeVideoReward) / 60;
				this.builderTimeVideo.Append("0");
				this.builderTimeVideo.Append(num);
				this.builderTimeVideo.Append(":");
				int num2 = this.TimeGetReward - TimePlay.timeVideoReward - 60 * num;
				if (num2 < 10)
				{
					this.builderTimeVideo.Append("0");
				}
				this.builderTimeVideo.Append(num2);
				this.txtTime.text = this.builderTimeVideo.ToString();
			}
			else
			{
				this.objButtonState[1].SetActive(false);
				this.objButtonState[2].SetActive(true);
				this.objButtonState[0].SetActive(false);
				this.state = GiftVideoReward.State.READY_TO_CLAIM;
				this.SaveState(this.state);
				this.txtTime.text = string.Empty;
			}
		}
	}

	public void OnClaim()
	{
		if (this.state != GiftVideoReward.State.READY_TO_CLAIM)
		{
			return;
		}
		AudioClick.Instance.OnClick();
		if (this.isPressButton)
		{
			return;
		}
		this.isPressButton = true;
		AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isSuccess)
		{
			if (isSuccess)
			{
				if (!this.isGetReward)
				{
					this.isGetReward = true;
					this.GetReward();
				}
				this.isPressButton = false;
			}
			else
			{
				this.isGetReward = false;
				this.isPressButton = false;
				this.state = GiftVideoReward.State.READY_TO_CLAIM;
			}
		});
		
	}

	private void GetReward()
	{
		this.SaveState(GiftVideoReward.State.COMPLETED);
		this.videoReward.RewardCurrent.setValue(this.videoReward.RewardCurrent.Data.Value + 1);
		if (this.videoReward.RewardCurrent.Data.Value < 5)
		{
			TimePlay.timeVideoReward = 0;
			this.videoReward.ListGiftVideoReward[this.videoReward.RewardCurrent.Data.Value].SaveState(GiftVideoReward.State.READY_TO_COUNTDOWN);
			this.videoReward.ShowRewardCurrent();
		}
		this.state = GiftVideoReward.State.COMPLETED;
		this.objButtonState[1].SetActive(false);
		this.objButtonState[2].SetActive(false);
		this.objButtonState[0].SetActive(false);
		this.objButtonState[3].SetActive(true);
		InforReward[] array = new InforReward[this.cardReward.Length];
		for (int i = 0; i < this.cardReward.Length; i++)
		{
			array[i] = new InforReward();
		}
		array[0].item = Item.Gem;
		array[0].amount = this.reward_secured[0].Value;
		PopupManager.Instance.SaveReward(Item.Gem, array[0].amount, "VideoGift_Index:" + this.cardReward.Length, null);
		switch (this.videoReward.RewardCurrent.Data.Value)
		{
		case 2:
			array[1].item = Item.Medkit;
			array[1].amount = this.reward_secured[1].Value;
			PopupManager.Instance.SaveReward(Item.Medkit, this.reward_secured[1].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			break;
		case 3:
			array[1].item = Item.Booster_Damage;
			array[1].amount = this.reward_secured[1].Value;
			PopupManager.Instance.SaveReward(Item.Booster_Damage, this.reward_secured[1].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			array[2].item = Item.Common_Crate;
			array[2].amount = this.reward_secured[2].Value;
			PopupManager.Instance.SaveReward(Item.Common_Crate, this.reward_secured[2].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			break;
		case 4:
			array[1].item = Item.Booster_Amor;
			array[1].amount = this.reward_secured[1].Value;
			PopupManager.Instance.SaveReward(Item.Booster_Amor, this.reward_secured[1].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			array[2].item = Item.Common_Crate;
			array[2].amount = this.reward_secured[2].Value;
			PopupManager.Instance.SaveReward(Item.Common_Crate, this.reward_secured[2].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			array[3].item = Item.Epic_Crate;
			array[3].amount = this.reward_secured[3].Value;
			PopupManager.Instance.SaveReward(Item.Epic_Crate, this.reward_secured[3].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			break;
		case 5:
			array[1].item = Item.Medkit;
			array[1].amount = this.reward_secured[1].Value;
			PopupManager.Instance.SaveReward(Item.Medkit, this.reward_secured[1].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			array[2].item = Item.Common_Crate;
			array[2].amount = this.reward_secured[2].Value;
			PopupManager.Instance.SaveReward(Item.Common_Crate, this.reward_secured[2].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			array[3].item = Item.Epic_Crate;
			array[3].amount = this.reward_secured[3].Value;
			PopupManager.Instance.SaveReward(Item.Epic_Crate, this.reward_secured[3].Value, "VideoGift_Index:" + this.cardReward.Length, null);
			break;
		}
		PopupManager.Instance.ShowCongratulation(array, false, null);
		this.isPressButton = false;
		MenuManager.Instance.topUI.ShowCoin();
	}

	public CardBase[] cardReward;

	public GameObject[] objButtonState;

	public Text txtTime;

	public Text txt_ReadyToClaim;

	public Text txt_NotReady;

	public GiftVideoReward.State state;

	private IntProfileData saveState;

	private PopupGift videoReward;

	private StringBuilder builderTimeVideo;

	private bool isGetReward;

	private bool isReadyToClaimed;

	private SecuredInt[] reward_secured;

	private bool isinit;

	private bool isPressButton;

	public int TimeGetReward;

	public enum State
	{
		NOT_READY,
		READY_TO_COUNTDOWN,
		READY_TO_CLAIM,
		COMPLETED
	}
}
