using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyGrenade : BaseEnemy2
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
			if (this._state == EnemyGrenade.EState.Parachute)
			{
				base.Gravity = 2f;
			}
			this._state = EnemyGrenade.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		EnemyGrenade.EState state = this._state;
		if (state != EnemyGrenade.EState.Parachute)
		{
			if (state != EnemyGrenade.EState.Jump3)
			{
				if (state == EnemyGrenade.EState.Walk1)
				{
					if (collision.contacts != null && collision.contacts[0].normal.y < 0.5f)
					{
						float num = UnityEngine.Random.Range(2f, 4f);
						this._targetX = ((!base.Flip) ? (this.pos.x - num) : (this.pos.x + num));
						base.Flip = !base.Flip;
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
		if (this._boneGuntip1 == null)
		{
			this._boneGuntip1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip1);
		}
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemyGrenade.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyGrenade.EState.Walk1;
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
		this._state = EnemyGrenade.EState.Parachute;
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		EnemyGrenade.EState state = this._state;
		if (state != EnemyGrenade.EState.Idle)
		{
			if (state != EnemyGrenade.EState.Jump1)
			{
				if (state != EnemyGrenade.EState.Attack)
				{
					if (state == EnemyGrenade.EState.Walk1)
					{
						flag = true;
						this.isMove = true;
						base.Flip = (this.pos.x > this._targetX);
					}
				}
				else
				{
					base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
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

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		switch (this._state)
		{
		case EnemyGrenade.EState.Jump1:
		case EnemyGrenade.EState.Jump3:
			this._speed = this.cacheEnemy.Speed * 2f;
			goto IL_B5;
		case EnemyGrenade.EState.Jump2:
			this._speed = this.cacheEnemy.Speed * 2f;
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			goto IL_B5;
		case EnemyGrenade.EState.Walk1:
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
				bool flag = this.pos.x < this._targetX && this._state == EnemyGrenade.EState.Walk1;
				if (flag)
				{
					this.ChangeState();
				}
			}
			else
			{
				this.pos.x = this.pos.x + this._speed * deltaTime;
				bool flag2 = this.pos.x > this._targetX && this._state == EnemyGrenade.EState.Walk1;
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
		if (this._state == EnemyGrenade.EState.Walk1)
		{
			this._state = EnemyGrenade.EState.Jump1;
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
			this._state = EnemyGrenade.EState.Attack;
		}
		else
		{
			EnemyGrenade.EState state = this._state;
			switch (state)
			{
			case EnemyGrenade.EState.Hit_Ice:
			case EnemyGrenade.EState.Idle:
				break;
			case EnemyGrenade.EState.Jump1:
				this._state = EnemyGrenade.EState.Jump2;
				goto IL_381;
			case EnemyGrenade.EState.Jump2:
				this._state = EnemyGrenade.EState.Jump3;
				goto IL_381;
			case EnemyGrenade.EState.Jump3:
				this.isMove = false;
				this._state = EnemyGrenade.EState.Idle;
				goto IL_381;
			case EnemyGrenade.EState.Parachute:
			{
				bool flag = UnityEngine.Random.Range(0, 2) == 1;
				if (flag)
				{
					this._state = EnemyGrenade.EState.Idle;
				}
				else
				{
					this._state = EnemyGrenade.EState.Walk1;
					float num = UnityEngine.Random.Range(1f, 3f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num) : (this.pos.x + num));
				}
				goto IL_381;
			}
			default:
				if (state != EnemyGrenade.EState.Attack)
				{
					goto IL_381;
				}
				break;
			case EnemyGrenade.EState.Walk1:
				this.isMove = false;
				this._state = EnemyGrenade.EState.Idle;
				goto IL_381;
			}
			if (!this.cacheEnemyData.ismove)
			{
				this._state = EnemyGrenade.EState.Idle;
			}
			else if (this.canAttack)
			{
				bool flag2 = UnityEngine.Random.Range(0, 4) == 1;
				if (flag2)
				{
					this._state = EnemyGrenade.EState.Idle;
				}
				else
				{
					float num2 = UnityEngine.Random.Range(2f, 5f);
					this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num2) : (this.pos.x + num2));
					this._state = EnemyGrenade.EState.Walk1;
				}
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
					float num3 = UnityEngine.Random.Range(3f, 4f);
					this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num3 : (-num3));
					this._state = EnemyGrenade.EState.Walk1;
				}
				else
				{
					float num4 = UnityEngine.Random.Range(1f, 2f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num4) : (this.pos.x + num4));
					this._state = EnemyGrenade.EState.Walk1;
				}
			}
		}
		IL_381:
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(15, 3, false);
	}

	protected override void OnAttack()
	{
		if (this._state == EnemyGrenade.EState.Hit_Ice || this._state == EnemyGrenade.EState.Parachute)
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
		if (this._state == EnemyGrenade.EState.Hit_Ice)
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
		if (this._state == EnemyGrenade.EState.Walk1 || this.isAttack)
		{
			base.SetEmptyAnim(0, 0f);
		}
		this._state = EnemyGrenade.EState.Hit_Ice;
		base.PlayAnim(9, 4, false);
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
				base.PlayAnim(2, 0, false);
				return;
			default:
				switch (lastWeapon)
				{
				case EWeapon.ICE:
					goto IL_6B;
				case EWeapon.MGL140:
					goto IL_95;
				}
				base.PlayAnim(1, 0, false);
				return;
			case EWeapon.ROCKET:
				goto IL_95;
			}
			break;
		}
		IL_6B:
		base.PlayAnim(6, 0, false);
		return;
		IL_87:
		base.PlayAnim(3, 0, false);
		return;
		IL_95:
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
		if (e.Data.Name.Contains("thown"))
		{
			float num = Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x);
			GameManager.Instance.audioManager.PlayThrownGrenade();
			int num2 = UnityEngine.Random.Range(3, 8);
			int force = Mathf.Clamp((int)(400f * num / (float)num2), 10, 500);
			Vector3 worldPosition = this._boneGuntip1.GetWorldPosition(this.transform);
			float num3 = this.cacheEnemy.Speed * 4f;
			GameManager.Instance.bombManager.CreateBomb(worldPosition, 1, this.skeletonAnimation.skeleton.FlipX, this.cacheEnemy.Damage, force, 1.5f);
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
			if (this._state != EnemyGrenade.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack_Min":
			if (this._state == EnemyGrenade.EState.Hit_Ice)
			{
				base.ResetTimeReload();
				return;
			}
			if (this.attackCount < this.modeLv)
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

	private EnemyGrenade.EState _state;

	private Bone _boneGuntip1;

	private float _targetX;

	private float _speed;

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
		Idle,
		Jump1,
		Jump2,
		Jump3,
		Parachute,
		PowerUp,
		Walk1
	}
}
