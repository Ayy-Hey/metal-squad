using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyPistol : BaseEnemy2
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
			this.UpdateColliderOffset();
		}
		if (collision.CompareTag("Found_Jump"))
		{
			if (this._state == EnemyPistol.EState.Parachute)
			{
				base.Gravity = 2f;
			}
			this._state = EnemyPistol.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		EnemyPistol.EState state = this._state;
		if (state != EnemyPistol.EState.Parachute)
		{
			if (state != EnemyPistol.EState.Jump3)
			{
				if (state == EnemyPistol.EState.Walk1)
				{
					if (collision.contacts != null && collision.contacts[0].normal.y < 0.5f)
					{
						float num = UnityEngine.Random.Range(2f, 4f);
						this._targetX = ((!base.Flip) ? (this.pos.x - num) : (this.pos.x + num));
						base.Flip = !base.Flip;
						this.UpdateColliderOffset();
					}
				}
			}
			else
			{
				this.rigidbody2D.velocity = Vector2.zero;
				this.ChangeState();
			}
		}
		else
		{
			base.Gravity = 2f;
			this.ChangeState();
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
			this._state = EnemyPistol.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyPistol.EState.Walk1;
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
		this._state = EnemyPistol.EState.Parachute;
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	private void UpdateColliderOffset()
	{
		this.colliderOffset.x = ((!base.Flip) ? 0.3f : -0.3f);
		this.colliderOffset.y = this.bodyCollider2D.offset.y;
		this.bodyCollider2D.offset = this.colliderOffset;
		this.colliderOffset.y = this.sitCollider.offset.y;
		this.sitCollider.offset = this.colliderOffset;
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		EnemyPistol.EState state = this._state;
		switch (state)
		{
		case EnemyPistol.EState.Attack_01:
		case EnemyPistol.EState.Attack_02:
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyPistol.EState.Attack_Sit_01:
		case EnemyPistol.EState.Attack_Sit_02:
			this.sitCollider.enabled = true;
			this.bodyCollider2D.enabled = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		default:
			switch (state)
			{
			case EnemyPistol.EState.Idle:
				base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
				break;
			case EnemyPistol.EState.Idle_Walk:
				this.sitCollider.enabled = true;
				this.bodyCollider2D.enabled = false;
				base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
				break;
			case EnemyPistol.EState.Jump1:
			{
				float d = Mathf.Sqrt(Mathf.Abs(5f * this.rigidbody2D.gravityScale * Physics2D.gravity.y));
				this.rigidbody2D.velocity = Vector2.up * d;
				break;
			}
			case EnemyPistol.EState.Walk1:
				flag = true;
				this.isMove = true;
				base.Flip = (this.pos.x > this._targetX);
				break;
			}
			break;
		}
		this.UpdateColliderOffset();
		int state2 = (int)this._state;
		bool loop = flag;
		base.PlayAnim(state2, 0, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		switch (this._state)
		{
		case EnemyPistol.EState.Jump1:
		case EnemyPistol.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			goto IL_B5;
		case EnemyPistol.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			goto IL_B5;
		case EnemyPistol.EState.Walk1:
			this._speed = this.cacheEnemy.Speed;
			goto IL_B5;
		}
		this._speed = 0f;
		IL_B5:
		if (this.isMove)
		{
			if (base.Flip)
			{
				this.pos.x = this.pos.x - this._speed * deltaTime;
				bool flag = this.pos.x < this._targetX && this._state == EnemyPistol.EState.Walk1;
				if (flag)
				{
					this.ChangeState();
				}
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				bool flag2 = this.pos.x > this._targetX && this._state == EnemyPistol.EState.Walk1;
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
		if (this._state == EnemyPistol.EState.Walk1)
		{
			this._state = EnemyPistol.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		if (this.isAttack)
		{
			EnemyPistol.EState state = this._state;
			if (state != EnemyPistol.EState.Idle_Walk)
			{
				this._state = ((this.modeLv >= 2) ? EnemyPistol.EState.Attack_02 : EnemyPistol.EState.Attack_01);
			}
			else
			{
				this._state = ((this.modeLv >= 2) ? EnemyPistol.EState.Attack_Sit_02 : EnemyPistol.EState.Attack_Sit_01);
			}
		}
		else
		{
			EnemyPistol.EState state2 = this._state;
			switch (state2)
			{
			case EnemyPistol.EState.Hit_Ice:
			case EnemyPistol.EState.Idle:
			case EnemyPistol.EState.Idle_Walk:
				this.sitCollider.enabled = false;
				this.bodyCollider2D.enabled = true;
				if (!this.cacheEnemyData.ismove)
				{
					this._state = ((this._state != EnemyPistol.EState.Idle) ? EnemyPistol.EState.Idle : EnemyPistol.EState.Idle_Walk);
				}
				else if (this.canAttack)
				{
					float num = UnityEngine.Random.Range(2f, 5f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num) : (this.pos.x + num));
					this._state = EnemyPistol.EState.Walk1;
				}
				else
				{
					float num2 = UnityEngine.Random.Range(2f, 3f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num2) : (this.pos.x + num2));
					this._state = EnemyPistol.EState.Walk1;
				}
				break;
			case EnemyPistol.EState.Jump1:
				this._state = EnemyPistol.EState.Jump2;
				break;
			case EnemyPistol.EState.Jump2:
				this._state = EnemyPistol.EState.Jump3;
				break;
			case EnemyPistol.EState.Jump3:
				this.isMove = false;
				this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyPistol.EState.Idle_Walk : EnemyPistol.EState.Idle);
				break;
			case EnemyPistol.EState.Parachute:
			{
				bool flag = UnityEngine.Random.Range(0, 2) == 1;
				if (flag)
				{
					this._state = EnemyPistol.EState.Idle;
				}
				else
				{
					this._state = EnemyPistol.EState.Walk1;
					float num3 = UnityEngine.Random.Range(1f, 3f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num3) : (this.pos.x + num3));
				}
				break;
			}
			default:
				switch (state2)
				{
				case EnemyPistol.EState.Attack_01:
				case EnemyPistol.EState.Attack_02:
				case EnemyPistol.EState.Attack_Sit_01:
				case EnemyPistol.EState.Attack_Sit_02:
				{
					this.sitCollider.enabled = false;
					this.bodyCollider2D.enabled = true;
					bool flag2 = UnityEngine.Random.Range(0, 2) == 1;
					if (!this.cacheEnemyData.ismove || flag2)
					{
						this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyPistol.EState.Idle : EnemyPistol.EState.Idle_Walk);
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
							float num4 = UnityEngine.Random.Range(3f, 4f);
							this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num4 : (-num4));
							this._state = EnemyPistol.EState.Walk1;
						}
						else
						{
							float num5 = UnityEngine.Random.Range(1f, 2f);
							this._targetX = ((!base.Flip) ? (this.pos.x - num5) : (this.pos.x + num5));
							this._state = EnemyPistol.EState.Walk1;
						}
					}
					break;
				}
				}
				break;
			case EnemyPistol.EState.Walk1:
				this.isMove = false;
				this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? EnemyPistol.EState.Idle_Walk : EnemyPistol.EState.Idle);
				break;
			}
		}
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(19, 3, false);
	}

	protected override void OnAttack()
	{
		if (this._state == EnemyPistol.EState.Hit_Ice || this._state == EnemyPistol.EState.Parachute)
		{
			base.ResetTimeReload();
			return;
		}
		base.OnAttack();
		this.ChangeState();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyPistol.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(11, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(10, 4, false);
				return;
			}
		}
		this.isMove = false;
		if (this._state == EnemyPistol.EState.Walk1 || this.isAttack)
		{
			base.SetEmptyAnim(0, 0f);
		}
		this._state = EnemyPistol.EState.Hit_Ice;
		base.PlayAnim(12, 4, false);
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
			goto IL_A2;
		case EWeapon.GRENADE_ICE:
			break;
		case EWeapon.GRENADE_MOLOYOV:
			goto IL_94;
		default:
			switch (lastWeapon)
			{
			case EWeapon.FLAME:
				goto IL_94;
			case EWeapon.THUNDER:
				base.PlayAnim(5, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_77;
				case EWeapon.MGL140:
					goto IL_A2;
				}
				base.PlayAnim(4, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_A2;
			}
			break;
		}
		IL_77:
		base.PlayAnim(9, 0, false);
		return;
		IL_94:
		base.PlayAnim(8, 0, false);
		return;
		IL_A2:
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
	}

	protected override void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.OnEvent(trackEntry, e);
		if (e.Data.Name.Contains("shoot"))
		{
			Vector3 worldPosition = this._boneGuntip1.GetWorldPosition(this.transform);
			Vector3 v = worldPosition - this._boneGuntip2.GetWorldPosition(this.transform);
			float speed = this.cacheEnemy.Speed * 4f;
			BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(10, v, worldPosition, this.cacheEnemy.Damage, speed, 0f);
			bulletEnemy.spriteRenderer.flipX = false;
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
			if (this._state != EnemyPistol.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack01":
		case "Attack02":
		case "Attack-walk01":
		case "Attack-walk02":
			if (this._state == EnemyPistol.EState.Hit_Ice)
			{
				base.ResetTimeReload();
				return;
			}
			if (this.modeLv == 1 && this.attackCount < 1)
			{
				this.attackCount++;
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
		case "Death_Grenade_Boom_fire":
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
	private string strBoneGuntip1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGuntip2;

	private EnemyPistol.EState _state;

	private Bone _boneGuntip1;

	private Bone _boneGuntip2;

	private float _targetX;

	private float _speed;

	private Vector2 colliderOffset;

	private enum EState
	{
		Attack_01,
		Attack_02,
		Attack_Sit_01,
		Attack_Sit_02,
		Death,
		Death_Elec,
		Death_Bomb,
		Death_Bomb2,
		Death_Bomb_Fire,
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
		Walk1
	}
}
