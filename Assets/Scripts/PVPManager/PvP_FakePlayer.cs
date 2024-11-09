using System;
using UnityEngine;

namespace PVPManager
{
	public class PvP_FakePlayer : PvP_PlayerProperty
	{
		public void Init()
		{
			UnityEngine.Debug.Log("Init fake player " + this.player.ActorNumber);
			this.isInit = false;
			base.IsFakePlayer = true;
			base.HP = 100f;
			base.IsReady = true;
			base.LoadedScene = true;
			base.IsInit = true;
			base.Score = 0;
			base.Alive = true;
			base.IsWinner = false;
			base.IsEndGame = false;
			base.ConstActorNumber = this.player.ActorNumber;
			base.NickName = "Guest " + UnityEngine.Random.Range(0, 1000000);
			base.CountryCode = UnityEngine.Random.Range(0, 50);
			base.Power = ProfileManager.pvpProfile.Power + UnityEngine.Random.Range(0, 5);
			base.WinRate = ProfileManager.pvpProfile.WinRate + UnityEngine.Random.Range(0, 5);
			base.PvpScore = ProfileManager.pvpProfile.Score + UnityEngine.Random.Range(0, 5);
			base.Vip = ProfileManager.pvpProfile.Vip + UnityEngine.Random.Range(0, 5);
			base.RankLevel = ProfileManager.pvpProfile.RankLevel + UnityEngine.Random.Range(0, 5);
			this.increaseTime = 0f;
			this.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.INIT;
			this.doneNextEnemyTurn = false;
			this.doneLoadedScene = false;
			this.randomDieEnemyTurn = UnityEngine.Random.Range(this.randomDieEnemyTurnMin, this.randomDieEnemyTurnMax);
			this.randomDieEnemyTurnTime = UnityEngine.Random.Range(this.randomDieEnemyTurnTimeMin, this.randomDieEnemyTurnTimeMax);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"The fake player ",
				base.ConstActorNumber,
				" will be die at Turn: ",
				this.randomDieEnemyTurn,
				" Time: ",
				this.randomDieEnemyTurnTime
			}));
			this.isInit = true;
		}

		private void Update()
		{
			if (!this.isInit)
			{
				return;
			}
			if (this.ChangeState())
			{
				this.changeState = true;
			}
			else
			{
				this.changeState = false;
			}
			if (this.changeState)
			{
				this.UpdateState();
				this.changeState = false;
			}
			if (this.fakePlayerState == PvP_FakePlayer.PvP_FakePlayer_State.PLAY_ENEMY_TURN)
			{
				this.UpdateTime(Time.deltaTime);
			}
		}

		private bool ChangeState()
		{
			if (PvP_LocalPlayer.Instance.IsEndGame)
			{
				return false;
			}
			bool flag = false;
			switch (this.fakePlayerState)
			{
			case PvP_FakePlayer.PvP_FakePlayer_State.INIT:
				if (this.TriggerLoadedScene())
				{
					this.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.LOADED_SCENE;
					flag = true;
				}
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.LOADED_SCENE:
				if (this.TriggerStartPlay())
				{
					this.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.PLAY_ENEMY_TURN;
					this.QuitLoadedScene();
					flag = true;
				}
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.NEXT_ENEMY_TURN:
				if (this.TriggerDoneNextEnemyTurn())
				{
					this.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.PLAY_ENEMY_TURN;
					this.QuitNextEnemyTurn();
					flag = true;
				}
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.PLAY_ENEMY_TURN:
				if (this.TriggerDie())
				{
					this.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.DIE;
					flag = true;
				}
				else if (this.TriggerNextEnemyTurn())
				{
					this.fakePlayerState = PvP_FakePlayer.PvP_FakePlayer_State.NEXT_ENEMY_TURN;
					flag = true;
				}
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.DIE:
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Fake player ",
					base.ConstActorNumber,
					" change state to ",
					this.fakePlayerState
				}));
			}
			return flag;
		}

		private bool TriggerLoadedScene()
		{
			return PvP_LocalPlayer.Instance.LoadedScene;
		}

		private bool TriggerStartPlay()
		{
			return PVPManager.Instance.isInit && this.doneLoadedScene;
		}

		private bool TriggerDoneNextEnemyTurn()
		{
			return this.doneNextEnemyTurn;
		}

		private void QuitNextEnemyTurn()
		{
			this.doneNextEnemyTurn = false;
		}

		private bool TriggerNextEnemyTurn()
		{
			return base.EnemyTurnTime > this.randomCurrentEnemyTurnTime;
		}

		private bool TriggerDie()
		{
			return base.EnemyTurn == this.randomDieEnemyTurn && base.EnemyTurnTime > this.randomDieEnemyTurnTime;
		}

		private void QuitLoadedScene()
		{
			this.doneLoadedScene = false;
		}

		private void SetupNewEnemyTurn(int turn)
		{
			base.EnemyTurn = turn;
			this.ResetTime();
			if (turn == this.randomDieEnemyTurn)
			{
				this.randomCurrentEnemyTurnTime = this.randomDieEnemyTurnTime + 5;
			}
			else
			{
				this.randomCurrentEnemyTurnTime = UnityEngine.Random.Range(this.randomEnemyTurnTimeMin, this.randomEnemyTurnTimeMax);
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Fake player ",
				base.ConstActorNumber,
				" Enemy Turn: ",
				turn,
				" Time:",
				this.randomCurrentEnemyTurnTime
			}));
		}

		private void UpdateState()
		{
			if (PvP_LocalPlayer.Instance.IsEndGame)
			{
				return;
			}
			switch (this.fakePlayerState)
			{
			case PvP_FakePlayer.PvP_FakePlayer_State.INIT:
				this.OnInit();
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.LOADED_SCENE:
				this.OnLoadedScene();
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.NEXT_ENEMY_TURN:
				this.OnNextEnemyTurn();
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.PLAY_ENEMY_TURN:
				this.OnPlayEnemyTurn();
				break;
			case PvP_FakePlayer.PvP_FakePlayer_State.DIE:
				this.OnDie();
				break;
			default:
				this.OnPlayEnemyTurn();
				break;
			}
		}

		private void OnInit()
		{
		}

		private void OnLoadedScene()
		{
			this.SetupNewEnemyTurn(0);
			this.doneLoadedScene = true;
		}

		private void OnNextEnemyTurn()
		{
			this.SetupNewEnemyTurn(base.EnemyTurn + 1);
			this.doneNextEnemyTurn = true;
		}

		private void OnPlayEnemyTurn()
		{
		}

		private void OnDie()
		{
			base.IsEndGame = true;
			PhotonSingleton<PvP_FakePlayerManager>.Instance.KillFakePlayer(this);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void UpdateTime(float deltaTime)
		{
			this.increaseTime += deltaTime;
			if (this.increaseTime >= 1f)
			{
				base.EnemyTurnTime++;
				this.increaseTime = 0f;
			}
		}

		public void ResetTime()
		{
			base.EnemyTurnTime = 0;
		}

		public PvP_FakePlayer.PvP_FakePlayer_State fakePlayerState;

		private bool changeState;

		private bool doneNextEnemyTurn;

		private bool doneLoadedScene;

		public int randomCurrentEnemyTurnTime;

		public int randomEnemyTurnTimeMin = 20;

		public int randomEnemyTurnTimeMax = 30;

		public int randomDieEnemyTurn;

		public int randomDieEnemyTurnMin = 20;

		public int randomDieEnemyTurnMax = 30;

		public int randomDieEnemyTurnTime;

		public int randomDieEnemyTurnTimeMin = 5;

		public int randomDieEnemyTurnTimeMax = 30;

		private bool isInit;

		private float increaseTime;

		public enum PvP_FakePlayer_State
		{
			INIT,
			LOADED_SCENE,
			NEXT_ENEMY_TURN,
			PLAY_ENEMY_TURN,
			DIE
		}
	}
}
