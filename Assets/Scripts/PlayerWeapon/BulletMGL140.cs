using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletMGL140 : BaseBullet
	{
		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			this.isCritical = false;
			if (hasDamage)
			{
				this.ReloadInfor();
			}
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.25f);
			this.Speed = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Speed_Bullet;
			this.Direction = Direction;
			this.Direction.Normalize();
			if (Direction == Vector2.zero)
			{
				Direction = new Vector2(1f, 0f);
			}
			int rankUpgrade = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade;
			for (int i = 0; i < this.bullets.Length; i++)
			{
				this.bullets[i].SetActive(rankUpgrade == i);
			}
			this.transform.rotation = Quaternion.identity;
			Quaternion rotation = Quaternion.LookRotation(Direction, -Vector3.forward);
			rotation.x = 0f;
			rotation.y = 0f;
			rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
			this.transform.rotation = rotation;
			this.counter_time_hide = 0f;
			this.curveId = UnityEngine.Random.Range(0, this.curves.Length);
			this.transform.localScale = Vector3.one * 0.6f;
			this.posBegin = this.transform.position;
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
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= 1f)
			{
				base.gameObject.SetActive(false);
			}
			if (Vector2.Distance(this.transform.position, this.posBegin) > 0.5f)
			{
				Vector3 localScale = this.transform.localScale;
				float num = Time.deltaTime * 10f;
				localScale.x += num;
				localScale.y += num;
				this.transform.localScale = new Vector3(Mathf.Clamp01(localScale.x), Mathf.Clamp01(localScale.y), 1f);
			}
			this.transform.position += this.Speed * Time.deltaTime * (Vector3)this.Direction;
			this.localPosChild = this.tfChild.localPosition;
			this.localPosChild.x = this.curves[this.curveId].Evaluate(this.counter_time_hide);
			this.tfChild.localPosition = this.localPosChild;
			if (!CameraController.Instance.IsInView(this.transform))
			{
				base.gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
			Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 1.5f, this.layermask);
			for (int i = 0; i < array.Length; i++)
			{
				BaseEnemy component = array[i].GetComponent<BaseEnemy>();
				if (component != null && component.isInCamera)
				{
					if (this.isCritical)
					{
						GameManager.Instance.fxManager.CreateCritical(component.GetPosition());
					}
					component.AddHealthPoint(-this.Damaged, EWeapon.MGL140);
				}
			}
			this.isInit = false;
			this.Damaged = 0f;
			this.Speed = 0f;
			try
			{
				GameManager.Instance.bulletManager.PoolBulletPlayer1[4].Store(this);
			}
			catch (Exception ex)
			{
			}
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Enemy") || coll.CompareTag("Tank") || coll.CompareTag("Boss") || coll.CompareTag("Ground"))
			{
				base.gameObject.SetActive(false);
			}
		}

		[SerializeField]
		private GameObject[] bullets;

		[SerializeField]
		private Animator anim;

		[SerializeField]
		private LayerMask layermask;

		[SerializeField]
		private Transform tfChild;

		[SerializeField]
		private AnimationCurve[] curves;

		private const float RADIUS_DAMAGED = 1.5f;

		private Vector2 posBegin;

		private int curveId;

		private Vector2 localPosChild;
	}
}
