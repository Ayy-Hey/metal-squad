using System;
using UnityEngine;

namespace SpawnEnemy
{
	public class EnemyPvp : MonoBehaviour
	{
		public ETypeEnemy Type
		{
			get
			{
				return this.Types[UnityEngine.Random.Range(0, this.Types.Length)];
			}
		}

		public ETypeEnemy[] Types;

		[Tooltip("Check nếu là enemy nhảy dù")]
		public bool isParachute;

		[Tooltip("Set di chuyển hoặc đứng yên")]
		public bool isMove = true;
	}
}
