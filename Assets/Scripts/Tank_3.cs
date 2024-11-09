using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Tank_3 : BaseEnemy2
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

	public override void Init(EnemyDataInfo enemyDataInfo, Action hideCallback)
	{
		base.Init(enemyDataInfo, hideCallback);
		this._boneGun1_1 = this.skeletonAnimation.skeleton.FindBone(this.strBoneGun1_1);
		this._boneGun1_2 = this.skeletonAnimation.skeleton.FindBone(this.strBoneGun1_2);
		this._boneGun2_1 = this.skeletonAnimation.skeleton.FindBone(this.strBoneGun2_1);
		this._boneGun2_2 = this.skeletonAnimation.skeleton.FindBone(this.strBoneGun2_2);
		this._boneTarget = this.skeletonAnimation.skeleton.FindBone(this.strBoneTarget);
		this._idAttack = 0;
		this._countAttack = 0;
		this._state = Tank_3.EState.Idle;
		this.ChangeState();
	}

	protected override void StartState()
	{
		base.StartState();
		int track = 0;
		bool loop = false;
		switch (this._state)
		{
		case Tank_3.EState.Aim:
			track = 1;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case Tank_3.EState.Attack1:
		case Tank_3.EState.Idle:
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		case Tank_3.EState.Attack3:
		{
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			float damage = Mathf.Max(this.cacheEnemy.Damage / 20f, 3f);
			this.flameAttack.Active(damage, false, null);
			Vector3 worldPosition = this._boneGun1_1.GetWorldPosition(this.transform);
			Vector3 toDirection = worldPosition - this._boneGun1_2.GetWorldPosition(this.transform);
			this.flameAttack.transform.position = worldPosition;
			this.flameAttack.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
			break;
		}
		case Tank_3.EState.Walk01:
		case Tank_3.EState.Walk02:
		{
			loop = true;
			this.isMove = true;
			float num = UnityEngine.Random.Range(2f, 4f);
			this._targetX = this.pos.x + ((this.pos.x <= CameraController.Instance.camPos.x) ? num : (-num));
			break;
		}
		}
		base.PlayAnim((int)this._state, track, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		switch (this._state)
		{
		case Tank_3.EState.Aim:
		{
			this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
			this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
			this._targetLockPos.x = ((!base.Flip) ? Mathf.Max(this.pos.x, this._targetLockPos.x) : Mathf.Min(this._targetLockPos.x, this.pos.x));
			this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.2f);
			this._boneTarget.SetPosition(this._targetPos);
			bool flag = Vector3.Distance(this._targetPos, this._targetLockPos) < 0.5f;
			if (flag)
			{
				this.ChangeState();
			}
			break;
		}
		case Tank_3.EState.Attack2:
			this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
			this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
			this._targetLockPos.x = ((!base.Flip) ? Mathf.Max(this.pos.x, this._targetLockPos.x) : Mathf.Min(this._targetLockPos.x, this.pos.x));
			this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.3f);
			this._boneTarget.SetPosition(this._targetPos);
			break;
		}
		if (this.isMove)
		{
			bool flag2 = Mathf.Abs(this.pos.x - this._targetX) < 0.1f;
			if (flag2)
			{
				this.isMove = false;
				this.ChangeState();
				return;
			}
			this.pos.x = Mathf.MoveTowards(this.pos.x, this._targetX, this.cacheEnemy.Speed * deltaTime);
		}
	}

	protected override void ChangeState()
	{
		if (this.isAttack)
		{
			if (this._state == Tank_3.EState.Aim)
			{
				this._state = Tank_3.EState.Attack2;
			}
			else
			{
				this._idAttack = UnityEngine.Random.Range(0, 3);
				int idAttack = this._idAttack;
				if (idAttack != 0)
				{
					if (idAttack != 1)
					{
						this._state = Tank_3.EState.Attack3;
					}
					else
					{
						this._state = Tank_3.EState.Attack1;
					}
				}
				else
				{
					this._state = Tank_3.EState.Aim;
				}
			}
		}
		else
		{
			switch (this._state)
			{
			case Tank_3.EState.Attack1:
			case Tank_3.EState.Attack2:
			case Tank_3.EState.Attack3:
			case Tank_3.EState.Walk01:
			case Tank_3.EState.Walk02:
			case Tank_3.EState.Idle:
			{
				this.isMove = false;
				float x = GameManager.Instance.player.transform.position.x;
				bool flag = Mathf.Abs(this.pos.x - x) > this.cacheEnemy.Vision_Max;
				bool flag2 = this.cacheEnemyData.ismove && this.meshRenderer.enabled && (!base.isInCamera || flag);
				if (flag2)
				{
					this._state = ((base.Flip != this.pos.x > x) ? Tank_3.EState.Walk02 : Tank_3.EState.Walk01);
				}
				else
				{
					this._state = Tank_3.EState.Idle;
				}
				break;
			}
			}
		}
		base.ChangeState();
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		this.isMove = false;
		this.ChangeState();
	}

	protected override void Hit()
	{
		base.PlayAnim(5, 3, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		this._state = Tank_3.EState.Die;
		base.PlayAnim((int)this._state, 0, false);
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
			if (!(text == "Attack"))
			{
				if (text == "Attack2")
				{
					Vector3 worldPosition = this._boneGun2_1.GetWorldPosition(this.transform);
					Vector3 v = worldPosition - this._boneGun2_2.GetWorldPosition(this.transform);
					float speed = Mathf.Min(this.cacheEnemy.Speed * 3f, 7f);
					GameManager.Instance.bulletManager.CreateBulletEnemy(8, v, worldPosition, this.cacheEnemy.Damage, speed, 0f).spriteRenderer.flipX = false;
				}
			}
			else
			{
				Vector3 worldPosition2 = this._boneGun1_1.GetWorldPosition(this.transform);
				Vector3 direction = worldPosition2 - this._boneGun1_2.GetWorldPosition(this.transform);
				GameManager.Instance.bulletManager.CreateRocketBossType1(this.cacheEnemy.Damage * 1.2f, this.cacheEnemy.Speed, direction, worldPosition2, GameManager.Instance.player.tfOrigin, 1f);
			}
		}
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "Attack") && !(text == "Attack2"))
			{
				if (!(text == "Attack3"))
				{
					if (!(text == "Die"))
					{
						if (text == "idel")
						{
							this.ChangeState();
						}
					}
					else
					{
						base.gameObject.SetActive(false);
					}
				}
				else
				{
					this._countAttack++;
					bool flag = this._countAttack > this.modeLv + 4;
					if (flag)
					{
						this.flameAttack.Deactive();
						this._countAttack = 0;
						base.ResetTimeReload();
						this.ChangeState();
					}
					else
					{
						base.PlayAnim((int)this._state, 0, false);
					}
				}
			}
			else
			{
				this._countAttack++;
				bool flag = (this._state != Tank_3.EState.Attack1) ? (this._countAttack > this.modeLv + 1) : (this._countAttack > this.modeLv);
				if (flag)
				{
					this._countAttack = 0;
					base.ResetTimeReload();
					if (this._state == Tank_3.EState.Attack2)
					{
						base.SetEmptyAnim(1, 0f);
					}
					this.ChangeState();
				}
				else
				{
					base.PlayAnim((int)this._state, 0, false);
				}
			}
		}
	}

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun1_1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun1_2;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun2_1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun2_2;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneTarget;

	[SerializeField]
	private AttackBox flameAttack;

	private Tank_3.EState _state;

	private Bone _boneGun1_1;

	private Bone _boneGun1_2;

	private Bone _boneGun2_1;

	private Bone _boneGun2_2;

	private Bone _boneTarget;

	private float _targetX;

	private Vector3 _targetLockPos;

	private Vector3 _targetPos;

	private Vector3 _veloSmoothTarget;

	private int _idAttack;

	private int _countAttack;

	private enum EState
	{
		Aim,
		Attack1,
		Attack2,
		Attack3,
		Die,
		Hit,
		Jump,
		Walk01,
		Walk02,
		Idle,
		Open,
		Open2
	}
}
