using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyRocket : BaseEnemy2
{
	private void OnValidate()
	{
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
		if (!this.skeletonAnimation)
		{
			this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
		}
		if (!this.meshRenderer)
		{
			this.meshRenderer = base.GetComponentInChildren<MeshRenderer>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.CompareTag("Found_Gulf") || collision.CompareTag("Found_Wall")) && this.isMove)
		{
			base.Flip = !base.Flip;
			float num = UnityEngine.Random.Range(1f, 3f);
			this._targetX = ((!base.Flip) ? (this.pos.x + num) : (this.pos.x - num));
		}
		if (collision.CompareTag("Found_Jump"))
		{
			if (this._state == EnemyRocket.EState.Parachute)
			{
				base.Gravity = 2f;
			}
			this._state = EnemyRocket.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (this._state)
		{
		case EnemyRocket.EState.Jump3:
			this.rigidbody2D.velocity = Vector2.zero;
			this.ChangeState();
			break;
		case EnemyRocket.EState.Parachute:
			base.Gravity = 2f;
			this.ChangeState();
			break;
		case EnemyRocket.EState.Walk1:
		case EnemyRocket.EState.Walk2:
			if (collision.contacts != null && collision.contacts[0].normal.y < 0.5f)
			{
				float num = UnityEngine.Random.Range(2f, 4f);
				this._targetX = ((!base.Flip) ? (this.pos.x - num) : (this.pos.x + num));
				base.Flip = !base.Flip;
			}
			break;
		}
	}

	public override void Init(EnemyDataInfo enemyDataInfo, Action hideCallback)
	{
		base.Init(enemyDataInfo, hideCallback);
		this.sitCollider.enabled = false;
		if (this._boneTarget == null)
		{
			this._boneTarget = this.skeletonAnimation.Skeleton.FindBone(this.strBoneTarget);
			this._boneGuntip1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip1);
		}
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemyRocket.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyRocket.EState.Walk1;
			this.ChangeState();
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}

	protected override void StartState()
	{
		base.StartState();
		bool loop = false;
		int track = 0;
		EnemyRocket.EState state = this._state;
		switch (state)
		{
		case EnemyRocket.EState.Idle:
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyRocket.EState.Idle_Walk:
			this.sitCollider.enabled = true;
			this.bodyCollider2D.enabled = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyRocket.EState.Jump1:
		{
			float d = Mathf.Sqrt(Mathf.Abs(5f * this.rigidbody2D.gravityScale * Physics2D.gravity.y));
			this.rigidbody2D.velocity = Vector2.up * d;
			break;
		}
		default:
			if (state == EnemyRocket.EState.Aim_Target)
			{
				track = 1;
				base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			}
			break;
		case EnemyRocket.EState.Walk1:
		case EnemyRocket.EState.Walk2:
			loop = true;
			this.isMove = true;
			base.Flip = (this.pos.x > this._targetX);
			break;
		}
		base.PlayAnim((int)this._state, track, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		EnemyRocket.EState state = this._state;
		switch (state)
		{
		case EnemyRocket.EState.Jump1:
		case EnemyRocket.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			break;
		case EnemyRocket.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			break;
		default:
			if (state != EnemyRocket.EState.Aim_Target)
			{
				if (state != EnemyRocket.EState.Attack)
				{
					this._speed = 0f;
				}
				else
				{
					float a = 1f - (float)this.modeLv * 0.5f;
					this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
					this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
					this._targetLockPos.x = Mathf.Max(a, this._targetLockPos.x);
					this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.2f);
					this._boneTarget.SetPosition(this._targetPos);
				}
			}
			else
			{
				float a = 1f - (float)this.modeLv * 0.5f;
				this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
				this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
				this._targetLockPos.x = Mathf.Max(a, this._targetLockPos.x);
				this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.2f);
				this._boneTarget.SetPosition(this._targetPos);
				bool flag = Vector3.Distance(this._targetPos, this._targetLockPos) < 1f;
				if (flag)
				{
					this._state = EnemyRocket.EState.Attack;
					base.PlayAnim((int)this._state, 0, false);
				}
			}
			break;
		case EnemyRocket.EState.Walk1:
			this._speed = this.cacheEnemy.Speed * 2f;
			break;
		case EnemyRocket.EState.Walk2:
			this._speed = this.cacheEnemy.Speed;
			break;
		}
		if (this.isMove)
		{
			bool flag2;
			if (base.Flip)
			{
				this.pos.x = this.pos.x - this._speed * deltaTime;
				flag2 = (this.pos.x < this._targetX && (this._state == EnemyRocket.EState.Walk1 || this._state == EnemyRocket.EState.Walk2));
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				flag2 = (this.pos.x > this._targetX && (this._state == EnemyRocket.EState.Walk1 || this._state == EnemyRocket.EState.Walk2));
			}
			if (flag2)
			{
				this.ChangeState();
			}
		}
	}

	public override void SetParachuter(float gravity = 0.5f)
	{
		base.SetParachuter(gravity);
		base.Gravity = gravity;
		this._state = EnemyRocket.EState.Parachute;
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		if (this._state == EnemyRocket.EState.Walk1 || this._state == EnemyRocket.EState.Walk2)
		{
			this._state = EnemyRocket.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		bool flag = this.isAttack && this._state != EnemyRocket.EState.Jump1 && this._state != EnemyRocket.EState.Jump2;
		if (flag)
		{
			this.isMove = false;
			this._state = EnemyRocket.EState.Aim_Target;
		}
		else
		{
			EnemyRocket.EState state = this._state;
			switch (state)
			{
			case EnemyRocket.EState.Hit_Ice:
			case EnemyRocket.EState.Idle:
			case EnemyRocket.EState.Idle_Walk:
				break;
			case EnemyRocket.EState.Jump1:
				this._state = EnemyRocket.EState.Jump2;
				goto IL_413;
			case EnemyRocket.EState.Jump2:
				this._state = EnemyRocket.EState.Jump3;
				goto IL_413;
			case EnemyRocket.EState.Jump3:
				this.isMove = false;
				this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyRocket.EState.Idle_Walk : EnemyRocket.EState.Idle);
				goto IL_413;
			case EnemyRocket.EState.Parachute:
			{
				bool flag2 = UnityEngine.Random.Range(0, 2) == 1;
				if (flag2)
				{
					this._state = EnemyRocket.EState.Idle;
				}
				else
				{
					this._state = EnemyRocket.EState.Walk1;
					float num = UnityEngine.Random.Range(1f, 3f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num) : (this.pos.x + num));
				}
				goto IL_413;
			}
			default:
				if (state != EnemyRocket.EState.Attack)
				{
					goto IL_413;
				}
				break;
			case EnemyRocket.EState.Walk1:
			case EnemyRocket.EState.Walk2:
				this.isMove = false;
				this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyRocket.EState.Idle_Walk : EnemyRocket.EState.Idle);
				goto IL_413;
			}
			this.sitCollider.enabled = false;
			this.bodyCollider2D.enabled = true;
			if (!this.cacheEnemyData.ismove)
			{
				this._state = ((this._state != EnemyRocket.EState.Idle) ? EnemyRocket.EState.Idle : EnemyRocket.EState.Idle_Walk);
			}
			else if (this.canAttack)
			{
				float num2 = UnityEngine.Random.Range(2f, 5f);
				this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num2) : (this.pos.x + num2));
				this._state = ((Mathf.Abs(this._targetX - this.pos.x) <= 3f) ? EnemyRocket.EState.Walk2 : EnemyRocket.EState.Walk1);
			}
			else
			{
				bool flag3 = false;
				CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
				if (orientaltion != CameraController.Orientation.HORIZONTAL)
				{
					if (orientaltion == CameraController.Orientation.VERTICAL)
					{
						flag3 = ((!CameraController.Instance.isVerticalDown) ? (this.pos.y < CameraController.Instance.camPos.y + CameraController.Instance.Size().y) : (this.pos.y > CameraController.Instance.camPos.y - CameraController.Instance.Size().y));
					}
				}
				else
				{
					flag3 = (this.pos.x < CameraController.Instance.camPos.x || Mathf.Abs(this.pos.x - CameraController.Instance.camPos.x) <= CameraController.Instance.Size().x + 3f);
				}
				if (flag3)
				{
					float num3 = UnityEngine.Random.Range(2f, 4f);
					this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num3 : (-num3));
					this._state = EnemyRocket.EState.Walk1;
				}
				else
				{
					float num4 = UnityEngine.Random.Range(1f, 2f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num4) : (this.pos.x + num4));
					this._state = EnemyRocket.EState.Walk2;
				}
			}
		}
		IL_413:
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(17, 3, false);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyRocket.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(9, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(8, 4, false);
				return;
			}
		}
		this.isMove = false;
		if (this.isAttack)
		{
			base.ResetTimeReload();
		}
		base.SetEmptyAnim(0, 0f);
		this._state = EnemyRocket.EState.Hit_Ice;
		base.PlayAnim(10, 4, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		this.sitCollider.enabled = false;
		base.SetEmptyAnims(0f);
		EWeapon lastWeapon = this.lastWeapon;
		switch (lastWeapon)
		{
		case EWeapon.GRENADE_M61:
		case EWeapon.GRENADE_CHEMICAL:
			goto IL_A1;
		case EWeapon.GRENADE_ICE:
			break;
		case EWeapon.GRENADE_MOLOYOV:
			goto IL_93;
		default:
			switch (lastWeapon)
			{
			case EWeapon.FLAME:
				goto IL_93;
			case EWeapon.THUNDER:
				base.PlayAnim(3, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_77;
				case EWeapon.MGL140:
					goto IL_A1;
				}
				base.PlayAnim(2, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_A1;
			}
			break;
		}
		IL_77:
		base.PlayAnim(7, 0, false);
		return;
		IL_93:
		base.PlayAnim(4, 0, false);
		return;
		IL_A1:
		bool flag = UnityEngine.Random.Range(0, 2) == 1;
		if (flag)
		{
			base.PlayAnim(5, 0, false);
		}
		else
		{
			base.PlayAnim(6, 0, false);
		}
	}

	protected override void Disable()
	{
		base.Disable();
	}

	protected override void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.OnEvent(trackEntry, e);
		if (e.Data.Name.Contains("shoot"))
		{
			Vector3 worldPosition = this._boneGuntip1.GetWorldPosition(this.transform);
			int angle = (int)this._boneGuntip1.GetQuaternion().eulerAngles.z - 90;
			GameManager.Instance.bulletManager.CreateRocketEnemy(worldPosition).SetFire(0, GameManager.Instance.player.tfOrigin.position, this.cacheEnemy.Damage, angle);
		}
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		switch (text)
		{
		case "Idle":
		case "Idle-walk":
		case "Jump01":
			if (this._state != EnemyRocket.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack":
			this.attackCount++;
			if (this.attackCount > this.modeLv)
			{
				this.attackCount = 0;
				base.ResetTimeReload();
				base.SetEmptyAnim(1, 0.3f);
				this.ChangeState();
			}
			else
			{
				base.PlayAnim(1, 0, false);
			}
			break;
		case "Death":
		case "Death-Elec":
		case "Death_Fire":
		case "Death_Ice":
		case "Death_Grenade_Boom":
		case "Death_Grenade_Boom2":
			base.gameObject.SetActive(false);
			break;
		}
	}

	[SerializeField]
	private Collider2D sitCollider;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneTarget;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGuntip1;

	private EnemyRocket.EState _state;

	private Bone _boneTarget;

	private Bone _boneGuntip1;

	private float _targetX;

	private bool _isStartAttack;

	private bool _isLockAttack;

	private float _speed;

	private Vector3 _targetPos;

	private Vector3 _targetLockPos;

	private Vector3 _veloSmoothTarget;

	private enum EState
	{
		Aim_Target,
		Attack,
		Death,
		Death_Elec,
		Death_Fire,
		Death_Bomb,
		Death_Bomb2,
		Death_Ice,
		Hit,
		Hit_Elec,
		Hit_Ice,
		Idle,
		Idle_Walk,
		Jump1,
		Jump2,
		Jump3,
		Parachute,
		PowerUp,
		Walk1,
		Walk2
	}
}
