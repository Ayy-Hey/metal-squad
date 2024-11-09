using System;
using UnityEngine;

[Serializable]
public class ETCAxis
{
	public ETCAxis(string axisName)
	{
		this.name = axisName;
		this.enable = true;
		this.speed = 15f;
		this.invertedAxis = false;
		this.isEnertia = false;
		this.inertia = 0f;
		this.inertiaThreshold = 0.08f;
		this.axisValue = 0f;
		this.axisSpeedValue = 0f;
		this.gravity = 0f;
		this.isAutoStab = false;
		this.autoStabThreshold = 0.01f;
		this.autoStabSpeed = 10f;
		this.maxAngle = 90f;
		this.minAngle = 90f;
		this.axisState = ETCAxis.AxisState.None;
		this.maxOverTimeValue = 1f;
		this.overTimeStep = 1f;
		this.isValueOverTime = false;
		this.axisThreshold = 0.5f;
		this.deadValue = 0.1f;
		this.actionOn = ETCAxis.ActionOn.Press;
	}

	public Transform directTransform
	{
		get
		{
			return this._directTransform;
		}
		set
		{
			this._directTransform = value;
			if (this._directTransform != null)
			{
				this.directCharacterController = this._directTransform.GetComponent<CharacterController>();
				this.directRigidBody = this._directTransform.GetComponent<Rigidbody>();
			}
			else
			{
				this.directCharacterController = null;
			}
		}
	}

	public void InitAxis()
	{
		if (this.autoLinkTagPlayer)
		{
			this.player = GameObject.FindGameObjectWithTag(this.autoTag);
			if (this.player)
			{
				this.directTransform = this.player.transform;
			}
		}
		this.startAngle = this.GetAngle();
	}

	public void UpdateAxis(float realValue, bool isOnDrag, ETCBase.ControlType type, bool deltaTime = true)
	{
		if ((this.autoLinkTagPlayer && this.player == null) || (this.player && !this.player.activeSelf))
		{
			this.player = GameObject.FindGameObjectWithTag(this.autoTag);
			if (this.player)
			{
				this.directTransform = this.player.transform;
			}
		}
		if (this.isAutoStab && this.axisValue == 0f && this._directTransform)
		{
			this.DoAutoStabilisation();
		}
		if (this.invertedAxis)
		{
			realValue *= -1f;
		}
		if (this.isValueOverTime && realValue != 0f)
		{
			this.axisValue += this.overTimeStep * Mathf.Sign(realValue) * Time.deltaTime;
			if (Mathf.Sign(this.axisValue) > 0f)
			{
				this.axisValue = Mathf.Clamp(this.axisValue, 0f, this.maxOverTimeValue);
			}
			else
			{
				this.axisValue = Mathf.Clamp(this.axisValue, -this.maxOverTimeValue, 0f);
			}
		}
		this.ComputAxisValue(realValue, type, isOnDrag, deltaTime);
	}

	public void UpdateButton()
	{
		if ((this.autoLinkTagPlayer && this.player == null) || (this.player && !this.player.activeSelf))
		{
			this.player = GameObject.FindGameObjectWithTag(this.autoTag);
			if (this.player)
			{
				this.directTransform = this.player.transform;
			}
		}
		if (this.isValueOverTime)
		{
			this.axisValue += this.overTimeStep * Time.deltaTime;
			this.axisValue = Mathf.Clamp(this.axisValue, 0f, this.maxOverTimeValue);
		}
		else if (this.axisState == ETCAxis.AxisState.Press || this.axisState == ETCAxis.AxisState.Down)
		{
			this.axisValue = 1f;
		}
		else
		{
			this.axisValue = 0f;
		}
		ETCAxis.ActionOn actionOn = this.actionOn;
		if (actionOn != ETCAxis.ActionOn.Down)
		{
			if (actionOn == ETCAxis.ActionOn.Press)
			{
				this.axisSpeedValue = this.axisValue * this.speed * Time.deltaTime;
				if (this.axisState == ETCAxis.AxisState.Press)
				{
					this.DoDirectAction();
				}
			}
		}
		else
		{
			this.axisSpeedValue = this.axisValue * this.speed;
			if (this.axisState == ETCAxis.AxisState.Down)
			{
				this.DoDirectAction();
			}
		}
	}

