using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace PVPManager
{
	public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
	{
		public void Awake()
		{
			this.playerListEntries = new Dictionary<int, GameObject>();
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PlayerOverviewEntryPrefab);
				gameObject.transform.SetParent(base.gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				gameObject.GetComponent<Text>().color = AsteroidsGame.GetColor(player.GetPlayerNumber());
				gameObject.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", player.NickName, player.GetScore(), 3);
				this.playerListEntries.Add(player.ActorNumber, gameObject);
			}
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			UnityEngine.Object.Destroy(this.playerListEntries[otherPlayer.ActorNumber].gameObject);
			this.playerListEntries.Remove(otherPlayer.ActorNumber);
		}

		public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
		{
			GameObject gameObject;
			if (this.playerListEntries.TryGetValue(targetPlayer.ActorNumber, out gameObject))
			{
				gameObject.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", targetPlayer.NickName, targetPlayer.GetScore(), targetPlayer.CustomProperties["PlayerLives"]);
			}
		}

		public GameObject PlayerOverviewEntryPrefab;

		private Dictionary<int, GameObject> playerListEntries;
	}
}
