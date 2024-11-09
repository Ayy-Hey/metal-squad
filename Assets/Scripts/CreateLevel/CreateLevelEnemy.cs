using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreateLevel
{
	[ExecuteInEditMode]
	public class CreateLevelEnemy : MonoBehaviour
	{
		public void AddEnemy(CreateLevelEnemyDataInfo enemyInfo)
		{
			if (this.enemyDataInfo == null)
			{
				this.enemyDataInfo = new List<CreateLevelEnemyDataInfo>();
			}
			if (!this.enemyDataInfo.Contains(enemyInfo))
			{
				this.enemyDataInfo.Add(enemyInfo);
			}
		}

		public void RemoveEnemy(CreateLevelEnemyDataInfo enemy)
		{
			if (this.enemyDataInfo.Contains(enemy))
			{
				this.enemyDataInfo.Remove(enemy);
			}
		}

		public List<CreateLevelEnemyDataInfo> enemyDataInfo;

		public EnemyData enemyData;
	}
}
