using System;
using UnityEngine;

namespace Player
{
	public class CacheGunProfile
	{
		public CacheGunProfile(WeaponProfile profile, PlayerMain _player)
		{
			this.profile = profile;
			this.player = _player;
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				this.CAPACITY = (this.TotalBullet = (int)profile.GetMaxOption(1));
				this.Damage = profile.GetMaxOption(2);
				this.Speed_Bullet = profile.GetMaxOption(0);
				this.Fire_Rate = profile.GetMaxOption(3);
				this.Critical = 0f;
				this.LevelUpgrade = 20;
				this.Time_Reload = 0.2f;
			}
			else
			{
				this.CAPACITY = (this.TotalBullet = (int)profile.GetOption(1));
				this.Damage = profile.GetOption(2);
				this.Speed_Bullet = profile.GetOption(0);
				this.Fire_Rate = profile.GetOption(3);
				this.Critical = profile.Get_Critical;
				this.LevelUpgrade = profile.GetLevelUpgrade();
				this.Time_Reload = profile.Get_Time_Reload;
			}
		}

		public void OnResetBullet()
		{
			this.CAPACITY = (this.TotalBullet = (int)this.profile.GetOption(1));
		}

		public void GetTrueDamage(out float _damage, out bool isCrit)
		{
			isCrit = false;
			float num = this.Damage;
			float num2 = UnityEngine.Random.Range(0f, 1f);
			if (num2 <= this.Critical)
			{
				isCrit = true;
				num *= 3f;
			}
			_damage = num;
		}

		public int RankUpgrade
		{
			get
			{
				return this.profile.GetRankUpped();
			}
		}

		public float Damage;

		public float Speed_Bullet;

		public float Fire_Rate;

		public float Critical;

		public float Time_Reload;

		public int LevelUpgrade;

		public int TotalBullet;

		public WeaponProfile profile;

		public int CAPACITY;

		public PlayerMain player;
	}
}
