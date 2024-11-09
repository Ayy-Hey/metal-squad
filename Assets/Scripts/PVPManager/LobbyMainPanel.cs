using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PVPManager
{
	public class LobbyMainPanel : MonoBehaviourPunCallbacks
	{
		public bool EnableCreateFakePlayer { get; set; }

		private void Update()
		{
			if (this.enableTimer)
			{
				this.remainTime -= Time.deltaTime;
				if (this.remainTime < 0f)
				{
					this.OnTimerEnd();
				}
			}
		}

		public void LoginAndConnect()
		{
			if (PhotonNetwork.InRoom)
			{
				UnityEngine.Debug.Log("++++++++++ Player still in room and can't login and connect");
				return;
			}
			UnityEngine.Debug.Log("++++++++++ LoginAndConnect");
			MenuManager.Instance.topUI.obj_BtnBack.SetActive(true);
			if (!PhotonNetwork.IsConnectedAndReady)
			{
				UnityEngine.Debug.Log("+++++++++++++++ First time login");
				PhotonSingleton<PvP_FakePlayerManager>.Instance.RemoveAllFakePlayer();
				PhotonNetwork.AutomaticallySyncScene = false;
				PhotonNetwork.BackgroundTimeout = 0f;
				PhotonNetwork.ConnectUsingSettings();
				this.currentJoinRoomTryTime = 0;
			}
			else
			{
				UnityEngine.Debug.Log("+++++++++++++++ Already login");
				this.OnConnectedToMaster();
			}
		}

		private void Start()
		{
			UnityEngine.Debug.Log("+++++++++++ LobbyMainPanel Start");
			PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEventReceived;
		}

		private void OnDestroy()
		{
			UnityEngine.Debug.Log("+++++++++++ LobbyMainPanel OnDestroy");
			PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEventReceived;
		}

		public override void OnConnectedToMaster()
		{
			UnityEngine.Debug.Log("+++++++++++++ OnConnectedToMaster");
			this.currentJoinRoomTryTime = 0;
			if (this.OnConnectedToMasterCallBack != null)
			{
				this.OnConnectedToMasterCallBack();
			}
		}

		public void SetRoomProperty(PvP_RoomProperty.RoomOnlineMode OnlineMode, byte maxPlayer, int bettingAmount, PvP_RoomProperty.RoomBettingType bettingType, int pvpScore, int deltaScore, string lobbyName = null, string roomName = null)
		{
			PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer = maxPlayer;
			PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount = bettingAmount;
			PhotonSingleton<PvP_RoomProperty>.Instance.BettingType = bettingType;
			PhotonSingleton<PvP_RoomProperty>.Instance.PvpScore = pvpScore;
			PhotonSingleton<PvP_RoomProperty>.Instance.DeltaScore = deltaScore;
			PhotonSingleton<PvP_RoomProperty>.Instance.LobbyName = lobbyName;
			PhotonSingleton<PvP_RoomProperty>.Instance.RoomName = roomName;
			PhotonSingleton<PvP_RoomProperty>.Instance.OnlineMode = OnlineMode;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"OnlineMode ",
				OnlineMode,
				" roomName: ",
				roomName,
				" maxPlayers: ",
				maxPlayer,
				" bettingAmount: ",
				bettingAmount,
				"bettingType: ",
				bettingType,
				" pvpScore: ",
				pvpScore
			}));
		}

		public void CreatRoomWithProperty(string lobbyName = null, string roomName = null)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer;
			if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				roomOptions.PlayerTtl = this.playerTimeToLive;
			}
			else if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				roomOptions.PlayerTtl = 0;
			}
			roomOptions.EmptyRoomTtl = this.emptyRoomTimeToLive;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("C0", PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer);
			hashtable.Add("C1", PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount);
			hashtable.Add("C2", (int)PhotonSingleton<PvP_RoomProperty>.Instance.BettingType);
			hashtable.Add("C4", (int)PhotonSingleton<PvP_RoomProperty>.Instance.OnlineMode);
			hashtable.Add("C3", PhotonSingleton<PvP_RoomProperty>.Instance.PvpScore);
			if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				int[] array = new int[5];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = UnityEngine.Random.Range(0, 100) % 2;
				}
				hashtable.Add("PVP_MAPS", array);
			}
			else if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				int num = UnityEngine.Random.Range(0, 100) % 3;
				hashtable.Add("COOP_MAP_INDEX", num);
			}
			roomOptions.CustomRoomProperties = hashtable;
			roomOptions.PublishUserId = true;
			roomOptions.CustomRoomPropertiesForLobby = new string[]
			{
				"C0",
				"C1",
				"C2",
				"C3",
				"C4"
			};
			TypedLobby typedLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
			if (!PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby, null) && this.OnCreateOrJoinRandomFailedCallBack != null)
			{
				this.OnCreateOrJoinRandomFailedCallBack();
			}
		}

		public override void OnCreateRoomFailed(short returnCode, string message)
		{
			UnityEngine.Debug.Log("+++++++++++++ OnCreateRoomFailed");
			if (this.OnCreateOrJoinRandomFailedCallBack != null)
			{
				this.OnCreateOrJoinRandomFailedCallBack();
			}
		}

		public void JoinRandomRoomWithProperty(bool ignorePvpScore = false, int increaseRange = 0, string lobbyName = null)
		{
			byte maxPlayer = PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer;
			int bettingAmount = PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			PvP_RoomProperty.RoomBettingType bettingType = PhotonSingleton<PvP_RoomProperty>.Instance.BettingType;
			int pvpScore = PhotonSingleton<PvP_RoomProperty>.Instance.PvpScore;
			int deltaScore = PhotonSingleton<PvP_RoomProperty>.Instance.DeltaScore;
			string text;
			if (!ignorePvpScore)
			{
				int a = Mathf.Max(pvpScore + deltaScore * (ProfileManager.pvpProfile.WinStreak - 1), 0);
				int num = Mathf.Max(a, pvpScore) + increaseRange;
				int num2 = Mathf.Min(a, pvpScore) - increaseRange;
				text = string.Concat(new object[]
				{
					"C0 == ",
					maxPlayer,
					" AND C1 == ",
					bettingAmount,
					" AND C2 == ",
					(int)bettingType,
					" AND C3 >= ",
					num2,
					" AND C3 <= ",
					num
				});
			}
			else
			{
				text = string.Concat(new object[]
				{
					"C0 == ",
					maxPlayer,
					" AND C1 == ",
					bettingAmount,
					" AND C2 == ",
					(int)bettingType
				});
			}
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("C0", PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer);
			hashtable.Add("C1", PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount);
			hashtable.Add("C2", (int)PhotonSingleton<PvP_RoomProperty>.Instance.BettingType);
			hashtable.Add("C4", (int)PhotonSingleton<PvP_RoomProperty>.Instance.OnlineMode);
			UnityEngine.Debug.Log(text);
			TypedLobby typedLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
			if (!PhotonNetwork.JoinRandomRoom(hashtable, maxPlayer, MatchmakingMode.FillRoom, typedLobby, text, null) && this.OnCreateOrJoinRandomFailedCallBack != null)
			{
				this.OnCreateOrJoinRandomFailedCallBack();
			}
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnJoinRandomFailed " + message);
			if (this.currentJoinRoomTryTime < this.joinRoomTryTime)
			{
				this.currentJoinRoomTryTime++;
				UnityEngine.Debug.Log("+++++++++++++++ Try to rejoin room " + this.currentJoinRoomTryTime);
				this.JoinRandomRoomWithProperty(false, this.currentJoinRoomTryTime * 5, null);
			}
			else if (this.currentJoinRoomTryTime == this.joinRoomTryTime)
			{
				this.currentJoinRoomTryTime++;
				UnityEngine.Debug.Log("+++++++++++++++ Try to rejoin room ignore pvp score");
				this.JoinRandomRoomWithProperty(true, 0, null);
			}
			else
			{
				UnityEngine.Debug.Log("+++++++++++++++ Try to create new room");
				this.CreatRoomWithProperty(null, null);
			}
		}

		public override void OnJoinedRoom()
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnJoinedRoom");
			PvP_LocalPlayer.Instance.Init();
			PhotonSingleton<PvP_RoomProperty>.Instance.IsFakePlayerMode = false;
			PvP_LocalPlayer.Instance.ConstActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
			if (this.OnJoinedRoomCallBack != null)
			{
				this.OnJoinedRoomCallBack();
			}
			this.CheckStartGame();
			this.StartTimer();
		}

		public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnPlayerEnteredRoom");
			object obj;
			if (newPlayer.CustomProperties.TryGetValue("IS_FAKE_PLAYER", out obj) && (bool)obj)
			{
				UnityEngine.Debug.Log("++++++++++++ Fake player entered room");
				this.CreateFakePlayerGameObject(newPlayer);
			}
			ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
			{
				{
					"CONST_ACTOR_NUMBER",
					newPlayer.ActorNumber
				}
			};
			newPlayer.SetCustomProperties(propertiesToSet, null, null);
			if (this.OnPlayerEnteredRoomCallBack != null)
			{
				this.OnPlayerEnteredRoomCallBack();
			}
			this.CheckStartGame();
			this.CancelTimer();
		}

		private void CreateFakePlayerGameObject(Photon.Realtime.Player newPlayer)
		{
			GameObject gameObject = new GameObject();
			PvP_FakePlayer pvP_FakePlayer = gameObject.AddComponent<PvP_FakePlayer>();
			pvP_FakePlayer.player = newPlayer;
			pvP_FakePlayer.Init();
			gameObject.name = typeof(PvP_FakePlayer).ToString();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			PhotonSingleton<PvP_FakePlayerManager>.Instance.AddFakePlayer(pvP_FakePlayer);
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			this.HandlePauseForOnlineMode(pauseStatus);
		}

		public void HandlePauseForOnlineMode(bool pauseStatus)
		{
			if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
			{
				return;
			}
			if (pauseStatus && PhotonNetwork.IsConnectedAndReady)
			{
				UnityEngine.Debug.Log("++++++++++ LobbyMainPanel disconnect photon when OnApplicationPause");
				PhotonNetwork.Disconnect();
				PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
				if (this.isStartButtonClicked)
				{
					UnityEngine.Debug.Log("++++++++++ LobbyMainPanel disconnect photon when OnApplicationPause but loading other scene");
					UnityEngine.Debug.Log("++++++++++ Change to Menu scene");
					SceneManager.LoadScene("Menu");
					this.isStartButtonClicked = false;
				}
			}
		}

		public override void OnLeftRoom()
		{
			try
			{
				UnityEngine.Debug.Log("+++++++++++++++ OnLeftRoom");
				this.CancelTimer();
				PvP_LocalPlayer.Instance.Init();
				PhotonSingleton<PvP_FakePlayerManager>.Instance.RemoveAllFakePlayer();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("+++++++++++++++ OnLeftRoom Exception " + ex.Message);
			}
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnPlayerLeftRoom");
			if (this.OnPlayerLeftdRoomCallBack != null)
			{
				this.OnPlayerLeftdRoomCallBack();
			}
			this.StartTimer();
			MenuManager.Instance.topUI.obj_BtnBack.SetActive(true);
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			UnityEngine.Debug.Log("+++++++++++++++ Lobby OnDisconnected: " + cause);
			MenuManager.Instance.ChangeTab(0);
			if (this.OnDisconnectedCallBack != null)
			{
				this.OnDisconnectedCallBack();
			}
			MenuManager.Instance.topUI.obj_BtnBack.SetActive(true);
		}

		public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnMasterClientSwitched");
		}

		public override void OnJoinRoomFailed(short returnCode, string message)
		{
			UnityEngine.Debug.Log("+++++++++++++++ OnJoinRoomFailed");
			MenuManager.Instance.BackForm();
		}

		public void OnLeaveGameButtonClicked()
		{
			PhotonNetwork.LeaveRoom(true);
		}

		private void CheckStartGame()
		{
			if (this.CheckPlayersReady())
			{
				MenuManager.Instance.topUI.obj_BtnBack.SetActive(false);
				if (PhotonNetwork.LocalPlayer.IsMasterClient)
				{
					base.StartCoroutine("SendStartEvent");
				}
			}
		}

		private IEnumerator SendStartEvent()
		{
			yield return new WaitForSeconds(0.5f);
			if (this.formPvP != null && PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer == 2)
			{
				this.formPvP.EffectVS();
			}
			yield return new WaitForSeconds(2.5f);
			PVPManager.RaiseEvent(0, null, ReceiverGroup.All);
			yield break;
		}

		private void OnPhotonEventReceived(EventData photonEvent)
		{
			if (photonEvent.Code == 0)
			{
				UnityEngine.Debug.Log("++++++++++++++ receive event START_BUTTON_CLICKED");
				GameMode.Instance.Style = GameMode.GameStyle.MultiPlayer;
				MenuManager.Instance.objLoading.SetActive(true);
				PhotonNetwork.CurrentRoom.IsOpen = false;
				PhotonNetwork.CurrentRoom.IsVisible = false;
				GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
				if (modePlay != GameMode.ModePlay.PvpMode)
				{
					if (modePlay != GameMode.ModePlay.CoOpMode)
					{
						MenuManager.Instance.async = PhotonNetwork.LoadLevel("Menu");
					}
					else
					{
						MenuManager.Instance.async = PhotonNetwork.LoadLevel("CoOpPlay");
					}
				}
				else
				{
					MenuManager.Instance.async = PhotonNetwork.LoadLevel("PvpPlay");
				}
				this.isStartButtonClicked = true;
			}
		}

		private bool CheckPlayersReady()
		{
			return PhotonNetwork.CurrentRoom.PlayerCount == PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer;
		}

		public void LocalPlayerPropertiesUpdated()
		{
		}

		public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
		}

		public void OnCreateFakePlayer()
		{
			if (PhotonNetwork.PlayerList.Length == 1 && this.EnableCreateFakePlayer)
			{
				UnityEngine.Debug.Log("+++++++++ Create fake player");
				PhotonNetwork.CurrentRoom.IsOpen = false;
				PhotonNetwork.CurrentRoom.IsVisible = false;
				PhotonNetwork.CurrentRoom.IsOffline = true;
				PhotonSingleton<PvP_RoomProperty>.Instance.IsFakePlayerMode = true;
				PhotonSingleton<PvP_FakePlayerManager>.Instance.Init();
				base.StartCoroutine(PhotonSingleton<PvP_FakePlayerManager>.Instance.StartCreateFakePlayer((int)PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - PhotonNetwork.PlayerList.Length));
			}
		}

		private void StartTimer()
		{
			if (GameMode.Instance.modePlay != GameMode.ModePlay.PvpMode)
			{
				return;
			}
			if (PhotonNetwork.PlayerList.Length == 1)
			{
				UnityEngine.Debug.Log("++++++ StartTimer");
				this.remainTime = (float)UnityEngine.Random.Range(15, 20);
				this.enableTimer = true;
			}
		}

		private void CancelTimer()
		{
			UnityEngine.Debug.Log("++++++ CancelTimer");
			this.enableTimer = false;
		}

		private void OnTimerEnd()
		{
			this.CancelTimer();
			this.OnCreateFakePlayer();
		}

		private int playerTimeToLive = 12000;

		private int emptyRoomTimeToLive;

		private int waitTimeToCreateFakePlayer = 10;

		public Action OnConnectedToMasterCallBack;

		public Action OnCreateOrJoinRandomFailedCallBack;

		public Action OnJoinedRoomCallBack;

		public Action OnPlayerEnteredRoomCallBack;

		public Action OnPlayerLeftdRoomCallBack;

		public Action OnDisconnectedCallBack;

		private bool enableTimer;

		private float remainTime;

		private int joinRoomTryTime = 4;

		private int currentJoinRoomTryTime;

		private bool isStartButtonClicked;

		public FormPVP formPvP;
	}
}
