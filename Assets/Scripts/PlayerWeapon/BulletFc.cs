using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletFc : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			base.gameObject.SetActive(true);
			if (hasDamage)
			{
				this.ReloadInfor();
			}
			this.Direction = Direction.normalized;
			if (this.customBulletSpriteMat)
			{
				this.customBulletSpriteMat.SetMaterial();
			}
			this.rank = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade;
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			this.transform.rotation = Quaternion.FromToRotation(Vector3.up, Direction);
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.7f);
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
			this.counter_time_hide += deltaTime;
			Vector3 position = this.transform.position;
			bool flag = position.x >= CameraController.Instance.viewPos.maxX || position.x <= CameraController.Instance.viewPos.minX || position.y >= CameraController.Instance.viewPos.maxY || position.y <= CameraController.Instance.viewPos.minY;
			if (this.counter_time_hide > 1.2f || flag)
			{
				this.Attack(null);
			}
			if (this.counter_time_hide >= 1f)
			{
				base.gameObject.SetActive(false);
			}
			this.transform.Translate(this.Speed * deltaTime * this.Direction, Space.World);
		}

		private void Attack(Collider2D coll)
		{
			GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
			Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 1.5f, this.maskEnemy);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					try
					{
						array[i].GetComponent<IHealth>().AddHealthPoint(-this.Damaged, EWeapon.FC_10);
					}
					catch
					{
					}
				}
			}
			base.gameObject.SetActive(false);
			SingletonGame<AudioController>.Instance.PlaySound_P(this.clipNo, 1f);
			Vector3 position = this.transform.position;
			int num = this.rank;
			if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						float num2 = -30f;
						for (int j = 0; j < 6; j++)
						{
							GameManager.Instance.bulletManager.CreateBulletFcPath2(this.player, position, this.Direction, num2, coll, !this.player.IsRemotePlayer);
							num2 += 60f;
						}
					}
				}
				else
				{
					float num2 = 0f;
					for (int k = 0; k < 5; k++)
					{
						GameManager.Instance.bulletManager.CreateBulletFcPath2(this.player, position, this.Direction, num2, coll, !this.player.IsRemotePlayer);
						num2 += 72f;
					}
				}
			}
			else
			{
				float num2 = -45f;
				for (int l = 0; l < 4; l++)
				{
					GameManager.Instance.bulletManager.CreateBulletFcPath2(this.player, position, this.Direction, num2, coll, !this.player.IsRemotePlayer);
					num2 += 90f;
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Obstacle") || coll.CompareTag("Rambo"))
			{
				return;
			}
			this.Attack(coll);
		}

		private void OnDisable()
		{
			this.isInit = false;
			this.Damaged = 0f;
			this.Speed = 0f;
			try
			{
				GameManager.Instance.bulletManager.PoolBulletPlayer2[3].Store(this);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Exception : " + ex.Message);
			}
		}

		[SerializeField]
		private CustomBulletSpriteMaterial customBulletSpriteMat;

		[SerializeField]
		private AudioClip clipNo;

		[SerializeField]
		private LayerMask maskEnemy;

		private int rank;

		private const float Radis_Damage = 1.5f;
	}
}
