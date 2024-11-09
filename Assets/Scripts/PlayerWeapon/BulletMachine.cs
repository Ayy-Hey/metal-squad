using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletMachine : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			this.isCritical = false;
			if (hasDamage)
			{
				this.ReloadInfor();
			}
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			int levelUpgrade = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.LevelUpgrade;
			this.Direction = VectorUtils.Rotate(Direction, offsetAngle);
			this.Direction.Normalize();
			if (Direction == Vector2.zero)
			{
				Direction = new Vector2(1f, 0f);
			}
			int rankUpgrade = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade;
			this.bulletScale.SetTexture(this.listTextures[rankUpgrade]);
			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, this.Direction);
			this.counter_time_hide = 0f;
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.3f);
			this.vtScaleTarget = Vector3.one;
			this.posBegin = this.transform.position;
			this.transform.localScale = Vector3.one * 0.6f;
			this.isInit = true;
		}

		private void ReloadInfor()
		{
			this.player.GunCurrent.WeaponCurrent.cacheGunProfile.GetTrueDamage(out this.Damaged, out this.isCritical);
			this.Damaged /= 2f;
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
			if (Vector2.Distance(this.transform.position, this.posBegin) > 1f)
			{
				this.transform.localScale = this.vtScaleTarget;
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
				GameManager.Instance.bulletManager.PoolBulletPlayer1[1].Store(this);
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
			if (coll.CompareTag("Tank") || coll.CompareTag("Boss"))
			{
				GameManager.Instance.fxManager.CreateFxBullet(this.transform.position);
			}
			IHealth component = coll.gameObject.GetComponent<IHealth>();
			if (component != null)
			{
				if (this.isCritical)
				{
					GameManager.Instance.fxManager.CreateCritical(this.transform.position);
				}
				component.AddHealthPoint(-this.Damaged, EWeapon.MACHINE);
				if (!coll.CompareTag("Tank") && !coll.CompareTag("Boss"))
				{
					GameManager.Instance.fxManager.ShowEffect(8, this.transform.position, Vector3.one, true, true);
				}
			}
			base.gameObject.SetActive(false);
		}

		private void OnBecameInvisible()
		{
			base.gameObject.SetActive(false);
		}

		private Vector2 posBegin;

		private Vector3 vtScaleTarget;

		[SerializeField]
		private BulletScaleWithTexture bulletScale;

		[SerializeField]
		private Texture[] listTextures;
	}
}
