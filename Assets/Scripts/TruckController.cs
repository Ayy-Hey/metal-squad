using System;
using System.Collections;
using UnityEngine;

public class TruckController : MonoBehaviour
{
	private void Start()
	{
		this.cameraDefaultPosition = this.cameraTransform.position - base.transform.position;
		this.wheelMotor.maxMotorTorque = 10000f;
		this.resetTimer = this.resetTimerMax;
	}

	private void Update()
	{
		if (UltimateButton.GetButtonDown("Gas") && this.wheelMotor.motorSpeed > -250f)
		{
			this.wheelMotor.motorSpeed = -250f;
		}
	}

	private void FixedUpdate()
	{
		this.isGrounded = Physics2D.Linecast(base.transform.position, this.groundCheck.position, 1 << LayerMask.NameToLayer("Default"));
		if (UltimateButton.GetButton("Gas"))
		{
			if (this.wheelMotor.motorSpeed > (float)(-(float)this.maxSpeed))
			{
				this.wheelMotor.motorSpeed = this.wheelMotor.motorSpeed - Time.deltaTime * this.accelerationSpeed;
			}
			if (this.isGrounded)
			{
				this.myRigidbody.AddForceAtPosition(new Vector2(20f, 0f), new Vector2(base.transform.position.x, base.transform.position.y - 1f));
			}
		}
		else if (UltimateButton.GetButton("Reverse") && this.wheelMotor.motorSpeed >= 0f)
		{
			if (this.wheelMotor.motorSpeed != (float)this.reverseSpeed)
			{
				this.wheelMotor.motorSpeed = (float)this.reverseSpeed;
			}
			if (this.isGrounded)
			{
				this.myRigidbody.AddForceAtPosition(new Vector2(-10f, 0f), new Vector2(base.transform.position.x, base.transform.position.y - 1f));
			}
		}
		else
		{
			if (this.wheelMotor.motorSpeed < 0f)
			{
				this.wheelMotor.motorSpeed = this.wheelMotor.motorSpeed + Time.deltaTime * this.decelerationSpeed;
			}
			else if (this.wheelMotor.motorSpeed > 0f && !UltimateButton.GetButton("Reverse"))
			{
				this.wheelMotor.motorSpeed = 0f;
			}
			if (this.isGrounded && this.wheelMotor.motorSpeed > -100f && this.myRigidbody.velocity.x > 2f)
			{
				this.myRigidbody.AddForceAtPosition(new Vector2(-30f, 0f), new Vector2(base.transform.position.x, base.transform.position.y - 1f));
			}
		}
		this.rearWheel.motor = this.wheelMotor;
		this.frontWheel.motor = this.wheelMotor;
		this.cameraTransform.position = base.transform.position + this.cameraDefaultPosition;
		if (!this.isGrounded && this.myRigidbody.velocity.magnitude <= 0.5f && !this.isResetting)
		{
			this.resetTimer -= Time.deltaTime;
			if (this.resetTimer <= 0f)
			{
				this.resetTimer = this.resetTimerMax;
				base.StartCoroutine("ResetTruck");
			}
		}
		else if (this.isGrounded && this.resetTimer != this.resetTimerMax)
		{
			this.resetTimer = this.resetTimerMax;
		}
	}

	private IEnumerator ResetTruck()
	{
		this.isResetting = true;
		this.myRigidbody.isKinematic = true;
		Vector2 originalPos = base.transform.position;
		Vector2 endPos = new Vector2(base.transform.position.x, base.transform.position.y + 3f);
		Quaternion originalRot = base.transform.rotation;
		for (float t = 0f; t < 1f; t += Time.deltaTime * 1.5f)
		{
			base.transform.position = Vector2.Lerp(originalPos, endPos, t);
			base.transform.rotation = Quaternion.Slerp(originalRot, Quaternion.identity, t);
			yield return new WaitForFixedUpdate();
		}
		this.backRigidbody.velocity = Vector2.zero;
		this.frontRigidbody.velocity = Vector2.zero;
		this.myRigidbody.isKinematic = false;
		this.isResetting = false;
		yield break;
	}

	[Header("Assigned Variables")]
	public Transform cameraTransform;

	public Rigidbody2D myRigidbody;

	public WheelJoint2D frontWheel;

	public WheelJoint2D rearWheel;

	public Rigidbody2D frontRigidbody;

	public Rigidbody2D backRigidbody;

	public Transform groundCheck;

	[Header("Speeds and Times")]
	public int maxSpeed = 1200;

	public int reverseSpeed = 250;

	public float accelerationSpeed = 10f;

	public float decelerationSpeed = 5f;

	public float resetTimerMax = 2.5f;

	private JointMotor2D wheelMotor;

	private Vector3 cameraDefaultPosition;

	private float resetTimer;

	private bool isResetting;

	private bool isGrounded;
}
