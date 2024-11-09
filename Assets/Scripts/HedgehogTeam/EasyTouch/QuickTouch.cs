using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Touch")]
	public class QuickTouch : QuickBase
	{
		public QuickTouch()
		{
			this.quickActionName = "QuickTouch" + base.GetInstanceID().ToString();
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
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchStart && this.actionTriggering == QuickTouch.ActionTriggering.Start && (this.currentGesture.fingerIndex == this.fingerIndex || this.isMultiTouch))
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchDown && this.actionTriggering == QuickTouch.ActionTriggering.Down && (this.currentGesture.fingerIndex == this.fingerIndex || this.isMultiTouch))
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchUp)
				{
					if (this.actionTriggering == QuickTouch.ActionTriggering.Up && (this.currentGesture.fingerIndex == this.fingerIndex || this.isMultiTouch))
					{
						if (this.IsOverMe(this.currentGesture))
						{
							this.onTouch.Invoke(this.currentGesture);
						}
						else
						{
							this.onTouchNotOverMe.Invoke(this.currentGesture);
						}
					}
					if (this.currentGesture.fingerIndex == this.fingerIndex)
					{
						this.fingerIndex = -1;
					}
				}
			}
			else
			{
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchStart2Fingers && this.actionTriggering == QuickTouch.ActionTriggering.Start)
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchDown2Fingers && this.actionTriggering == QuickTouch.ActionTriggering.Down)
				{
					this.DoAction(this.currentGesture);
				}
				if (this.currentGesture.type == EasyTouch.EvtType.On_TouchUp2Fingers && this.actionTriggering == QuickTouch.ActionTriggering.Up)
				{
					this.DoAction(this.currentGesture);
				}
			}
		}

		private void DoAction(Gesture gesture)
		{
			if (this.IsOverMe(gesture))
			{
				this.onTouch.Invoke(gesture);
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
		public QuickTouch.OnTouch onTouch;

		public QuickTouch.OnTouchNotOverMe onTouchNotOverMe;

		public QuickTouch.ActionTriggering actionTriggering;

		private Gesture currentGesture;

		[Serializable]
		public class OnTouch : UnityEvent<Gesture>
		{
		}

		[Serializable]
		public class OnTouchNotOverMe : UnityEvent<Gesture>
		{
		}

		public enum ActionTriggering
		{
			Start,
			Down,
			Up
		}
	}
}
