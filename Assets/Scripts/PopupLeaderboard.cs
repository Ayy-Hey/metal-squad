using System;
using System.Collections;
using ABI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using Profile.UI;
using UnityEngine;

public class PopupLeaderboard : PopupBase, IEnhancedScrollerDelegate
{
	public override void OnClose()
	{
		if (!this.isSkipLoad)
		{
		}
		try
		{
			MenuManager.Instance.topUI.ShowCoin();
		}
		catch
		{
		}
		base.StopAllCoroutines();
		base.OnClose();
	}

	public override void Show()
	{
		base.Show();
		this.scroller.cellViewVisibilityChanged = new CellViewVisibilityChangedDelegate(this.CellViewVisibilityChanged);
		this.scroller.Delegate = this;
		PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).OnGlobalEnded = delegate()
		{
			this.OnShowGlobal();
			UnityEngine.Debug.Log("Reload OnShowGlobal");
		};
		this.OnShowGlobal();
	}

	public void OnShowGlobal()
	{
		this._data = new SmallList<UserLB>();
		for (int i = 0; i < PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).ListTopPlayer.Count; i++)
		{
			this._data.Add(PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).ListTopPlayer[i]);
		}
		try
		{
			this.scroller.ReloadData(0f);
		}
		catch (Exception ex)
		{
		}
		if (!FirebaseDatabaseManager.Instance.isLoginFB)
		{
			this.cellViewPlayer.ShowMyProfile(this);
		}
		else
		{
			this.cellViewPlayer.SetData(PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).MyProfile, this);
		}
		this.loginButton.SetActive(!FirebaseDatabaseManager.Instance.isLoginFB);
		this.objActiveButtonFilter[0].SetActive(true);
		this.objActiveButtonFilter[1].SetActive(false);
	}

	public void OnShowFriends()
	{
		if (!FirebaseDatabaseManager.Instance.isLoginFB)
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.You_must_login_to_facebook_in_advance, null), PopupManager.Instance.GetText(Localization0.Login, null));
			return;
		}
		this._data = new SmallList<UserLB>();
		this._data.Add(null);
		for (int i = 0; i < PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).ListTopFriends.Count; i++)
		{
			this._data.Add(PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).ListTopFriends[i]);
		}
		this.scroller.ReloadData(0f);
		this.objActiveButtonFilter[1].SetActive(true);
		this.objActiveButtonFilter[0].SetActive(false);
	}

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return this._data.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 100f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		CellViewLeaderboard cellViewLeaderboard = scroller.GetCellView(this.cellViewPrefab) as CellViewLeaderboard;
		cellViewLeaderboard.name = "Cell " + dataIndex.ToString();
		return cellViewLeaderboard;
	}

	private void CellViewVisibilityChanged(EnhancedScrollerCellView cellView)
	{
		CellViewLeaderboard cellViewLeaderboard = cellView as CellViewLeaderboard;
		if (cellView.active)
		{
			if (this._data[cellView.cellIndex] == null)
			{
				cellViewLeaderboard.OnShowInvite();
			}
			else
			{
				cellViewLeaderboard.SetData(this._data[cellView.cellIndex], this);
			}
		}
		else
		{
			cellViewLeaderboard.OnClear();
		}
	}

	public void FacebookLogin()
	{
		
	}

	private IEnumerator WaitForPlayFabLoginCoroutine()
	{
		yield return new WaitUntil(() => FirebaseDatabaseManager.Instance.isLoginFB);
		int score = 0;
		if (this.isPvpLeaderboard)
		{
			score = ProfileManager.pvpProfile.Score;
		}
		else
		{
			score = ProfileManager._CampaignProfile.GetTotalStar;
		}
		PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).PostMyScore(score, delegate(bool result)
		{
			if (result)
			{
				PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).GetMyProfile(delegate(bool ok)
				{
					if (ok)
					{
						this.OnShowMyProfile();
					}
				});
				PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).GetTopPlayer(delegate(bool isSuccessGetTopPlayer)
				{
					if (isSuccessGetTopPlayer)
					{
						this.OnShowGlobal();
					}
				});
			}
		});
		FirebaseDatabaseManager.Instance.CheckVersionFB(delegate(int version)
		{
			DialogWarningOldData dialogWarningOldData = UnityEngine.Object.Instantiate(Resources.Load("Popup/DialogWarningOldData", typeof(DialogWarningOldData)), MenuManager.Instance.formCanvas.transform) as DialogWarningOldData;
			dialogWarningOldData.version = version;
			dialogWarningOldData.OnClosed = delegate()
			{
			};
			dialogWarningOldData.Show();
		});
		yield break;
	}

	private void OnShowMyProfile()
	{
		this.cellViewPlayer.SetData(PlayfabManager.Instance.GetLeaderBoard(this.isPvpLeaderboard).MyProfile, this);
	}

	public GameObject loginButton;

	public GameObject objGlobal;

	public GameObject objFriends;

	public GameObject objInvite;

	public CellViewLeaderboard cellViewPlayer;

	public GameObject[] objActiveButtonFilter;

	public PopupProfile profile;

	public Sprite[] spritesVip;

	public bool isSkipLoad;

	public bool isPvpLeaderboard;

	public GameObject txtPvpPoint;

	private SmallList<UserLB> _data;

	public EnhancedScroller scroller;

	public EnhancedScrollerCellView cellViewPrefab;
}
