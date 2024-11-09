using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class NhenNhay : BaseEnemy
{
	private void OnValidate()
	{
		try
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
		catch
		{
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Ground"))
		{
			this._isJump = false;
			this._veloY = UnityEngine.Random.Range(4.5f, 6.5f);
			this._state = NhenNhay.EState.Jump5;
			this.PlayAnim(false);
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			GameManager.Instance.ListEnemy.Remove(this);
			if (this.OnEnemyDeaded != null)
			{
				this.OnEnemyDeaded();
				this.OnEnemyDeaded = null;
			}
			this.hide(this);
		}
		catch
		{
		}
	}

	public void InitEnemy(EnemyDataInfo info, Action<NhenNhay> onHide = null)
	{
		float mode = GameMode.Instance.GetMode();
		this.cacheEnemyData = info;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		this.Init(this.data.datas[info.level].damage * mode, this.data.datas[info.level].speed, this.data.datas[info.level].hp * mode, false, info.Vt2, onHide, false);
	}

	public void Init(float damage, float speed, float hp, bool flip, Vector3 Pos, Action<NhenNhay> onHide = null, bool useByBoss = true)
	{
		this.useByBoss = useByBoss;
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
			this.maskGround = LayerMask.NameToLayer("Ground");
		}
		this.HP = hp;
		this.cacheEnemy.HP = Mathf.Round(hp / GameMode.Instance.GetMode());
		this.cacheEnemy.Damage = damage;
		this.cacheEnemy.Speed = speed;
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		if (useByBoss)
		{
			GameManager.Instance.ListEnemy.Add(this);
		}
		else
		{
			this._coolDownHide = 5f;
		}
		this.hide = onHide;
		base.gameObject.SetActive(true);
		this.transform.position = Pos;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.skeletonAnimation.skeleton.FlipX = flip;
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		if (this.meshRenderer)
		{
			this.meshRenderer.sortingOrder = 0;
		}
		this._countDelayAttack = 0;
		this._veloX = (float)((!flip) ? -1 : 1);
		this._isDie = false;
		this._foundRambo = false;
		this._state = NhenNhay.EState.Jump3;
		this._isJump = true;
		this._veloY = 0f;
		this.PlayAnim(false);
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (!base.isInCamera && !this.useByBoss)
		{
			this._coolDownHide -= deltaTime;
			if (this._coolDownHide <= 0f)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		this._pos = this.transform.position;
		if (!this._foundRambo)
		{
			this._foundRambo = (Mathf.Abs(this._pos.x - GameManager.Instance.player.transform.position.x) < 0.2f);
		}
		if (this._isJump)
		{
			if (!this._foundRambo)
			{
				this._pos.x = this._pos.x + this._veloX * this.cacheEnemy.Speed * deltaTime;
			}
			this._pos.y = this._pos.y + this._veloY * deltaTime;
			this._veloY -= deltaTime * 9.8f;
			this.transform.position = this._pos;
			if (this._veloY < 0f && this._state == NhenNhay.EState.Jump2)
			{
				this._state = NhenNhay.EState.Jump3;
				this.PlayAnim(false);
			}
		}
	}

	public void Pause(bool pause)
	{
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
	}

	private void PlayAnim(bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], loop);
	}

	public override void Hit(float damage)
	{
		if (this.HP > 0f)
		{
			SingletonGame<PingPongColorSpine>.Instance.PingPongColor(this.skeletonAnimation, 0.2f);
			return;
		}
		this._isDie = true;
		this._isJump = false;
		this._state = NhenNhay.EState.Attack2;
		this.PlayAnim(false);
	}

	private void Hide()
	{
		if (this._isDie)
		{
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
		}
		else
		{
			this._pos = this.transform.position;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this._pos, Vector2.down, 5f, this.maskGround);
			if (raycastHit2D.transform)
			{
				this._pos = raycastHit2D.point;
			}
			GameManager.Instance.fxManager.ShowEffNo02(this._pos, Vector3.one);
		}
		base.gameObject.SetActive(false);
		if (!this.useByBoss)
		{
			base.CalculatorToDie(true, false);
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		switch (this._state)
		{
		case NhenNhay.EState.Attack1:
			this._state = NhenNhay.EState.Attack2;
			this.PlayAnim(false);
			break;
		case NhenNhay.EState.Attack2:
			try
			{
				if (!this._isDie)
				{
					for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
					{
						float num = Vector3.Distance(this.transform.position, GameManager.Instance.ListRambo[i].tfOrigin.transform.position);
						if (num <= 1.5f)
						{
							GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
						}
					}
				}
				this.Hide();
			}
			catch
			{
			}
			break;
		case NhenNhay.EState.Idle:
			if (this._foundRambo)
			{
				this._state = NhenNhay.EState.Attack1;
				this.PlayAnim(true);
			}
			else
			{
				this._state = NhenNhay.EState.Jump1;
				this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < GameManager.Instance.player.transform.position.x);
				this.PlayAnim(false);
			}
			break;
		case NhenNhay.EState.Jump1:
			this._veloX = (float)((!this.skeletonAnimation.skeleton.FlipX) ? -1 : 1);
			this._state = NhenNhay.EState.Jump2;
			this.PlayAnim(false);
			this._isJump = true;
			break;
		case NhenNhay.EState.Jump4:
			this._state = NhenNhay.EState.Jump5;
			this.PlayAnim(false);
			break;
		case NhenNhay.EState.Jump5:
			if (this.meshRenderer)
			{
				this.meshRenderer.sortingOrder = 7;
			}
			if (this._foundRambo)
			{
				this._state = NhenNhay.EState.Attack1;
				this.PlayAnim(true);
			}
			else
			{
				this._state = ((UnityEngine.Random.Range(0, 5) != 1) ? NhenNhay.EState.Jump1 : NhenNhay.EState.Idle);
				this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < GameManager.Instance.player.transform.position.x);
				this.PlayAnim(false);
			}
			break;
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	private Action<NhenNhay> hide;

	private Spine.Animation[] anims;

	private bool _isJump;

	private NhenNhay.EState _state;

	private float _veloY;

	private float _veloX;

	private Vector3 _pos;

	private bool _foundRambo;

	private int _countDelayAttack;

	private bool _isDie;

	private LayerMask maskGround;

	private bool useByBoss;

	private float _coolDownHide;

	private enum EState
	{
		Attack1,
		Attack2,
		Idle,
		Jump1,
		Jump2,
		Jump3,
		Jump4,
		Jump5
	}
}
