using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreateLevel
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(CreateLevelEnemy))]
	public class CreateLevelPoint : MonoBehaviour
	{
		public void SetPoint(float posX, float posY, bool isUnlocked)
		{
			this.checkpoint_pos_x = posX;
			this.checkpoint_pos_y = posY;
			this.isUnlocked = isUnlocked;
			base.transform.position = new Vector3(posX, posY);
		}

		public void GetPos()
		{
			this.checkpoint_pos_x = base.transform.position.x;
			this.checkpoint_pos_y = base.transform.position.y;
			this.enemydata.enemyData = new EnemyData();
			this.enemydata.enemyData.enemyDataInfo = new List<EnemyDataInfo>();
			for (int i = 0; i < this.enemydata.enemyDataInfo.Count; i++)
			{
				if (this.enemydata.enemyDataInfo[i])
				{
					this.enemydata.enemyDataInfo[i].SetEnemydataInfo();
					this.enemydata.enemyData.enemyDataInfo.Add(this.enemydata.enemyDataInfo[i].enemyDataInfo);
				}
			}
			this.point = new CheckPoint();
			this.point.checkpoint_pos_x = this.checkpoint_pos_x;
			this.point.checkpoint_pos_y = this.checkpoint_pos_y;
			this.point.isUnlocked = this.isUnlocked;
			this.point.enemyData = this.enemydata.enemyData;
		}

		private void Awake()
		{
			if (Application.isPlaying)
			{
				return;
			}
			try
			{
				base.transform.parent.GetComponent<CreateLevelLevel>().AddPoint(this);
			}
			catch
			{
			}
		}

		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				return;
			}
			try
			{
				base.transform.parent.GetComponent<CreateLevelLevel>().RemovePoint(this);
			}
			catch
			{
			}
		}

		private void Reset()
		{
			this.enemydata = base.GetComponent<CreateLevelEnemy>();
		}

		public float checkpoint_pos_x;

		public float checkpoint_pos_y;

		public bool isUnlocked;

		public CreateLevelEnemy enemydata;

		public CheckPoint point;
	}
}
