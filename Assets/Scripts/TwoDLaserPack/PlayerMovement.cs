using System;
using UnityEngine;

namespace TwoDLaserPack
{
	public class PlayerMovement : MonoBehaviour
	{
		private void Start()
		{
			if (base.gameObject.GetComponent<Animator>() != null)
			{
				this.playerAnimator = base.gameObject.GetComponent<Animator>();
			}
		}

		private void moveForward(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y + amount * Time.deltaTime, base.transform.position.z);
			base.transform.position = position;
		}

		private void moveBack(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y - amount * Time.deltaTime, base.transform.position.z);
			base.transform.position = position;
		}

		private void moveRight(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x + amount * Time.deltaTime, base.transform.position.y, base.transform.position.z);
			base.transform.position = position;
		}

		private void moveLeft(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x - amount * Time.deltaTime, base.transform.position.y, base.transform.position.z);
			base.transform.position = position;
		}

		private void HandlePlayerToggles()
		{
		}

		private void HandlePlayerMovement()
		{
			float axis = UnityEngine.Input.GetAxis("Horizontal");
			float axis2 = UnityEngine.Input.GetAxis("Vertical");
			if (Mathf.Abs(axis) > 0f || Mathf.Abs(axis2) > 0f)
			{
				this.IsMoving = true;
				if (this.playerAnimator != null)
				{
					this.playerAnimator.SetBool("IsMoving", true);
				}
			}
			else
			{
				this.IsMoving = false;
				if (this.playerAnimator != null)
				{
					this.playerAnimator.SetBool("IsMoving", false);
				}
			}
			Vector2 facingDirection = Vector2.zero;
			PlayerMovement.PlayerMovementType playerMovementType = this.playerMovementType;
			if (playerMovementType != PlayerMovement.PlayerMovementType.Normal)
			{
				if (playerMovementType == PlayerMovement.PlayerMovementType.FreeAim)
				{
					if (axis2 > 0f)
					{
						this.moveForward(this.freeAimMovementSpeed);
					}
					else if (axis2 < 0f)
					{
						this.moveBack(this.freeAimMovementSpeed);
					}
					if (axis > 0f)
					{
						this.moveRight(this.freeAimMovementSpeed);
					}
					else if (axis < 0f)
					{
						this.moveLeft(this.freeAimMovementSpeed);
					}
					Vector3 a = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 0f));
					facingDirection = a - base.transform.position;
				}
			}
			else
			{
				if (axis < 0f && this.SmoothSpeedX > -7f)
				{
					this.SmoothSpeedX -= 22f * Time.deltaTime;
				}
				else if (axis > 0f && this.SmoothSpeedX < 7f)
				{
					this.SmoothSpeedX += 22f * Time.deltaTime;
				}
				else if (this.SmoothSpeedX > 33f * Time.deltaTime)
				{
					this.SmoothSpeedX -= 33f * Time.deltaTime;
				}
				else if (this.SmoothSpeedX < -33f * Time.deltaTime)
				{
					this.SmoothSpeedX += 33f * Time.deltaTime;
				}
				else
				{
					this.SmoothSpeedX = 0f;
				}
				if (axis2 < 0f && this.SmoothSpeedY > -7f)
				{
					this.SmoothSpeedY -= 22f * Time.deltaTime;
				}
				else if (axis2 > 0f && this.SmoothSpeedY < 7f)
				{
					this.SmoothSpeedY += 22f * Time.deltaTime;
				}
				else if (this.SmoothSpeedY > 33f * Time.deltaTime)
				{
					this.SmoothSpeedY -= 33f * Time.deltaTime;
				}
				else if (this.SmoothSpeedY < -33f * Time.deltaTime)
				{
					this.SmoothSpeedY += 33f * Time.deltaTime;
				}
				else
				{
					this.SmoothSpeedY = 0f;
				}
				Vector2 v = new Vector2(base.transform.position.x + this.SmoothSpeedX * Time.deltaTime, base.transform.position.y + this.SmoothSpeedY * Time.deltaTime);
				base.transform.position = v;
			}
			this.CalculateAimAndFacingAngles(facingDirection);
			Vector3 position = Camera.main.WorldToViewportPoint(base.transform.position);
			position.x = Mathf.Clamp(position.x, 0.05f, 0.95f);
			position.y = Mathf.Clamp(position.y, 0.05f, 0.95f);
			base.transform.position = Camera.main.ViewportToWorldPoint(position);
		}

		private void CalculateAimAndFacingAngles(Vector2 facingDirection)
		{
			this.aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
			if (this.aimAngle < 0f)
			{
				this.aimAngle = 6.28318548f + this.aimAngle;
			}
			base.transform.eulerAngles = new Vector3(0f, 0f, this.aimAngle * 57.29578f);
		}

		private void Update()
		{
			this.HandlePlayerMovement();
			this.HandlePlayerToggles();
		}

		public PlayerMovement.PlayerMovementType playerMovementType;

		public bool IsMoving;

		public float aimAngle;

		[Range(1f, 5f)]
		public float freeAimMovementSpeed = 2f;

		private float SmoothSpeedX;

		private float SmoothSpeedY;

		private const float SmoothMaxSpeedX = 7f;

		private const float SmoothMaxSpeedY = 7f;

		private const float AccelerationX = 22f;

		private const float AccelerationY = 22f;

		private const float DecelerationX = 33f;

		private const float DecelerationY = 33f;

		private Animator playerAnimator;

		public enum PlayerMovementType
		{
			Normal,
			FreeAim
		}
	}
}
