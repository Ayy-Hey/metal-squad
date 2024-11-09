using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PVPManager
{
	public class AsteroidsGameManager : MonoBehaviourPunCallbacks
	{
		public void Awake()
		{
			AsteroidsGameManager.Instance = this;
		}

		public override void OnEnable()
		{
			base.OnEnable();
		}

		public void Start()
		{
			this.InfoText.text = "Waiting for other players...";
			ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
			{
				{
					"PlayerLoadedLevel",
					true
				}
			};
			PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet, null, null);
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}

		private IEnumerator SpawnAsteroid()
		{
			for (;;)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
				Vector2 direction = UnityEngine.Random.insideUnitCircle;
				Vector3 position = Vector3.zero;
				if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
				{
					position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0f, direction.y * Camera.main.orthographicSize);
				}
				else
				{
					position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0f, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
				}
				position -= position.normalized * 0.1f;
				Vector3 force = -position.normalized * 1000f;
				Vector3 torque = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(500f, 1500f);
				object[] instantiationData = new object[]
				{
					force,
					torque,
					true
				};
				PhotonNetwork.InstantiateSceneObject("BigAsteroid", position, Quaternion.Euler(UnityEngine.Random.value * 360f, UnityEngine.Random.value * 360f, UnityEngine.Random.value * 360f), 0, instantiationData);
			}
			yield break;
		}

		private IEnumerator EndOfGame(string winner, int score)
		{
			for (float timer = 5f; timer > 0f; timer -= Time.deltaTime)
			{
				this.InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));
				yield return new WaitForEndOfFrame();
			}
			PhotonNetwork.LeaveRoom(true);
			yield break;
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			SceneManager.LoadScene("LobbyScene");
		}

		public override void OnLeftRoom()
		{
			PhotonNetwork.Disconnect();
		}

		public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
		{
			if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
			{
				base.StartCoroutine(this.SpawnAsteroid());
			}
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			this.CheckEndOfGame();
		}

		public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			if (changedProps.ContainsKey("PlayerLives"))
			{
				this.CheckEndOfGame();
				return;
			}
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (changedProps.ContainsKey("PlayerLoadedLevel") && this.CheckAllPlayerLoadedLevel())
			{
				ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
				{
					{
						"StartTime",
						(float)PhotonNetwork.Time
					}
				};
				PhotonNetwork.CurrentRoom.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		private void StartGame()
		{
			float num = 360f / (float)PhotonNetwork.CurrentRoom.PlayerCount * (float)PhotonNetwork.LocalPlayer.GetPlayerNumber();
			float x = 20f * Mathf.Sin(num * 0.0174532924f);
			float z = 20f * Mathf.Cos(num * 0.0174532924f);
			Vector3 position = new Vector3(x, 0f, z);
			Quaternion rotation = Quaternion.Euler(0f, num, 0f);
			PhotonNetwork.Instantiate("Spaceship", position, rotation, 0, null);
			if (PhotonNetwork.IsMasterClient)
			{
				base.StartCoroutine(this.SpawnAsteroid());
			}
		}

		private bool CheckAllPlayerLoadedLevel()
		{
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				object obj;
				if (!player.CustomProperties.TryGetValue("PlayerLoadedLevel", out obj) || !(bool)obj)
				{
					return false;
				}
			}
			return true;
		}

		private void CheckEndOfGame()
		{
			bool flag = true;
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				object obj;
				if (player.CustomProperties.TryGetValue("PlayerLives", out obj) && (int)obj > 0)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (PhotonNetwork.IsMasterClient)
				{
					base.StopAllCoroutines();
				}
				string winner = string.Empty;
				int num = -1;
				foreach (Photon.Realtime.Player player2 in PhotonNetwork.PlayerList)
				{
					if (player2.GetScore() > num)
					{
						winner = player2.NickName;
						num = player2.GetScore();
					}
				}
				base.StartCoroutine(this.EndOfGame(winner, num));
			}
		}

		private void OnCountdownTimerIsExpired()
		{
			this.StartGame();
		}

		public static AsteroidsGameManager Instance;

		public Text InfoText;

		public GameObject[] AsteroidPrefabs;
	}
}
