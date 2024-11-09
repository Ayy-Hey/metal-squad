using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayAnMau : MonoBehaviour
	{
		private void Start()
		{
			this.skeleton.state.Complete += this.HandleComplete;
		}

		public void OnPlay()
		{
			base.gameObject.SetActive(true);
			this.skeleton.state.SetAnimation(0, this.anim, true);
			if (this._audio != null)
			{
				SingletonGame<AudioController>.Instance.PlaySound_P(this._audio, 1f);
			}
			this.CountShow = 0;
		}

		private void HandleComplete(TrackEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			if (entry.Animation == this.anim.Animation)
			{
				this.CountShow++;
				if (this.CountShow > 3)
				{
					base.gameObject.SetActive(false);
				}
			}
		}

		public SkeletonAnimation skeleton;

		public AnimationReferenceAsset anim;

		public AudioClip _audio;

		private int CountShow;
	}
}