	public void ResetAxis()
	{
		if (!this.isEnertia || (this.isEnertia && Mathf.Abs(this.axisValue) < this.inertiaThreshold))
		{
			this.axisValue = 0f;
			this.axisSpeedValue = 0f;
		}
	}

	public void DoDirectAction()
	{
		if (this.directTransform)
		{
			Vector3 influencedAxis = this.GetInfluencedAxis();
			switch (this.directAction)
			{
			case ETCAxis.DirectAction.Rotate:
				this.directTransform.Rotate(influencedAxis * this.axisSpeedValue, Space.World);
				break;
			case ETCAxis.DirectAction.RotateLocal:
				this.directTransform.Rotate(influencedAxis * this.axisSpeedValue, Space.Self);
				break;
			case ETCAxis.DirectAction.Translate:
				if (this.directCharacterController == null)
				{
					this.directTransform.Translate(influencedAxis * this.axisSpeedValue, Space.World);
				}
				else if (this.directCharacterController.isGrounded || !this.isLockinJump)
				{
					Vector3 motion = influencedAxis * this.axisSpeedValue;
					this.directCharacterController.Move(motion);
					this.lastMove = influencedAxis * (this.axisSpeedValue / Time.deltaTime);
				}
				else
				{
					this.directCharacterController.Move(this.lastMove * Time.deltaTime);
				}
				break;
			case ETCAxis.DirectAction.TranslateLocal:
				if (this.directCharacterController == null)
				{
					this.directTransform.Translate(influencedAxis * this.axisSpeedValue, Space.Self);
				}
				else if (this.directCharacterController.isGrounded || !this.isLockinJump)
				{
					Vector3 motion2 = this.directCharacterController.transform.TransformDirection(influencedAxis) * this.axisSpeedValue;
					this.directCharacterController.Move(motion2);
					this.lastMove = this.directCharacterController.transform.TransformDirection(influencedAxis) * (this.axisSpeedValue / Time.deltaTime);
				}
				else
				{
					this.directCharacterController.Move(this.lastMove * Time.deltaTime);
				}
				break;
			case ETCAxis.DirectAction.Scale:
				this.directTransform.localScale += influencedAxis * this.axisSpeedValue;
				break;
			case ETCAxis.DirectAction.Force:
				if (this.directRigidBody != null)
				{
					this.directRigidBody.AddForce(influencedAxis * this.axisValue * this.speed);
				}
				else
				{
					UnityEngine.Debug.LogWarning("ETCAxis : " + this.name + " No rigidbody on gameobject : " + this._directTransform.name);
				}
				break;
			case ETCAxis.DirectAction.RelativeForce:
				if (this.directRigidBody != null)
				{
					this.directRigidBody.AddRelativeForce(influencedAxis * this.axisValue * this.speed);
				}
				else
				{
					UnityEngine.Debug.LogWarning("ETCAxis : " + this.name + " No rigidbody on gameobject : " + this._directTransform.name);
				}
				break;
			case ETCAxis.DirectAction.Torque:
				if (this.directRigidBody != null)
				{
					this.directRigidBody.AddTorque(influencedAxis * this.axisValue * this.speed);
				}
				else
				{
					UnityEngine.Debug.LogWarning("ETCAxis : " + this.name + " No rigidbody on gameobject : " + this._directTransform.name);
				}
				break;
			case ETCAxis.DirectAction.RelativeTorque:
				if (this.directRigidBody != null)
				{
					this.directRigidBody.AddRelativeTorque(influencedAxis * this.axisValue * this.speed);
				}
				else
				{
					UnityEngine.Debug.LogWarning("ETCAxis : " + this.name + " No rigidbody on gameobject : " + this._directTransform.name);
				}
				break;
			case ETCAxis.DirectAction.Jump:
				if (this.directCharacterController != null && !this.isJump)
				{
					this.isJump = true;
					this.currentGravity = this.speed;
				}
				break;
			}
			if (this.isClampRotation && this.directAction == ETCAxis.DirectAction.RotateLocal)
			{
				this.DoAngleLimitation();
			}
		}
	}

