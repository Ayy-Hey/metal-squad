using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ETCJoystick : ETCBase, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public ETCJoystick()
	{
		this.joystickType = ETCJoystick.JoystickType.Static;
		this.allowJoystickOverTouchPad = false;
		this.radiusBase = ETCJoystick.RadiusBase.Width;
		this.axisX = new ETCAxis("Horizontal");
		this.axisY = new ETCAxis("Vertical");
		this._visible = true;
		this._activated = true;
		this.joystickArea = ETCJoystick.JoystickArea.FullScreen;
		this.isDynamicActif = false;
		this.isOnDrag = false;
		this.isOnTouch = false;
		this.axisX.unityAxis = "Horizontal";
		this.axisY.unityAxis = "Vertical";
		this.enableKeySimulation = true;
		this.isNoReturnThumb = false;
		this.showPSInspector = false;
		this.showAxesInspector = false;
		this.showEventInspector = false;
		this.showSpriteInspector = false;
	}

	public bool IsNoReturnThumb
	{
		get
		{
			return this.isNoReturnThumb;
		}
		set
		{
			this.isNoReturnThumb = value;
		}
	}

	public bool IsNoOffsetThumb
	{
		get
		{
			return this.isNoOffsetThumb;
		}
		set
		{
			this.isNoOffsetThumb = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic)
		{
			this.rectTransform().anchorMin = new Vector2(0.5f, 0.5f);
			this.rectTransform().anchorMax = new Vector2(0.5f, 0.5f);
			this.rectTransform().SetAsLastSibling();
			base.visible = false;
		}
		if (this.allowSimulationStandalone && this.enableKeySimulation && !Application.isEditor && this.joystickType != ETCJoystick.JoystickType.Dynamic)
		{
			this.SetVisible(this.visibleOnStandalone);
		}
	}

	public override void Start()
	{
		this.axisX.InitAxis();
		this.axisY.InitAxis();
		if (this.enableCamera)
		{
			this.InitCameraLookAt();
		}
		this.tmpAxis = Vector2.zero;
		this.OldTmpAxis = Vector2.zero;
		this.noReturnPosition = this.thumb.position;
		this.pointId = -1;
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic)
		{
			base.visible = false;
		}
		base.Start();
		if (this.enableCamera && this.cameraMode == ETCBase.CameraMode.SmoothFollow && this.cameraTransform && this.cameraLookAt)
		{
			this.cameraTransform.position = this.cameraLookAt.TransformPoint(new Vector3(0f, this.followHeight, -this.followDistance));
			this.cameraTransform.LookAt(this.cameraLookAt);
		}
		if (this.enableCamera && this.cameraMode == ETCBase.CameraMode.Follow && this.cameraTransform && this.cameraLookAt)
		{
			this.cameraTransform.position = this.cameraLookAt.position + this.followOffset;
			this.cameraTransform.LookAt(this.cameraLookAt.position);
		}
	}

	public override void Update()
	{
		base.Update();
		if (this.isTouchDown && ProfileManager.settingProfile.TypeControl == 0)
		{
			this.isOnDrag = true;
			this.isOnTouch = true;
			float radius = this.GetRadius();
			this.thumbPosition = this.currentPos - this.pressPos;
			this.thumbPosition.x = (float)Mathf.FloorToInt(this.thumbPosition.x);
			this.thumbPosition.y = (float)Mathf.FloorToInt(this.thumbPosition.y);
			if (this.thumbPosition.magnitude > radius)
			{
				if (!this.isNoReturnThumb)
				{
					this.thumbPosition = this.thumbPosition.normalized * radius;
				}
				else
				{
					this.thumbPosition = this.thumbPosition.normalized * radius;
				}
			}
			this.thumb.anchoredPosition = this.thumbPosition;
		}
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic && !this._visible && this._activated)
		{
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (this.isTouchOverJoystickArea(ref zero, ref zero2))
			{
				GameObject firstUIElement = base.GetFirstUIElement(zero2);
				if (firstUIElement == null || (this.allowJoystickOverTouchPad && firstUIElement.GetComponent<ETCTouchPad>()) || (firstUIElement != null && firstUIElement.GetComponent<ETCArea>()))
				{
					this.cachedRectTransform.anchoredPosition = zero;
					base.visible = true;
				}
			}
		}
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic && this._visible && this.GetTouchCount() == 0)
		{
			base.visible = false;
		}
	}

	public override void LateUpdate()
	{
		if (this.enableCamera && !this.cameraLookAt)
		{
			this.InitCameraLookAt();
		}
		base.LateUpdate();
	}

	private void InitCameraLookAt()
	{
		if (this.cameraTargetMode == ETCBase.CameraTargetMode.FromDirectActionAxisX)
		{
			this.cameraLookAt = this.axisX.directTransform;
		}
		else if (this.cameraTargetMode == ETCBase.CameraTargetMode.FromDirectActionAxisY)
		{
			this.cameraLookAt = this.axisY.directTransform;
			if (this.isTurnAndMove)
			{
				this.cameraLookAt = this.axisX.directTransform;
			}
		}
		else if (this.cameraTargetMode == ETCBase.CameraTargetMode.LinkOnTag)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.camTargetTag);
			if (gameObject)
			{
				this.cameraLookAt = gameObject.transform;
			}
		}
		if (this.cameraLookAt)
		{
			this.cameraLookAtCC = this.cameraLookAt.GetComponent<CharacterController>();
		}
	}

	protected override void UpdateControlState()
	{
		if (this._visible)
		{
			this.UpdateJoystick();
		}
		else if (this.joystickType == ETCJoystick.JoystickType.Dynamic)
		{
			this.OnUp(false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic && !this.isDynamicActif && this._activated && this.pointId == -1)
		{
			eventData.pointerDrag = base.gameObject;
			eventData.pointerPress = base.gameObject;
			this.isDynamicActif = true;
			this.pointId = eventData.pointerId;
		}
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic && !eventData.eligibleForClick)
		{
			this.OnPointerUp(eventData);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.onTouchStart.Invoke();
		this.pointId = eventData.pointerId;
		this.isTouchDown = true;
		this.currentPos = eventData.pressPosition;
		Vector2 vector = new Vector2(base.GetComponent<RectTransform>().anchoredPosition.x + 640f, base.GetComponent<RectTransform>().anchoredPosition.y + 360f);
		int typeControl = ProfileManager.settingProfile.TypeControl;
		if (typeControl != 0)
		{
			if (typeControl == 1)
			{
				this.pressPos = new Vector2(vector.x * (float)Screen.width / 1280f, vector.y * (float)Screen.height / 720f);
			}
		}
		else
		{
			this.pressPos = new Vector2(vector.x * (float)Screen.width / 1280f, vector.y * (float)Screen.height / 720f);
		}
		if (this.isNoOffsetThumb)
		{
			this.OnDrag(eventData);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		this.isTouchDown = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.isTouchDown = false;
		if (this.pointId == eventData.pointerId)
		{
			this.isOnDrag = true;
			this.isOnTouch = true;
			float radius = this.GetRadius();
			this.thumbPosition = eventData.position - this.pressPos;
			this.thumbPosition.x = (float)Mathf.FloorToInt(this.thumbPosition.x);
			this.thumbPosition.y = (float)Mathf.FloorToInt(this.thumbPosition.y);
			if (this.thumbPosition.magnitude > radius)
			{
				if (GameManager.Instance.player != null)
				{
					GameManager.Instance.player._PlayerSpine.FlipX = (this.thumbPosition.x < 0f && Mathf.Abs(this.thumbPosition.x) > 0.2f);
					GameManager.Instance.player.isLockDirection = true;
				}
				if (!this.isNoReturnThumb)
				{
					this.thumbPosition = this.thumbPosition.normalized * radius;
				}
				else
				{
					this.thumbPosition = this.thumbPosition.normalized * radius;
				}
			}
			else if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player.isLockDirection = false;
			}
			this.thumb.anchoredPosition = this.thumbPosition;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.isTouchDown = false;
		GameManager.Instance.player.isLockDirection = false;
		if (this.pointId == eventData.pointerId)
		{
			this.OnUp(true);
		}
	}

	private void OnUp(bool real = true)
	{
		this.isOnDrag = false;
		this.isOnTouch = false;
		if (this.isNoReturnThumb)
		{
			this.noReturnPosition = this.thumb.position;
			this.noReturnOffset = this.thumbPosition;
		}
		if (!this.isNoReturnThumb)
		{
			this.thumbPosition = Vector2.zero;
			this.thumb.anchoredPosition = Vector2.zero;
			this.axisX.axisState = ETCAxis.AxisState.None;
			this.axisY.axisState = ETCAxis.AxisState.None;
		}
		if (!this.axisX.isEnertia && !this.axisY.isEnertia)
		{
			this.axisX.ResetAxis();
			this.axisY.ResetAxis();
			this.tmpAxis = Vector2.zero;
			this.OldTmpAxis = Vector2.zero;
			if (real)
			{
				this.onMoveEnd.Invoke();
			}
		}
		if (this.joystickType == ETCJoystick.JoystickType.Dynamic)
		{
			base.visible = false;
			this.isDynamicActif = false;
		}
		this.pointId = -1;
		if (real)
		{
			this.onTouchUp.Invoke();
		}
	}

	protected override void DoActionBeforeEndOfFrame()
	{
		this.axisX.DoGravity();
		this.axisY.DoGravity();
	}

	private void UpdateJoystick()
	{
		if (this.enableKeySimulation && !this.isOnTouch && this._activated && this._visible)
		{
			float axis = UnityEngine.Input.GetAxis(this.axisX.unityAxis);
			float axis2 = UnityEngine.Input.GetAxis(this.axisY.unityAxis);
			if (!this.isNoReturnThumb)
			{
				this.thumb.localPosition = Vector2.zero;
			}
			this.isOnDrag = false;
			if (axis != 0f)
			{
				this.isOnDrag = true;
				this.thumb.localPosition = new Vector2(this.GetRadius() * axis, this.thumb.localPosition.y);
			}
			if (axis2 != 0f)
			{
				this.isOnDrag = true;
				this.thumb.localPosition = new Vector2(this.thumb.localPosition.x, this.GetRadius() * axis2);
			}
			this.thumbPosition = this.thumb.localPosition;
		}
		this.OldTmpAxis.x = this.axisX.axisValue;
		this.OldTmpAxis.y = this.axisY.axisValue;
		this.tmpAxis = this.thumbPosition / this.GetRadius();
		this.axisX.UpdateAxis(this.tmpAxis.x, this.isOnDrag, ETCBase.ControlType.Joystick, true);
		this.axisY.UpdateAxis(this.tmpAxis.y, this.isOnDrag, ETCBase.ControlType.Joystick, true);
		if ((this.axisX.axisValue != 0f || this.axisY.axisValue != 0f) && this.OldTmpAxis == Vector2.zero)
		{
			this.onMoveStart.Invoke();
		}
		if (this.axisX.axisValue != 0f || this.axisY.axisValue != 0f)
		{
			if (!this.isTurnAndMove)
			{
				if (this.axisX.actionOn == ETCAxis.ActionOn.Down && (this.axisX.axisState == ETCAxis.AxisState.DownLeft || this.axisX.axisState == ETCAxis.AxisState.DownRight))
				{
					this.axisX.DoDirectAction();
				}
				else if (this.axisX.actionOn == ETCAxis.ActionOn.Press)
				{
					this.axisX.DoDirectAction();
				}
				if (this.axisY.actionOn == ETCAxis.ActionOn.Down && (this.axisY.axisState == ETCAxis.AxisState.DownUp || this.axisY.axisState == ETCAxis.AxisState.DownDown))
				{
					this.axisY.DoDirectAction();
				}
				else if (this.axisY.actionOn == ETCAxis.ActionOn.Press)
				{
					this.axisY.DoDirectAction();
				}
			}
			else
			{
				this.DoTurnAndMove();
			}
			this.onMove.Invoke(new Vector2(this.axisX.axisValue, this.axisY.axisValue));
			this.onMoveSpeed.Invoke(new Vector2(this.axisX.axisSpeedValue, this.axisY.axisSpeedValue));
		}
		else if (this.axisX.axisValue == 0f && this.axisY.axisValue == 0f && this.OldTmpAxis != Vector2.zero)
		{
			this.onMoveEnd.Invoke();
		}
		if (!this.isTurnAndMove)
		{
			if (this.axisX.axisValue == 0f && this.axisX.directCharacterController && !this.axisX.directCharacterController.isGrounded && this.axisX.isLockinJump)
			{
				this.axisX.DoDirectAction();
			}
			if (this.axisY.axisValue == 0f && this.axisY.directCharacterController && !this.axisY.directCharacterController.isGrounded && this.axisY.isLockinJump)
			{
				this.axisY.DoDirectAction();
			}
		}
		else if (this.axisX.axisValue == 0f && this.axisY.axisValue == 0f && this.axisX.directCharacterController && !this.axisX.directCharacterController.isGrounded && this.tmLockInJump)
		{
			this.DoTurnAndMove();
		}
		float num = 1f;
		if (this.axisX.invertedAxis)
		{
			num = -1f;
		}
		if (Mathf.Abs(this.OldTmpAxis.x) < this.axisX.axisThreshold && Mathf.Abs(this.axisX.axisValue) >= this.axisX.axisThreshold)
		{
			if (this.axisX.axisValue * num > 0f)
			{
				this.axisX.axisState = ETCAxis.AxisState.DownRight;
				this.OnDownRight.Invoke();
			}
			else if (this.axisX.axisValue * num < 0f)
			{
				this.axisX.axisState = ETCAxis.AxisState.DownLeft;
				this.OnDownLeft.Invoke();
			}
			else
			{
				this.axisX.axisState = ETCAxis.AxisState.None;
			}
		}
		else if (this.axisX.axisState != ETCAxis.AxisState.None)
		{
			if (this.axisX.axisValue * num > 0f)
			{
				this.axisX.axisState = ETCAxis.AxisState.PressRight;
				this.OnPressRight.Invoke();
			}
			else if (this.axisX.axisValue * num < 0f)
			{
				this.axisX.axisState = ETCAxis.AxisState.PressLeft;
				this.OnPressLeft.Invoke();
			}
			else
			{
				this.axisX.axisState = ETCAxis.AxisState.None;
			}
		}
		num = 1f;
		if (this.axisY.invertedAxis)
		{
			num = -1f;
		}
		if (Mathf.Abs(this.OldTmpAxis.y) < this.axisY.axisThreshold && Mathf.Abs(this.axisY.axisValue) >= this.axisY.axisThreshold)
		{
			if (this.axisY.axisValue * num > 0f)
			{
				this.axisY.axisState = ETCAxis.AxisState.DownUp;
				this.OnDownUp.Invoke();
			}
			else if (this.axisY.axisValue * num < 0f)
			{
				this.axisY.axisState = ETCAxis.AxisState.DownDown;
				this.OnDownDown.Invoke();
			}
			else
			{
				this.axisY.axisState = ETCAxis.AxisState.None;
			}
		}
		else if (this.axisY.axisState != ETCAxis.AxisState.None)
		{
			if (this.axisY.axisValue * num > 0f)
			{
				this.axisY.axisState = ETCAxis.AxisState.PressUp;
				this.OnPressUp.Invoke();
			}
			else if (this.axisY.axisValue * num < 0f)
			{
				this.axisY.axisState = ETCAxis.AxisState.PressDown;
				this.OnPressDown.Invoke();
			}
			else
			{
				this.axisY.axisState = ETCAxis.AxisState.None;
			}
		}
	}

	private bool isTouchOverJoystickArea(ref Vector2 localPosition, ref Vector2 screenPosition)
	{
		bool flag = false;
		bool flag2 = false;
		screenPosition = Vector2.zero;
		int touchCount = this.GetTouchCount();
		int num = 0;
		while (num < touchCount && !flag)
		{
			if (UnityEngine.Input.GetTouch(num).phase == TouchPhase.Began)
			{
				screenPosition = UnityEngine.Input.GetTouch(num).position;
				flag2 = true;
			}
			if (flag2 && this.isScreenPointOverArea(screenPosition, ref localPosition))
			{
				flag = true;
			}
			num++;
		}
		return flag;
	}

	private bool isScreenPointOverArea(Vector2 screenPosition, ref Vector2 localPosition)
	{
		bool result = false;
		if (this.joystickArea != ETCJoystick.JoystickArea.UserDefined)
		{
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.cachedRootCanvas.rectTransform(), screenPosition, null, out localPosition))
			{
				switch (this.joystickArea)
				{
				case ETCJoystick.JoystickArea.FullScreen:
					result = true;
					break;
				case ETCJoystick.JoystickArea.Left:
					if (localPosition.x < 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.Right:
					if (localPosition.x > 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.Top:
					if (localPosition.y > 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.Bottom:
					if (localPosition.y < 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.TopLeft:
					if (localPosition.y > 0f && localPosition.x < 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.TopRight:
					if (localPosition.y > 0f && localPosition.x > 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.BottomLeft:
					if (localPosition.y < 0f && localPosition.x < 0f)
					{
						result = true;
					}
					break;
				case ETCJoystick.JoystickArea.BottomRight:
					if (localPosition.y < 0f && localPosition.x > 0f)
					{
						result = true;
					}
					break;
				}
			}
		}
		else if (RectTransformUtility.RectangleContainsScreenPoint(this.userArea, screenPosition, this.cachedRootCanvas.worldCamera))
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.cachedRootCanvas.rectTransform(), screenPosition, this.cachedRootCanvas.worldCamera, out localPosition);
			result = true;
		}
		return result;
	}

	private int GetTouchCount()
	{
		return UnityEngine.Input.touchCount;
	}

	public float GetRadius()
	{
		float result = 0f;
		ETCJoystick.RadiusBase radiusBase = this.radiusBase;
		if (radiusBase != ETCJoystick.RadiusBase.Width)
		{
			if (radiusBase != ETCJoystick.RadiusBase.Height)
			{
				if (radiusBase == ETCJoystick.RadiusBase.UserDefined)
				{
					result = this.radiusBaseValue;
				}
			}
			else
			{
				result = this.cachedRectTransform.sizeDelta.y * 0.5f;
			}
		}
		else
		{
			result = this.cachedRectTransform.sizeDelta.x * 0.5f;
		}
		return result;
	}

	protected override void SetActivated()
	{
		base.GetComponent<CanvasGroup>().blocksRaycasts = this._activated;
		if (!this._activated)
		{
			this.OnUp(false);
		}
	}

	protected override void SetVisible(bool visible = true)
	{
		bool enabled = this._visible;
		if (!visible)
		{
			enabled = visible;
		}
		base.GetComponent<Image>().enabled = enabled;
		this.thumb.GetComponent<Image>().enabled = enabled;
		base.GetComponent<CanvasGroup>().blocksRaycasts = this._activated;
	}

	private void DoTurnAndMove()
	{
		float num = Mathf.Atan2(this.axisX.axisValue, this.axisY.axisValue) * 57.29578f;
		AnimationCurve animationCurve = this.tmMoveCurve;
		Vector2 vector = new Vector2(this.axisX.axisValue, this.axisY.axisValue);
		float d = animationCurve.Evaluate(vector.magnitude) * this.tmSpeed;
		if (this.axisX.directTransform != null)
		{
			this.axisX.directTransform.rotation = Quaternion.Euler(new Vector3(0f, num + this.tmAdditionnalRotation, 0f));
			if (this.axisX.directCharacterController != null)
			{
				if (this.axisX.directCharacterController.isGrounded || !this.tmLockInJump)
				{
					Vector3 a = this.axisX.directCharacterController.transform.TransformDirection(Vector3.forward) * d;
					this.axisX.directCharacterController.Move(a * Time.deltaTime);
					this.tmLastMove = a;
				}
				else
				{
					this.axisX.directCharacterController.Move(this.tmLastMove * Time.deltaTime);
				}
			}
			else
			{
				this.axisX.directTransform.Translate(Vector3.forward * d * Time.deltaTime, Space.Self);
			}
		}
	}

	public void InitCurve()
	{
		this.axisX.InitDeadCurve();
		this.axisY.InitDeadCurve();
		this.InitTurnMoveCurve();
	}

	public void InitTurnMoveCurve()
	{
		this.tmMoveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
		this.tmMoveCurve.postWrapMode = WrapMode.PingPong;
		this.tmMoveCurve.preWrapMode = WrapMode.PingPong;
	}

	public float DistanceThumbToOrigin()
	{
		return this.thumbPosition.magnitude;
	}

	public bool isTouchDown;

	public Vector2 currentPos;

	public Vector2 pressPos = Vector2.zero;

	[SerializeField]
	public ETCJoystick.OnMoveStartHandler onMoveStart;

	[SerializeField]
	public ETCJoystick.OnMoveHandler onMove;

	[SerializeField]
	public ETCJoystick.OnMoveSpeedHandler onMoveSpeed;

	[SerializeField]
	public ETCJoystick.OnMoveEndHandler onMoveEnd;

	[SerializeField]
	public ETCJoystick.OnTouchStartHandler onTouchStart;

	[SerializeField]
	public ETCJoystick.OnTouchUpHandler onTouchUp;

	[SerializeField]
	public ETCJoystick.OnDownUpHandler OnDownUp;

	[SerializeField]
	public ETCJoystick.OnDownDownHandler OnDownDown;

	[SerializeField]
	public ETCJoystick.OnDownLeftHandler OnDownLeft;

	[SerializeField]
	public ETCJoystick.OnDownRightHandler OnDownRight;

	[SerializeField]
	public ETCJoystick.OnDownUpHandler OnPressUp;

	[SerializeField]
	public ETCJoystick.OnDownDownHandler OnPressDown;

	[SerializeField]
	public ETCJoystick.OnDownLeftHandler OnPressLeft;

	[SerializeField]
	public ETCJoystick.OnDownRightHandler OnPressRight;

	public ETCJoystick.JoystickType joystickType;

	public bool allowJoystickOverTouchPad;

	public ETCJoystick.RadiusBase radiusBase;

	public float radiusBaseValue;

	public ETCAxis axisX;

	public ETCAxis axisY;

	public RectTransform thumb;

	public ETCJoystick.JoystickArea joystickArea;

	public RectTransform userArea;

	public bool isTurnAndMove;

	public float tmSpeed = 10f;

	public float tmAdditionnalRotation;

	public AnimationCurve tmMoveCurve;

	public bool tmLockInJump;

	private Vector3 tmLastMove;

	private Vector2 thumbPosition;

	private bool isDynamicActif;

	private Vector2 tmpAxis;

	private Vector2 OldTmpAxis;

	private bool isOnTouch;

	[SerializeField]
	private bool isNoReturnThumb;

	private Vector2 noReturnPosition;

	private Vector2 noReturnOffset;

	[SerializeField]
	private bool isNoOffsetThumb;

	[Serializable]
	public class OnMoveStartHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnMoveSpeedHandler : UnityEvent<Vector2>
	{
	}

	[Serializable]
	public class OnMoveHandler : UnityEvent<Vector2>
	{
	}

	[Serializable]
	public class OnMoveEndHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnTouchStartHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnTouchUpHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnDownUpHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnDownDownHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnDownLeftHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnDownRightHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnPressUpHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnPressDownHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnPressLeftHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnPressRightHandler : UnityEvent
	{
	}

	public enum JoystickArea
	{
		UserDefined,
		FullScreen,
		Left,
		Right,
		Top,
		Bottom,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	public enum JoystickType
	{
		Dynamic,
		Static
	}

	public enum RadiusBase
	{
		Width,
		Height,
		UserDefined
	}
}
