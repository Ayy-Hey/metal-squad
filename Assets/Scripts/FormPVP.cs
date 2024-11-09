using System;
using System.Collections;
using ABI;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using PVPManager;
using UnityEngine;
using UnityEngine.UI;

public class FormPVP : FormBase
{
	private IEnumerator Start()
	{
		this.objFakeLoading.SetActive(true);
		PlayfabManager.Instance.GetLeaderBoard(true).GetTopPlayer(delegate(bool ok)
		{
			PlayfabManager.Instance.GetLeaderBoard(true).PostMyScore(ProfileManager.pvpProfile.Score, delegate(bool result)
			{
				UnityEngine.Debug.Log("result_" + result);
				PlayfabManager.Instance.GetLeaderBoard(true).GetMyProfile(null);
				if (result)
				{
					PlayfabManager.Instance.GetLeaderBoard(true).GetTopPlayer(null);
				}
			});
		});
		this.lobbyMainPanel.EnableCreateFakePlayer = (RemoteConfigFirebase.Instance.GetLongValue(RemoteConfigFirebase.MULTIPLAYER_FAKEPLAYER, 1L) == 1L);
		this.RegisterLobbyCallbacks();
		this.txtLoading.text = "97%";
		yield return new WaitForSeconds(1f);
		this.txtLoading.text = "98%";
		yield return new WaitForSeconds(1f);
		this.txtLoading.text = "100%";
		this.objFakeLoading.SetActive(false);
		yield break;
	}

	private void OnDestroy()
	{
		this.RemoveLobbyCallbacks();
	}

	private void RegisterLobbyCallbacks()
	{
		LobbyMainPanel lobbyMainPanel = this.lobbyMainPanel;
		lobbyMainPanel.OnConnectedToMasterCallBack = (Action)Delegate.Combine(lobbyMainPanel.OnConnectedToMasterCallBack, new Action(this.OnConnectedToMaster));
		LobbyMainPanel lobbyMainPanel2 = this.lobbyMainPanel;
		lobbyMainPanel2.OnCreateOrJoinRandomFailedCallBack = (Action)Delegate.Combine(lobbyMainPanel2.OnCreateOrJoinRandomFailedCallBack, new Action(this.OnCreateOrJoinRandomFailed));
		LobbyMainPanel lobbyMainPanel3 = this.lobbyMainPanel;
		lobbyMainPanel3.OnJoinedRoomCallBack = (Action)Delegate.Combine(lobbyMainPanel3.OnJoinedRoomCallBack, new Action(this.OnJoinedRoom));
		LobbyMainPanel lobbyMainPanel4 = this.lobbyMainPanel;
		lobbyMainPanel4.OnPlayerEnteredRoomCallBack = (Action)Delegate.Combine(lobbyMainPanel4.OnPlayerEnteredRoomCallBack, new Action(this.OnPlayerEnteredRoom));
		LobbyMainPanel lobbyMainPanel5 = this.lobbyMainPanel;
		lobbyMainPanel5.OnPlayerLeftdRoomCallBack = (Action)Delegate.Combine(lobbyMainPanel5.OnPlayerLeftdRoomCallBack, new Action(this.OnPlayerLeftRoom));
		LobbyMainPanel lobbyMainPanel6 = this.lobbyMainPanel;
		lobbyMainPanel6.OnDisconnectedCallBack = (Action)Delegate.Combine(lobbyMainPanel6.OnDisconnectedCallBack, new Action(this.OnDisconnected));
	}

