using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyFire : BaseEnemy2
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
			if (this._state == EnemyFire.EState.Day1 || this._state == EnemyFire.EState.Parachute)
			{
				if (this.dayDu)
				{
					this.dayDu.Off();
					this.dayDu = null;
				}
				base.Gravity = 2f;
			}
			this._state = EnemyFire.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (this._state)
		{
		case EnemyFire.EState.Jump3:
			this.rigidbody2D.velocity = Vector2.zero;
			this.ChangeState();
			break;
		case EnemyFire.EState.Parachute:
		case EnemyFire.EState.Day1:
			if (this.dayDu)
			{
				this.dayDu.Off();
				this.dayDu = null;
			}
			base.Gravity = 2f;
			this.ChangeState();
			break;
		case EnemyFire.EState.Walk1:
		case EnemyFire.EState.Walk2:
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
			this._state = EnemyFire.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyFire.EState.Walk1;
			this.ChangeState();
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (this.isAttack && !this.isDie)
		{
			if (!this._isStartAttack)
			{
				bool flag = base.Flip == this.pos.x < GameManager.Instance.player.transform.position.x;
				if (this._state == EnemyFire.EState.Hit_Ice || flag)
				{
					return;
				}
				this._isStartAttack = true;
				base.PlayAnim(0, 2, false);
			}
			else
			{
				float a = 1f - (float)this.modeLv * 0.5f;
				this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
				this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
				this._targetLockPos.x = Mathf.Max(a, this._targetLockPos.x);
				this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.2f);
				this._boneTarget.SetPosition(this._targetPos);
				if (!this._isLockAttack)
				{
					this._isLockAttack = (Vector3.Distance(this._targetPos, this._targetLockPos) < 1f);
					if (this._isLockAttack)
					{
						base.PlayAnim(1, 1, false);
						this.flame.Active(this.cacheEnemy.Damage, this._boneGuntip1.GetWorldPosition(this.transform), this._boneGuntip1.GetQuaternion());
					}
				}
			}
			if (this.flame.isInit)
			{
				this.flame.OnUpdate(this._boneGuntip1.GetWorldPosition(this.transform), this._boneGuntip1.GetQuaternion());
			}
		}
	}

	public override void SetParachuter(float gravity = 0.5f)
	{
		base.SetParachuter(gravity);
		base.Gravity = gravity;
		bool flag = UnityEngine.Random.Range(0, 4) < 3;
		this._state = ((!flag) ? EnemyFire.EState.Parachute : EnemyFire.EState.Day1);
		if (flag)
		{
			this._state = EnemyFire.EState.Day1;
			Vector3 pos = this.pos;
			pos.x += ((!base.Flip) ? 0.2f : -0.2f);
			this.dayDu = EnemyManager.Instance.CreateDayDu(pos);
		}
		else
		{
			this._state = EnemyFire.EState.Parachute;
		}
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		switch (this._state)
		{
		case EnemyFire.EState.Idle:
			this.isMove = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyFire.EState.Idle_Walk:
			this.isMove = false;
			this.sitCollider.enabled = true;
			this.bodyCollider2D.enabled = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyFire.EState.Jump1:
		{
			float d = Mathf.Sqrt(Mathf.Abs(5f * this.rigidbody2D.gravityScale * Physics2D.gravity.y));
			this.rigidbody2D.velocity = Vector2.up * d;
			break;
		}
		case EnemyFire.EState.Walk1:
		case EnemyFire.EState.Walk2:
			flag = true;
			this.isMove = true;
			base.Flip = (this.pos.x > this._targetX);
			break;
		}
		int state = (int)this._state;
		bool loop = flag;
		base.PlayAnim(state, 0, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		switch (this._state)
		{
		case EnemyFire.EState.Jump1:
		case EnemyFire.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			break;
		case EnemyFire.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			break;
		case EnemyFire.EState.Walk1:
			this._speed = this.cacheEnemy.Speed * 2f;
			break;
		case EnemyFire.EState.Walk2:
			this._speed = this.cacheEnemy.Speed;
			break;
		}
		if (this.isMove)
		{
			if (base.Flip)
			{
				this.pos.x = this.pos.x - this._speed * deltaTime;
				bool flag = this.pos.x < this._targetX && (this._state == EnemyFire.EState.Walk1 || this._state == EnemyFire.EState.Walk2);
				if (flag)
				{
					this.ChangeState();
				}
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				bool flag2 = this.pos.x > this._targetX && (this._state == EnemyFire.EState.Walk1 || this._state == EnemyFire.EState.Walk2);
				if (flag2)
				{
					this.ChangeState();
				}
			}
		}
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		if (this._state == EnemyFire.EState.Walk1 || this._state == EnemyFire.EState.Walk2)
		{
			this._state = EnemyFire.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		switch (this._state)
		{
		case EnemyFire.EState.Hit_Ice:
		case EnemyFire.EState.Idle:
		case EnemyFire.EState.Idle_Walk:
		case EnemyFire.EState.Day2:
			this.sitCollider.enabled = false;
			this.bodyCollider2D.enabled = true;
			if (!this.cacheEnemyData.ismove)
			{
				this._state = ((this._state != EnemyFire.EState.Idle) ? EnemyFire.EState.Idle : EnemyFire.EState.Idle_Walk);
			}
			else if (this.canAttack)
			{
				this._targetX = GameManager.Instance.player.transform.position.x + UnityEngine.Random.Range(-2f, 2f);
				this._state = ((Mathf.Abs(this._targetX - this.pos.x) <= 3f) ? EnemyFire.EState.Walk2 : EnemyFire.EState.Walk1);
			}
			else if (!base.isInCamera)
			{
				bool flag = false;
				CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
				if (orientaltion != CameraController.Orientation.HORIZONTAL)
				{
					if (orientaltion == CameraController.Orientation.VERTICAL)
					{
						flag = ((!CameraController.Instance.isVerticalDown) ? (this.pos.y < CameraController.Instance.camPos.y + CameraController.Instance.Size().y) : (this.pos.y > CameraController.Instance.camPos.y - CameraController.Instance.Size().y));
					}
				}
				else
				{
					flag = (this.pos.x < CameraController.Instance.camPos.x || Mathf.Abs(this.pos.x - CameraController.Instance.camPos.x) <= CameraController.Instance.Size().x + 3f);
				}
				if (flag)
				{
					float num = UnityEngine.Random.Range(1f, 3f);
					this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num : (-num));
					this._state = EnemyFire.EState.Walk1;
				}
				else
				{
					float num2 = UnityEngine.Random.Range(1f, 2f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num2) : (this.pos.x + num2));
					this._state = EnemyFire.EState.Walk2;
				}
			}
			else
			{
				this._targetX = GameManager.Instance.player.transform.position.x;
				this._state = ((Mathf.Abs(this._targetX - this.pos.x) <= 3f) ? EnemyFire.EState.Walk2 : EnemyFire.EState.Walk1);
			}
			break;
		case EnemyFire.EState.Jump1:
			this._state = EnemyFire.EState.Jump2;
			break;
		case EnemyFire.EState.Jump2:
			this._state = EnemyFire.EState.Jump3;
			break;
		case EnemyFire.EState.Jump3:
			this.isMove = false;
			this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyFire.EState.Idle_Walk : EnemyFire.EState.Idle);
			break;
		case EnemyFire.EState.Parachute:
		{
			bool flag2 = UnityEngine.Random.Range(0, 2) == 1;
			if (flag2)
			{
				this._state = EnemyFire.EState.Idle;
			}
			else
			{
				this._state = EnemyFire.EState.Walk1;
				float num3 = UnityEngine.Random.Range(1f, 3f);
				this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num3) : (this.pos.x + num3));
			}
			break;
		}
		case EnemyFire.EState.Walk1:
		case EnemyFire.EState.Walk2:
			this.isMove = false;
			this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyFire.EState.Idle_Walk : EnemyFire.EState.Idle);
			break;
		case EnemyFire.EState.Day1:
			this._state = EnemyFire.EState.Day2;
			break;
		}
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(17, 3, false);
	}

	protected override void OnAttack()
	{
		if (this._state == EnemyFire.EState.Hit_Ice || this._state == EnemyFire.EState.Parachute)
		{
			base.ResetTimeReload();
			return;
		}
		this._isStartAttack = false;
		this._isLockAttack = false;
		this._targetPos = this._boneGuntip1.GetLocalPosition();
		this._boneTarget.SetPosition(this._targetPos);
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyFire.EState.Hit_Ice)
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
		if (this.dayDu)
		{
			this.dayDu.Off();
			this.dayDu = null;
		}
		if (this._state == EnemyFire.EState.Walk1 || this._state == EnemyFire.EState.Walk2)
		{
			base.SetEmptyAnim(0, 0f);
		}
		this._state = EnemyFire.EState.Hit_Ice;
		base.PlayAnim(10, 4, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		this.sitCollider.enabled = false;
		base.SetEmptyAnims(0f);
		this.flame.Off();
		EWeapon lastWeapon = this.lastWeapon;
		switch (lastWeapon)
		{
		case EWeapon.GRENADE_M61:
		case EWeapon.GRENADE_CHEMICAL:
			goto IL_AC;
		case EWeapon.GRENADE_ICE:
			break;
		case EWeapon.GRENADE_MOLOYOV:
			goto IL_9E;
		default:
			switch (lastWeapon)
			{
			case EWeapon.FLAME:
				goto IL_9E;
			case EWeapon.THUNDER:
				base.PlayAnim(3, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_82;
				case EWeapon.MGL140:
					goto IL_AC;
				}
				base.PlayAnim(2, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_AC;
			}
			break;
		}
		IL_82:
		base.PlayAnim(7, 0, false);
		return;
		IL_9E:
		base.PlayAnim(4, 0, false);
		return;
		IL_AC:
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
		case "day-2":
			if (this._state != EnemyFire.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack-Fire":
		{
			this.attackCount++;
			int num2 = this.modeLv * 2 + 4;
			if (this.attackCount >= num2 || this._state == EnemyFire.EState.Hit_Ice)
			{
				this.flame.Off();
				this.attackCount = 0;
				base.ResetTimeReload();
				base.SetEmptyAnim(2, 0.2f);
				base.SetEmptyAnim(1, 0.2f);
			}
			else
			{
				base.PlayAnim(1, 1, false);
			}
			break;
		}
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

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneTarget;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGuntip1;

	[SerializeField]
	private FlameEnemyFire flame;

	private EnemyFire.EState _state;

	private Bone _boneTarget;

	private Bone _boneGuntip1;

	private float _targetX;

	private bool _isStartAttack;

	private bool _isLockAttack;

	private float _speed;

	private Vector3 _targetPos;

	private Vector3 _targetLockPos;

	private Vector3 _veloSmoothTarget;

	private DayDu dayDu;

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
		Walk2,
		Day1,
		Day2
	}
}
