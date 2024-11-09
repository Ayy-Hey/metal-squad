using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UltimateJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEventSystemHandler
{
	private void OnDisable()
	{
		this.joystickState = false;
		this._pointerId = -10;
		if (this.dynamicPositioning || this.disableVisuals || this.draggable)
		{
			this.joystickSizeFolder.position = this.defaultPos;
			this.joystickCenter = this.joystickBase.position;
		}
		if (this.throwable)
		{
			base.StartCoroutine("ThrowableMovement");
		}
		else
		{
			this.joystick.position = this.joystickCenter;
			if (this.updateHighlightPosition)
			{
				this.highlightJoystick.transform.position = this.joystickCenter;
			}
		}
		if (this.showTension && !this.throwable)
		{
			this.TensionAccentReset();
		}
		if (this.useAnimation)
		{
			this.joystickAnimator.SetBool(this.animationID, false);
		}
		if (this.tapCountOption == UltimateJoystick.TapCountOption.TouchRelease)
		{
			if (this.currentTapTime > 0f)
			{
				base.StartCoroutine("TapCountDelay");
			}
			this.currentTapTime = 0f;
		}
	}

	private void Awake()
	{
		if (Application.isPlaying && this.joystickName != string.Empty)
		{
			if (UltimateJoystick.UltimateJoysticks.ContainsKey(this.joystickName))
			{
				UltimateJoystick.UltimateJoysticks.Remove(this.joystickName);
			}
			UltimateJoystick.UltimateJoysticks.Add(this.joystickName, base.GetComponent<UltimateJoystick>());
		}
	}

	private void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.UpdateSizeAndPlacement();
		this.CheckJoystickHighlightForUse();
		if (this.showHighlight)
		{
			this.UpdateHighlightColor(this.highlightColor);
		}
		if (this.showTension)
		{
			this.TensionAccentReset();
		}
		if (this.useFade)
		{
			this.fadeInSpeed = 1f / this.fadeInDuration;
			this.fadeOutSpeed = 1f / this.fadeOutDuration;
		}
		if (this.useAnimation)
		{
			if (this.joystickAnimator == null)
			{
				this.joystickAnimator = base.GetComponent<Animator>();
			}
			if (this.joystickAnimator == null)
			{
				UnityEngine.Debug.LogError("Ultimate Joystick - This object does not have an Animator component attached to it. Please make sure to attach an Animator to this object before using the Use Animation option.\n\nObject Name: " + base.gameObject.name + "\n");
				this.useAnimation = false;
			}
			else
			{
				this.animationID = Animator.StringToHash("Touch");
			}
		}
		if (!this.GetParentCanvas().GetComponent<UltimateJoystickScreenSizeUpdater>())
		{
			this.GetParentCanvas().gameObject.AddComponent(typeof(UltimateJoystickScreenSizeUpdater));
		}
	}

	public void OnPointerDown(PointerEventData touchInfo)
	{
		if (this.joystickState)
		{
			return;
		}
		this.joystickState = true;
		this._pointerId = touchInfo.pointerId;
		if (this.throwable && this.isThrowing)
		{
			base.StopCoroutine("ThrowableMovement");
		}
		if (this.dynamicPositioning || this.disableVisuals)
		{
			this.joystickSizeFolder.position = touchInfo.position - this.textureCenter;
			this.joystickCenter = touchInfo.position;
		}
		if (this.useAnimation)
		{
			this.joystickAnimator.SetBool(this.animationID, true);
		}
		if (this.useFade && this.joystickGroup != null)
		{
			base.StartCoroutine("FadeLogic");
		}
		if (this.tapCountOption != UltimateJoystick.TapCountOption.NoCount)
		{
			if (this.tapCountOption == UltimateJoystick.TapCountOption.Accumulate)
			{
				if (this.currentTapTime <= 0f)
				{
					this.tapCount = 1;
					base.StartCoroutine("TapCountdown");
				}
				else
				{
					this.tapCount++;
				}
				if (this.currentTapTime > 0f && this.tapCount >= this.targetTapCount)
				{
					this.currentTapTime = 0f;
					base.StartCoroutine("TapCountDelay");
				}
			}
			else
			{
				base.StartCoroutine("TapCountdown");
			}
		}
		this.UpdateJoystick(touchInfo);
	}

	public void OnDrag(PointerEventData touchInfo)
	{
		if (touchInfo.pointerId != this._pointerId)
		{
			return;
		}
		this.UpdateJoystick(touchInfo);
	}

	public void OnPointerUp(PointerEventData touchInfo)
	{
		if (touchInfo.pointerId != this._pointerId)
		{
			return;
		}
		this.joystickState = false;
		this._pointerId = -10;
		if (this.dynamicPositioning || this.disableVisuals || this.draggable)
		{
			this.joystickSizeFolder.position = this.defaultPos;
			this.joystickCenter = this.joystickBase.position;
		}
		if (this.throwable)
		{
			base.StartCoroutine("ThrowableMovement");
		}
		else
		{
			this.joystick.position = this.joystickCenter;
			if (this.updateHighlightPosition)
			{
				this.highlightJoystick.transform.position = this.joystickCenter;
			}
		}
		if (this.showTension && !this.throwable)
		{
			this.TensionAccentReset();
		}
		if (this.useAnimation)
		{
			this.joystickAnimator.SetBool(this.animationID, false);
		}
		if (this.tapCountOption == UltimateJoystick.TapCountOption.TouchRelease)
		{
			if (this.currentTapTime > 0f)
			{
				base.StartCoroutine("TapCountDelay");
			}
			this.currentTapTime = 0f;
		}
	}

	private void UpdateJoystick(PointerEventData touchInfo)
	{
		Vector2 vector = touchInfo.position - (Vector2)this.joystickCenter;
		if (this.axis == UltimateJoystick.Axis.X)
		{
			vector.y = 0f;
		}
		else if (this.axis == UltimateJoystick.Axis.Y)
		{
			vector.x = 0f;
		}
		if (this.boundary == UltimateJoystick.Boundary.Circular)
		{
			vector = Vector2.ClampMagnitude(vector, this.radius);
		}
		else if (this.boundary == UltimateJoystick.Boundary.Square)
		{
			vector.x = Mathf.Clamp(vector.x, -this.radius, this.radius);
			vector.y = Mathf.Clamp(vector.y, -this.radius, this.radius);
		}
		this.joystick.transform.position = this.joystickCenter +(Vector3)vector;
		if (this.updateHighlightPosition)
		{
			this.highlightJoystick.transform.position = this.joystick.transform.position;
		}
		if (this.showTension)
		{
			this.TensionAccentDisplay();
		}
		if (this.draggable)
		{
			Vector3 b = touchInfo.position;
			if (this.axis != UltimateJoystick.Axis.Both)
			{
				if (this.axis == UltimateJoystick.Axis.X)
				{
					b.y = this.joystickCenter.y;
				}
				else
				{
					b.x = this.joystickCenter.x;
				}
			}
			float num = Vector3.Distance(this.joystickCenter, b);
			if (num >= this.radius)
			{
				Vector2 vector2 = (this.joystick.position - this.joystickCenter) / this.radius;
				this.joystickSizeFolder.position += new Vector3(vector2.x, vector2.y, 0f) * (num - this.radius);
				this.joystickCenter = this.joystickBase.position;
			}
		}
	}

	private Vector2 ConfigureImagePosition(Vector2 textureSize, Vector2 customSpacing)
	{
		Vector2 vector = customSpacing / 100f;
		float num = (float)Screen.width * vector.x - textureSize.x * vector.x;
		float y = (float)Screen.height * vector.y - textureSize.y * vector.y;
		Vector2 result;
		result.x = ((this.anchor != UltimateJoystick.Anchor.Left) ? ((float)Screen.width - textureSize.x - num) : num);
		result.y = y;
		return result;
	}

	private void TensionAccentDisplay()
	{
		Vector2 vector = (this.joystick.position - this.joystickCenter) / this.radius;
		if (vector.x > 0f)
		{
			if (this.tensionAccentRight != null)
			{
				this.tensionAccentRight.color = Color.Lerp(this.tensionColorNone, this.tensionColorFull, vector.x);
			}
			if (this.tensionAccentLeft != null && this.tensionAccentLeft.color != this.tensionColorNone)
			{
				this.tensionAccentLeft.color = this.tensionColorNone;
			}
		}
		else
		{
			vector.x = Mathf.Abs(vector.x);
			if (this.tensionAccentLeft != null)
			{
				this.tensionAccentLeft.color = Color.Lerp(this.tensionColorNone, this.tensionColorFull, vector.x);
			}
			if (this.tensionAccentRight != null && this.tensionAccentRight.color != this.tensionColorNone)
			{
				this.tensionAccentRight.color = this.tensionColorNone;
			}
		}
		if (vector.y > 0f)
		{
			if (this.tensionAccentUp != null)
			{
				this.tensionAccentUp.color = Color.Lerp(this.tensionColorNone, this.tensionColorFull, vector.y);
			}
			if (this.tensionAccentDown != null && this.tensionAccentDown.color != this.tensionColorNone)
			{
				this.tensionAccentDown.color = this.tensionColorNone;
			}
		}
		else
		{
			vector.y = Mathf.Abs(vector.y);
			if (this.tensionAccentDown != null)
			{
				this.tensionAccentDown.color = Color.Lerp(this.tensionColorNone, this.tensionColorFull, vector.y);
			}
			if (this.tensionAccentUp != null && this.tensionAccentUp.color != this.tensionColorNone)
			{
				this.tensionAccentUp.color = this.tensionColorNone;
			}
		}
	}

	private void TensionAccentReset()
	{
		if (this.tensionAccentUp != null)
		{
			this.tensionAccentUp.color = this.tensionColorNone;
		}
		if (this.tensionAccentDown != null)
		{
			this.tensionAccentDown.color = this.tensionColorNone;
		}
		if (this.tensionAccentLeft != null)
		{
			this.tensionAccentLeft.color = this.tensionColorNone;
		}
		if (this.tensionAccentRight != null)
		{
			this.tensionAccentRight.color = this.tensionColorNone;
		}
	}

	private IEnumerator ThrowableMovement()
	{
		this.isThrowing = true;
		float throwSpeed = 1f / this.throwDuration;
		Vector3 startJoyPos = this.joystick.position;
		for (float i = 0f; i < 1f; i += Time.deltaTime * throwSpeed)
		{
			this.joystick.position = Vector3.Lerp(startJoyPos, this.joystickCenter, i);
			if (this.updateHighlightPosition)
			{
				this.highlightJoystick.transform.position = this.joystick.position;
			}
			if (this.showTension)
			{
				this.TensionAccentDisplay();
			}
			yield return null;
		}
		this.isThrowing = false;
		this.joystick.position = this.joystickCenter;
		if (this.updateHighlightPosition)
		{
			this.highlightJoystick.transform.position = this.joystick.position;
		}
		if (this.showTension)
		{
			this.TensionAccentReset();
		}
		yield break;
	}

	private Canvas GetParentCanvas()
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			if (parent.transform.GetComponent<Canvas>())
			{
				return parent.transform.GetComponent<Canvas>();
			}
			parent = parent.transform.parent;
		}
		return null;
	}

	private CanvasGroup GetCanvasGroup()
	{
		if (base.GetComponent<CanvasGroup>())
		{
			return base.GetComponent<CanvasGroup>();
		}
		base.gameObject.AddComponent<CanvasGroup>();
		return base.GetComponent<CanvasGroup>();
	}

	private IEnumerator FadeLogic()
	{
		float currentFade = this.joystickGroup.alpha;
		if (float.IsInfinity(this.fadeInSpeed))
		{
			this.joystickGroup.alpha = this.fadeTouched;
		}
		else
		{
			float fadeIn = 0f;
			while (fadeIn < 1f && this.joystickState)
			{
				this.joystickGroup.alpha = Mathf.Lerp(currentFade, this.fadeTouched, fadeIn);
				yield return null;
				fadeIn += Time.deltaTime * this.fadeInSpeed;
			}
			if (this.joystickState)
			{
				this.joystickGroup.alpha = this.fadeTouched;
			}
		}
		while (this.joystickState)
		{
			yield return null;
		}
		currentFade = this.joystickGroup.alpha;
		if (float.IsInfinity(this.fadeOutSpeed))
		{
			this.joystickGroup.alpha = this.fadeUntouched;
		}
		else
		{
			float fadeOut = 0f;
			while (fadeOut < 1f && !this.joystickState)
			{
				this.joystickGroup.alpha = Mathf.Lerp(currentFade, this.fadeUntouched, fadeOut);
				yield return null;
				fadeOut += Time.deltaTime * this.fadeOutSpeed;
			}
			if (!this.joystickState)
			{
				this.joystickGroup.alpha = this.fadeUntouched;
			}
		}
		yield break;
	}

	private IEnumerator TapCountdown()
	{
		this.currentTapTime = this.tapCountDuration;
		while (this.currentTapTime > 0f)
		{
			this.currentTapTime -= Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	private IEnumerator TapCountDelay()
	{
		this.tapCountAchieved = true;
		yield return new WaitForEndOfFrame();
		this.tapCountAchieved = false;
		yield break;
	}

	private void CheckJoystickHighlightForUse()
	{
		if (!this.showHighlight)
		{
			this.updateHighlightPosition = false;
		}
		else if (this.highlightJoystick == null)
		{
			this.updateHighlightPosition = false;
		}
		else if (this.joystick.GetComponent<Image>() == this.highlightJoystick)
		{
			this.updateHighlightPosition = false;
		}
		else
		{
			this.updateHighlightPosition = true;
		}
	}

	private Vector2 GetPositionValues()
	{
		return (this.joystick.position - this.joystickCenter) / this.radius;
	}

	private static bool JoystickConfirmed(string joystickName)
	{
		if (!UltimateJoystick.UltimateJoysticks.ContainsKey(joystickName))
		{
			UnityEngine.Debug.LogWarning("Ultimate Joystick - No Ultimate Joystick has been registered with the name: " + joystickName + ".");
			return false;
		}
		return true;
	}

	private void ResetJoystick()
	{
		this.isThrowing = false;
		base.StopCoroutine("ThrowableMovement");
		this.joystickState = false;
		this._pointerId = -10;
		if (this.dynamicPositioning || this.disableVisuals || this.draggable)
		{
			this.joystickSizeFolder.position = this.defaultPos;
			this.joystickCenter = this.joystickBase.position;
		}
		this.joystick.position = this.joystickCenter;
		if (this.updateHighlightPosition)
		{
			this.highlightJoystick.transform.position = this.joystickCenter;
		}
		if (this.showTension)
		{
			this.TensionAccentReset();
		}
		if (this.useAnimation)
		{
			this.joystickAnimator.SetBool(this.animationID, false);
		}
	}

	private void UpdateSizeAndPlacement()
	{
		if (this.joystickSizeFolder == null || this.joystickBase == null || this.joystick == null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError("Ultimate Joystick - There are some needed components that are not currently assigned. Please check the Assigned Variables section and be sure to assign all of the components.");
			}
			return;
		}
		float num = (float)((this.scalingAxis != UltimateJoystick.ScalingAxis.Height) ? Screen.width : Screen.height);
		float num2 = num * (this.joystickSize / 10f);
		if (this.baseTrans == null)
		{
			this.baseTrans = base.GetComponent<RectTransform>();
		}
		Vector2 vector = this.ConfigureImagePosition(new Vector2(num2, num2), new Vector2(this.customSpacing_X, this.customSpacing_Y));
		if (this.joystickTouchSize == UltimateJoystick.JoystickTouchSize.Custom)
		{
			float num3 = this.customTouchSize_X / 100f;
			float num4 = this.customTouchSize_Y / 100f;
			this.baseTrans.sizeDelta = new Vector2((float)Screen.width * num3, (float)Screen.height * num4);
			Vector2 v = this.ConfigureImagePosition(this.baseTrans.sizeDelta, new Vector2(this.customTouchSizePos_X, this.customTouchSizePos_Y));
			this.baseTrans.position = v;
		}
		else
		{
			float d = (this.joystickTouchSize != UltimateJoystick.JoystickTouchSize.Large) ? ((this.joystickTouchSize != UltimateJoystick.JoystickTouchSize.Medium) ? 1.01f : 1.51f) : 2f;
			Vector2 vector2 = new Vector2(num2, num2);
			this.baseTrans.sizeDelta = vector2 * d;
			this.baseTrans.position = vector - (this.baseTrans.sizeDelta - vector2) / 2f;
		}
		if (this.dynamicPositioning || this.disableVisuals || this.draggable)
		{
			this.textureCenter = new Vector2(num2 / 2f, num2 / 2f);
			this.defaultPos = vector;
		}
		this.joystickSizeFolder.sizeDelta = new Vector2(num2, num2);
		this.joystickSizeFolder.position = vector;
		this.radius = this.joystickSizeFolder.sizeDelta.x * (this.radiusModifier / 10f);
		this.joystickCenter = this.joystickSizeFolder.position + new Vector3(this.joystickSizeFolder.sizeDelta.x / 2f, this.joystickSizeFolder.sizeDelta.y / 2f);
		if (this.useFade && this.joystickGroup == null)
		{
			this.joystickGroup = this.GetCanvasGroup();
		}
	}

	public Vector4 GetSpaceJoystick()
	{
		float num = (float)((this.scalingAxis != UltimateJoystick.ScalingAxis.Height) ? Screen.width : Screen.height);
		float num2 = num * (this.joystickSize / 10f);
		Vector2 textureSize = new Vector2(num2, num2);
		Vector2 vector = this.ConfigureImagePosition(textureSize, new Vector2(0f, 0f));
		Vector2 vector2 = this.ConfigureImagePosition(textureSize, new Vector2(50f, 100f));
		return new Vector4
		{
			x = ((vector2.y <= vector.y) ? vector.y : vector2.y),
			y = ((vector2.y <= vector.y) ? vector2.y : vector.y),
			z = ((vector.x <= vector2.x) ? vector.x : vector2.x),
			w = ((vector.x <= vector2.x) ? vector2.x : vector.x)
		};
	}

	public Vector2 GetCustomSpacing(Vector2 pos)
	{
		float num = (float)((this.scalingAxis != UltimateJoystick.ScalingAxis.Height) ? Screen.width : Screen.height);
		float num2 = num * (this.joystickSize / 10f);
		Vector2 result;
		result.y = pos.y / ((float)Screen.height - num2) * 100f;
		result.x = ((this.anchor != UltimateJoystick.Anchor.Left) ? ((float)Screen.width - num2 - pos.x) : pos.x) / ((float)Screen.width - num2) * 100f;
		return result;
	}

	public void UpdatePositioning()
	{
		if (Application.isPlaying)
		{
			this.ResetJoystick();
		}
		this.UpdateSizeAndPlacement();
	}

	public float GetHorizontalAxis()
	{
		float num = this.GetPositionValues().x;
		if (Mathf.Abs(num) <= this.deadZone)
		{
			num = 0f;
		}
		return num;
	}

	public float GetVerticalAxis()
	{
		float num = this.GetPositionValues().y;
		if (Mathf.Abs(num) <= this.deadZone)
		{
			num = 0f;
		}
		return num;
	}

	public float GetHorizontalAxisRaw()
	{
		float num = this.GetPositionValues().x;
		if (Mathf.Abs(num) <= this.deadZone)
		{
			num = 0f;
		}
		else
		{
			num = ((num >= 0f) ? 1f : -1f);
		}
		return num;
	}

	public float GetVerticalAxisRaw()
	{
		float num = this.GetPositionValues().y;
		if (Mathf.Abs(num) <= this.deadZone)
		{
			num = 0f;
		}
		else
		{
			num = ((num >= 0f) ? 1f : -1f);
		}
		return num;
	}

	public float HorizontalAxis
	{
		get
		{
			float num = this.GetPositionValues().x;
			if (Mathf.Abs(num) <= this.deadZone)
			{
				num = 0f;
			}
			return num;
		}
	}

	public float VerticalAxis
	{
		get
		{
			float num = this.GetPositionValues().y;
			if (Mathf.Abs(num) <= this.deadZone)
			{
				num = 0f;
			}
			return num;
		}
	}

	public float GetDistance()
	{
		return Vector3.Distance(this.joystick.position, this.joystickCenter) / this.radius;
	}

	public void UpdateHighlightColor(Color targetColor)
	{
		if (!this.showHighlight)
		{
			return;
		}
		this.highlightColor = targetColor;
		if (this.highlightBase != null)
		{
			this.highlightBase.color = this.highlightColor;
		}
		if (this.highlightJoystick != null)
		{
			this.highlightJoystick.color = this.highlightColor;
		}
	}

	public void UpdateTensionColors(Color targetTensionNone, Color targetTensionFull)
	{
		if (!this.showTension)
		{
			return;
		}
		this.tensionColorNone = targetTensionNone;
		this.tensionColorFull = targetTensionFull;
	}

	public bool GetJoystickState()
	{
		return this.joystickState;
	}

	public bool GetTapCount()
	{
		return this.tapCountAchieved;
	}

	public void DisableJoystick()
	{
		this.joystickState = false;
		this._pointerId = -10;
		if (this.dynamicPositioning || this.disableVisuals || this.draggable)
		{
			this.joystickSizeFolder.position = this.defaultPos;
			this.joystickCenter = this.joystickBase.position;
		}
		this.joystick.position = this.joystickCenter;
		if (this.updateHighlightPosition)
		{
			this.highlightJoystick.transform.position = this.joystickCenter;
		}
		if (this.showTension)
		{
			this.TensionAccentReset();
		}
		if (this.useAnimation)
		{
			this.joystickAnimator.SetBool(this.animationID, false);
		}
		if (this.useFade)
		{
			this.joystickGroup.alpha = this.fadeUntouched;
		}
		base.gameObject.SetActive(false);
	}

	public void EnableJoystick()
	{
		this.joystick.position = this.joystickCenter;
		base.gameObject.SetActive(true);
	}

	public static UltimateJoystick GetUltimateJoystick(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return null;
		}
		return UltimateJoystick.UltimateJoysticks[joystickName];
	}

	public static float GetHorizontalAxis(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return 0f;
		}
		return UltimateJoystick.UltimateJoysticks[joystickName].GetHorizontalAxis();
	}

	public static float GetVerticalAxis(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return 0f;
		}
		return UltimateJoystick.UltimateJoysticks[joystickName].GetVerticalAxis();
	}

	public static float GetHorizontalAxisRaw(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return 0f;
		}
		return UltimateJoystick.UltimateJoysticks[joystickName].GetHorizontalAxisRaw();
	}

	public static float GetVerticalAxisRaw(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return 0f;
		}
		return UltimateJoystick.UltimateJoysticks[joystickName].GetVerticalAxisRaw();
	}

	public static float GetDistance(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return 0f;
		}
		return UltimateJoystick.UltimateJoysticks[joystickName].GetDistance();
	}

	public static bool GetJoystickState(string joystickName)
	{
		return UltimateJoystick.JoystickConfirmed(joystickName) && UltimateJoystick.UltimateJoysticks[joystickName].joystickState;
	}

	public static bool GetTapCount(string joystickName)
	{
		return UltimateJoystick.JoystickConfirmed(joystickName) && UltimateJoystick.UltimateJoysticks[joystickName].GetTapCount();
	}

	public static void DisableJoystick(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return;
		}
		UltimateJoystick.UltimateJoysticks[joystickName].DisableJoystick();
	}

	public static void EnableJoystick(string joystickName)
	{
		if (!UltimateJoystick.JoystickConfirmed(joystickName))
		{
			return;
		}
		UltimateJoystick.UltimateJoysticks[joystickName].EnableJoystick();
	}

	public RectTransform joystick;

	public RectTransform joystickSizeFolder;

	public RectTransform joystickBase;

	private RectTransform baseTrans;

	private Vector2 textureCenter = Vector2.zero;

	private Vector2 defaultPos = Vector2.zero;

	private Vector3 joystickCenter = Vector3.zero;

	public Image highlightBase;

	public Image highlightJoystick;

	public Image tensionAccentUp;

	public Image tensionAccentDown;

	public Image tensionAccentLeft;

	public Image tensionAccentRight;

	public UltimateJoystick.ScalingAxis scalingAxis = UltimateJoystick.ScalingAxis.Height;

	public UltimateJoystick.Anchor anchor;

	public UltimateJoystick.JoystickTouchSize joystickTouchSize;

	public float joystickSize = 1.75f;

	public float radiusModifier = 4.5f;

	private float radius = 1f;

	public bool dynamicPositioning;

	public float customTouchSize_X = 50f;

	public float customTouchSize_Y = 75f;

	public float customTouchSizePos_X;

	public float customTouchSizePos_Y;

	public float customSpacing_X = 5f;

	public float customSpacing_Y = 20f;

	public bool disableVisuals;

	public bool useFade;

	private CanvasGroup joystickGroup;

	public float fadeUntouched = 1f;

	public float fadeTouched = 0.5f;

	public float fadeInDuration = 1f;

	public float fadeOutDuration = 1f;

	private float fadeInSpeed = 1f;

	private float fadeOutSpeed = 1f;

	public bool useAnimation;

	public Animator joystickAnimator;

	private int animationID;

	public bool showHighlight;

	public Color highlightColor = new Color(1f, 1f, 1f, 1f);

	public bool showTension;

	public Color tensionColorNone = new Color(1f, 1f, 1f, 1f);

	public Color tensionColorFull = new Color(1f, 1f, 1f, 1f);

	public bool throwable;

	public bool draggable;

	public float throwDuration = 0.5f;

	private bool isThrowing;

	public UltimateJoystick.Axis axis;

	public UltimateJoystick.Boundary boundary;

	public UltimateJoystick.TapCountOption tapCountOption;

	public float tapCountDuration = 0.5f;

	public int targetTapCount = 2;

	private float currentTapTime;

	private int tapCount;

	public float deadZone;

	private static Dictionary<string, UltimateJoystick> UltimateJoysticks = new Dictionary<string, UltimateJoystick>();

	public string joystickName;

	private bool joystickState;

	private bool tapCountAchieved;

	private bool updateHighlightPosition;

	private int _pointerId = -10;

	public enum ScalingAxis
	{
		Width,
		Height
	}

	public enum Anchor
	{
		Left,
		Right
	}

	public enum JoystickTouchSize
	{
		Default,
		Medium,
		Large,
		Custom
	}

	public enum Axis
	{
		Both,
		X,
		Y
	}

	public enum Boundary
	{
		Circular,
		Square
	}

	public enum TapCountOption
	{
		NoCount,
		Accumulate,
		TouchRelease
	}
}
