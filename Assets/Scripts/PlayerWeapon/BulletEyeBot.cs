using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletEyeBot : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			this.Direction = VectorUtils.Rotate(Direction, offsetAngle);
			this.Direction.Normalize();
			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, this.Direction);
			this.counter_time_hide = 0f;
			SingletonGame<AudioController>.Instance.PlaySound(this._audio, 0.2f);
			this.isInit = true;
		}

		public override void OnInitNPC(PlayerMain _player, Vector2 Direction, float Damaged, float Speed)
		{
			this.OnInit(_player, Direction, 0f, true);
			this.Damaged = Damaged;
			this.Speed = Speed;
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
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= 3f)
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
				GameManager.Instance.bulletManager.PoolBulletEyeBot.Store(this);
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
			bool flag = false;
			if (coll.CompareTag("Tank") || coll.CompareTag("Boss"))
			{
				GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
			}
			else
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				if (num <= 0.2f && this.player._PlayerBooster.IsCritical)
				{
					this.Damaged *= 3f;
					flag = true;
				}
			}
			IHealth component = coll.gameObject.GetComponent<IHealth>();
			if (component != null)
			{
				if (flag)
				{
					GameManager.Instance.fxManager.CreateCritical(coll.transform.position);
				}
				component.AddHealthPoint(-this.Damaged, EWeapon.EyeBotBullet);
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
	}
}
