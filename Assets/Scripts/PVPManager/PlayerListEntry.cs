using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace PVPManager
{
	public class PlayerListEntry : MonoBehaviour
	{
		public void OnEnable()
		{
			PlayerNumbering.OnPlayerNumberingChanged += this.OnPlayerNumberingChanged;
		}

		public void Start()
		{
			if (PhotonNetwork.LocalPlayer.ActorNumber != this.ownerId)
			{
				this.PlayerReadyButton.gameObject.SetActive(false);
			}
			else
			{
				this.PlayerReadyButton.onClick.AddListener(delegate()
				{
					this.isPlayerReady = !this.isPlayerReady;
					this.SetPlayerReady(this.isPlayerReady);
					PvP_LocalPlayer.Instance.IsReady = this.isPlayerReady;
					if (PhotonNetwork.IsMasterClient)
					{
						UnityEngine.Object.FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
					}
				});
			}
		}

		public void OnDisable()
		{
			PlayerNumbering.OnPlayerNumberingChanged -= this.OnPlayerNumberingChanged;
		}

		public void Initialize(int playerId, string playerName)
		{
			this.ownerId = playerId;
			this.PlayerNameText.text = playerName;
		}

		private void OnPlayerNumberingChanged()
		{
			foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
			{
				if (player.ActorNumber == this.ownerId)
				{
					this.PlayerColorImage.color = AsteroidsGame.GetColor(player.GetPlayerNumber());
				}
			}
		}

		public void SetPlayerReady(bool playerReady)
		{
			this.PlayerReadyButton.GetComponentInChildren<Text>().text = ((!playerReady) ? "Ready?" : "Ready!");
			this.PlayerReadyImage.enabled = playerReady;
		}

		[Header("UI References")]
		public Text PlayerNameText;

		public Image PlayerColorImage;

		public Button PlayerReadyButton;

		public Image PlayerReadyImage;

		private int ownerId;

		private bool isPlayerReady;
	}
}
