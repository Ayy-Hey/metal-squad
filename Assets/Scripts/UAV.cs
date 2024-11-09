using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class UAV : BaseEnemy
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

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.skeletonAnimation.state.Event -= this.OnEvent;
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			GameManager.Instance.ListEnemy.Remove(this);
			this.dieCallback(this);
		}
		catch
		{
		}
	}

	public void Init(EnemyDataInfo dataInfo, Action<UAV> dieCallback)
	{
		this.dieCallback = dieCallback;
		this.cacheEnemyData = dataInfo;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		this._level = dataInfo.level;
		this.cacheEnemy = new Enemy();
		this.HP = this.data.datas[this._level].hp * GameMode.Instance.GetMode();
		this.cacheEnemy.HP = this.data.datas[this._level].hp;
		this.cacheEnemy.Damage = this.data.datas[this._level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[this._level].speed;
		base.gameObject.SetActive(true);
		this.transform.position = dataInfo.Vt2;
		this._mode = (int)GameMode.Instance.EMode;
		this.TimeCounterUpgradeEnemy = (float)((this._mode >= 2) ? 0 : (15 + 15 * this._mode));
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this._gunTips = new Bone[this.guntips.Length];
		for (int i = 0; i < this._gunTips.Length; i++)
		{
			this._gunTips[i] = this.skeletonAnimation.skeleton.FindBone(this.guntips[i]);
		}
		this.InitLaser();
		this._coolDownTimeHide = 8f;
		this._nextAttack = UAV.EState.Attack1;
		this.SetMoveState();
		this._changeState = false;
		this.isInit = true;
	}

	private void InitLaser()
	{
		this.laser.SetTexture(0);
		this.laser.SetMainTextureTilling(new Vector2(3f, 1f));
		this.laser.speedChangeOffset = 2f;
		this.laser.Off();
		this.tfLaserEff.gameObject.SetActive(false);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit || this._state == UAV.EState.Die)
		{
			return;
		}
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		if (this.TimeCounterUpgradeEnemy > 0f)
		{
			this.TimeCounterUpgradeEnemy -= deltaTime;
			if (this.TimeCounterUpgradeEnemy <= 0f)
			{
				this._mode += ((this._mode >= 2) ? 0 : 1);
				this.TimeCounterUpgradeEnemy = (float)((this._mode >= 2) ? 0 : (15 + 15 * this._mode));
			}
		}
	}

	public void Pause(bool pause)
	{
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
	}

	private void StartState()
	{
		this._changeState = true;
		int track = 0;
		bool loop = false;
		switch (this._state)
		{
		case UAV.EState.Aim_sung:
			track = 2;
			this._targetMove = GameManager.Instance.player.tfOrigin.position;
			this.tfTargetGun.position = this._gunTips[0].GetWorldPosition(this.transform);
			if (this._nextAttack == UAV.EState.Attack2)
			{
				this.tfLaserEff.gameObject.SetActive(true);
			}
			break;
		case UAV.EState.Attack1:
			track = 1;
			loop = true;
			this._totalFire = 3 + this._mode * 2;
			this._countFire = 0;
			break;
		case UAV.EState.Attack2:
			track = 1;
			loop = true;
			break;
		case UAV.EState.Run_Left_1:
		case UAV.EState.Run_Left_2:
		case UAV.EState.Run_Left_3:
		case UAV.EState.Run_Right_1:
		case UAV.EState.Run_Right_2:
		case UAV.EState.Run_Right_3:
		case UAV.EState.Run:
			this._timeMove = 0.5f;
			loop = true;
			break;
		}
		this.PlayAnim(track, loop);
	}

	private void UpdateState(float deltaTime)
	{
		UAV.EState state = this._state;
		switch (state)
		{
		case UAV.EState.Aim_sung:
		{
			this.tfTargetGun.position = Vector3.SmoothDamp(this.tfTargetGun.position, this._targetMove, ref this._velocityMove, 0.3f);
			float num = Vector3.Distance(this.tfTargetGun.position, this._targetMove);
			if (this._nextAttack == UAV.EState.Attack2)
			{
				this.tfLaserEff.position = this._gunTips[0].GetWorldPosition(this.transform);
			}
			if (num < 0.1f)
			{
				this.ChangeState();
			}
			goto IL_24B;
		}
		case UAV.EState.Attack1:
			this.tfTargetGun.position = Vector3.SmoothDamp(this.tfTargetGun.position, GameManager.Instance.player.tfOrigin.position, ref this._velocityMove, 0.3f);
			goto IL_24B;
		case UAV.EState.Attack2:
		{
			this.tfTargetGun.position = Vector3.SmoothDamp(this.tfTargetGun.position, GameManager.Instance.player.tfOrigin.position, ref this._velocityMove, 0.5f);
			this._bulletPos = this._gunTips[0].GetWorldPosition(this.transform);
			this._bulletDirection = this._bulletPos - this._gunTips[1].GetWorldPosition(this.transform);
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this._bulletPos, this._bulletDirection, 25f, this.playerMask);
			if (raycastHit2D.transform)
			{
				try
				{
					raycastHit2D.transform.GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage * 0.1f, EWeapon.NONE);
				}
				catch
				{
				}
			}
			this.tfLaserEff.position = this._bulletPos;
			this._bulletDirection = this._bulletPos + this._bulletDirection.normalized * 25f;
			this.laser.OnShow(deltaTime, this._bulletPos, this._bulletDirection);
			goto IL_24B;
		}
		default:
			switch (state)
			{
			case UAV.EState.Run_Right_2:
			case UAV.EState.Run:
				break;
			case UAV.EState.Run_Right_3:
				goto IL_24B;
			default:
				goto IL_24B;
			}
			break;
		case UAV.EState.Run_Left_2:
			break;
		}
		this.MoveToTarget();
		IL_24B:
		if (this._coolDownAttack > 0f)
		{
			this._coolDownAttack -= deltaTime;
		}
	}

	private void MoveToTarget()
	{
		this._pos = this.transform.position;
		this._pos = Vector3.SmoothDamp(this._pos, this._targetMove, ref this._velocityMove, this._timeMove);
		this.transform.position = this._pos;
		float num = Vector3.Distance(this._pos, this._targetMove);
		if (num <= 0.1f)
		{
			this.ChangeState();
		}
	}

	private void ChangeState()
	{
		switch (this._state)
		{
		case UAV.EState.Aim_sung:
			this._state = this._nextAttack;
			this._nextAttack = ((this._nextAttack != UAV.EState.Attack1) ? UAV.EState.Attack1 : UAV.EState.Attack2);
			break;
		case UAV.EState.Attack1:
			this.skeletonAnimation.state.SetEmptyAnimations(0f);
			this.SetMoveState();
			break;
		case UAV.EState.Attack2:
			this.skeletonAnimation.state.SetEmptyAnimations(0f);
			this.SetMoveState();
			this.tfLaserEff.gameObject.SetActive(false);
			this.laser.Off();
			break;
		case UAV.EState.Idle:
			if (!base.isInCamera || GameManager.Instance.player.isInVisible)
			{
				this.SetMoveState();
			}
			else if (this._coolDownAttack <= 0f)
			{
				this._state = UAV.EState.Aim_sung;
			}
			break;
		case UAV.EState.Run_Left_2:
		case UAV.EState.Run_Right_2:
		case UAV.EState.Run:
			if (this._coolDownAttack <= 0f && !GameManager.Instance.player.isInVisible)
			{
				this._state = UAV.EState.Aim_sung;
			}
			else if (base.isInCamera)
			{
				this._state = UAV.EState.Idle;
				this.ResetCoolDownAttack();
			}
			else
			{
				this.SetMoveState();
			}
			break;
		}
		this._changeState = false;
	}

	private void ResetCoolDownAttack()
	{
		if (this._coolDownAttack <= 0f)
		{
			this._coolDownAttack = this.cacheEnemy.Time_Reload_Attack - (float)this._mode * this.cacheEnemy.Time_Reload_Attack / 4f;
		}
	}

	private void SetMoveState()
	{
		this.CachePos();
		this.ResetCoolDownAttack();
		float num = CameraController.Instance.camPos.x - 6f;
		float num2 = num + 12f;
		this._targetMove.x = UnityEngine.Random.Range(num, num2);
		this._targetMove.y = UnityEngine.Random.Range(CameraController.Instance.camPos.y - 2f, CameraController.Instance.camPos.y + 2f);
		float num3 = Mathf.Abs(this._targetMove.y - this._playerPos.y);
		float num4 = Mathf.Abs(this._targetMove.x - this._playerPos.x);
		if (num3 < 2f && num4 < 2f)
		{
			float num5 = UnityEngine.Random.Range(2f, 3f);
			num5 = ((UnityEngine.Random.Range(0, 2) != 1) ? (-num5) : num5);
			this._targetMove.x = this._playerPos.x + num5;
			this._targetMove.x = ((this._targetMove.x <= num2 && this._targetMove.x >= num) ? this._targetMove.x : (this._playerPos.x - num5));
		}
		float num6 = this._targetMove.x - this._pos.x;
		if (Mathf.Abs(num6) < 0.5f)
		{
			this._state = UAV.EState.Run;
		}
		else if (num6 < 0f)
		{
			this._state = UAV.EState.Run_Left_2;
		}
		else
		{
			this._state = UAV.EState.Run_Right_2;
		}
	}

	private void CachePos()
	{
		this._pos = this.transform.position;
		this._playerPos = GameManager.Instance.player.transform.position;
	}

	private void PlayAnim(int track, bool loop)
	{
		int num = (int)this._state;
		num = ((num != 12) ? num : 5);
		this.skeletonAnimation.state.SetAnimation(track, this.anims[num], loop);
	}

	public override void Hit(float damage)
	{
		if (this.HP < 0f)
		{
			this._state = UAV.EState.Die;
			this.PlayAnim(0, false);
			if (base.isInCamera)
			{
				GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
			}
			base.gameObject.SetActive(false);
			base.CalculatorToDie(true, false);
		}
		else
		{
			this.skeletonAnimation.state.SetAnimation(3, this.anims[4], false);
		}
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		UAV.EState state = this._state;
		if (state == UAV.EState.Attack1)
		{
			this._bulletPos = this._gunTips[0].GetWorldPosition(this.transform);
			this._bulletDirection = this._bulletPos - this._gunTips[1].GetWorldPosition(this.transform);
			this._bullet = GameManager.Instance.bulletManager.CreateBulletEnemy(5, this._bulletDirection, this._bulletPos, this.cacheEnemy.Damage, this.cacheEnemy.Speed * 3f, 0f);
			this._bullet.transform.localScale = Vector3.one * 1.5f;
			this._bullet.spriteRenderer.flipX = false;
			this._bullet.SetSkipGround();
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		if (name != null)
		{
			if (!(name == "attack-1"))
			{
				if (!(name == "attack-2"))
				{
					if (name == "idle")
					{
						this.ChangeState();
					}
				}
				else
				{
					this.tfLaserEff.gameObject.SetActive(false);
					this.laser.Off();
					this.ChangeState();
				}
			}
			else
			{
				this._countFire++;
				if (this._countFire >= this._totalFire)
				{
					this.ChangeState();
				}
			}
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string[] guntips;

	[SerializeField]
	private Transform tfTargetGun;

	[SerializeField]
	private LaserHandMade laser;

	[SerializeField]
	private Transform tfLaserEff;

	[SerializeField]
	private LayerMask playerMask;

	private Action<UAV> dieCallback;

	private UAV.EState _state;

	private UAV.EState _nextAttack;

	private bool _changeState;

	private Spine.Animation[] anims;

	private Bone[] _gunTips;

	private int _level;

	private int _mode;

	private int _totalFire;

	private int _countFire;

	private int _countLase;

	private BulletEnemy _bullet;

	private Vector3 _bulletPos;

	private Vector3 _bulletDirection;

	private Vector3 _targetMove;

	private Vector3 _velocityMove;

	private float _timeMove;

	private float _coolDownAttack;

	private Vector3 _pos;

	private Vector3 _playerPos;

	private float _coolDownTimeHide;

	private enum EState
	{
		Aim_sung,
		Attack1,
		Attack2,
		Die,
		Hit,
		Idle,
		Run_Left_1,
		Run_Left_2,
		Run_Left_3,
		Run_Right_1,
		Run_Right_2,
		Run_Right_3,
		Run
	}
}
