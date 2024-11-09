using System;
using UnityEngine;

public class PopupRate : PopupBase
{
	public void ClickToStar(int index)
	{
		this.starRate = index;
		for (int i = 0; i < this.listStarOn.Length; i++)
		{
			this.listStarOn[i].SetActive(i <= index);
		}
	}

	public void Submit()
	{
		if (this.starRate >= 4)
		{
			if (this.isOk != null)
			{
				this.isOk(true);
			}
			base.OnClose();
		}
		else
		{
			this.OnClose();
		}
	}

	public override void OnClose()
	{
		if (this.isOk != null)
		{
			this.isOk(false);
		}
		base.OnClose();
	}

	private int starRate;

	public Action<bool> isOk;

	public GameObject[] listStarOn;

	public GameObject[] listStarOff;
}
