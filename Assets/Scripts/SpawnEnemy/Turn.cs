using System;
using System.Collections;
using UnityEngine;

namespace SpawnEnemy
{
	public class Turn : MonoBehaviour
	{
		public void OnShow()
		{
			this.time_countdown = 0f;
			this.countEnemyBorn = 0;
			this.TotalEnemy = this.enemies.Length;
			base.StartCoroutine(this.OnCreateEnemy());
			this.ignoreTimeToNext = true;
			this.isInit = true;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.isInit)
			{
				return;
			}
			if (this.ignoreTimeToNext)
			{
				if (this.countEnemyBorn == this.enemies.Length)
				{
					if (GameManager.Instance.ListEnemy.Count > 0)
					{
						bool flag = false;
						for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
						{
							if (GameManager.Instance.ListEnemy[i].isInCamera)
							{
								flag = true;
								this.time_countdown = 0f;
								break;
							}
						}
						if (flag)
						{
							return;
						}
						this.time_countdown += deltaTime;
					}
					bool flag2 = GameManager.Instance.ListEnemy.Count == 0 || this.time_countdown >= 5f;
					if (flag2)
					{
						this.OnClear();
						this.isInit = false;
					}
				}
				return;
			}
			this.time_countdown += deltaTime;
			if (this.time_countdown >= this.Time_To_Next_Turn)
			{
				if (this.OnClear != null)
				{
					this.OnClear();
				}
				this.isInit = false;
			}
		}

		private IEnumerator OnCreateEnemy()
		{
			for (int i = 0; i < this.enemies.Length; i++)
			{
				Enemy enemy = this.enemies[i];
				while (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
				{
					yield return null;
				}
				yield return new WaitForSeconds(enemy.TimeDelay);
				EnemyDataInfo cacheEnemyData = new EnemyDataInfo();
				cacheEnemyData.ismove = enemy.isMove;
				cacheEnemyData.type = (int)enemy.Type;
				cacheEnemyData.pos_x = enemy.transform.position.x;
				cacheEnemyData.pos_y = ((!enemy.isParachute) ? enemy.transform.position.y : CameraController.Instance.TopCamera());
				cacheEnemyData.DropCoin = enemy.DropCoin;
				cacheEnemyData.ValueCoin = enemy.ValueCoin;
				cacheEnemyData.level = enemy.Level;
				this.countEnemyBorn++;
				BaseEnemy baseEnemy = EnemyManager.Instance.CreateNewEnemy(cacheEnemyData, false);
				if (baseEnemy != null)
				{
					if (enemy.isParachute)
					{
						baseEnemy.SetParachuter(0.3f);
					}
					BaseEnemy baseEnemy2 = baseEnemy;
					baseEnemy2.OnEnemyDeaded = (Action)Delegate.Combine(baseEnemy2.OnEnemyDeaded, new Action(delegate()
					{
						GameManager.Instance.TotalEnemyKilled++;
						this.TotalEnemy--;
						if (this.OnClear != null && this.TotalEnemy <= 0 && this.isInit)
						{
							this.OnClear();
							this.isInit = false;
						}
						baseEnemy.OnEnemyDeaded = null;
					}));
				}
				else
				{
					GameManager.Instance.TotalEnemyKilled++;
					this.TotalEnemy--;
					if (this.OnClear != null && this.TotalEnemy <= 0 && this.isInit)
					{
						this.OnClear();
						this.isInit = false;
					}
				}
				float r = CameraController.Instance.camPos.x + CameraController.Instance.Size().x;
				float j = CameraController.Instance.camPos.x - CameraController.Instance.Size().x;
				float t = CameraController.Instance.camPos.y + CameraController.Instance.Size().y;
				if (cacheEnemyData.pos_x > r)
				{
					this.posWarning.x = r - 0.5f;
					this.posWarning.y = Mathf.Min(cacheEnemyData.pos_y, t - 1f);
					if (this.posWarning.y > CameraController.Instance.camPos.y)
					{
						GameManager.Instance.fxManager.ShowFxWarning(1f, this.posWarning, Fx_Warning.CameraLock.X, 0, null);
					}
					else
					{
						GameManager.Instance.fxManager.ShowFxWarning(1f, this.posWarning, Fx_Warning.CameraLock.X, 4, null);
					}
				}
				else if (cacheEnemyData.pos_x < j)
				{
					this.posWarning.x = j + 0.5f;
					this.posWarning.y = Mathf.Min(cacheEnemyData.pos_y, t - 1f);
					if (this.posWarning.y > CameraController.Instance.camPos.y)
					{
						GameManager.Instance.fxManager.ShowFxWarning(1f, this.posWarning, Fx_Warning.CameraLock.X, 0, null);
					}
					else
					{
						GameManager.Instance.fxManager.ShowFxWarning(1f, this.posWarning, Fx_Warning.CameraLock.X, 3, null);
					}
				}
				else if (cacheEnemyData.pos_y > t)
				{
					this.posWarning.x = cacheEnemyData.pos_x;
					this.posWarning.y = t - 1f;
					GameManager.Instance.fxManager.ShowFxWarning(1f, this.posWarning, Fx_Warning.CameraLock.Y, 0, null);
				}
			}
			yield break;
		}

		public Enemy[] enemies;

		public Action OnClear;

		private int TotalEnemy;

		[Tooltip("Thời gian để giêt enemy ở turn này. Nếu quá thời gian hoặc enemy chêt hêt thì tự động next Turn")]
		public float Time_To_Next_Turn = 10f;

		[Tooltip("bỏ qua thời gian next turn, bắt buộc phải clear hết enemy mới next")]
		public bool ignoreTimeToNext;

		[Tooltip("Thời gian chờ để bắt đầu sinh enemy ở turn này")]
		public float Time_Delay;

		private float time_countdown;

		private bool isInit;

		private Vector3 posWarning;

		private int countEnemyBorn;
	}
}
