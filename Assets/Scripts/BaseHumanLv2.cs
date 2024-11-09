using System;
using System.Collections.Generic;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BaseHumanLv2 : BaseHuman, IIce, IToxic
{
	public override void Hit(float damage)
	{
		base.Hit(damage * (1f / this.DEF_SCALE));
		this.timePingPongColor = 0f;
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			if (this.bodyCollider2D != null)
			{
				this.bodyCollider2D.enabled = false;
			}
			base.SetDie(true);
			this.State = ECharactor.DIE;
		}
	}

	protected void OnInit(EnemyCharactor DataEnemy, int Level, Vector2 pos)
	{
		base.InitEnemy(DataEnemy, Level);
		this.SetIdle();
		Vector3 vector = pos;
		this.transform.position = vector;
		this.PBorn = vector;
		this.posBegin = pos;
		this.SetTypeLevel(BaseEnemy.ELevel.NORMAL);
		if (this.IceAnim == null)
		{
			this.IceAnim = base.FindAnimation(this.hit_ice);
		}
		this.SPEED_SCALE = 1f;
		this.DAMAGE_SCALE = 1f;
		this.FIRE_RATE_SCALE = 1f;
		this.DEF_SCALE = 1f;
		this.PowerAnim = base.FindAnimation(this.powerAnim);
		this.boxCollider.enabled = true;
		this.Time_Reload_Attack = float.MinValue;
		this.isHitIcle = false;
		GameMode.Mode emode = GameMode.Instance.EMode;
		if (emode != GameMode.Mode.NORMAL)
		{
			if (emode != GameMode.Mode.HARD)
			{
				if (emode == GameMode.Mode.SUPER_HARD)
				{
					this.TIME_UPGRADE_TO_HARD = 1;
					this.TIME_UPGRADE_TO_SUPER_HARD = 1;
				}
			}
			else
			{
				this.TIME_UPGRADE_TO_HARD = 1;
				this.TIME_UPGRADE_TO_SUPER_HARD = 15;
			}
		}
		else
		{
			this.TIME_UPGRADE_TO_HARD = 15;
			this.TIME_UPGRADE_TO_SUPER_HARD = 15;
			if (GameManager.Instance.Level == global::ELevel.LEVEL_50)
			{
				this.TIME_UPGRADE_TO_HARD = 5;
				this.TIME_UPGRADE_TO_SUPER_HARD = 5;
			}
		}
	}

	protected virtual void RotateHandControl()
	{
	}

	protected virtual void OnShoot()
	{
	}

	protected virtual void OnKnife()
	{
	}

	protected virtual void OnGrenade()
	{
	}

	protected virtual void ChangeLevel(BaseEnemy.ELevel eLevel)
	{
		this.skeletonAnimation.state.SetAnimation(2, this.PowerAnim, false);
	}

	protected new void SetIdle()
	{
		if (this.State == ECharactor.IDLE)
		{
			return;
		}
		this.State = ECharactor.IDLE;
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
	}

	protected new void SetRun()
	{
		if (this.State == ECharactor.RUN || this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.RUN;
		this.LastTimeStuck = Time.timeSinceLevelLoad;
	}

	protected void SetAttack1()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim, false);
	}

	protected void SetAttack2()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim2, false);
	}

	public void SetTypeLevel(BaseEnemy.ELevel eLevel)
	{
		if (this.eLevel == eLevel)
		{
			return;
		}
		this.eLevel = eLevel;
		this.ChangeLevel(eLevel);
	}

	protected void SetTypeWeapon(BaseHumanLv2.ETypeWeapon typeWeapon)
	{
		if (this.TypeWeapon == typeWeapon)
		{
			return;
		}
		this.TypeWeapon = typeWeapon;
		if (this.TypeWeapon == BaseHumanLv2.ETypeWeapon.GRENADE)
		{
			this.TypeHandControl = BaseHumanLv2.ETypeHandControl.ROTATE;
		}
	}

	protected void SetTypeHandControl(BaseHumanLv2.ETypeHandControl typeHandControl)
	{
		if (this.TypeHandControl == typeHandControl)
		{
			return;
		}
		this.TypeHandControl = typeHandControl;
	}

	protected void OnUpdateAI()
	{
		base.CheckWithCamera();
		if (GameManager.Instance.StateManager.EState != EGamePlay.BEGIN && GameManager.Instance.StateManager.EState != EGamePlay.NONE)
		{
			if (!this.cacheEnemyData.ismove && base.GetPosition().x < CameraController.Instance.LeftCamera() && this.State != ECharactor.DIE)
			{
				base.SetDie(false);
				return;
			}
			if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState == EGamePlay.BEGIN || GameManager.Instance.StateManager.EState == EGamePlay.NONE)
			{
				return;
			}
			if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState == EGamePlay.BEGIN || GameManager.Instance.StateManager.EState == EGamePlay.NONE)
			{
				return;
			}
			if (!base.isInCamera && this.hasShow && this.State != ECharactor.DIE && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
			{
				this.TimeAutoDie += Time.deltaTime;
				if (this.TimeAutoDie >= 5f)
				{
					base.SetDie(false);
					return;
				}
			}
			else
			{
				this.TimeAutoDie = 0f;
			}
		}
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING && base.GetPosition().x > CameraController.Instance.RightCamera() + 3f && !base.isInCamera)
		{
			return;
		}
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			if (this.bodyCollider2D != null)
			{
				this.bodyCollider2D.enabled = false;
			}
			base.SetDie(true);
			this.State = ECharactor.DIE;
			this.isInit = false;
			return;
		}
		if (this.isHitIcle)
		{
			return;
		}
		if (this.TargetCurrent == null)
		{
			this.TargetCurrent = this.GetTarget();
			return;
		}
		if (base.isInCamera && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
		{
			this.TimeCounterUpgradeEnemy += Time.deltaTime;
			BaseEnemy.ELevel eLevel = this.eLevel;
			if (eLevel != BaseEnemy.ELevel.NORMAL)
			{
				if (eLevel != BaseEnemy.ELevel.HARD)
				{
					if (eLevel != BaseEnemy.ELevel.SUPER_HARD)
					{
					}
				}
				else if (this.TimeCounterUpgradeEnemy >= (float)this.TIME_UPGRADE_TO_SUPER_HARD)
				{
					this.TimeCounterUpgradeEnemy = 0f;
					this.SetTypeLevel(BaseEnemy.ELevel.SUPER_HARD);
				}
			}
			else if (this.TimeCounterUpgradeEnemy >= (float)this.TIME_UPGRADE_TO_HARD)
			{
				this.TimeCounterUpgradeEnemy = 0f;
				this.SetTypeLevel(BaseEnemy.ELevel.HARD);
			}
		}
		if (Time.timeSinceLevelLoad - this.TimeGetTarget >= 3f)
		{
			this.TargetCurrent = this.GetTarget();
			this.TimeGetTarget = Time.timeSinceLevelLoad;
		}
		if (this.cacheEnemyData.ismove)
		{
			this.OnDynamic(this.TargetCurrent);
		}
		else
		{
			this.OnStatic(this.TargetCurrent);
		}
		this.skeletonAnimation.skeleton.SetColor(base.PingPongColor());
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.transform.localPosition = new Vector3(this.GunTipBlood.WorldX, this.GunTipBlood.WorldY);
		}
		TrackEntry current = this.skeletonAnimation.state.GetCurrent(0);
		ECharactor state = this.State;
		if (state != ECharactor.IDLE)
		{
			if (state == ECharactor.RUN)
			{
				if (current == null || current.Animation != this.WalkAnim)
				{
					this.skeletonAnimation.state.SetAnimation(0, this.WalkAnim, true);
				}
				this.skeletonAnimation.loop = true;
				this.skeletonAnimation.AnimationName = this.walkAnim;
			}
		}
		else
		{
			if (current == null || current.Animation != this.IdleAnim)
			{
				this.skeletonAnimation.state.SetAnimation(0, this.IdleAnim, true);
			}
			this.skeletonAnimation.loop = true;
			this.skeletonAnimation.AnimationName = this.idleAnim;
		}
	}

	private void OnStatic(Transform TargetCurrent)
	{
		if (TargetCurrent == null)
		{
			return;
		}
		bool flip = TargetCurrent.position.x < this.transform.position.x;
		this.SetFlip(flip);
		this.RotateHandControl();
		this.ChooseTypeAttack(TargetCurrent);
	}

	private void OnDynamic(Transform TargetCurrent)
	{
		if (this.isJump || TargetCurrent == null)
		{
			return;
		}
		float num = Vector2.Distance(this.transform.position, TargetCurrent.position);
		this.DetectedCamera();
		this.TimeHoldState += Time.deltaTime;
		switch (this.EAIType)
		{
		case EEnemyAIType.CLEAR:
			this.ResetAI(num);
			break;
		case EEnemyAIType.MOVE_TO_TARGET:
		{
			bool flip = TargetCurrent.position.x < this.transform.position.x;
			this.SetFlip(flip);
			this.Move();
			if (num <= this.cacheEnemy.CacheVision)
			{
				this.EAIType = EEnemyAIType.FINDING;
				this.SetIdle();
				this.TimeDelay = Time.timeSinceLevelLoad;
			}
			break;
		}
		case EEnemyAIType.FINDING:
			if (Time.timeSinceLevelLoad - this.TimeDelay >= 1f)
			{
				if (!GameManager.Instance.player.IsInVisible() && this.TargetInView(TargetCurrent, (TargetCurrent.position.x <= base.Origin().x) ? Vector2.left : Vector2.right) && num - 1f <= this.cacheEnemy.CacheVision)
				{
					this.SetIdle();
					this.EAIType = EEnemyAIType.HAS_FOUND;
					this.TimeHoldState = 0f;
				}
				else
				{
					this.ResetAI(num);
					this.P0 = this.transform.position;
					this.PTarget = TargetCurrent.position;
					float num2 = Vector3.Distance(this.P0, this.PTarget);
					if (num2 < 1.5f)
					{
						this.EAIType = EEnemyAIType.NOT_FOUND_IDLE;
						this.SetIdle();
					}
					else
					{
						this.IsMovedTarget = false;
						this.EAIType = EEnemyAIType.NOT_FOUND_WALK;
						this.SetRun();
					}
				}
			}
			break;
		case EEnemyAIType.NOT_FOUND_WALK:
			this.ResetAI(num);
			this.TimeDelay2 += Time.deltaTime;
			if (!this.IsMovedTarget && this.TimeDelay2 >= 1f)
			{
				this.SetRun();
				bool flip = TargetCurrent.position.x < this.transform.position.x;
				this.SetFlip(flip);
				this.Move();
				num = Mathf.Abs(this.transform.position.x - TargetCurrent.position.x);
				if (num <= 0.5f)
				{
					this.TimeDelay2 = 0f;
					this.IsMovedTarget = true;
					this.SetIdle();
				}
			}
			else if (this.IsMovedTarget && this.TimeDelay2 >= 1f)
			{
				this.SetRun();
				bool flip = this.P0.x < this.transform.position.x;
				this.SetFlip(flip);
				this.Move();
				float num3 = Mathf.Abs(this.transform.position.x - this.P0.x);
				if (num3 <= 0.5f)
				{
					this.TimeDelay2 = 0f;
					this.IsMovedTarget = false;
					this.SetIdle();
				}
			}
			if (!GameManager.Instance.player.IsInVisible() && this.TargetInView(TargetCurrent, (TargetCurrent.position.x <= base.Origin().x) ? Vector2.left : Vector2.right) && num <= this.cacheEnemy.CacheVision)
			{
				this.SetIdle();
				this.EAIType = EEnemyAIType.HAS_FOUND;
				this.TimeHoldState = 0f;
			}
			break;
		case EEnemyAIType.NOT_FOUND_IDLE:
			this.ResetAI(num);
			if (!GameManager.Instance.player.IsInVisible() && this.TargetInView(TargetCurrent, (TargetCurrent.position.x <= base.Origin().x) ? Vector2.left : Vector2.right) && num <= this.cacheEnemy.CacheVision)
			{
				this.SetIdle();
				this.EAIType = EEnemyAIType.HAS_FOUND;
				this.TimeHoldState = 0f;
			}
			break;
		case EEnemyAIType.HAS_FOUND:
		{
			bool flip = TargetCurrent.position.x < this.transform.position.x;
			this.SetFlip(flip);
			this.RotateHandControl();
			this.ChooseTypeAttack(TargetCurrent);
			this.ResetAI(num);
			if (!this.TargetInView(TargetCurrent, (TargetCurrent.position.x <= base.Origin().x) ? Vector2.left : Vector2.right))
			{
				this.EAIType = EEnemyAIType.MOVE_TO_TARGET;
			}
			break;
		}
		case EEnemyAIType.HAS_GULF:
			this.SetIdle();
			this.ChooseTypeAttack(TargetCurrent);
			if ((this.saveLastFlip && TargetCurrent.position.x > this.PGulf.x) || (!this.saveLastFlip && TargetCurrent.position.x < this.PGulf.x))
			{
				this.EAIType = EEnemyAIType.MOVE_TO_TARGET;
			}
			break;
		case EEnemyAIType.HAS_WALL:
		{
			this.TimeDelay2 += Time.deltaTime;
			if (!this.IsMovedTarget && this.TimeDelay2 >= 1f)
			{
				this.SetRun();
				bool flip = this.PBorn.x < this.transform.position.x;
				this.SetFlip(flip);
				this.PBorn.y = this.transform.position.y;
				num = Mathf.Abs(this.PBorn.x - this.transform.position.x);
				this.Move();
				if (num <= 0.5f)
				{
					this.TimeDelay2 = 0f;
					this.IsMovedTarget = true;
					this.SetIdle();
				}
			}
			else if (this.IsMovedTarget && this.TimeDelay2 >= 1f)
			{
				bool flip = this.P0.x < this.transform.position.x;
				this.SetFlip(flip);
				this.SetRun();
				this.Move();
				float num4 = Mathf.Abs(this.transform.position.x - this.P0.x);
				if (num4 <= 0.5f)
				{
					this.TimeDelay2 = 0f;
					this.IsMovedTarget = false;
					this.SetIdle();
				}
			}
			bool flag = (this.transform.position.x >= this.P0.x && TargetCurrent.position.x >= this.P0.x) || (this.transform.position.x < this.P0.x && TargetCurrent.position.x < this.P0.x);
			if (!GameManager.Instance.player.IsInVisible() && this.TargetInView(TargetCurrent, (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left) && flag)
			{
				this.SetIdle();
				this.EAIType = EEnemyAIType.HAS_FOUND;
				this.TimeHoldState = 0f;
			}
			break;
		}
		case EEnemyAIType.WITHOUT_CAMERA:
		{
			bool flip = TargetCurrent.position.x < this.transform.position.x;
			this.SetFlip(flip);
			this.Move();
			if (base.isInCamera)
			{
				this.EAIType = EEnemyAIType.MOVE_TO_TARGET;
			}
			break;
		}
		}
	}

	public override void ResetAIEnemy(bool isResetPosition)
	{
		this.EAIType = EEnemyAIType.CLEAR;
		if (isResetPosition)
		{
			this.transform.position = this.posBegin;
		}
	}

	private void ChooseTypeAttack(Transform TargetCurrent)
	{
		if (GameManager.Instance.player.IsInVisible())
		{
			this.SetIdle();
			this.ResetAIEnemy(false);
			return;
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (Time.timeSinceLevelLoad - this.Time_Reload_Attack < this.cacheEnemy.Time_Reload_AttackLv2 * (1f / this.FIRE_RATE_SCALE))
		{
			return;
		}
		this.Time_Reload_Attack = Time.timeSinceLevelLoad;
		if (this.boxCollider != null)
		{
			this.boxCollider.enabled = true;
		}
		if (this.bodyCollider2D != null)
		{
			this.bodyCollider2D.enabled = true;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.Origin(), (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left, 1.5f, this.layerMaskKnife);
		bool flag = this.TargetInView(TargetCurrent, (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left);
		switch (this.TypeWeapon)
		{
		case BaseHumanLv2.ETypeWeapon.KNIFE:
			if (raycastHit2D.collider)
			{
				this.OnKnife();
			}
			else
			{
				this.SetIdle();
			}
			break;
		case BaseHumanLv2.ETypeWeapon.GUN:
			if (flag)
			{
				this.OnShoot();
			}
			break;
		case BaseHumanLv2.ETypeWeapon.GRENADE:
			this.OnGrenade();
			break;
		case BaseHumanLv2.ETypeWeapon.GUN_AND_KNIFE:
			if (raycastHit2D.collider)
			{
				this.OnKnife();
			}
			else if (flag)
			{
				this.OnShoot();
			}
			break;
		case BaseHumanLv2.ETypeWeapon.GRENADE_AND_KNIFE:
			if (raycastHit2D.collider)
			{
				this.OnKnife();
			}
			else
			{
				this.OnGrenade();
			}
			break;
		}
	}

	private void ResetAI(float dts)
	{
		if (dts > this.cacheEnemy.CacheVision)
		{
			this.EAIType = EEnemyAIType.MOVE_TO_TARGET;
			this.SetRun();
		}
		else if (dts <= this.cacheEnemy.CacheVision && !GameManager.Instance.player.IsInVisible())
		{
			this.SetIdle();
			this.EAIType = EEnemyAIType.HAS_FOUND;
			this.TimeHoldState = 0f;
		}
	}

	private void DetectedCamera()
	{
		if (this.EAIType == EEnemyAIType.HAS_GULF || this.EAIType == EEnemyAIType.HAS_WALL || this.EAIType == EEnemyAIType.WITHOUT_CAMERA)
		{
			return;
		}
		float num = Mathf.Abs(CameraController.Instance.transform.position.x - this.transform.position.x);
		if (num > 6f)
		{
			this.SetRun();
			this.EAIType = EEnemyAIType.WITHOUT_CAMERA;
		}
	}

	private void DetectedGulf()
	{
		if (this.EAIType == EEnemyAIType.HAS_GULF || this.State != ECharactor.RUN)
		{
			return;
		}
		this.SetIdle();
		this.EAIType = EEnemyAIType.HAS_GULF;
		this.PGulf = this.transform.position;
		this.saveLastFlip = this.skeletonAnimation.skeleton.FlipX;
	}

	private void DetectedWall()
	{
		if (this.EAIType == EEnemyAIType.HAS_WALL)
		{
			return;
		}
		this.SetIdle();
		this.EAIType = EEnemyAIType.HAS_WALL;
		this.PWall = this.transform.position;
		this.saveLastFlip = this.skeletonAnimation.skeleton.FlipX;
		this.IsMovedTarget = false;
		this.P0 = this.transform.position;
	}

	protected void Move()
	{
		this.State = ECharactor.RUN;
		if (this.rigidbody2D.velocity.x < this.cacheEnemy.Speed * this.SPEED_SCALE)
		{
			this.rigidbody2D.AddForce(((!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left) * this.rigidbody2D.mass, ForceMode2D.Impulse);
		}
		if (Mathf.Abs(this.rigidbody2D.velocity.x) > this.cacheEnemy.Speed * this.SPEED_SCALE)
		{
			this.rigidbody2D.velocity = new Vector2(Mathf.Sign(this.rigidbody2D.velocity.x) * this.cacheEnemy.Speed * this.SPEED_SCALE, this.rigidbody2D.velocity.y);
		}
		if (Time.timeSinceLevelLoad - this.LastTimeStuck >= 2f)
		{
			this.LastTimeStuck = Time.timeSinceLevelLoad;
			bool flag = this.StuckBound.Contains(this.transform.position);
			Vector2 position = this.transform.position;
			position.x -= 0.25f;
			position.y -= 0.25f;
			this.StuckBound = new Rect(position, new Vector2(0.5f, 0.5f));
			if (flag)
			{
				this.Time_Stuck_Wall = 0f;
				Vector2 a = new Vector2(0f, 1f);
				this.rigidbody2D.AddForce(a * 600f * this.rigidbody2D.mass);
				this.ResetAI(Vector2.Distance(this.transform.position, this.TargetCurrent.position));
			}
		}
	}

	protected bool TargetInView(Transform tfTarget, Vector2 dir)
	{
		Vector2 a = new Vector2(tfTarget.position.x, tfTarget.position.y);
		Vector2 vector = a - base.Origin();
		vector.Normalize();
		Vector2 origin = new Vector2(this.GunTipBone.WorldX + this.transform.position.x, this.GunTipBone.WorldY + this.transform.position.y);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, vector);
		if (raycastHit2D.collider && raycastHit2D.collider.CompareTag("platform") && this.TypeHandControl == BaseHumanLv2.ETypeHandControl.NORMAL)
		{
			return false;
		}
		float num = Vector3.Angle(vector, dir);
		float num2 = Vector2.Distance(tfTarget.position, base.Origin());
		return num2 <= 1.5f || num <= (float)((this.TypeHandControl != BaseHumanLv2.ETypeHandControl.NORMAL) ? 45 : 25);
	}

	protected new Transform GetTarget()
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		bool flag = false;
		float num3 = float.MaxValue;
		int index = int.MaxValue;
		Vector2 a = Vector2.zero;
		Vector2 v = Vector2.zero;
		List<BaseCharactor> list = new List<BaseCharactor>();
		for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
		{
			a = GameManager.Instance.ListRambo[i].Origin();
			v = a - base.Origin();
			v.Normalize();
			float num4 = Vector3.Angle(v, (a.x <= base.Origin().x) ? Vector2.left : Vector2.right);
			float num5 = Vector2.Distance(a, base.Origin());
			if (num5 <= 1.5f)
			{
				num4 = 0f;
			}
			float num6 = Mathf.Abs(a.x - base.Origin().x);
			if (num4 <= (float)((this.TypeHandControl != BaseHumanLv2.ETypeHandControl.NORMAL) ? 45 : 25))
			{
				list.Add(GameManager.Instance.ListRambo[i]);
			}
			if (num4 < num)
			{
				num = num4;
			}
			if (num6 < num2)
			{
				num2 = num6;
			}
			if (flag)
			{
			}
		}
		if (list.Count <= 0)
		{
			return GameManager.Instance.ListRambo[UnityEngine.Random.Range(0, GameManager.Instance.ListRambo.Count)].transform;
		}
		num = float.MaxValue;
		num2 = float.MaxValue;
		int num7 = int.MaxValue;
		flag = false;
		int num8 = int.MaxValue;
		int num9 = int.MaxValue;
		for (int j = 0; j < list.Count; j++)
		{
			float hpcurrent = list[j].HPCurrent;
			a = list[j].Origin();
			v = a - base.Origin();
			v.Normalize();
			float num4 = Vector3.Angle(v, (a.x <= base.Origin().x) ? Vector2.left : Vector2.right);
			float num6 = Mathf.Abs(a.x - base.Origin().x);
			if (num4 < num)
			{
				num = num4;
			}
			if (num6 < num2)
			{
				num2 = num6;
				num7 = j;
			}
			if (hpcurrent < num3)
			{
				num3 = hpcurrent;
				index = j;
			}
			if (flag)
			{
				num8 = j;
			}
			else
			{
				num9 = j;
			}
		}
		switch (this.TypeTargetAttack)
		{
		case BaseHumanLv2.ETypeTargetAttack.MAIN_CHARACTER:
			return list[(num8 == int.MaxValue) ? num7 : num8].transform;
		case BaseHumanLv2.ETypeTargetAttack.NPC:
			return list[(num9 == int.MaxValue) ? num7 : num9].transform;
		case BaseHumanLv2.ETypeTargetAttack.HP:
			return list[index].transform;
		default:
			return list[num7].transform;
		}
	}

	public void Hit(float damaged, EWeapon weapon)
	{
		base.AddHealthPoint(damaged, weapon);
		if (!this.isHitIcle && this.State != ECharactor.DIE)
		{
			this.isHitIcle = true;
			if (this.IceAnim != null)
			{
				this.skeletonAnimation.state.SetAnimation(0, this.IceAnim, false);
			}
			else
			{
				this.isHitIcle = false;
			}
		}
	}

	public void HitToxic(float damaged_percent)
	{
		this.cacheEnemy.Speed = this.cacheEnemy.Speed - this.cacheEnemy.Speed * damaged_percent;
		this.cacheEnemy.Speed_BulletLv2 = this.cacheEnemy.Speed_BulletLv2 - this.cacheEnemy.Speed_BulletLv2 * damaged_percent;
		this.cacheEnemy.Time_Reload_AttackLv2 = this.cacheEnemy.Time_Reload_AttackLv2 + this.cacheEnemy.Time_Reload_AttackLv2 * damaged_percent;
	}

	public void ReleaseToxic(float damaged_percent)
	{
		float num = damaged_percent / (1f - damaged_percent);
		this.cacheEnemy.Speed = this.cacheEnemy.Speed + this.cacheEnemy.Speed * num;
		this.cacheEnemy.Speed_BulletLv2 = this.cacheEnemy.Speed_BulletLv2 + this.cacheEnemy.Speed_BulletLv2 * num;
		this.cacheEnemy.Time_Reload_AttackLv2 = this.cacheEnemy.Time_Reload_AttackLv2 - this.cacheEnemy.Time_Reload_AttackLv2 * num;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		string tag = other.tag;
		if (tag != null)
		{
			if (!(tag == "Found_Gulf"))
			{
				if (!(tag == "Found_Wall"))
				{
					if (tag == "Found_Jump")
					{
						this.Time_Stuck_Wall = 0f;
						string[] array = other.name.Split(new char[]
						{
							'/'
						});
						Vector2 a = new Vector2(float.Parse(array[0]) * (float)((!this.skeletonAnimation.skeleton.FlipX) ? 1 : -1), float.Parse(array[1]));
						this.rigidbody2D.AddForce(a * 600f * this.rigidbody2D.mass);
						this.isJump = true;
						this.ResetAI(Vector2.Distance(this.transform.position, this.TargetCurrent.position));
					}
				}
				else
				{
					this.Time_Stuck_Wall = 0f;
					this.DetectedWall();
				}
			}
			else
			{
				this.Time_Stuck_Wall = 0f;
				if (GameManager.Instance.StateManager.EState == EGamePlay.BEGIN)
				{
					this.DetectedWall();
					return;
				}
				this.DetectedGulf();
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (this.EAIType == EEnemyAIType.HAS_WALL && other.CompareTag("Found_Wall"))
		{
			this.Time_Stuck_Wall += Time.deltaTime;
			if (this.Time_Stuck_Wall >= 3f)
			{
				this.Time_Stuck_Wall = 0f;
				this.DetectedGulf();
			}
		}
	}

	[SpineAnimation("", "", true, false, startsWith = "Hit_ice")]
	public string hit_ice;

	public Spine.Animation IceAnim;

	[SerializeField]
	[Header("______________________AI_____________________")]
	protected BaseHumanLv2.ETypeWeapon TypeWeapon = BaseHumanLv2.ETypeWeapon.GUN;

	[SerializeField]
	protected BaseHumanLv2.ETypeHandControl TypeHandControl;

	protected EEnemyAIType EAIType;

	[SerializeField]
	protected BaseHumanLv2.ETypeTargetAttack TypeTargetAttack;

	private Vector3 P0;

	private Vector3 PTarget;

	private Vector3 PGulf;

	private Vector3 PWall;

	private Vector3 PBorn;

	private bool saveLastFlip;

	private float TimeDelay;

	private float TimeDelay2;

	private bool IsMovedTarget;

	protected Transform TargetCurrent;

	private float TimeGetTarget;

	protected float SPEED_SCALE;

	protected float DAMAGE_SCALE;

	protected float FIRE_RATE_SCALE;

	protected float DEF_SCALE;

	[SpineAnimation("", "", true, false, startsWith = "PowerUp")]
	[SerializeField]
	protected string powerAnim;

	protected Spine.Animation PowerAnim;

	[SerializeField]
	protected LayerMask layerMaskKnife;

	[SerializeField]
	private LayerMask layerMaskGulf;

	[SerializeField]
	private LayerMask layerWall;

	private float TimeHoldState;

	protected bool isHitIcle;

	private Vector2 posBegin;

	private float Time_Stuck_Wall;

	private float LastTimeStuck;

	private Rect StuckBound;

	private const float STUCK_BOUND_WIDTH = 0.5f;

	private const float STUCK_BOUND_HEIGHT = 0.5f;

	private const float CHECK_STUCK_DELTA = 2f;

	public enum ETypeWeapon
	{
		KNIFE,
		GUN,
		GRENADE,
		GUN_AND_KNIFE,
		GRENADE_AND_KNIFE
	}

	public enum ETypeHandControl
	{
		NORMAL,
		ROTATE
	}

	public enum ETypeTargetAttack
	{
		NORMAL,
		MAIN_CHARACTER,
		NPC,
		HP
	}
}
