using System;
using Spine.Unity;
using UnityEngine;

public class GachaChildSpine : MonoBehaviour
{
	public void Show(bool hasTurn)
	{
		this.skeletonGraphic.AnimationState.SetAnimation(0, (!hasTurn) ? this.anim[0] : this.anim[1], true);
	}

	public void SetStatus(bool isActive)
	{
		this.skeletonGraphic.color = ((!isActive) ? Color.gray : Color.white);
	}

	public SkeletonGraphic skeletonGraphic;

	public AnimationReferenceAsset[] anim;
}
