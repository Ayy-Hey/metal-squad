using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Swipe")]
	public class QuickSwipe : QuickBase
	{
		public QuickSwipe()
		{
			this.quickActionName = "QuickSwipe" + base.GetInstanceID().ToString();
		}

		public override void OnEnable()
		{
			base.OnEnable();
			EasyTouch.On_Drag += this.On_Drag;
			EasyTouch.On_Swipe += this.On_Swipe;
			EasyTouch.On_DragEnd += this.On_DragEnd;
			EasyTouch.On_SwipeEnd += this.On_SwipeEnd;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			this.UnsubscribeEvent();
		}

		private void OnDestroy()
		{
			this.UnsubscribeEvent();
		}

		private void UnsubscribeEvent()
		{
			EasyTouch.On_Swipe -= this.On_Swipe;
			EasyTouch.On_SwipeEnd -= this.On_SwipeEnd;
		}

		private void On_Swipe(Gesture gesture)
		{
			if (gesture.touchCount == 1 && ((gesture.pickedObject != base.gameObject && !this.allowSwipeStartOverMe) || this.allowSwipeStartOverMe))
			{
				this.fingerIndex = gesture.fingerIndex;
				if (this.actionTriggering == QuickSwipe.ActionTriggering.InProgress && this.isRightDirection(gesture))
				{
					this.onSwipeAction.Invoke(gesture);
					if (this.enableSimpleAction)
					{
						this.DoAction(gesture);
					}
				}
			}
		}

		private void On_SwipeEnd(Gesture gesture)
		{
			if (this.actionTriggering == QuickSwipe.ActionTriggering.End && this.isRightDirection(gesture))
			{
				this.onSwipeAction.Invoke(gesture);
				if (this.enableSimpleAction)
				{
					this.DoAction(gesture);
				}
			}
			if (this.fingerIndex == gesture.fingerIndex)
			{
				this.fingerIndex = -1;
			}
		}

		private void On_DragEnd(Gesture gesture)
		{
			if (gesture.pickedObject == base.gameObject && this.allowSwipeStartOverMe)
			{
				this.On_SwipeEnd(gesture);
			}
		}

		private void On_Drag(Gesture gesture)
		{
			if (gesture.pickedObject == base.gameObject && this.allowSwipeStartOverMe)
			{
				this.On_Swipe(gesture);
			}
		}

		private bool isRightDirection(Gesture gesture)
		{
			float num = -1f;
			if (this.inverseAxisValue)
			{
				num = 1f;
			}
			this.axisActionValue = 0f;
			switch (this.swipeDirection)
			{
			case QuickSwipe.SwipeDirection.Vertical:
				if (gesture.swipe == EasyTouch.SwipeDirection.Up || gesture.swipe == EasyTouch.SwipeDirection.Down)
				{
					this.axisActionValue = gesture.deltaPosition.y * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.Horizontal:
				if (gesture.swipe == EasyTouch.SwipeDirection.Left || gesture.swipe == EasyTouch.SwipeDirection.Right)
				{
					this.axisActionValue = gesture.deltaPosition.x * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.DiagonalRight:
				if (gesture.swipe == EasyTouch.SwipeDirection.UpRight || gesture.swipe == EasyTouch.SwipeDirection.DownLeft)
				{
					this.axisActionValue = gesture.deltaPosition.magnitude * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.DiagonalLeft:
				if (gesture.swipe == EasyTouch.SwipeDirection.UpLeft || gesture.swipe == EasyTouch.SwipeDirection.DownRight)
				{
					this.axisActionValue = gesture.deltaPosition.magnitude * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.Up:
				if (gesture.swipe == EasyTouch.SwipeDirection.Up)
				{
					this.axisActionValue = gesture.deltaPosition.y * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.UpRight:
				if (gesture.swipe == EasyTouch.SwipeDirection.UpRight)
				{
					this.axisActionValue = gesture.deltaPosition.magnitude * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.Right:
				if (gesture.swipe == EasyTouch.SwipeDirection.Right)
				{
					this.axisActionValue = gesture.deltaPosition.x * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.DownRight:
				if (gesture.swipe == EasyTouch.SwipeDirection.DownRight)
				{
					this.axisActionValue = gesture.deltaPosition.magnitude * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.Down:
				if (gesture.swipe == EasyTouch.SwipeDirection.Down)
				{
					this.axisActionValue = gesture.deltaPosition.y * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.DownLeft:
				if (gesture.swipe == EasyTouch.SwipeDirection.DownLeft)
				{
					this.axisActionValue = gesture.deltaPosition.magnitude * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.Left:
				if (gesture.swipe == EasyTouch.SwipeDirection.Left)
				{
					this.axisActionValue = gesture.deltaPosition.x * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.UpLeft:
				if (gesture.swipe == EasyTouch.SwipeDirection.UpLeft)
				{
					this.axisActionValue = gesture.deltaPosition.magnitude * num;
					return true;
				}
				break;
			case QuickSwipe.SwipeDirection.All:
				this.axisActionValue = gesture.deltaPosition.magnitude * num;
				return true;
			}
			this.axisActionValue = 0f;
			return false;
		}

		private void DoAction(Gesture gesture)
		{
			switch (this.directAction)
			{
			case QuickBase.DirectAction.Rotate:
			case QuickBase.DirectAction.RotateLocal:
				this.axisActionValue *= this.sensibility;
				break;
			case QuickBase.DirectAction.Translate:
			case QuickBase.DirectAction.TranslateLocal:
			case QuickBase.DirectAction.Scale:
				this.axisActionValue /= Screen.dpi;
				this.axisActionValue *= this.sensibility;
				break;
			}
			base.DoDirectAction(this.axisActionValue);
		}

		[SerializeField]
		public QuickSwipe.OnSwipeAction onSwipeAction;

		public bool allowSwipeStartOverMe = true;

		public QuickSwipe.ActionTriggering actionTriggering;

		public QuickSwipe.SwipeDirection swipeDirection = QuickSwipe.SwipeDirection.All;

		private float axisActionValue;

		public bool enableSimpleAction;

		[Serializable]
		public class OnSwipeAction : UnityEvent<Gesture>
		{
		}

		public enum ActionTriggering
		{
			InProgress,
			End
		}

		public enum SwipeDirection
		{
			Vertical,
			Horizontal,
			DiagonalRight,
			DiagonalLeft,
			Up,
			UpRight,
			Right,
			DownRight,
			Down,
			DownLeft,
			Left,
			UpLeft,
			All
		}
	}
}
