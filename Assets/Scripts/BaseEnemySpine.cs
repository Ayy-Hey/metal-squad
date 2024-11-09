using System;
using System.Collections;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BaseEnemySpine : BaseEnemy
{
	public virtual IEnumerator PreloadSkeletonData()
	{
		yield return new WaitUntil(() => this.skeletonAnimation.skeletonDataAsset.GetSkeletonData(false) != null);
		EnemyManager.Instance.CountEnemyPreloadDataSuccess++;
		yield break;
	}

	public override void InitEnemy(EnemyCharactor DataEnemy, int Level)
	{
		base.InitEnemy(DataEnemy, Level);
		if (this.skeletonAnimation != null)
		{
			this.skeletonAnimation.skeleton.SetColor(new Color(1f, 1f, 1f, 1f));
			this.skeletonAnimation.skeleton.FlipX = true;
		}
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		this.InitSpine();
	}

	private void InitSpine()
	{
		this.DeadAnim = this.FindAnimation(this.deadAnim);
		this.IdleAnim = this.FindAnimation(this.idleAnim);
		this.WalkAnim = this.FindAnimation(this.walkAnim);
		this.AttackAnim = this.FindAnimation(this.attackAnim);
		this.ParachuterAnim = this.FindAnimation(this.parachuterAnim);
	}

	protected Spine.Animation FindAnimation(string name)
	{
		try
		{
			if (!string.IsNullOrEmpty(name))
			{
				return this.skeletonAnimation.skeleton.Data.FindAnimation(name);
			}
		}
		catch
		{
		}
		return null;
	}

	public override void SetDie()
	{
		if (this.State != ECharactor.DIE)
		{
			this.skeletonAnimation.state.ClearTrack(0);
			this.skeletonAnimation.state.SetAnimation(0, this.DeadAnim, false);
		}
		base.SetDie();
	}

	public override void SetAttack()
	{
		base.SetAttack();
		this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim, false);
	}

	public override void SetHit()
	{
		base.SetHit();
		this.skeletonAnimation.state.SetAnimation(2, this.hitAnim, false);
	}

	public override void SetParachuter(float gravity = 0.5f)
	{
		if (this.isParachuter)
		{
			return;
		}
		try
		{
			if (this.ID != 20 && this.ID != 21 && this.ID != 22)
			{
				this.isInit = false;
			}
			if (this.ID != 100)
			{
				this.rigidbody2D.gravityScale = gravity;
			}
			this.meshRenderer.enabled = true;
			this.isParachuter = true;
			this.skeletonAnimation.state.SetAnimation(0, this.IdleAnim, false);
			this.skeletonAnimation.state.AddAnimation(0, this.ParachuterAnim, true, 0f);
			if (this.lineBloodEnemy != null)
			{
				this.lineBloodEnemy.gameObject.SetActive(true);
				this.lineBloodEnemy.Reset();
			}
		}
		catch
		{
		}
	}

	public override void SetFlip(bool isFlip)
	{
		if (Time.timeSinceLevelLoad - this.timeWaitingFlip >= 0.5f)
		{
			this.timeWaitingFlip = Time.timeSinceLevelLoad;
			this.skeletonAnimation.skeleton.FlipX = isFlip;
		}
	}

	public SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false, startsWith = "Death01")]
	public string deadAnim;

	[SpineAnimation("", "", true, false, startsWith = "Idle")]
	public string idleAnim;

	[SpineAnimation("", "", true, false, startsWith = "Walk")]
	public string walkAnim;

	[SpineAnimation("", "", true, false, startsWith = "Attack01")]
	public string attackAnim;

	[SpineAnimation("", "", true, false, startsWith = "Hit")]
	public string hitAnim;

	[SpineAnimation("", "", true, false, startsWith = "Parachute")]
	public string parachuterAnim;

	public Spine.Animation AttackAnim;

	public Spine.Animation DeadAnim;

	public Spine.Animation IdleAnim;

	public Spine.Animation WalkAnim;

	public Spine.Animation ParachuterAnim;

	public Bone GunTipBone;

	public Bone GunTipBone2;

	public Bone GunTipBlood;

	public bool preloadSekeletonSuccess;

	private float timeWaitingFlip;
}
