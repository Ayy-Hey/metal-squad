using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ShowGoldUI : MonoBehaviour
{
	public void OnInit()
	{
		this.strBuilder = new StringBuilder();
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void Show()
	{
		this.animGold.Play(0);
		GameManager.Instance.COIN_COLLECTED += ((!GameManager.Instance.isDoubleCoin) ? 1 : 2) * 3;
		this.txtCoin.text = GameManager.Instance.COIN_COLLECTED.ToString();
	}

	[SerializeField]
	private Text txtCoin;

	[SerializeField]
	private Animator animGold;

	[SerializeField]
	private StringBuilder strBuilder;
}
