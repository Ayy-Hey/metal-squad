using System;
using Achievement.UI;
using DailyQuest.UI;
using Rank.UI;
using UnityEngine;

public class PopupQuest : PopupBase
{
	private void OnEnable()
	{
		if ((float)Screen.width < (float)Screen.height * 1.7f)
		{
			this.tableObj.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		else
		{
			this.tableObj.localScale = Vector3.one;
		}
		this.ShowThongBao();
	}

	public override void Show()
	{
		base.Show();
		this.ShowDailyQuest(null);
	}

	public void ShowDailyQuest()
	{
		this.ShowDailyQuest(null);
	}

	public void ShowDailyQuest(Action<bool> callback2)
	{
		this.callback2 = callback2;
		base.Show();
		MenuManager.Instance.topUI.ReloadCoin();
		this.achievementUI.Hide();
		this.dailyQuest.Show(delegate
		{
			MenuManager.Instance.topUI.ReloadCoin();
			this.rankUI.ShowInfor();
			this.ShowThongBao();
		}, delegate(bool callbackRank)
		{
			if (callbackRank)
			{
				this.rankUI.ShowInfor();
				this.ShowThongBao();
			}
		});
		this.rankUI.ShowInfor();
		if (TutorialUIManager.tutorialIDCurrent == TutorialUIManager.TutorialID.Tut_RewardQuest)
		{
			MenuManager.Instance.tutorial.LoadRewardQuest();
			MenuManager.Instance.tutorial.NextTutorial(1);
		}
		
	}

	public void ShowAchievement()
	{
		this.ShowAchievement(null);
	}

	public void ShowAchievement(Action<bool> callback2)
	{
		this.callback2 = callback2;
		base.Show();
		MenuManager.Instance.topUI.ReloadCoin();
		this.dailyQuest.Hide();
		DataLoader.ReloadProfileAchievement();
		this.achievementUI.Show(delegate(bool callback)
		{
			if (callback)
			{
				MenuManager.Instance.topUI.ReloadCoin();
				this.rankUI.ShowInfor();
				this.ShowThongBao();
			}
		}, delegate(bool callbackRank)
		{
			if (callbackRank)
			{
				MenuManager.Instance.topUI.ReloadCoin();
				this.rankUI.ShowInfor();
				this.ShowThongBao();
			}
		});
		this.rankUI.ShowInfor();
		
	}

	public override void OnClose()
	{
		if (this.callback2 != null)
		{
			this.callback2(true);
		}
		base.OnClose();
	}

	private void ShowThongBao()
	{
		try
		{
			if (this.objAlertAchieventment != null)
			{
				this.objAlertAchieventment.SetActive(AchievementManager.Instance.hasCompleted);
			}
			if (this.objAlertDailyQuest != null)
			{
				this.objAlertDailyQuest.SetActive(DailyQuestManager.Instance.CheckDailyQuest() != -1);
			}
		}
		catch
		{
		}
	}

	public DailyQuest.UI.DailyQuest dailyQuest;

	[SerializeField]
	private RankUI rankUI;

	[SerializeField]
	private AchievementUI achievementUI;

	[SerializeField]
	private GameObject objAlertAchieventment;

	[SerializeField]
	private GameObject objAlertDailyQuest;

	private Action<bool> callback2;

	[SerializeField]
	private RectTransform tableObj;
}
