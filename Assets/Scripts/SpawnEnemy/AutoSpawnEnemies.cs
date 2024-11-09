using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;
using UnityEngine.Events;

namespace SpawnEnemy
{
	public class AutoSpawnEnemies : MonoBehaviour
	{
		public void OnInit()
		{
			this.basetrigger.TriggerTarget = GameManager.Instance.player.transform;
			this.basetrigger.OnEnteredTrigger = delegate()
			{
				if (this.isInit)
				{
					return;
				}
				this.isInit = true;
				if (this.OnStarted != null)
				{
					this.OnStarted();
				}
				LeanTween.cancel(CameraController.Instance.gameObject);
				this.OnStart(this.basetrigger.LeftBoundary);
			};
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				this.OnSetCoinDrop();
				this.OnResetTotalEnemies();
			}
		}

		public void SetLevelEnemy(int level)
		{
			for (int i = 0; i < this.turns.Length; i++)
			{
				Turn turn = this.turns[i];
				for (int j = 0; j < turn.enemies.Length; j++)
				{
					if (turn.enemies[j] != null)
					{
						turn.enemies[j].Level = level;
					}
				}
			}
		}

		private void OnResetTotalEnemies()
		{
			int num = 0;
			for (int i = 0; i < this.turns.Length; i++)
			{
				Turn turn = this.turns[i];
				num += turn.enemies.Length;
			}
			GameManager.Instance.StateManager.inforGamePlayPause.TotalEnemy += num;
		}

		private void OnSetCoinDrop()
		{
			for (int i = 0; i < this.turns.Length; i++)
			{
				Turn turn = this.turns[i];
				if (!(turn == null))
				{
					for (int j = 0; j < turn.enemies.Length; j++)
					{
						Enemy enemy = turn.enemies[j];
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
		}

		public void OnStart(float LeftBoundary)
		{
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				CameraController.Instance.StopMoveLeftBoundary = true;
				CameraController.Instance.NumericBoundaries.LeftBoundary = LeftBoundary;
			}
			base.StartCoroutine(this.OnShow());
		}

		public void OnUpdate(float deltaTime)
		{
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
					if (this.IDTurn < this.turns.Length)
					{
						base.StartCoroutine(this.OnShow());
						return;
					}
					base.StopAllCoroutines();
					CameraController.Instance.StopMoveLeftBoundary = false;
					if (this.basetrigger != null)
					{
						this.basetrigger.gameObject.SetActive(false);
					}
					if (this.eventCompleted.GetPersistentEventCount() > 0)
					{
						UnityEngine.Debug.Log("event completed auto spawn -----------------------------------------------");
						this.eventCompleted.Invoke();
						return;
					}
					if (this.eventCompleted2 != null)
					{
						this.eventCompleted2.Invoke();
					}
					if (this.OnCompleted != null)
					{
						this.OnCompleted();
						UnityEngine.Debug.Log("completed auto spawn -----------------------------------------------");
					}
					else
					{
						UnityEngine.Debug.Log("next check point -----------------------------------------------");
						CameraController.Instance.NewCheckpoint(true, 5f);
					}
				};
			}
		}

		public Turn[] turns;

		[SerializeField]
		private ProCamera2DTriggerBoundaries basetrigger;

		private bool isInit;

		private int IDTurn;

		public Action OnCompleted;

		public Action OnStarted;

		public UnityEvent eventCompleted;

		public UnityEvent eventCompleted2;
	}
}
