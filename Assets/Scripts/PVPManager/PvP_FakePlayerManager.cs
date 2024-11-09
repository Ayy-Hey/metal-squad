using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace PVPManager
{
	public class PvP_FakePlayerManager : PhotonSingleton<PvP_FakePlayerManager>
	{
		public void Init()
		{
			this.fakePlayerList = new List<PvP_FakePlayer>();
		}

		public void AddFakePlayer(PvP_FakePlayer newFakePlayer)
		{
			if (this.fakePlayerList != null)
			{
				this.fakePlayerList.Add(newFakePlayer);
			}
		}

		public void RemoveAllFakePlayer()
		{
			if (this.fakePlayerList == null)
			{
				return;
			}
			foreach (PvP_FakePlayer pvP_FakePlayer in this.fakePlayerList)
			{
				pvP_FakePlayer.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.DIE;
				UnityEngine.Object.Destroy(pvP_FakePlayer.gameObject);
			}
			this.fakePlayerList.Clear();
		}

		public void KillFakePlayer(PvP_FakePlayer player)
		{
			UnityEngine.Debug.Log("++++++++++++ Kill fake player: " + player.ConstActorNumber);
			ExitGames.Client.Photon.Hashtable value = new ExitGames.Client.Photon.Hashtable
			{
				{
					"IS_FAKE_PLAYER",
					true
				},
				{
					"CONST_ACTOR_NUMBER",
					player.ConstActorNumber
				}
			};
			EventData eventData = new EventData();
			eventData.Parameters = new Dictionary<byte, object>();
			eventData.Parameters.Add(249, value);
			eventData.Parameters.Add(254, player.ConstActorNumber);
			eventData.Code = 254;
			PhotonNetwork.CurrentRoom.LoadBalancingClient.OnEvent(eventData);
			this.fakePlayerList.Remove(player);
		}

		public IEnumerator StartCreateFakePlayer(int playerCnt)
		{
			int curPlayerCnt = playerCnt;
			WaitForSeconds wait = new WaitForSeconds((float)UnityEngine.Random.Range(1, 3));
			while ((float)curPlayerCnt > 0f)
			{
				yield return wait;
				this.CreateFakePlayer();
				curPlayerCnt--;
			}
			yield break;
		}

		public void CreateFakePlayer()
		{
			try
			{
				UnityEngine.Debug.Log("++++++++++++ Create fake player");
				ExitGames.Client.Photon.Hashtable value = new ExitGames.Client.Photon.Hashtable
				{
					{
						"IS_FAKE_PLAYER",
						true
					},
					{
						"HP_PROP",
						100f
					},
					{
						"IS_READY_PROP",
						true
					},
					{
						"LOADED_SCENE_PROP",
						true
					},
					{
						"IS_INIT_PROP",
						true
					},
					{
						"ALIVE_PROP",
						true
					},
					{
						"IS_WINNER_PROP",
						false
					},
					{
						"IS_END_GAME_PROP",
						false
					}
				};
				EventData eventData = new EventData();
				eventData.Parameters = new Dictionary<byte, object>();
				eventData.Parameters.Add(249, value);
				eventData.Code = byte.MaxValue;
				PhotonNetwork.CurrentRoom.LoadBalancingClient.OnEvent(eventData);
			}
			catch (Exception ex)
			{
			}
		}

		private List<PvP_FakePlayer> fakePlayerList;
	}
}
