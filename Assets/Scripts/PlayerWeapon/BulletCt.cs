using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletCt : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			base.gameObject.SetActive(true);
			if (hasDamage)
			{
				this.ReloadInfor();
			}
			this.childScale = 0.1f;
			this.Direction = Direction.normalized;
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			this.transform.rotation = Quaternion.FromToRotation(Vector3.up, Direction);
			this.pingpongRange = 1f;
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.7f);
			this.tfChild.transform.localScale = Vector3.one * this.childScale;
			this.tfChild.transform.rotation = Quaternion.identity;
			int rankUpgrade = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade;
			if (rankUpgrade != 0)
			{
				if (rankUpgrade != 1)
				{
					if (rankUpgrade == 2)
					{
						this.unitDamages[0].Init(new Action<Vector3, bool>(this.OnAttack), true);
						this.unitDamages[1].Init(new Action<Vector3, bool>(this.OnAttack), false);
						this.unitDamages[2].Init(new Action<Vector3, bool>(this.OnAttack), false);
					}
				}
				else
				{
					this.unitDamages[0].Init(new Action<Vector3, bool>(this.OnAttack), true);
					this.unitDamages[1].Init(new Action<Vector3, bool>(this.OnAttack), false);
				}
			}
			else
			{
				this.unitDamages[0].Init(new Action<Vector3, bool>(this.OnAttack), true);
			}
			this.counter_time_hide = 0f;
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
			if (this.childScale < 0.99f)
			{
				this.childScale = Mathf.SmoothDamp(this.childScale, 1f, ref this.veloChildScale, 0.2f);
				if (this.childScale >= 0.99f)
				{
					this.childScale = 1f;
				}
				this.tfChild.transform.localScale = this.childScale * Vector3.one;
			}
			this.time += deltaTime * 2f;
			float t = Mathf.PingPong(this.time, 1f);
			float num = Mathf.SmoothStep(-this.pingpongRange, this.pingpongRange, t);
			if (this.unitDamages[1].isInit)
			{
				this.unitPos = this.unitDamages[1].transform.localPosition;
				this.unitPos.y = num;
				this.unitDamages[1].transform.localPosition = this.unitPos;
			}
			if (this.unitDamages[2].isInit)
			{
				this.unitPos = this.unitDamages[2].transform.localPosition;
				this.unitPos.y = -num;
				this.unitDamages[2].transform.localPosition = this.unitPos;
			}
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= 1f)
			{
				base.gameObject.SetActive(false);
				GameManager.Instance.fxManager.CreateFxBullet2(1, this.transform.position, 0, 2f, true);
				GameManager.Instance.fxManager.CreateFxNoMainCt(this.unitDamages[0].transform.position, 1f);
			}
			this.transform.Translate(this.Speed * deltaTime * this.Direction, Space.World);
		}

		private void OnAttack(Vector3 point, bool isMain)
		{
			float radius;
			float num;
			if (isMain)
			{
				radius = 1.5f;
				num = this.Damaged;
				GameManager.Instance.fxManager.CreateFxNoMainCt(point, 1f);
			}
			else
			{
				radius = 1f;
				num = this.Damaged * 2f / 3f;
			}
			Collider2D[] array = Physics2D.OverlapCircleAll(point, radius, this.maskEnemy);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					try
					{
						array[i].GetComponent<IHealth>().AddHealthPoint(-num, EWeapon.CT_9);
					}
					catch
					{
					}
				}
			}
			for (int j = 0; j < this.unitDamages.Length; j++)
			{
				if (this.unitDamages[j].isInit)
				{
					return;
				}
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
				GameManager.Instance.bulletManager.PoolBulletPlayer1[5].Store(this);
			}
			catch (Exception ex)
			{
			}
		}

		[SerializeField]
		private Transform tfChild;

		[SerializeField]
		private float childSpeedRotate;

		[SerializeField]
		private BulletCt_UnitDamge[] unitDamages;

		[SerializeField]
		private LayerMask maskEnemy;

		private float childScale;

		private float veloChildScale;

		private float time;

		private Vector2 unitPos;

		private const float Radis_Damage = 1.5f;

		private float pingpongRange;
	}
}
