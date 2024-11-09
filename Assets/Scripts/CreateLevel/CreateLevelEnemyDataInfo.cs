using System;
using UnityEngine;

namespace CreateLevel
{
	[ExecuteInEditMode]
	public class CreateLevelEnemyDataInfo : MonoBehaviour
	{
		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				return;
			}
			base.name = this.type.ToString() + "_" + this.level;
		}

		private void Awake()
		{
			if (Application.isPlaying)
			{
				return;
			}
			try
			{
				base.transform.parent.GetComponent<CreateLevelEnemy>().AddEnemy(this);
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
				base.transform.parent.GetComponent<CreateLevelEnemy>().RemoveEnemy(this);
			}
			catch
			{
			}
		}

		public void SetEnemydataInfo()
		{
			this.enemyDataInfo = new EnemyDataInfo();
			this.enemyDataInfo.type = this.type;
			this.enemyDataInfo.isMove = this.isMove;
			this.enemyDataInfo.level = this.level;
			this.enemyDataInfo.pos_x = base.transform.position.x;
			this.enemyDataInfo.pos_y = base.transform.position.y;
		}

		public void SetEnemyDataInfo(int type, bool isMove, int level, float posX, float posY)
		{
			this.type = (ETypeEnemy)type;
			this.isMove = isMove;
			this.level = level;
			base.transform.position = new Vector3(posX, posY);
			base.name = this.type.ToString() + "_" + level;
		}

		public ETypeEnemy type;

		public bool isMove;

		public int level;

		public EnemyDataInfo enemyDataInfo;
	}
}
