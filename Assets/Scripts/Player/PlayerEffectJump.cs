using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerEffectJump : MonoBehaviour
	{
		private void Start()
		{
			this.skeleton.state.Complete += this.HandleComplete;
		}

		public void OnPlay(Vector3 pos)
		{
			base.gameObject.SetActive(true);
			base.transform.position = pos;
			this.skeleton.state.SetAnimation(0, this.anim, false).TimeScale = 1.5f;
		}

		private void HandleComplete(TrackEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			base.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			GameManager.Instance.fxManager.PoolFxJump.Store(this);
		}

		public SkeletonAnimation skeleton;

		public AnimationReferenceAsset anim;
	}
}
