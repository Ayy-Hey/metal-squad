using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class VeTinh : BaseEnemy
{
	private Action<EnemyCallback> EDieCallback { get; set; }

	private void OnValidate()
	{
		try
		{
			if (!this.skeletonAnimation)
			{
				this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
			}
			Spine.Animation[] items = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.anims = new string[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				this.anims[i] = items[i].Name;
			}
			if (!this.data)
			{
				this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
			}
		}
		catch
		{
			UnityEngine.Debug.LogError("setup Skeleton Animation!");
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			GameManager.Instance.ListEnemy.Remove(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void Update()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (!this._pause)
			{
				this.Pause(true);
			}
			return;
		}
		if (this._pause)
		{
			this.Pause(false);
		}
		if (!this.isInit && this._state != VeTinh.EState.Die)
		{
			float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
			if (num <= this.data.datas[this.level].maxVision)
			{
				this.Init();
			}
		}
		this.OnUpdate(Time.deltaTime);
	}

	private void Pause(bool v)
	{
		this._pause = v;
		this.skeletonAnimation.timeScale = (float)((!v) ? 1 : 0);
	}

	public void Init()
	{
		this.cacheEnemy = new Enemy();
		this.cacheEnemyData = new EnemyDataInfo();
		this.HP = (this.cacheEnemy.HP = this.data.datas[this.level].hp * GameMode.Instance.GetMode());
		this.cacheEnemy.Damage = this.data.datas[this.level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[this.level].speed;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.gameObject.SetActive(true);
			this.lineBloodEnemy.Reset();
		}
		this._startPos = this.transform.position;
		this._isInListEnemy = false;
		this._targetX = this._startPos.x + this.protectRange;
		this._timeReload = 0f;
		this._state = VeTinh.EState.Run;
		this._changeState = false;
		this.laser.DisableEff();
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (base.isInCamera == !this._isInListEnemy)
		{
			this._isInListEnemy = !this._isInListEnemy;
			if (this._isInListEnemy)
			{
				GameManager.Instance.ListEnemy.Add(this);
			}
			else
			{
				GameManager.Instance.ListEnemy.Remove(this);
			}
		}
		this._pos = this.transform.position;
		if (!this._changeState)
		{
			this._changeState = true;
			switch (this._state)
			{
			case VeTinh.EState.Attack1:
				this.laser.Off();
				this.laser.SetMainMaterials(1);
				this._loop = false;
				break;
			case VeTinh.EState.Attack2:
				this._loop = true;
				this._attackCount = 0;
				this.laser.ActiveEff();
				break;
			case VeTinh.EState.Attack3:
				this.laser.Off();
				this.laser.DisableEff();
				this._loop = false;
				this._timeReload = this.data.datas[this.level].timeReload;
				break;
			case VeTinh.EState.Run:
				this.laser.SetMainMaterials(0);
				this._loop = true;
				break;
			}
			this.PlayAnim(this._state, this._loop);
		}
		else
		{
			VeTinh.EState state = this._state;
			if (state != VeTinh.EState.Attack2)
			{
				if (state == VeTinh.EState.Run)
				{
					this._hit = Physics2D.Raycast(this.transform.position, Vector2.down, 10f, this.checkMask);
					if (this._hit.transform.CompareTag("Rambo") && this._timeReload <= 0f)
					{
						this.ChangeState();
					}
					else
					{
						this._time += deltaTime;
						this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetX, this.cacheEnemy.Speed * deltaTime);
						this._pos.y = this._startPos.y + this.curves[this.moveType].Evaluate(this._time);
						this.transform.position = this._pos;
						if (this._pos.x == this._targetX)
						{
							this.ResetTargetX();
						}
						this.laser.OnShow(deltaTime, this.laserPointTrans.position, this._hit.point);
					}
				}
			}
			else
			{
				this._hit = Physics2D.Raycast(this.transform.position, Vector2.down, 10f, this.checkMask);
				if (this._hit.transform.CompareTag("Rambo"))
				{
					try
					{
						this._hit.transform.GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage * deltaTime, EWeapon.NONE);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.Log(ex.Message);
					}
				}
				this.laser.OnShow(deltaTime, this.laserPointTrans.position, this._hit.point);
			}
			if (this._timeReload > 0f)
			{
				this._timeReload -= deltaTime;
			}
		}
	}

	private void ResetTargetX()
	{
		if (this._targetX == this._startPos.x)
		{
			this._targetX = this._startPos.x + this.protectRange;
		}
		else
		{
			this._targetX = this._startPos.x;
		}
	}

	private void ChangeState()
	{
		switch (this._state)
		{
		case VeTinh.EState.Hit_Ice:
			this._state = this._oldState;
			break;
		case VeTinh.EState.Attack1:
			this._state = VeTinh.EState.Attack2;
			break;
		case VeTinh.EState.Attack2:
			this._state = VeTinh.EState.Attack3;
			break;
		case VeTinh.EState.Attack3:
			this._state = VeTinh.EState.Run;
			break;
		case VeTinh.EState.Run:
			this._state = VeTinh.EState.Attack1;
			break;
		default:
			this._state = VeTinh.EState.Run;
			break;
		}
		this._changeState = false;
	}

	private void PlayAnim(VeTinh.EState state, bool loop)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.anims[(int)state], loop);
	}

	public override void Hit(float damage)
	{
		Log.Info("___________" + this.HP);
		if (this.HP <= 0f)
		{
			this.Die(true);
			return;
		}
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.anims[0], false);
	}

	private void Die(bool isRambo = true)
	{
		try
		{
			this._state = VeTinh.EState.Die;
			this.isInit = false;
			this.SetDie();
			base.CalculatorToDie(isRambo, false);
			base.gameObject.SetActive(false);
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		if (name != null)
		{
			if (!(name == "attack1") && !(name == "attack3") && !(name == "Hit_ice"))
			{
				if (name == "attack2")
				{
					this._attackCount++;
					if (this._attackCount >= 5)
					{
						this._attackCount = 0;
						this.ChangeState();
					}
				}
			}
			else
			{
				this.ChangeState();
			}
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private int level;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private LaserHandMade laser;

	[SerializeField]
	private Transform laserPointTrans;

	[SerializeField]
	[Range(-20f, 20f)]
	private float protectRange = 3f;

	[SerializeField]
	private int moveType;

	[SerializeField]
	private AnimationCurve[] curves;

	[SerializeField]
	private LayerMask checkMask;

	private VeTinh.EState _state;

	private VeTinh.EState _oldState;

	private bool _changeState;

	private Vector3 _pos;

	private Vector2 _startPos;

	private float _time;

	private bool _loop;

	private float _targetX;

	private int _attackCount;

	private RaycastHit2D _hit;

	private bool _isInListEnemy;

	private bool _pause;

	private float _timeReload;

	private enum EState
	{
		Hit,
		Hit_Ice,
		Attack1,
		Attack2,
		Attack3,
		Run,
		Die
	}
}
