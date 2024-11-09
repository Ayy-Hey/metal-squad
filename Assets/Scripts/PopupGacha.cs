using System;
using System.Collections;
using System.Text;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PopupGacha : PopupBase
{
	public void ShowInforBox()
	{
		this.objInfor.SetActive(true);
		for (int i = 0; i < this.objItemEpic.Length; i++)
		{
			this.objItemEpic[i].SetActive(this.idGachaSelect != 0);
			this.objItemCommon[i].SetActive(this.idGachaSelect == 0);
		}
		this.txtName.text = ((this.idGachaSelect != 0) ? PopupManager.Instance.GetText(Localization0.Epic_Crate, null) : PopupManager.Instance.GetText(Localization0.Common_Crate, null));
		this.txtName.color = ((this.idGachaSelect != 0) ? Color.yellow : Color.white);
		for (int j = 0; j < this.txtPercentItem.Length; j++)
		{
			this.txtPercentItem[j].text = ((this.idGachaSelect != 0) ? this.PercentBox2[j] : this.PercentBox1[j]);
		}
		this.imgBox.sprite = PopupManager.Instance.sprite_Item[(this.idGachaSelect != 0) ? 39 : 38];
		this.listComponnetGacha.gameObject.SetActive(false);
	}

	public void DisableInforBox()
	{
		this.listComponnetGacha.gameObject.SetActive(true);
		this.objInfor.SetActive(false);
	}

	public override void Show()
	{
		base.Show();
		if (ThisPlatform.IsIphoneX)
		{
			this.objGacha.transform.localPosition = new Vector3(-35f, this.objGacha.transform.localPosition.y, this.objGacha.transform.localPosition.z);
		}
		else
		{
			float num = ((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f;
			if (num < 0.9f)
			{
				this.listGacha[0].GetComponent<RectTransform>().localScale = Vector3.one * 0.55f;
				this.listGacha[1].GetComponent<RectTransform>().localScale = Vector3.one * 0.55f;
				this.listGacha[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-324f, -53.4f);
				this.listGacha[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-11.9f, -45.2f);
			}
			this.objGacha.transform.localPosition = new Vector3(0f, this.objGacha.transform.localPosition.y, this.objGacha.transform.localPosition.z);
		}
		this.listInforGacha = new ListInforGacha[2];
		for (int i = 0; i < 2; i++)
		{
			this.listInforGacha[i] = new ListInforGacha();
			this.listInforGacha[i].Create(DataLoader.dataGacha[i]["components"].Count);
			int num2 = 0;
			for (int j = 0; j < DataLoader.dataGacha[i]["components"].Count; j++)
			{
				this.listInforGacha[i].inforSpin[j] = new InforGacha();
				this.listInforGacha[i].inforSpin[j].item = (Item)DataLoader.dataGacha[i]["components"][j]["id"].ToInt();
				this.listInforGacha[i].inforSpin[j].amount = DataLoader.dataGacha[i]["components"][j]["quantity"].ToInt();
				this.listInforGacha[i].inforSpin[j].startWeight = num2 + 1;
				num2 += DataLoader.dataGacha[i]["components"][j]["weight"].ToInt();
				this.listInforGacha[i].inforSpin[j].endWeight = num2;
				if (!this.listInforGacha[i].inforDisplay.ContainsKey(this.listInforGacha[i].inforSpin[j].item.ToString()))
				{
					this.listInforGacha[i].inforDisplay.Add(this.listInforGacha[i].inforSpin[j].item.ToString(), this.listInforGacha[i].inforSpin[j].item);
				}
			}
		}
		this.ClickToGacha(0);
		this.gachaChild[1].SetStatus(false);
		this.tran_ComponnetGachaBase = this.listComponnetGacha.listObjs[0].transform;
		this.tran_ComponnetGachaBase.localPosition = Vector3.zero;
		this.strBuilder = new StringBuilder();
		this.DisableInforBox();
	}

	private void ClickToGacha(int index)
	{
		this.isNotSkip = false;
		this.gachaChild[this.idGachaSelect].SetStatus(false);
		this.idGachaSelect = index;
		this.txtGachaName.text = ((this.idGachaSelect != 0) ? PopupManager.Instance.GetText(Localization0.Epic_Crate, null) : PopupManager.Instance.GetText(Localization0.Common_Crate, null));
		this.txtGachaName.color = this.gachaNameColors[this.idGachaSelect];
		this.gachaChild[this.idGachaSelect].SetStatus(true);
		this.amountComponent = this.listInforGacha[this.idGachaSelect].inforDisplay.Count;
		this.listComponnetGacha.CreateObj(this.amountComponent);
		this.txt_AmountGacha.text = string.Empty + ProfileManager.userProfile.GetTotalGacha(this.idGachaSelect);
		int num = 0;
		foreach (Item item in this.listInforGacha[this.idGachaSelect].inforDisplay.Values)
		{
			CardReward component = this.listComponnetGacha.listObjs[num].GetComponent<CardReward>();
			component.transform.localScale = 100f * Vector3.one;
			component.item = item;
			component.sprite_Item.sprite = PopupManager.Instance.sprite_Item[(int)component.item];
			component.sprite_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(component.item)];
			component.ShowBorderEffect();
			string text = "x" + this.listInforGacha[this.idGachaSelect].inforSpin[num].amount.ToString();
			component.txtMesh_Name.text = text;
			num++;
		}
		this.CheckButtonSpin();
	}

	private void Update()
	{
		this.tran_ComponnetGachaBase.localPosition -= Vector3.right * 50f * Time.deltaTime;
		for (int i = 0; i < this.amountComponent; i++)
		{
			this.tran_ComponnentCase = this.listComponnetGacha.listObjs[i].transform;
			this.tran_ComponnentCase.localPosition = Vector3.right * (this.tran_ComponnetGachaBase.localPosition.x + (float)(165 * i) - Time.deltaTime);
			if (this.tran_ComponnentCase.localPosition.x < (float)(-(float)this.amountComponent * 165 / 2))
			{
				this.pos_Case.x = this.tran_ComponnentCase.localPosition.x + (float)(this.amountComponent * 165);
				this.pos_Case.y = this.tran_ComponnentCase.localPosition.y;
				this.pos_Case.z = 0f;
				this.tran_ComponnentCase.localPosition = this.pos_Case;
			}
			else if (this.tran_ComponnentCase.localPosition.x >= (float)(this.amountComponent * 165 / 2))
			{
				this.pos_Case.x = this.tran_ComponnentCase.localPosition.x - (float)(this.amountComponent * 165);
				this.pos_Case.y = this.tran_ComponnentCase.localPosition.y;
				this.pos_Case.z = 0f;
				this.tran_ComponnentCase.localPosition = this.pos_Case;
			}
		}
		if (this.idGachaSelect == 0 && this.txtTimeFree.gameObject.activeSelf && this.strBuilder != null)
		{
			int num = 300 - TimePlay.timeGachaFree;
			int num2 = num / 60;
			int num3 = num % 60;
			this.strBuilder.Length = 0;
			this.strBuilder.Append(PopupManager.Instance.GetText(Localization0.Free_in, null) + " ");
			if (num2 < 10)
			{
				this.strBuilder.Append("0");
			}
			this.strBuilder.Append(num2);
			this.strBuilder.Append(":");
			if (num3 < 10)
			{
				this.strBuilder.Append("0");
			}
			this.strBuilder.Append(num3);
			this.txtTimeFree.text = this.strBuilder.ToString();
			if (num <= 0)
			{
				PlayerPrefs.SetInt(this.saveStateButtonFree, 0);
				this.CheckGachaFree();
				this.txtTimeFree.gameObject.SetActive(false);
			}
		}
	}

	private void ShowAnimReward(int idGacha, Action OnAnimRewardCompleted)
	{
		if (idGacha < 0)
		{
			return;
		}
		this.objReward.SetActive(true);
		AudioClick.Instance.OnOpenBox();
		this.skeletonGraphic.Skeleton.SetSkin(this.SkinAnim[idGacha]);
		this.skeletonGraphic.AnimationState.SetAnimation(0, this.anim2[idGacha], false);
		this.skeletonGraphic.AnimationState.Complete += delegate(TrackEntry trackEntry)
		{
			if (trackEntry.Animation == this.anim2[idGacha].Animation)
			{
				OnAnimRewardCompleted();
			}
		};
	}

	public void BuyGacha()
	{
		MenuManager.Instance.popupBuy.Show(PopupBuy.ItemType.Gacha, this.idGachaSelect, this.listGacha[this.idGachaSelect].item, (this.idGachaSelect != 0) ? PopupManager.Instance.GetText(Localization0.Epic_Crate, null) : PopupManager.Instance.GetText(Localization0.Common_Crate, null), PopupManager.Instance.sprite_Item[(int)this.listGacha[this.idGachaSelect].item], DataLoader.dataGacha[this.idGachaSelect]["cost"].ToInt(), ProfileManager.userProfile.GetTotalGacha(this.idGachaSelect), -1, true, new Action(this.CheckButtonSpin));
	}

	public void SpinGacha()
	{
		
		this.isReadyOpenSpin = false;
		base.StartCoroutine(this.WaitSpin());
		if (!this.objImgVideo.activeSelf)
		{
			this.isReadyOpenSpin = true;
		}
		else
		{
			AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					this.isVideoShow = true;
					this.isReadyOpenSpin = true;
				}
				else
				{
					this.isReadyOpenSpin = false;
					base.StopCoroutine(this.WaitSpin());
				}
			});
		}
	}

	private IEnumerator WaitSpin()
	{
		yield return new WaitUntil(() => this.isReadyOpenSpin);
		if (!this.isNotSkip)
		{
			PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Gacha, this.idGachaSelect), -1, base.name + "_Use", null);
		}
		this.isNotSkip = false;
		if (this.idGachaSelect == 0)
		{
			if (ProfileManager.userProfile.gachaFreeProfile.Data || PlayerPrefs.GetInt(this.saveStateButtonFree, 0) == 0)
			{
				ProfileManager.userProfile.gachaFreeProfile.setValue(false);
				PlayerPrefs.SetInt(this.saveStateButtonFree, 1);
			}
			this.CheckGachaFree();
		}
		this.CheckButtonSpin();
		int totalReward = 3;
		InforReward[] listRewards = new InforReward[totalReward];
		for (int i = 0; i < totalReward; i++)
		{
			int rewardGacha = this.GetRewardGacha();
			if (rewardGacha < 0)
			{
				break;
			}
			PopupManager.Instance.SaveReward(this.listInforGacha[this.idGachaSelect].inforSpin[rewardGacha].item, this.listInforGacha[this.idGachaSelect].inforSpin[rewardGacha].amount, "Gacha:" + this.idGachaSelect, null);
			listRewards[i] = new InforReward();
			listRewards[i].amount = this.listInforGacha[this.idGachaSelect].inforSpin[rewardGacha].amount;
			listRewards[i].item = this.listInforGacha[this.idGachaSelect].inforSpin[rewardGacha].item;
		}
		this.ShowAnimReward(this.idGachaSelect, delegate
		{
			PopupManager.Instance.ShowCongratulation(listRewards, true, null);
			this.objReward.SetActive(false);
			if (this.isVideoShow)
			{
				PlayerPrefs.SetInt(this.saveStateButtonFree, 2);
				this.CheckGachaFree();
				TimePlay.timeGachaFree = 0;
				PlayerPrefs.GetInt("metal.squad.time.play.of.day.gacha", 0);
				this.isVideoShow = false;
			}
		});
		DailyQuestManager.Instance.MissionGacha(null);
		yield break;
	}

	private int GetRewardGacha()
	{
		int num = UnityEngine.Random.Range(1, this.listInforGacha[this.idGachaSelect].inforSpin[this.listInforGacha[this.idGachaSelect].inforSpin.Length - 1].endWeight + 1);
		for (int i = 0; i < this.listInforGacha[this.idGachaSelect].inforSpin.Length; i++)
		{
			if (this.listInforGacha[this.idGachaSelect].inforSpin[i].startWeight <= num && num <= this.listInforGacha[this.idGachaSelect].inforSpin[i].endWeight)
			{
				return i;
			}
		}
		return -1;
	}

	private void CheckButtonSpin()
	{
		this.txt_AmountGacha.text = string.Empty + ProfileManager.userProfile.GetTotalGacha(this.idGachaSelect);
		this.btn_Spin.interactable = (ProfileManager.userProfile.GetTotalGacha(this.idGachaSelect) > 0);
		this.obj_EffectBtnSpin.SetActive(this.btn_Spin.interactable);
		this.twn_ScaleBtnPlus.enabled = (ProfileManager.userProfile.GetTotalGacha(this.idGachaSelect) <= 0);
		this.twn_ScaleBtnPlus.rectTransform().localScale = Vector3.one;
		for (int i = 0; i < this.gachaChild.Length; i++)
		{
			this.gachaChild[i].Show(ProfileManager.userProfile.GetTotalGacha(i) > 0);
		}
		this.CheckGachaFree();
	}

	private void CheckGachaFree()
	{
		if (this.idGachaSelect != 0)
		{
			this.txtButtonSpin[0].text = PopupManager.Instance.GetText(Localization0.Open, null).ToUpper();
			this.txtButtonSpin[1].text = string.Empty;
			this.objImgVideo.SetActive(false);
			this.txtTimeFree.gameObject.SetActive(false);
			return;
		}
		int @int = PlayerPrefs.GetInt(this.saveStateButtonFree, 0);
		if (ProfileManager.userProfile.gachaFreeProfile.Data || @int == 0)
		{
			this.txtButtonSpin[1].text = PopupManager.Instance.GetText(Localization0.Free, null).ToUpper();
			this.txtButtonSpin[0].text = string.Empty;
			this.btn_Spin.interactable = true;
			this.obj_EffectBtnSpin.SetActive(this.btn_Spin.interactable);
			this.twn_ScaleBtnPlus.enabled = false;
			this.objImgVideo.SetActive(false);
			this.txtTimeFree.gameObject.SetActive(false);
			this.isNotSkip = true;
		}
		else if (ProfileManager.userProfile.GetTotalGacha(this.idGachaSelect) <= 0)
		{
			this.isNotSkip = true;
			if (@int != 1)
			{
				if (@int == 2)
				{
					this.btn_Spin.interactable = false;
					this.obj_EffectBtnSpin.SetActive(this.btn_Spin.interactable);
					this.twn_ScaleBtnPlus.enabled = true;
					this.objImgVideo.SetActive(false);
					this.txtTimeFree.gameObject.SetActive(true);
				}
			}
			else
			{
				this.btn_Spin.interactable = true;
				this.obj_EffectBtnSpin.SetActive(this.btn_Spin.interactable);
				this.twn_ScaleBtnPlus.enabled = false;
				this.objImgVideo.SetActive(true);
				this.txtTimeFree.gameObject.SetActive(false);
			}
			this.txtButtonSpin[1].text = PopupManager.Instance.GetText(Localization0.Free, null).ToUpper();
			this.txtButtonSpin[0].text = string.Empty;
		}
		else
		{
			this.objImgVideo.SetActive(false);
			this.txtTimeFree.gameObject.SetActive(false);
			this.txtButtonSpin[0].text = PopupManager.Instance.GetText(Localization0.Open, null).ToUpper();
			this.txtButtonSpin[1].text = string.Empty;
			this.isNotSkip = false;
		}
	}

	public void BtnClickToGacha(int index)
	{
		this.ClickToGacha(index);
		
	}

	public GameObject objInfor;

	public Text txtName;

	public Text[] txtPercentItem;

	public Image imgBox;

	private int idGachaSelect;

	private int amountComponent;

	private Transform tran_ComponnetGachaBase;

	public Text txtGachaName;

	public Color[] gachaNameColors;

	public Text txt_AmountGacha;

	public Button btn_Spin;

	public GameObject obj_EffectBtnSpin;

	public TweenScale twn_ScaleBtnPlus;

	public CardBase[] listGacha;

	public FactoryObject listComponnetGacha;

	public SkeletonGraphic skeletonGraphic;

	public AnimationReferenceAsset[] anim2;

	[SpineSkin("", "", true, false)]
	public string[] SkinAnim;

	public GameObject objReward;

	public GameObject objGacha;

	public GachaChildSpine[] gachaChild;

	public Text[] txtButtonSpin;

	private StringBuilder strBuilder;

	private ListInforGacha[] listInforGacha;

	private readonly string[] PercentBox1 = new string[]
	{
		"17%",
		"17%",
		"11%",
		"32%",
		"18%",
		"5%"
	};

	private readonly string[] PercentBox2 = new string[]
	{
		"24%",
		"25%",
		"17%",
		"15%",
		"16%",
		"3%"
	};

	public GameObject[] objItemEpic;

	public GameObject[] objItemCommon;

	private Transform tran_ComponnentCase;

	private Vector3 pos_Case;

	private bool isReadyOpenSpin;

	private bool isNotSkip;

	private bool isVideoShow;

	public GameObject objImgVideo;

	public Text txtTimeFree;

	private string saveStateButtonFree = "sora.metal.squad.formgacha.statebuttonfree";
}
