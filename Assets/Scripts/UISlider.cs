using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
[ExecuteInEditMode]
public class UISlider : UIProgressBar
{
	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	protected override void OnStart()
	{
		GameObject go = (!(this.mBG != null) || (!(this.mBG.GetComponent<Collider>() != null) && !(this.mBG.GetComponent<Collider2D>() != null))) ? base.gameObject : this.mBG.gameObject;
		UIEventListener uieventListener = UIEventListener.Get(go);
		UIEventListener uieventListener2 = uieventListener;
		uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener uieventListener3 = uieventListener;
		uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uieventListener4 = UIEventListener.Get(this.thumb.gameObject);
			UIEventListener uieventListener5 = uieventListener4;
			uieventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener uieventListener6 = uieventListener4;
			uieventListener6.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener6.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
	}

	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = ((!(this.mFG == null)) ? (base.value - base.ScreenToValue(UICamera.lastEventPosition)) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastEventPosition);
	}

	public override void OnPan(Vector2 delta)
	{
		if (base.enabled && this.isColliderEnabled)
		{
			base.OnPan(delta);
		}
	}

	[SerializeField]
	[HideInInspector]
	private Transform foreground;

	[SerializeField]
	[HideInInspector]
	private float rawValue = 1f;

	[SerializeField]
	[HideInInspector]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	[SerializeField]
	[HideInInspector]
	protected bool mInverted;

	private enum Direction
	{
		Horizontal,
		Vertical,
		Upgraded
	}
}
