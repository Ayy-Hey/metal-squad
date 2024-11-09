using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ETCButton : ETCBase, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IEventSystemHandler
{
	public ETCButton()
	{
		this.axis = new ETCAxis("Button");
		this._visible = true;
		this._activated = true;
		this.isOnTouch = false;
		this.enableKeySimulation = true;
		this.axis.unityAxis = "Jump";
		this.showPSInspector = true;
		this.showSpriteInspector = false;
		this.showBehaviourInspector = false;
		this.showEventInspector = false;
	}

	protected override void Awake()
	{
		base.Awake();
		this.cachedImage = base.GetComponent<Image>();
	}

	public override void Start()
	{
		this.axis.InitAxis();
		base.Start();
		this.isOnPress = false;
		if (this.allowSimulationStandalone && this.enableKeySimulation && !Application.isEditor)
		{
			this.SetVisible(this.visibleOnStandalone);
		}
	}

	protected override void UpdateControlState()
	{
		this.UpdateButton();
	}

	protected override void DoActionBeforeEndOfFrame()
	{
		this.axis.DoGravity();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.isSwipeIn && !this.isOnTouch)
		{
			if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<ETCBase>() && eventData.pointerDrag != base.gameObject)
			{
				this.previousDargObject = eventData.pointerDrag;
			}
			eventData.pointerDrag = base.gameObject;
			eventData.pointerPress = base.gameObject;
			this.OnPointerDown(eventData);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this._activated && !this.isOnTouch)
		{
			this.pointId = eventData.pointerId;
			this.axis.ResetAxis();
			this.axis.axisState = ETCAxis.AxisState.Down;
			this.isOnPress = false;
			this.isOnTouch = true;
			this.onDown.Invoke();
			this.ApllyState();
			this.axis.UpdateButton();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.pointId == eventData.pointerId)
		{
			this.isOnPress = false;
			this.isOnTouch = false;
			this.axis.axisState = ETCAxis.AxisState.Up;
			this.axis.axisValue = 0f;
			this.onUp.Invoke();
			this.ApllyState();
			if (this.previousDargObject)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(this.previousDargObject, eventData, ExecuteEvents.pointerUpHandler);
				this.previousDargObject = null;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.pointId == eventData.pointerId && this.axis.axisState == ETCAxis.AxisState.Press && !this.isSwipeOut)
		{
			this.OnPointerUp(eventData);
		}
	}

	private void UpdateButton()
	{
		if (this.axis.axisState == ETCAxis.AxisState.Down)
		{
			this.isOnPress = true;
			this.axis.axisState = ETCAxis.AxisState.Press;
		}
		if (this.isOnPress)
		{
			this.axis.UpdateButton();
			this.onPressed.Invoke();
			this.onPressedValue.Invoke(this.axis.axisValue);
		}
		if (this.axis.axisState == ETCAxis.AxisState.Up)
		{
			this.isOnPress = false;
			this.axis.axisState = ETCAxis.AxisState.None;
		}
		if (this.enableKeySimulation && this._activated && this._visible && !this.isOnTouch)
		{
			if (Input.GetButton(this.axis.unityAxis) && this.axis.axisState == ETCAxis.AxisState.None)
			{
				this.axis.ResetAxis();
				this.onDown.Invoke();
				this.axis.axisState = ETCAxis.AxisState.Down;
			}
			if (!Input.GetButton(this.axis.unityAxis) && this.axis.axisState == ETCAxis.AxisState.Press)
			{
				this.axis.axisState = ETCAxis.AxisState.Up;
				this.axis.axisValue = 0f;
				this.onUp.Invoke();
			}
			this.axis.UpdateButton();
			this.ApllyState();
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

	private void ApllyState()
	{
		if (this.cachedImage)
		{
			ETCAxis.AxisState axisState = this.axis.axisState;
			if (axisState != ETCAxis.AxisState.Down && axisState != ETCAxis.AxisState.Press)
			{
				this.cachedImage.sprite = this.normalSprite;
				this.cachedImage.color = this.normalColor;
			}
			else
			{
				this.cachedImage.sprite = this.pressedSprite;
				this.cachedImage.color = this.pressedColor;
			}
		}
	}

	protected override void SetActivated()
	{
		if (!this._activated)
		{
			this.isOnPress = false;
			this.isOnTouch = false;
			this.axis.axisState = ETCAxis.AxisState.None;
			this.axis.axisValue = 0f;
			this.ApllyState();
		}
	}

	[SerializeField]
	public ETCButton.OnDownHandler onDown;

	[SerializeField]
	public ETCButton.OnPressedHandler onPressed;

	[SerializeField]
	public ETCButton.OnPressedValueandler onPressedValue;

	[SerializeField]
	public ETCButton.OnUPHandler onUp;

	public ETCAxis axis;

	public Sprite normalSprite;

	public Color normalColor;

	public Sprite pressedSprite;

	public Color pressedColor;

	private Image cachedImage;

	private bool isOnPress;

	private GameObject previousDargObject;

	private bool isOnTouch;

	[Serializable]
	public class OnDownHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnPressedHandler : UnityEvent
	{
	}

	[Serializable]
	public class OnPressedValueandler : UnityEvent<float>
	{
	}

	[Serializable]
	public class OnUPHandler : UnityEvent
	{
	}
}
