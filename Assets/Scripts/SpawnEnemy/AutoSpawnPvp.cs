using System;
using System.Collections;
using PVPManager;
using UnityEngine;
using UnityEngine.Events;

namespace SpawnEnemy
{
	public class AutoSpawnPvp : MonoBehaviour
	{
		public void OnInit()
		{
			if (this.isInit)
			{
				return;
			}
			for (int i = 0; i < this.turns.Length; i++)
			{
				this.turns[i].Init();
			}
			this.SetLevelEnemy(Mathf.Min(PvP_LocalPlayer.Instance.EnemyTurn * PVPManager.PVPManager.Instance.IncreaseEnemyLevelNextTurn, 14));
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				this.OnSetCoinDrop();
				this.OnResetTotalEnemies();
			}
			if (this.OnStarted != null)
			{
				this.OnStarted.Invoke();
			}
			LeanTween.cancel(CameraController.Instance.gameObject);
			base.StartCoroutine(this.OnShow());
			this.isInit = true;
		}

		public void SetLevelEnemy(int level)
		{
			for (int i = 0; i < this.turns.Length; i++)
			{
				TurnPvp turnPvp = this.turns[i];
				for (int j = 0; j < turnPvp.enemies.Length; j++)
				{
					turnPvp.enemies[j].Level = level;
				}
			}
		}

		private void OnResetTotalEnemies()
		{
			int num = 0;
			for (int i = 0; i < this.turns.Length; i++)
			{
				TurnPvp turnPvp = this.turns[i];
				num += turnPvp.enemies.Length;
			}
			GameManager.Instance.StateManager.inforGamePlayPause.TotalEnemy += num;
		}

		private void OnSetCoinDrop()
		{
			for (int i = 0; i < this.turns.Length; i++)
			{
				TurnPvp turnPvp = this.turns[i];
				for (int j = 0; j < turnPvp.enemies.Length; j++)
				{
					Enemy enemy = turnPvp.enemies[j];
					float num = UnityEngine.Random.Range(0f, 1f);
					if (GameManager.Instance.MAX_COIN_DROP <= 0)
					{
						return;
					}
					if (num >= 0.7f && !enemy.DropCoin)
					{
						int[] array = new int[]
						{
							1,
							1,
							1,
							2,
							2,
							2,
							3,
							3,
							3,
							3
						};
						int num2 = array[UnityEngine.Random.Range(0, array.Length)];
						GameManager.Instance.MAX_COIN_DROP -= num2 * 3;
						enemy.DropCoin = true;
						enemy.ValueCoin = num2;
					}
				}
			}
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.isInit)
			{
				return;
			}
			for (int i = 0; i < this.turns.Length; i++)
			{
				this.turns[i].OnUpdate(deltaTime);
			}
		}

		private IEnumerator OnShow()
		{
			while (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
			{
				yield return null;
			}
			if (this.IDTurn < this.turns.Length)
			{
				yield return new WaitForSeconds(this.turns[this.IDTurn].Time_Delay);
				this.StartRunTurn();
			}
			yield break;
		}

		private void StartRunTurn()
		{
			if (this.IDTurn < this.turns.Length)
			{
				this.turns[this.IDTurn].OnShow();
				this.turns[this.IDTurn].OnClear = delegate()
				{
					this.IDTurn++;
					if (this.IDTurn >= this.turns.Length)
					{
						this.isInit = false;
						base.StopAllCoroutines();
						if (this.OnCompleted != null)
						{
							this.OnCompleted.Invoke();
						}
						return;
					}
					base.StartCoroutine(this.OnShow());
				};
			}
		}

		[HideInInspector]
		public bool isInit;

		public TurnPvp[] turns;

		private int IDTurn;

		public UnityEvent OnStarted;

		public UnityEvent OnCompleted;
	}
}
