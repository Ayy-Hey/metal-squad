using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyDrone : BaseEnemy2
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
			if (this._state == EnemyDrone.EState.Day1 || this._state == EnemyDrone.EState.Parachute)
			{
				if (this.dayDu)
				{
					this.dayDu.Off();
					this.dayDu = null;
				}
				base.Gravity = 2f;
			}
			this._state = EnemyDrone.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (this._state)
		{
		case EnemyDrone.EState.Jump3:
			this.rigidbody2D.velocity = Vector2.zero;
			this.ChangeState();
			break;
		case EnemyDrone.EState.Parachute:
		case EnemyDrone.EState.Day1:
			if (this.dayDu)
			{
				this.dayDu.Off();
				this.dayDu = null;
			}
			base.Gravity = 2f;
			this.ChangeState();
			break;
		case EnemyDrone.EState.Walk1:
		case EnemyDrone.EState.Walk2:
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
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemyDrone.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyDrone.EState.Walk1;
			this.ChangeState();
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}

	public override void SetParachuter(float gravity = 0.5f)
	{
		base.SetParachuter(gravity);
		base.Gravity = gravity;
		bool flag = UnityEngine.Random.Range(0, 4) < 3;
		this._state = ((!flag) ? EnemyDrone.EState.Parachute : EnemyDrone.EState.Day1);
		if (flag)
		{
			this._state = EnemyDrone.EState.Day1;
			Vector3 pos = this.pos;
			pos.x += ((!base.Flip) ? 0.2f : -0.2f);
			this.dayDu = EnemyManager.Instance.CreateDayDu(pos);
		}
		else
		{
			this._state = EnemyDrone.EState.Parachute;
		}
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		EnemyDrone.EState state = this._state;
		if (state != EnemyDrone.EState.Idle)
		{
			if (state != EnemyDrone.EState.Jump1)
			{
				if (state != EnemyDrone.EState.Walk1 && state != EnemyDrone.EState.Walk2)
				{
					if (state == EnemyDrone.EState.Attack1_2)
					{
						flag = true;
						this.CreateDrone();
					}
				}
				else
				{
					flag = true;
					this.isMove = true;
					base.Flip = (this.pos.x > this._targetX);
				}
			}
			else
			{
				float d = Mathf.Sqrt(Mathf.Abs(5f * this.rigidbody2D.gravityScale * Physics2D.gravity.y));
				this.rigidbody2D.velocity = Vector2.up * d;
			}
		}
		else
		{
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
		}
		int state2 = (int)this._state;
		bool loop = flag;
		base.PlayAnim(state2, 0, loop);
	}

	private void CreateDrone()
	{
		float speed = this.cacheEnemy.Speed * 4f;
		Vector3 camPos = CameraController.Instance.camPos;
		camPos.z = 0f;
		camPos.x += ((this.pos.x >= camPos.x) ? (CameraController.Instance.Size().x + 1f) : (-CameraController.Instance.Size().x - 1f));
		camPos.y += UnityEngine.Random.Range(1f, 2f);
		this.FreeDrone = new Action(GameManager.Instance.bulletManager.CreateDrone(this.cacheEnemy.Damage, speed, camPos, new Action(AttackDone)).FreeDrove);
	}

	private void AttackDone()
	{
		this.attackCount++;
		if (this.isDie || this._state == EnemyDrone.EState.Hit_Ice)
		{
			this.attackCount = 0;
		}
		else if (this.attackCount > this.modeLv)
		{
			this.attackCount = 0;
			this.ChangeState();
		}
		else
		{
			this.CreateDrone();
		}
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		switch (this._state)
		{
		case EnemyDrone.EState.Jump1:
		case EnemyDrone.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			goto IL_D5;
		case EnemyDrone.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			goto IL_D5;
		case EnemyDrone.EState.Walk1:
			this._speed = this.cacheEnemy.Speed * 2f;
			goto IL_D5;
		case EnemyDrone.EState.Walk2:
			this._speed = this.cacheEnemy.Speed;
			goto IL_D5;
		}
		this._speed = 0f;
		IL_D5:
		if (this.isMove)
		{
			if (base.Flip)
			{
				this.pos.x = this.pos.x - this._speed * deltaTime;
				bool flag = this.pos.x < this._targetX && (this._state == EnemyDrone.EState.Walk1 || this._state == EnemyDrone.EState.Walk2);
				if (flag)
				{
					this.ChangeState();
				}
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				bool flag2 = this.pos.x > this._targetX && (this._state == EnemyDrone.EState.Walk1 || this._state == EnemyDrone.EState.Walk2);
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
		if (this._state == EnemyDrone.EState.Walk1 || this._state == EnemyDrone.EState.Walk2)
		{
			this._state = EnemyDrone.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		if (this.isAttack && !this._isStartAttack)
		{
			this._isStartAttack = true;
			this._state = EnemyDrone.EState.Attack1_1;
		}
		else
		{
			EnemyDrone.EState state = this._state;
			switch (state)
			{
			case EnemyDrone.EState.Hit_Ice:
			case EnemyDrone.EState.Idle:
			case EnemyDrone.EState.Day2:
				break;
			case EnemyDrone.EState.Jump1:
				this._state = EnemyDrone.EState.Jump2;
				goto IL_416;
			case EnemyDrone.EState.Jump2:
				this._state = EnemyDrone.EState.Jump3;
				goto IL_416;
			case EnemyDrone.EState.Jump3:
				this.isMove = false;
				this._state = EnemyDrone.EState.Idle;
				goto IL_416;
			case EnemyDrone.EState.Parachute:
			{
				bool flag = UnityEngine.Random.Range(0, 2) == 1;
				if (flag)
				{
					this._state = EnemyDrone.EState.Idle;
				}
				else
				{
					this._state = EnemyDrone.EState.Walk1;
					float num = UnityEngine.Random.Range(3f, 5f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x + num) : (this.pos.x - num));
				}
				goto IL_416;
			}
			default:
				switch (state)
				{
				case EnemyDrone.EState.Attack1_1:
					this._state = EnemyDrone.EState.Attack1_2;
					goto IL_416;
				case EnemyDrone.EState.Attack1_2:
					this._state = EnemyDrone.EState.Attack1_3;
					goto IL_416;
				case EnemyDrone.EState.Attack1_3:
					break;
				default:
					goto IL_416;
				}
				break;
			case EnemyDrone.EState.Walk1:
			case EnemyDrone.EState.Walk2:
				if (base.isInCamera || this.pos.x > CameraController.Instance.camPos.x)
				{
					this.isMove = false;
					this._state = EnemyDrone.EState.Idle;
				}
				else
				{
					this._targetX = CameraController.Instance.camPos.x - UnityEngine.Random.Range(2f, 4f);
					this._state = EnemyDrone.EState.Walk1;
				}
				goto IL_416;
			case EnemyDrone.EState.Day1:
				this._state = EnemyDrone.EState.Day2;
				goto IL_416;
			}
			if (!this.cacheEnemyData.ismove || base.isInCamera)
			{
				this._state = EnemyDrone.EState.Idle;
			}
			else if (this.canAttack)
			{
				float num2 = UnityEngine.Random.Range(3f, 5f);
				this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x + num2) : (this.pos.x - num2));
				this._state = EnemyDrone.EState.Walk1;
			}
			else
			{
				bool flag2 = false;
				CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
				if (orientaltion != CameraController.Orientation.HORIZONTAL)
				{
					if (orientaltion == CameraController.Orientation.VERTICAL)
					{
						flag2 = ((!CameraController.Instance.isVerticalDown) ? (this.pos.y < CameraController.Instance.camPos.y + CameraController.Instance.Size().y) : (this.pos.y > CameraController.Instance.camPos.y - CameraController.Instance.Size().y));
					}
				}
				else
				{
					flag2 = (this.pos.x < CameraController.Instance.camPos.x || Mathf.Abs(this.pos.x - CameraController.Instance.camPos.x) <= CameraController.Instance.Size().x + 3f);
				}
				if (flag2)
				{
					float num3 = UnityEngine.Random.Range(3f, 4f);
					this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num3 : (-num3));
					this._state = EnemyDrone.EState.Walk1;
				}
				else
				{
					float num4 = UnityEngine.Random.Range(1f, 2f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num4) : (this.pos.x + num4));
					this._state = EnemyDrone.EState.Walk2;
				}
			}
		}
		IL_416:
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(17, 3, false);
	}

	protected override void OnAttack()
	{
		this._isStartAttack = false;
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyDrone.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(10, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(9, 4, false);
				return;
			}
		}
		this.isMove = false;
		if (this.dayDu)
		{
			this.dayDu.Off();
			this.dayDu = null;
		}
		if (this.isAttack || this._state == EnemyDrone.EState.Walk1 || this._state == EnemyDrone.EState.Walk2)
		{
			base.SetEmptyAnim(0, 0f);
		}
		if (this.FreeDrone != null)
		{
			this.FreeDrone();
			this.FreeDrone = null;
		}
		this._isStartAttack = false;
		this._state = EnemyDrone.EState.Hit_Ice;
		base.PlayAnim(11, 4, false);
	}

	protected override void Die(bool isRambo)
	{
		if (this.FreeDrone != null)
		{
			this.FreeDrone();
			this.FreeDrone = null;
		}
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		EWeapon lastWeapon = this.lastWeapon;
		switch (lastWeapon)
		{
		case EWeapon.GRENADE_M61:
		case EWeapon.GRENADE_CHEMICAL:
			goto IL_B2;
		case EWeapon.GRENADE_ICE:
			break;
		case EWeapon.GRENADE_MOLOYOV:
			goto IL_A4;
		default:
			switch (lastWeapon)
			{
			case EWeapon.FLAME:
				goto IL_A4;
			case EWeapon.THUNDER:
				base.PlayAnim(4, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_88;
				case EWeapon.MGL140:
					goto IL_B2;
				}
				base.PlayAnim(3, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_B2;
			}
			break;
		}
		IL_88:
		base.PlayAnim(8, 0, false);
		return;
		IL_A4:
		base.PlayAnim(5, 0, false);
		return;
		IL_B2:
		bool flag = UnityEngine.Random.Range(0, 2) == 1;
		if (flag)
		{
			base.PlayAnim(6, 0, false);
		}
		else
		{
			base.PlayAnim(7, 0, false);
		}
	}

	protected override void Disable()
	{
		base.Disable();
		if (this.FreeDrone != null)
		{
			this.FreeDrone();
			this.FreeDrone = null;
		}
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
		case "Attack1-1":
			if (this._state != EnemyDrone.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Attack1-3":
			if (this._state != EnemyDrone.EState.Hit_Ice)
			{
				base.ResetTimeReload();
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
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

	private Action FreeDrone;

	private EnemyDrone.EState _state;

	private float _targetX;

	private bool _isStartAttack;

	private float _speed;

	private Vector3 _targetPos;

	private Vector3 _targetLockPos;

	private Vector3 _veloSmoothTarget;

	private DayDu dayDu;

	private enum EState
	{
		Attack1_1,
		Attack1_2,
		Attack1_3,
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
