using System;
using UnityEngine;

namespace SpawnEnemy
{
	public class Enemy : MonoBehaviour
	{
		public bool DropCoin { get; set; }

		public int ValueCoin { get; set; }

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				return;
			}
			base.name = this.Type.ToString() + "_" + this.Level;
		}

		public ETypeEnemy Type;

		public int Level;

		[Tooltip("Thời gian chờ để bắt đầu sinh enemy này. Sau khi turn start")]
		public float TimeDelay;

		[Tooltip("Check nếu là enemy nhảy dù")]
		public bool isParachute;

		[Tooltip("Set di chuyển hoặc đứng yên")]
		public bool isMove = true;
	}
}
