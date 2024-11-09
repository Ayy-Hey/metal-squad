using System;
using System.Collections;
using System.Text;
using ABI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using PlayerStory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PVPManager
{
	public class PVPManager : MonoBehaviourPunCallbacks
	{
		public static PVPManager Instance
		{
			get
			{
				if (PVPManager.instance == null)
				{
					PVPManager.instance = UnityEngine.Object.FindObjectOfType<PVPManager>();
				}
				return PVPManager.instance;
			}
		}

		private void Awake()
		{
			if (!PVPManager.instance)
			{
				PVPManager.instance = this;
				return;
			}
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}

		private IEnumerator Start()
		{
			UnityEngine.Debug.Log("+++++++++++++++ PVPManager Start");
			this.GetRoomPropertyPvpMaps();
			this.LostBettingAmount();
			PlayerSetupGameOver playerGameOver = PlayerManagerStory.Instance.playerGameOver;
			playerGameOver.OnEnded = (Action)Delegate.Combine(playerGameOver.OnEnded, new Action(this.OnAirPlainGameOverEnd));
			PlayerManagerStory playerManagerStory = PlayerManagerStory.Instance;
			playerManagerStory.OnAirPlainStartGameEnded = (Action)Delegate.Combine(playerManagerStory.OnAirPlainStartGameEnded, new Action(this.OnAirPlainStartGameEnd));
			this.isFirstPlay = true;
			this.currentMapIdx = 0;
			PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEventReceived;
			yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
			Vector3 vtBoder = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f));
			CameraController.Instance.NumericBoundaries.LeftBoundary = -vtBoder.x;
			CameraController.Instance.NumericBoundaries.RightBoundary = vtBoder.x;
			PvP_LocalPlayer.Instance.LoadedScene = true;
			PvP_LocalPlayer.Instance.EnemyTurn = 0;
			this.startedGame = false;
			this.totalPlayTime = 0f;
			this.strBuilderTime = new StringBuilder();
			this.avatarShowed = false;
			this.completedFadeCameraFx = false;
			this.GetAllActorNumberAtStartGame();
			this.ShowAllPlayersStatus();
			this.status.text = PopupManager.Instance.GetText(Localization0.Waiting_For_Other_Player, null);
			UnityEngine.Debug.Log("............... Waiting for other player");
			yield break;
		}

		private void OnDestroy()
		{
			UnityEngine.Debug.Log("+++++++++++++++ PVPManager OnDestroy");
			PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEventReceived;
		}

		private void GetRoomPropertyPvpMaps()
		{
			object obj;
			if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("PVP_MAPS", out obj))
			{
				UnityEngine.Debug.Log("+++++++++++++++ Get pvp maps ok");
				this.PvpMaps = (int[])obj;
			}
		}

		private void OnAirPlainStartGameEnd()
		{
			if (this.isFirstPlay)
			{
				this.isFirstPlay = false;
				return;
			}
			UnityEngine.Debug.Log("+++++++++++ OnAirPlainStartGameEnd");
			this.RunNextMap();
		}

		private void OnAirPlainGameOverEnd()
		{
			UnityEngine.Debug.Log("+++++++++++ OnAirPlainGameOverEnd");
			CameraController.Instance.isInit = false;
			this.completedFadeCameraFx = false;
			CameraController.Instance.FadeCameraFx(delegate
			{
				CameraController.Instance.ResetCamera();
				GameManager.Instance.hudManager.HideControl();
				this.DestroyCurrentMap();
				Resources.UnloadUnusedAssets();
				base.StartCoroutine("WaitForLoadingNextmap");
			}, delegate
			{
				GameManager.Instance.hudManager.ShowControl(1.1f);
				this.completedFadeCameraFx = true;
				CameraController.Instance.isInit = true;
			});
		}

		private void DestroyCurrentMap()
		{
			UnityEngine.Debug.Log("+++++++++++++ DestroyCurrentMap");
			this.currentPvpMap.isInit = false;
			GameManager.Instance.StateManager.EState = EGamePlay.PAUSE;
			EnemyManager.Instance.OnClearEnemy();
			UnityEngine.Object.Destroy(this.currentPvpMap.gameObject);
		}

		private IEnumerator WaitForLoadingNextmap()
		{
			UnityEngine.Debug.Log("++++++++++++ Old map destroy complete");
			this.currentMapIdx++;
			yield return base.StartCoroutine(this.InstantiatePvpMap(this.currentMapIdx));
			this.OnNextTurn();
			GameManager.Instance.StateManager.EState = EGamePlay.RUNNING;
			yield return new WaitUntil(() => this.completedFadeCameraFx);
			this.completedFadeCameraFx = false;
			if (this.currentPvpMap.tfEffectAir != null)
			{
				PlayerManagerStory.Instance.SetPosEffectAir(this.currentPvpMap.tfEffectAir.position);
			}
			PlayerManagerStory.Instance.StartAirPlain();
			yield break;
		}

		public IEnumerator InstantiatePvpMap(int mapIdx)
		{
			UnityEngine.Debug.Log("++++++++++++ InstantiatePvpMap " + mapIdx);
			GameObject pvpMapObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(this.GetMapPath(mapIdx)), Vector3.zero, Quaternion.identity);
			this.currentPvpMap = pvpMapObject.GetComponent<PVPMap>();
			this.currentPvpMap.OnNextTurn.AddListener(new UnityAction(this.OnNextTurn));
			if (!EnemyManager.Instance.IsInit)
			{
				EnemyManager.Instance.OnInit();
			}
			try
			{
				if (this.currentPvpMap.tfEffectAir != null)
				{
					PlayerManagerStory.Instance.SetPosEffectAir(this.currentPvpMap.tfEffectAir.position);
				}
			}
			catch (Exception ex)
			{
			}
			yield return null;
			yield break;
		}

		private string GetMapPath(int mapIdx)
		{
			string text = string.Concat(new object[]
			{
				"PVP_Map/PVP_Map_",
				mapIdx,
				"_",
				this.PvpMaps[mapIdx]
			});
			UnityEngine.Debug.Log("Map Path: " + text);
			return text;
		}

		public bool OnCompleteMap()
		{
			UnityEngine.Debug.Log("Compelete map " + this.currentMapIdx);
			if (this.currentMapIdx >= this.numPvpMap - 1)
			{
				this.EndOfGame(true);
				return true;
			}
			return false;
		}

		private void RunNextMap()
		{
			this.currentPvpMap.Init(0);
		}

		private void OnNextTurn()
		{
			GameManager.Instance.ResetTime();
			PvP_LocalPlayer.Instance.EnemyTurn++;
			base.StartCoroutine(this.ShowStatus(PopupManager.Instance.GetText(Localization0.Wave, null) + " " + (PvP_LocalPlayer.Instance.EnemyTurn + 1)));
			UnityEngine.Debug.Log("EnemyTurn " + PvP_LocalPlayer.Instance.EnemyTurn);
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
				if (this.currentPvpMap != null && this.currentPvpMap.isInit)
				{
					this.currentPvpMap.OnUpdate(Time.deltaTime);
				}
				GameManager.Instance.UpdateTime(Time.deltaTime);
				this.totalPlayTime += Time.deltaTime;
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			this.HandlePauseForOnlineMode(pauseStatus);
		}

		public void HandlePauseForOnlineMode(bool pauseStatus)
		{
			if (pauseStatus && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
			{
				this.ChangeMasterClientifAvailble();
				PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
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
			this.CheckStartGame();
			this.ShowAllPlayersStatus();
			this.CheckEndOfGame();
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
			else if (cause == DisconnectCause.DisconnectByClientLogic)
			{
				try
				{
					PhotonSingleton<PvP_FakePlayerManager>.Instance.RemoveAllFakePlayer();
				}
				catch (Exception ex)
				{
				}
			}
			else
			{
				UnityEngine.Debug.Log("+++++++++++++++ EndOfGame OnDisconnected: " + cause);
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
			if (changedProps.ContainsKey("ENEMY_TURN_PROP") && this.isWorstPlayer())
			{
				UnityEngine.Debug.Log("+++++++++++++++ EndOfGame isWorstPlayer");
				this.EndOfGame(false);
			}
			this.CheckLocalPlayerIsBestPlayerAfterDie(targetPlayer, changedProps);
			this.CheckLocalPlayerHP(targetPlayer, changedProps);
			if (changedProps.ContainsKey("LOADED_SCENE_PROP") || changedProps.ContainsKey("IS_INIT_PROP"))
			{
				this.CheckStartGame();
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
					this.UpdatePlayerStatus(playerByActorNumber, this.playerCards[i]);
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

		private void CheckLocalPlayerIsBestPlayerAfterDie(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			if (!changedProps.ContainsKey("ENEMY_TURN_PROP") && !changedProps.ContainsKey("ENEMY_TURN_TIME_PROP"))
			{
				return;
			}
			if (targetPlayer.IsLocal)
			{
				return;
			}
			if (PvP_LocalPlayer.Instance.IsEndGame)
			{
				return;
			}
			if (!PvP_LocalPlayer.Instance.Alive && !this.isBestPlayer())
			{
				NetworkCountdownTimer.Instance.CancelNetworkTimer();
				UnityEngine.Debug.Log("+++++++++++++++ EndOfGame CheckLocalPlayerIsBestPlayerAfterDie");
				this.EndOfGame(false);
			}
		}

		private void CheckLocalPlayerHP(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			if (!changedProps.ContainsKey("HP_PROP"))
			{
				return;
			}
			if (this.CheckAllPlayerLoadedLevel() && targetPlayer.IsLocal)
			{
				this.CheckEndOfGame();
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

		private void OnStartGameCountdownTimerIsExpired()
		{
			this.currentPvpMap.Init(0);
			GameManager.Instance.ResetTime();
			this.GetAllActorNumberAtStartGame();
			PhotonNetwork.BackgroundTimeout = 10f;
			this.isInit = true;
			this.startedGame = true;
			ControlManager.Instance.OnShowControlStartGame();
			base.StartCoroutine(this.ShowStatus(PopupManager.Instance.GetText(Localization0.Start, null)));
			UnityEngine.Debug.Log("............... Start Game!!!");
		}

		private void LostBettingAmount()
		{
			PvP_RoomProperty.RoomBettingType bettingType = PhotonSingleton<PvP_RoomProperty>.Instance.BettingType;
			if (bettingType != PvP_RoomProperty.RoomBettingType.GOLD)
			{
				if (bettingType == PvP_RoomProperty.RoomBettingType.GEM)
				{
					PopupManager.Instance.SaveReward(Item.Gem, -PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount, "PVP_LostBetting_1vs" + (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1), null);
				}
			}
			else
			{
				PopupManager.Instance.SaveReward(Item.Gold, -PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount, "PVP_LostBetting_1vs" + (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1), null);
			}
		}

		private void CheckEndOfGame()
		{
			object lockCheckEndGame = this.LockCheckEndGame;
			lock (lockCheckEndGame)
			{
				if (!PvP_LocalPlayer.Instance.IsEndGame)
				{
					if (!PvP_LocalPlayer.Instance.Alive)
					{
						if (this.GetPlayingPlayerCount() <= 1)
						{
							UnityEngine.Debug.Log("+++++++++++++ Khong con player khac dang choi trong thoi gian count down sau khi die");
							NetworkCountdownTimer.Instance.CancelNetworkTimer();
							this.EndOfGame(true);
						}
					}
					else
					{
						float hp = PvP_LocalPlayer.Instance.HP;
						if (hp > 0f)
						{
							if (this.GetPlayingPlayerCount() <= 1)
							{
								UnityEngine.Debug.Log("+++++++++++++ Khong con player khac dang choi");
								this.EndOfGame(true);
							}
						}
						else
						{
							PvP_LocalPlayer.Instance.Alive = false;
							if (this.isBestPlayer())
							{
								UnityEngine.Debug.Log("++++++++++++ Die nhung wave tam thoi nhieu nhat");
								GameManager.Instance.hudManager.HideLineBoss();
								GameManager.Instance.hudManager.HideControl();
								NetworkCountdownTimer.Instance.StartNetworkTimer(this.waitTimeAfterDie, new Action(this.OnCheckEndGameCountdownTimerIsExpired), false);
							}
							else
							{
								UnityEngine.Debug.Log("+++++++++++++++ EndOfGame CheckEndOfGame !isBestPlayer");
								this.EndOfGame(false);
							}
						}
					}
				}
			}
		}

		private void OnCheckEndGameCountdownTimerIsExpired()
		{
			UnityEngine.Debug.Log("............... Check End Game After I Die!!!");
			this.CheckEndGameAfterDie();
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

		private void CheckEndGameAfterDie()
		{
			UnityEngine.Debug.Log("++++++++ CheckEndGameAfterDie");
			object lockCheckEndGameAfterDie = this.LockCheckEndGameAfterDie;
			lock (lockCheckEndGameAfterDie)
			{
				if (!PvP_LocalPlayer.Instance.IsEndGame)
				{
					if (!PhotonNetwork.IsConnectedAndReady)
					{
						UnityEngine.Debug.Log("+++++++++++++++ EndOfGame Thua luon sau khi time out after die ma khong co internet");
						this.EndOfGame(false);
					}
					else
					{
						UnityEngine.Debug.Log("+++++++++++++++ EndOfGame CheckEndGameAfterDie");
						this.EndOfGame(this.isBestPlayer());
					}
				}
			}
		}

		private bool isWorstPlayer()
		{
			object lockIsWorstPlayer = this.LockIsWorstPlayer;
			bool result;
			lock (lockIsWorstPlayer)
			{
				int enemyTurn = PvP_LocalPlayer.Instance.EnemyTurn;
				foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
				{
					if (!player.IsLocal)
					{
						object obj;
						if (player.CustomProperties.TryGetValue("ENEMY_TURN_PROP", out obj) && (int)obj - enemyTurn >= this.lessTurnsToLose)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		private bool isBestPlayer()
		{
			object lockIsBestPlayer = this.LockIsBestPlayer;
			bool result;
			lock (lockIsBestPlayer)
			{
				int enemyTurn = PvP_LocalPlayer.Instance.EnemyTurn;
				int enemyTurnTime = PvP_LocalPlayer.Instance.EnemyTurnTime;
				foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
				{
					if (!player.IsLocal)
					{
						object obj;
						if (player.CustomProperties.TryGetValue("ENEMY_TURN_PROP", out obj))
						{
							if ((int)obj > enemyTurn)
							{
								return false;
							}
							object obj2;
							if ((int)obj == enemyTurn && player.CustomProperties.TryGetValue("ENEMY_TURN_TIME_PROP", out obj2) && enemyTurnTime <= (int)obj2)
							{
								return false;
							}
						}
					}
				}
				result = true;
			}
			return result;
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

		private void EndOfGame(bool isWinner)
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
						if (isWinner)
						{
							if (this.GetPlayingPlayerCount() > 1)
							{
								foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
								{
									if (player.IsLocal)
									{
										this.SetPlayerIsEndGame(player, true);
									}
									else
									{
										this.SetPlayerIsEndGame(player, false);
									}
								}
								UnityEngine.Debug.Log("++++++++ I win and send end game event to other player");
								PVPManager.RaiseEvent(1, null, ReceiverGroup.All);
							}
							else
							{
								UnityEngine.Debug.Log("++++++++ I win");
								this.SetPlayerIsEndGame(PhotonNetwork.LocalPlayer, true);
								this.QuitRoom(true);
							}
						}
						else
						{
							UnityEngine.Debug.Log("++++++++ I lose");
							this.SetPlayerIsEndGame(PhotonNetwork.LocalPlayer, false);
							this.QuitRoom(false);
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
				GameManager.Instance.StateManager.popupEndMission.state = EndMission.PvPWin;
			}
			else
			{
				GameManager.Instance.StateManager.popupEndMission.state = EndMission.PvPLost;
			}
			GameManager.Instance.StateManager.popupEndMission.OnShow();
		}

		private void ShowNotStartGame()
		{
			int bettingAmount = PhotonSingleton<PvP_RoomProperty>.Instance.BettingAmount;
			PvP_RoomProperty.RoomBettingType bettingType = PhotonSingleton<PvP_RoomProperty>.Instance.BettingType;
			if (bettingType != PvP_RoomProperty.RoomBettingType.GOLD)
			{
				if (bettingType == PvP_RoomProperty.RoomBettingType.GEM)
				{
					PopupManager.Instance.SaveReward(Item.Gem, bettingAmount, "PVP_RefundBetting_1vs" + (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1), null);
				}
			}
			else
			{
				PopupManager.Instance.SaveReward(Item.Gold, bettingAmount, "PVP_RefundBetting_1vs" + (int)(PhotonSingleton<PvP_RoomProperty>.Instance.MaxPlayer - 1), null);
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

		private static PVPManager instance;

		[HideInInspector]
		public bool isInit;

		private int numPvpMap = 5;

		private int[] PvpMaps;

		public const string PVP_Map_Prefix = "PVP_Map/PVP_Map_";

		public PVPMap currentPvpMap;

		public int currentMapIdx;

		public Text status;

		private float waitTimeAfterDie = 45f;

		private float waitTimeStartGame = 3f;

		private float waitTimeAfterDisconnect = 12f;

		private float poolingCheckInternetConnectionTime = 1f;

		public int IncreaseEnemyLevelNextTurn = 1;

		public PvpPlayerStatusCard[] playerCards;

		private int[] actorNumbers;

		private bool avatarShowed;

		private bool completedFadeCameraFx;

		private int lessTurnsToLose = 3;

		private bool isLeftRoomBeforeLoadScene;

		private readonly object LockCheckEndGame = new object();

		private readonly object LockCheckEndGameAfterDie = new object();

		private readonly object LockEndGame = new object();

		private readonly object LockIsBestPlayer = new object();

		private readonly object LockIsWorstPlayer = new object();

		private bool startedGame;

		private bool isFirstPlay;

		public float totalPlayTime;

		private StringBuilder strBuilderTime;

		private WaitForSeconds wait;

		public enum PvP_Event
		{
			START_BUTTON_CLICKED,
			END_GAME
		}
	}
}
