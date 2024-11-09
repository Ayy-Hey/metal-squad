using System;
using UnityEngine;

namespace UltimateJoystickExample.Spaceship
{
	public class PlayerController : MonoBehaviour
	{
		private void Start()
		{
			this.myRigidbody = base.GetComponent<Rigidbody>();
		}

		private void Update()
		{
			this.movePosition = new Vector3(UltimateJoystick.GetHorizontalAxis("Movement"), UltimateJoystick.GetVerticalAxis("Movement"), 0f);
			this.shootPosition = new Vector3(UltimateJoystick.GetHorizontalAxis("Shooting"), UltimateJoystick.GetVerticalAxis("Shooting"), 0f);
			if (!this.canControl)
			{
				return;
			}
			if (UltimateJoystick.GetJoystickState("Shooting") && this.shootingTimer <= 0f)
			{
				this.shootingTimer = this.shootingCooldown;
				this.CreateBullets();
			}
			if (this.shootingTimer > 0f)
			{
				this.shootingTimer -= Time.deltaTime;
			}
		}

		private void FixedUpdate()
		{
			if (!this.canControl)
			{
				this.myRigidbody.rotation = Quaternion.identity;
				this.myRigidbody.position = Vector3.zero;
				this.myRigidbody.velocity = Vector3.zero;
				this.myRigidbody.angularVelocity = Vector3.zero;
			}
			else
			{
				Vector3 b = new Vector3(this.movePosition.x, 0f, this.movePosition.y);
				base.transform.LookAt(base.transform.position + b);
				Vector3 b2 = new Vector3(this.shootPosition.x, 0f, this.shootPosition.y);
				this.gunTrans.LookAt(this.gunTrans.position + b2);
				this.myRigidbody.AddForce(base.transform.forward * UltimateJoystick.GetDistance("Movement") * 1000f * this.accelerationSpeed * Time.deltaTime);
				if (this.myRigidbody.velocity.magnitude > this.maxSpeed)
				{
					this.myRigidbody.velocity = this.myRigidbody.velocity.normalized * this.maxSpeed;
				}
				this.CheckExitScreen();
			}
		}

		private void CheckExitScreen()
		{
			if (Camera.main == null)
			{
				return;
			}
			if (Mathf.Abs(this.myRigidbody.position.x) > Camera.main.orthographicSize * Camera.main.aspect)
			{
				this.myRigidbody.position = new Vector3(-Mathf.Sign(this.myRigidbody.position.x) * Camera.main.orthographicSize * Camera.main.aspect, 0f, this.myRigidbody.position.z);
			}
			if (Mathf.Abs(this.myRigidbody.position.z) > Camera.main.orthographicSize)
			{
				this.myRigidbody.position = new Vector3(this.myRigidbody.position.x, this.myRigidbody.position.y, -Mathf.Sign(this.myRigidbody.position.z) * Camera.main.orthographicSize);
			}
		}

		private void CreateBullets()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, this.bulletSpawnPos.position, this.bulletSpawnPos.rotation);
			gameObject.name = this.bulletPrefab.name;
			gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 200f;
			UnityEngine.Object.Destroy(gameObject, 3f);
		}

		[Header("Speeds")]
		public float rotationSpeed = 45f;

		public float accelerationSpeed = 2f;

		public float maxSpeed = 3f;

		public float shootingCooldown = 0.2f;

		[Header("Assigned Variables")]
		public GameObject bulletPrefab;

		private Rigidbody myRigidbody;

		public Transform gunTrans;

		public Transform bulletSpawnPos;

		public GameObject playerVisuals;

		private float shootingTimer;

		private bool canControl = true;

		private Vector3 movePosition;

		private Vector3 shootPosition;
	}
}
