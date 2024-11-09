using System;
using UnityEngine;

public class UIShowInforManager : MonoBehaviour
{
	public static UIShowInforManager Instance
	{
		get
		{
			if (UIShowInforManager.instance == null)
			{
				UIShowInforManager.instance = UnityEngine.Object.FindObjectOfType<UIShowInforManager>();
			}
			return UIShowInforManager.instance;
		}
	}

	public void ShowMission(int coin, int ms, int exp, string strMission)
	{
		foreach (ShowInforMission showInforMission in this.showInforMission)
		{
			if (!showInforMission.isShow)
			{
				showInforMission.Show(coin, ms, exp, strMission);
				return;
			}
		}
	}

	public void ShowUnlock(string str)
	{
		foreach (ShowInforMission showInforMission in this.showInforMission)
		{
			if (!showInforMission.isShow)
			{
				showInforMission.ShowUnlock(str, "Congratulations!");
				return;
			}
		}
	}

	public void OnShowAchievement(string strDesc, string urlIcon)
	{
		foreach (ShowInforMission showInforMission in this.showInforMission)
		{
			if (!showInforMission.isShow)
			{
				showInforMission.ShowAchievement(strDesc, urlIcon);
				return;
			}
		}
	}

	public void OnShowDailyQuest(string strDesc, Item item, int amount)
	{
		foreach (ShowInforMission showInforMission in this.showInforMission)
		{
			if (!showInforMission.isShow)
			{
				showInforMission.ShowDailyQuest(strDesc, item, amount);
				return;
			}
		}
	}

	private static UIShowInforManager instance;

	public ShowInforMission[] showInforMission;
}
