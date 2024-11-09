using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletFcPath2 : CachingMonoBehaviour
	{
		public void Init(PlayerMain _player, Vector3 pos, Vector2 direction, float angle, Collider2D igroneColl, Action callback, bool hasDamage = true)
		{
			this.player = _player;
			base.gameObject.SetActive(true);
			this.callback = callback;
			this.igroneColl = igroneColl;
			this.counter_time_hide = 0f;
			if (this.customBulletSpriteMat)
			{
				this.customBulletSpriteMat.SetMaterial();
			}
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			if (hasDamage)
			{
				this.player.GunCurrent.WeaponCurrent.cacheGunProfile.GetTrueDamage(out this.Damaged, out this.isCrit);
			}
			else
			{
				this.Damaged = 0f;
			}
			this.maxHide = 0.3f + (float)this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade * 0.1f;
			this.transform.position = pos;
			this.Direction = direction.normalized;
			Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.up, this.Direction).eulerAngles;
			eulerAngles.z += angle;
			this.transform.eulerAngles = eulerAngles;
			this.isInit = true;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.isInit)
			{
				return;
			}
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= this.maxHide)
			{
				base.gameObject.SetActive(false);
			}
			this.transform.Translate(this.Speed * deltaTime * this.transform.up, Space.World);
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Obstacle") || coll.CompareTag("Rambo"))
			{
				return;
			}
			if (this.igroneColl && this.igroneColl == coll)
			{
				return;
			}
			GameManager.Instance.fxManager.CreateFxBullet2(2, this.transform.position, 1, 1f, true);
			try
			{
				coll.GetComponent<IHealth>().AddHealthPoint(-this.Damaged, EWeapon.FC_10);
			}
			catch
			{
			}
			base.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			this.isInit = false;
			this.Damaged = 0f;
			this.Speed = 0f;
			try
			{
				this.callback();
			}
			catch
			{
			}
		}

		[SerializeField]
		private CustomBulletSpriteMaterial customBulletSpriteMat;

		[HideInInspector]
		public bool isInit;

		private Action callback;

		private Collider2D igroneColl;

		private bool isCrit;

		private float Speed;

		private float Damaged;

		private Vector2 Direction;

		private float counter_time_hide;

		private float maxHide;

		private PlayerMain player;
	}
}
