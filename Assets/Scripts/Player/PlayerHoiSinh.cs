using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerHoiSinh : MonoBehaviour
	{
		private void Start()
		{
			this.skeleton.state.Event += this.HandleEvent;
			this.skeleton.state.Complete += this.HandleComplete;
		}

		public void OnPlay(Action OnStartEvent)
		{
			base.gameObject.SetActive(true);
			this.OnStartEvent = OnStartEvent;
			this.skeleton.state.SetAnimation(0, this.animHoiSinh, false);
		}

		private void HandleEvent(TrackEntry entry, Spine.Event e)
		{
			if (entry == null)
			{
				return;
			}
			string name = e.Data.Name;
			if (name != null)
			{
				if (name == "nhan-vat")
				{
					if (this.OnStartEvent != null)
					{
						this.OnStartEvent();
					}
				}
			}
		}

		private void HandleComplete(TrackEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			if (entry.Animation == this.animHoiSinh.Animation)
			{
				base.gameObject.SetActive(false);
			}
		}

		public SkeletonAnimation skeleton;

		public AnimationReferenceAsset animHoiSinh;

		private Action OnStartEvent;
	}
}
