using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ETCInput : MonoBehaviour
{
	public static ETCInput instance
	{
		get
		{
			if (!ETCInput._instance)
			{
				ETCInput._instance = (UnityEngine.Object.FindObjectOfType(typeof(ETCInput)) as ETCInput);
				if (!ETCInput._instance)
				{
					GameObject gameObject = new GameObject("InputManager");
					ETCInput._instance = gameObject.AddComponent<ETCInput>();
				}
			}
			return ETCInput._instance;
		}
	}

	public void RegisterControl(ETCBase ctrl)
	{
		if (this.controls.ContainsKey(ctrl.name))
		{
			UnityEngine.Debug.LogWarning("ETCInput control : " + ctrl.name + " already exists");
		}
		else
		{
			this.controls.Add(ctrl.name, ctrl);
			if (ctrl.GetType() == typeof(ETCJoystick))
			{
				this.RegisterAxis((ctrl as ETCJoystick).axisX);
				this.RegisterAxis((ctrl as ETCJoystick).axisY);
			}
			else if (ctrl.GetType() == typeof(ETCTouchPad))
			{
				this.RegisterAxis((ctrl as ETCTouchPad).axisX);
				this.RegisterAxis((ctrl as ETCTouchPad).axisY);
			}
			else if (ctrl.GetType() == typeof(ETCDPad))
			{
				this.RegisterAxis((ctrl as ETCDPad).axisX);
				this.RegisterAxis((ctrl as ETCDPad).axisY);
			}
			else if (ctrl.GetType() == typeof(ETCButton))
			{
				this.RegisterAxis((ctrl as ETCButton).axis);
			}
		}
	}

	public void UnRegisterControl(ETCBase ctrl)
	{
		if (this.controls.ContainsKey(ctrl.name) && ctrl.enabled)
		{
			this.controls.Remove(ctrl.name);
			if (ctrl.GetType() == typeof(ETCJoystick))
			{
				this.UnRegisterAxis((ctrl as ETCJoystick).axisX);
				this.UnRegisterAxis((ctrl as ETCJoystick).axisY);
			}
			else if (ctrl.GetType() == typeof(ETCTouchPad))
			{
				this.UnRegisterAxis((ctrl as ETCTouchPad).axisX);
				this.UnRegisterAxis((ctrl as ETCTouchPad).axisY);
			}
			else if (ctrl.GetType() == typeof(ETCDPad))
			{
				this.UnRegisterAxis((ctrl as ETCDPad).axisX);
				this.UnRegisterAxis((ctrl as ETCDPad).axisY);
			}
			else if (ctrl.GetType() == typeof(ETCButton))
			{
				this.UnRegisterAxis((ctrl as ETCButton).axis);
			}
		}
	}

	public void Create()
	{
	}

	public static void Register(ETCBase ctrl)
	{
		ETCInput.instance.RegisterControl(ctrl);
	}

	public static void UnRegister(ETCBase ctrl)
	{
		ETCInput.instance.UnRegisterControl(ctrl);
	}

	public static void SetControlVisible(string ctrlName, bool value)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			ETCInput.control.visible = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		}
	}

	public static bool GetControlVisible(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			return ETCInput.control.visible;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		return false;
	}

	public static void SetControlActivated(string ctrlName, bool value)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			ETCInput.control.activated = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		}
	}

	public static bool GetControlActivated(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			return ETCInput.control.activated;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		return false;
	}

	public static void SetControlSwipeIn(string ctrlName, bool value)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			ETCInput.control.isSwipeIn = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		}
	}

	public static bool GetControlSwipeIn(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			return ETCInput.control.isSwipeIn;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		return false;
	}

	public static void SetControlSwipeOut(string ctrlName, bool value)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			ETCInput.control.isSwipeOut = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		}
	}

	public static bool GetControlSwipeOut(string ctrlName, bool value)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			return ETCInput.control.isSwipeOut;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		return false;
	}

	public static void SetDPadAxesCount(string ctrlName, ETCBase.DPadAxis value)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			ETCInput.control.dPadAxisCount = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		}
	}

	public static ETCBase.DPadAxis GetDPadAxesCount(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			return ETCInput.control.dPadAxisCount;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + ctrlName + " doesn't exist");
		return ETCBase.DPadAxis.Two_Axis;
	}

	public static ETCJoystick GetControlJoystick(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control) && ETCInput.control.GetType() == typeof(ETCJoystick))
		{
			return (ETCJoystick)ETCInput.control;
		}
		return null;
	}

	public static ETCDPad GetControlDPad(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control) && ETCInput.control.GetType() == typeof(ETCDPad))
		{
			return (ETCDPad)ETCInput.control;
		}
		return null;
	}

	public static ETCTouchPad GetControlTouchPad(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control) && ETCInput.control.GetType() == typeof(ETCTouchPad))
		{
			return (ETCTouchPad)ETCInput.control;
		}
		return null;
	}

	public static ETCButton GetControlButton(string ctrlName)
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control) && ETCInput.control.GetType() == typeof(ETCJoystick))
		{
			return (ETCButton)ETCInput.control;
		}
		return null;
	}

	public static void SetControlSprite(string ctrlName, Sprite spr, Color color = default(Color))
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control))
		{
			Image component = ETCInput.control.GetComponent<Image>();
			if (component)
			{
				component.sprite = spr;
				component.color = color;
			}
		}
	}

	public static void SetJoystickThumbSprite(string ctrlName, Sprite spr, Color color = default(Color))
	{
		if (ETCInput.instance.controls.TryGetValue(ctrlName, out ETCInput.control) && ETCInput.control.GetType() == typeof(ETCJoystick))
		{
			ETCJoystick etcjoystick = (ETCJoystick)ETCInput.control;
			if (etcjoystick)
			{
				Image component = etcjoystick.thumb.GetComponent<Image>();
				if (component)
				{
					component.sprite = spr;
					component.color = color;
				}
			}
		}
	}

	public static void SetButtonPressedSprite(string ctrlName, Sprite spr, Color color = default(Color))
	{
	}

	public static void SetAxisSpeed(string axisName, float speed)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.speed = speed;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static void SetAxisGravity(string axisName, float gravity)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.gravity = gravity;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static void SetTurnMoveSpeed(string ctrlName, float speed)
	{
		ETCJoystick controlJoystick = ETCInput.GetControlJoystick(ctrlName);
		if (controlJoystick)
		{
			controlJoystick.tmSpeed = speed;
		}
	}

	public static void ResetAxis(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.axisValue = 0f;
			ETCInput.axis.axisSpeedValue = 0f;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static void SetAxisEnabled(string axisName, bool value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.enable = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static bool GetAxisEnabled(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.enable;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return false;
	}

	public static void SetAxisInverted(string axisName, bool value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.invertedAxis = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static bool GetAxisInverted(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.invertedAxis;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return false;
	}

	public static void SetAxisDeadValue(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.deadValue = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisDeadValue(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.deadValue;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisSensitivity(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.speed = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisSensitivity(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.speed;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisThreshold(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.axisThreshold = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisThreshold(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisThreshold;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisInertia(string axisName, bool value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.isEnertia = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static bool GetAxisInertia(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.isEnertia;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return false;
	}

	public static void SetAxisInertiaSpeed(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.inertia = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisInertiaSpeed(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.inertia;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisInertiaThreshold(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.inertiaThreshold = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisInertiaThreshold(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.inertiaThreshold;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisAutoStabilization(string axisName, bool value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.isAutoStab = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static bool GetAxisAutoStabilization(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.isAutoStab;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return false;
	}

	public static void SetAxisAutoStabilizationSpeed(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.autoStabSpeed = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisAutoStabilizationSpeed(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.autoStabSpeed;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisAutoStabilizationThreshold(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.autoStabThreshold = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisAutoStabilizationThreshold(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.autoStabThreshold;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisClampRotation(string axisName, bool value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.isClampRotation = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static bool GetAxisClampRotation(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.isClampRotation;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return false;
	}

	public static void SetAxisClampRotationValue(string axisName, float min, float max)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.minAngle = min;
			ETCInput.axis.maxAngle = max;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static void SetAxisClampRotationMinValue(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.minAngle = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static void SetAxisClampRotationMaxValue(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.maxAngle = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisClampRotationMinValue(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.minAngle;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static float GetAxisClampRotationMaxValue(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.maxAngle;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisDirecTransform(string axisName, Transform value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.directTransform = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static Transform GetAxisDirectTransform(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.directTransform;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return null;
	}

	public static void SetAxisDirectAction(string axisName, ETCAxis.DirectAction value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.directAction = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static ETCAxis.DirectAction GetAxisDirectAction(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.directAction;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return ETCAxis.DirectAction.Rotate;
	}

	public static void SetAxisAffectedAxis(string axisName, ETCAxis.AxisInfluenced value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.axisInfluenced = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static ETCAxis.AxisInfluenced GetAxisAffectedAxis(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisInfluenced;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return ETCAxis.AxisInfluenced.X;
	}

	public static void SetAxisOverTime(string axisName, bool value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.isValueOverTime = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static bool GetAxisOverTime(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.isValueOverTime;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return false;
	}

	public static void SetAxisOverTimeStep(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.overTimeStep = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisOverTimeStep(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.overTimeStep;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static void SetAxisOverTimeMaxValue(string axisName, float value)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			ETCInput.axis.maxOverTimeValue = value;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		}
	}

	public static float GetAxisOverTimeMaxValue(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.maxOverTimeValue;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return -1f;
	}

	public static float GetAxis(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisValue;
		}
		UnityEngine.Debug.LogWarning("ETCInput : " + axisName + " doesn't exist");
		return 0f;
	}

	public static float GetAxisSpeed(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisSpeedValue;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return 0f;
	}

	public static bool GetAxisDownUp(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.DownUp;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisDownDown(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.DownDown;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisDownRight(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.DownRight;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisDownLeft(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.DownLeft;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisPressedUp(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.PressUp;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisPressedDown(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.PressDown;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisPressedRight(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.PressRight;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetAxisPressedLeft(string axisName)
	{
		if (ETCInput.instance.axes.TryGetValue(axisName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.PressLeft;
		}
		UnityEngine.Debug.LogWarning(axisName + " doesn't exist");
		return false;
	}

	public static bool GetButtonDown(string buttonName)
	{
		if (ETCInput.instance.axes.TryGetValue(buttonName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.Down;
		}
		UnityEngine.Debug.LogWarning(buttonName + " doesn't exist");
		return false;
	}

	public static bool GetButton(string buttonName)
	{
		if (ETCInput.instance.axes.TryGetValue(buttonName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.Down || ETCInput.axis.axisState == ETCAxis.AxisState.Press;
		}
		UnityEngine.Debug.LogWarning(buttonName + " doesn't exist");
		return false;
	}

	public static bool GetButtonUp(string buttonName)
	{
		if (ETCInput.instance.axes.TryGetValue(buttonName, out ETCInput.axis))
		{
			return ETCInput.axis.axisState == ETCAxis.AxisState.Up;
		}
		UnityEngine.Debug.LogWarning(buttonName + " doesn't exist");
		return false;
	}

	public static float GetButtonValue(string buttonName)
	{
		if (ETCInput.instance.axes.TryGetValue(buttonName, out ETCInput.axis))
		{
			return ETCInput.axis.axisValue;
		}
		UnityEngine.Debug.LogWarning(buttonName + " doesn't exist");
		return -1f;
	}

	private void RegisterAxis(ETCAxis axis)
	{
		if (ETCInput.instance.axes.ContainsKey(axis.name))
		{
			UnityEngine.Debug.LogWarning("ETCInput axis : " + axis.name + " already exists");
		}
		else
		{
			this.axes.Add(axis.name, axis);
		}
	}

	private void UnRegisterAxis(ETCAxis axis)
	{
		if (ETCInput.instance.axes.ContainsKey(axis.name))
		{
			this.axes.Remove(axis.name);
		}
	}

	public static ETCInput _instance;

	private Dictionary<string, ETCAxis> axes = new Dictionary<string, ETCAxis>();

	private Dictionary<string, ETCBase> controls = new Dictionary<string, ETCBase>();

	private static ETCBase control;

	private static ETCAxis axis;
}
