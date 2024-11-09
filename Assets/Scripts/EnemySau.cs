using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemySau : CachingMonoBehaviour
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
		if (!this.renderer)
		{
			this.renderer = base.GetComponentInChildren<Renderer>();
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.skeletonAnimation.state.Event -= this.OnEvent;
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			GameManager.Instance.ListEnemy.Remove(this.boxEnemy);
			this.hide(this);
		}
		catch
		{
		}
	}

	public void InitEnemy(EnemyDataInfo dataInfo, bool isCreateWithJson, Action<EnemySau> hideAction = null)
	{
		this.boxEnemy.cacheEnemyData = dataInfo;
		float mode = GameMode.Instance.GetMode();
		this.Init(this.data.datas[dataInfo.level].hp * mode, this.data.datas[dataInfo.level].speed, this.data.datas[dataInfo.level].damage * mode, dataInfo.Vt2, hideAction, false);
		this.boxEnemy.ReloadInfor(1f, 0.5f);
		this.boxEnemy.isCreateWithJson = isCreateWithJson;
		this.boxEnemy.CheckPoint = Mathf.Max(0, GameManager.Instance.CheckPoint);
		if (isCreateWithJson)
		{
			this.boxEnemy.OnEnemyDeaded = delegate()
			{
				this.boxEnemy.isCreateWithJson = false;
				isCreateWithJson = false;
				DataLoader.LevelDataCurrent.points[this.boxEnemy.CheckPoint].totalEnemy--;
				GameManager.Instance.giftManager.CreateItemWeapon(this.boxEnemy.Origin());
				GameManager.Instance.TotalEnemyKilled++;
				this.boxEnemy.OnEnemyDeaded = null;
			};
		}
	}

	public void Init(float hp, float speed, float damage, Vector3 pos, Action<EnemySau> hideAction = null, bool useByBoss = true)
	{
		this.hide = hideAction;
		this._useByBoss = useByBoss;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.sauAttack.AddTargetTag("Rambo");
		this.flameAttack.AddTargetTag("Rambo");
		this._speed = speed;
		this._damage = damage;
		this._coolDownHide = 5f;
		this.boxEnemy.InitEnemy(hp);
		this.boxEnemy.cacheEnemy.HP = Mathf.Round(hp / GameMode.Instance.GetMode());
		this.boxEnemy.OnHit = new Action<float, EWeapon>(this.OnHit);
		this.sauAttack.Deactive();
		this.flameAttack.Deactive();
		GameManager.Instance.ListEnemy.Add(this.boxEnemy);
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		if (this.rigidbody2D && !this.renderer.isVisible)
		{
			this.rigidbody2D.isKinematic = true;
		}
		this._state = EnemySau.EState.Run1;
		this._changeState = false;
		this.isInit = true;
	}

	private void OnHit(float arg1, EWeapon arg2)
	{
		if (this.boxEnemy.HP > 0f)
		{
			SingletonGame<PingPongColorSpine>.Instance.PingPongColor(this.skeletonAnimation, 0.3f);
			return;
		}
		this._state = EnemySau.EState.Die;
		this.boxEnemy.isInCamera = false;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		GameManager.Instance.fxManager.ShowFxNo01(this.boxEnemy.transform.position, 1f);
		this.PlayAnim(true);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit || this._state == EnemySau.EState.Die)
		{
			return;
		}
		if (!this.renderer.isVisible)
		{
			if (this._coolDownHide <= 0f)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this._coolDownHide -= deltaTime;
		}
		this.boxEnemy.isInCamera = this.renderer.isVisible;
		if (this.boxEnemy.isInCamera && this.rigidbody2D && this.rigidbody2D.isKinematic)
		{
			this.rigidbody2D.isKinematic = false;
		}
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		if (this.tfLineBlood)
		{
			this.tfLineBlood.position = this.boxEnemy.transform.position + Vector3.up / 2f;
		}
	}

	public void Pause(bool pause)
	{
		if (this._pause != pause)
		{
			this._pause = pause;
			if (pause)
			{
				this.skeletonAnimation.timeScale = 0f;
				if (this.particleFlame.isPlaying)
				{
					this.particleFlame.Pause();
				}
			}
			else
			{
				this.skeletonAnimation.timeScale = 1f;
				if (this.particleFlame.isPaused)
				{
					this.particleFlame.Play();
				}
			}
		}
	}

	private void StartState()
	{
		this._changeState = true;
		bool loop = false;
		switch (this._state)
		{
		case EnemySau.EState.Attack1:
		case EnemySau.EState.Attack2:
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < GameManager.Instance.player.transform.position.x);
			break;
		case EnemySau.EState.Run1:
			loop = true;
			this._targetX = GameManager.Instance.player.transform.position.x;
			this.sauAttack.Active(this._damage / 10f, false, null);
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < this._targetX);
			break;
		case EnemySau.EState.Run2:
		{
			loop = true;
			float num = UnityEngine.Random.Range(2f, 4f);
			this._targetX = this.transform.position.x + ((this.transform.position.x >= CameraController.Instance.camPos.x) ? (-num) : num);
			this._targetX = Mathf.Max(CameraController.Instance.transform.position.x - 6f, Mathf.Min(CameraController.Instance.transform.position.x + 3f, this._targetX));
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < this._targetX);
			break;
		}
		}
		this.PlayAnim(loop);
	}

	private void UpdateState(float deltaTime)
	{
		switch (this._state)
		{
		case EnemySau.EState.Run1:
		case EnemySau.EState.Run2:
		{
			this._pos = this.transform.position;
			this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetX, this._speed * deltaTime);
			this.transform.position = this._pos;
			float num = Mathf.Abs(this._pos.x - GameManager.Instance.player.transform.position.x);
			if (this._pos.x == this._targetX || num < 2f)
			{
				if (this._state == EnemySau.EState.Run1)
				{
					this.sauAttack.Deactive();
				}
				this.ChangeState();
			}
			break;
		}
		}
	}

	private void ChangeState()
	{
		float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
		switch (this._state)
		{
		case EnemySau.EState.Attack1:
		case EnemySau.EState.Attack2:
			this._state = ((num > 3f) ? EnemySau.EState.Run2 : EnemySau.EState.Run1);
			break;
		case EnemySau.EState.Run1:
		case EnemySau.EState.Run2:
			this._state = ((num > 4f) ? EnemySau.EState.Attack1 : EnemySau.EState.Attack2);
			break;
		}
		this._changeState = false;
	}

	private void PlayAnim(bool loop)
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], loop);
	}

	private void CreateBullet()
	{
		GameManager.Instance.bulletManager.CreateBulletEnemy(10, GameManager.Instance.player.tfOrigin.transform.position - this.firePoint.transform.position, this.firePoint.position, this._damage, this._speed * 2f, 0f).spriteRenderer.flipX = false;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		EnemySau.EState state = this._state;
		if (state != EnemySau.EState.Attack1)
		{
			if (state == EnemySau.EState.Attack2)
			{
				this.particleFlame.Play();
				this.flameAttack.Active(this._damage, true, null);
			}
		}
		else
		{
			this.CreateBullet();
			this._countBullet++;
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "attack"))
			{
				if (!(text == "attack2"))
				{
					if (text == "die")
					{
						this._countAnimDiePlay++;
						if (this._countAnimDiePlay > 3)
						{
							this._countAnimDiePlay = 0;
							base.gameObject.SetActive(false);
							if (!this._useByBoss)
							{
								this.boxEnemy.CallDie(true);
							}
						}
					}
				}
				else
				{
					this.ChangeState();
					this.flameAttack.Deactive();
				}
			}
			else
			{
				this.ChangeState();
			}
		}
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private Renderer renderer;

	[SerializeField]
	private BoxBoss1_3 boxEnemy;

	[SerializeField]
	private Transform tfLineBlood;

	[SerializeField]
	private AttackBox sauAttack;

	[SerializeField]
	private AttackBox flameAttack;

	[SerializeField]
	private Transform firePoint;

	[SerializeField]
	private ParticleSystem particleFlame;

	private Action<EnemySau> hide;

	private EnemySau.EState _state;

	private bool _changeState;

	private Spine.Animation[] anims;

	private float _targetX;

	private Vector3 _pos;

	private float _speed;

	private float _damage;

	private int _countBullet;

	private bool _pause;

	private int _countAnimDiePlay;

	private float _coolDownHide;

	private bool _useByBoss;

	private enum EState
	{
		Attack1,
		Attack2,
		Die,
		Hit,
		Run1,
		Run2
	}
}
