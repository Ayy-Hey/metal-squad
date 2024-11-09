using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyKnife : BaseEnemy2
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
			if (this._state == EnemyKnife.EState.Day1 || this._state == EnemyKnife.EState.Parachute)
			{
				if (this.dayDu)
				{
					this.dayDu.Off();
					this.dayDu = null;
				}
				base.Gravity = 2f;
			}
			this._state = EnemyKnife.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (this._state)
		{
		case EnemyKnife.EState.Jump3:
			this.rigidbody2D.velocity = Vector2.zero;
			this.ChangeState();
			break;
		case EnemyKnife.EState.Parachute:
		case EnemyKnife.EState.Day1:
			if (this.dayDu)
			{
				this.dayDu.Off();
				this.dayDu = null;
			}
			base.Gravity = 2f;
			this.ChangeState();
			break;
		case EnemyKnife.EState.Walk1:
		case EnemyKnife.EState.Walk2:
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
		if (this._boneGuntip1 == null)
		{
			this._boneGuntip1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip1);
		}
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemyKnife.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyKnife.EState.Walk1;
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
		this._state = ((!flag) ? EnemyKnife.EState.Parachute : EnemyKnife.EState.Day1);
		if (flag)
		{
			this._state = EnemyKnife.EState.Day1;
			Vector3 pos = this.pos;
			pos.x += ((!base.Flip) ? 0.2f : -0.2f);
			this.dayDu = EnemyManager.Instance.CreateDayDu(pos);
		}
		else
		{
			this._state = EnemyKnife.EState.Parachute;
		}
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		EnemyKnife.EState state = this._state;
		if (state != EnemyKnife.EState.Attack01 && state != EnemyKnife.EState.Attack02 && state != EnemyKnife.EState.Idle)
		{
			if (state != EnemyKnife.EState.Jump1)
			{
				if (state == EnemyKnife.EState.Walk1 || state == EnemyKnife.EState.Walk2)
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
			this.isMove = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
		}
		int state2 = (int)this._state;
		bool loop = flag;
		base.PlayAnim(state2, 0, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		EnemyKnife.EState state = this._state;
		switch (state)
		{
		case EnemyKnife.EState.Jump1:
		case EnemyKnife.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			break;
		case EnemyKnife.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			break;
		default:
			if (state != EnemyKnife.EState.Attack01 && state != EnemyKnife.EState.Attack02)
			{
				this._speed = 0f;
			}
			else
			{
				this.tfColliderKnife.position = this._boneGuntip1.GetWorldPosition(this.transform);
			}
			break;
		case EnemyKnife.EState.Walk1:
			this._speed = this.cacheEnemy.Speed * 2f;
			break;
		case EnemyKnife.EState.Walk2:
			this._speed = this.cacheEnemy.Speed;
			break;
		}
		if (this.isMove)
		{
			bool flag;
			if (base.Flip)
			{
				this.pos.x = this.pos.x - this._speed * deltaTime;
				flag = (this.pos.x < this._targetX);
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				flag = (this.pos.x > this._targetX);
			}
			flag = ((flag || Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x) < 1f) && (this._state == EnemyKnife.EState.Walk1 || this._state == EnemyKnife.EState.Walk2));
			if (flag)
			{
				this.ChangeState();
			}
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		bool flag = this._state != EnemyKnife.EState.Jump1 && this._state != EnemyKnife.EState.Jump2 && Vector3.Distance(this.tfOrigin.position, GameManager.Instance.player.tfOrigin.position) <= 1.5f;
		if (this.isAttack && flag)
		{
			this._state = ((this.modeLv != 0) ? EnemyKnife.EState.Attack02 : EnemyKnife.EState.Attack01);
		}
		else
		{
			EnemyKnife.EState state = this._state;
			switch (state)
			{
			case EnemyKnife.EState.Hit_Ice:
			case EnemyKnife.EState.Idle:
			case EnemyKnife.EState.Day2:
				break;
			case EnemyKnife.EState.Jump1:
				this._state = EnemyKnife.EState.Jump2;
				goto IL_44F;
			case EnemyKnife.EState.Jump2:
				this._state = EnemyKnife.EState.Jump3;
				goto IL_44F;
			case EnemyKnife.EState.Jump3:
				this.isMove = false;
				this._state = EnemyKnife.EState.Idle;
				goto IL_44F;
			case EnemyKnife.EState.Parachute:
			{
				bool flag2 = UnityEngine.Random.Range(0, 2) == 1;
				if (flag2)
				{
					this._state = EnemyKnife.EState.Idle;
				}
				else
				{
					this._state = EnemyKnife.EState.Walk1;
					float num = UnityEngine.Random.Range(1f, 3f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num) : (this.pos.x + num));
				}
				goto IL_44F;
			}
			default:
				if (state != EnemyKnife.EState.Attack01 && state != EnemyKnife.EState.Attack02)
				{
					goto IL_44F;
				}
				break;
			case EnemyKnife.EState.Walk1:
			case EnemyKnife.EState.Walk2:
				this.isMove = false;
				this._state = EnemyKnife.EState.Idle;
				goto IL_44F;
			case EnemyKnife.EState.Day1:
				this._state = EnemyKnife.EState.Day2;
				goto IL_44F;
			}
			if (!this.cacheEnemyData.ismove)
			{
				this._state = EnemyKnife.EState.Idle;
			}
			else if (this.canAttack)
			{
				this._targetX = GameManager.Instance.player.transform.position.x + UnityEngine.Random.Range(-1f, 1f);
				this._state = ((Mathf.Abs(this._targetX - this.pos.x) <= 3f) ? EnemyKnife.EState.Walk2 : EnemyKnife.EState.Walk1);
			}
			else if (!base.isInCamera)
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
					float num2 = UnityEngine.Random.Range(1f, 3f);
					this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num2 : (-num2));
					this._state = EnemyKnife.EState.Walk1;
				}
				else
				{
					float num3 = UnityEngine.Random.Range(1f, 2f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num3) : (this.pos.x + num3));
					this._state = EnemyKnife.EState.Walk2;
				}
			}
			else
			{
				this._targetX = GameManager.Instance.player.transform.position.x;
				this._state = ((Mathf.Abs(this._targetX - this.pos.x) <= 3f) ? EnemyKnife.EState.Walk2 : EnemyKnife.EState.Walk1);
			}
		}
		IL_44F:
		base.ChangeState();
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		if (this._state == EnemyKnife.EState.Walk1 || this._state == EnemyKnife.EState.Walk2)
		{
			this._state = EnemyKnife.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(16, 3, false);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyKnife.EState.Hit_Ice)
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
		base.SetEmptyAnim(0, 0f);
		this._state = EnemyKnife.EState.Hit_Ice;
		base.PlayAnim(10, 4, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		EWeapon lastWeapon = this.lastWeapon;
		switch (lastWeapon)
		{
		case EWeapon.GRENADE_M61:
		case EWeapon.GRENADE_CHEMICAL:
			goto IL_95;
		case EWeapon.GRENADE_ICE:
			break;
		case EWeapon.GRENADE_MOLOYOV:
			goto IL_87;
		default:
			switch (lastWeapon)
			{
			case EWeapon.FLAME:
				goto IL_87;
			case EWeapon.THUNDER:
				base.PlayAnim(3, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_6B;
				case EWeapon.MGL140:
					goto IL_95;
				}
				base.PlayAnim(2, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_95;
			}
			break;
		}
		IL_6B:
		base.PlayAnim(7, 0, false);
		return;
		IL_87:
		base.PlayAnim(4, 0, false);
		return;
		IL_95:
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
		if (e.Data.Name.Contains("hummer"))
		{
			this.colliderKnife.OverlapCollider(this.contactFilter, this.colliderKnifeContacts);
			for (int i = 0; i < this.colliderKnifeContacts.Length; i++)
			{
				if (this.colliderKnifeContacts[i] != null)
				{
					try
					{
						this.colliderKnifeContacts[i].GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
					}
					catch
					{
					}
				}
				this.colliderKnifeContacts[i] = null;
			}
		}
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		switch (text)
		{
		case "Idle":
		case "Jump01":
		case "day-2":
			if (this._state != EnemyKnife.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack01":
			base.ResetTimeReload();
			this.ChangeState();
			break;
		case "Attack02":
			this.attackCount++;
			if (this.attackCount < this.modeLv)
			{
				base.PlayAnim((int)this._state, 0, false);
			}
			else
			{
				this.attackCount = 0;
				base.ResetTimeReload();
				this.ChangeState();
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

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGuntip1;

	[SerializeField]
	private Transform tfColliderKnife;

	[SerializeField]
	private Collider2D colliderKnife;

	[SerializeField]
	private ContactFilter2D contactFilter;

	private EnemyKnife.EState _state;

	private Bone _boneGuntip1;

	private float _targetX;

	private float _speed;

	private Vector3 _targetPos;

	private Vector3 _targetLockPos;

	private Vector3 _veloSmoothTarget;

	private Collider2D[] colliderKnifeContacts = new Collider2D[3];

	private DayDu dayDu;

	private enum EState
	{
		Attack01,
		Attack02,
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
