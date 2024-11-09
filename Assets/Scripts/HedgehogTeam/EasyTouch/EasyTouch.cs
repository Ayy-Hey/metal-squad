using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HedgehogTeam.EasyTouch
{
	public class EasyTouch : MonoBehaviour
	{
		public EasyTouch()
		{
			this.enable = true;
			this.allowUIDetection = true;
			this.enableUIMode = true;
			this.autoUpdatePickedUI = false;
			this.enabledNGuiMode = false;
			this.nGUICameras = new List<Camera>();
			this.autoSelect = true;
			this.touchCameras = new List<ECamera>();
			this.pickableLayers3D = 1;
			this.enable2D = false;
			this.pickableLayers2D = 1;
			this.gesturePriority = EasyTouch.GesturePriority.Tap;
			this.StationaryTolerance = 15f;
			this.longTapTime = 1f;
			this.doubleTapDetection = EasyTouch.DoubleTapDetection.BySystem;
			this.doubleTapTime = 0.3f;
			this.swipeTolerance = 0.85f;
			this.alwaysSendSwipe = false;
			this.enable2FingersGesture = true;
			this.twoFingerPickMethod = EasyTouch.TwoFingerPickMethod.Finger;
			this.enable2FingersSwipe = true;
			this.enablePinch = true;
			this.minPinchLength = 0f;
			this.enableTwist = true;
			this.minTwistAngle = 0f;
			this.enableSimulation = true;
			this.twistKey = KeyCode.LeftAlt;
			this.swipeKey = KeyCode.LeftControl;
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchCancelHandler On_Cancel;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.Cancel2FingersHandler On_Cancel2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchStartHandler On_TouchStart;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchDownHandler On_TouchDown;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchUpHandler On_TouchUp;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SimpleTapHandler On_SimpleTap;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DoubleTapHandler On_DoubleTap;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.LongTapStartHandler On_LongTapStart;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.LongTapHandler On_LongTap;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.LongTapEndHandler On_LongTapEnd;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DragStartHandler On_DragStart;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DragHandler On_Drag;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DragEndHandler On_DragEnd;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SwipeStartHandler On_SwipeStart;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SwipeHandler On_Swipe;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SwipeEndHandler On_SwipeEnd;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchStart2FingersHandler On_TouchStart2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchDown2FingersHandler On_TouchDown2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TouchUp2FingersHandler On_TouchUp2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SimpleTap2FingersHandler On_SimpleTap2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DoubleTap2FingersHandler On_DoubleTap2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.LongTapStart2FingersHandler On_LongTapStart2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.LongTap2FingersHandler On_LongTap2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.LongTapEnd2FingersHandler On_LongTapEnd2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TwistHandler On_Twist;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.TwistEndHandler On_TwistEnd;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.PinchHandler On_Pinch;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.PinchInHandler On_PinchIn;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.PinchOutHandler On_PinchOut;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.PinchEndHandler On_PinchEnd;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DragStart2FingersHandler On_DragStart2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.Drag2FingersHandler On_Drag2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.DragEnd2FingersHandler On_DragEnd2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SwipeStart2FingersHandler On_SwipeStart2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.Swipe2FingersHandler On_Swipe2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.SwipeEnd2FingersHandler On_SwipeEnd2Fingers;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.EasyTouchIsReadyHandler On_EasyTouchIsReady;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.OverUIElementHandler On_OverUIElement;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EasyTouch.UIElementTouchUpHandler On_UIElementTouchUp;

		public static EasyTouch instance
		{
			get
			{
				if (!EasyTouch._instance)
				{
					EasyTouch._instance = (UnityEngine.Object.FindObjectOfType(typeof(EasyTouch)) as EasyTouch);
					if (!EasyTouch._instance)
					{
						GameObject gameObject = new GameObject("Easytouch");
						EasyTouch._instance = gameObject.AddComponent<EasyTouch>();
					}
				}
				return EasyTouch._instance;
			}
		}

		public static Gesture current
		{
			get
			{
				return EasyTouch.instance._currentGesture;
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying && Application.isEditor)
			{
				this.Init();
			}
		}

		private void Awake()
		{
			this.Init();
		}

		private void Start()
		{
			for (int i = 0; i < 100; i++)
			{
				this.singleDoubleTap[i] = new EasyTouch.DoubleTap();
			}
			int num = this.touchCameras.FindIndex((ECamera c) => c.camera == Camera.main);
			if (num < 0)
			{
				this.touchCameras.Add(new ECamera(Camera.main, false));
			}
			if (EasyTouch.On_EasyTouchIsReady != null)
			{
				EasyTouch.On_EasyTouchIsReady();
			}
			this._currentGestures.Add(new Gesture());
		}

		private void Init()
		{
		}

		private void OnDrawGizmos()
		{
		}

		private void Update()
		{
			if (this.enable && EasyTouch.instance == this)
			{
				if (Application.isPlaying && UnityEngine.Input.touchCount > 0)
				{
					this.enableRemote = true;
				}
				if (Application.isPlaying && UnityEngine.Input.touchCount == 0)
				{
					this.enableRemote = false;
				}
				int num = this.input.TouchCount();
				if (this.oldTouchCount == 2 && num != 2 && num > 0)
				{
					this.CreateGesture2Finger(EasyTouch.EvtType.On_Cancel2Fingers, Vector2.zero, Vector2.zero, Vector2.zero, 0f, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, 0f);
				}
				this.UpdateTouches(true, num);
				this.twoFinger.oldPickedObject = this.twoFinger.pickedObject;
				if (this.enable2FingersGesture && num == 2)
				{
					this.TwoFinger();
				}
				for (int i = 0; i < 100; i++)
				{
					if (this.fingers[i] != null)
					{
						this.OneFinger(i);
					}
				}
				this.oldTouchCount = num;
			}
		}

		private void LateUpdate()
		{
			if (this._currentGestures.Count > 1)
			{
				this._currentGestures.RemoveAt(0);
			}
			else
			{
				this._currentGestures[0] = new Gesture();
			}
			this._currentGesture = this._currentGestures[0];
		}

		private void UpdateTouches(bool realTouch, int touchCount)
		{
			this.fingers.CopyTo(this.tmpArray, 0);
			if (realTouch || this.enableRemote)
			{
				this.ResetTouches();
				for (int i = 0; i < touchCount; i++)
				{
					Touch touch = UnityEngine.Input.GetTouch(i);
					int num = 0;
					while (num < 100 && this.fingers[i] == null)
					{
						if (this.tmpArray[num] != null && this.tmpArray[num].fingerIndex == touch.fingerId)
						{
							this.fingers[i] = this.tmpArray[num];
						}
						num++;
					}
					if (this.fingers[i] == null)
					{
						this.fingers[i] = new Finger();
						this.fingers[i].fingerIndex = touch.fingerId;
						this.fingers[i].gesture = EasyTouch.GestureType.None;
						this.fingers[i].phase = TouchPhase.Began;
					}
					else
					{
						this.fingers[i].phase = touch.phase;
					}
					if (this.fingers[i].phase != TouchPhase.Began)
					{
						this.fingers[i].deltaPosition = touch.position - this.fingers[i].position;
					}
					else
					{
						this.fingers[i].deltaPosition = Vector2.zero;
					}
					this.fingers[i].position = touch.position;
					this.fingers[i].tapCount = touch.tapCount;
					this.fingers[i].deltaTime = touch.deltaTime;
					this.fingers[i].touchCount = touchCount;
				}
			}
			else
			{
				for (int j = 0; j < touchCount; j++)
				{
					this.fingers[j] = this.input.GetMouseTouch(j, this.fingers[j]);
					this.fingers[j].touchCount = touchCount;
				}
			}
		}

		private void ResetTouches()
		{
			for (int i = 0; i < 100; i++)
			{
				this.fingers[i] = null;
			}
		}

		private void OneFinger(int fingerIndex)
		{
			if (this.fingers[fingerIndex].gesture == EasyTouch.GestureType.None)
			{
				if (!this.singleDoubleTap[fingerIndex].inDoubleTap)
				{
					this.singleDoubleTap[fingerIndex].inDoubleTap = true;
					this.singleDoubleTap[fingerIndex].time = 0f;
					this.singleDoubleTap[fingerIndex].count = 1;
				}
				this.fingers[fingerIndex].startTimeAction = Time.realtimeSinceStartup;
				this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Acquisition;
				this.fingers[fingerIndex].startPosition = this.fingers[fingerIndex].position;
				if (this.autoSelect && this.GetPickedGameObject(this.fingers[fingerIndex], false))
				{
					this.fingers[fingerIndex].pickedObject = this.pickedObject.pickedObj;
					this.fingers[fingerIndex].isGuiCamera = this.pickedObject.isGUI;
					this.fingers[fingerIndex].pickedCamera = this.pickedObject.pickedCamera;
				}
				if (this.allowUIDetection)
				{
					this.fingers[fingerIndex].isOverGui = this.IsScreenPositionOverUI(this.fingers[fingerIndex].position);
					this.fingers[fingerIndex].pickedUIElement = this.GetFirstUIElementFromCache();
				}
				this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_TouchStart, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
			}
			if (this.singleDoubleTap[fingerIndex].inDoubleTap)
			{
				this.singleDoubleTap[fingerIndex].time += Time.deltaTime;
			}
			this.fingers[fingerIndex].actionTime = Time.realtimeSinceStartup - this.fingers[fingerIndex].startTimeAction;
			if (this.fingers[fingerIndex].phase == TouchPhase.Canceled)
			{
				this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Cancel;
			}
			if (this.fingers[fingerIndex].phase != TouchPhase.Ended && this.fingers[fingerIndex].phase != TouchPhase.Canceled)
			{
				if (this.fingers[fingerIndex].phase == TouchPhase.Stationary && this.fingers[fingerIndex].actionTime >= this.longTapTime && this.fingers[fingerIndex].gesture == EasyTouch.GestureType.Acquisition)
				{
					this.fingers[fingerIndex].gesture = EasyTouch.GestureType.LongTap;
					this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_LongTapStart, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
				}
				if (((this.fingers[fingerIndex].gesture == EasyTouch.GestureType.Acquisition || this.fingers[fingerIndex].gesture == EasyTouch.GestureType.LongTap) && this.fingers[fingerIndex].phase == TouchPhase.Moved && this.gesturePriority == EasyTouch.GesturePriority.Slips) || ((this.fingers[fingerIndex].gesture == EasyTouch.GestureType.Acquisition || this.fingers[fingerIndex].gesture == EasyTouch.GestureType.LongTap) && !this.FingerInTolerance(this.fingers[fingerIndex]) && this.gesturePriority == EasyTouch.GesturePriority.Tap))
				{
					if (this.fingers[fingerIndex].gesture == EasyTouch.GestureType.LongTap)
					{
						this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Cancel;
						this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_LongTapEnd, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
						this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Acquisition;
					}
					else
					{
						this.fingers[fingerIndex].oldSwipeType = EasyTouch.SwipeDirection.None;
						if (this.fingers[fingerIndex].pickedObject)
						{
							this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Drag;
							this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_DragStart, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
							if (this.alwaysSendSwipe)
							{
								this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_SwipeStart, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
							}
						}
						else
						{
							this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Swipe;
							this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_SwipeStart, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
						}
					}
				}
				EasyTouch.EvtType evtType = EasyTouch.EvtType.None;
				EasyTouch.GestureType gesture = this.fingers[fingerIndex].gesture;
				if (gesture != EasyTouch.GestureType.LongTap)
				{
					if (gesture != EasyTouch.GestureType.Drag)
					{
						if (gesture == EasyTouch.GestureType.Swipe)
						{
							evtType = EasyTouch.EvtType.On_Swipe;
						}
					}
					else
					{
						evtType = EasyTouch.EvtType.On_Drag;
					}
				}
				else
				{
					evtType = EasyTouch.EvtType.On_LongTap;
				}
				EasyTouch.SwipeDirection swipe = this.GetSwipe(new Vector2(0f, 0f), this.fingers[fingerIndex].deltaPosition);
				if (evtType != EasyTouch.EvtType.None)
				{
					this.fingers[fingerIndex].oldSwipeType = swipe;
					this.CreateGesture(fingerIndex, evtType, this.fingers[fingerIndex], swipe, 0f, this.fingers[fingerIndex].deltaPosition);
					if (evtType == EasyTouch.EvtType.On_Drag && this.alwaysSendSwipe)
					{
						this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_Swipe, this.fingers[fingerIndex], swipe, 0f, this.fingers[fingerIndex].deltaPosition);
					}
				}
				this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_TouchDown, this.fingers[fingerIndex], swipe, 0f, this.fingers[fingerIndex].deltaPosition);
			}
			else
			{
				switch (this.fingers[fingerIndex].gesture)
				{
				case EasyTouch.GestureType.Drag:
					this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_DragEnd, this.fingers[fingerIndex], this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].startPosition - this.fingers[fingerIndex].position).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
					if (this.alwaysSendSwipe)
					{
						this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_SwipeEnd, this.fingers[fingerIndex], this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
					}
					break;
				case EasyTouch.GestureType.Swipe:
					this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_SwipeEnd, this.fingers[fingerIndex], this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
					break;
				case EasyTouch.GestureType.LongTap:
					this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_LongTapEnd, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
					break;
				case EasyTouch.GestureType.Cancel:
					this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_Cancel, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
					break;
				case EasyTouch.GestureType.Acquisition:
					if (this.doubleTapDetection == EasyTouch.DoubleTapDetection.BySystem)
					{
						if (this.FingerInTolerance(this.fingers[fingerIndex]))
						{
							if (this.fingers[fingerIndex].tapCount < 2)
							{
								this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_SimpleTap, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
							}
							else
							{
								this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_DoubleTap, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
							}
						}
					}
					else if (!this.singleDoubleTap[fingerIndex].inWait)
					{
						this.singleDoubleTap[fingerIndex].finger = this.fingers[fingerIndex];
						base.StartCoroutine(this.SingleOrDouble(fingerIndex));
					}
					else
					{
						this.singleDoubleTap[fingerIndex].count++;
					}
					break;
				}
				this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_TouchUp, this.fingers[fingerIndex], EasyTouch.SwipeDirection.None, 0f, Vector2.zero);
				this.fingers[fingerIndex] = null;
			}
		}

		private IEnumerator SingleOrDouble(int fingerIndex)
		{
			this.singleDoubleTap[fingerIndex].inWait = true;
			float time2Wait = this.doubleTapTime - this.singleDoubleTap[fingerIndex].finger.actionTime;
			if (time2Wait < 0f)
			{
				time2Wait = 0f;
			}
			yield return new WaitForSeconds(time2Wait);
			if (this.singleDoubleTap[fingerIndex].count < 2)
			{
				this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_SimpleTap, this.singleDoubleTap[fingerIndex].finger, EasyTouch.SwipeDirection.None, 0f, this.singleDoubleTap[fingerIndex].finger.deltaPosition);
			}
			else
			{
				this.CreateGesture(fingerIndex, EasyTouch.EvtType.On_DoubleTap, this.singleDoubleTap[fingerIndex].finger, EasyTouch.SwipeDirection.None, 0f, this.singleDoubleTap[fingerIndex].finger.deltaPosition);
			}
			this.singleDoubleTap[fingerIndex].Stop();
			base.StopCoroutine("SingleOrDouble");
			yield break;
		}

		private void CreateGesture(int touchIndex, EasyTouch.EvtType message, Finger finger, EasyTouch.SwipeDirection swipe, float swipeLength, Vector2 swipeVector)
		{
			bool flag = true;
			if (this.autoUpdatePickedUI && this.allowUIDetection)
			{
				finger.isOverGui = this.IsScreenPositionOverUI(finger.position);
				finger.pickedUIElement = this.GetFirstUIElementFromCache();
			}
			if (this.enabledNGuiMode && message == EasyTouch.EvtType.On_TouchStart)
			{
				finger.isOverGui = (finger.isOverGui || this.IsTouchOverNGui(finger.position, false));
			}
			if (this.enableUIMode || this.enabledNGuiMode)
			{
				flag = !finger.isOverGui;
			}
			Gesture gesture = finger.GetGesture();
			if (this.autoUpdatePickedObject && this.autoSelect && message != EasyTouch.EvtType.On_Drag && message != EasyTouch.EvtType.On_DragEnd && message != EasyTouch.EvtType.On_DragStart)
			{
				if (this.GetPickedGameObject(finger, false))
				{
					gesture.pickedObject = this.pickedObject.pickedObj;
					gesture.pickedCamera = this.pickedObject.pickedCamera;
					gesture.isGuiCamera = this.pickedObject.isGUI;
				}
				else
				{
					gesture.pickedObject = null;
					gesture.pickedCamera = null;
					gesture.isGuiCamera = false;
				}
			}
			gesture.swipe = swipe;
			gesture.swipeLength = swipeLength;
			gesture.swipeVector = swipeVector;
			gesture.deltaPinch = 0f;
			gesture.twistAngle = 0f;
			if (flag)
			{
				this.RaiseEvent(message, gesture);
			}
			else if (finger.isOverGui)
			{
				if (message == EasyTouch.EvtType.On_TouchUp)
				{
					this.RaiseEvent(EasyTouch.EvtType.On_UIElementTouchUp, gesture);
				}
				else
				{
					this.RaiseEvent(EasyTouch.EvtType.On_OverUIElement, gesture);
				}
			}
		}

		private void TwoFinger()
		{
			bool flag = false;
			if (this.twoFinger.currentGesture == EasyTouch.GestureType.None)
			{
				if (!this.singleDoubleTap[99].inDoubleTap)
				{
					this.singleDoubleTap[99].inDoubleTap = true;
					this.singleDoubleTap[99].time = 0f;
					this.singleDoubleTap[99].count = 1;
				}
				this.twoFinger.finger0 = this.GetTwoFinger(-1);
				this.twoFinger.finger1 = this.GetTwoFinger(this.twoFinger.finger0);
				this.twoFinger.startTimeAction = Time.realtimeSinceStartup;
				this.twoFinger.currentGesture = EasyTouch.GestureType.Acquisition;
				this.fingers[this.twoFinger.finger0].startPosition = this.fingers[this.twoFinger.finger0].position;
				this.fingers[this.twoFinger.finger1].startPosition = this.fingers[this.twoFinger.finger1].position;
				this.fingers[this.twoFinger.finger0].oldPosition = this.fingers[this.twoFinger.finger0].position;
				this.fingers[this.twoFinger.finger1].oldPosition = this.fingers[this.twoFinger.finger1].position;
				this.twoFinger.oldFingerDistance = Mathf.Abs(Vector2.Distance(this.fingers[this.twoFinger.finger0].position, this.fingers[this.twoFinger.finger1].position));
				this.twoFinger.startPosition = new Vector2((this.fingers[this.twoFinger.finger0].position.x + this.fingers[this.twoFinger.finger1].position.x) / 2f, (this.fingers[this.twoFinger.finger0].position.y + this.fingers[this.twoFinger.finger1].position.y) / 2f);
				this.twoFinger.position = this.twoFinger.startPosition;
				this.twoFinger.oldStartPosition = this.twoFinger.startPosition;
				this.twoFinger.deltaPosition = Vector2.zero;
				this.twoFinger.startDistance = this.twoFinger.oldFingerDistance;
				if (this.autoSelect)
				{
					if (this.GetTwoFingerPickedObject())
					{
						this.twoFinger.pickedObject = this.pickedObject.pickedObj;
						this.twoFinger.pickedCamera = this.pickedObject.pickedCamera;
						this.twoFinger.isGuiCamera = this.pickedObject.isGUI;
					}
					else
					{
						this.twoFinger.ClearPickedObjectData();
					}
				}
				if (this.allowUIDetection)
				{
					if (this.GetTwoFingerPickedUIElement())
					{
						this.twoFinger.pickedUIElement = this.pickedObject.pickedObj;
						this.twoFinger.isOverGui = true;
					}
					else
					{
						this.twoFinger.ClearPickedUIData();
					}
				}
				this.CreateGesture2Finger(EasyTouch.EvtType.On_TouchStart2Fingers, this.twoFinger.startPosition, this.twoFinger.startPosition, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.oldFingerDistance);
			}
			if (this.singleDoubleTap[99].inDoubleTap)
			{
				this.singleDoubleTap[99].time += Time.deltaTime;
			}
			this.twoFinger.timeSinceStartAction = Time.realtimeSinceStartup - this.twoFinger.startTimeAction;
			this.twoFinger.position = new Vector2((this.fingers[this.twoFinger.finger0].position.x + this.fingers[this.twoFinger.finger1].position.x) / 2f, (this.fingers[this.twoFinger.finger0].position.y + this.fingers[this.twoFinger.finger1].position.y) / 2f);
			this.twoFinger.deltaPosition = this.twoFinger.position - this.twoFinger.oldStartPosition;
			this.twoFinger.fingerDistance = Mathf.Abs(Vector2.Distance(this.fingers[this.twoFinger.finger0].position, this.fingers[this.twoFinger.finger1].position));
			if (this.fingers[this.twoFinger.finger0].phase == TouchPhase.Canceled || this.fingers[this.twoFinger.finger1].phase == TouchPhase.Canceled)
			{
				this.twoFinger.currentGesture = EasyTouch.GestureType.Cancel;
			}
			if (this.fingers[this.twoFinger.finger0].phase != TouchPhase.Ended && this.fingers[this.twoFinger.finger1].phase != TouchPhase.Ended && this.twoFinger.currentGesture != EasyTouch.GestureType.Cancel)
			{
				if (this.twoFinger.currentGesture == EasyTouch.GestureType.Acquisition && this.twoFinger.timeSinceStartAction >= this.longTapTime && this.FingerInTolerance(this.fingers[this.twoFinger.finger0]) && this.FingerInTolerance(this.fingers[this.twoFinger.finger1]))
				{
					this.twoFinger.currentGesture = EasyTouch.GestureType.LongTap;
					this.CreateGesture2Finger(EasyTouch.EvtType.On_LongTapStart2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
				}
				if (((!this.FingerInTolerance(this.fingers[this.twoFinger.finger0]) || !this.FingerInTolerance(this.fingers[this.twoFinger.finger1])) && this.gesturePriority == EasyTouch.GesturePriority.Tap) || ((this.fingers[this.twoFinger.finger0].phase == TouchPhase.Moved || this.fingers[this.twoFinger.finger1].phase == TouchPhase.Moved) && this.gesturePriority == EasyTouch.GesturePriority.Slips))
				{
					flag = true;
				}
				if (flag && this.twoFinger.currentGesture != EasyTouch.GestureType.Tap)
				{
					Vector2 currentDistance = this.fingers[this.twoFinger.finger0].position - this.fingers[this.twoFinger.finger1].position;
					Vector2 previousDistance = this.fingers[this.twoFinger.finger0].oldPosition - this.fingers[this.twoFinger.finger1].oldPosition;
					float currentDelta = currentDistance.magnitude - previousDistance.magnitude;
					if (this.enable2FingersSwipe)
					{
						float num = Vector2.Dot(this.fingers[this.twoFinger.finger0].deltaPosition.normalized, this.fingers[this.twoFinger.finger1].deltaPosition.normalized);
						if (num > 0f)
						{
							if (this.twoFinger.oldGesture == EasyTouch.GestureType.LongTap)
							{
								this.CreateStateEnd2Fingers(this.twoFinger.currentGesture, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, false, this.twoFinger.fingerDistance, 0f, 0f);
								this.twoFinger.startTimeAction = Time.realtimeSinceStartup;
							}
							if (this.twoFinger.pickedObject && !this.twoFinger.dragStart && !this.alwaysSendSwipe)
							{
								this.twoFinger.currentGesture = EasyTouch.GestureType.Drag;
								this.CreateGesture2Finger(EasyTouch.EvtType.On_DragStart2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
								this.CreateGesture2Finger(EasyTouch.EvtType.On_SwipeStart2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
								this.twoFinger.dragStart = true;
							}
							else if (!this.twoFinger.pickedObject && !this.twoFinger.swipeStart)
							{
								this.twoFinger.currentGesture = EasyTouch.GestureType.Swipe;
								this.CreateGesture2Finger(EasyTouch.EvtType.On_SwipeStart2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
								this.twoFinger.swipeStart = true;
							}
						}
						else if (num < 0f)
						{
							this.twoFinger.dragStart = false;
							this.twoFinger.swipeStart = false;
						}
						if (this.twoFinger.dragStart)
						{
							this.CreateGesture2Finger(EasyTouch.EvtType.On_Drag2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.oldStartPosition, this.twoFinger.position), 0f, this.twoFinger.deltaPosition, 0f, 0f, this.twoFinger.fingerDistance);
							this.CreateGesture2Finger(EasyTouch.EvtType.On_Swipe2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.oldStartPosition, this.twoFinger.position), 0f, this.twoFinger.deltaPosition, 0f, 0f, this.twoFinger.fingerDistance);
						}
						if (this.twoFinger.swipeStart)
						{
							this.CreateGesture2Finger(EasyTouch.EvtType.On_Swipe2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.oldStartPosition, this.twoFinger.position), 0f, this.twoFinger.deltaPosition, 0f, 0f, this.twoFinger.fingerDistance);
						}
					}
					this.DetectPinch(currentDelta);
					this.DetecTwist(previousDistance, currentDistance, currentDelta);
				}
				else if (this.twoFinger.currentGesture == EasyTouch.GestureType.LongTap)
				{
					this.CreateGesture2Finger(EasyTouch.EvtType.On_LongTap2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
				}
				this.CreateGesture2Finger(EasyTouch.EvtType.On_TouchDown2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.oldStartPosition, this.twoFinger.position), 0f, this.twoFinger.deltaPosition, 0f, 0f, this.twoFinger.fingerDistance);
				this.fingers[this.twoFinger.finger0].oldPosition = this.fingers[this.twoFinger.finger0].position;
				this.fingers[this.twoFinger.finger1].oldPosition = this.fingers[this.twoFinger.finger1].position;
				this.twoFinger.oldFingerDistance = this.twoFinger.fingerDistance;
				this.twoFinger.oldStartPosition = this.twoFinger.position;
				this.twoFinger.oldGesture = this.twoFinger.currentGesture;
			}
			else if (this.twoFinger.currentGesture != EasyTouch.GestureType.Acquisition && this.twoFinger.currentGesture != EasyTouch.GestureType.Tap)
			{
				this.CreateStateEnd2Fingers(this.twoFinger.currentGesture, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, true, this.twoFinger.fingerDistance, 0f, 0f);
				this.twoFinger.currentGesture = EasyTouch.GestureType.None;
				this.twoFinger.pickedObject = null;
				this.twoFinger.swipeStart = false;
				this.twoFinger.dragStart = false;
			}
			else
			{
				this.twoFinger.currentGesture = EasyTouch.GestureType.Tap;
				this.CreateStateEnd2Fingers(this.twoFinger.currentGesture, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, true, this.twoFinger.fingerDistance, 0f, 0f);
			}
		}

		private void DetectPinch(float currentDelta)
		{
			if (this.enablePinch)
			{
				if ((Mathf.Abs(this.twoFinger.fingerDistance - this.twoFinger.startDistance) >= this.minPinchLength && this.twoFinger.currentGesture != EasyTouch.GestureType.Pinch) || this.twoFinger.currentGesture == EasyTouch.GestureType.Pinch)
				{
					if (currentDelta != 0f && this.twoFinger.oldGesture == EasyTouch.GestureType.LongTap)
					{
						this.CreateStateEnd2Fingers(this.twoFinger.currentGesture, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, false, this.twoFinger.fingerDistance, 0f, 0f);
						this.twoFinger.startTimeAction = Time.realtimeSinceStartup;
					}
					this.twoFinger.currentGesture = EasyTouch.GestureType.Pinch;
					if (currentDelta > 0f)
					{
						this.CreateGesture2Finger(EasyTouch.EvtType.On_PinchOut, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.startPosition, this.twoFinger.position), 0f, Vector2.zero, 0f, Mathf.Abs(this.twoFinger.fingerDistance - this.twoFinger.oldFingerDistance), this.twoFinger.fingerDistance);
					}
					if (currentDelta < 0f)
					{
						this.CreateGesture2Finger(EasyTouch.EvtType.On_PinchIn, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.startPosition, this.twoFinger.position), 0f, Vector2.zero, 0f, Mathf.Abs(this.twoFinger.fingerDistance - this.twoFinger.oldFingerDistance), this.twoFinger.fingerDistance);
					}
					if (currentDelta < 0f || currentDelta > 0f)
					{
						this.CreateGesture2Finger(EasyTouch.EvtType.On_Pinch, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, this.GetSwipe(this.twoFinger.startPosition, this.twoFinger.position), 0f, Vector2.zero, 0f, currentDelta, this.twoFinger.fingerDistance);
					}
				}
				this.twoFinger.lastPinch = ((currentDelta <= 0f) ? this.twoFinger.lastPinch : currentDelta);
			}
		}

		private void DetecTwist(Vector2 previousDistance, Vector2 currentDistance, float currentDelta)
		{
			if (this.enableTwist)
			{
				float num = Vector2.Angle(previousDistance, currentDistance);
				if (previousDistance == currentDistance)
				{
					num = 0f;
				}
				if ((Mathf.Abs(num) >= this.minTwistAngle && this.twoFinger.currentGesture != EasyTouch.GestureType.Twist) || this.twoFinger.currentGesture == EasyTouch.GestureType.Twist)
				{
					if (this.twoFinger.oldGesture == EasyTouch.GestureType.LongTap)
					{
						this.CreateStateEnd2Fingers(this.twoFinger.currentGesture, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, false, this.twoFinger.fingerDistance, 0f, 0f);
						this.twoFinger.startTimeAction = Time.realtimeSinceStartup;
					}
					this.twoFinger.currentGesture = EasyTouch.GestureType.Twist;
					if (num != 0f)
					{
						num *= Mathf.Sign(Vector3.Cross(previousDistance, currentDistance).z);
					}
					this.CreateGesture2Finger(EasyTouch.EvtType.On_Twist, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, num, 0f, this.twoFinger.fingerDistance);
				}
				this.twoFinger.lastTwistAngle = ((num == 0f) ? this.twoFinger.lastTwistAngle : num);
			}
		}

		private void CreateStateEnd2Fingers(EasyTouch.GestureType gesture, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float time, bool realEnd, float fingerDistance, float twist = 0f, float pinch = 0f)
		{
			switch (gesture)
			{
			case EasyTouch.GestureType.LongTap:
				this.CreateGesture2Finger(EasyTouch.EvtType.On_LongTapEnd2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
				goto IL_1D2;
			case EasyTouch.GestureType.Pinch:
				this.CreateGesture2Finger(EasyTouch.EvtType.On_PinchEnd, startPosition, position, deltaPosition, time, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, this.twoFinger.lastPinch, fingerDistance);
				goto IL_1D2;
			case EasyTouch.GestureType.Twist:
				this.CreateGesture2Finger(EasyTouch.EvtType.On_TwistEnd, startPosition, position, deltaPosition, time, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, this.twoFinger.lastTwistAngle, 0f, fingerDistance);
				goto IL_1D2;
			default:
				if (gesture != EasyTouch.GestureType.Tap)
				{
					goto IL_1D2;
				}
				break;
			case EasyTouch.GestureType.Acquisition:
				break;
			}
			if (this.doubleTapDetection == EasyTouch.DoubleTapDetection.BySystem)
			{
				if (this.fingers[this.twoFinger.finger0].tapCount < 2 && this.fingers[this.twoFinger.finger1].tapCount < 2)
				{
					this.CreateGesture2Finger(EasyTouch.EvtType.On_SimpleTap2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
				}
				else
				{
					this.CreateGesture2Finger(EasyTouch.EvtType.On_DoubleTap2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
				}
				this.twoFinger.currentGesture = EasyTouch.GestureType.None;
				this.twoFinger.pickedObject = null;
				this.twoFinger.swipeStart = false;
				this.twoFinger.dragStart = false;
				this.singleDoubleTap[99].Stop();
				base.StopCoroutine("SingleOrDouble2Fingers");
			}
			else if (!this.singleDoubleTap[99].inWait)
			{
				base.StartCoroutine("SingleOrDouble2Fingers");
			}
			else
			{
				this.singleDoubleTap[99].count++;
			}
			IL_1D2:
			if (realEnd)
			{
				if (this.twoFinger.dragStart)
				{
					this.CreateGesture2Finger(EasyTouch.EvtType.On_DragEnd2Fingers, startPosition, position, deltaPosition, time, this.GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0f, 0f, fingerDistance);
				}
				if (this.twoFinger.swipeStart)
				{
					this.CreateGesture2Finger(EasyTouch.EvtType.On_SwipeEnd2Fingers, startPosition, position, deltaPosition, time, this.GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0f, 0f, fingerDistance);
				}
				this.CreateGesture2Finger(EasyTouch.EvtType.On_TouchUp2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
			}
		}

		private IEnumerator SingleOrDouble2Fingers()
		{
			this.singleDoubleTap[99].inWait = true;
			yield return new WaitForSeconds(this.doubleTapTime);
			if (this.singleDoubleTap[99].count < 2)
			{
				this.CreateGesture2Finger(EasyTouch.EvtType.On_SimpleTap2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
			}
			else
			{
				this.CreateGesture2Finger(EasyTouch.EvtType.On_DoubleTap2Fingers, this.twoFinger.startPosition, this.twoFinger.position, this.twoFinger.deltaPosition, this.twoFinger.timeSinceStartAction, EasyTouch.SwipeDirection.None, 0f, Vector2.zero, 0f, 0f, this.twoFinger.fingerDistance);
			}
			this.twoFinger.currentGesture = EasyTouch.GestureType.None;
			this.twoFinger.pickedObject = null;
			this.twoFinger.swipeStart = false;
			this.twoFinger.dragStart = false;
			this.singleDoubleTap[99].Stop();
			base.StopCoroutine("SingleOrDouble2Fingers");
			yield break;
		}

		private void CreateGesture2Finger(EasyTouch.EvtType message, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float actionTime, EasyTouch.SwipeDirection swipe, float swipeLength, Vector2 swipeVector, float twist, float pinch, float twoDistance)
		{
			bool flag = true;
			Gesture gesture = new Gesture();
			gesture.isOverGui = false;
			if (this.enabledNGuiMode && message == EasyTouch.EvtType.On_TouchStart2Fingers)
			{
				gesture.isOverGui = (gesture.isOverGui || (this.IsTouchOverNGui(this.twoFinger.position, false) && this.IsTouchOverNGui(this.twoFinger.position, false)));
			}
			gesture.touchCount = 2;
			gesture.fingerIndex = -1;
			gesture.startPosition = startPosition;
			gesture.position = position;
			gesture.deltaPosition = deltaPosition;
			gesture.actionTime = actionTime;
			gesture.deltaTime = Time.deltaTime;
			gesture.swipe = swipe;
			gesture.swipeLength = swipeLength;
			gesture.swipeVector = swipeVector;
			gesture.deltaPinch = pinch;
			gesture.twistAngle = twist;
			gesture.twoFingerDistance = twoDistance;
			gesture.pickedObject = this.twoFinger.pickedObject;
			gesture.pickedCamera = this.twoFinger.pickedCamera;
			gesture.isGuiCamera = this.twoFinger.isGuiCamera;
			if (this.autoUpdatePickedObject && message != EasyTouch.EvtType.On_Drag && message != EasyTouch.EvtType.On_DragEnd && message != EasyTouch.EvtType.On_Twist && message != EasyTouch.EvtType.On_TwistEnd && message != EasyTouch.EvtType.On_Pinch && message != EasyTouch.EvtType.On_PinchEnd && message != EasyTouch.EvtType.On_PinchIn && message != EasyTouch.EvtType.On_PinchOut)
			{
				if (this.GetTwoFingerPickedObject())
				{
					gesture.pickedObject = this.pickedObject.pickedObj;
					gesture.pickedCamera = this.pickedObject.pickedCamera;
					gesture.isGuiCamera = this.pickedObject.isGUI;
				}
				else
				{
					this.twoFinger.ClearPickedObjectData();
				}
			}
			gesture.pickedUIElement = this.twoFinger.pickedUIElement;
			gesture.isOverGui = this.twoFinger.isOverGui;
			if (this.allowUIDetection && this.autoUpdatePickedUI && message != EasyTouch.EvtType.On_Drag && message != EasyTouch.EvtType.On_DragEnd && message != EasyTouch.EvtType.On_Twist && message != EasyTouch.EvtType.On_TwistEnd && message != EasyTouch.EvtType.On_Pinch && message != EasyTouch.EvtType.On_PinchEnd && message != EasyTouch.EvtType.On_PinchIn && message != EasyTouch.EvtType.On_PinchOut && message == EasyTouch.EvtType.On_SimpleTap2Fingers)
			{
				if (this.GetTwoFingerPickedUIElement())
				{
					gesture.pickedUIElement = this.pickedObject.pickedObj;
					gesture.isOverGui = true;
				}
				else
				{
					this.twoFinger.ClearPickedUIData();
				}
			}
			if (this.enableUIMode || (this.enabledNGuiMode && this.allowUIDetection))
			{
				flag = !gesture.isOverGui;
			}
			if (flag)
			{
				this.RaiseEvent(message, gesture);
			}
			else if (gesture.isOverGui)
			{
				if (message == EasyTouch.EvtType.On_TouchUp2Fingers)
				{
					this.RaiseEvent(EasyTouch.EvtType.On_UIElementTouchUp, gesture);
				}
				else
				{
					this.RaiseEvent(EasyTouch.EvtType.On_OverUIElement, gesture);
				}
			}
		}

		private int GetTwoFinger(int index)
		{
			int num = index + 1;
			bool flag = false;
			while (num < 10 && !flag)
			{
				if (this.fingers[num] != null && num >= index)
				{
					flag = true;
				}
				num++;
			}
			return num - 1;
		}

		private bool GetTwoFingerPickedObject()
		{
			bool result = false;
			if (this.twoFingerPickMethod == EasyTouch.TwoFingerPickMethod.Finger)
			{
				if (this.GetPickedGameObject(this.fingers[this.twoFinger.finger0], false))
				{
					GameObject pickedObj = this.pickedObject.pickedObj;
					if (this.GetPickedGameObject(this.fingers[this.twoFinger.finger1], false) && pickedObj == this.pickedObject.pickedObj)
					{
						result = true;
					}
				}
			}
			else if (this.GetPickedGameObject(this.fingers[this.twoFinger.finger0], true))
			{
				result = true;
			}
			return result;
		}

		private bool GetTwoFingerPickedUIElement()
		{
			bool result = false;
			if (this.fingers[this.twoFinger.finger0] == null)
			{
				return false;
			}
			if (this.twoFingerPickMethod == EasyTouch.TwoFingerPickMethod.Finger)
			{
				if (this.IsScreenPositionOverUI(this.fingers[this.twoFinger.finger0].position))
				{
					GameObject firstUIElementFromCache = this.GetFirstUIElementFromCache();
					if (this.IsScreenPositionOverUI(this.fingers[this.twoFinger.finger1].position))
					{
						GameObject firstUIElementFromCache2 = this.GetFirstUIElementFromCache();
						if (firstUIElementFromCache2 == firstUIElementFromCache || firstUIElementFromCache2.transform.IsChildOf(firstUIElementFromCache.transform) || firstUIElementFromCache.transform.IsChildOf(firstUIElementFromCache2.transform))
						{
							this.pickedObject.pickedObj = firstUIElementFromCache;
							this.pickedObject.isGUI = true;
							result = true;
						}
					}
				}
			}
			else if (this.IsScreenPositionOverUI(this.twoFinger.position))
			{
				this.pickedObject.pickedObj = this.GetFirstUIElementFromCache();
				this.pickedObject.isGUI = true;
				result = true;
			}
			return result;
		}

		private void RaiseEvent(EasyTouch.EvtType evnt, Gesture gesture)
		{
			gesture.type = evnt;
			switch (evnt)
			{
			case EasyTouch.EvtType.On_TouchStart:
				if (EasyTouch.On_TouchStart != null)
				{
					EasyTouch.On_TouchStart(gesture);
				}
				break;
			case EasyTouch.EvtType.On_TouchDown:
				if (EasyTouch.On_TouchDown != null)
				{
					EasyTouch.On_TouchDown(gesture);
				}
				break;
			case EasyTouch.EvtType.On_TouchUp:
				if (EasyTouch.On_TouchUp != null)
				{
					EasyTouch.On_TouchUp(gesture);
				}
				break;
			case EasyTouch.EvtType.On_SimpleTap:
				if (EasyTouch.On_SimpleTap != null)
				{
					EasyTouch.On_SimpleTap(gesture);
				}
				break;
			case EasyTouch.EvtType.On_DoubleTap:
				if (EasyTouch.On_DoubleTap != null)
				{
					EasyTouch.On_DoubleTap(gesture);
				}
				break;
			case EasyTouch.EvtType.On_LongTapStart:
				if (EasyTouch.On_LongTapStart != null)
				{
					EasyTouch.On_LongTapStart(gesture);
				}
				break;
			case EasyTouch.EvtType.On_LongTap:
				if (EasyTouch.On_LongTap != null)
				{
					EasyTouch.On_LongTap(gesture);
				}
				break;
			case EasyTouch.EvtType.On_LongTapEnd:
				if (EasyTouch.On_LongTapEnd != null)
				{
					EasyTouch.On_LongTapEnd(gesture);
				}
				break;
			case EasyTouch.EvtType.On_DragStart:
				if (EasyTouch.On_DragStart != null)
				{
					EasyTouch.On_DragStart(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Drag:
				if (EasyTouch.On_Drag != null)
				{
					EasyTouch.On_Drag(gesture);
				}
				break;
			case EasyTouch.EvtType.On_DragEnd:
				if (EasyTouch.On_DragEnd != null)
				{
					EasyTouch.On_DragEnd(gesture);
				}
				break;
			case EasyTouch.EvtType.On_SwipeStart:
				if (EasyTouch.On_SwipeStart != null)
				{
					EasyTouch.On_SwipeStart(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Swipe:
				if (EasyTouch.On_Swipe != null)
				{
					EasyTouch.On_Swipe(gesture);
				}
				break;
			case EasyTouch.EvtType.On_SwipeEnd:
				if (EasyTouch.On_SwipeEnd != null)
				{
					EasyTouch.On_SwipeEnd(gesture);
				}
				break;
			case EasyTouch.EvtType.On_TouchStart2Fingers:
				if (EasyTouch.On_TouchStart2Fingers != null)
				{
					EasyTouch.On_TouchStart2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_TouchDown2Fingers:
				if (EasyTouch.On_TouchDown2Fingers != null)
				{
					EasyTouch.On_TouchDown2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_TouchUp2Fingers:
				if (EasyTouch.On_TouchUp2Fingers != null)
				{
					EasyTouch.On_TouchUp2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_SimpleTap2Fingers:
				if (EasyTouch.On_SimpleTap2Fingers != null)
				{
					EasyTouch.On_SimpleTap2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_DoubleTap2Fingers:
				if (EasyTouch.On_DoubleTap2Fingers != null)
				{
					EasyTouch.On_DoubleTap2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_LongTapStart2Fingers:
				if (EasyTouch.On_LongTapStart2Fingers != null)
				{
					EasyTouch.On_LongTapStart2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_LongTap2Fingers:
				if (EasyTouch.On_LongTap2Fingers != null)
				{
					EasyTouch.On_LongTap2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_LongTapEnd2Fingers:
				if (EasyTouch.On_LongTapEnd2Fingers != null)
				{
					EasyTouch.On_LongTapEnd2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Twist:
				if (EasyTouch.On_Twist != null)
				{
					EasyTouch.On_Twist(gesture);
				}
				break;
			case EasyTouch.EvtType.On_TwistEnd:
				if (EasyTouch.On_TwistEnd != null)
				{
					EasyTouch.On_TwistEnd(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Pinch:
				if (EasyTouch.On_Pinch != null)
				{
					EasyTouch.On_Pinch(gesture);
				}
				break;
			case EasyTouch.EvtType.On_PinchIn:
				if (EasyTouch.On_PinchIn != null)
				{
					EasyTouch.On_PinchIn(gesture);
				}
				break;
			case EasyTouch.EvtType.On_PinchOut:
				if (EasyTouch.On_PinchOut != null)
				{
					EasyTouch.On_PinchOut(gesture);
				}
				break;
			case EasyTouch.EvtType.On_PinchEnd:
				if (EasyTouch.On_PinchEnd != null)
				{
					EasyTouch.On_PinchEnd(gesture);
				}
				break;
			case EasyTouch.EvtType.On_DragStart2Fingers:
				if (EasyTouch.On_DragStart2Fingers != null)
				{
					EasyTouch.On_DragStart2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Drag2Fingers:
				if (EasyTouch.On_Drag2Fingers != null)
				{
					EasyTouch.On_Drag2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_DragEnd2Fingers:
				if (EasyTouch.On_DragEnd2Fingers != null)
				{
					EasyTouch.On_DragEnd2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_SwipeStart2Fingers:
				if (EasyTouch.On_SwipeStart2Fingers != null)
				{
					EasyTouch.On_SwipeStart2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Swipe2Fingers:
				if (EasyTouch.On_Swipe2Fingers != null)
				{
					EasyTouch.On_Swipe2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_SwipeEnd2Fingers:
				if (EasyTouch.On_SwipeEnd2Fingers != null)
				{
					EasyTouch.On_SwipeEnd2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Cancel:
				if (EasyTouch.On_Cancel != null)
				{
					EasyTouch.On_Cancel(gesture);
				}
				break;
			case EasyTouch.EvtType.On_Cancel2Fingers:
				if (EasyTouch.On_Cancel2Fingers != null)
				{
					EasyTouch.On_Cancel2Fingers(gesture);
				}
				break;
			case EasyTouch.EvtType.On_OverUIElement:
				if (EasyTouch.On_OverUIElement != null)
				{
					EasyTouch.On_OverUIElement(gesture);
				}
				break;
			case EasyTouch.EvtType.On_UIElementTouchUp:
				if (EasyTouch.On_UIElementTouchUp != null)
				{
					EasyTouch.On_UIElementTouchUp(gesture);
				}
				break;
			}
			int num = this._currentGestures.FindIndex((Gesture obj) => obj.type == gesture.type && obj.fingerIndex == gesture.fingerIndex);
			if (num > -1)
			{
				this._currentGestures[num].touchCount = gesture.touchCount;
				this._currentGestures[num].position = gesture.position;
				this._currentGestures[num].actionTime = gesture.actionTime;
				this._currentGestures[num].pickedCamera = gesture.pickedCamera;
				this._currentGestures[num].pickedObject = gesture.pickedObject;
				this._currentGestures[num].pickedUIElement = gesture.pickedUIElement;
				this._currentGestures[num].isOverGui = gesture.isOverGui;
				this._currentGestures[num].isGuiCamera = gesture.isGuiCamera;
				this._currentGestures[num].deltaPinch += gesture.deltaPinch;
				this._currentGestures[num].deltaPosition += gesture.deltaPosition;
				this._currentGestures[num].deltaTime += gesture.deltaTime;
				this._currentGestures[num].twistAngle += gesture.twistAngle;
			}
			if (num == -1)
			{
				this._currentGestures.Add((Gesture)gesture.Clone());
				if (this._currentGestures.Count > 0)
				{
					this._currentGesture = this._currentGestures[0];
				}
			}
		}

		private bool GetPickedGameObject(Finger finger, bool isTowFinger = false)
		{
			if (finger == null && !isTowFinger)
			{
				return false;
			}
			this.pickedObject.isGUI = false;
			this.pickedObject.pickedObj = null;
			this.pickedObject.pickedCamera = null;
			if (this.touchCameras.Count > 0)
			{
				for (int i = 0; i < this.touchCameras.Count; i++)
				{
					if (this.touchCameras[i].camera != null && this.touchCameras[i].camera.enabled)
					{
						Vector2 position = Vector2.zero;
						if (!isTowFinger)
						{
							position = finger.position;
						}
						else
						{
							position = this.twoFinger.position;
						}
						if (this.GetGameObjectAt(position, this.touchCameras[i].camera, this.touchCameras[i].guiCamera))
						{
							return true;
						}
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("No camera is assigned to EasyTouch");
			}
			return false;
		}

		private bool GetGameObjectAt(Vector2 position, Camera cam, bool isGuiCam)
		{
			Ray ray = cam.ScreenPointToRay(position);
			if (this.enable2D)
			{
				LayerMask mask = this.pickableLayers2D;
				RaycastHit2D[] array = new RaycastHit2D[1];
				if (Physics2D.GetRayIntersectionNonAlloc(ray, array, float.PositiveInfinity, mask) > 0)
				{
					this.pickedObject.pickedCamera = cam;
					this.pickedObject.isGUI = isGuiCam;
					this.pickedObject.pickedObj = array[0].collider.gameObject;
					return true;
				}
			}
			LayerMask mask2 = this.pickableLayers3D;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 3.40282347E+38f, mask2))
			{
				this.pickedObject.pickedCamera = cam;
				this.pickedObject.isGUI = isGuiCam;
				this.pickedObject.pickedObj = raycastHit.collider.gameObject;
				return true;
			}
			return false;
		}

		private EasyTouch.SwipeDirection GetSwipe(Vector2 start, Vector2 end)
		{
			Vector2 normalized = (end - start).normalized;
			if (Vector2.Dot(normalized, Vector2.up) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.Up;
			}
			if (Vector2.Dot(normalized, -Vector2.up) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.Down;
			}
			if (Vector2.Dot(normalized, Vector2.right) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.Right;
			}
			if (Vector2.Dot(normalized, -Vector2.right) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.Left;
			}
			Vector2 lhs = normalized;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			if (Vector2.Dot(lhs, vector.normalized) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.UpRight;
			}
			Vector2 lhs2 = normalized;
			Vector2 vector2 = new Vector2(0.5f, -0.5f);
			if (Vector2.Dot(lhs2, vector2.normalized) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.DownRight;
			}
			Vector2 lhs3 = normalized;
			Vector2 vector3 = new Vector2(-0.5f, 0.5f);
			if (Vector2.Dot(lhs3, vector3.normalized) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.UpLeft;
			}
			Vector2 lhs4 = normalized;
			Vector2 vector4 = new Vector2(-0.5f, -0.5f);
			if (Vector2.Dot(lhs4, vector4.normalized) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeDirection.DownLeft;
			}
			return EasyTouch.SwipeDirection.Other;
		}

		private bool FingerInTolerance(Finger finger)
		{
			return (finger.position - finger.startPosition).sqrMagnitude <= this.StationaryTolerance * this.StationaryTolerance;
		}

		private bool IsTouchOverNGui(Vector2 position, bool isTwoFingers = false)
		{
			bool flag = false;
			if (this.enabledNGuiMode)
			{
				LayerMask mask = this.nGUILayers;
				int num = 0;
				while (!flag && num < this.nGUICameras.Count)
				{
					Vector2 v = Vector2.zero;
					if (!isTwoFingers)
					{
						v = position;
					}
					else
					{
						v = this.twoFinger.position;
					}
					Ray ray = this.nGUICameras[num].ScreenPointToRay(v);
					RaycastHit raycastHit;
					flag = Physics.Raycast(ray, out raycastHit, float.MaxValue, mask);
					num++;
				}
			}
			return flag;
		}

		private Finger GetFinger(int finderId)
		{
			int num = 0;
			Finger finger = null;
			while (num < 10 && finger == null)
			{
				if (this.fingers[num] != null && this.fingers[num].fingerIndex == finderId)
				{
					finger = this.fingers[num];
				}
				num++;
			}
			return finger;
		}

		private bool IsScreenPositionOverUI(Vector2 position)
		{
			this.uiEventSystem = EventSystem.current;
			if (this.uiEventSystem != null)
			{
				this.uiPointerEventData = new PointerEventData(this.uiEventSystem);
				this.uiPointerEventData.position = position;
				this.uiEventSystem.RaycastAll(this.uiPointerEventData, this.uiRaycastResultCache);
				return this.uiRaycastResultCache.Count > 0;
			}
			return false;
		}

		private GameObject GetFirstUIElementFromCache()
		{
			if (this.uiRaycastResultCache.Count > 0)
			{
				return this.uiRaycastResultCache[0].gameObject;
			}
			return null;
		}

		private GameObject GetFirstUIElement(Vector2 position)
		{
			if (this.IsScreenPositionOverUI(position))
			{
				return this.GetFirstUIElementFromCache();
			}
			return null;
		}

		public static bool IsFingerOverUIElement(int fingerIndex)
		{
			if (EasyTouch.instance != null)
			{
				Finger finger = EasyTouch.instance.GetFinger(fingerIndex);
				return finger != null && EasyTouch.instance.IsScreenPositionOverUI(finger.position);
			}
			return false;
		}

		public static GameObject GetCurrentPickedUIElement(int fingerIndex, bool isTwoFinger)
		{
			if (!(EasyTouch.instance != null))
			{
				return null;
			}
			Finger finger = EasyTouch.instance.GetFinger(fingerIndex);
			if (finger != null || isTwoFinger)
			{
				Vector2 position = Vector2.zero;
				if (!isTwoFinger)
				{
					position = finger.position;
				}
				else
				{
					position = EasyTouch.instance.twoFinger.position;
				}
				return EasyTouch.instance.GetFirstUIElement(position);
			}
			return null;
		}

		public static GameObject GetCurrentPickedObject(int fingerIndex, bool isTwoFinger)
		{
			if (!(EasyTouch.instance != null))
			{
				return null;
			}
			Finger finger = EasyTouch.instance.GetFinger(fingerIndex);
			if ((finger != null || isTwoFinger) && EasyTouch.instance.GetPickedGameObject(finger, isTwoFinger))
			{
				return EasyTouch.instance.pickedObject.pickedObj;
			}
			return null;
		}

		public static GameObject GetGameObjectAt(Vector2 position, bool isTwoFinger = false)
		{
			if (EasyTouch.instance != null)
			{
				if (isTwoFinger)
				{
					position = EasyTouch.instance.twoFinger.position;
				}
				if (EasyTouch.instance.touchCameras.Count > 0)
				{
					int i = 0;
					while (i < EasyTouch.instance.touchCameras.Count)
					{
						if (EasyTouch.instance.touchCameras[i].camera != null && EasyTouch.instance.touchCameras[i].camera.enabled)
						{
							if (EasyTouch.instance.GetGameObjectAt(position, EasyTouch.instance.touchCameras[i].camera, EasyTouch.instance.touchCameras[i].guiCamera))
							{
								return EasyTouch.instance.pickedObject.pickedObj;
							}
							return null;
						}
						else
						{
							i++;
						}
					}
				}
			}
			return null;
		}

		public static int GetTouchCount()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.input.TouchCount();
			}
			return 0;
		}

		public static void ResetTouch(int fingerIndex)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.GetFinger(fingerIndex).gesture = EasyTouch.GestureType.None;
			}
		}

		public static void SetEnabled(bool enable)
		{
			EasyTouch.instance.enable = enable;
			if (enable)
			{
				EasyTouch.instance.ResetTouches();
			}
		}

		public static bool GetEnabled()
		{
			return EasyTouch.instance && EasyTouch.instance.enable;
		}

		public static void SetEnableUIDetection(bool enable)
		{
			if (EasyTouch.instance != null)
			{
				EasyTouch.instance.allowUIDetection = enable;
			}
		}

		public static bool GetEnableUIDetection()
		{
			return EasyTouch.instance && EasyTouch.instance.allowUIDetection;
		}

		public static void SetUICompatibily(bool value)
		{
			if (EasyTouch.instance != null)
			{
				EasyTouch.instance.enableUIMode = value;
			}
		}

		public static bool GetUIComptability()
		{
			return EasyTouch.instance != null && EasyTouch.instance.enableUIMode;
		}

		public static void SetAutoUpdateUI(bool value)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.autoUpdatePickedUI = value;
			}
		}

		public static bool GetAutoUpdateUI()
		{
			return EasyTouch.instance && EasyTouch.instance.autoUpdatePickedUI;
		}

		public static void SetNGUICompatibility(bool value)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.enabledNGuiMode = value;
			}
		}

		public static bool GetNGUICompatibility()
		{
			return EasyTouch.instance && EasyTouch.instance.enabledNGuiMode;
		}

		public static void SetEnableAutoSelect(bool value)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.autoSelect = value;
			}
		}

		public static bool GetEnableAutoSelect()
		{
			return EasyTouch.instance && EasyTouch.instance.autoSelect;
		}

		public static void SetAutoUpdatePickedObject(bool value)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.autoUpdatePickedObject = value;
			}
		}

		public static bool GetAutoUpdatePickedObject()
		{
			return EasyTouch.instance && EasyTouch.instance.autoUpdatePickedObject;
		}

		public static void Set3DPickableLayer(LayerMask mask)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.pickableLayers3D = mask;
			}
		}

		public static LayerMask Get3DPickableLayer()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.pickableLayers3D;
			}
			return LayerMask.GetMask(new string[]
			{
				"Default"
			});
		}

		public static void AddCamera(Camera cam, bool guiCam = false)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.touchCameras.Add(new ECamera(cam, guiCam));
			}
		}

		public static void RemoveCamera(Camera cam)
		{
			if (EasyTouch.instance)
			{
				int num = EasyTouch.instance.touchCameras.FindIndex((ECamera c) => c.camera == cam);
				if (num > -1)
				{
					EasyTouch.instance.touchCameras[num] = null;
					EasyTouch.instance.touchCameras.RemoveAt(num);
				}
			}
		}

		public static Camera GetCamera(int index = 0)
		{
			if (!EasyTouch.instance)
			{
				return null;
			}
			if (index < EasyTouch.instance.touchCameras.Count)
			{
				return EasyTouch.instance.touchCameras[index].camera;
			}
			return null;
		}

		public static void SetEnable2DCollider(bool value)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.enable2D = value;
			}
		}

		public static bool GetEnable2DCollider()
		{
			return EasyTouch.instance && EasyTouch.instance.enable2D;
		}

		public static void Set2DPickableLayer(LayerMask mask)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.pickableLayers2D = mask;
			}
		}

		public static LayerMask Get2DPickableLayer()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.pickableLayers2D;
			}
			return LayerMask.GetMask(new string[]
			{
				"Default"
			});
		}

		public static void SetGesturePriority(EasyTouch.GesturePriority value)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.gesturePriority = value;
			}
		}

		public static EasyTouch.GesturePriority GetGesturePriority()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.gesturePriority;
			}
			return EasyTouch.GesturePriority.Tap;
		}

		public static void SetStationaryTolerance(float tolerance)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.StationaryTolerance = tolerance;
			}
		}

		public static float GetStationaryTolerance()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.StationaryTolerance;
			}
			return -1f;
		}

		public static void SetLongTapTime(float time)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.longTapTime = time;
			}
		}

		public static float GetlongTapTime()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.longTapTime;
			}
			return -1f;
		}

		public static void SetDoubleTapTime(float time)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.doubleTapTime = time;
			}
		}

		public static float GetDoubleTapTime()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.doubleTapTime;
			}
			return -1f;
		}

		public static void SetDoubleTapMethod(EasyTouch.DoubleTapDetection detection)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.doubleTapDetection = detection;
			}
		}

		public static EasyTouch.DoubleTapDetection GetDoubleTapMethod()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.doubleTapDetection;
			}
			return EasyTouch.DoubleTapDetection.BySystem;
		}

		public static void SetSwipeTolerance(float tolerance)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.swipeTolerance = tolerance;
			}
		}

		public static float GetSwipeTolerance()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.swipeTolerance;
			}
			return -1f;
		}

		public static void SetEnable2FingersGesture(bool enable)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.enable2FingersGesture = enable;
			}
		}

		public static bool GetEnable2FingersGesture()
		{
			return EasyTouch.instance && EasyTouch.instance.enable2FingersGesture;
		}

		public static void SetTwoFingerPickMethod(EasyTouch.TwoFingerPickMethod pickMethod)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.twoFingerPickMethod = pickMethod;
			}
		}

		public static EasyTouch.TwoFingerPickMethod GetTwoFingerPickMethod()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.twoFingerPickMethod;
			}
			return EasyTouch.TwoFingerPickMethod.Finger;
		}

		public static void SetEnablePinch(bool enable)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.enablePinch = enable;
			}
		}

		public static bool GetEnablePinch()
		{
			return EasyTouch.instance && EasyTouch.instance.enablePinch;
		}

		public static void SetMinPinchLength(float length)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.minPinchLength = length;
			}
		}

		public static float GetMinPinchLength()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.minPinchLength;
			}
			return -1f;
		}

		public static void SetEnableTwist(bool enable)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.enableTwist = enable;
			}
		}

		public static bool GetEnableTwist()
		{
			return EasyTouch.instance && EasyTouch.instance.enableTwist;
		}

		public static void SetMinTwistAngle(float angle)
		{
			if (EasyTouch.instance)
			{
				EasyTouch.instance.minTwistAngle = angle;
			}
		}

		public static float GetMinTwistAngle()
		{
			if (EasyTouch.instance)
			{
				return EasyTouch.instance.minTwistAngle;
			}
			return -1f;
		}

		public static bool GetSecondeFingerSimulation()
		{
			return EasyTouch.instance != null && EasyTouch.instance.enableSimulation;
		}

		public static void SetSecondFingerSimulation(bool value)
		{
			if (EasyTouch.instance != null)
			{
				EasyTouch.instance.enableSimulation = value;
			}
		}

		private static EasyTouch _instance;

		private Gesture _currentGesture = new Gesture();

		private List<Gesture> _currentGestures = new List<Gesture>();

		public bool enable;

		public bool enableRemote;

		public EasyTouch.GesturePriority gesturePriority;

		public float StationaryTolerance;

		public float longTapTime;

		public float swipeTolerance;

		public float minPinchLength;

		public float minTwistAngle;

		public EasyTouch.DoubleTapDetection doubleTapDetection;

		public float doubleTapTime;

		public bool alwaysSendSwipe;

		public bool enable2FingersGesture;

		public bool enableTwist;

		public bool enablePinch;

		public bool enable2FingersSwipe;

		public EasyTouch.TwoFingerPickMethod twoFingerPickMethod;

		public List<ECamera> touchCameras;

		public bool autoSelect;

		public LayerMask pickableLayers3D;

		public bool enable2D;

		public LayerMask pickableLayers2D;

		public bool autoUpdatePickedObject;

		public bool allowUIDetection;

		public bool enableUIMode;

		public bool autoUpdatePickedUI;

		public bool enabledNGuiMode;

		public LayerMask nGUILayers;

		public List<Camera> nGUICameras;

		public bool enableSimulation;

		public KeyCode twistKey;

		public KeyCode swipeKey;

		public bool showGuiInspector;

		public bool showSelectInspector;

		public bool showGestureInspector;

		public bool showTwoFingerInspector;

		public bool showSecondFingerInspector;

		private EasyTouchInput input = new EasyTouchInput();

		private Finger[] fingers = new Finger[100];

		public Texture secondFingerTexture;

		private TwoFingerGesture twoFinger = new TwoFingerGesture();

		private int oldTouchCount;

		private EasyTouch.DoubleTap[] singleDoubleTap = new EasyTouch.DoubleTap[100];

		private Finger[] tmpArray = new Finger[100];

		private EasyTouch.PickedObject pickedObject = new EasyTouch.PickedObject();

		private List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

		private PointerEventData uiPointerEventData;

		private EventSystem uiEventSystem;

		[Serializable]
		private class DoubleTap
		{
			public void Stop()
			{
				this.inDoubleTap = false;
				this.inWait = false;
				this.time = 0f;
				this.count = 0;
			}

			public bool inDoubleTap;

			public bool inWait;

			public float time;

			public int count;

			public Finger finger;
		}

		private class PickedObject
		{
			public GameObject pickedObj;

			public Camera pickedCamera;

			public bool isGUI;
		}

		public delegate void TouchCancelHandler(Gesture gesture);

		public delegate void Cancel2FingersHandler(Gesture gesture);

		public delegate void TouchStartHandler(Gesture gesture);

		public delegate void TouchDownHandler(Gesture gesture);

		public delegate void TouchUpHandler(Gesture gesture);

		public delegate void SimpleTapHandler(Gesture gesture);

		public delegate void DoubleTapHandler(Gesture gesture);

		public delegate void LongTapStartHandler(Gesture gesture);

		public delegate void LongTapHandler(Gesture gesture);

		public delegate void LongTapEndHandler(Gesture gesture);

		public delegate void DragStartHandler(Gesture gesture);

		public delegate void DragHandler(Gesture gesture);

		public delegate void DragEndHandler(Gesture gesture);

		public delegate void SwipeStartHandler(Gesture gesture);

		public delegate void SwipeHandler(Gesture gesture);

		public delegate void SwipeEndHandler(Gesture gesture);

		public delegate void TouchStart2FingersHandler(Gesture gesture);

		public delegate void TouchDown2FingersHandler(Gesture gesture);

		public delegate void TouchUp2FingersHandler(Gesture gesture);

		public delegate void SimpleTap2FingersHandler(Gesture gesture);

		public delegate void DoubleTap2FingersHandler(Gesture gesture);

		public delegate void LongTapStart2FingersHandler(Gesture gesture);

		public delegate void LongTap2FingersHandler(Gesture gesture);

		public delegate void LongTapEnd2FingersHandler(Gesture gesture);

		public delegate void TwistHandler(Gesture gesture);

		public delegate void TwistEndHandler(Gesture gesture);

		public delegate void PinchInHandler(Gesture gesture);

		public delegate void PinchOutHandler(Gesture gesture);

		public delegate void PinchEndHandler(Gesture gesture);

		public delegate void PinchHandler(Gesture gesture);

		public delegate void DragStart2FingersHandler(Gesture gesture);

		public delegate void Drag2FingersHandler(Gesture gesture);

		public delegate void DragEnd2FingersHandler(Gesture gesture);

		public delegate void SwipeStart2FingersHandler(Gesture gesture);

		public delegate void Swipe2FingersHandler(Gesture gesture);

		public delegate void SwipeEnd2FingersHandler(Gesture gesture);

		public delegate void EasyTouchIsReadyHandler();

		public delegate void OverUIElementHandler(Gesture gesture);

		public delegate void UIElementTouchUpHandler(Gesture gesture);

		public enum GesturePriority
		{
			Tap,
			Slips
		}

		public enum DoubleTapDetection
		{
			BySystem,
			ByTime
		}

		public enum GestureType
		{
			Tap,
			Drag,
			Swipe,
			None,
			LongTap,
			Pinch,
			Twist,
			Cancel,
			Acquisition
		}

		public enum SwipeDirection
		{
			None,
			Left,
			Right,
			Up,
			Down,
			UpLeft,
			UpRight,
			DownLeft,
			DownRight,
			Other,
			All
		}

		public enum TwoFingerPickMethod
		{
			Finger,
			Average
		}

		public enum EvtType
		{
			None,
			On_TouchStart,
			On_TouchDown,
			On_TouchUp,
			On_SimpleTap,
			On_DoubleTap,
			On_LongTapStart,
			On_LongTap,
			On_LongTapEnd,
			On_DragStart,
			On_Drag,
			On_DragEnd,
			On_SwipeStart,
			On_Swipe,
			On_SwipeEnd,
			On_TouchStart2Fingers,
			On_TouchDown2Fingers,
			On_TouchUp2Fingers,
			On_SimpleTap2Fingers,
			On_DoubleTap2Fingers,
			On_LongTapStart2Fingers,
			On_LongTap2Fingers,
			On_LongTapEnd2Fingers,
			On_Twist,
			On_TwistEnd,
			On_Pinch,
			On_PinchIn,
			On_PinchOut,
			On_PinchEnd,
			On_DragStart2Fingers,
			On_Drag2Fingers,
			On_DragEnd2Fingers,
			On_SwipeStart2Fingers,
			On_Swipe2Fingers,
			On_SwipeEnd2Fingers,
			On_EasyTouchIsReady,
			On_Cancel,
			On_Cancel2Fingers,
			On_OverUIElement,
			On_UIElementTouchUp
		}
	}
}