	private void RemoveLobbyCallbacks()
	{
		LobbyMainPanel lobbyMainPanel = this.lobbyMainPanel;
		lobbyMainPanel.OnConnectedToMasterCallBack = (Action)Delegate.Remove(lobbyMainPanel.OnConnectedToMasterCallBack, new Action(this.OnConnectedToMaster));
		LobbyMainPanel lobbyMainPanel2 = this.lobbyMainPanel;
		lobbyMainPanel2.OnCreateOrJoinRandomFailedCallBack = (Action)Delegate.Remove(lobbyMainPanel2.OnCreateOrJoinRandomFailedCallBack, new Action(this.OnCreateOrJoinRandomFailed));
		LobbyMainPanel lobbyMainPanel3 = this.lobbyMainPanel;
		lobbyMainPanel3.OnJoinedRoomCallBack = (Action)Delegate.Remove(lobbyMainPanel3.OnJoinedRoomCallBack, new Action(this.OnJoinedRoom));
		LobbyMainPanel lobbyMainPanel4 = this.lobbyMainPanel;
		lobbyMainPanel4.OnPlayerEnteredRoomCallBack = (Action)Delegate.Remove(lobbyMainPanel4.OnPlayerEnteredRoomCallBack, new Action(this.OnPlayerEnteredRoom));
		LobbyMainPanel lobbyMainPanel5 = this.lobbyMainPanel;
		lobbyMainPanel5.OnPlayerLeftdRoomCallBack = (Action)Delegate.Remove(lobbyMainPanel5.OnPlayerLeftdRoomCallBack, new Action(this.OnPlayerLeftRoom));
		LobbyMainPanel lobbyMainPanel6 = this.lobbyMainPanel;
		lobbyMainPanel6.OnDisconnectedCallBack = (Action)Delegate.Remove(lobbyMainPanel6.OnDisconnectedCallBack, new Action(this.OnDisconnected));
	}

	public override void Show()
	{
		base.Show();
		
		MenuManager.Instance.ChangeTab(0);
	}

	public void Show1vs1()
	{
		try
		{
			this.obj_1VS1.SetActive(true);
			this.obj_1VS3.SetActive(false);
		}
		catch
		{
		}
	}

	public void Show1vs3()
	{
		try
		{
			this.obj_1VS1.SetActive(false);
			this.obj_1VS3.SetActive(true);
		}
		catch
		{
		}
	}

	private void OpenPVP()
	{
		this.LoginAndConnectPhoton();
	}

	private void LoginAndConnectPhoton()
	{
		UnityEngine.Debug.Log("++++++++++++ LoginAndConnectPhoton");
		PopupManager.Instance.ShowMiniLoading();
		this.lobbyMainPanel.LoginAndConnect();
	}

