using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowInforMission : MonoBehaviour
{
	public void Show(int coin, int ms, int exp, string strMission)
	{
		if (this.isShow)
		{
			return;
		}
		foreach (GameObject gameObject in this.childObject)
		{
			gameObject.SetActive(false);
		}
		this.childObject[0].SetActive(true);
		base.gameObject.SetActive(true);
		this.isShow = true;
		this.anim.Play(0);
		try
		{
			this.txtMission.text = strMission;
			this.txtCoin.text = coin.ToString();
			this.txtMS.text = ms.ToString();
			this.txtExp.text = exp.ToString();
		}
		catch
		{
		}
	}

	public void ShowUnlock(string str, string strtitle = "Congratulations!")
	{
		if (this.isShow)
		{
			return;
		}
		foreach (GameObject gameObject in this.childObject)
		{
			gameObject.SetActive(false);
		}
		this.childObject[1].SetActive(true);
		base.gameObject.SetActive(true);
		this.isShow = true;
		this.anim.Play(0);
		try
		{
			this.txtTitleUnlock.text = str;
			this.txtUnlock.text = strtitle;
		}
		catch
		{
		}
	}

	public void ShowAchievement(string strDecs, string urlIcon)
	{
		if (this.isShow)
		{
			return;
		}
		foreach (GameObject gameObject in this.childObject)
		{
			gameObject.SetActive(false);
		}
		this.childObject[2].SetActive(true);
		base.gameObject.SetActive(true);
		this.isShow = true;
		this.anim.Play(0);
		try
		{
			this.txtDescAchievement.text = strDecs;
			this.imgIconAchievement.sprite = Resources.Load<Sprite>(urlIcon);
		}
		catch
		{
		}
	}

	public void ShowDailyQuest(string strDecs, Item item, int amount)
	{
		if (this.isShow)
		{
			return;
		}
		foreach (GameObject gameObject in this.childObject)
		{
			gameObject.SetActive(false);
		}
		this.childObject[3].SetActive(true);
		base.gameObject.SetActive(true);
		this.isShow = true;
		this.anim.Play(0);
		try
		{
			this.txtDescDailyQuest.text = strDecs;
			this.imgIconDailyQuest[0].SetActive(item == Item.Gold);
			this.imgIconDailyQuest[1].SetActive(item == Item.Gem);
			this.txtValue.text = amount.ToString();
		}
		catch
		{
		}
	}

	public void OnCompleted()
	{
		this.isShow = false;
		base.gameObject.SetActive(false);
	}

	public GameObject[] childObject;

	[SerializeField]
	[Header("___________Mission___________")]
	private Text txtMission;

	[SerializeField]
	private Text txtCoin;

	[SerializeField]
	private Text txtMS;

	[SerializeField]
	private Text txtExp;

	[SerializeField]
	private Animator anim;

	public bool isShow;

	[Header("___________Unlocked___________")]
	public Text txtUnlock;

	public Text txtTitleUnlock;

	[Header("___________Achievement___________")]
	public Text txtDescAchievement;

	public Image imgIconAchievement;

	[Header("___________DailyQuest___________")]
	public Text txtDescDailyQuest;

	public Text txtValue;

	public GameObject[] imgIconDailyQuest;
}
