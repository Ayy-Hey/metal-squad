using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Bee3 : BaseEnemy
{
	private void OnValidate()
	{
		if (!this.skeletonAnimation)
		{
			this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
		}
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.skeletonAnimation.state.Event -= this.OnEvent;
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			if (this.OnEnemyDeaded != null)
			{
				this.OnEnemyDeaded();
				this.OnEnemyDeaded = null;
			}
			GameManager.Instance.ListEnemy.Remove(this);
			if (this.dieCallback != null)
			{
				this.dieCallback(this);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public void Pause(bool pause)
	{
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
	}

	public void Init(Action<Bee3> dieCallback)
	{
		this.dieCallback = dieCallback;
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
			if (this.cacheEnemyData == null)
			{
				this.cacheEnemyData = new EnemyDataInfo();
			}
		}
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		base.gameObject.SetActive(true);
		this.bodyCollider2D.enabled = true;
		this.HP = (this.cacheEnemy.HP = this.data.datas[this.cacheEnemyData.level].hp * GameMode.Instance.GetMode());
		this.cacheEnemy.Damage = this.data.datas[this.cacheEnemyData.level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[this.cacheEnemyData.level].speed;
		this.cacheEnemy.Time_Reload_Attack = this.data.datas[this.cacheEnemyData.level].timeReload;
		this._targetBone = this.skeletonAnimation.skeleton.FindBone(this.targetBone);
		this._gunBone1 = this.skeletonAnimation.skeleton.FindBone(this.gunTip1);
		this._gunBone2 = this.skeletonAnimation.skeleton.FindBone(this.gunTip2);
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.transform.position = this.cacheEnemyData.Vt2;
		base.gameObject.layer = 13;
		this._coolDownHide = 2f;
		this.hasShow = false;
		this._state = Bee3.EState.Walk;
		this._changeState = false;
		this._begin = true;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit || this._state == Bee3.EState.Die)
		{
			return;
		}
		base.isInCamera = this.meshRenderer.isVisible;
		if (base.isInCamera && !this.hasShow)
		{
			this.hasShow = true;
		}
		if (this.hasShow && !base.isInCamera)
		{
			this._coolDownHide -= deltaTime;
			if (this._coolDownHide <= 0f)
			{
				this.Die(false);
				return;
			}
		}
		if (this._coolDownAttack > 0f)
		{
			this._coolDownAttack -= deltaTime;
		}
		if (!this._changeState)
		{
			this._changeState = true;
			int trackIndex = 0;
			bool loop = false;
			switch (this._state)
			{
			case Bee3.EState.Attack:
				trackIndex = 1;
				loop = true;
				this.skeletonAnimation.state.SetEmptyAnimation(2, 1f);
				break;
			case Bee3.EState.Idle:
				this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > GameManager.Instance.player.transform.position.x);
				break;
			case Bee3.EState.Walk:
				this._targetPos = CameraController.Instance.transform.position;
				this._targetPos.z = 0f;
				if (!this._begin)
				{
					this._targetPos.x = UnityEngine.Random.Range(this._targetPos.x - 6f, this._targetPos.x + 6f);
				}
				else
				{
					this._begin = false;
					this._targetPos.x = ((this.transform.position.x >= this._targetPos.x) ? UnityEngine.Random.Range(this._targetPos.x + 1f, this._targetPos.x + 5f) : UnityEngine.Random.Range(this._targetPos.x - 5f, this._targetPos.x - 1f));
				}
				this._targetPos.y = UnityEngine.Random.Range(this._targetPos.y + 1f, this._targetPos.y + 2.5f);
				this._timeToTarget = 1f;
				this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > this._targetPos.x);
				loop = true;
				break;
			case Bee3.EState.Aim:
				this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > GameManager.Instance.player.transform.position.x);
				trackIndex = 2;
				this._timeToTarget = 0.3f;
				this._pos = this.transform.position;
				this._currentTamPos = this._gunBone1.GetLocalPosition();
				this._targetPos = GameManager.Instance.player.tfOrigin.position;
				this._targetPos.y = this._targetPos.y - this._pos.y;
				this._targetPos.x = ((!this.skeletonAnimation.skeleton.FlipX) ? (this._targetPos.x - this._pos.x) : (this._pos.x - this._targetPos.x));
				this._targetPos.y = Mathf.Min(this._targetPos.y, this._currentTamPos.y);
				this._targetBone.SetPosition(this._currentTamPos);
				break;
			}
			this.skeletonAnimation.state.SetAnimation(trackIndex, this.anims[(int)this._state], loop);
		}
		else
		{
			switch (this._state)
			{
			case Bee3.EState.Walk:
			{
				this.transform.position = Vector3.SmoothDamp(this.transform.position, this._targetPos, ref this._veloMoveTarget, this._timeToTarget);
				float num = Vector3.Distance(this.transform.position, this._targetPos);
				float num2 = CameraController.Instance.camPos.y + CameraController.Instance.Size().y - 1f;
				bool flag = base.isInCamera && this.transform.position.y >= num2 && this.transform.position.y <= this._targetPos.y;
				if (num < 0.1f || flag)
				{
					this.ChangeState();
				}
				break;
			}
			case Bee3.EState.Aim:
			{
				this._currentTamPos = Vector3.SmoothDamp(this._currentTamPos, this._targetPos, ref this._veloMoveTarget, this._timeToTarget);
				float num3 = Vector3.Distance(this._currentTamPos, this._targetPos);
				if (num3 <= 0.1f)
				{
					this._currentTamPos = this._targetPos;
					this.ChangeState();
				}
				this._targetBone.SetPosition(this._currentTamPos);
				break;
			}
			}
		}
	}

	private void ChangeState()
	{
		switch (this._state)
		{
		case Bee3.EState.Attack:
			this._state = ((UnityEngine.Random.Range(0, 4) != 1) ? Bee3.EState.Walk : Bee3.EState.Idle);
			break;
		case Bee3.EState.Idle:
		{
			this._state = ((this._coolDownAttack <= 0f && !GameManager.Instance.player.isInVisible) ? Bee3.EState.Aim : Bee3.EState.Idle);
			bool flag = base.isInCamera && this.transform.position.y >= CameraController.Instance.camPos.y + CameraController.Instance.Size().y - 1.5f;
			this._state = ((!flag) ? Bee3.EState.Walk : this._state);
			break;
		}
		case Bee3.EState.Walk:
			this._state = ((this._coolDownAttack <= 0f && !GameManager.Instance.player.isInVisible) ? Bee3.EState.Aim : Bee3.EState.Idle);
			break;
		case Bee3.EState.Aim:
			this._state = Bee3.EState.Attack;
			break;
		}
		this._changeState = false;
	}

	public override void Hit(float damage)
	{
		if (this.HP <= 0f)
		{
			this.Die(true);
			return;
		}
		this.skeletonAnimation.state.SetAnimation(3, this.anims[2], false);
	}

	private void Die(bool isRambo = true)
	{
		this.bodyCollider2D.enabled = false;
		base.CalculatorToDie(isRambo, false);
		this._state = Bee3.EState.Die;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		if (isRambo)
		{
			this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], false);
			try
			{
				if (isRambo)
				{
					GameManager.Instance.fxManager.ShowFxNoSpine01(1, this.transform.position, Vector3.one / 2f);
				}
			}
			catch
			{
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string text = e.ToString();
		if (text != null)
		{
			if (text == "shot")
			{
				Vector3 worldPosition = this._gunBone1.GetWorldPosition(this.transform);
				Vector3 v = worldPosition - this._gunBone2.GetWorldPosition(this.transform);
				GameManager.Instance.bulletManager.CreateBulletEnemy(0, v, worldPosition, this.cacheEnemy.Damage, this.cacheEnemy.Speed * 2f, 0f).spriteRenderer.flipX = false;
				SingletonGame<AudioController>.Instance.PlaySound(this.fireClip, 0.3f);
			}
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "Attack01"))
			{
				if (!(text == "Death01"))
				{
					if (text == "Idle01")
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
				this._countFire++;
				if (this._countFire >= this.maxFire)
				{
					this._countFire = 0;
					this._coolDownAttack = this.cacheEnemy.Time_Reload_Attack;
					this.skeletonAnimation.state.SetEmptyAnimation(1, 0.2f);
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
	private string targetBone;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string gunTip1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string gunTip2;

	[SerializeField]
	private AudioClip fireClip;

	[SerializeField]
	private int maxFire;

	private Action<Bee3> dieCallback;

	private Bee3.EState _state;

	private bool _changeState;

	private bool _begin;

	private Spine.Animation[] anims;

	private Bone _targetBone;

	private Bone _gunBone1;

	private Bone _gunBone2;

	private Vector3 _currentTamPos;

	private Vector3 _targetPos;

	private Vector3 _veloMoveTarget;

	private int _countFire;

	private float _coolDownAttack;

	private float _timeToTarget;

	private float _coolDownHide;

	private Vector3 _pos;

	private enum EState
	{
		Attack,
		Die,
		Hit,
		Idle,
		Walk,
		Aim
	}
}
