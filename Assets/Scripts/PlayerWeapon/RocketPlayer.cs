using System;
using System.Collections;
using UnityEngine;

namespace PlayerWeapon
{
	public class RocketPlayer : BaseBullet
	{
		private void ExplosiveBullet()
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode && this.syncRocketPlayer != null)
			{
				if (this.syncRocketPlayer.IsRemote)
				{
					return;
				}
				this.syncRocketPlayer.SendRpc_ExplosiveBullet();
			}
			base.gameObject.SetActive(false);
			this.isInit = false;
			this.isFire = false;
			this.hasTarget = false;
			this.ShowEffectExplosiveBullet();
			Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 1.5f, this.layerMask);
			for (int i = 0; i < array.Length; i++)
			{
				BaseEnemy component = array[i].GetComponent<BaseEnemy>();
				if (component != null && component.isInCamera)
				{
					if (this.isCritical)
					{
						GameManager.Instance.fxManager.CreateCritical(component.GetPosition());
					}
					component.AddHealthPoint(-this.Damaged, EWeapon.ROCKET);
				}
			}
			try
			{
				GameManager.Instance.bulletManager.PoolBulletPlayer2[4].Store(this);
			}
			catch (Exception ex)
			{
			}
			this.isCritical = false;
		}

		protected void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.gameObject.layer == 19)
			{
				return;
			}
			this.ExplosiveBullet();
		}

		public void ShowEffectExplosiveBullet()
		{
			if (ProfileManager.weaponsSpecial[4].GetLevelUpgrade() < 4)
			{
				GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
			}
			else
			{
				GameManager.Instance.fxManager.ShowExplosionSpine(this.transform.position, 0);
			}
			this.spriteRenderer.gameObject.SetActive(false);
			this.boxCollider2D.enabled = false;
			this.rigidbody2D.isKinematic = true;
		}

		private IEnumerator Hide()
		{
			yield return this.timehide;
			yield break;
		}

		public override void OnInitRocket1(PlayerMain _player, BaseEnemy target, Quaternion quaternion, float speed, float Damage, bool isCritical)
		{
			this.player = _player;
			base.OnInitRocket1(this.player, target, quaternion, speed, Damage, isCritical);
			this.isFire = true;
			this.types = RocketPlayer.Types.Type_1;
			this.Damaged = Damage;
			this.target = target;
			this.speed = speed;
			this.transform.rotation = quaternion;
			this.transform.localScale = Vector3.one * 0.6f;
			this.posBegin = this.transform.position;
			for (int i = 0; i < this.bullets.Length; i++)
			{
				this.bullets[i].SetActive(false);
			}
			this.bullets[this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade].SetActive(true);
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.3f);
		}

		public override void OnInitRocket2(PlayerMain _player, Vector3 target, Quaternion quaternion, float speed, float Damage, bool isCritical)
		{
			this.player = _player;
			base.OnInitRocket2(this.player, target, quaternion, speed, Damage, isCritical);
			this.isFire = true;
			this.types = RocketPlayer.Types.Type_2;
			this.Damaged = Damage;
			this.vtTarget = target;
			this.speed = speed;
			this.transform.rotation = quaternion;
			this.transform.localScale = Vector3.one * 0.3f;
			this.posBegin = this.transform.position;
			for (int i = 0; i < this.bullets.Length; i++)
			{
				this.bullets[i].SetActive(false);
			}
			this.bullets[this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade].SetActive(true);
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 0.3f);
		}

		public override void OnInit(PlayerMain _player, Vector2 Direction, float offsetAngle, bool hasDamage = true)
		{
			this.player = _player;
			this.last_turn = 0f;
			this.turn = 2.5f;
			this.isFire = false;
			this.hasTarget = true;
			this.boxCollider2D.enabled = true;
			this.rigidbody2D.isKinematic = false;
			this.spriteRenderer.gameObject.SetActive(true);
			this.LastTimeStuck = Time.timeSinceLevelLoad;
			this.counter_time_hide = 0f;
			this.transform.localScale = Vector3.one * 0.3f;
			this.posBegin = this.transform.position;
		}

		public override void OnUpdate(float deltaTime)
		{
			if (!this.isFire)
			{
				return;
			}
			base.OnUpdate(deltaTime);
			if (!CameraController.Instance.IsInView(this.transform))
			{
				this.ExplosiveBullet();
				return;
			}
			this.counter_time_hide += deltaTime;
			if (this.counter_time_hide >= 3f)
			{
				this.ExplosiveBullet();
				this.counter_time_hide = 0f;
				return;
			}
			if (Vector2.Distance(this.transform.position, this.posBegin) > 0.5f)
			{
				this.transform.localScale = Vector3.one;
			}
			if (Time.timeSinceLevelLoad - this.LastTimeStuck >= 0.2f)
			{
				this.LastTimeStuck = Time.timeSinceLevelLoad;
				bool flag = this.StuckBound.Contains(this.transform.position);
				Vector2 position = this.transform.position;
				position.x -= 0.1f;
				position.y -= 0.1f;
				this.StuckBound = new Rect(position, new Vector2(0.2f, 0.2f));
				if (flag)
				{
					this.ExplosiveBullet();
					return;
				}
			}
			RocketPlayer.Types types = this.types;
			if (types != RocketPlayer.Types.Type_1)
			{
				if (types == RocketPlayer.Types.Type_2)
				{
					if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.vtTarget.x, this.vtTarget.y)) < 0.2f && this.hasTarget)
					{
						this.hasTarget = false;
					}
					if (this.hasTarget)
					{
						Quaternion b = Quaternion.LookRotation(this.transform.position - this.vtTarget, Vector3.forward);
						b.x = 0f;
						b.y = 0f;
						this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b, deltaTime * this.turn);
						this.last_turn += deltaTime * deltaTime * 50f;
						this.turn += this.last_turn;
					}
					this.rigidbody2D.velocity = this.transform.up * this.speed;
				}
			}
			else
			{
				if (this.target.isInit && this.target.HP > 0f)
				{
					Quaternion b2 = Quaternion.LookRotation(this.transform.position - this.target.Origin3(), Vector3.forward);
					b2.x = 0f;
					b2.y = 0f;
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b2, deltaTime * this.turn);
					this.last_turn += deltaTime * deltaTime * 50f;
					this.turn += this.last_turn;
				}
				else if (GameManager.Instance.ListEnemy.Count > 0)
				{
					int index = 0;
					float num = Vector3.Distance(this.transform.position, GameManager.Instance.ListEnemy[0].Origin3());
					for (int i = 1; i < GameManager.Instance.ListEnemy.Count; i++)
					{
						float num2 = Vector3.Distance(this.transform.position, GameManager.Instance.ListEnemy[i].Origin3());
						if (num2 < num)
						{
							index = i;
							num = num2;
						}
						this.target = GameManager.Instance.ListEnemy[index];
					}
				}
				this.rigidbody2D.velocity = this.transform.up * this.speed;
			}
		}

		private RocketPlayer.Types types;

		private float speed;

		private float turn = 2.5f;

		private float last_turn;

		private bool isFire;

		private bool hasTarget;

		private BaseEnemy target;

		private Vector3 vtTarget;

		private WaitForSeconds timehide = new WaitForSeconds(0.5f);

		private float LastTimeStuck;

		private Rect StuckBound;

		private const float STUCK_BOUND_WIDTH = 0.2f;

		private const float STUCK_BOUND_HEIGHT = 0.2f;

		private const float CHECK_STUCK_DELTA = 0.2f;

		public GameObject[] bullets;

		public SpriteRenderer spriteRenderer;

		public BoxCollider2D boxCollider2D;

		private Vector2 posBegin;

		[SerializeField]
		private LayerMask layerMask;

		public GameObject duoiTenLua;

		public SyncRocketPlayer syncRocketPlayer;

		public enum Types
		{
			Type_1,
			Type_2
		}
	}
}