	private void OnConnectedToMaster()
	{
		UnityEngine.Debug.Log("+++++++++++++ OnConnectedToMaster");
		if (!this.buttonClicked)
		{
			return;
		}
		this.buttonClicked = false;
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay != GameMode.ModePlay.PvpMode)
		{
			if (modePlay == GameMode.ModePlay.CoOpMode)
			{
				this.JoinCoOpRoom();
			}
		}
		else
		{
			this.JoinPvpRoom();
		}
	}

	private void JoinPvpRoom()
	{
		if (FormPVP.isSolo)
		{
			this.cardPvp = this.card1vs1Pvp;
		}
		else
		{
			this.cardPvp = this.card1vs3Pvp;
		}
		this.lobbyMainPanel.SetRoomProperty(PvP_RoomProperty.RoomOnlineMode.PVP, (byte)((!FormPVP.isSolo) ? 4 : 2), this.bettingAmount, this.bettingType, ProfileManager.pvpProfile.Score, 5, null, null);
		this.lobbyMainPanel.JoinRandomRoomWithProperty(false, 0, null);
	}

	private void JoinCoOpRoom()
	{
		this.cardPvp = this.card1vs1Pvp;
		this.lobbyMainPanel.SetRoomProperty(PvP_RoomProperty.RoomOnlineMode.COOP, 2, 0, PvP_RoomProperty.RoomBettingType.GOLD, ProfileManager.pvpProfile.Score, 5, null, null);
		this.lobbyMainPanel.JoinRandomRoomWithProperty(false, 0, null);
	}

	private void OnDisconnected()
	{
		try
		{
			UnityEngine.Debug.Log("+++++++++ FormPvp OnDisconnected");
			PopupManager.Instance.CloseMiniLoading();
			if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				PopupManager.Instance.ShowDialog(delegate(bool ok)
				{
				}, 0, PopupManager.Instance.GetText(Localization0.Cannot_connect_to_your_opponent, null), PopupManager.Instance.GetText(Localization0.Warning, null));
			}
			else if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				PopupManager.Instance.ShowDialog(delegate(bool ok)
				{
				}, 0, PopupManager.Instance.GetText(Localization0.Cannot_connect_to_your_teammate, null), PopupManager.Instance.GetText(Localization0.Warning, null));
			}
		}
		catch (Exception ex)
		{
		}
	}

	private void OnCreateOrJoinRandomFailed()
	{
		PopupManager.Instance.CloseMiniLoading();
		PhotonNetwork.Disconnect();
		UnityEngine.Debug.Log("+++++++++ OnCreateOrJoinRandomFailed");
	}

	private void OnJoinedRoom()
	{
		PopupManager.Instance.CloseMiniLoading();
		MenuManager.Instance.ChangeTab(1);
		this.OnTabClosed = new Action(this.LeaveRoom);
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay != GameMode.ModePlay.PvpMode)
		{
			if (modePlay == GameMode.ModePlay.CoOpMode)
			{
				this.OnJoinedRoomCoOp();
			}
		}
		else
		{
			this.OnJoinedRoomPvp();
		}
	}

	private void OnJoinedRoomPvp()
	{
		if (FormPVP.isSolo)
		{
			
			this.Show1vs1();
		}
		else
		{
			
			this.Show1vs3();
		}
		this.UpdateAllPlayerCardPvp();
	}

	private void OnJoinedRoomCoOp()
	{
		this.Show1vs1();
		this.UpdateAllPlayerCardPvp();
	}

	private void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom(true);
	}

	private void OnPlayerEnteredRoom()
	{
		this.UpdateAllPlayerCardPvp();
	}

	private void OnPlayerLeftRoom()
	{
		this.UpdateAllPlayerCardPvp();
	}

	private void UpdateAllPlayerCardPvp()
	{
		this.ResetAllCardPvp();
		this.GetAllActorNumber();
		for (int i = 0; i < this.actorNumbers.Length; i++)
		{
            Photon.Realtime.Player playerByActorNumber = this.GetPlayerByActorNumber(this.actorNumbers[i]);
			if (playerByActorNumber != null)
			{
				this.UpdateCardWithPlayerInfo(this.cardPvp[i], playerByActorNumber);
			}
		}
	}

	private void ResetAllCardPvp()
	{
		for (int i = 0; i < this.cardPvp.Length; i++)
		{
			this.ResetCardPvp(this.cardPvp[i]);
		}
	}

	private void ResetCardPvp(CardPvp card)
	{
		card.txt_Name.text = string.Empty;
		card.img_Main.sprite = this.DefaultAvatar;
		card.img_BG.color = new Color(0.917647064f, 0.4f, 0.435294122f);
		card.obj_Lock.SetActive(true);
		card.img_Rank.sprite = this.arrRankIcon[0];
		card.txt_Rank.text = PopupManager.Instance.GetText(Localization0.Score, null) + ": ...";
		card.txt_Power.text = PopupManager.Instance.GetText(Localization0.Power, null) + ": ...";
		card.txt_Winrate.text = PopupManager.Instance.GetText(Localization0.Winrate, null) + ": ...";
		card.CountryFlag.gameObject.SetActive(false);
	}

	private void UpdateCardWithPlayerInfo(CardPvp card, Photon.Realtime.Player player)
	{
		card.txt_Name.text = player.NickName;
		card.obj_Lock.SetActive(false);
		if (player.IsLocal)
		{
			card.img_BG.color = new Color(0.403921574f, 0.6745098f, 0.9764706f);
		}
		object obj;
		if (player.CustomProperties.TryGetValue("RANK_LEVEL_PROP", out obj))
		{
			card.img_Rank.sprite = this.arrRankIcon[(int)obj];
		}
		if (player.CustomProperties.TryGetValue("PVP_SCORE_PROP", out obj))
		{
			card.txt_Rank.text = PopupManager.Instance.GetText(Localization0.Score, null) + ": " + (int)obj;
		}
		if (player.CustomProperties.TryGetValue("POWER_PROP", out obj))
		{
			card.txt_Power.text = PopupManager.Instance.GetText(Localization0.Power, null) + ": " + (int)obj;
		}
		if (player.CustomProperties.TryGetValue("WIN_RATE_PROP", out obj))
		{
			card.txt_Winrate.text = string.Concat(new object[]
			{
				PopupManager.Instance.GetText(Localization0.Winrate, null),
				": ",
				(int)obj,
				"%"
			});
		}
		string text = string.Empty;
		if (player.CustomProperties.TryGetValue("AVATAR_URL_PROP", out obj))
		{
			text = (string)obj;
		}
		if (text != string.Empty)
		{
			Sprite userPictureSprite = null;
			base.StartCoroutine(MonoSingleton<FBAndPlayfabMgr>.Instance.GetProfilePic(text, delegate(Texture2D tex)
			{
				try
				{
					userPictureSprite = Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), Vector2.zero);
				}
				catch
				{
				}
				if (userPictureSprite != null)
				{
					card.img_Main.sprite = userPictureSprite;
				}
			}));
		}
		int num = -1;
		if (player.CustomProperties.TryGetValue("COUNTRY_CODE_PROP", out obj))
		{
			num = (int)obj;
		}
		if (num != -1)
		{
			Sprite countryFlagSprite = null;
			string url = string.Format("http://www.countryflags.io/{0}/shiny/64.png", (CountryCode)num);
			base.StartCoroutine(MonoSingleton<FBAndPlayfabMgr>.Instance.GetProfilePic(url, delegate(Texture2D tex)
			{
				try
				{
					countryFlagSprite = Sprite.Create(tex, new Rect(0f, 0f, 64f, 64f), Vector2.zero);
				}
				catch
				{
				}
				if (countryFlagSprite != null)
				{
					card.CountryFlag.gameObject.SetActive(true);
					card.CountryFlag.sprite = countryFlagSprite;
				}
			}));
		}
	}

	private void GetAllActorNumber()
	{
		int activePlayerNum = this.GetActivePlayerNum();
		this.actorNumbers = new int[activePlayerNum];
		int constActorNumber = PvP_LocalPlayer.Instance.ConstActorNumber;
		int num = 0;
		int num2 = 0;
		foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
		{
			if (!player.IsInactive)
			{
				object obj;
				if (player.CustomProperties.TryGetValue("CONST_ACTOR_NUMBER", out obj))
				{
					this.actorNumbers[num2] = (int)obj;
				}
				if (player == PhotonNetwork.LocalPlayer)
				{
					num = num2;
				}
				num2++;
			}
		}
		int num3 = this.actorNumbers[0];
		this.actorNumbers[0] = constActorNumber;
		this.actorNumbers[num] = num3;
	}

	private int GetActivePlayerNum()
	{
		int num = 0;
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			if (!PhotonNetwork.PlayerList[i].IsInactive)
			{
				num++;
			}
		}
		return num;
	}

	private Photon.Realtime.Player GetPlayerByActorNumber(int actorNumber)
	{
		foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
		{
			object obj;
			if (player.CustomProperties.TryGetValue("CONST_ACTOR_NUMBER", out obj) && (int)obj == actorNumber)
			{
				return player;
			}
		}
		return null;
	}

	private void Update()
	{
		if (this.img_EffectCoreVS.gameObject.activeSelf && this.img_EffectCoreVS.fillAmount <= 0.5f)
		{
			this.img_EffectCoreVS.fillAmount += Time.deltaTime * 0.7f;
		}
		else if (this.img_EffectCoreVS.fillAmount > 0.5f)
		{
			this.img_EffectVS.gameObject.SetActive(true);
		}
	}

	public void EffectVS()
	{
		this.img_EffectCoreVS.fillAmount = 0f;
		this.img_EffectCoreVS.gameObject.SetActive(true);
	}

	public void BtnGold(CardBase card)
	{
		AudioClick.Instance.OnClick();
		GameMode.Instance.modePlay = GameMode.ModePlay.PvpMode;
		FormPVP.isSolo = card.isOn;
		if (!FormPVP.isSolo)
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.PvP_1vs3_is_under_maintenance_Please_try_it_again_later, null), PopupManager.Instance.GetText(Localization0.Maintenance, null));
			return;
		}
		
		if (ProfileManager.userProfile.Coin < int.Parse(card.txt_Amount.text))
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
				if (ok)
				{
					PopupManager.Instance.ShowInapp(INAPP_TYPE.COINS, delegate(bool closed)
					{
						if (closed)
						{
							MenuManager.Instance.topUI.ShowCoin();
						}
					});
				}
			}, 1, string.Concat(new string[]
			{
				PopupManager.Instance.GetText(Localization0.Not_enough, null),
				" ",
				PopupManager.Instance.GetText(Localization0.Gold, null),
				". \n ",
				PopupManager.Instance.GetText(Localization0.Would_you_like_to_get_more, null),
				"?"
			}), PopupManager.Instance.GetText(Localization0.Warning, null));
			return;
		}
		this.objIfYouWinPvP.SetActive(true);
		this.bettingAmount = int.Parse(card.txt_Amount.text);
		this.bettingType = PvP_RoomProperty.RoomBettingType.GOLD;
		this.buttonClicked = true;
		this.OpenPVP();
	}

	public void BtnGem(CardBase card)
	{
		AudioClick.Instance.OnClick();
		GameMode.Instance.modePlay = GameMode.ModePlay.PvpMode;
		FormPVP.isSolo = card.isOn;
		if (!FormPVP.isSolo)
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.PvP_1vs3_is_under_maintenance_Please_try_it_again_later, null), PopupManager.Instance.GetText(Localization0.Maintenance, null));
			return;
		}
		
		if (ProfileManager.userProfile.Ms < int.Parse(card.txt_Amount.text))
		{
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
				if (ok)
				{
					PopupManager.Instance.ShowInapp(INAPP_TYPE.MS, delegate(bool closed)
					{
						if (closed)
						{
							MenuManager.Instance.topUI.ShowCoin();
						}
					});
				}
			}, 1, string.Concat(new string[]
			{
				PopupManager.Instance.GetText(Localization0.Not_enough, null),
				" ",
				PopupManager.Instance.GetText(Localization0.Gem, null),
				". \n ",
				PopupManager.Instance.GetText(Localization0.Would_you_like_to_get_more, null),
				"?"
			}), PopupManager.Instance.GetText(Localization0.Warning, null));
			return;
		}
		this.objIfYouWinPvP.SetActive(true);
		this.bettingAmount = int.Parse(card.txt_Amount.text);
		this.bettingType = PvP_RoomProperty.RoomBettingType.GEM;
		this.buttonClicked = true;
		this.OpenPVP();
	}

	public void BtnLeaderboard()
	{
		AudioClick.Instance.OnClick();
		
		MenuManager.Instance.popupLeaderboard.Show();
	}

	public void BtnShowHelp(bool is1vs1)
	{
		AudioClick.Instance.OnClick();
		if (is1vs1)
		{
			
			string[] nameSpec = new string[]
			{
				"x2"
			};
			string[] nameSpec2 = new string[]
			{
				" 1vs1"
			};
			PopupManager.Instance.ShowDialog(delegate(bool callback)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.Each_type_of_PvP_requires_a_certain_amount_of_Gold_or_Gem, nameSpec), PopupManager.Instance.GetText(Localization0.PVP, nameSpec2));
		}
		else
		{
			
			string[] nameSpec3 = new string[]
			{
				"x4"
			};
			string[] nameSpec4 = new string[]
			{
				" 1vs3"
			};
			PopupManager.Instance.ShowDialog(delegate(bool callback)
			{
			}, 0, PopupManager.Instance.GetText(Localization0.Each_type_of_PvP_requires_a_certain_amount_of_Gold_or_Gem, nameSpec3), PopupManager.Instance.GetText(Localization0.PVP, nameSpec4));
		}
	}

	public void BtnCoOp()
	{
		GameMode.Instance.modePlay = GameMode.ModePlay.CoOpMode;
		this.buttonClicked = true;
		this.objIfYouWinPvP.SetActive(false);
		this.OpenCoOp();
	}

	public void OpenCoOp()
	{
		this.LoginAndConnectPhoton();
	}

	public LobbyMainPanel lobbyMainPanel;

	public Sprite DefaultAvatar;

	public CardPvp[] card1vs1Pvp;

	public CardPvp[] card1vs3Pvp;

	public CardPvp[] cardPvp;

	public GameObject obj_1VS1;

	public GameObject obj_1VS3;

	public static bool isSolo;

	private int bettingAmount;

	private PvP_RoomProperty.RoomBettingType bettingType;

	public Sprite[] arrRankIcon;

	private bool buttonClicked;

	private int[] actorNumbers;

	public Image img_EffectCoreVS;

	public Image img_EffectVS;

	public GameObject objFakeLoading;

	public Text txtLoading;

	public GameObject objIfYouWinPvP;
}