	public void DoGravity()
	{
		if (this.directCharacterController != null && this.gravity != 0f)
		{
			if (!this.isJump)
			{
				Vector3 a = new Vector3(0f, -this.gravity, 0f);
				this.directCharacterController.Move(a * Time.deltaTime);
			}
			else
			{
				this.currentGravity -= this.gravity * Time.deltaTime;
				Vector3 a2 = new Vector3(0f, this.currentGravity, 0f);
				this.directCharacterController.Move(a2 * Time.deltaTime);
			}
			if (this.directCharacterController.isGrounded)
			{
				this.isJump = false;
				this.currentGravity = 0f;
			}
		}
	}

	private void ComputAxisValue(float realValue, ETCBase.ControlType type, bool isOnDrag, bool deltaTime)
	{
		if (this.enable)
		{
			if (type == ETCBase.ControlType.Joystick)
			{
				if (this.valueMethod == ETCAxis.AxisValueMethod.Classical)
				{
					float num = Mathf.Max(Mathf.Abs(realValue), 0.001f);
					float num2 = Mathf.Max(num - this.deadValue, 0f) / (1f - this.deadValue) / num;
					realValue *= num2;
				}
				else
				{
					realValue = this.curveValue.Evaluate(realValue);
				}
			}
			if (this.isEnertia)
			{
				realValue -= this.axisValue;
				realValue /= this.inertia;
				this.axisValue += realValue;
				if (Mathf.Abs(this.axisValue) < this.inertiaThreshold && !isOnDrag)
				{
					this.axisValue = 0f;
				}
			}
			else if (!this.isValueOverTime || (this.isValueOverTime && realValue == 0f))
			{
				this.axisValue = realValue;
			}
			if (deltaTime)
			{
				this.axisSpeedValue = this.axisValue * this.speed * Time.deltaTime;
			}
			else
			{
				this.axisSpeedValue = this.axisValue * this.speed;
			}
		}
		else
		{
			this.axisValue = 0f;
			this.axisSpeedValue = 0f;
		}
	}

	private Vector3 GetInfluencedAxis()
	{
		Vector3 result = Vector3.zero;
		ETCAxis.AxisInfluenced axisInfluenced = this.axisInfluenced;
		if (axisInfluenced != ETCAxis.AxisInfluenced.X)
		{
			if (axisInfluenced != ETCAxis.AxisInfluenced.Y)
			{
				if (axisInfluenced == ETCAxis.AxisInfluenced.Z)
				{
					result = Vector3.forward;
				}
			}
			else
			{
				result = Vector3.up;
			}
		}
		else
		{
			result = Vector3.right;
		}
		return result;
	}

	private float GetAngle()
	{
		float num = 0f;
		if (this._directTransform != null)
		{
			ETCAxis.AxisInfluenced axisInfluenced = this.axisInfluenced;
			if (axisInfluenced != ETCAxis.AxisInfluenced.X)
			{
				if (axisInfluenced != ETCAxis.AxisInfluenced.Y)
				{
					if (axisInfluenced == ETCAxis.AxisInfluenced.Z)
					{
						num = this._directTransform.localRotation.eulerAngles.z;
					}
				}
				else
				{
					num = this._directTransform.localRotation.eulerAngles.y;
				}
			}
			else
			{
				num = this._directTransform.localRotation.eulerAngles.x;
			}
			if (num <= 360f && num >= 180f)
			{
				num -= 360f;
			}
		}
		return num;
	}

	private void DoAutoStabilisation()
	{
		float num = this.GetAngle();
		if (num <= 360f && num >= 180f)
		{
			num -= 360f;
		}
		if (num > this.startAngle - this.autoStabThreshold || num < this.startAngle + this.autoStabThreshold)
		{
			float num2 = 0f;
			Vector3 zero = Vector3.zero;
			if (num > this.startAngle - this.autoStabThreshold)
			{
				num2 = num + this.autoStabSpeed / 100f * Mathf.Abs(num - this.startAngle) * Time.deltaTime * -1f;
			}
			if (num < this.startAngle + this.autoStabThreshold)
			{
				num2 = num + this.autoStabSpeed / 100f * Mathf.Abs(num - this.startAngle) * Time.deltaTime;
			}
			ETCAxis.AxisInfluenced axisInfluenced = this.axisInfluenced;
			if (axisInfluenced != ETCAxis.AxisInfluenced.X)
			{
				if (axisInfluenced != ETCAxis.AxisInfluenced.Y)
				{
					if (axisInfluenced == ETCAxis.AxisInfluenced.Z)
					{
						zero = new Vector3(this._directTransform.localRotation.eulerAngles.x, this._directTransform.localRotation.eulerAngles.y, num2);
					}
				}
				else
				{
					zero = new Vector3(this._directTransform.localRotation.eulerAngles.x, num2, this._directTransform.localRotation.eulerAngles.z);
				}
			}
			else
			{
				zero = new Vector3(num2, this._directTransform.localRotation.eulerAngles.y, this._directTransform.localRotation.eulerAngles.z);
			}
			this._directTransform.localRotation = Quaternion.Euler(zero);
		}
	}

