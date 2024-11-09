using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Twist")]
	public class QuickTwist : QuickBase
	{
		public QuickTwist()
		{
			this.quickActionName = "QuickTwist" + base.GetInstanceID().ToString();
		}

		public override void OnEnable()
		{
			EasyTouch.On_Twist += this.On_Twist;
			EasyTouch.On_TwistEnd += this.On_TwistEnd;
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
			EasyTouch.On_Twist -= this.On_Twist;
			EasyTouch.On_TwistEnd -= this.On_TwistEnd;
		}

		private void On_Twist(Gesture gesture)
		{
			if (this.actionTriggering == QuickTwist.ActionTiggering.InProgress && this.IsRightRotation(gesture))
			{
				this.DoAction(gesture);
			}
		}

		private void On_TwistEnd(Gesture gesture)
		{
			if (this.actionTriggering == QuickTwist.ActionTiggering.End && this.IsRightRotation(gesture))
			{
				this.DoAction(gesture);
			}
		}

		private bool IsRightRotation(Gesture gesture)
		{
			this.axisActionValue = 0f;
			float num = 1f;
			if (this.inverseAxisValue)
			{
				num = -1f;
			}
			QuickTwist.ActionRotationDirection actionRotationDirection = this.rotationDirection;
			if (actionRotationDirection != QuickTwist.ActionRotationDirection.All)
			{
				if (actionRotationDirection != QuickTwist.ActionRotationDirection.Clockwise)
				{
					if (actionRotationDirection == QuickTwist.ActionRotationDirection.Counterclockwise)
					{
						if (gesture.twistAngle > 0f)
						{
							this.axisActionValue = gesture.twistAngle * this.sensibility * num;
							return true;
						}
					}
				}
				else if (gesture.twistAngle < 0f)
				{
					this.axisActionValue = gesture.twistAngle * this.sensibility * num;
					return true;
				}
				return false;
			}
			this.axisActionValue = gesture.twistAngle * this.sensibility * num;
			return true;
		}

		private void DoAction(Gesture gesture)
		{
			if (this.isGestureOnMe)
			{
				if (this.realType == QuickBase.GameObjectType.UI)
				{
					if (gesture.isOverGui && (gesture.pickedUIElement == base.gameObject || gesture.pickedUIElement.transform.IsChildOf(base.transform)))
					{
						this.onTwistAction.Invoke(gesture);
						if (this.enableSimpleAction)
						{
							base.DoDirectAction(this.axisActionValue);
						}
					}
				}
				else if (((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI) && gesture.GetCurrentPickedObject(true) == base.gameObject)
				{
					this.onTwistAction.Invoke(gesture);
					if (this.enableSimpleAction)
					{
						base.DoDirectAction(this.axisActionValue);
					}
				}
			}
			else if ((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI)
			{
				this.onTwistAction.Invoke(gesture);
				if (this.enableSimpleAction)
				{
					base.DoDirectAction(this.axisActionValue);
				}
			}
		}

		[SerializeField]
		public QuickTwist.OnTwistAction onTwistAction;

		public bool isGestureOnMe;

		public QuickTwist.ActionTiggering actionTriggering;

		public QuickTwist.ActionRotationDirection rotationDirection;

		private float axisActionValue;

		public bool enableSimpleAction;

		[Serializable]
		public class OnTwistAction : UnityEvent<Gesture>
		{
		}

		public enum ActionTiggering
		{
			InProgress,
			End
		}

		public enum ActionRotationDirection
		{
			All,
			Clockwise,
			Counterclockwise
		}
	}
}
