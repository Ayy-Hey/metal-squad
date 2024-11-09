using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ETCTouchPad : ETCBase, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IEventSystemHandler
{
	public ETCTouchPad()
	{
		this.axisX = new ETCAxis("Horizontal");
		this.axisX.speed = 1f;
		this.axisY = new ETCAxis("Vertical");
		this.axisY.speed = 1f;
		this._visible = true;
		this._activated = true;
		this.showPSInspector = true;
		this.showSpriteInspector = false;
		this.showBehaviourInspector = false;
		this.showEventInspector = false;
		this.tmpAxis = Vector2.zero;
		this.isOnDrag = false;
		this.isOnTouch = false;
		this.axisX.unityAxis = "Horizontal";
		this.axisY.unityAxis = "Vertical";
		this.enableKeySimulation = true;
		this.enableKeySimulation = false;
		this.isOut = false;
		this.axisX.axisState = ETCAxis.AxisState.None;
		this.useFixedUpdate = false;
		this.isDPI = false;
	}

	protected override void Awake()
	{
		base.Awake();
		this.cachedVisible = this._visible;
		this.cachedImage = base.GetComponent<Image>();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (!this.cachedVisible)
		{
			this.cachedImage.color = new Color(0f, 0f, 0f, 0f);
		}
		if (this.allowSimulationStandalone && this.enableKeySimulation && !Application.isEditor)
		{
			this.SetVisible(this.visibleOnStandalone);
		}
	}

	public override void Start()
	{
		base.Start();
		this.tmpAxis = Vector2.zero;
		this.OldTmpAxis = Vector2.zero;
		this.axisX.InitAxis();
		this.axisY.InitAxis();
	}

	protected override void UpdateControlState()
	{
		this.UpdateTouchPad();
	}

	protected override void DoActionBeforeEndOfFrame()
	{
		this.axisX.DoGravity();
		this.axisY.DoGravity();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.isSwipeIn && this.axisX.axisState == ETCAxis.AxisState.None && this._activated && !this.isOnTouch)
		{
			if (eventData.pointerDrag != null && eventData.pointerDrag != base.gameObject)
			{
				this.previousDargObject = eventData.pointerDrag;
			}
			else if (eventData.pointerPress != null && eventData.pointerPress != base.gameObject)
			{
				this.previousDargObject = eventData.pointerPress;
			}
			eventData.pointerDrag = base.gameObject;
			eventData.pointerPress = base.gameObject;
			this.OnPointerDown(eventData);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (this.pointId == eventData.pointerId)
		{
			this.onMoveStart.Invoke();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (base.activated && !this.isOut && this.pointId == eventData.pointerId)
		{
			this.isOnTouch = true;
			this.isOnDrag = true;
			if (this.isDPI)
			{
				this.tmpAxis = new Vector2(eventData.delta.x / Screen.dpi * 100f, eventData.delta.y / Screen.dpi * 100f);
			}
			else
			{
				this.tmpAxis = new Vector2(eventData.delta.x, eventData.delta.y);
			}
			if (!this.axisX.enable)
			{
				this.tmpAxis.x = 0f;
			}
			if (!this.axisY.enable)
			{
				this.tmpAxis.y = 0f;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this._activated && !this.isOnTouch)
		{
			this.axisX.axisState = ETCAxis.AxisState.Down;
			this.tmpAxis = eventData.delta;
			this.isOut = false;
			this.isOnTouch = true;
			this.pointId = eventData.pointerId;
			this.onTouchStart.Invoke();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.pointId == eventData.pointerId)
		{
			this.isOnDrag = false;
			this.isOnTouch = false;
			this.tmpAxis = Vector2.zero;
			this.OldTmpAxis = Vector2.zero;
			this.axisX.axisState = ETCAxis.AxisState.None;
			this.axisY.axisState = ETCAxis.AxisState.None;
			if (!this.axisX.isEnertia && !this.axisY.isEnertia)
			{
				this.axisX.ResetAxis();
				this.axisY.ResetAxis();
				this.onMoveEnd.Invoke();
			}
			this.onTouchUp.Invoke();
			if (this.previousDargObject)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(this.previousDargObject, eventData, ExecuteEvents.pointerUpHandler);
				this.previousDargObject = null;
			}
			this.pointId = -1;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.pointId == eventData.pointerId && !this.isSwipeOut)
		{
			this.isOut = true;
			this.OnPointerUp(eventData);
		}
	}

	private void UpdateTouchPad()
	{
		if (this.enableKeySimulation && !this.isOnTouch && this._activated && this._visible)
		{
			this.isOnDrag = false;
			this.tmpAxis = Vector2.zero;
			float axis = UnityEngine.Input.GetAxis(this.axisX.unityAxis);
			float axis2 = UnityEngine.Input.GetAxis(this.axisY.unityAxis);
			if (axis != 0f)
			{
				this.isOnDrag = true;
				this.tmpAxis = new Vector2(1f * Mathf.Sign(axis), this.tmpAxis.y);
			}
			if (axis2 != 0f)
			{
				this.isOnDrag = true;
				this.tmpAxis = new Vector2(this.tmpAxis.x, 1f * Mathf.Sign(axis2));
			}
		}
		this.OldTmpAxis.x = this.axisX.axisValue;
		this.OldTmpAxis.y = this.axisY.axisValue;
		this.axisX.UpdateAxis(this.tmpAxis.x, this.isOnDrag, ETCBase.ControlType.DPad, true);
		this.axisY.UpdateAxis(this.tmpAxis.y, this.isOnDrag, ETCBase.ControlType.DPad, true);
		if (this.axisX.axisValue != 0f || this.axisY.axisValue != 0f)
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
			this.onMove.Invoke(new Vector2(this.axisX.axisValue, this.axisY.axisValue));
			this.onMoveSpeed.Invoke(new Vector2(this.axisX.axisSpeedValue, this.axisY.axisSpeedValue));
		}
		else if (this.axisX.axisValue == 0f && this.axisY.axisValue == 0f && this.OldTmpAxis != Vector2.zero)
		{
			this.onMoveEnd.Invoke();
		}
		float num = 1f;
		if (this.axisX.invertedAxis)
		{
			num = -1f;
		}
		if (this.OldTmpAxis.x == 0f && Mathf.Abs(this.axisX.axisValue) > 0f)
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
		if (this.OldTmpAxis.y == 0f && Mathf.Abs(this.axisY.axisValue) > 0f)
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
		this.tmpAxis = Vector2.zero;
	}

	protected override void SetVisible(bool forceUnvisible = false)
	{
		if (Application.isPlaying)
		{
			if (!this._visible)
			{
				this.cachedImage.color = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				this.cachedImage.color = new Color(1f, 1f, 1f, 1f);
			}
		}
	}

	protected override void SetActivated()
	{
		if (!this._activated)
		{
			this.isOnDrag = false;
			this.isOnTouch = false;
			this.tmpAxis = Vector2.zero;
			this.OldTmpAxis = Vector2.zero;
			this.axisX.axisState = ETCAxis.AxisState.None;
			this.axisY.axisState = ETCAxis.AxisState.None;
			if (!this.axisX.isEnertia && !this.axisY.isEnertia)
			{
				this.axisX.ResetAxis();
				this.axisY.ResetAxis();
			}
			this.pointId = -1;
		}
	}

	[SerializeField]
	public ETCTouchPad.OnMoveStartHandler onMoveStart;

	[SerializeField]
	public ETCTouchPad.OnMoveHandler onMove;

	[SerializeField]
	public ETCTouchPad.OnMoveSpeedHandler onMoveSpeed;

	[SerializeField]
	public ETCTouchPad.OnMoveEndHandler onMoveEnd;

	[SerializeField]
	public ETCTouchPad.OnTouchStartHandler onTouchStart;

	[SerializeField]
	public ETCTouchPad.OnTouchUPHandler onTouchUp;

	[SerializeField]
	public ETCTouchPad.OnDownUpHandler OnDownUp;

	[SerializeField]
	public ETCTouchPad.OnDownDownHandler OnDownDown;

	[SerializeField]
	public ETCTouchPad.OnDownLeftHandler OnDownLeft;

	[SerializeField]
	public ETCTouchPad.OnDownRightHandler OnDownRight;

	[SerializeField]
	public ETCTouchPad.OnDownUpHandler OnPressUp;

	[SerializeField]
	public ETCTouchPad.OnDownDownHandler OnPressDown;

	[SerializeField]
	public ETCTouchPad.OnDownLeftHandler OnPressLeft;

	[SerializeField]
	public ETCTouchPad.OnDownRightHandler OnPressRight;

	public ETCAxis axisX;

	public ETCAxis axisY;

	public bool isDPI;

	private Image cachedImage;

	private Vector2 tmpAxis;

	private Vector2 OldTmpAxis;

	private GameObject previousDargObject;

	private bool isOut;

	private bool isOnTouch;

	private bool cachedVisible;

	[Serializable]
	public class OnMoveStartHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnMoveHandler : UnityEvent<Vector2>
	{
	}

	[Serializable]
	public class OnMoveSpeedHandler : UnityEvent<Vector2>
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
	public class OnTouchUPHandler : UnityEvent
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
}