	private void DoAngleLimitation()
	{
		Quaternion localRotation = this._directTransform.localRotation;
		localRotation.x /= localRotation.w;
		localRotation.y /= localRotation.w;
		localRotation.z /= localRotation.w;
		localRotation.w = 1f;
		ETCAxis.AxisInfluenced axisInfluenced = this.axisInfluenced;
		if (axisInfluenced != ETCAxis.AxisInfluenced.X)
		{
			if (axisInfluenced != ETCAxis.AxisInfluenced.Y)
			{
				if (axisInfluenced == ETCAxis.AxisInfluenced.Z)
				{
					float num = 114.59156f * Mathf.Atan(localRotation.z);
					num = Mathf.Clamp(num, -this.minAngle, this.maxAngle);
					localRotation.z = Mathf.Tan(0.008726646f * num);
				}
			}
			else
			{
				float num = 114.59156f * Mathf.Atan(localRotation.y);
				num = Mathf.Clamp(num, -this.minAngle, this.maxAngle);
				localRotation.y = Mathf.Tan(0.008726646f * num);
			}
		}
		else
		{
			float num = 114.59156f * Mathf.Atan(localRotation.x);
			num = Mathf.Clamp(num, -this.minAngle, this.maxAngle);
			localRotation.x = Mathf.Tan(0.008726646f * num);
		}
		this._directTransform.localRotation = localRotation;
	}

	public void InitDeadCurve()
	{
		this.curveValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
		this.curveValue.postWrapMode = WrapMode.PingPong;
		this.curveValue.preWrapMode = WrapMode.PingPong;
	}

	public string name;

	public bool autoLinkTagPlayer;

	public string autoTag = "Player";

	public GameObject player;

	public bool enable;

	public bool invertedAxis;

	public float speed;

	public float deadValue;

	public ETCAxis.AxisValueMethod valueMethod;

	public AnimationCurve curveValue;

	public bool isEnertia;

	public float inertia;

	public float inertiaThreshold;

	public bool isAutoStab;

	public float autoStabThreshold;

	public float autoStabSpeed;

	private float startAngle;

	public bool isClampRotation;

	public float maxAngle;

	public float minAngle;

	public bool isValueOverTime;

	public float overTimeStep;

	public float maxOverTimeValue;

	public float axisValue;

	public float axisSpeedValue;

	public float axisThreshold;

	public bool isLockinJump;

	private Vector3 lastMove;

	public ETCAxis.AxisState axisState;

	[SerializeField]
	private Transform _directTransform;

	public ETCAxis.DirectAction directAction;

	public ETCAxis.AxisInfluenced axisInfluenced;

	public ETCAxis.ActionOn actionOn;

	public CharacterController directCharacterController;

	public Rigidbody directRigidBody;

	public float gravity;

	public float currentGravity;

	public bool isJump;

	public string unityAxis;

	public bool showGeneralInspector;

	public bool showDirectInspector;

	public bool showInertiaInspector;

	public bool showSimulatinInspector;

	public enum DirectAction
	{
		Rotate,
		RotateLocal,
		Translate,
		TranslateLocal,
		Scale,
		Force,
		RelativeForce,
		Torque,
		RelativeTorque,
		Jump
	}

	public enum AxisInfluenced
	{
		X,
		Y,
		Z
	}

	public enum AxisValueMethod
	{
		Classical,
		Curve
	}

	public enum AxisState
	{
		None,
		Down,
		Press,
		Up,
		DownUp,
		DownDown,
		DownLeft,
		DownRight,
		PressUp,
		PressDown,
		PressLeft,
		PressRight
	}

	public enum ActionOn
	{
		Down,
		Press
	}
}
