using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerShield : MonoBehaviour
	{
		private void Start()
		{
			this.skeleton.state.Complete += this.HandleComplete;
		}

		public void OnPlay()
		{
			this.countShow = 0;
			base.gameObject.SetActive(true);
			this.skeleton.state.SetAnimation(0, this.anim, true);
		}

		private void HandleComplete(TrackEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			if (entry.Animation == this.anim.Animation)
			{
				this.countShow++;
				if (this.countShow >= 5)
				{
					base.gameObject.SetActive(false);
				}
			}
		}

		public SkeletonAnimation skeleton;

		public AnimationReferenceAsset anim;

		private int countShow;
	}
}
