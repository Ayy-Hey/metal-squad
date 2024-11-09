using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick LongTap")]
	public class QuickLongTap : QuickBase
	{
		public QuickLongTap()
		{
			this.quickActionName = "QuickLongTap" + base.GetInstanceID().ToString();
		}

		private void Update()
		{
			this.currentGesture = EasyTouch.current;
			if (!this.is2Finger)
			{
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchStart && this.fingerIndex == -1 && this.IsOverMe(this.currentGesture))
				{
					this.fingerIndex = this.currentGesture.fingerIndex;
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_LongTapStart && this.actionTriggering == QuickLongTap.ActionTriggering.Start && (this.currentGesture.fingerIndex == this.fingerIndex || this.isMultiTouch))
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_LongTap && this.actionTriggering == QuickLongTap.ActionTriggering.InProgress && (this.currentGesture.fingerIndex == this.fingerIndex || this.isMultiTouch))
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_LongTapEnd && this.actionTriggering == QuickLongTap.ActionTriggering.End && (this.currentGesture.fingerIndex == this.fingerIndex || this.isMultiTouch))
				{
					this.DoAction(this.currentGesture);
					this.fingerIndex = -1;
				}
			}
			else
			{
				if (this.currentGesture.type == EasyTouch.EvtType.On_LongTapStart2Fingers && this.actionTriggering == QuickLongTap.ActionTriggering.Start)
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_LongTap2Fingers && this.actionTriggering == QuickLongTap.ActionTriggering.InProgress)
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_LongTapEnd2Fingers && this.actionTriggering == QuickLongTap.ActionTriggering.End)
				{
					this.DoAction(this.currentGesture);
				}
			}
		}

		private void DoAction(Gesture gesture)
		{
			if (this.IsOverMe(gesture))
			{
				this.onLongTap.Invoke(gesture);
			}
		}

		private bool IsOverMe(Gesture gesture)
		{
			bool result = false;
			if (this.realType == QuickBase.GameObjectType.UI)
			{
				if (gesture.isOverGui && (gesture.pickedUIElement == base.gameObject || gesture.pickedUIElement.transform.IsChildOf(base.transform)))
				{
					result = true;
				}
			}
			else if (((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI) && EasyTouch.GetGameObjectAt(gesture.position, this.is2Finger) == base.gameObject)
			{
				result = true;
			}
			return result;
		}

		[SerializeField]
		public QuickLongTap.OnLongTap onLongTap;

		public QuickLongTap.ActionTriggering actionTriggering;

		private Gesture currentGesture;

		[Serializable]
		public class OnLongTap : UnityEvent<Gesture>
		{
		}

		public enum ActionTriggering
		{
			Start,
			InProgress,
			End
		}
	}
}
