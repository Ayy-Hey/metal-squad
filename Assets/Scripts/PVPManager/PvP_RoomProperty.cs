using System;

namespace PVPManager
{
	public class PvP_RoomProperty : PhotonSingleton<PvP_RoomProperty>
	{
		public string LobbyName { get; set; }

		public string RoomName { get; set; }

		public byte MaxPlayer { get; set; }

		public int BettingAmount { get; set; }

		public PvP_RoomProperty.RoomBettingType BettingType { get; set; }

		public int PvpScore { get; set; }

		public int DeltaScore { get; set; }

		public bool IsFakePlayerMode { get; set; }

		public PvP_RoomProperty.RoomOnlineMode OnlineMode { get; set; }

		public const string MAX_PLAYER = "C0";

		public const string BETTING_AMOUNT = "C1";

		public const string BETTING_TYPE = "C2";

		public const string PVP_SCORE = "C3";

		public const string ONLINE_MODE = "C4";

		public const string DEFAULT_LOBBY = "DEFAULT_LOBBY";

		public const int PVP_MAP_NUM = 5;

		public const string PVP_MAPS = "PVP_MAPS";

		public const int COOP_MAP_NUM = 3;

		public const string COOP_MAP_INDEX = "COOP_MAP_INDEX";

		public const int IGNORE_PVP_SCORE = -1;

		public enum RoomBettingType
		{
			GOLD,
			GEM
		}

		public enum RoomOnlineMode
		{
			PVP,
			COOP
		}

		public enum PvpMapType
		{
			Type0,
			Type1,
			TotalType
		}
	}
}
