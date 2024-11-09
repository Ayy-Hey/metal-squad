using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Tap")]
	public class QuickTap : QuickBase
	{
		public QuickTap()
		{
			this.quickActionName = "QuickTap" + base.GetInstanceID().ToString();
		}

		private void Update()
		{
			this.currentGesture = EasyTouch.current;
			if (!this.is2Finger)
			{
				if (this.currentGesture.type == EasyTouch.EvtType.On_DoubleTap && this.actionTriggering == QuickTap.ActionTriggering.Double_Tap)
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_SimpleTap && this.actionTriggering == QuickTap.ActionTriggering.Simple_Tap)
				{
					this.DoAction(this.currentGesture);
				}
			}
			else
			{
				if (this.currentGesture.type == EasyTouch.EvtType.On_DoubleTap2Fingers && this.actionTriggering == QuickTap.ActionTriggering.Double_Tap)
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_SimpleTap2Fingers && this.actionTriggering == QuickTap.ActionTriggering.Simple_Tap)
				{
					this.DoAction(this.currentGesture);
				}
			}
		}

		private void DoAction(Gesture gesture)
		{
			if (this.realType == QuickBase.GameObjectType.UI)
			{
				if (gesture.isOverGui && (gesture.pickedUIElement == base.gameObject || gesture.pickedUIElement.transform.IsChildOf(base.transform)))
				{
					this.onTap.Invoke(gesture);
				}
			}
			else if (((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI) && EasyTouch.GetGameObjectAt(gesture.position, this.is2Finger) == base.gameObject)
			{
				this.onTap.Invoke(gesture);
			}
		}

		[SerializeField]
		public QuickTap.OnTap onTap;

		public QuickTap.ActionTriggering actionTriggering;

		private Gesture currentGesture;

		[Serializable]
		public class OnTap : UnityEvent<Gesture>
		{
		}

		public enum ActionTriggering
		{
			Simple_Tap,
			Double_Tap
		}
	}
}
