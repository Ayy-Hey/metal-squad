using System;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace PVPManager
{
	public class PvP_PlayerProperty : MonoBehaviour
	{
		public int CharactorID
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("CHARACTOR_ID", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"CHARACTOR_ID",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool IsFakePlayer
		{
			get
			{
				object obj;
				return this.player.CustomProperties.TryGetValue("IS_FAKE_PLAYER", out obj) && (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"IS_FAKE_PLAYER",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int ConstActorNumber
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("CONST_ACTOR_NUMBER", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"CONST_ACTOR_NUMBER",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int CountryCode
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("COUNTRY_CODE_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"COUNTRY_CODE_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int Power
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("POWER_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"POWER_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int RankLevel
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("RANK_LEVEL_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"RANK_LEVEL_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int PvpScore
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("PVP_SCORE_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"PVP_SCORE_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int WinRate
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("WIN_RATE_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"WIN_RATE_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int Vip
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("VIP_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"VIP_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public float HP
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("HP_PROP", out obj))
				{
					return (float)obj;
				}
				return 0f;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"HP_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public float MaxHP
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("MAX_HP_PROP", out obj))
				{
					return (float)obj;
				}
				return 0f;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"MAX_HP_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int EnemyTurn
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("ENEMY_TURN_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"ENEMY_TURN_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int EnemyTurnTime
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("ENEMY_TURN_TIME_PROP", out obj))
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"ENEMY_TURN_TIME_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool IsReady
		{
			get
			{
				object obj;
				return this.player.CustomProperties.TryGetValue("IS_READY_PROP", out obj) && (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"IS_READY_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool LoadedScene
		{
			get
			{
				object obj;
				return this.player.CustomProperties.TryGetValue("LOADED_SCENE_PROP", out obj) && (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"LOADED_SCENE_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool IsInit
		{
			get
			{
				object obj;
				return this.player.CustomProperties.TryGetValue("IS_INIT_PROP", out obj) && (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"IS_INIT_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool Alive
		{
			get
			{
				object obj;
				return !this.player.CustomProperties.TryGetValue("ALIVE_PROP", out obj) || (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"ALIVE_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool IsWinner
		{
			get
			{
				object obj;
				return this.player.CustomProperties.TryGetValue("IS_WINNER_PROP", out obj) && (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"IS_WINNER_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public bool IsEndGame
		{
			get
			{
				object obj;
				return this.player.CustomProperties.TryGetValue("IS_END_GAME_PROP", out obj) && (bool)obj;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"IS_END_GAME_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public string NickName
		{
			get
			{
				return this.player.NickName;
			}
			set
			{
				this.player.NickName = value;
			}
		}

		public string AvatarUrl
		{
			get
			{
				object obj;
				if (this.player.CustomProperties.TryGetValue("AVATAR_URL_PROP", out obj))
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				Hashtable propertiesToSet = new Hashtable
				{
					{
						"AVATAR_URL_PROP",
						value
					}
				};
				this.player.SetCustomProperties(propertiesToSet, null, null);
			}
		}

		public int Score
		{
			get
			{
				return this.player.GetScore();
			}
			set
			{
				this.player.SetScore(value);
			}
		}

		public const string IS_FAKE_PLAYER = "IS_FAKE_PLAYER";

		public const string CONST_ACTOR_NUMBER = "CONST_ACTOR_NUMBER";

		public const string PVP_SCORE_PROP = "PVP_SCORE_PROP";

		public const string HP_PROP = "HP_PROP";

		public const string MAX_HP_PROP = "MAX_HP_PROP";

		public const string ENEMY_TURN_PROP = "ENEMY_TURN_PROP";

		public const string ENEMY_TURN_TIME_PROP = "ENEMY_TURN_TIME_PROP";

		public const string IS_READY_PROP = "IS_READY_PROP";

		public const string LOADED_SCENE_PROP = "LOADED_SCENE_PROP";

		public const string IS_INIT_PROP = "IS_INIT_PROP";

		public const string ALIVE_PROP = "ALIVE_PROP";

		public const string IS_WINNER_PROP = "IS_WINNER_PROP";

		public const string IS_END_GAME_PROP = "IS_END_GAME_PROP";

		public const string COUNTRY_CODE_PROP = "COUNTRY_CODE_PROP";

		public const string POWER_PROP = "POWER_PROP";

		public const string WIN_RATE_PROP = "WIN_RATE_PROP";

		public const string VIP_PROP = "VIP_PROP";

		public const string RANK_LEVEL_PROP = "RANK_LEVEL_PROP";

		public const string AVATAR_URL_PROP = "AVATAR_URL_PROP";

		public const string CHARACTOR_ID = "CHARACTOR_ID";

		public Photon.Realtime.Player player;
	}
}
