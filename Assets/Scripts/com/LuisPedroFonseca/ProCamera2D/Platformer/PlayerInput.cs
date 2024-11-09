using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.Platformer
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerInput : MonoBehaviour
	{
		private void Start()
		{
			this._characterController = base.GetComponent<CharacterController>();
		}

		private void Update()
		{
			if ((this._characterController.collisionFlags & CollisionFlags.Sides) != CollisionFlags.None)
			{
				this.currentSpeed = 0f;
			}
			if ((this._characterController.collisionFlags & CollisionFlags.Below) != CollisionFlags.None)
			{
				this.amountToMove.y = -1f;
				this.totalJumps = 0;
			}
			else
			{
				this.amountToMove.y = this.amountToMove.y - this.gravity * Time.deltaTime;
			}
			if ((UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) && this.totalJumps < this.jumpsAllowed)
			{
				this.totalJumps++;
				this.amountToMove.y = this.jumpHeight;
			}
			float target = UnityEngine.Input.GetAxis("Horizontal") * this.runSpeed;
			this.currentSpeed = this.IncrementTowards(this.currentSpeed, target, this.acceleration);
			if (base.transform.position.z != 0f)
			{
				this.amountToMove.z = -base.transform.position.z;
			}
			this.amountToMove.x = this.currentSpeed;
			if (this.amountToMove.x != 0f)
			{
				this.Body.localScale = new Vector2(Mathf.Sign(this.amountToMove.x) * Mathf.Abs(this.Body.localScale.x), this.Body.localScale.y);
			}
			this._characterController.Move(this.amountToMove * Time.deltaTime);
		}

		private float IncrementTowards(float n, float target, float a)
		{
			if (n == target)
			{
				return n;
			}
			float num = Mathf.Sign(target - n);
			n += a * Time.deltaTime * num;
			return (num != Mathf.Sign(target - n)) ? target : n;
		}

		public Transform Body;

		public float gravity = 20f;

		public float runSpeed = 12f;

		public float acceleration = 30f;

		public float jumpHeight = 12f;

		public int jumpsAllowed = 2;

		private float currentSpeed;

		private Vector3 amountToMove;

		private int totalJumps;

		private CharacterController _characterController;
	}
}
