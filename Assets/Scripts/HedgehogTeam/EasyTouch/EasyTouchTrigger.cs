using System;
using System.Collections.Generic;
using UnityEngine;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Trigger")]
	[Serializable]
	public class EasyTouchTrigger : MonoBehaviour
	{
		private void Start()
		{
			EasyTouch.SetEnableAutoSelect(true);
		}

		private void OnEnable()
		{
			this.SubscribeEasyTouchEvent();
		}

		private void OnDisable()
		{
			this.UnsubscribeEasyTouchEvent();
		}

		private void OnDestroy()
		{
			this.UnsubscribeEasyTouchEvent();
		}

		private void SubscribeEasyTouchEvent()
		{
			if (this.IsRecevier4(EasyTouch.EvtType.On_Cancel))
			{
				EasyTouch.On_Cancel += this.On_Cancel;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TouchStart))
			{
				EasyTouch.On_TouchStart += this.On_TouchStart;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TouchDown))
			{
				EasyTouch.On_TouchDown += this.On_TouchDown;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TouchUp))
			{
				EasyTouch.On_TouchUp += this.On_TouchUp;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_SimpleTap))
			{
				EasyTouch.On_SimpleTap += this.On_SimpleTap;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_LongTapStart))
			{
				EasyTouch.On_LongTapStart += this.On_LongTapStart;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_LongTap))
			{
				EasyTouch.On_LongTap += this.On_LongTap;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_LongTapEnd))
			{
				EasyTouch.On_LongTapEnd += this.On_LongTapEnd;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_DoubleTap))
			{
				EasyTouch.On_DoubleTap += this.On_DoubleTap;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_DragStart))
			{
				EasyTouch.On_DragStart += this.On_DragStart;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_Drag))
			{
				EasyTouch.On_Drag += this.On_Drag;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_DragEnd))
			{
				EasyTouch.On_DragEnd += this.On_DragEnd;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_SwipeStart))
			{
				EasyTouch.On_SwipeStart += this.On_SwipeStart;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_Swipe))
			{
				EasyTouch.On_Swipe += this.On_Swipe;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_SwipeEnd))
			{
				EasyTouch.On_SwipeEnd += this.On_SwipeEnd;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TouchStart2Fingers))
			{
				EasyTouch.On_TouchStart2Fingers += this.On_TouchStart2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TouchDown2Fingers))
			{
				EasyTouch.On_TouchDown2Fingers += this.On_TouchDown2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TouchUp2Fingers))
			{
				EasyTouch.On_TouchUp2Fingers += this.On_TouchUp2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_SimpleTap2Fingers))
			{
				EasyTouch.On_SimpleTap2Fingers += this.On_SimpleTap2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_LongTapStart2Fingers))
			{
				EasyTouch.On_LongTapStart2Fingers += this.On_LongTapStart2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_LongTap2Fingers))
			{
				EasyTouch.On_LongTap2Fingers += this.On_LongTap2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_LongTapEnd2Fingers))
			{
				EasyTouch.On_LongTapEnd2Fingers += this.On_LongTapEnd2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_DoubleTap2Fingers))
			{
				EasyTouch.On_DoubleTap2Fingers += this.On_DoubleTap2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_SwipeStart2Fingers))
			{
				EasyTouch.On_SwipeStart2Fingers += this.On_SwipeStart2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_Swipe2Fingers))
			{
				EasyTouch.On_Swipe2Fingers += this.On_Swipe2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_SwipeEnd2Fingers))
			{
				EasyTouch.On_SwipeEnd2Fingers += this.On_SwipeEnd2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_DragStart2Fingers))
			{
				EasyTouch.On_DragStart2Fingers += this.On_DragStart2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_Drag2Fingers))
			{
				EasyTouch.On_Drag2Fingers += this.On_Drag2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_DragEnd2Fingers))
			{
				EasyTouch.On_DragEnd2Fingers += this.On_DragEnd2Fingers;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_Pinch))
			{
				EasyTouch.On_Pinch += this.On_Pinch;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_PinchIn))
			{
				EasyTouch.On_PinchIn += this.On_PinchIn;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_PinchOut))
			{
				EasyTouch.On_PinchOut += this.On_PinchOut;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_PinchEnd))
			{
				EasyTouch.On_PinchEnd += this.On_PinchEnd;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_Twist))
			{
				EasyTouch.On_Twist += this.On_Twist;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_TwistEnd))
			{
				EasyTouch.On_TwistEnd += this.On_TwistEnd;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_OverUIElement))
			{
				EasyTouch.On_OverUIElement += this.On_OverUIElement;
			}
			if (this.IsRecevier4(EasyTouch.EvtType.On_UIElementTouchUp))
			{
				EasyTouch.On_UIElementTouchUp += this.On_UIElementTouchUp;
			}
		}

		private void UnsubscribeEasyTouchEvent()
		{
			EasyTouch.On_Cancel -= this.On_Cancel;
			EasyTouch.On_TouchStart -= this.On_TouchStart;
			EasyTouch.On_TouchDown -= this.On_TouchDown;
			EasyTouch.On_TouchUp -= this.On_TouchUp;
			EasyTouch.On_SimpleTap -= this.On_SimpleTap;
			EasyTouch.On_LongTapStart -= this.On_LongTapStart;
			EasyTouch.On_LongTap -= this.On_LongTap;
			EasyTouch.On_LongTapEnd -= this.On_LongTapEnd;
			EasyTouch.On_DoubleTap -= this.On_DoubleTap;
			EasyTouch.On_DragStart -= this.On_DragStart;
			EasyTouch.On_Drag -= this.On_Drag;
			EasyTouch.On_DragEnd -= this.On_DragEnd;
			EasyTouch.On_SwipeStart -= this.On_SwipeStart;
			EasyTouch.On_Swipe -= this.On_Swipe;
			EasyTouch.On_SwipeEnd -= this.On_SwipeEnd;
			EasyTouch.On_TouchStart2Fingers -= this.On_TouchStart2Fingers;
			EasyTouch.On_TouchDown2Fingers -= this.On_TouchDown2Fingers;
			EasyTouch.On_TouchUp2Fingers -= this.On_TouchUp2Fingers;
			EasyTouch.On_SimpleTap2Fingers -= this.On_SimpleTap2Fingers;
			EasyTouch.On_LongTapStart2Fingers -= this.On_LongTapStart2Fingers;
			EasyTouch.On_LongTap2Fingers -= this.On_LongTap2Fingers;
			EasyTouch.On_LongTapEnd2Fingers -= this.On_LongTapEnd2Fingers;
			EasyTouch.On_DoubleTap2Fingers -= this.On_DoubleTap2Fingers;
			EasyTouch.On_SwipeStart2Fingers -= this.On_SwipeStart2Fingers;
			EasyTouch.On_Swipe2Fingers -= this.On_Swipe2Fingers;
			EasyTouch.On_SwipeEnd2Fingers -= this.On_SwipeEnd2Fingers;
			EasyTouch.On_DragStart2Fingers -= this.On_DragStart2Fingers;
			EasyTouch.On_Drag2Fingers -= this.On_Drag2Fingers;
			EasyTouch.On_DragEnd2Fingers -= this.On_DragEnd2Fingers;
			EasyTouch.On_Pinch -= this.On_Pinch;
			EasyTouch.On_PinchIn -= this.On_PinchIn;
			EasyTouch.On_PinchOut -= this.On_PinchOut;
			EasyTouch.On_PinchEnd -= this.On_PinchEnd;
			EasyTouch.On_Twist -= this.On_Twist;
			EasyTouch.On_TwistEnd -= this.On_TwistEnd;
			EasyTouch.On_OverUIElement += this.On_OverUIElement;
			EasyTouch.On_UIElementTouchUp += this.On_UIElementTouchUp;
		}

		private void On_TouchStart(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TouchStart, gesture);
		}

		private void On_TouchDown(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TouchDown, gesture);
		}

		private void On_TouchUp(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TouchUp, gesture);
		}

		private void On_SimpleTap(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_SimpleTap, gesture);
		}

		private void On_DoubleTap(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_DoubleTap, gesture);
		}

		private void On_LongTapStart(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_LongTapStart, gesture);
		}

		private void On_LongTap(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_LongTap, gesture);
		}

		private void On_LongTapEnd(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_LongTapEnd, gesture);
		}

		private void On_SwipeStart(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_SwipeStart, gesture);
		}

		private void On_Swipe(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Swipe, gesture);
		}

		private void On_SwipeEnd(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_SwipeEnd, gesture);
		}

		private void On_DragStart(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_DragStart, gesture);
		}

		private void On_Drag(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Drag, gesture);
		}

		private void On_DragEnd(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_DragEnd, gesture);
		}

		private void On_Cancel(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Cancel, gesture);
		}

		private void On_TouchStart2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TouchStart2Fingers, gesture);
		}

		private void On_TouchDown2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TouchDown2Fingers, gesture);
		}

		private void On_TouchUp2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TouchUp2Fingers, gesture);
		}

		private void On_LongTapStart2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_LongTapStart2Fingers, gesture);
		}

		private void On_LongTap2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_LongTap2Fingers, gesture);
		}

		private void On_LongTapEnd2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_LongTapEnd2Fingers, gesture);
		}

		private void On_DragStart2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_DragStart2Fingers, gesture);
		}

		private void On_Drag2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Drag2Fingers, gesture);
		}

		private void On_DragEnd2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_DragEnd2Fingers, gesture);
		}

		private void On_SwipeStart2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_SwipeStart2Fingers, gesture);
		}

		private void On_Swipe2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Swipe2Fingers, gesture);
		}

		private void On_SwipeEnd2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_SwipeEnd2Fingers, gesture);
		}

		private void On_Twist(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Twist, gesture);
		}

		private void On_TwistEnd(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_TwistEnd, gesture);
		}

		private void On_Pinch(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_Pinch, gesture);
		}

		private void On_PinchOut(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_PinchOut, gesture);
		}

		private void On_PinchIn(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_PinchIn, gesture);
		}

		private void On_PinchEnd(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_PinchEnd, gesture);
		}

		private void On_SimpleTap2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_SimpleTap2Fingers, gesture);
		}

		private void On_DoubleTap2Fingers(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_DoubleTap2Fingers, gesture);
		}

		private void On_UIElementTouchUp(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_UIElementTouchUp, gesture);
		}

		private void On_OverUIElement(Gesture gesture)
		{
			this.TriggerScheduler(EasyTouch.EvtType.On_OverUIElement, gesture);
		}

		public void AddTrigger(EasyTouch.EvtType ev)
		{
			EasyTouchTrigger.EasyTouchReceiver easyTouchReceiver = new EasyTouchTrigger.EasyTouchReceiver();
			easyTouchReceiver.enable = true;
			easyTouchReceiver.restricted = true;
			easyTouchReceiver.eventName = ev;
			easyTouchReceiver.gameObject = null;
			easyTouchReceiver.otherReceiver = false;
			easyTouchReceiver.name = "New trigger";
			this.receivers.Add(easyTouchReceiver);
			if (Application.isPlaying)
			{
				this.UnsubscribeEasyTouchEvent();
				this.SubscribeEasyTouchEvent();
			}
		}

		public bool SetTriggerEnable(string triggerName, bool value)
		{
			EasyTouchTrigger.EasyTouchReceiver trigger = this.GetTrigger(triggerName);
			if (trigger != null)
			{
				trigger.enable = value;
				return true;
			}
			return false;
		}

		public bool GetTriggerEnable(string triggerName)
		{
			EasyTouchTrigger.EasyTouchReceiver trigger = this.GetTrigger(triggerName);
			return trigger != null && trigger.enable;
		}

		private void TriggerScheduler(EasyTouch.EvtType evnt, Gesture gesture)
		{
			foreach (EasyTouchTrigger.EasyTouchReceiver easyTouchReceiver in this.receivers)
			{
				if (easyTouchReceiver.enable && easyTouchReceiver.eventName == evnt && ((easyTouchReceiver.restricted && ((gesture.pickedObject == base.gameObject && easyTouchReceiver.triggerType == EasyTouchTrigger.ETTType.Object3D) || (gesture.pickedUIElement == base.gameObject && easyTouchReceiver.triggerType == EasyTouchTrigger.ETTType.UI))) || (!easyTouchReceiver.restricted && (easyTouchReceiver.gameObject == null || (easyTouchReceiver.gameObject == gesture.pickedObject && easyTouchReceiver.triggerType == EasyTouchTrigger.ETTType.Object3D) || (gesture.pickedUIElement == easyTouchReceiver.gameObject && easyTouchReceiver.triggerType == EasyTouchTrigger.ETTType.UI)))))
				{
					GameObject gameObject = base.gameObject;
					if (easyTouchReceiver.otherReceiver && easyTouchReceiver.gameObjectReceiver != null)
					{
						gameObject = easyTouchReceiver.gameObjectReceiver;
					}
					switch (easyTouchReceiver.parameter)
					{
					case EasyTouchTrigger.ETTParameter.None:
						gameObject.SendMessage(easyTouchReceiver.methodName, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Gesture:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Finger_Id:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.fingerIndex, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Touch_Count:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.touchCount, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Start_Position:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.startPosition, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Position:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.position, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Delta_Position:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.deltaPosition, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Swipe_Type:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.swipe, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Swipe_Length:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.swipeLength, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Swipe_Vector:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.swipeVector, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Delta_Pinch:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.deltaPinch, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.Twist_Anlge:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.twistAngle, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.ActionTime:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.actionTime, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.DeltaTime:
						gameObject.SendMessage(easyTouchReceiver.methodName, gesture.deltaTime, SendMessageOptions.DontRequireReceiver);
						break;
					case EasyTouchTrigger.ETTParameter.PickedObject:
						if (gesture.pickedObject != null)
						{
							gameObject.SendMessage(easyTouchReceiver.methodName, gesture.pickedObject, SendMessageOptions.DontRequireReceiver);
						}
						break;
					case EasyTouchTrigger.ETTParameter.PickedUIElement:
						if (gesture.pickedUIElement != null)
						{
							gameObject.SendMessage(easyTouchReceiver.methodName, gesture.pickedObject, SendMessageOptions.DontRequireReceiver);
						}
						break;
					}
				}
			}
		}

		private bool IsRecevier4(EasyTouch.EvtType evnt)
		{
			int num = this.receivers.FindIndex((EasyTouchTrigger.EasyTouchReceiver e) => e.eventName == evnt);
			return num > -1;
		}

		private EasyTouchTrigger.EasyTouchReceiver GetTrigger(string triggerName)
		{
			return this.receivers.Find((EasyTouchTrigger.EasyTouchReceiver n) => n.name == triggerName);
		}

		[SerializeField]
		public List<EasyTouchTrigger.EasyTouchReceiver> receivers = new List<EasyTouchTrigger.EasyTouchReceiver>();

		public enum ETTParameter
		{
			None,
			Gesture,
			Finger_Id,
			Touch_Count,
			Start_Position,
			Position,
			Delta_Position,
			Swipe_Type,
			Swipe_Length,
			Swipe_Vector,
			Delta_Pinch,
			Twist_Anlge,
			ActionTime,
			DeltaTime,
			PickedObject,
			PickedUIElement
		}

		public enum ETTType
		{
			Object3D,
			UI
		}

		[Serializable]
		public class EasyTouchReceiver
		{
			public bool enable;

			public EasyTouchTrigger.ETTType triggerType;

			public string name;

			public bool restricted;

			public GameObject gameObject;

			public bool otherReceiver;

			public GameObject gameObjectReceiver;

			public EasyTouch.EvtType eventName;

			public string methodName;

			public EasyTouchTrigger.ETTParameter parameter;
		}
	}
}
