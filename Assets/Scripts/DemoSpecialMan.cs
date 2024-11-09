using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class DemoSpecialMan : DemoBaseObject
{
	private void OnEnable()
	{
		base.Init();
		this.animParachute = this.skeletonAnimation.Skeleton.Data.FindAnimation(this.parachute);
		this.skeletonAnimation.state.SetAnimation(0, this.animParachute, true);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		this.animFire = this.skeletonAnimation.Skeleton.Data.FindAnimation(this.fire);
		this.skeletonAnimation.state.SetAnimation(0, this.animFire, true);
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string idle;

	[SpineAnimation("", "", true, false)]
	public string parachute;

	[SpineAnimation("", "", true, false)]
	public string fire;

	private Spine.Animation AnimIdle;

	private Spine.Animation animParachute;

	private Spine.Animation animFire;
}
