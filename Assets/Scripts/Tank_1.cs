using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Tank_1 : BaseEnemy2
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
		this._countAttack = 0;
		this._state = Tank_1.EState.Idle;
		this.ChangeState();
	}

	protected override void StartState()
	{
		base.StartState();
		int track = 0;
		bool loop = false;
		Tank_1.EState state = this._state;
		switch (state)
		{
		case Tank_1.EState.Walk01:
		case Tank_1.EState.Walk02:
		{
			loop = true;
			this.isMove = true;
			float num = UnityEngine.Random.Range(2f, 4f);
			this._targetX = this.pos.x + ((this.pos.x <= CameraController.Instance.camPos.x) ? num : (-num));
			goto IL_BE;
		}
		case Tank_1.EState.Idle:
			break;
		default:
			if (state != Tank_1.EState.Attack)
			{
				goto IL_BE;
			}
			break;
		}
		base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
		IL_BE:
		base.PlayAnim((int)this._state, track, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		Tank_1.EState state = this._state;
		if (state != Tank_1.EState.Attack)
		{
			if (state != Tank_1.EState.Walk01)
			{
				if (state != Tank_1.EState.Walk02)
				{
				}
			}
		}
		if (this.isMove)
		{
			bool flag = Mathf.Abs(this.pos.x - this._targetX) < 0.1f;
			if (flag)
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
			this._state = Tank_1.EState.Attack;
		}
		else
		{
			Tank_1.EState state = this._state;
			switch (state)
			{
			case Tank_1.EState.Walk01:
			case Tank_1.EState.Walk02:
			case Tank_1.EState.Idle:
				break;
			default:
				if (state != Tank_1.EState.Attack)
				{
					goto IL_EC;
				}
				break;
			}
			float x = GameManager.Instance.player.transform.position.x;
			bool flag = Mathf.Abs(this.pos.x - x) > this.cacheEnemy.Vision_Max;
			bool flag2 = this.cacheEnemyData.ismove && this.meshRenderer.enabled && (!base.isInCamera || flag);
			if (flag2)
			{
				this._state = ((base.Flip != this.pos.x > x) ? Tank_1.EState.Walk02 : Tank_1.EState.Walk01);
			}
			else
			{
				this._state = Tank_1.EState.Idle;
			}
		}
		IL_EC:
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
		base.PlayAnim(2, 3, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		this._state = Tank_1.EState.Die;
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
			if (text == "Attack")
			{
				Vector3 worldPosition = this._boneGun1_1.GetWorldPosition(this.transform);
				Vector3 direction = worldPosition - this._boneGun1_2.GetWorldPosition(this.transform);
				GameManager.Instance.bulletManager.CreateRocketBossType1(this.cacheEnemy.Damage, this.cacheEnemy.Speed, direction, worldPosition, GameManager.Instance.player.tfOrigin, 1f);
			}
		}
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "Attack"))
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
				bool flag = this._countAttack > this.modeLv;
				if (flag)
				{
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
	}

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun1_1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun1_2;

	private Tank_1.EState _state;

	private Bone _boneGun1_1;

	private Bone _boneGun1_2;

	private float _targetX;

	private int _countAttack;

	private enum EState
	{
		Attack,
		Die,
		Hit,
		Jump,
		Walk01,
		Walk02,
		Idle,
		Open
	}
}
