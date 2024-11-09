using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletIce : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			this.isCritical = false;
			if (hasDamage)
			{
				this.ReloadInfor();
			}
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.7f);
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			int levelUpgrade = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.LevelUpgrade;
			this.Direction = VectorUtils.Rotate(Direction, offsetAngle);
			this.Direction.Normalize();
			if (Direction == Vector2.zero)
			{
				Direction = new Vector2(1f, 0f);
			}
			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, this.Direction);
			this.counter_time_hide = 0f;
			this.transform.localScale = Vector3.one * 0.6f;
			this.posBegin = this.transform.position;
			int num = levelUpgrade / 10;
			this.bulletScale.SetTexture(this.textures[num]);
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				this.Speed = 8f;
				this.Damaged = 50f;
			}
			this.isInit = true;
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
			if (Vector2.Distance(this.transform.position, this.posBegin) > 0.5f)
			{
				Vector3 localScale = this.transform.localScale;
				float num = Time.deltaTime * 10f;
				localScale.x += num;
				localScale.y += num;
				this.transform.localScale = new Vector3(Mathf.Clamp01(localScale.x), Mathf.Clamp01(localScale.y), 1f);
			}
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= 1f)
			{
				base.gameObject.SetActive(false);
			}
			this.transform.position += this.Speed * Time.deltaTime * (Vector3)this.Direction;
		}

		private void OnDisable()
		{
			this.isInit = false;
			this.Damaged = 0f;
			this.Speed = 0f;
			try
			{
				GameManager.Instance.bulletManager.PoolBulletPlayer1[2].Store(this);
			}
			catch (Exception ex)
			{
			}
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Obstacle") || coll.CompareTag("Rambo") || coll.CompareTag("platform"))
			{
				return;
			}
			if (coll.CompareTag("Enemy"))
			{
				IIce component = coll.GetComponent<IIce>();
				if (component != null)
				{
					if (this.isCritical)
					{
						GameManager.Instance.fxManager.CreateCritical(this.transform.position);
					}
					component.Hit(-this.Damaged, EWeapon.ICE);
				}
				else
				{
					IHealth component2 = coll.gameObject.GetComponent<IHealth>();
					if (component2 != null)
					{
						if (this.isCritical)
						{
							GameManager.Instance.fxManager.CreateCritical(this.transform.position);
						}
						component2.AddHealthPoint(-this.Damaged, EWeapon.ICE);
					}
				}
			}
			else
			{
				IHealth component3 = coll.gameObject.GetComponent<IHealth>();
				if (component3 != null)
				{
					if (this.isCritical)
					{
						GameManager.Instance.fxManager.CreateCritical(this.transform.position);
					}
					component3.AddHealthPoint(-this.Damaged, EWeapon.ICE);
				}
			}
			GameManager.Instance.fxManager.CreateFxBullet2(1, this.transform.position, 1, 1f, true);
			base.gameObject.SetActive(false);
		}

		private void OnBecameInvisible()
		{
			base.gameObject.SetActive(false);
		}

		private Vector2 posBegin;

		[SerializeField]
		private BulletScaleWithTexture bulletScale;

		[SerializeField]
		private Texture[] textures;
	}
}
