using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnEnemy
{
	public class EnemyPoolPvp : MonoSingleton<EnemyPoolPvp>
	{
		private void Start()
		{
			this.InitPool();
		}

		private void InitPool()
		{
			this.EnemyPool = new ObjectPooling<Enemy>(this.ListEnemy.Count, null, null);
			for (int i = 0; i < this.ListEnemy.Count; i++)
			{
				this.EnemyPool.Store(this.ListEnemy[i]);
			}
		}

		public Enemy CreateEnemy()
		{
			Enemy enemy = this.EnemyPool.New();
			if (enemy == null)
			{
				enemy = UnityEngine.Object.Instantiate<Transform>(this.ListEnemy[0].transform).GetComponent<Enemy>();
				enemy.transform.parent = this.ListEnemy[0].transform.parent;
				this.ListEnemy.Add(enemy);
			}
			enemy.gameObject.SetActive(true);
			return enemy;
		}

		private void DestroyEnemy()
		{
			for (int i = 0; i < this.ListEnemy.Count; i++)
			{
				if (this.ListEnemy[i] != null && this.ListEnemy[i].gameObject.activeSelf)
				{
					this.ListEnemy[i].gameObject.SetActive(false);
				}
			}
		}

		private void OnDestroy()
		{
			try
			{
				this.DestroyEnemy();
			}
			catch
			{
			}
		}

		[Header("____ Enemy Pool ____")]
		[SerializeField]
		private List<Enemy> ListEnemy;

		public ObjectPooling<Enemy> EnemyPool;
	}
}
