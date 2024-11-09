using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class ChuotRocket : BaseEnemy
{
	private void Reset()
	{
		this.OnValidate();
	}

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
			if (this.HideAction != null)
			{
				this.HideAction();
			}
			this.skeletonAnimation.state.Complete -= this.OnComplete;
			GameManager.Instance.ListEnemy.Remove(this);
			EnemyManager.Instance.RocketMousePool.Store(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Gulf"))
		{
			base.gameObject.SetActive(false);
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Found_Jump") || collision.transform.CompareTag("Found_Gulf") || collision.transform.CompareTag("Found_Wall"))
		{
			this._state = ChuotRocket.EState.Attack;
			this.PlayAnim(0, this._state, true);
		}
		if (collision.transform.CompareTag("Gulf"))
		{
			base.gameObject.SetActive(false);
			return;
		}
	}

	public void Init(EnemyDataInfo dataInfo)
	{
		base.gameObject.SetActive(true);
		this.bodyCollider2D.enabled = true;
		this.transform.position = dataInfo.Vt2;
		this.cacheEnemyData = dataInfo;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
		}
		this.cacheEnemy.HP = this.data.datas[this.cacheEnemyData.level].hp;
		this.HP = this.cacheEnemy.HP * GameMode.Instance.GetMode();
		this.cacheEnemy.Damage = this.data.datas[this.cacheEnemyData.level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[this.cacheEnemyData.level].speed;
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		this._detectedRambo = false;
		this._state = ChuotRocket.EState.Run2;
		this.PlayAnim(0, this._state, true);
		bool flag = dataInfo.Vt2.x < CameraController.Instance.transform.position.x;
		GameManager.Instance.ListEnemy.Add(this);
		this._vectorMove = (float)((!flag) ? -1 : 1);
		this.skeletonAnimation.skeleton.FlipX = flag;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this._count = 0;
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
		if (this._state == ChuotRocket.EState.Attack)
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
			if (num <= this.maxDetectedX)
			{
				this._detectedRambo = true;
				this._state = ChuotRocket.EState.Run1;
				this.PlayAnim(0, this._state, true);
			}
		}
		else
		{
			this._target = GameManager.Instance.player.transform.position.x;
			float num2 = Mathf.Abs(this.transform.position.x - this._target);
			if (num2 <= 0.2f)
			{
				base.isInCamera = false;
				this._state = ChuotRocket.EState.Attack;
				this.PlayAnim(0, this._state, true);
				this.bodyCollider2D.enabled = false;
			}
		}
		float x = ((!this._detectedRambo) ? this.cacheEnemy.Speed : (this.cacheEnemy.Speed * 2f)) * this._vectorMove * deltaTime;
		this.transform.Translate(x, 0f, 0f);
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
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
	}

	private void PlayAnim(int track, ChuotRocket.EState state, bool loop = true)
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
		if (this.HP <= 0f && this._state != ChuotRocket.EState.Attack)
		{
			this._state = ChuotRocket.EState.Attack;
			this.PlayAnim(0, this._state, true);
			this.bodyCollider2D.enabled = false;
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		ChuotRocket.EState state = this._state;
		if (state == ChuotRocket.EState.Attack)
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
	[Tooltip("khoảng cách x tối đa để bọ thấy và bắt đầu tấn công")]
	[Range(3f, 12f)]
	private float maxDetectedX;

	private Action HideAction;

	private ChuotRocket.EState _state;

	private float _vectorMove;

	private bool _detectedRambo;

	private float _target;

	private int _effCount;

	private int _count;

	private bool _show;

	private enum EState
	{
		Attack,
		Run1,
		Run2
	}
}
