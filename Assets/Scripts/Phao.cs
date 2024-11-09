using System;
using System.Collections;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Phao : BaseEnemy
{
	private void OnValidate()
	{
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
	}

	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.Init();
		yield break;
	}

	private void Init()
	{
		this.cacheEnemy = new Enemy();
		this.cacheEnemy.HP = (this.HP = this.data.datas[this.Level].hp * GameMode.Instance.GetMode());
		this.cacheEnemy.Damage = this.data.datas[this.Level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[this.Level].speed;
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.gameObject.SetActive(true);
			this.lineBloodEnemy.Reset();
		}
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this._gunBone = this.skeletonAnimation.skeleton.FindBone(this.gunbone);
		this._state = Phao.EState.Idle;
		this.PlayAnim();
		this.isInListEnemy = false;
		this.isInit = true;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "attack-1"))
			{
				if (name == "attack-2")
				{
					GameManager.Instance.bulletManager.CreateRocketBossType1(this.cacheEnemy.Damage, this.cacheEnemy.Speed, Vector3.left, this._gunBone.GetWorldPosition(this.skeletonAnimation.transform), GameManager.Instance.player.tfOrigin, 0.5f);
				}
			}
			else
			{
				GameManager.Instance.bulletManager.CreateBulletAnim(ETypeBullet.BOSS_1_2, this._gunBone.GetWorldPosition(this.skeletonAnimation.transform), Vector2.left, this.cacheEnemy.Damage, this.cacheEnemy.Speed * 5f);
				CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
			}
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		if (name != null)
		{
			if (name == "idle" || name == "attack-1" || name == "attack-2")
			{
				this.ChangeState();
			}
		}
	}

	private void Update()
	{
		if (!this.isInit || this._state == Phao.EState.Die || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.isInListEnemy && base.isInCamera)
		{
			this.isInListEnemy = true;
			GameManager.Instance.ListEnemy.Add(this);
		}
		if (this.isInListEnemy && !base.isInCamera)
		{
			this.isInListEnemy = false;
			GameManager.Instance.ListEnemy.Remove(this);
		}
	}

	private void ChangeState()
	{
		if (this._state == Phao.EState.Die)
		{
			return;
		}
		Phao.EState state = this._state;
		if (state != Phao.EState.Attack1 && state != Phao.EState.Attack2)
		{
			if (state == Phao.EState.Idle)
			{
				if (this.isInListEnemy)
				{
					float num = this.transform.position.x - GameManager.Instance.player.transform.position.x;
					if (num < 5f)
					{
						this._state = Phao.EState.Attack2;
					}
					else
					{
						this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Phao.EState.Attack1 : Phao.EState.Attack2);
					}
				}
			}
		}
		else
		{
			this._state = Phao.EState.Idle;
		}
		this.PlayAnim();
	}

	private void PlayAnim()
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], false);
	}

	public override void Hit(float damage)
	{
		if (this.HP <= 0f)
		{
			this.isInit = false;
			this._state = Phao.EState.Die;
			this.PlayAnim();
			this.bodyCollider2D.enabled = false;
			GameManager.Instance.ListEnemy.Remove(this);
			GameManager.Instance.fxManager.ShowExplosionSpine(this.tfOrigin.position, 0);
		}
		else
		{
			this.skeletonAnimation.state.SetAnimation(1, this.anims[3], false);
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private int Level;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string gunbone;

	private Phao.EState _state;

	private bool isInListEnemy;

	private Spine.Animation[] anims;

	private Bone _gunBone;

	private float _modeRate;

	private bool _isAttack1;

	private enum EState
	{
		Attack1,
		Attack2,
		Die,
		Hit,
		Idle
	}
}
