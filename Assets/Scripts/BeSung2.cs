using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BeSung2 : BaseEnemy2
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Found_Gulf") || collision.CompareTag("Found_Jump") || collision.CompareTag("Found_Wall"))
		{
			this._onlyIdle = true;
			if (this.isMove)
			{
				this.isMove = false;
				this.ChangeState();
			}
		}
	}

	public override void Init(EnemyDataInfo enemyDataInfo, Action hideCallback)
	{
		base.Init(enemyDataInfo, hideCallback);
		if (this._boneGun1 == null)
		{
			this._boneAim = this.skeletonAnimation.Skeleton.FindBone(this.strBoneAim);
			this._boneGun1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun1);
			this._boneGun2 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun2);
			this._boneGun3 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun3);
			this._boneGun4 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun4);
			this._boneGun5 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun5);
			this._boneGun6 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneGun6);
		}
		base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
		if (this.pos.x < CameraController.Instance.camPos.x)
		{
			this._state = BeSung2.EState.Run2;
		}
		else
		{
			this._state = BeSung2.EState.Idle1;
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
		BeSung2.EState state = this._state;
		switch (state)
		{
		case BeSung2.EState.Idle1:
		case BeSung2.EState.Idle1_2:
			break;
		case BeSung2.EState.Idle2:
			base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
			break;
		case BeSung2.EState.Run:
		case BeSung2.EState.Run2:
			loop = true;
			this.isMove = true;
			base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
			break;
		default:
			if (state == BeSung2.EState.Attack)
			{
				base.Flip = (this.pos.x < GameManager.Instance.player.transform.position.x);
				base.PlayAnim(0, 1, false);
				this._aimPos = this._boneGun1.GetSkeletonSpacePosition();
				if (base.Flip)
				{
					this._aimPos.x = -this._aimPos.x;
				}
				this._rootAimPosY = this._aimPos.y - 0.5f;
				this._timePingPongAim = 0.5f;
				this._boneAim.SetPosition(this._aimPos);
				this._maxAttack = 3 + this.modeLv * 3;
			}
			break;
		}
		base.PlayAnim((int)this._state, track, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		BeSung2.EState state = this._state;
		if (state != BeSung2.EState.Run && state != BeSung2.EState.Run2)
		{
			if (state == BeSung2.EState.Attack)
			{
				this._aimPos.y = this._rootAimPosY + Mathf.PingPong(this._timePingPongAim, 1f);
				this._timePingPongAim += deltaTime;
				this._boneAim.SetPosition(this._aimPos);
			}
		}
		else
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
		bool flag = this.isAttack && base.isInCamera && this._state != BeSung2.EState.Idle1;
		if (flag)
		{
			this._state = BeSung2.EState.Attack;
		}
		else
		{
			BeSung2.EState state = this._state;
			if (state != BeSung2.EState.Idle1)
			{
				bool flag2 = !base.isInCamera || Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x) > this.cacheEnemy.Vision_Max;
				flag2 = (flag2 && !this._onlyIdle);
				if (flag2)
				{
					this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? BeSung2.EState.Run2 : BeSung2.EState.Run);
				}
				else
				{
					this._state = BeSung2.EState.Idle2;
				}
			}
			else if (base.isInCamera)
			{
				this._state = BeSung2.EState.Idle1_2;
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
		if (this._state == BeSung2.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(4, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(3, 4, false);
				return;
			}
		}
		this.isMove = false;
		if (this._state == BeSung2.EState.Attack)
		{
			base.SetEmptyAnim(1, 0.3f);
		}
		this._state = BeSung2.EState.Hit_Ice;
		base.PlayAnim(5, 0, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.SetEmptyAnims(0f);
		base.PlayAnim(2, 0, false);
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
		float speed = Mathf.Min(this.cacheEnemy.Speed * 12f, 16f) + UnityEngine.Random.Range(-2f, 2f);
		float num = this.cacheEnemy.Damage / 4f;
		Vector3 vector = this.pos;
		Vector3 direction = this.pos;
		float num2 = 0.75f;
		string text = e.ToString();
		if (text != null)
		{
			if (!(text == "shoot1"))
			{
				if (!(text == "shoot2"))
				{
					if (text == "shoot3")
					{
						vector = this._boneGun5.GetWorldPosition(this.transform);
						direction = vector - this._boneGun6.GetWorldPosition(this.transform);
					}
				}
				else
				{
					vector = this._boneGun3.GetWorldPosition(this.transform);
					direction = vector - this._boneGun4.GetWorldPosition(this.transform);
				}
			}
			else
			{
				vector = this._boneGun1.GetWorldPosition(this.transform);
				direction = vector - this._boneGun2.GetWorldPosition(this.transform);
				num *= 2f;
				num2 *= 2f;
			}
		}
		GameManager.Instance.bulletManager.CreateBulletBeSung(speed, num, vector, direction, num2);
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		switch (text)
		{
		case "Attack":
			this.attackCount++;
			if (this.attackCount >= this._maxAttack)
			{
				this.attackCount = 0;
				base.ResetTimeReload();
				base.SetEmptyAnim(1, 0.2f);
				this.ChangeState();
			}
			else
			{
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
	private string strBoneAim;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun2;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun3;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneGun4;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun5;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneGun6;

	private BeSung2.EState _state;

	private Bone _boneAim;

	private Bone _boneGun1;

	private Bone _boneGun2;

	private Bone _boneGun3;

	private Bone _boneGun4;

	private Bone _boneGun5;

	private Bone _boneGun6;

	private bool _onlyIdle;

	private Vector2 _aimPos;

	private float _rootAimPosY;

	private float _timePingPongAim;

	private int _maxAttack;

	private enum EState
	{
		Aim,
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
