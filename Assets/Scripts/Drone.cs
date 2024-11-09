using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Drone : CachingMonoBehaviour
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
			this.HideAction();
			GameManager.Instance.bulletManager.DronePool.Store(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		this.Explosive();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			return;
		}
		ISkill component = collision.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		this.Explosive();
	}

	public void Init(float damage, float speed, Vector3 pos, Action hideAction)
	{
		this._damage = damage;
		this._speed = speed;
		this.HideAction = hideAction;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this._startPos = pos;
		this._vetor = (float)((pos.x <= CameraController.Instance.transform.position.x) ? 1 : -1);
		this._idX = UnityEngine.Random.Range(0, this.curvesX.Length);
		this._idY = UnityEngine.Random.Range(0, this.curvesY.Length);
		this.PlayAnim(0, Drone.EState.Walk, true);
		this._lifeTime = 0f;
		this._curveTime = 0f;
		this._isFree = false;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this._pos = this.transform.position;
		if (this._isFree)
		{
			this._pos.y = this._pos.y - deltaTime * this._speed;
			this.transform.position = this._pos;
			return;
		}
		if (this._lifeTime < 2.5f)
		{
			this._lifeTime += deltaTime;
			this._pos.x = this._startPos.x + this._vetor * this.curvesX[this._idX].Evaluate(this._curveTime);
			this._pos.y = this._startPos.y + this.curvesY[this._idY].Evaluate(this._curveTime);
			this._curveTime = Mathf.MoveTowards(this._curveTime, 1f, deltaTime / 2f);
			if (this._lifeTime >= 2.5f)
			{
				this._ramboPos = GameManager.Instance.player.transform.position;
				this.PlayAnim(1, Drone.EState.Hit, true);
			}
		}
		else
		{
			this._pos = Vector3.MoveTowards(this._pos, this._ramboPos, deltaTime * this._speed);
			if (this._pos == this._ramboPos)
			{
				this.Explosive();
			}
		}
		this.transform.position = this._pos;
	}

	public void FreeDrove()
	{
		if (this.isInit)
		{
			this._isFree = true;
		}
	}

	private void PlayAnim(int track, Drone.EState state, bool loop = true)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(track, this.anims[(int)state], loop);
	}

	private void Explosive()
	{
		try
		{
			this.DisableObject();
			for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
			{
				float num = Vector3.Distance(GameManager.Instance.ListRambo[i].transform.position, this._pos);
				if (num <= 1.5f)
				{
					GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
				}
			}
			GameManager.Instance.fxManager.ShowFxNo01(this._pos, 1f);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void DisableObject()
	{
		base.gameObject.SetActive(false);
		this.isInit = false;
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private AnimationCurve[] curvesX;

	[SerializeField]
	private AnimationCurve[] curvesY;

	private float _speed;

	private float _damage;

	private int _idX;

	private int _idY;

	private Action HideAction;

	private bool _isFree;

	private float _vetor;

	private Vector3 _pos;

	private Vector3 _startPos;

	private Vector3 _ramboPos;

	private float _lifeTime;

	private float _curveTime;

	private enum EState
	{
		Hit,
		Walk
	}
}
