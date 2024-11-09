using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BugBomb : BaseEnemy
{
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
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			GameManager.Instance.ListEnemy.Remove(this);
			EnemyManager.Instance.BugBombPool.Store(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Obstacle") || collision.transform.CompareTag("Obstacle2"))
		{
			if (this._isJump)
			{
				this._isJump = false;
				this._state = BugBomb.EState.Jump5;
				this.PlayAnim(0, this._state, false);
				base.isInCamera = false;
			}
			return;
		}
		if (collision.transform.CompareTag("Gulf"))
		{
			base.gameObject.SetActive(false);
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Gulf"))
		{
			base.gameObject.SetActive(false);
			return;
		}
		if ((collision.CompareTag("Found_Wall") || collision.CompareTag("Found_Jump") || collision.CompareTag("Found_Gulf")) && !this._isJump)
		{
			this._isJump = true;
			this._state = BugBomb.EState.Jump1;
			this.PlayAnim(0, this._state, false);
		}
	}

	public void Init(EnemyDataInfo dataInfo)
	{
		base.gameObject.SetActive(true);
		this.bodyCollider2D.enabled = true;
		this.cacheEnemyData = dataInfo;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		this.transform.position = dataInfo.Vt2;
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
		}
		this.cacheEnemy.HP = this.data.datas[dataInfo.level].hp;
		this.HP = this.cacheEnemy.HP * GameMode.Instance.GetMode();
		this.cacheEnemy.Damage = this.data.datas[dataInfo.level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[dataInfo.level].speed;
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		GameManager.Instance.ListEnemy.Add(this);
		this._isJump = false;
		this._detectedRambo = false;
		this._state = BugBomb.EState.Run2;
		this.PlayAnim(0, this._state, true);
		bool flag = dataInfo.Vt2.x < CameraController.Instance.transform.position.x;
		this._vectorMove = (float)((!flag) ? -1 : 1);
		this.skeletonAnimation.skeleton.FlipX = flag;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.meshRenderer.sortingOrder = 7;
		this._count = 0;
		this._show = false;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.isInit)
		{
			return;
		}
		if (this._state == BugBomb.EState.Attack)
		{
			return;
		}
		if (!this._detectedRambo)
		{
			float num = 0f;
			CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
			if (orientaltion != CameraController.Orientation.HORIZONTAL)
			{
				if (orientaltion == CameraController.Orientation.VERTICAL)
				{
					num = Mathf.Abs(this.transform.position.y - GameManager.Instance.player.GetPosition().y);
				}
			}
			else
			{
				num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.GetPosition().x);
			}
			if (num <= this.maxDetectedX && !this._isJump)
			{
				this._detectedRambo = true;
				this._state = BugBomb.EState.Run1;
				this.PlayAnim(0, this._state, true);
			}
		}
		else
		{
			this._target = GameManager.Instance.player.transform.position.x;
			float num2 = Mathf.Abs(this.transform.position.x - this._target);
			if (num2 <= 0.5f)
			{
				base.isInCamera = false;
				this._state = BugBomb.EState.Attack;
				this.PlayAnim(0, this._state, true);
				this.bodyCollider2D.enabled = false;
			}
		}
		if (this._state != BugBomb.EState.Jump1)
		{
			float x = ((!this._detectedRambo) ? this.cacheEnemy.Speed : (this.cacheEnemy.Speed * 2f)) * this._vectorMove * deltaTime;
			this.transform.Translate(x, 0f, 0f);
		}
		if (this._show && !this.meshRenderer.isVisible)
		{
			base.gameObject.SetActive(false);
		}
		if (!this._show && this.meshRenderer.isVisible)
		{
			this._show = true;
		}
	}

	public void Pause(bool pause)
	{
		if (pause)
		{
			this.skeletonAnimation.timeScale = 0f;
			this._velocity = this.rigidbody2D.velocity;
		}
		else
		{
			this.skeletonAnimation.timeScale = 1f;
			this.rigidbody2D.velocity = this._velocity;
		}
	}

	private void PlayAnim(int track, BugBomb.EState state, bool loop = true)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(track, this.anims[(int)state], loop);
	}

	private void Explosive()
	{
		try
		{
			base.gameObject.SetActive(false);
			for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
			{
				float num = Vector3.Distance(GameManager.Instance.ListRambo[i].transform.position, this.transform.position);
				if (num <= 1.5f)
				{
					GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
				}
			}
			GameManager.Instance.fxManager.ShowFxNoSpine01(1, this.transform.position, Vector3.one * 0.75f);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public override void Hit(float damage)
	{
		if (this.HP <= 0f && this._state != BugBomb.EState.Attack)
		{
			this._state = BugBomb.EState.Attack;
			this.PlayAnim(0, this._state, true);
			this.bodyCollider2D.enabled = false;
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		BugBomb.EState state = this._state;
		if (state != BugBomb.EState.Attack)
		{
			if (state != BugBomb.EState.Jump1)
			{
				if (state == BugBomb.EState.Jump5)
				{
					float x = GameManager.Instance.player.transform.position.x;
					float num = Mathf.Abs(this.transform.position.x - x);
					this._state = ((num > this.maxDetectedX) ? BugBomb.EState.Run2 : BugBomb.EState.Run1);
					this.PlayAnim(0, this._state, true);
				}
			}
			else
			{
				this._state = BugBomb.EState.Jump2;
				this.PlayAnim(0, this._state, false);
				this.rigidbody2D.velocity = Mathf.Min(this.data.datas[this.cacheEnemyData.level].speed * 5f, 7f) * Vector2.up;
			}
		}
		else
		{
			this._count++;
			if (this._count >= 3)
			{
				this.Explosive();
			}
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string[] anims;

	[SerializeField]
	[Range(3f, 12f)]
	[Tooltip("khoảng cách x tối đa để bọ thấy và bắt đầu tấn công")]
	private float maxDetectedX;

	private BugBomb.EState _state;

	private float _vectorMove;

	private bool _detectedRambo;

	private bool _isJump;

	private float _target;

	private int _effCount;

	private Vector2 _velocity;

	private int _count;

	private bool _show;

	private enum EState
	{
		Attack,
		Idle,
		Jump1,
		Jump2,
		Jump3,
		Jump5,
		Run1,
		Run2
	}
}
