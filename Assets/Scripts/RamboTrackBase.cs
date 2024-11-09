using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class RamboTrackBase : MonoBehaviour
{
	public void Init(SkeletonAnimation skeletonAnimation)
	{
		this.DeathAnim = skeletonAnimation.skeleton.Data.FindAnimation(this.death_Anim);
		this.AttackAnim = skeletonAnimation.skeleton.Data.FindAnimation(this.attack_Anim);
		this.RunAnim = skeletonAnimation.skeleton.Data.FindAnimation(this.run_Anim);
		this.HitAnim = skeletonAnimation.skeleton.Data.FindAnimation(this.hit_Anim);
		this.OpenAnim = skeletonAnimation.skeleton.Data.FindAnimation(this.open_Anim);
		this.IdleAnim = skeletonAnimation.skeleton.Data.FindAnimation(this.idleAnim);
	}

	[SpineAnimation("", "", true, false)]
	public string attack_Anim;

	[SpineAnimation("", "", true, false)]
	public string death_Anim;

	[SpineAnimation("", "", true, false)]
	public string hit_Anim;

	[SpineAnimation("", "", true, false)]
	public string open_Anim;

	[SpineAnimation("", "", true, false)]
	public string run_Anim;

	[SpineAnimation("", "", true, false)]
	public string idleAnim;

	public Spine.Animation AttackAnim;

	public Spine.Animation DeathAnim;

	public Spine.Animation HitAnim;

	public Spine.Animation OpenAnim;

	public Spine.Animation RunAnim;

	public Spine.Animation IdleAnim;
}
