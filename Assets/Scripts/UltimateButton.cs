using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UltimateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEventSystemHandler
{
	private void Awake()
	{
		if (Application.isPlaying && this.buttonName != string.Empty)
		{
			this.RegisterButton(this.buttonName);
		}
	}

	private void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		switch (this.positioning)
		{
		case UltimateButton.Positioning.ScreenSpace:
			this.UpdatePositioning();
			break;
		case UltimateButton.Positioning.RelativeToJoystick:
			base.StartCoroutine(this.WaitForPositioning());
			break;
		case UltimateButton.Positioning.RelativeToButton:
			base.StartCoroutine(this.WaitForPositioning());
			break;
		}
		if (this.showHighlight && this.buttonHighlight != null)
		{
			this.buttonHighlight.color = this.highlightColor;
		}
		if (this.showTension)
		{
			this.tensionFadeInSpeed = 1f / this.tensionFadeInDuration;
			this.tensionFadeOutSpeed = 1f / this.tensionFadeOutDuration;
		}
		if (this.useFade)
		{
			this.canvasGroup = base.GetComponent<CanvasGroup>();
			if (this.canvasGroup == null)
			{
				base.gameObject.AddComponent(typeof(CanvasGroup));
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			this.fadeInSpeed = 1f / this.fadeInDuration;
			this.fadeOutSpeed = 1f / this.fadeOutDuration;
			this.canvasGroup.alpha = this.fadeUntouched;
		}
		if (!this.GetParentCanvas().GetComponent<UltimateButtonScreenSizeUpdater>())
		{
			this.GetParentCanvas().gameObject.AddComponent(typeof(UltimateButtonScreenSizeUpdater));
		}
		if (this.transmitInput && this.receiver != null)
		{
			this.downHandler = this.receiver.GetComponent<IPointerDownHandler>();
			this.dragHandler = this.receiver.GetComponent<IDragHandler>();
			this.upHandler = this.receiver.GetComponent<IPointerUpHandler>();
		}
	}

	private void OnDisable()
	{
		this.getButton = false;
		this._pointerId = -10;
		if (!this.isHovering)
		{
			return;
		}
		if (this.tapCountOption == UltimateButton.TapCountOption.TouchRelease)
		{
			if (this.currentTapTime > 0f)
			{
				if (this.buttonName != string.Empty)
				{
					base.StartCoroutine("GetTapCountDelay");
				}
				if (this.tapCountEvent != null)
				{
					this.tapCountEvent.Invoke();
				}
			}
			this.currentTapTime = 0f;
		}
		if (this.useAnimation && this.buttonAnimator != null)
		{
			this.buttonAnimator.SetBool("Touch", false);
		}
		this.isHovering = false;
	}

	private IEnumerator WaitForPositioning()
	{
		yield return null;
		UltimateButton.Positioning positioning = this.positioning;
		if (positioning == UltimateButton.Positioning.RelativeToJoystick || positioning != UltimateButton.Positioning.RelativeToButton)
		{
			this.UpdateSizeAndPlacementRelativeToJoystick();
		}
		else
		{
			this.UpdateSizeAndPlacementRelativeToButton();
		}
		yield break;
	}

	public void RegisterButton(string newName)
	{
		if (UltimateButton.UltimateButtons.ContainsKey(this.buttonName))
		{
			UltimateButton.UltimateButtons.Remove(this.buttonName);
		}
		this.buttonName = newName;
		if (UltimateButton.UltimateButtons.ContainsKey(newName))
		{
			UltimateButton.UltimateButtons.Remove(newName);
		}
		UltimateButton.UltimateButtons.Add(newName, base.GetComponent<UltimateButton>());
	}

	public void OnPointerEnter(PointerEventData touchInfo)
	{
		if (this._pointerId != -10)
		{
			return;
		}
		this._pointerId = touchInfo.pointerId;
		this.getButton = true;
		if (this.buttonName != string.Empty)
		{
			base.StartCoroutine("GetButtonDownDelay");
		}
		if (this.onButtonDown != null)
		{
			this.onButtonDown.Invoke();
		}
		if (this.useAnimation && this.buttonAnimator != null)
		{
			this.buttonAnimator.SetBool("Touch", true);
		}
		if (this.tapCountOption != UltimateButton.TapCountOption.NoCount)
		{
			if (this.tapCountOption == UltimateButton.TapCountOption.Accumulate)
			{
				if (this.currentTapTime <= 0f)
				{
					base.StartCoroutine("TapCountdown");
					this.tapCount = 1;
				}
				else
				{
					this.tapCount++;
				}
				if (this.currentTapTime > 0f && this.tapCount >= this.targetTapCount)
				{
					this.currentTapTime = 0f;
					if (this.buttonName != string.Empty)
					{
						base.StartCoroutine("GetTapCountDelay");
					}
					if (this.tapCountEvent != null)
					{
						this.tapCountEvent.Invoke();
					}
				}
			}
			else if (this.currentTapTime <= 0f)
			{
				base.StartCoroutine("TapCountdown");
			}
			else
			{
				this.currentTapTime = this.tapCountDuration;
			}
		}
		if (this.useFade && this.canvasGroup != null)
		{
			base.StartCoroutine("ButtonFade");
		}
		if (this.showTension && this.tensionAccent != null)
		{
			base.StartCoroutine("TensionAccentFade");
		}
		this.isHovering = true;
		if (this.transmitInput && this.downHandler != null)
		{
			this.downHandler.OnPointerDown(touchInfo);
		}
	}

	public void OnDrag(PointerEventData touchInfo)
	{
		if (touchInfo.pointerId != this._pointerId)
		{
			return;
		}
		if (this.transmitInput && this.dragHandler != null)
		{
			this.dragHandler.OnDrag(touchInfo);
		}
		if (!this.trackInput)
		{
			return;
		}
		if (!this.IsInRange(touchInfo.position) && this.isHovering)
		{
			this.isHovering = false;
			this.getButton = false;
			if (this.useAnimation && this.buttonAnimator != null)
			{
				this.buttonAnimator.SetBool("Touch", false);
			}
		}
		else if (this.IsInRange(touchInfo.position) && !this.isHovering)
		{
			this.isHovering = true;
			this.getButton = true;
			if (this.showTension && this.tensionAccent != null)
			{
				base.StartCoroutine("TensionAccentFade");
			}
			if (this.useFade && this.canvasGroup != null)
			{
				base.StartCoroutine("ButtonFade");
			}
			if (this.useAnimation && this.buttonAnimator != null)
			{
				this.buttonAnimator.SetBool("Touch", true);
			}
		}
	}

	public void OnPointerExit(PointerEventData touchInfo)
	{
		if (touchInfo.pointerId != this._pointerId)
		{
			return;
		}
		this.getButton = false;
		this._pointerId = -10;
		if (!this.isHovering)
		{
			return;
		}
		if (this.buttonName != string.Empty)
		{
			base.StartCoroutine("GetButtonUpDelay");
		}
		if (this.onButtonUp != null)
		{
			this.onButtonUp.Invoke();
		}
		if (this.tapCountOption == UltimateButton.TapCountOption.TouchRelease)
		{
			if (this.currentTapTime > 0f)
			{
				if (this.buttonName != string.Empty)
				{
					base.StartCoroutine("GetTapCountDelay");
				}
				if (this.tapCountEvent != null)
				{
					this.tapCountEvent.Invoke();
				}
			}
			this.currentTapTime = 0f;
		}
		if (this.useAnimation && this.buttonAnimator != null)
		{
			this.buttonAnimator.SetBool("Touch", false);
		}
		this.isHovering = false;
		if (this.transmitInput && this.upHandler != null)
		{
			this.upHandler.OnPointerUp(touchInfo);
		}
	}

	private bool IsInRange(Vector2 inputPos)
	{
		bool result = false;
		if (this.touchSize != UltimateButton.TouchSize.Default)
		{
			result = true;
		}
		else if (this.imageStyle == UltimateButton.ImageStyle.Circular)
		{
			float num = Vector2.Distance(inputPos, this.buttonCenter);
			if (num > this._inputRange)
			{
				result = false;
			}
			else if (num < this._inputRange)
			{
				result = true;
			}
		}
		else
		{
			inputPos -= this.buttonCenter;
			result = (inputPos.x <= this._inputRange && inputPos.x >= -this._inputRange && inputPos.y <= this._inputRange && inputPos.y >= -this._inputRange);
		}
		return result;
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

	private IEnumerator GetButtonDownDelay()
	{
		this.getButtonDown = true;
		yield return new WaitForEndOfFrame();
		this.getButtonDown = false;
		yield break;
	}

	private IEnumerator GetButtonUpDelay()
	{
		this.getButtonUp = true;
		yield return new WaitForEndOfFrame();
		this.getButtonUp = false;
		yield break;
	}

	private IEnumerator GetTapCountDelay()
	{
		this.getTapCount = true;
		yield return new WaitForEndOfFrame();
		this.getTapCount = false;
		yield break;
	}

	private IEnumerator TensionAccentFade()
	{
		Color currentColor = this.tensionAccent.color;
		if (float.IsInfinity(this.tensionFadeInSpeed))
		{
			this.tensionAccent.color = this.tensionColorFull;
		}
		else
		{
			float fadeIn = 0f;
			while (fadeIn < 1f && this.getButton)
			{
				this.tensionAccent.color = Color.Lerp(currentColor, this.tensionColorFull, fadeIn);
				yield return null;
				fadeIn += Time.deltaTime * this.tensionFadeInSpeed;
			}
			if (this.getButton)
			{
				this.tensionAccent.color = this.tensionColorFull;
			}
		}
		while (this.getButton)
		{
			yield return null;
		}
		currentColor = this.tensionAccent.color;
		if (float.IsInfinity(this.tensionFadeOutSpeed))
		{
			this.tensionAccent.color = this.tensionColorNone;
		}
		else
		{
			float fadeOut = 0f;
			while (fadeOut < 1f && !this.getButton)
			{
				this.tensionAccent.color = Color.Lerp(currentColor, this.tensionColorNone, fadeOut);
				yield return null;
				fadeOut += Time.deltaTime * this.tensionFadeOutSpeed;
			}
			if (!this.getButton)
			{
				this.tensionAccent.color = this.tensionColorNone;
			}
		}
		yield break;
	}

	private IEnumerator ButtonFade()
	{
		float currentFade = this.canvasGroup.alpha;
		if (float.IsInfinity(this.fadeInSpeed))
		{
			this.canvasGroup.alpha = this.fadeTouched;
		}
		else
		{
			float fadeIn = 0f;
			while (fadeIn < 1f && this.getButton)
			{
				this.canvasGroup.alpha = Mathf.Lerp(currentFade, this.fadeTouched, fadeIn);
				yield return null;
				fadeIn += Time.unscaledDeltaTime * this.fadeInSpeed;
			}
			if (this.getButton)
			{
				this.canvasGroup.alpha = this.fadeTouched;
			}
		}
		while (this.getButton)
		{
			yield return null;
		}
		currentFade = this.canvasGroup.alpha;
		if (float.IsInfinity(this.fadeOutSpeed))
		{
			this.canvasGroup.alpha = this.fadeUntouched;
		}
		else
		{
			float fadeOut = 0f;
			while (fadeOut < 1f && !this.getButton)
			{
				this.canvasGroup.alpha = Mathf.Lerp(currentFade, this.fadeUntouched, fadeOut);
				yield return null;
				fadeOut += Time.unscaledDeltaTime * this.fadeOutSpeed;
			}
			if (!this.getButton)
			{
				this.canvasGroup.alpha = this.fadeUntouched;
			}
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

	private Vector2 ConfigureImagePosition(Vector2 textureSize, Vector2 customSpacing)
	{
		Vector2 vector = customSpacing / 100f;
		float num = (float)Screen.width * vector.x - textureSize.x * vector.x;
		float y = (float)Screen.height * vector.y - textureSize.y * vector.y;
		Vector2 result;
		result.x = ((this.anchor != UltimateButton.Anchor.Left) ? ((float)Screen.width - textureSize.x - num) : num);
		result.y = y;
		return result;
	}

	private void ResetButton()
	{
		this.getButton = false;
		this.getButtonDown = false;
		this.getButtonUp = false;
		this._pointerId = -10;
		if (this.useAnimation && this.buttonAnimator != null)
		{
			this.buttonAnimator.SetBool("Touch", false);
		}
		base.StopCoroutine("TensionAccentFade");
		base.StopCoroutine("ButtonFade");
		if (this.useFade)
		{
			this.canvasGroup.alpha = this.fadeUntouched;
		}
		if (this.showTension)
		{
			this.tensionAccent.color = this.tensionColorNone;
		}
		this.isHovering = false;
	}

	public Vector4 GetSpaceButton()
	{
		float num = (float)((this.scalingAxis != UltimateButton.ScalingAxis.Height) ? Screen.width : Screen.height);
		float num2 = num * (this.buttonSize / 10f);
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
		float num = (float)((this.scalingAxis != UltimateButton.ScalingAxis.Height) ? Screen.width : Screen.height);
		float num2 = num * (this.buttonSize / 10f);
		Vector2 result;
		result.y = pos.y / ((float)Screen.height - num2) * 100f;
		result.x = ((this.anchor != UltimateButton.Anchor.Left) ? ((float)Screen.width - num2 - pos.x) : pos.x) / ((float)Screen.width - num2) * 100f;
		return result;
	}

	private void UpdateSizeAndPlacement()
	{
		if (this.sizeFolder == null)
		{
			return;
		}
		float num = (float)((this.scalingAxis != UltimateButton.ScalingAxis.Height) ? Screen.width : Screen.height);
		float num2 = num * (this.buttonSize / 10f);
		if (this.baseTrans == null)
		{
			this.baseTrans = base.GetComponent<RectTransform>();
		}
		Vector2 vector = this.ConfigureImagePosition(new Vector2(num2, num2), new Vector2(this.customSpacing_X, this.customSpacing_Y));
		float d = (this.touchSize != UltimateButton.TouchSize.Large) ? ((this.touchSize != UltimateButton.TouchSize.Medium) ? 1.01f : 1.51f) : 2f;
		Vector2 vector2 = new Vector2(num2, num2);
		this.baseTrans.sizeDelta = vector2 * d;
		this.baseTrans.position = vector - (this.baseTrans.sizeDelta - vector2) / 2f;
		this.sizeFolder.sizeDelta = new Vector2(num2, num2);
		this.sizeFolder.position = vector;
		this.buttonCenter = this.sizeFolder.position;
		this.buttonCenter += new Vector2(this.baseTrans.sizeDelta.x, this.baseTrans.sizeDelta.y) / 2f;
		this._inputRange = this.baseTrans.sizeDelta.x / 2f * this.inputRange;
		if (this.useFade && this.canvasGroup == null)
		{
			this.canvasGroup = this.GetCanvasGroup();
		}
	}

	private void UpdateSizeAndPlacementRelativeToButton()
	{
		if (this.rectTrans == null || this.targetButtonBase == null || this.sizeFolder == null)
		{
			return;
		}
		float num = this.rectTrans.sizeDelta.x * (this.buttonSize / 5f);
		if (this.baseTrans == null)
		{
			this.baseTrans = base.GetComponent<RectTransform>();
		}
		float d = (this.touchSize != UltimateButton.TouchSize.Large) ? ((this.touchSize != UltimateButton.TouchSize.Medium) ? 1.01f : 1.51f) : 2f;
		Vector2 vector = new Vector2(num, num);
		this.baseTrans.sizeDelta = vector * d;
		Vector2 vector2 = this.targetButtonBase.position;
		if (this.anchorSide == UltimateButton.AnchorSide.Top)
		{
			vector2.x -= vector.x / 2f;
			vector2.y += this.rectTrans.sizeDelta.y / 2f;
		}
		else if (this.anchorSide == UltimateButton.AnchorSide.Bottom)
		{
			vector2.x -= vector.x / 2f;
			vector2.y -= this.rectTrans.sizeDelta.y / 2f + vector.y;
		}
		else if (this.anchorSide == UltimateButton.AnchorSide.Right)
		{
			vector2.x += this.rectTrans.sizeDelta.x / 2f;
			vector2.y -= vector.y / 2f;
		}
		else if (this.anchorSide == UltimateButton.AnchorSide.Left)
		{
			vector2.x -= this.rectTrans.sizeDelta.x / 2f + vector.x;
			vector2.y -= vector.y / 2f;
		}
		Vector2 b = default(Vector2);
		float num2 = this.customSpacing_X;
		if (this.anchorSide == UltimateButton.AnchorSide.Left)
		{
			num2 = (num2 - 100f) * -1f;
		}
		float num3 = this.customSpacing_Y;
		if (this.anchorSide == UltimateButton.AnchorSide.Bottom)
		{
			num3 = (num3 - 100f) * -1f;
		}
		b.x = this.rectTrans.sizeDelta.x * ((num2 - 50f) / 100f);
		b.y = this.rectTrans.sizeDelta.y * ((num3 - 50f) / 100f);
		vector2 += b;
		this.baseTrans.position = vector2 - (this.baseTrans.sizeDelta - vector) / 2f;
		this.sizeFolder.sizeDelta = new Vector2(num, num);
		this.sizeFolder.position = vector2;
		this.buttonCenter = this.sizeFolder.position;
		this.buttonCenter += new Vector2(this.baseTrans.sizeDelta.x, this.baseTrans.sizeDelta.y) / 2f;
		this._inputRange = this.baseTrans.sizeDelta.x / 2f * this.inputRange;
	}

	private void UpdateSizeAndPlacementRelativeToJoystick()
	{
		if (this.joystick == null || this.sizeFolder == null)
		{
			return;
		}
		float num = this.joystick.joystickSizeFolder.sizeDelta.x * (this.buttonSize / 5f);
		if (this.baseTrans == null)
		{
			this.baseTrans = base.GetComponent<RectTransform>();
		}
		float d = (this.touchSize != UltimateButton.TouchSize.Large) ? ((this.touchSize != UltimateButton.TouchSize.Medium) ? 1.01f : 1.51f) : 2f;
		Vector2 vector = new Vector2(num, num);
		this.baseTrans.sizeDelta = vector * d;
		Vector2 vector2 = this.joystick.joystick.position;
		if (this.anchorSide == UltimateButton.AnchorSide.Top)
		{
			vector2.x -= vector.x / 2f;
			vector2.y += this.joystick.joystickSizeFolder.sizeDelta.y / 2f;
		}
		else if (this.anchorSide == UltimateButton.AnchorSide.Bottom)
		{
			vector2.x -= vector.x / 2f;
			vector2.y -= this.joystick.joystickSizeFolder.sizeDelta.y / 2f + vector.y;
		}
		else if (this.anchorSide == UltimateButton.AnchorSide.Right)
		{
			vector2.x += this.joystick.joystickSizeFolder.sizeDelta.x / 2f;
			vector2.y -= vector.y / 2f;
		}
		else if (this.anchorSide == UltimateButton.AnchorSide.Left)
		{
			vector2.x -= this.joystick.joystickSizeFolder.sizeDelta.x / 2f + vector.x;
			vector2.y -= vector.y / 2f;
		}
		Vector2 b = default(Vector2);
		float num2 = this.customSpacing_X;
		if (this.anchorSide == UltimateButton.AnchorSide.Left)
		{
			num2 = (num2 - 100f) * -1f;
		}
		float num3 = this.customSpacing_Y;
		if (this.anchorSide == UltimateButton.AnchorSide.Bottom)
		{
			num3 = (num3 - 100f) * -1f;
		}
		b.x = this.joystick.joystickSizeFolder.sizeDelta.x * ((num2 - 50f) / 100f);
		b.y = this.joystick.joystickSizeFolder.sizeDelta.y * ((num3 - 50f) / 100f);
		vector2 += b;
		this.baseTrans.position = vector2 - (this.baseTrans.sizeDelta - vector) / 2f;
		this.sizeFolder.sizeDelta = new Vector2(num, num);
		this.sizeFolder.position = vector2;
		this.buttonCenter = this.sizeFolder.position;
		this.buttonCenter += new Vector2(this.baseTrans.sizeDelta.x, this.baseTrans.sizeDelta.y) / 2f;
		this._inputRange = this.baseTrans.sizeDelta.x / 2f * this.inputRange;
	}

	public void UpdatePositioning()
	{
		if (Application.isPlaying)
		{
			this.ResetButton();
		}
		if (this.positioning == UltimateButton.Positioning.ScreenSpace)
		{
			this.UpdateSizeAndPlacement();
		}
		else if (this.positioning == UltimateButton.Positioning.RelativeToJoystick)
		{
			this.UpdateSizeAndPlacementRelativeToJoystick();
		}
		else if (this.positioning == UltimateButton.Positioning.RelativeToButton)
		{
			this.UpdateSizeAndPlacementRelativeToButton();
		}
	}

	public void UpdateBaseColor(Color targetColor)
	{
		if (this.buttonBase == null)
		{
			return;
		}
		this.baseColor = targetColor;
		this.buttonBase.color = this.baseColor;
	}

	public void UpdateHighlightColor(Color targetColor)
	{
		if (!this.showHighlight)
		{
			return;
		}
		this.highlightColor = targetColor;
		if (this.buttonHighlight != null)
		{
			this.buttonHighlight.color = this.highlightColor;
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

	public void DisableButton()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.getButton = false;
		this.getButtonDown = false;
		this.getButtonUp = false;
		this._pointerId = -10;
		if (this.useAnimation && this.buttonAnimator != null)
		{
			this.buttonAnimator.SetBool("Touch", false);
		}
		base.StopCoroutine("TensionAccentFade");
		base.StopCoroutine("ButtonFade");
		if (this.useFade)
		{
			this.canvasGroup.alpha = this.fadeUntouched;
		}
		if (this.showTension)
		{
			this.tensionAccent.color = this.tensionColorNone;
		}
		this.isHovering = false;
		base.gameObject.SetActive(false);
	}

	public void EnableButton()
	{
		if (base.gameObject.activeInHierarchy)
		{
			return;
		}
		base.gameObject.SetActive(true);
	}

	public static UltimateButton GetUltimateButton(string buttonName)
	{
		if (!UltimateButton.ButtonConfirmed(buttonName))
		{
			return null;
		}
		return UltimateButton.UltimateButtons[buttonName];
	}

	public static bool GetButtonDown(string buttonName)
	{
		return UltimateButton.ButtonConfirmed(buttonName) && UltimateButton.UltimateButtons[buttonName].getButtonDown;
	}

	public static bool GetButton(string buttonName)
	{
		return UltimateButton.ButtonConfirmed(buttonName) && UltimateButton.UltimateButtons[buttonName].getButton;
	}

	public static bool GetButtonUp(string buttonName)
	{
		return UltimateButton.ButtonConfirmed(buttonName) && UltimateButton.UltimateButtons[buttonName].getButtonUp;
	}

	public static bool GetTapCount(string buttonName)
	{
		return UltimateButton.ButtonConfirmed(buttonName) && UltimateButton.UltimateButtons[buttonName].getTapCount;
	}

	public static void DisableButton(string buttonName)
	{
		if (!UltimateButton.ButtonConfirmed(buttonName))
		{
			return;
		}
		UltimateButton.UltimateButtons[buttonName].DisableButton();
	}

	public static void EnableButton(string buttonName)
	{
		if (!UltimateButton.ButtonConfirmed(buttonName))
		{
			return;
		}
		UltimateButton.UltimateButtons[buttonName].EnableButton();
	}

	private static bool ButtonConfirmed(string buttonName)
	{
		if (!UltimateButton.UltimateButtons.ContainsKey(buttonName))
		{
			UnityEngine.Debug.LogWarning("No Ultimate Button has been registered with the name: " + buttonName + ".");
			return false;
		}
		return true;
	}

	private RectTransform baseTrans;

	public RectTransform sizeFolder;

	public Image buttonBase;

	public Image buttonHighlight;

	public Image tensionAccent;

	public Animator buttonAnimator;

	public UltimateButton.Positioning positioning = UltimateButton.Positioning.ScreenSpace;

	public UltimateButton.ScalingAxis scalingAxis = UltimateButton.ScalingAxis.Height;

	public UltimateButton.Anchor anchor = UltimateButton.Anchor.Right;

	public UltimateButton.TouchSize touchSize;

	public UltimateJoystick joystick;

	public RectTransform rectTrans;

	public RectTransform targetButtonBase;

	public UltimateButton.AnchorSide anchorSide;

	public float buttonSize = 1.75f;

	public float customSpacing_X = 5f;

	public float customSpacing_Y = 20f;

	private CanvasGroup canvasGroup;

	public UltimateButton.ImageStyle imageStyle;

	public float inputRange = 1f;

	private float _inputRange = 1f;

	private Vector2 buttonCenter = Vector2.zero;

	private bool isHovering;

	public bool trackInput;

	public bool transmitInput;

	public GameObject receiver;

	private IPointerDownHandler downHandler;

	private IDragHandler dragHandler;

	private IPointerUpHandler upHandler;

	public UltimateButton.TapCountOption tapCountOption;

	public float tapCountDuration = 0.5f;

	public int targetTapCount = 2;

	private float currentTapTime;

	private int tapCount;

	public Color baseColor = Color.white;

	public bool showHighlight;

	public bool showTension;

	public Color highlightColor = Color.white;

	public Color tensionColorNone = Color.white;

	public Color tensionColorFull = Color.white;

	public float tensionFadeInDuration = 1f;

	public float tensionFadeOutDuration = 1f;

	private float tensionFadeInSpeed = 1f;

	private float tensionFadeOutSpeed = 1f;

	public bool useAnimation;

	public bool useFade;

	public float fadeUntouched = 1f;

	public float fadeTouched = 0.5f;

	public float fadeInDuration = 1f;

	public float fadeOutDuration = 1f;

	private float fadeInSpeed = 1f;

	private float fadeOutSpeed = 1f;

	public string buttonName;

	private static Dictionary<string, UltimateButton> UltimateButtons = new Dictionary<string, UltimateButton>();

	private bool getButtonDown;

	private bool getButton;

	private bool getButtonUp;

	private bool getTapCount;

	public UnityEvent onButtonDown;

	public UnityEvent onButtonUp;

	public UnityEvent tapCountEvent;

	private int _pointerId = -10;

	public enum Positioning
	{
		Disabled,
		ScreenSpace,
		RelativeToJoystick,
		RelativeToButton
	}

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

	public enum TouchSize
	{
		Default,
		Medium,
		Large
	}

	public enum AnchorSide
	{
		Top,
		Bottom,
		Left,
		Right
	}

	public enum ImageStyle
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
