using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Rank;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CellViewLeaderboard : EnhancedScrollerCellView
{
	public UserLB user { get; set; }

	private void Start()
	{
		if (this.buttonInvite != null)
		{
			this.buttonInvite.onClick.AddListener(new UnityAction(this.OnInvite));
		}
		if (this.buttonShowProfile != null)
		{
			this.buttonShowProfile.onClick.AddListener(new UnityAction(this.OnShow));
		}
	}

	public void SetData(UserLB user, PopupLeaderboard leaderboard)
	{
		if (this.typeObject.Length > 1)
		{
			this.typeObject[0].SetActive(true);
			this.typeObject[1].SetActive(false);
		}
		int exp = 0;
		try
		{
			if (user == null || !leaderboard.gameObject.activeSelf || !leaderboard.isActive)
			{
				base.gameObject.SetActive(false);
				return;
			}
			exp = int.Parse(user.GetValue(PlayfabManager.EKey.Vip));
		}
		catch (Exception ex)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.user = user;
		this.leaderboard = leaderboard;
		base.gameObject.SetActive(true);
		int num = VipManager.Instance.LevelCurrent(exp);
		num++;
		this.imgVip.sprite = leaderboard.spritesVip[num];
		this.txtPower.text = user.GetValue(PlayfabManager.EKey.Power);
		int num2 = 0;
		try
		{
			num2 = int.Parse(user.GetValue(PlayfabManager.EKey.LevelRank));
		}
		catch
		{
		}
		RankInfor rankInfoByLevel = RankManager.Instance.GetRankInfoByLevel(num2);
		if (num2 < this.spriteIcons.Length)
		{
			this.imgRank.sprite = this.spriteIcons[num2];
		}
		this.SetUserPictureSprite(user);
		int num3 = 998;
		try
		{
			num3 = user.entry.Position;
		}
		catch (Exception ex2)
		{
		}
		this.SetUserPosition(num3 + 1);
		this.userNameText.text = ((!string.IsNullOrEmpty(user.UserName)) ? user.UserName : user.ID);
		base.gameObject.name = user.ID;
		int num4 = 0;
		try
		{
			num4 = user.entry.StatValue;
		}
		catch (Exception ex3)
		{
		}
		if (leaderboard.isPvpLeaderboard)
		{
			this.objScore[0].SetActive(false);
			this.objScore[1].SetActive(true);
			this.txtScorePvp.text = num4.ToString();
		}
		else
		{
			this.objScore[1].SetActive(false);
			this.objScore[0].SetActive(true);
			this.txtScore.text = num4.ToString();
		}
		if (user.entry != null && user.entry.Profile.PlayerId.Equals(FirebaseDatabaseManager.Instance.IDPlayFab))
		{
			this.bgImage.sprite = this.bgSprites[0];
		}
		else
		{
			this.bgImage.sprite = this.bgSprites[1];
		}
		if (user.spriteFlag != null)
		{
			this.countryImage.gameObject.SetActive(true);
			this.countryImage.sprite = user.spriteFlag;
		}
	}

	private void SetUserPictureSprite(UserLB user)
	{
		if (user.spriteAvatar != null)
		{
			this.userPictureImage.sprite = user.spriteAvatar;
			try
			{
				this.userPictureImage.gameObject.SetActive(true);
				this.userPictureImage.transform.GetChild(0).gameObject.SetActive(true);
			}
			catch
			{
			}
		}
	}

	private void SetUserPosition(int userPosition)
	{
		this.userPositionText.text = userPosition.ToString();
		this.userPositionBox.gameObject.SetActive(true);
		switch (userPosition)
		{
		case 1:
			this.userPositionBox.sprite = this.golden;
			break;
		case 2:
			this.userPositionBox.sprite = this.silver;
			break;
		case 3:
			this.userPositionBox.sprite = this.bronze;
			break;
		default:
			this.userPositionBox.gameObject.SetActive(false);
			break;
		}
	}

	private void SetColorPlayer(bool isPlayer)
	{
	}

	private void SetUserPositionBoxColor(Color color)
	{
		this.userPositionBox.color = color;
	}

	public void OnClear()
	{
		this.userPictureImage.sprite = this.spriteDefault;
		base.StopAllCoroutines();
	}

	public void ShowMyProfile(PopupLeaderboard leaderboard)
	{
		if (!leaderboard.gameObject.activeSelf || !leaderboard.isActive)
		{
			return;
		}
		if (this.typeObject.Length > 1)
		{
			this.typeObject[0].SetActive(true);
			this.typeObject[1].SetActive(false);
		}
		this.user = new UserLB();
		this.user.CoppyDataProfile();
		this.userPositionText.text = "???";
		this.userNameText.text = ProfileManager.InforChars[ProfileManager.settingProfile.IDChar].Name;
		if (!string.IsNullOrEmpty(ProfileManager.pvpProfile.UserName))
		{
			this.userNameText.text = ProfileManager.pvpProfile.UserName;
		}
		if (leaderboard.isPvpLeaderboard)
		{
			this.objScore[0].SetActive(false);
			this.objScore[1].SetActive(true);
			this.txtScorePvp.text = ProfileManager.pvpProfile.Score.ToString();
		}
		else
		{
			this.objScore[1].SetActive(false);
			this.objScore[0].SetActive(true);
			this.txtScore.text = ProfileManager._CampaignProfile.GetTotalStar.ToString();
		}
		this.imgVip.sprite = leaderboard.spritesVip[Mathf.Clamp(VipManager.Instance.LevelCurrent(), 0, leaderboard.spritesVip.Length)];
		this.txtPower.text = PowerManager.Instance.TotalPower().ToString();
		RankInfor rankCurrentByExp = RankManager.Instance.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
		this.imgRank.sprite = this.spriteIcons[rankCurrentByExp.Level];
	}

	public void OnShowInvite()
	{
		if (this.typeObject.Length > 1)
		{
			this.typeObject[0].SetActive(false);
			this.typeObject[1].SetActive(true);
		}
	}

	private void OnInvite()
	{
		List<string> list = null;
		string empty = string.Empty;
		string text = "Metal Squad: Shooting Game";
		string text2 = "This game is awesome, join me. now.";
		string message = text2;
		IEnumerable<string> to = list;
		IEnumerable<object> filters = null;
		IEnumerable<string> excludeIds = null;
		int? maxRecipients = null;
		string data = empty;
		string title = text;
		
	}


	public void OnShow()
	{
		if (this.leaderboard != null && this.user != null)
		{
			this.leaderboard.transform.position = Vector3.one * 999f;
			this.leaderboard.profile.OnShow(this.user, delegate
			{
				this.leaderboard.transform.position = Vector3.zero;
			});
		}
	}

	public Image userPictureImage;

	public Image countryImage;

	public Image userPositionBox;

	public Text userPositionText;

	public Text userNameText;

	public Image bgImage;

	public Sprite[] bgSprites;

	[Header("First Places colors")]
	public Sprite golden;

	public Sprite silver;

	public Sprite bronze;

	[SerializeField]
	private Sprite[] spriteIcons;

	public Image imgVip;

	public Text txtPower;

	public Image imgRank;

	public GameObject[] objScore;

	public Text txtScorePvp;

	public Text txtScore;

	private PopupLeaderboard leaderboard;

	public Sprite spriteDefault;

	public GameObject[] typeObject;

	public Button buttonInvite;

	public Button buttonShowProfile;

}
