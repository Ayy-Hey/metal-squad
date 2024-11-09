using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCongratulation : PopupBase
{
	public void ShowListReward(InforReward[] list, Action actionClose, bool isDoubleReward)
	{
		if (list == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.isDoubleReward = isDoubleReward;
		base.Show();
		this.cacheReward.Clear();
		this.amountItem = 0;
		this.OnClosed = actionClose;
		this.listRewards.DestroyAll();
		this.CardDoubleReward.Clear();
		this.listRewards.CreateObj(list.Length);
		this.amountItem = list.Length;
		for (int i = 0; i < list.Length; i++)
		{
			CardReward component = this.listRewards.listObjs[i].GetComponent<CardReward>();
			component.rectTransform().localScale = Vector3.one * 0.95f;
			component.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(list[i].item)];
			component.item = list[i].item;
			component.txt_Note.gameObject.SetActive(false);
			component.img_Main.sprite = PopupManager.Instance.sprite_Item[(int)list[i].item];
			component.txt_Amount.text = string.Empty + list[i].amount;
			component.img_Vip.gameObject.SetActive(list[i].vipLevel >= 0);
			component.img_Vip.sprite = PopupManager.Instance.spriteVip[list[i].vipLevel + 1];
			this.cacheReward.Add(component);
		}
		base.StartCoroutine(this.PlayAnimShowList());
	}

	public void OnDoubleReward()
	{
		this.objDoubleReward.SetActive(false);
		AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isDone)
		{
			if (isDone)
			{
				base.Invoke("DoubleReward", 0.5f);
			}
			else
			{
				this.objDoubleReward.SetActive(true);
			}
		});
	}

	private void DoubleReward()
	{
		this.amountItem += this.cacheReward.Count;
		this.listRewards.CreateObj(this.amountItem);
		for (int i = 0; i < this.cacheReward.Count; i++)
		{
			CardReward component = this.listRewards.listObjs[i + this.cacheReward.Count].GetComponent<CardReward>();
			component.gameObject.SetActive(false);
			component.rectTransform().localScale = Vector3.one * 0.95f;
			component.img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(this.cacheReward[i].item)];
			component.item = this.cacheReward[i].item;
			component.img_Main.sprite = PopupManager.Instance.sprite_Item[(int)this.cacheReward[i].item];
			component.txt_Amount.text = this.cacheReward[i].txt_Amount.text;
			component.img_Vip.gameObject.SetActive(false);
			component.txt_Note.gameObject.SetActive(true);
			component.txt_Note.text = PopupManager.Instance.GetText(Localization0.Bonus, null).ToUpper();
			this.CardDoubleReward.Add(component);
			PopupManager.Instance.SaveReward(component.item, int.Parse(this.cacheReward[i].txt_Amount.text), "DoubleReward_PopupCongratulation", null);
		}
		base.StartCoroutine(this.PlayAnimShowList2());
	}

	public IEnumerator PlayAnimShowList()
	{
		this.objDoubleReward.SetActive(false);
		this.objSkip.SetActive(false);
		this.objTapScreenToSkip.SetActive(false);
		PopupManager.isblockInput = true;
		for (int j = 0; j < this.listRewards.listObjs.Count; j++)
		{
			this.listRewards.listObjs[j].SetActive(false);
		}
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < this.amountItem; i++)
		{
			AudioClick.Instance.OnBumb();
			this.listRewards.listObjs[i].SetActive(true);
			CardReward card = this.listRewards.listObjs[i].GetComponent<CardReward>();
			card.twn_AlphaAll.PlayFromStart(false);
			card.twn_ScaleAll.PlayFromStart(false);
			card.twn_AlphaGlow.PlayFromStart(false);
			card.twn_ScaleGlow.PlayFromStart(false);
			card.ShowBorderEffect();
			yield return new WaitForSeconds(0.3f);
		}
		PopupManager.isblockInput = false;
		if (!this.isDoubleReward)
		{
			this.objDoubleReward.SetActive(false);
			this.objSkip.SetActive(false);
			this.objTapScreenToSkip.SetActive(true);
		}
		else
		{
			base.StartCoroutine(this.WaitVideoReady());
		}
		yield break;
	}

	private IEnumerator WaitVideoReady()
	{
		this.objDoubleReward.SetActive(true);
		this.objSkip.SetActive(true);
		this.imgBGVideo.color = this.colorNotReady;
		this.txtobjVideoLoading.SetActive(true);
		yield return new WaitUntil(() => AdmobManager.Instance.IsVideoReady());
		this.imgBGVideo.color = this.colorReady;
		this.txtobjVideoLoading.SetActive(false);
		yield break;
	}

	public IEnumerator PlayAnimShowList2()
	{
		this.objSkip.SetActive(false);
		for (int i = 0; i < this.CardDoubleReward.Count; i++)
		{
			AudioClick.Instance.OnBumb();
			CardReward card = this.CardDoubleReward[i];
			card.gameObject.SetActive(true);
			card.twn_AlphaAll.PlayFromStart(false);
			card.twn_ScaleAll.PlayFromStart(false);
			card.twn_AlphaGlow.PlayFromStart(false);
			card.twn_ScaleGlow.PlayFromStart(false);
			card.ShowBorderEffect();
			yield return new WaitForSeconds(0.3f);
		}
		PopupManager.isblockInput = false;
		this.objDoubleReward.SetActive(false);
		this.objSkip.SetActive(false);
		this.objTapScreenToSkip.SetActive(true);
		yield break;
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private int amountItem;

	public GridLayoutGroup girdLayoutGroup;

	public FactoryObject listRewards;

	private List<CardReward> cacheReward = new List<CardReward>();

	private List<CardReward> CardDoubleReward = new List<CardReward>();

	public GameObject objSkip;

	public GameObject objDoubleReward;

	public GameObject objTapScreenToSkip;

	private bool isDoubleReward;

	public GameObject txtobjVideoLoading;

	public Color colorNotReady;

	public Color colorReady;

	public Image imgBGVideo;
}
