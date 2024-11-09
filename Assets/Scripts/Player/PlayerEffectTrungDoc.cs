using System;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerEffectTrungDoc : MonoBehaviour
	{
		public void OnPlay()
		{
			base.gameObject.SetActive(true);
			this.skeleton.state.SetAnimation(0, this.anim, true);
		}

		public SkeletonAnimation skeleton;

		public AnimationReferenceAsset anim;
	}
}
