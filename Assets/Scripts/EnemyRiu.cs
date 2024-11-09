using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyRiu : BaseEnemy2
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
			if (this._state == EnemyRiu.EState.Day1)
			{
				if (this.dayDu)
				{
					this.dayDu.Off();
					this.dayDu = null;
				}
				base.Gravity = 2f;
			}
			this._state = EnemyRiu.EState.Jump1;
			this.isChangeState = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		EnemyRiu.EState state = this._state;
		if (state != EnemyRiu.EState.Day1)
		{
			if (state != EnemyRiu.EState.Jump3)
			{
				if (state == EnemyRiu.EState.Walk)
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
			if (this.dayDu)
			{
				this.dayDu.Off();
				this.dayDu = null;
			}
			base.Gravity = 2f;
			this.ChangeState();
		}
	}

	public override void Init(EnemyDataInfo enemyDataInfo, Action hideCallback)
	{
		base.Init(enemyDataInfo, hideCallback);
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemyRiu.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyRiu.EState.Walk;
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
		this._state = EnemyRiu.EState.Day1;
		this._state = EnemyRiu.EState.Day1;
		Vector3 pos = this.pos;
		pos.x += ((!base.Flip) ? 0.2f : -0.2f);
		this.dayDu = EnemyManager.Instance.CreateDayDu(pos);
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		EnemyRiu.EState state = this._state;
		switch (state)
		{
		case EnemyRiu.EState.Idle:
			this.isMove = false;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyRiu.EState.Jump1:
		{
			this.isMove = true;
			float d = Mathf.Sqrt(Mathf.Abs(5f * this.rigidbody2D.gravityScale * Physics2D.gravity.y));
			this.rigidbody2D.velocity = Vector2.up * d;
			break;
		}
		default:
			if (state != EnemyRiu.EState.Attack01)
			{
				if (state == EnemyRiu.EState.Attack02)
				{
					this.isMove = false;
					this.isAttack1 = false;
					base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
					Vector3 localPosition = this.colliderKnife2.transform.localPosition;
					localPosition.x = ((!base.Flip) ? Mathf.Abs(localPosition.x) : (-Mathf.Abs(localPosition.x)));
					this.colliderKnife2.transform.localPosition = localPosition;
				}
			}
			else
			{
				this.isMove = false;
				this.isAttack1 = false;
				base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
				Vector3 localPosition2 = this.colliderKnife.transform.localPosition;
				localPosition2.x = ((!base.Flip) ? Mathf.Abs(localPosition2.x) : (-Mathf.Abs(localPosition2.x)));
				this.colliderKnife.transform.localPosition = localPosition2;
			}
			break;
		case EnemyRiu.EState.Walk:
			flag = true;
			this.isMove = true;
			base.Flip = (this.pos.x > this._targetX);
			break;
		}
		int state2 = (int)this._state;
		bool loop = flag;
		base.PlayAnim(state2, 0, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		EnemyRiu.EState state = this._state;
		switch (state)
		{
		case EnemyRiu.EState.Jump1:
		case EnemyRiu.EState.Jump3:
			this.pos.x = this.pos.x + ((!base.Flip) ? (this.cacheEnemy.Speed * deltaTime) : (-this.cacheEnemy.Speed * deltaTime));
			break;
		case EnemyRiu.EState.Jump2:
			this.pos.x = this.pos.x + ((!base.Flip) ? (this.cacheEnemy.Speed * deltaTime) : (-this.cacheEnemy.Speed * deltaTime));
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			break;
		default:
			if (state != EnemyRiu.EState.Attack01)
			{
				if (state == EnemyRiu.EState.Attack02)
				{
					if (this.isMove)
					{
						this.pos.x = this.pos.x + ((!base.Flip) ? (this.cacheEnemy.Speed * deltaTime) : (-this.cacheEnemy.Speed * deltaTime));
					}
				}
			}
			break;
		case EnemyRiu.EState.Walk:
		{
			this.pos.x = Mathf.MoveTowards(this.pos.x, this._targetX, this.cacheEnemy.Speed * deltaTime);
			bool flag = Mathf.Abs(this.pos.x - this._targetX) <= 0.1f || Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x) < 1f;
			if (flag)
			{
				this.isMove = false;
				this.ChangeState();
			}
			break;
		}
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		bool flag = this._state != EnemyRiu.EState.Jump1 && this._state != EnemyRiu.EState.Jump2 && Vector3.Distance(this.tfOrigin.position, GameManager.Instance.player.tfOrigin.position) <= 2f;
		if (this.isAttack && flag)
		{
			this._state = ((this.modeLv != 0 && UnityEngine.Random.Range(0, 3) != 0) ? EnemyRiu.EState.Attack02 : EnemyRiu.EState.Attack01);
		}
		else
		{
			EnemyRiu.EState state = this._state;
			switch (state)
			{
			case EnemyRiu.EState.Hit_Ice:
			case EnemyRiu.EState.Idle:
			case EnemyRiu.EState.Day2:
				break;
			case EnemyRiu.EState.Jump1:
				this._state = EnemyRiu.EState.Jump2;
				goto IL_37E;
			case EnemyRiu.EState.Jump2:
				this._state = EnemyRiu.EState.Jump3;
				goto IL_37E;
			case EnemyRiu.EState.Jump3:
				this.isMove = false;
				this._state = EnemyRiu.EState.Idle;
				goto IL_37E;
			default:
				if (state != EnemyRiu.EState.Attack01 && state != EnemyRiu.EState.Attack02)
				{
					goto IL_37E;
				}
				break;
			case EnemyRiu.EState.Walk:
				this.isMove = false;
				this._state = EnemyRiu.EState.Idle;
				goto IL_37E;
			case EnemyRiu.EState.Day1:
				this._state = EnemyRiu.EState.Day2;
				goto IL_37E;
			}
			if (!this.cacheEnemyData.ismove)
			{
				this._state = EnemyRiu.EState.Idle;
			}
			else if (this.canAttack)
			{
				this._targetX = GameManager.Instance.player.transform.position.x + UnityEngine.Random.Range(-1f, 1f);
				this._state = EnemyRiu.EState.Walk;
			}
			else if (!base.isInCamera)
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
					float num = UnityEngine.Random.Range(1f, 3f);
					this._targetX = CameraController.Instance.camPos.x + ((this.pos.x >= CameraController.Instance.camPos.x) ? num : (-num));
					this._state = EnemyRiu.EState.Walk;
				}
				else
				{
					float num2 = UnityEngine.Random.Range(1f, 2f);
					this._targetX = ((!base.Flip) ? (this.pos.x - num2) : (this.pos.x + num2));
					this._state = EnemyRiu.EState.Walk;
				}
			}
			else
			{
				this._targetX = GameManager.Instance.player.transform.position.x;
				this._state = EnemyRiu.EState.Walk;
			}
		}
		IL_37E:
		base.ChangeState();
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		if (this._state == EnemyRiu.EState.Walk)
		{
			this._state = EnemyRiu.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(13, 3, false);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyRiu.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(7, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(6, 4, false);
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
		this._state = EnemyRiu.EState.Hit_Ice;
		base.PlayAnim(8, 4, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.GRENADE_MOLOYOV || lastWeapon == EWeapon.FLAME)
			{
				base.PlayAnim(4, 0, false);
				return;
			}
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(3, 0, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(2, 0, false);
				return;
			}
		}
		base.PlayAnim(5, 0, false);
	}

	protected override void Disable()
	{
		base.Disable();
	}

	protected override void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.OnEvent(trackEntry, e);
		string text = e.ToString();
		if (text != null)
		{
			if (!(text == "shoot"))
			{
				if (!(text == "shoot2"))
				{
					if (!(text == "star"))
					{
						if (text == "end")
						{
							this.isMove = false;
						}
					}
					else
					{
						this.isMove = true;
					}
				}
				else
				{
					this.colliderKnife2.OverlapCollider(this.contactFilter, this.colliderKnifeContacts);
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
			else
			{
				if (this.isAttack1)
				{
					return;
				}
				this.isAttack1 = true;
				this.colliderKnife.OverlapCollider(this.contactFilter, this.colliderKnifeContacts);
				for (int j = 0; j < this.colliderKnifeContacts.Length; j++)
				{
					if (this.colliderKnifeContacts[j] != null)
					{
						try
						{
							this.colliderKnifeContacts[j].GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
						}
						catch
						{
						}
					}
					this.colliderKnifeContacts[j] = null;
				}
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
			if (this._state != EnemyRiu.EState.Hit_Ice)
			{
				this.ChangeState();
			}
			break;
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack-1":
		case "Attack-2":
			base.ResetTimeReload();
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

	[SerializeField]
	private Collider2D colliderKnife;

	[SerializeField]
	private Collider2D colliderKnife2;

	[SerializeField]
	private ContactFilter2D contactFilter;

	private EnemyRiu.EState _state;

	private float _targetX;

	private Collider2D[] colliderKnifeContacts = new Collider2D[3];

	private DayDu dayDu;

	private bool isAttack1;

	private enum EState
	{
		Attack01,
		Attack02,
		Death,
		Death_Elec,
		Death_Fire,
		Death_Ice,
		Hit,
		Hit_Elec,
		Hit_Ice,
		Idle,
		Jump1,
		Jump2,
		Jump3,
		PowerUp,
		Walk,
		Day1,
		Day2
	}
}
