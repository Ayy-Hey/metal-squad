using System;
using Photon.Pun;
using UnityEngine;

namespace PVPManager
{
	public class PvP_LocalPlayer : PvP_PlayerProperty
	{
		public static PvP_LocalPlayer Instance
		{
			get
			{
				object @lock = PvP_LocalPlayer.m_Lock;
				PvP_LocalPlayer instance;
				lock (@lock)
				{
					if (PvP_LocalPlayer.m_Instance == null)
					{
						PvP_LocalPlayer.m_Instance = (PvP_LocalPlayer)UnityEngine.Object.FindObjectOfType(typeof(PvP_LocalPlayer));
						if (PvP_LocalPlayer.m_Instance == null)
						{
							GameObject gameObject = new GameObject();
							PvP_LocalPlayer.m_Instance = gameObject.AddComponent<PvP_LocalPlayer>();
							gameObject.name = typeof(PvP_LocalPlayer).ToString() + " (Singleton)";
							UnityEngine.Object.DontDestroyOnLoad(gameObject);
						}
					}
					instance = PvP_LocalPlayer.m_Instance;
				}
				return instance;
			}
		}

		public void Init()
		{
			UnityEngine.Debug.Log("Init PvP_LocalPlayer");
			this.player = PhotonNetwork.LocalPlayer;
			base.IsReady = false;
			base.LoadedScene = false;
			base.Score = 0;
			base.Alive = true;
			base.IsWinner = false;
			base.IsEndGame = false;
			base.EnemyTurn = 0;
			base.NickName = ((!(ProfileManager.pvpProfile.UserName != string.Empty)) ? ("Guest " + UnityEngine.Random.Range(0, 1000000)) : ProfileManager.pvpProfile.UserName);
			base.CountryCode = ProfileManager.pvpProfile.CountryCode;
			base.Power = ProfileManager.pvpProfile.Power;
			base.WinRate = ProfileManager.pvpProfile.WinRate;
			base.PvpScore = ProfileManager.pvpProfile.Score;
			base.Vip = ProfileManager.pvpProfile.Vip;
			base.RankLevel = ProfileManager.pvpProfile.RankLevel;
			base.AvatarUrl = ProfileManager.pvpProfile.AvatarUrl;
			base.ConstActorNumber = this.player.ActorNumber;
			base.CharactorID = ProfileManager.settingProfile.IDChar;
			base.MaxHP = ProfileManager.rambos[base.CharactorID].HP;
		}

		private static PvP_LocalPlayer m_Instance;

		private static object m_Lock = new object();
	}
}
