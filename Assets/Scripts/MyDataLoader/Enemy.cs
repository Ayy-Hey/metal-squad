using System;
using UnityEngine;

namespace MyDataLoader
{
	public class Enemy : ICloneable
	{
		public void Vision()
		{
			this.CacheVision = UnityEngine.Random.Range(this.Vision_Min, this.Vision_Max);
		}

		public object Clone()
		{
			return base.MemberwiseClone();
		}

		public int Level;

		public float HP;

		public float Damage;

		public float Speed;

		public float Speed_Bullet;

		public float Time_Reload_Attack;

		public float Vision_Min;

		public float Vision_Max;

		public float Radius_Damage;

		public float Distance_Shoot;

		public int Number_Bomb;

		public float Distance_Bomb;

		public bool Gift;

		public int Gift_Value;

		public float DamageLv2;

		public float Speed_BulletLv2;

		public float Time_Reload_AttackLv2;

		public float CacheVision;
	}
}
