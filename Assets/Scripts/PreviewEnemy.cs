using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PreviewEnemy : MonoBehaviour
{
	private void Start()
	{
		this.skeletonAnimation.state.SetAnimation(0, this.animIdle, true);
		this.skeletonAnimation.state.Complete += this.HandleComplete;
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		if (this.isIce)
		{
			this.skeletonAnimation.state.SetAnimation(0, this.animIdle, true);
			this.isIce = false;
		}
	}

	public void ShowIce()
	{
		if (this.isIce)
		{
			return;
		}
		this.isIce = true;
		this.skeletonAnimation.state.SetAnimation(0, this.animIce, false);
	}

	public SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string animIce;

	[SpineAnimation("", "", true, false)]
	public string animIdle;

	private bool isIce;
}
