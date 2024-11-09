using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyMiniGun2 : BaseEnemy2
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
			if (this._state == EnemyMiniGun2.EState.Day1)
			{
				if (this.dayDu)
				{
					this.dayDu.Off();
					this.dayDu = null;
				}
				base.Gravity = 2f;
			}
			this._state = EnemyMiniGun2.EState.Jump1;
			this.isChangeState = false;
			this.isMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		EnemyMiniGun2.EState state = this._state;
		if (state != EnemyMiniGun2.EState.Day1)
		{
			if (state != EnemyMiniGun2.EState.Jump3)
			{
				if (state == EnemyMiniGun2.EState.Walk)
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
		if (this._boneGuntip1 == null)
		{
			this._boneGuntip1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip1);
			this._boneGuntip2 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip2);
			this._boneGuntip3 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip3);
			this._boneGuntip4 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGuntip4);
		}
		if (this.cacheEnemyData.ismove)
		{
			this._state = EnemyMiniGun2.EState.Idle;
			this.ChangeState();
		}
		else
		{
			this._state = EnemyMiniGun2.EState.Walk;
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
		bool flag = true;
		this._state = EnemyMiniGun2.EState.Day1;
		if (flag)
		{
			this._state = EnemyMiniGun2.EState.Day1;
			Vector3 pos = this.pos;
			pos.x += ((!base.Flip) ? 0.2f : -0.2f);
			this.dayDu = EnemyManager.Instance.CreateDayDu(pos);
		}
		this.isChangeState = true;
		base.PlayAnim((int)this._state, 0, true);
	}

	protected override void StartState()
	{
		base.StartState();
		bool flag = false;
		EnemyMiniGun2.EState state = this._state;
		switch (state)
		{
		case EnemyMiniGun2.EState.Idle:
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case EnemyMiniGun2.EState.Jump1:
		{
			this.isMove = true;
			float d = Mathf.Sqrt(Mathf.Abs(5f * this.rigidbody2D.gravityScale * Physics2D.gravity.y));
			this.rigidbody2D.velocity = Vector2.up * d;
			break;
		}
		default:
			if (state == EnemyMiniGun2.EState.Attack)
			{
				base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			}
			break;
		case EnemyMiniGun2.EState.Walk:
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
		switch (this._state)
		{
		case EnemyMiniGun2.EState.Jump1:
		case EnemyMiniGun2.EState.Jump3:
			this.pos.x = this.pos.x + ((!base.Flip) ? (this.cacheEnemy.Speed * deltaTime) : (-this.cacheEnemy.Speed * deltaTime));
			break;
		case EnemyMiniGun2.EState.Jump2:
			this.pos.x = this.pos.x + ((!base.Flip) ? (this.cacheEnemy.Speed * deltaTime) : (-this.cacheEnemy.Speed * deltaTime));
			if (this.rigidbody2D.velocity.y < 0f)
			{
				this.ChangeState();
			}
			break;
		case EnemyMiniGun2.EState.Walk:
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

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		if (this._state == EnemyMiniGun2.EState.Walk)
		{
			this._state = EnemyMiniGun2.EState.Jump1;
			this.isChangeState = false;
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		bool flag = this.isAttack && base.isInCamera;
		if (flag)
		{
			this._state = EnemyMiniGun2.EState.Attack;
			base.ChangeState();
			return;
		}
		switch (this._state)
		{
		case EnemyMiniGun2.EState.Jump1:
			this._state = EnemyMiniGun2.EState.Jump2;
			goto IL_2ED;
		case EnemyMiniGun2.EState.Jump2:
			this._state = EnemyMiniGun2.EState.Jump3;
			goto IL_2ED;
		case EnemyMiniGun2.EState.Jump3:
			this.isMove = false;
			this._state = EnemyMiniGun2.EState.Idle;
			goto IL_2ED;
		case EnemyMiniGun2.EState.Walk:
			this.isMove = false;
			this._state = EnemyMiniGun2.EState.Idle;
			goto IL_2ED;
		case EnemyMiniGun2.EState.Day1:
			this._state = EnemyMiniGun2.EState.Day2;
			goto IL_2ED;
		}
		if (!this.cacheEnemyData.ismove)
		{
			this._state = EnemyMiniGun2.EState.Idle;
		}
		else if (this.canAttack)
		{
			float num = UnityEngine.Random.Range(2f, 5f);
			this._targetX = ((this.pos.x >= CameraController.Instance.camPos.x) ? (this.pos.x - num) : (this.pos.x + num));
			this._state = EnemyMiniGun2.EState.Walk;
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
				this._state = EnemyMiniGun2.EState.Walk;
			}
			else
			{
				float num3 = UnityEngine.Random.Range(1f, 2f);
				this._targetX = ((!base.Flip) ? (this.pos.x - num3) : (this.pos.x + num3));
				this._state = EnemyMiniGun2.EState.Walk;
			}
		}
		IL_2ED:
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(12, 3, false);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == EnemyMiniGun2.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(6, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(5, 4, false);
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
		this._state = EnemyMiniGun2.EState.Hit_Ice;
		base.PlayAnim(7, 4, false);
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
				base.PlayAnim(3, 0, false);
				return;
			}
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(2, 0, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(1, 0, false);
				return;
			}
		}
		base.PlayAnim(4, 0, false);
	}

	protected override void Disable()
	{
		base.Disable();
	}

	protected override void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.OnEvent(trackEntry, e);
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "shoot1"))
			{
				if (name == "shoot2")
				{
					Vector3 worldPosition = this._boneGuntip3.GetWorldPosition(this.transform);
					Vector3 direction = worldPosition - this._boneGuntip4.GetWorldPosition(this.transform);
					float speed = Mathf.Min(this.cacheEnemy.Speed * 7f, 16f);
					GameManager.Instance.bulletManager.CreateBulletBeSung(speed, this.cacheEnemy.Damage, worldPosition, direction, 0.7f);
				}
			}
			else
			{
				Vector3 worldPosition = this._boneGuntip1.GetWorldPosition(this.transform);
				Vector3 direction = worldPosition - this._boneGuntip2.GetWorldPosition(this.transform);
				float speed = Mathf.Min(this.cacheEnemy.Speed * 7f, 16f);
				GameManager.Instance.bulletManager.CreateBulletBeSung(speed, this.cacheEnemy.Damage, worldPosition, direction, 0.7f);
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
			if (this._state != EnemyMiniGun2.EState.Hit_Ice)
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
			else
			{
				base.PlayAnim((int)this._state, 0, false);
			}
			break;
		case "Death":
		case "Death-Elec":
		case "Death_Fire":
		case "Death_Ice":
			base.gameObject.SetActive(false);
			break;
		}
	}

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGuntip1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGuntip2;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGuntip3;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGuntip4;

	private EnemyMiniGun2.EState _state;

	private Bone _boneGuntip1;

	private Bone _boneGuntip2;

	private Bone _boneGuntip3;

	private Bone _boneGuntip4;

	private float _targetX;

	private DayDu dayDu;

	private enum EState
	{
		Attack,
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
