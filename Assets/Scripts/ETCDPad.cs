using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ETCDPad : ETCBase, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public ETCDPad()
	{
		this.axisX = new ETCAxis("Horizontal");
		this.axisY = new ETCAxis("Vertical");
		this._visible = true;
		this._activated = true;
		this.dPadAxisCount = ETCBase.DPadAxis.Two_Axis;
		this.tmpAxis = Vector2.zero;
		this.showPSInspector = true;
		this.showSpriteInspector = false;
		this.showBehaviourInspector = false;
		this.showEventInspector = false;
		this.isOnDrag = false;
		this.isOnTouch = false;
		this.axisX.unityAxis = "Horizontal";
		this.axisY.unityAxis = "Vertical";
		this.enableKeySimulation = true;
	}

	public override void Start()
	{
		base.Start();
		this.tmpAxis = Vector2.zero;
		this.OldTmpAxis = Vector2.zero;
		this.axisX.InitAxis();
		this.axisY.InitAxis();
		if (this.allowSimulationStandalone && this.enableKeySimulation && !Application.isEditor)
		{
			this.SetVisible(this.visibleOnStandalone);
		}
	}

	protected override void UpdateControlState()
	{
		this.UpdateDPad();
	}

	protected override void DoActionBeforeEndOfFrame()
	{
		this.axisX.DoGravity();
		this.axisY.DoGravity();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this._activated && !this.isOnTouch)
		{
			this.onTouchStart.Invoke();
			this.GetTouchDirection(eventData.position, eventData.pressEventCamera);
			this.isOnTouch = true;
			this.isOnDrag = true;
			this.pointId = eventData.pointerId;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (this._activated && this.pointId == eventData.pointerId)
		{
			this.isOnTouch = true;
			this.isOnDrag = true;
			this.GetTouchDirection(eventData.position, eventData.pressEventCamera);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.pointId == eventData.pointerId)
		{
			this.isOnTouch = false;
			this.isOnDrag = false;
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
			this.pointId = -1;
			this.onTouchUp.Invoke();
		}
	}

	private void UpdateDPad()
	{
		if (this.enableKeySimulation && !this.isOnTouch && this._activated && this._visible)
		{
			float axis = UnityEngine.Input.GetAxis(this.axisX.unityAxis);
			float axis2 = UnityEngine.Input.GetAxis(this.axisY.unityAxis);
			this.isOnDrag = false;
			this.tmpAxis = Vector2.zero;
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
		if ((this.axisX.axisValue != 0f || this.axisY.axisValue != 0f) && this.OldTmpAxis == Vector2.zero)
		{
			this.onMoveStart.Invoke();
		}
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
	}

	protected override void SetVisible(bool forceUnvisible = false)
	{
		bool visible = this._visible;
		if (!base.visible)
		{
			visible = base.visible;
		}
		base.GetComponent<Image>().enabled = visible;
	}

	protected override void SetActivated()
	{
		if (!this._activated)
		{
			this.isOnTouch = false;
			this.isOnDrag = false;
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

	private void GetTouchDirection(Vector2 position, Camera cam)
	{
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.cachedRectTransform, position, cam, out vector);
		Vector2 vector2 = this.rectTransform().sizeDelta / this.buttonSizeCoef;
		this.tmpAxis = Vector2.zero;
		if ((vector.x < -vector2.x / 2f && vector.y > -vector2.y / 2f && vector.y < vector2.y / 2f && this.dPadAxisCount == ETCBase.DPadAxis.Two_Axis) || (this.dPadAxisCount == ETCBase.DPadAxis.Four_Axis && vector.x < -vector2.x / 2f))
		{
			this.tmpAxis.x = -1f;
		}
		if ((vector.x > vector2.x / 2f && vector.y > -vector2.y / 2f && vector.y < vector2.y / 2f && this.dPadAxisCount == ETCBase.DPadAxis.Two_Axis) || (this.dPadAxisCount == ETCBase.DPadAxis.Four_Axis && vector.x > vector2.x / 2f))
		{
			this.tmpAxis.x = 1f;
		}
		if ((vector.y > vector2.y / 2f && vector.x > -vector2.x / 2f && vector.x < vector2.x / 2f && this.dPadAxisCount == ETCBase.DPadAxis.Two_Axis) || (this.dPadAxisCount == ETCBase.DPadAxis.Four_Axis && vector.y > vector2.y / 2f))
		{
			this.tmpAxis.y = 1f;
		}
		if ((vector.y < -vector2.y / 2f && vector.x > -vector2.x / 2f && vector.x < vector2.x / 2f && this.dPadAxisCount == ETCBase.DPadAxis.Two_Axis) || (this.dPadAxisCount == ETCBase.DPadAxis.Four_Axis && vector.y < -vector2.y / 2f))
		{
			this.tmpAxis.y = -1f;
		}
	}

	[SerializeField]
	public ETCDPad.OnMoveStartHandler onMoveStart;

	[SerializeField]
	public ETCDPad.OnMoveHandler onMove;

	[SerializeField]
	public ETCDPad.OnMoveSpeedHandler onMoveSpeed;

	[SerializeField]
	public ETCDPad.OnMoveEndHandler onMoveEnd;

	[SerializeField]
	public ETCDPad.OnTouchStartHandler onTouchStart;

	[SerializeField]
	public ETCDPad.OnTouchUPHandler onTouchUp;

	[SerializeField]
	public ETCDPad.OnDownUpHandler OnDownUp;

	[SerializeField]
	public ETCDPad.OnDownDownHandler OnDownDown;

	[SerializeField]
	public ETCDPad.OnDownLeftHandler OnDownLeft;

	[SerializeField]
	public ETCDPad.OnDownRightHandler OnDownRight;

	[SerializeField]
	public ETCDPad.OnDownUpHandler OnPressUp;

	[SerializeField]
	public ETCDPad.OnDownDownHandler OnPressDown;

	[SerializeField]
	public ETCDPad.OnDownLeftHandler OnPressLeft;

	[SerializeField]
	public ETCDPad.OnDownRightHandler OnPressRight;

	public ETCAxis axisX;

	public ETCAxis axisY;

	public Sprite normalSprite;

	public Color normalColor;

	public Sprite pressedSprite;

	public Color pressedColor;

	private Vector2 tmpAxis;

	private Vector2 OldTmpAxis;

	private bool isOnTouch;

	private Image cachedImage;

	public float buttonSizeCoef = 3f;

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
