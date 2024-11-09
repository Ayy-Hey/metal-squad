using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public abstract class ETCBase : MonoBehaviour
{
	public ETCBase.RectAnchor anchor
	{
		get
		{
			return this._anchor;
		}
		set
		{
			if (value != this._anchor)
			{
				this._anchor = value;
				this.SetAnchorPosition();
			}
		}
	}

	public Vector2 anchorOffet
	{
		get
		{
			return this._anchorOffet;
		}
		set
		{
			if (value != this._anchorOffet)
			{
				this._anchorOffet = value;
				this.SetAnchorPosition();
			}
		}
	}

	public bool visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			if (value != this._visible)
			{
				this._visible = value;
				this.SetVisible(true);
			}
		}
	}

	public bool activated
	{
		get
		{
			return this._activated;
		}
		set
		{
			if (value != this._activated)
			{
				this._activated = value;
				this.SetActivated();
			}
		}
	}

	protected virtual void Awake()
	{
		this.cachedRectTransform = (base.transform as RectTransform);
		this.cachedRootCanvas = base.transform.parent.GetComponent<Canvas>();
		if (!this.allowSimulationStandalone)
		{
			this.enableKeySimulation = false;
		}
		this.visibleAtStart = this._visible;
		this.activatedAtStart = this._activated;
		if (!this.isUnregisterAtDisable)
		{
			ETCInput.instance.RegisterControl(this);
		}
	}

	public virtual void Start()
	{
		if (this.enableCamera && this.autoLinkTagCam)
		{
			this.cameraTransform = null;
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.autoCamTag);
			if (gameObject)
			{
				this.cameraTransform = gameObject.transform;
			}
		}
	}

	public virtual void OnEnable()
	{
		if (this.isUnregisterAtDisable)
		{
			ETCInput.instance.RegisterControl(this);
		}
		this.visible = this.visibleAtStart;
		this.activated = this.activatedAtStart;
	}

	private void OnDisable()
	{
		if (ETCInput._instance && this.isUnregisterAtDisable)
		{
			ETCInput.instance.UnRegisterControl(this);
		}
		this.visibleAtStart = this._visible;
		this.activated = this._activated;
		this.visible = false;
		this.activated = false;
	}

	private void OnDestroy()
	{
		if (ETCInput._instance)
		{
			ETCInput.instance.UnRegisterControl(this);
		}
	}

	public virtual void Update()
	{
		if (!this.useFixedUpdate)
		{
			base.StartCoroutine("UpdateVirtualControl");
		}
	}

	public virtual void FixedUpdate()
	{
		if (this.useFixedUpdate)
		{
			base.StartCoroutine("FixedUpdateVirtualControl");
		}
	}

	public virtual void LateUpdate()
	{
		if (this.enableCamera)
		{
			if (this.autoLinkTagCam && this.cameraTransform == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag(this.autoCamTag);
				if (gameObject)
				{
					this.cameraTransform = gameObject.transform;
				}
			}
			ETCBase.CameraMode cameraMode = this.cameraMode;
			if (cameraMode != ETCBase.CameraMode.Follow)
			{
				if (cameraMode == ETCBase.CameraMode.SmoothFollow)
				{
					this.CameraSmoothFollow();
				}
			}
			else
			{
				this.CameraFollow();
			}
		}
	}

	protected virtual void UpdateControlState()
	{
	}

	protected virtual void SetVisible(bool forceUnvisible = true)
	{
	}

	protected virtual void SetActivated()
	{
	}

	public void SetAnchorPosition()
	{
		switch (this._anchor)
		{
		case ETCBase.RectAnchor.BottomLeft:
			this.rectTransform().anchorMin = new Vector2(0f, 0f);
			this.rectTransform().anchorMax = new Vector2(0f, 0f);
			this.rectTransform().anchoredPosition = new Vector2(this.rectTransform().sizeDelta.x / 2f + this._anchorOffet.x, this.rectTransform().sizeDelta.y / 2f + this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.BottomCenter:
			this.rectTransform().anchorMin = new Vector2(0.5f, 0f);
			this.rectTransform().anchorMax = new Vector2(0.5f, 0f);
			this.rectTransform().anchoredPosition = new Vector2(this._anchorOffet.x, this.rectTransform().sizeDelta.y / 2f + this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.BottonRight:
			this.rectTransform().anchorMin = new Vector2(1f, 0f);
			this.rectTransform().anchorMax = new Vector2(1f, 0f);
			this.rectTransform().anchoredPosition = new Vector2(-this.rectTransform().sizeDelta.x / 2f - this._anchorOffet.x, this.rectTransform().sizeDelta.y / 2f + this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.CenterLeft:
			this.rectTransform().anchorMin = new Vector2(0f, 0.5f);
			this.rectTransform().anchorMax = new Vector2(0f, 0.5f);
			this.rectTransform().anchoredPosition = new Vector2(this.rectTransform().sizeDelta.x / 2f + this._anchorOffet.x, this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.Center:
			this.rectTransform().anchorMin = new Vector2(0.5f, 0.5f);
			this.rectTransform().anchorMax = new Vector2(0.5f, 0.5f);
			this.rectTransform().anchoredPosition = new Vector2(this._anchorOffet.x, this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.CenterRight:
			this.rectTransform().anchorMin = new Vector2(1f, 0.5f);
			this.rectTransform().anchorMax = new Vector2(1f, 0.5f);
			this.rectTransform().anchoredPosition = new Vector2(-this.rectTransform().sizeDelta.x / 2f - this._anchorOffet.x, this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.TopLeft:
			this.rectTransform().anchorMin = new Vector2(0f, 1f);
			this.rectTransform().anchorMax = new Vector2(0f, 1f);
			this.rectTransform().anchoredPosition = new Vector2(this.rectTransform().sizeDelta.x / 2f + this._anchorOffet.x, -this.rectTransform().sizeDelta.y / 2f - this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.TopCenter:
			this.rectTransform().anchorMin = new Vector2(0.5f, 1f);
			this.rectTransform().anchorMax = new Vector2(0.5f, 1f);
			this.rectTransform().anchoredPosition = new Vector2(this._anchorOffet.x, -this.rectTransform().sizeDelta.y / 2f - this._anchorOffet.y);
			break;
		case ETCBase.RectAnchor.TopRight:
			this.rectTransform().anchorMin = new Vector2(1f, 1f);
			this.rectTransform().anchorMax = new Vector2(1f, 1f);
			this.rectTransform().anchoredPosition = new Vector2(-this.rectTransform().sizeDelta.x / 2f - this._anchorOffet.x, -this.rectTransform().sizeDelta.y / 2f - this._anchorOffet.y);
			break;
		}
	}

	protected GameObject GetFirstUIElement(Vector2 position)
	{
		this.uiEventSystem = EventSystem.current;
		if (!(this.uiEventSystem != null))
		{
			return null;
		}
		this.uiPointerEventData = new PointerEventData(this.uiEventSystem);
		this.uiPointerEventData.position = position;
		this.uiEventSystem.RaycastAll(this.uiPointerEventData, this.uiRaycastResultCache);
		if (this.uiRaycastResultCache.Count > 0)
		{
			return this.uiRaycastResultCache[0].gameObject;
		}
		return null;
	}

	protected void CameraSmoothFollow()
	{
		if (!this.cameraTransform || !this.cameraLookAt)
		{
			return;
		}
		float y = this.cameraLookAt.eulerAngles.y;
		float b = this.cameraLookAt.position.y + this.followHeight;
		float num = this.cameraTransform.eulerAngles.y;
		float num2 = this.cameraTransform.position.y;
		num = Mathf.LerpAngle(num, y, this.followRotationDamping * Time.deltaTime);
		num2 = Mathf.Lerp(num2, b, this.followHeightDamping * Time.deltaTime);
		Quaternion rotation = Quaternion.Euler(0f, num, 0f);
		Vector3 vector = this.cameraLookAt.position;
		vector -= rotation * Vector3.forward * this.followDistance;
		vector = new Vector3(vector.x, num2, vector.z);
		RaycastHit raycastHit;
		if (this.enableWallDetection && Physics.Linecast(new Vector3(this.cameraLookAt.position.x, this.cameraLookAt.position.y + 1f, this.cameraLookAt.position.z), vector, out raycastHit))
		{
			vector = new Vector3(raycastHit.point.x, num2, raycastHit.point.z);
		}
		this.cameraTransform.position = vector;
		this.cameraTransform.LookAt(this.cameraLookAt);
	}

	protected void CameraFollow()
	{
		if (!this.cameraTransform || !this.cameraLookAt)
		{
			return;
		}
		Vector3 b = this.followOffset;
		this.cameraTransform.position = this.cameraLookAt.position + b;
		this.cameraTransform.LookAt(this.cameraLookAt.position);
	}

	private IEnumerator UpdateVirtualControl()
	{
		this.DoActionBeforeEndOfFrame();
		yield return new WaitForEndOfFrame();
		this.UpdateControlState();
		yield break;
	}

	private IEnumerator FixedUpdateVirtualControl()
	{
		this.DoActionBeforeEndOfFrame();
		yield return new WaitForFixedUpdate();
		this.UpdateControlState();
		yield break;
	}

	protected virtual void DoActionBeforeEndOfFrame()
	{
	}

	protected RectTransform cachedRectTransform;

	protected Canvas cachedRootCanvas;

	public bool isUnregisterAtDisable;

	private bool visibleAtStart = true;

	private bool activatedAtStart = true;

	[SerializeField]
	protected ETCBase.RectAnchor _anchor;

	[SerializeField]
	protected Vector2 _anchorOffet;

	[SerializeField]
	protected bool _visible;

	[SerializeField]
	protected bool _activated;

	public bool enableCamera;

	public ETCBase.CameraMode cameraMode;

	public string camTargetTag = "Player";

	public bool autoLinkTagCam = true;

	public string autoCamTag = "MainCamera";

	public Transform cameraTransform;

	public ETCBase.CameraTargetMode cameraTargetMode;

	public bool enableWallDetection;

	public LayerMask wallLayer = 0;

	public Transform cameraLookAt;

	protected CharacterController cameraLookAtCC;

	public Vector3 followOffset = new Vector3(0f, 6f, -6f);

	public float followDistance = 10f;

	public float followHeight = 5f;

	public float followRotationDamping = 5f;

	public float followHeightDamping = 5f;

	public int pointId = -1;

	public bool enableKeySimulation;

	public bool allowSimulationStandalone;

	public bool visibleOnStandalone = true;

	public ETCBase.DPadAxis dPadAxisCount;

	public bool useFixedUpdate;

	private List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

	private PointerEventData uiPointerEventData;

	private EventSystem uiEventSystem;

	public bool isOnDrag;

	public bool isSwipeIn;

	public bool isSwipeOut;

	public bool showPSInspector;

	public bool showSpriteInspector;

	public bool showEventInspector;

	public bool showBehaviourInspector;

	public bool showAxesInspector;

	public bool showTouchEventInspector;

	public bool showDownEventInspector;

	public bool showPressEventInspector;

	public bool showCameraInspector;

	public enum ControlType
	{
		Joystick,
		TouchPad,
		DPad,
		Button
	}

	public enum RectAnchor
	{
		UserDefined,
		BottomLeft,
		BottomCenter,
		BottonRight,
		CenterLeft,
		Center,
		CenterRight,
		TopLeft,
		TopCenter,
		TopRight
	}

	public enum DPadAxis
	{
		Two_Axis,
		Four_Axis
	}

	public enum CameraMode
	{
		Follow,
		SmoothFollow
	}

	public enum CameraTargetMode
	{
		UserDefined,
		LinkOnTag,
		FromDirectActionAxisX,
		FromDirectActionAxisY
	}
}
