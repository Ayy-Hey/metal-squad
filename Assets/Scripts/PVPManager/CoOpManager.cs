using System;
using System.Collections;
using System.Linq;
using System.Text;
using ABI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace PVPManager
{
	public class CoOpManager : MonoBehaviourPunCallbacks
	{
		public static CoOpManager Instance
		{
			get
			{
				if (CoOpManager.instance == null)
				{
					CoOpManager.instance = UnityEngine.Object.FindObjectOfType<CoOpManager>();
				}
				return CoOpManager.instance;
			}
		}

		private void Awake()
		{
			if (!CoOpManager.instance)
			{
				CoOpManager.instance = this;
				return;
			}
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}

		private IEnumerator Start()
		{
			UnityEngine.Debug.Log("+++++++++++++++ CoOpManager Start");
			PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEventReceived;
			Vector3 vtBoder = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f));
			CameraController.Instance.NumericBoundaries.LeftBoundary = -vtBoder.x;
			CameraController.Instance.NumericBoundaries.RightBoundary = vtBoder.x;
			PvP_LocalPlayer.Instance.LoadedScene = true;
			this.startedGame = false;
			this.totalPlayTime = 0f;
			this.strBuilderTime = new StringBuilder();
			this.avatarShowed = false;
			this.GetAllActorNumberAtStartGame();
			this.ShowAllPlayersStatus();
			this.status.text = PopupManager.Instance.GetText(Localization0.Waiting_For_Other_Player, null);
			UnityEngine.Debug.Log("............... Waiting for other player");
			yield return null;
			yield break;
		}

		public IEnumerator InstantiateCoOpMap()
		{
			int mapIdx = 0;
			object CoOpMapIndex;
			if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("COOP_MAP_INDEX", out CoOpMapIndex))
			{
				UnityEngine.Debug.Log("+++++++++++++++ Get CoOp maps ok");
				mapIdx = (int)CoOpMapIndex;
			}
			UnityEngine.Debug.Log("++++++++++++ InstantiateCoOpMap " + mapIdx);
			GameObject coOpMapObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(this.GetMapPath(mapIdx)), Vector3.zero, Quaternion.identity);
			this.currentCoOpMap = coOpMapObject.GetComponent<CoOpMap>();
			this.CreateOnlineRambo();
			yield return null;
			yield break;
		}

		private string GetMapPath(int mapIdx)
		{
			string text = "CoOp_Map/CoOp_Map_" + mapIdx;
			UnityEngine.Debug.Log("Map Path: " + text);
			return text;
		}

		private void OnDestroy()
		{
			UnityEngine.Debug.Log("+++++++++++++++ CoOp Manager OnDestroy");
			PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEventReceived;
		}

		private void Update()
		{
			if (PvP_LocalPlayer.Instance.LoadedScene && !this.isInit && this.isLeftRoomBeforeLoadScene)
			{
				this.QuitRoom(PvP_LocalPlayer.Instance.IsWinner);
				this.isLeftRoomBeforeLoadScene = false;
				return;
			}
			if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
			{
				return;
			}
			if (PvP_LocalPlayer.Instance.Alive && !PvP_LocalPlayer.Instance.IsEndGame && PhotonNetwork.LocalPlayer.ActorNumber != -1)
			{
				GameManager.Instance.UpdateTime(Time.deltaTime);
				this.totalPlayTime += Time.deltaTime;
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			this.HandlePauseForOnlineMode(pauseStatus);
			this.CheckLeftRoomBeforeLoadedScene(false);
		}

		private bool CheckLeftRoomBeforeLoadedScene(bool isWinner)
		{
			if (!this.isInit || !PvP_LocalPlayer.Instance.LoadedScene)
			{
				this.isLeftRoomBeforeLoadScene = true;
				UnityEngine.Debug.Log("++++++++ I left room before load scene");
				if (!isWinner)
				{
					this.status.text = PopupManager.Instance.GetText(Localization0.You_Entered_Game_Too_Late, null);
				}
				this.SetPlayerIsEndGame(PhotonNetwork.LocalPlayer, isWinner);
				return true;
			}
			return false;
		}

		public void HandlePauseForOnlineMode(bool pauseStatus)
		{
			if (pauseStatus)
			{
				if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom && !this.isInit)
				{
					UnityEngine.Debug.Log("++++++++++++ OnApplicationPause before CoOpManager Init");
					PhotonNetwork.Disconnect();
					PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
				}
				else if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom && this.isInit && PhotonNetwork.IsMasterClient)
				{
					UnityEngine.Debug.Log("++++++++++++ Master OnApplicationPause after CoOpManager Init");
					this.ChangeMasterClientifAvailble();
					PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
				}
				else
				{
					UnityEngine.Debug.Log("++++++++++++ CoOpManager OnApplicationPause but do nothing");
				}
			}
		}

		public void ChangeMasterClientifAvailble()
		{
			if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
			{
				return;
			}
			UnityEngine.Debug.Log("+++++++++++++ Switch master OnApplicationPause");
			PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
		}

		public override void OnLeftRoom()
		{
			UnityEngine.Debug.Log("+++++++++++++++ I left room");
			PhotonNetwork.Disconnect();
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			UnityEngine.Debug.Log("+++++++++++++++ Other player left room");
			object obj;
			otherPlayer.CustomProperties.TryGetValue("CONST_ACTOR_NUMBER", out obj);
			this.RemovePlayerInListRambo((int)obj);
			this.CheckStartGame();
			this.ShowAllPlayersStatus();
			if (this.isInit && this.CheckAllPlayerLoadedLevel())
			{
				this.CheckEndOfGame();
			}
		}

		public void RemovePlayerInListRambo(int actorNumber)
		{
			UnityEngine.Debug.Log("+++++++++ RemovePlayerInListRambo");
			GameManager.Instance.ListRambo.Remove(GameManager.Instance.ListRambo.Find((BaseCharactor rambo) => ((PlayerMain)rambo).actorNumber == actorNumber));
		}

		public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
		{
			UnityEngine.Debug.Log("+++++++++++++++ Other player rejoin room");
			if (!this.startedGame)
			{
				base.StartCoroutine(this.PoolingCheckStartGame(20));
			}
		}

		public IEnumerator PoolingCheckStartGame(int countdownValue)
		{
			int currCountdownValue = countdownValue;
			this.wait = new WaitForSeconds(this.poolingCheckInternetConnectionTime);
			while ((float)currCountdownValue > 0f)
			{
				UnityEngine.Debug.Log("PoolingCheckStartGame " + currCountdownValue);
				yield return this.wait;
				if (this.CheckStartGame())
				{
					UnityEngine.Debug.Log("PoolingCheckStartGame Ok");
					yield break;
				}
				currCountdownValue--;
			}
			UnityEngine.Debug.Log("PoolingCheckStartGame Fail");
			this.ShowNotStartGame();
			yield break;
		}

		public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnMasterClientSwitched");
		}

		public override void OnJoinRoomFailed(short returnCode, string message)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"+++++++++++++++ OnJoinRoomFailed: returnCode ",
				returnCode,
				" ",
				message
			}));
			base.StartCoroutine(this.ShowStatus(PopupManager.Instance.GetText(Localization0.Rejoin_Room_Failed, null)));
			this.EndOfGame(false);
		}

		public override void OnJoinedRoom()
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnJoinedRoom");
			PvP_LocalPlayer.Instance.LoadedScene = true;
			PvP_LocalPlayer.Instance.IsInit = true;
			this.StopTryToRejoin();
			base.StartCoroutine(this.ShowStatus(PopupManager.Instance.GetText(Localization0.Rejoin_Room_Success, null)));
		}

		private void StopTryToRejoin()
		{
			CountdownTimer.Instance.CancelTimer();
			base.StopCoroutine("PoolingReconnectAndRejoin");
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnDisconnected: " + cause);
			if (cause == DisconnectCause.ClientTimeout && !PvP_LocalPlayer.Instance.IsEndGame && GameManager.Instance.player.HPCurrent > 0f)
			{
				if (!PhotonSingleton<PvP_RoomProperty>.Instance.IsFakePlayerMode)
				{
					this.status.text = PopupManager.Instance.GetText(Localization0.You_Lost_Internet, null);
					UnityEngine.Debug.Log("+++++++++++++++ Start Disconnect Timer");
					CountdownTimer.Instance.StartTimer(this.waitTimeAfterDisconnect, new Action(this.OnTimeoutDisconnect));
					base.StartCoroutine(this.PoolingReconnectAndRejoin((int)this.waitTimeAfterDisconnect - 2));
				}
				else
				{
					UnityEngine.Debug.Log("+++++++++++++++ Thua luon khi lost connection trong fake player mode");
					this.EndOfGame(false);
				}
			}
			else if (cause != DisconnectCause.DisconnectByClientLogic)
			{
				UnityEngine.Debug.Log("+++++++++++++++ EndOfGame OnDisconnected: " + cause);
				this.status.text = PopupManager.Instance.GetText(Localization0.You_Are_Disconnect, null);
				this.EndOfGame(false);
			}
		}

		public void OnTimeoutDisconnect()
		{
			base.StartCoroutine(this.ShowStatus(PopupManager.Instance.GetText(Localization0.You_Are_Disconnect, null)));
			UnityEngine.Debug.Log("+++++++++++++++ EndOfGame OnTimeoutDisconnect");
			this.EndOfGame(false);
		}

		public IEnumerator PoolingReconnectAndRejoin(int countdownValue)
		{
			int currCountdownValue = countdownValue;
			this.wait = new WaitForSeconds(this.poolingCheckInternetConnectionTime);
			while ((float)currCountdownValue > 0f)
			{
				yield return this.wait;
				this.ReconnectAndRejoin();
				currCountdownValue--;
			}
			yield break;
		}

		private void ReconnectAndRejoin()
		{
			this.status.text = PopupManager.Instance.GetText(Localization0.Try_To_Reconnect_And_Rejoin, null);
			PhotonNetwork.AuthValues = new AuthenticationValues();
			PhotonNetwork.AuthValues.UserId = PhotonNetwork.LocalPlayer.UserId;
			PhotonNetwork.ReconnectAndRejoin();
		}

		private IEnumerator ShowStatus(string content)
		{
			this.status.text = content;
			yield return new WaitForSeconds(2f);
			this.status.text = string.Empty;
			yield break;
		}

		public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			this.ShowAllPlayersStatus();
			if (changedProps.ContainsKey("LOADED_SCENE_PROP") || changedProps.ContainsKey("IS_INIT_PROP"))
			{
				this.CheckStartGame();
			}
			this.CheckLocalPlayerHP(targetPlayer, changedProps);
			if (!targetPlayer.IsLocal)
			{
				this.InitRemoteHUD();
				this.UpdateRemoteHUD(targetPlayer, changedProps);
			}
		}

		private void CheckLocalPlayerHP(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			if (!changedProps.ContainsKey("HP_PROP"))
			{
				return;
			}
			if (this.CheckAllPlayerLoadedLevel())
			{
				this.CheckEndOfGame();
			}
		}

		private void ShowAllPlayersStatus()
		{
			if (this.actorNumbers == null)
			{
				return;
			}
			if (this.actorNumbers.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.actorNumbers.Length; i++)
			{
                Photon.Realtime.Player playerByActorNumber = this.GetPlayerByActorNumber(this.actorNumbers[i]);
				if (playerByActorNumber != null)
				{
				}
			}
			this.avatarShowed = true;
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

		private void GetAllActorNumberAtStartGame()
		{
			if (PhotonNetwork.PlayerList.Length == 0)
			{
				return;
			}
			this.actorNumbers = new int[PhotonNetwork.PlayerList.Length];
			int constActorNumber = PvP_LocalPlayer.Instance.ConstActorNumber;
			int num = 0;
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
			{
				object obj;
				if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("CONST_ACTOR_NUMBER", out obj))
				{
					this.actorNumbers[i] = (int)obj;
				}
				if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
				{
					num = i;
				}
			}
			int num2 = this.actorNumbers[0];
			this.actorNumbers[0] = constActorNumber;
			this.actorNumbers[num] = num2;
		}

		private void UpdatePlayerStatus(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			if (!card.gameObject.activeSelf)
			{
				card.gameObject.SetActive(true);
			}
			this.UpdateAvatar(player, card);
			if (this.UpdateEndGame(player, card))
			{
				return;
			}
			this.UpdateBackground(player, card);
			this.UpdateConnectionStatus(player, card);
			this.UpdateTimeTurn(player, card);
			this.UpdateTurn(player, card);
		}

		private void UpdateAvatar(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			if (!this.avatarShowed)
			{
				string text = string.Empty;
				object obj;
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
							card.Avatar.sprite = userPictureSprite;
						}
					}));
				}
			}
		}

		private bool UpdateEndGame(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			object obj;
			object obj2;
			if (player.CustomProperties.TryGetValue("IS_END_GAME_PROP", out obj) && (bool)obj && player.CustomProperties.TryGetValue("IS_WINNER_PROP", out obj2))
			{
				if (!(bool)obj2)
				{
					this.ShowLoseGameCard(card);
				}
				return true;
			}
			return false;
		}

		private void ShowLoseGameCard(PvpPlayerStatusCard card)
		{
			card.Avatar.color = new Color(0.3137255f, 0.3137255f, 0.3137255f);
			card.Connected.gameObject.SetActive(false);
			card.Disconnected.gameObject.SetActive(false);
		}

		private void UpdateBackground(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			if (player.IsLocal)
			{
				card.GreenBg.gameObject.SetActive(true);
				card.RedBg.gameObject.SetActive(false);
			}
			else
			{
				card.GreenBg.gameObject.SetActive(false);
				card.RedBg.gameObject.SetActive(true);
			}
		}

		private void UpdateConnectionStatus(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			if (player.IsInactive)
			{
				card.Connected.gameObject.SetActive(false);
				card.Disconnected.gameObject.SetActive(true);
			}
			else
			{
				card.Connected.gameObject.SetActive(true);
				card.Disconnected.gameObject.SetActive(false);
			}
		}

		private void UpdateTimeTurn(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			object obj;
			if (player.CustomProperties.TryGetValue("ENEMY_TURN_TIME_PROP", out obj))
			{
				int num = (int)obj;
				int num2 = num / 60;
				int num3 = num - num2 * 60;
				this.strBuilderTime.Length = 0;
				if (num2 < 10)
				{
					this.strBuilderTime.Append("0");
				}
				this.strBuilderTime.Append(num2);
				this.strBuilderTime.Append(":");
				if (num3 < 10)
				{
					this.strBuilderTime.Append("0");
				}
				this.strBuilderTime.Append(num3);
				card.Time.text = this.strBuilderTime.ToString();
			}
		}

		private void UpdateTurn(Photon.Realtime.Player player, PvpPlayerStatusCard card)
		{
			object obj;
			if (player.CustomProperties.TryGetValue("ENEMY_TURN_PROP", out obj))
			{
				card.Wave.text = string.Empty + ((int)obj + 1);
			}
		}

		private bool CheckStartGame()
		{
			bool flag = false;
			if (!this.startedGame)
			{
				flag = this.CheckAllPlayerLoadedLevel();
				if (flag)
				{
					NetworkCountdownTimer.Instance.StartNetworkTimer(this.waitTimeStartGame, new Action(this.OnStartGameCountdownTimerIsExpired), true);
				}
			}
			return flag;
		}

		private void CreateOnlineRambo()
		{
			UnityEngine.Debug.Log("+++++++++ CreateOnlineRambo");
			int idchar = ProfileManager.settingProfile.IDChar;
			if (idchar != 0)
			{
				if (idchar != 1)
				{
					if (idchar == 2)
					{
						GameManager.Instance.player = PhotonNetwork.Instantiate("GameObject/Player/PlayerBoy2_Online", new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null).GetComponent<PlayerMain>();
					}
				}
				else
				{
					GameManager.Instance.player = PhotonNetwork.Instantiate("GameObject/Player/PlayerGirl_Online", new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null).GetComponent<PlayerMain>();
				}
			}
			else
			{
				GameManager.Instance.player = PhotonNetwork.Instantiate("GameObject/Player/PlayerBoy1_Online", new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null).GetComponent<PlayerMain>();
			}
		}

		private void OnStartGameCountdownTimerIsExpired()
		{
			if (PhotonNetwork.LocalPlayer.IsMasterClient)
			{
				this.currentCoOpMap.CreatOnlineBoss();
			}
			GameManager.Instance.ResetTime();
			this.GetAllActorNumberAtStartGame();
			PhotonNetwork.BackgroundTimeout = 10f;
			this.isInit = true;
			this.startedGame = true;
			ControlManager.Instance.OnShowControlStartGame();
			base.StartCoroutine(this.ShowStatus(PopupManager.Instance.GetText(Localization0.Start, null)));
			UnityEngine.Debug.Log("............... Start Game!!!");
		}

		private void InitRemoteHUD()
		{
			if (this.remoteHUD.IsInit)
			{
				return;
			}
            Photon.Realtime.Player player = PhotonNetwork.PlayerList.ToList<Photon.Realtime.Player>().Find((Photon.Realtime.Player x) => !x.IsLocal);
			object obj;
			if (!player.CustomProperties.TryGetValue("MAX_HP_PROP", out obj))
			{
				return;
			}
			object obj2;
			if (!player.CustomProperties.TryGetValue("CHARACTOR_ID", out obj2))
			{
				return;
			}
			this.remoteHUD.Init((int)obj2, (float)obj);
		}

		private void UpdateRemoteHUD(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			if (targetPlayer.IsLocal)
			{
				return;
			}
			if (!changedProps.ContainsKey("HP_PROP"))
			{
				return;
			}
			if (!this.remoteHUD.IsInit)
			{
				return;
			}
			object obj;
			targetPlayer.CustomProperties.TryGetValue("HP_PROP", out obj);
			this.remoteHUD.ChangeHPSlide((float)obj);
		}

		private int GetPlayingPlayerCount()
		{
			int num = 0;
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				object obj;
				if (player.CustomProperties.TryGetValue("IS_END_GAME_PROP", out obj) && !(bool)obj)
				{
					num++;
				}
			}
			return num;
		}

		private void CheckEndOfGame()
		{
			object lockCheckEndGame = this.LockCheckEndGame;
			lock (lockCheckEndGame)
			{
				if (!PvP_LocalPlayer.Instance.IsEndGame)
				{
					if (this.GetAlivePlayerNum() == 0)
					{
						this.EndOfGame(false);
					}
				}
			}
		}

		private int GetAlivePlayerNum()
		{
			int num = 0;
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				object obj;
				if (player.CustomProperties.TryGetValue("HP_PROP", out obj) && (float)obj > 0f)
				{
					num++;
				}
			}
			return num;
		}

		public void EndOfGame(bool isWinner)
		{
			object lockEndGame = this.LockEndGame;
			lock (lockEndGame)
			{
				if (!PvP_LocalPlayer.Instance.IsEndGame)
				{
					if (!this.CheckLeftRoomBeforeLoadedScene(isWinner))
					{
						this.StopTryToRejoin();
						UnityEngine.Debug.Log("++++++++ EndOfGame: " + isWinner);
						if (PhotonNetwork.LocalPlayer.IsMasterClient)
						{
							foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
							{
								this.SetPlayerIsEndGame(player, isWinner);
							}
							UnityEngine.Debug.Log("++++++++ Send end game event to other player");
							CoOpManager.RaiseEvent(1, null, ReceiverGroup.All);
						}
						else
						{
							UnityEngine.Debug.Log("+++++++++++ Client End Game");
							this.SetPlayerIsEndGame(PhotonNetwork.LocalPlayer, isWinner);
							this.QuitRoom(isWinner);
						}
					}
				}
			}
		}

		private void SetPlayerIsEndGame(Photon.Realtime.Player player, bool isWinner)
		{
			ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
			{
				{
					"IS_WINNER_PROP",
					isWinner
				},
				{
					"IS_END_GAME_PROP",
					true
				}
			};
			player.SetCustomProperties(propertiesToSet, null, null);
		}

		private void OnPhotonEventReceived(EventData photonEvent)
		{
			byte code = photonEvent.Code;
			if (code == 1)
			{
				UnityEngine.Debug.Log("++++++++++++++ receive event END_GAME");
				this.QuitRoom(PvP_LocalPlayer.Instance.IsWinner);
			}
		}

		private void QuitRoom(bool isWinner)
		{
			if (this.isEndGame)
			{
				return;
			}
			this.isEndGame = true;
			UnityEngine.Debug.Log("QuitRoom and show mission");
			GameManager.Instance.StateManager.EState = EGamePlay.PAUSE;
			PhotonNetwork.LeaveRoom(true);
			if (isWinner && !this.isInit)
			{
				this.ShowNotStartGame();
				return;
			}
			if (isWinner)
			{
				GameManager.Instance.StateManager.popupEndMission.state = EndMission.CoOpWin;
			}
			else
			{
				GameManager.Instance.StateManager.popupEndMission.state = EndMission.CoOpLost;
			}
			GameManager.Instance.StateManager.popupEndMission.OnShow();
			GameManager.Instance.StateManager.popupEndMission.objGroupButton.SetActive(true);
		}

		private void ShowNotStartGame()
		{
			int bettingAmount = PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			PvP_RoomProperty.RoomBettingType bettingType = PhotonSingleton<PvP_RoomProperty>.Instance.BettingType;
			if (bettingType != PvP_RoomProperty.RoomBettingType.GOLD)
			{
				if (bettingType == PvP_RoomProperty.RoomBettingType.GEM)
				{
					PopupManager.Instance.SaveReward(Item.Gem, bettingAmount, "Coop_RoomBetting_Type:GEM", null);
				}
			}
			else
			{
				PopupManager.Instance.SaveReward(Item.Gold, bettingAmount, "Coop_RoomBetting_Type:GOLD", null);
			}
			GameMode.Instance.Style = GameMode.GameStyle.SinglPlayer;
			PopupManager.Instance.ShowDialog(delegate(bool ok)
			{
				FormLoadout.typeForm = FormLoadout.Type.PVP;
				GameMode.Instance.Style = GameMode.GameStyle.MultiPlayer;
				GameMode.Instance.modePlay = GameMode.ModePlay.PvpMode;
				GameManager.Instance.hudManager.OnLoadScene("UIPvp", true);
			}, 0, PopupManager.Instance.GetText(Localization0.Your_opponent_has_left_the_room, null), PopupManager.Instance.GetText(Localization0.Warning, null));
		}

		public static void RaiseEvent(byte eventCode, object content, ReceiverGroup group)
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				Receivers = group
			};
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};
			PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, sendOptions);
		}

		private bool CheckAllPlayerLoadedLevel()
		{
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				object obj;
				if (!player.CustomProperties.TryGetValue("LOADED_SCENE_PROP", out obj))
				{
					return false;
				}
				if (!(bool)obj)
				{
					return false;
				}
				object obj2;
				if (!player.CustomProperties.TryGetValue("IS_INIT_PROP", out obj2))
				{
					return false;
				}
				if (!(bool)obj2)
				{
					return false;
				}
			}
			return true;
		}

		public void XinThua(GameObject obj)
		{
			GameManager.Instance.player.isBatTu = false;
			GameManager.Instance.player.AddHealthPoint(-9999f, EWeapon.NONE);
			obj.SetActive(false);
		}

		public void BatTu(Text txt)
		{
			GameManager.Instance.player.isBatTu = !GameManager.Instance.player.isBatTu;
			txt.text = ((!GameManager.Instance.player.isBatTu) ? "Bat Tu" : "Ko Bat Tu");
		}

		private static CoOpManager instance;

		[HideInInspector]
		public bool isInit;

		public const string CoOp_Map_Prefix = "CoOp_Map/CoOp_Map_";

		public CoOpMap currentCoOpMap;

		public Text status;

		private float waitTimeStartGame = 3f;

		private float waitTimeAfterDisconnect = 12f;

		private float poolingCheckInternetConnectionTime = 1f;

		public PvpPlayerStatusCard[] playerCards;

		private int[] actorNumbers;

		private bool avatarShowed;

		private bool isLeftRoomBeforeLoadScene;

		private readonly object LockCheckEndGame = new object();

		private readonly object LockEndGame = new object();

		private bool startedGame;

		public float totalPlayTime;

		private StringBuilder strBuilderTime;

		private WaitForSeconds wait;

		private bool isEndGame;

		public CoOpPlayerHUD remoteHUD;
	}
}
