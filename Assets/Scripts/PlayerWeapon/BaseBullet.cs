using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BaseBullet : CachingMonoBehaviour
	{
		public void TryAwake()
		{
			if (this.transform == null)
			{
				this.transform = base.GetComponent<Transform>();
			}
			try
			{
				if (this.rigidbody2D == null)
				{
					this.rigidbody2D = base.GetComponent<Rigidbody2D>();
				}
			}
			catch
			{
				UnityEngine.Debug.Log("Rigidbody null");
			}
		}

		public void OnPause()
		{
			if (this.rigidbody2D != null)
			{
				this.rigidbody2D.isKinematic = true;
				this.savedVelocity = this.rigidbody2D.velocity;
				this.rigidbody2D.velocity = Vector2.zero;
			}
		}

		public void OnResume()
		{
			if (this.rigidbody2D != null)
			{
				this.rigidbody2D.isKinematic = false;
				this.rigidbody2D.velocity = this.savedVelocity;
			}
		}

		public virtual void OnInit(PlayerMain player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
		}

		public virtual void OnUpdate(float deltaTime)
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.PREVIEW || GameManager.Instance.StateManager.EState == EGamePlay.LOST || GameManager.Instance.StateManager.EState == EGamePlay.WAITING_BOSS || GameManager.Instance.StateManager.EState == EGamePlay.WIN)
			{
				base.gameObject.SetActive(false);
			}
		}

		public virtual void OnInitNPC(PlayerMain player, Vector2 Direction, float Damaged, float Speed)
		{
		}

		public virtual void OnInitRocket1(PlayerMain player, BaseEnemy target, Quaternion quaternion, float speed, float Damage, bool isCritical)
		{
			this.isCritical = isCritical;
		}

		public virtual void OnInitRocket2(PlayerMain player, Vector3 target, Quaternion quaternion, float speed, float Damage, bool isCritical)
		{
			this.isCritical = isCritical;
		}

		public float Damaged;

		protected float Speed;

		protected Vector2 Direction;

		protected Vector2 savedVelocity;

		protected bool isInit;

		protected float counter_time_hide;

		protected bool isCritical;

		public AudioClip _audio;

		public PlayerMain player;
	}
}
