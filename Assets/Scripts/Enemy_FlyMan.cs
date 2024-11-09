using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Enemy_FlyMan : BaseEnemy2
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
		if (this._boneBan == null)
		{
			this._boneBan = this.skeletonAnimation.Skeleton.FindBone(this.strBoneBan);
			this._boneMini1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneMini1);
			this._boneMini2 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneMini2);
			this._boneMini3 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneMini3);
			this._boneMini4 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneMini4);
			this._boneMini5 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneMini5);
			this._boneMini6 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneMini6);
			this._boneRocket1 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneRoket1);
			this._boneRocket2 = this.skeletonAnimation.Skeleton.FindBone(this.strBoneRoket2);
		}
		base.Gravity = 0f;
		this.ChangeState();
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
		Enemy_FlyMan.EState state = this._state;
		switch (state)
		{
		case Enemy_FlyMan.EState.Aim_MiniGun:
			track = 2;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			this._targetPos = this._boneMini1.GetLocalPosition();
			this._boneBan.SetPosition(this._targetPos);
			break;
		case Enemy_FlyMan.EState.Attack_Mini:
			track = 1;
			break;
		case Enemy_FlyMan.EState.Attack_Rocket:
			track = 1;
			base.Flip = (this.pos.x > GameManager.Instance.player.transform.position.x);
			break;
		default:
			switch (state)
			{
			case Enemy_FlyMan.EState.Idle:
				loop = true;
				break;
			case Enemy_FlyMan.EState.Run:
			{
				loop = true;
				this.isMove = true;
				float num = UnityEngine.Random.Range(2f, 6f);
				this._targetPos.x = ((this.pos.x <= CameraController.Instance.camPos.x) ? (this.pos.x + num) : (this.pos.x - num));
				this._targetPos.y = UnityEngine.Random.Range(CameraController.Instance.camPos.y, CameraController.Instance.camPos.y + 3f);
				base.Flip = (this.pos.x > this._targetPos.x);
				break;
			}
			}
			break;
		}
		base.PlayAnim((int)this._state, track, loop);
	}

	protected override void UpdateState(float deltaTime)
	{
		base.UpdateState(deltaTime);
		Enemy_FlyMan.EState state = this._state;
		if (state != Enemy_FlyMan.EState.Aim_MiniGun)
		{
			if (state != Enemy_FlyMan.EState.Attack_Mini)
			{
				if (this.isMove)
				{
					this.pos = Vector3.SmoothDamp(this.pos, this._targetPos, ref this._veloSmoothTarget, 0.5f);
					bool flag = Vector3.Distance(this.pos, this._targetPos) < 0.1f;
					if (flag)
					{
						this.isMove = false;
						this.ChangeState();
					}
				}
				if (this._coolDownIdle > 0f)
				{
					this._coolDownIdle -= deltaTime;
					if (this._coolDownIdle <= 0f)
					{
						this.ChangeState();
					}
				}
			}
			else
			{
				this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
				this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
				this._targetLockPos.x = Mathf.Max(0f, this._targetLockPos.x);
				this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.3f);
				this._boneBan.SetPosition(this._targetPos);
			}
		}
		else
		{
			this._targetLockPos = GameManager.Instance.player.tfOrigin.position - this.pos;
			this._targetLockPos.x = ((!base.Flip) ? this._targetLockPos.x : (-this._targetLockPos.x));
			this._targetLockPos.x = Mathf.Max(0f, this._targetLockPos.x);
			this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLockPos, ref this._veloSmoothTarget, 0.3f);
			this._boneBan.SetPosition(this._targetPos);
			bool flag2 = Vector3.Distance(this._targetPos, this._targetLockPos) < 0.5f;
			if (flag2)
			{
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
		if (this.isAttack && base.isInCamera)
		{
			Enemy_FlyMan.EState state = this._state;
			if (state != Enemy_FlyMan.EState.Aim_MiniGun)
			{
				this._state = ((UnityEngine.Random.Range(0, 6) >= 3) ? Enemy_FlyMan.EState.Aim_MiniGun : Enemy_FlyMan.EState.Attack_Rocket);
			}
			else
			{
				this._state = Enemy_FlyMan.EState.Attack_Mini;
			}
		}
		else if (base.isInCamera && UnityEngine.Random.Range(0, 3) >= 2)
		{
			this._state = Enemy_FlyMan.EState.Idle;
			this._coolDownIdle = this.anims[11].Duration;
		}
		else
		{
			this._state = Enemy_FlyMan.EState.Run;
			this._coolDownIdle = 0f;
		}
		base.ChangeState();
	}

	protected override void OnPowerUp()
	{
		base.OnPowerUp();
		base.PlayAnim(12, 3, false);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
	}

	protected override void Hit()
	{
		base.Hit();
		if (this._state == Enemy_FlyMan.EState.Hit_Ice)
		{
			return;
		}
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(9, 4, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				base.PlayAnim(8, 4, false);
				return;
			}
		}
		this.isMove = false;
		this._coolDownIdle = 0f;
		base.SetEmptyAnim(2, 0f);
		base.SetEmptyAnim(1, 0f);
		this._state = Enemy_FlyMan.EState.Hit_Ice;
		base.PlayAnim(10, 4, false);
	}

	protected override void Die(bool isRambo)
	{
		base.Die(isRambo);
		base.Gravity = 0.5f;
		base.SetEmptyAnims(0f);
		EWeapon lastWeapon = this.lastWeapon;
		if (lastWeapon != EWeapon.GRENADE_ICE)
		{
			if (lastWeapon == EWeapon.GRENADE_MOLOYOV || lastWeapon == EWeapon.FLAME)
			{
				base.PlayAnim(6, 0, false);
				return;
			}
			if (lastWeapon == EWeapon.THUNDER)
			{
				base.PlayAnim(5, 0, false);
				return;
			}
			if (lastWeapon != EWeapon.ICE)
			{
				if (base.Flip == GameManager.Instance.player._PlayerSpine.FlipX)
				{
					base.PlayAnim(3, 0, false);
				}
				else
				{
					base.PlayAnim(4, 0, false);
				}
				return;
			}
		}
		base.PlayAnim(7, 0, false);
	}

	protected override void Disable()
	{
		base.Disable();
	}

	protected override void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.OnEvent(trackEntry, e);
		string text = e.ToString();
		if (text != null)
		{
			if (!(text == "shoot-rocket"))
			{
				if (!(text == "shootmini1"))
				{
					if (!(text == "shootmini2"))
					{
						if (text == "shootmini3")
						{
							Vector3 worldPosition = this._boneMini5.GetWorldPosition(this.transform);
							Vector3 vector = worldPosition - this._boneMini6.GetWorldPosition(this.transform);
							float damage = this.cacheEnemy.Damage / 2f;
							float speed = Mathf.Min(this.cacheEnemy.Speed * 3f, 7f);
							GameManager.Instance.bulletManager.CreateBulletEnemy(10, vector, worldPosition, damage, speed, 0f).spriteRenderer.flipX = false;
						}
					}
					else
					{
						Vector3 worldPosition = this._boneMini3.GetWorldPosition(this.transform);
						Vector3 vector = worldPosition - this._boneMini4.GetWorldPosition(this.transform);
						float damage = this.cacheEnemy.Damage / 2f;
						float speed = Mathf.Min(this.cacheEnemy.Speed * 3f, 7f);
						GameManager.Instance.bulletManager.CreateBulletEnemy(10, vector, worldPosition, damage, speed, 0f).spriteRenderer.flipX = false;
					}
				}
				else
				{
					Vector3 worldPosition = this._boneMini1.GetWorldPosition(this.transform);
					Vector3 vector = worldPosition - this._boneMini2.GetWorldPosition(this.transform);
					float damage = this.cacheEnemy.Damage / 2f;
					float speed = Mathf.Min(this.cacheEnemy.Speed * 3f, 7f);
					GameManager.Instance.bulletManager.CreateBulletEnemy(10, vector, worldPosition, damage, speed, 0f).spriteRenderer.flipX = false;
				}
			}
			else
			{
				Vector3 worldPosition = this._boneRocket1.GetWorldPosition(this.transform);
				Vector3 vector = worldPosition - this._boneRocket2.GetWorldPosition(this.transform);
				GameManager.Instance.bulletManager.CreateRocketBossType1(this.cacheEnemy.Damage, this.cacheEnemy.Speed, vector, worldPosition, GameManager.Instance.player.tfOrigin, 0.5f).transform.localScale = Vector3.one / 2f;
			}
		}
	}

	protected override void OnComplete(TrackEntry trackEntry)
	{
		base.OnComplete(trackEntry);
		string text = trackEntry.ToString();
		switch (text)
		{
		case "Hit-Ice":
			this.ChangeState();
			break;
		case "Attack_mini":
			this.attackCount++;
			if (this.attackCount > this.modeLv)
			{
				base.SetEmptyAnim(2, 0f);
				base.SetEmptyAnim(1, 0f);
				this.attackCount = 0;
				base.ResetTimeReload();
				this.ChangeState();
			}
			else
			{
				base.PlayAnim((int)this._state, 0, false);
			}
			break;
		case "Attack_rocket":
			this.attackCount++;
			if (this.attackCount > this.modeLv)
			{
				base.SetEmptyAnim(1, 0f);
				this.attackCount = 0;
				base.ResetTimeReload();
				this.ChangeState();
			}
			else
			{
				base.PlayAnim((int)this._state, 0, false);
			}
			break;
		case "Death":
		case "Death2":
		case "Death-Elec":
		case "Death_Fire":
		case "Death_Ice":
			base.Gravity = 0f;
			base.gameObject.SetActive(false);
			break;
		}
	}

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneBan;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneRoket1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneRoket2;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneMini1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneMini2;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneMini3;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string strBoneMini4;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneMini5;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string strBoneMini6;

	private Enemy_FlyMan.EState _state;

	private Bone _boneBan;

	private Bone _boneRocket1;

	private Bone _boneRocket2;

	private Bone _boneMini1;

	private Bone _boneMini2;

	private Bone _boneMini3;

	private Bone _boneMini4;

	private Bone _boneMini5;

	private Bone _boneMini6;

	private Vector3 _targetPos;

	private Vector3 _targetLockPos;

	private Vector3 _veloSmoothTarget;

	private float _coolDownIdle;

	private enum EState
	{
		Aim_MiniGun,
		Attack_Mini,
		Attack_Rocket,
		Death,
		Death_2,
		Death_Elec,
		Death_Fire,
		Death_Ice,
		Hit,
		Hit_Elec,
		Hit_Ice,
		Idle,
		PowerUp,
		Run
	}
}
