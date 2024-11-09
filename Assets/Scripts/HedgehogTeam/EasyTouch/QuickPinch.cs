using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Pinch")]
	public class QuickPinch : QuickBase
	{
		public QuickPinch()
		{
			this.quickActionName = "QuickPinch" + base.GetInstanceID().ToString();
		}

		public override void OnEnable()
		{
			EasyTouch.On_Pinch += this.On_Pinch;
			EasyTouch.On_PinchIn += this.On_PinchIn;
			EasyTouch.On_PinchOut += this.On_PinchOut;
			EasyTouch.On_PinchEnd += this.On_PichEnd;
		}

		public override void OnDisable()
		{
			this.UnsubscribeEvent();
		}

		private void OnDestroy()
		{
			this.UnsubscribeEvent();
		}

		private void UnsubscribeEvent()
		{
			EasyTouch.On_Pinch -= this.On_Pinch;
			EasyTouch.On_PinchIn -= this.On_PinchIn;
			EasyTouch.On_PinchOut -= this.On_PinchOut;
			EasyTouch.On_PinchEnd -= this.On_PichEnd;
		}

		private void On_Pinch(Gesture gesture)
		{
			if (this.actionTriggering == QuickPinch.ActionTiggering.InProgress && this.pinchDirection == QuickPinch.ActionPinchDirection.All)
			{
				this.DoAction(gesture);
			}
		}

		private void On_PinchIn(Gesture gesture)
		{
			if (this.actionTriggering == QuickPinch.ActionTiggering.InProgress & this.pinchDirection == QuickPinch.ActionPinchDirection.PinchIn)
			{
				this.DoAction(gesture);
			}
		}

		private void On_PinchOut(Gesture gesture)
		{
			if (this.actionTriggering == QuickPinch.ActionTiggering.InProgress & this.pinchDirection == QuickPinch.ActionPinchDirection.PinchOut)
			{
				this.DoAction(gesture);
			}
		}

		private void On_PichEnd(Gesture gesture)
		{
			if (this.actionTriggering == QuickPinch.ActionTiggering.End)
			{
				this.DoAction(gesture);
			}
		}

		private void DoAction(Gesture gesture)
		{
			this.axisActionValue = gesture.deltaPinch * this.sensibility * Time.deltaTime;
			if (this.isGestureOnMe)
			{
				if (this.realType == QuickBase.GameObjectType.UI)
				{
					if (gesture.isOverGui && (gesture.pickedUIElement == base.gameObject || gesture.pickedUIElement.transform.IsChildOf(base.transform)))
					{
						this.onPinchAction.Invoke(gesture);
						if (this.enableSimpleAction)
						{
							base.DoDirectAction(this.axisActionValue);
						}
					}
				}
				else if (((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI) && gesture.GetCurrentPickedObject(true) == base.gameObject)
				{
					this.onPinchAction.Invoke(gesture);
					if (this.enableSimpleAction)
					{
						base.DoDirectAction(this.axisActionValue);
					}
				}
			}
			else if ((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI)
			{
				this.onPinchAction.Invoke(gesture);
				if (this.enableSimpleAction)
				{
					base.DoDirectAction(this.axisActionValue);
				}
			}
		}

		[SerializeField]
		public QuickPinch.OnPinchAction onPinchAction;

		public bool isGestureOnMe;

		public QuickPinch.ActionTiggering actionTriggering;

		public QuickPinch.ActionPinchDirection pinchDirection;

		private float axisActionValue;

		public bool enableSimpleAction;

		[Serializable]
		public class OnPinchAction : UnityEvent<Gesture>
		{
		}

		public enum ActionTiggering
		{
			InProgress,
			End
		}

		public enum ActionPinchDirection
		{
			All,
			PinchIn,
			PinchOut
		}
	}
}
