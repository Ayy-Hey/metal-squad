using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletM4A1 : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			if (hasDamage)
			{
				this.ReloadInfor();
			}
			this.isCritical = false;
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			int levelUpgrade = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.LevelUpgrade;
			this.Direction = VectorUtils.Rotate(Direction, offsetAngle);
			this.Direction.Normalize();
			if (Direction == Vector2.zero)
			{
				Direction = new Vector2(1f, 0f);
			}
			int num = levelUpgrade / 10;
			this.bulletScale.SetTexture(this.listTextures[num]);
			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, this.Direction);
			this.counter_time_hide = 0f;
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.15f);
			this.vtScaleTarget = Vector3.one;
			this.transform.localScale = Vector3.one * 0.3f;
			this.posBegin = this.transform.position;
			this.ScaleSpeed = 5f;
			this.isInit = true;
		}

		public override void OnInitNPC(PlayerMain _player, Vector2 Direction, float Damaged, float Speed)
		{
			this.OnInit(_player, Direction, 0f, true);
			this.Damaged = Damaged;
			this.Damaged = Damaged - Damaged * GameManager.Instance.RatePower;
			this.Speed = Speed;
		}

		private void ReloadInfor()
		{
			this.player.GunCurrent.WeaponCurrent.cacheGunProfile.GetTrueDamage(out this.Damaged, out this.isCritical);
		}

		public override void OnUpdate(float deltaTime)
		{
			if (!this.isInit)
			{
				return;
			}
			base.OnUpdate(deltaTime);
			if (GameManager.Instance.StateManager.EState == EGamePlay.PAUSE)
			{
				return;
			}
			if (Vector2.Distance(this.transform.position, this.posBegin) > 1.5f)
			{
				this.transform.localScale = this.vtScaleTarget;
				this.ScaleSpeed = 1f;
			}
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= 1f)
			{
				base.gameObject.SetActive(false);
			}
			this.transform.position += this.Speed * this.ScaleSpeed * Time.deltaTime * (Vector3)this.Direction;
		}

		private void OnDisable()
		{
			this.isInit = false;
			this.Damaged = 0f;
			this.Speed = 0f;
			try
			{
				GameManager.Instance.bulletManager.PoolBulletPlayer1[0].Store(this);
			}
			catch (Exception ex)
			{
			}
		}

		protected void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Obstacle") || coll.CompareTag("Rambo"))
			{
				return;
			}

			GameManager.Instance.fxManager.CreateFxBullet2(2, this.transform.position, 1, 1f, true);
			IHealth component = coll.gameObject.GetComponent<IHealth>();
            Debug.Log(coll.name);
			if (component != null)
			{
				if (this.isCritical)
				{
					GameManager.Instance.fxManager.CreateCritical(this.transform.position);
				}
                Debug.Log("OnTriggerEnter2D bullet dmg " + this.Damaged);
                component.AddHealthPoint(-this.Damaged, EWeapon.M4A1);
			}
			base.gameObject.SetActive(false);
		}

		private Vector2 posBegin;

		[SerializeField]
		private BulletScaleWithTexture bulletScale;

		[SerializeField]
		private Texture[] listTextures;

		private Vector3 vtScaleTarget;

		private float ScaleSpeed = 1f;
	}
}
