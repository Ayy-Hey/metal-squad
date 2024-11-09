using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemySungCoi : BaseEnemy2
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
			if (this._state == EnemySungCoi.EState.Parachute)
			{
				base.Gravity = 2f;
			}
			this._state = EnemySungCoi.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (this._state)
		{
		case EnemySungCoi.EState.Jump3:
			this.rigidbody2D.velocity = Vector2.zero;
			this.ChangeState();
			break;
		case EnemySungCoi.EState.Parachute:
			base.Gravity = 2f;
			this.ChangeState();
			break;
		case EnemySungCoi.EState.Walk1:
		case EnemySungCoi.EState.Walk2:
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
		if (this._boneGuntip1 == null)
		{
			this._boneGuntip1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip1);
			this._boneGuntip2 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip2);
		}
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemySungCoi.EState.Idle_Walk;
			this.ChangeState();
		}
		else
		{
			this._state = EnemySungCoi.EState.Walk1;
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
		bool flag = false;
		EnemySungCoi.EState state = this._state;
		if (state != EnemySungCoi.EState.Idle_Walk)
		{
			if (state != EnemySungCoi.EState.Jump1)
			{
				if (state != EnemySungCoi.EState.Walk1 && state != EnemySungCoi.EState.Walk2)
				{
					if (state == EnemySungCoi.EState.Attack)
					{
						this.sitCollider.enabled = true;
						this.bodyCollider2D.enabled = false;
						base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
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
			this.sitCollider.enabled = true;
			this.bodyCollider2D.enabled = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
		}
		int state2 = (int)this._state;
		bool loop = flag;
		base.PlayAnim(state2, 0, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		switch (this._state)
		{
		case EnemySungCoi.EState.Jump1:
		case EnemySungCoi.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			goto IL_D5;
		case EnemySungCoi.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			goto IL_D5;
		case EnemySungCoi.EState.Walk1:
			this._speed = this.cacheEnemy.Speed * 2f;
			goto IL_D5;
		case EnemySungCoi.EState.Walk2:
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
				bool flag = this.pos.x < this._targetX && (this._state == EnemySungCoi.EState.Walk1 || this._state == EnemySungCoi.EState.Walk2);
				if (flag)
				{
					this.ChangeState();
				}
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				bool flag2 = this.pos.x > this._targetX && (this._state == EnemySungCoi.EState.Walk1 || this._state == EnemySungCoi.EState.Walk2);
				if (flag2)
				{
					this.ChangeState();
				}
			}
		}
	}

	public override void SetParachuter(float gravity = 0.5f)
	{
		base.SetParachuter(gravity);
		base.Gravity = gravity;
		this._state = EnemySungCoi.EState.Parachute;
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		if (this._state == EnemySungCoi.EState.Walk1 || this._state == EnemySungCoi.EState.Walk2)
		{
			this._state = EnemySungCoi.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		bool flag = this.isAttack && this._state != EnemySungCoi.EState.Jump1 && this._state != EnemySungCoi.EState.Jump2 && this._state != EnemySungCoi.EState.Hit_Ice;
		if (flag)
		{
			this._state = EnemySungCoi.EState.Attack;
		}
		else
		{
			EnemySungCoi.EState state = this._state;
			switch (state)
			{
			case EnemySungCoi.EState.Hit_Ice:
			case EnemySungCoi.EState.Idle_Walk:
				this.sitCollider.enabled = false;
				this.bodyCollider2D.enabled = true;
				if (!this.cacheEnemyData.ismove)
				{
					this._state = EnemySungCoi.EState.Idle_Walk;
				}
				else if (this.canAttack)
				{
					float num = UnityEngine.Random.Range(2f, 5f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num) : (this.pos.x + num));
					this._state = ((this._targetX <= 3f) ? EnemySungCoi.EState.Walk2 : EnemySungCoi.EState.Walk1);
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
						float num2 = UnityEngine.Random.Range(2f, 4f);
						this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num2 : (-num2));
						this._state = EnemySungCoi.EState.Walk1;
					}
					else
					{
						float num3 = UnityEngine.Random.Range(1f, 2f);
						this._targetX = ((!base.Flip) ? (this.pos.x - num3) : (this.pos.x + num3));
						this._state = EnemySungCoi.EState.Walk2;
					}
				}
				break;
			case EnemySungCoi.EState.Jump1:
				this._state = EnemySungCoi.EState.Jump2;
				break;
			case EnemySungCoi.EState.Jump2:
				this._state = EnemySungCoi.EState.Jump3;
				break;
			case EnemySungCoi.EState.Jump3:
				this.isMove = false;
				this._state = EnemySungCoi.EState.Idle_Walk;
				break;
			case EnemySungCoi.EState.Parachute:
			{
				bool flag3 = UnityEngine.Random.Range(0, 2) == 1;
				if (flag3)
				{
					this._state = EnemySungCoi.EState.Idle_Walk;
				}
				else
				{
					this._state = EnemySungCoi.EState.Walk1;
					float num4 = UnityEngine.Random.Range(1f, 3f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num4) : (this.pos.x + num4));
				}
				break;
			}
			default:
				if (state == EnemySungCoi.EState.Attack)
				{
					this.sitCollider.enabled = false;
					this.bodyCollider2D.enabled = true;
					bool flag4 = UnityEngine.Random.Range(0, 3) < 2 && this.cacheEnemyData.ismove;
					if (flag4)
					{
						float num5 = UnityEngine.Random.Range(2f, 5f);
						this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num5) : (this.pos.x + num5));
						this._state = ((Mathf.Abs(this._targetX - this.pos.x) <= 3f) ? EnemySungCoi.EState.Walk2 : EnemySungCoi.EState.Walk1);
					}
					else
					{
						this._state = EnemySungCoi.EState.Idle_Walk;
					}
				}
				break;
			case EnemySungCoi.EState.Walk1:
			case EnemySungCoi.EState.Walk2:
				this.isMove = false;
				this._state = EnemySungCoi.EState.Idle_Walk;
				break;
			}
		}
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(15, 3, false);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemySungCoi.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(8, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(7, 4, false);
				return;
			}
		}
		this.isMove = false;
		if (this._state == EnemySungCoi.EState.Walk1 || this._state == EnemySungCoi.EState.Walk2 || this._state == EnemySungCoi.EState.Attack)
		{
			base.SetEmptyAnim(0, 0f);
		}
		this._state = EnemySungCoi.EState.Hit_Ice;
		base.PlayAnim(9, 4, false);
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
				base.PlayAnim(2, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_77;
				case EWeapon.MGL140:
					goto IL_A1;
				}
				base.PlayAnim(1, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_A1;
			}
			break;
		}
		IL_77:
		base.PlayAnim(6, 0, false);
		return;
		IL_93:
		base.PlayAnim(3, 0, false);
		return;
		IL_A1:
		bool flag = UnityEngine.Random.Range(0, 2) == 1;
		if (flag)
		{
			base.PlayAnim(4, 0, false);
		}
		else
		{
			base.PlayAnim(5, 0, false);
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
			float speed = Mathf.Min(this.cacheEnemy.Speed * 3f, 3f);
			float timeRotateToTaget = UnityEngine.Random.Range(1.5f, 2f);
			Vector3 worldPosition = this._boneGuntip1.GetWorldPosition(this.transform);
			Vector3 v = worldPosition - this._boneGuntip2.GetWorldPosition(this.transform);
			GameManager.Instance.bulletManager.CreateBulletSungCoi(speed, this.cacheEnemy.Damage, timeRotateToTaget, worldPosition, v);
		}
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		switch (text)
		{
		case "Idle-walk":
		case "Jump01":
			if (this._state != EnemySungCoi.EState.Hit_Ice)
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
				this.ChangeState();
			}
			else if (this._state != EnemySungCoi.EState.Hit_Ice)
			{
				base.PlayAnim((int)this._state, 0, false);
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

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGuntip1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGuntip2;

	private EnemySungCoi.EState _state;

	private Bone _boneGuntip1;

	private Bone _boneGuntip2;

	private float _targetX;

	private float _speed;

	private Vector3 _targetPos;

	private Vector3 _targetLockPos;

	private Vector3 _veloSmoothTarget;

	private enum EState
	{
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
