using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BeSung : BaseEnemy2
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
		if (!this.meshRenderer)
		{
			this.meshRenderer = base.GetComponentInChildren<MeshRenderer>();
		}
	}

	public override void Init(EnemyDataInfo enemyDataInfo, Action hideCallback)
	{
		base.Init(enemyDataInfo, hideCallback);
		if (this._boneGun1 == null)
		{
			this._boneGun1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun1);
			this._boneGun2 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun2);
		}
		base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
		if (this.pos.x < CameraController.Instance.camPos.x)
		{
			this._state = BeSung.EState.Run2;
		}
		else
		{
			this._state = BeSung.EState.Idle1;
			this.bodyCollider2D.enabled = false;
		}
		this.isChangeState = false;
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}

	public override void SetParachuter(float gravity = 0.5f)
	{
	}

	protected override void StartState()
	{
		base.StartState();
		bool loop = false;
		int track = 0;
		BeSung.EState state = this._state;
		switch (state)
		{
		case BeSung.EState.Idle1:
		case BeSung.EState.Idle1_2:
			goto IL_AF;
		case BeSung.EState.Idle2:
			break;
		case BeSung.EState.Run:
		case BeSung.EState.Run2:
			loop = true;
			this.isMove = true;
			base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
			goto IL_AF;
		default:
			if (state != BeSung.EState.Attack)
			{
				goto IL_AF;
			}
			break;
		}
		base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
		IL_AF:
		base.PlayAnim((int)this._state, track, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		BeSung.EState state = this._state;
		if (state == BeSung.EState.Run || state == BeSung.EState.Run2)
		{
			this.pos.x = this.pos.x + ((!base.Flip) ? (-this.cacheEnemy.Speed * deltaTime) : (this.cacheEnemy.Speed * deltaTime));
			bool flag = Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x) < this.cacheEnemy.Vision_Max;
			if (flag)
			{
				this.isMove = false;
				this.ChangeState();
			}
		}
	}

	protected override void ChangeState()
	{
		if (this.isDie)
		{
			return;
		}
		bool flag = this.isAttack && base.isInCamera && this._state != BeSung.EState.Idle1;
		if (flag)
		{
			this._state = BeSung.EState.Attack;
		}
		else
		{
			BeSung.EState state = this._state;
			if (state != BeSung.EState.Idle1)
			{
				bool flag2 = !base.isInCamera || Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x) > this.cacheEnemy.Vision_Max;
				flag2 = (flag2 && !this._onlyIdle);
				if (flag2)
				{
					this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? BeSung.EState.Run2 : BeSung.EState.Run);
				}
				else
				{
					this._state = BeSung.EState.Idle2;
				}
			}
			else if (base.isInCamera)
			{
				this._state = BeSung.EState.Idle1_2;
				this.bodyCollider2D.enabled = true;
			}
		}
		base.ChangeState();
	}

	protected override void OnStuckMove()
	{
		base.OnStuckMove();
		this.isMove = false;
		this.ChangeState();
	}

	protected override void OnPowerUp()
	{
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == BeSung.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(3, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(2, 4, false);
				return;
			}
		}
		this.isMove = false;
		this._state = BeSung.EState.Hit_Ice;
		base.PlayAnim(4, 0, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		base.PlayAnim(1, 0, false);
		if (isRambo)
		{
			GameManager.Instance.fxManager.ShowFxNoSpine01(1, this.tfOrigin.position, Vector3.one / 2f);
		}
	}

	protected override void Disable()
	{
		base.Disable();
	}

	protected override void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.OnEvent(trackEntry, e);
		float speed = Mathf.Min(this.cacheEnemy.Speed * 12f, 20f);
		Vector3 worldPosition = this._boneGun1.GetWorldPosition(this.transform);
		Vector3 direction = worldPosition - this._boneGun2.GetWorldPosition(this.transform);
		GameManager.Instance.bulletManager.CreateBulletBeSung(speed, this.cacheEnemy.Damage, worldPosition, direction, 2f);
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		switch (text)
		{
		case "Attack":
			this.attackCount++;
			if (this.attackCount > this.modeLv)
			{
				this.attackCount = 0;
				base.ResetTimeReload();
				this.ChangeState();
			}
			else
			{
				base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
				base.PlayAnim((int)this._state, 0, false);
			}
			break;
		case "Death":
			base.gameObject.SetActive(false);
			break;
		case "idel1":
		case "idel2":
		case "idel1-2":
		case "run":
		case "run2":
		case "Hit-Ice":
			this.ChangeState();
			break;
		}
	}

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun2;

	private BeSung.EState _state;

	private Bone _boneGun1;

	private Bone _boneGun2;

	private bool _onlyIdle;

	private enum EState
	{
		Attack,
		Death,
		Hit,
		Hit_Elec,
		Hit_Ice,
		Idle1,
		Idle2,
		Idle1_2,
		Run,
		Run2
	}
}
