using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerInput : MonoBehaviour
	{
		private void Start()
		{
			this._characterController = base.GetComponent<CharacterController>();
			ProCamera2DCinematics[] array = UnityEngine.Object.FindObjectsOfType<ProCamera2DCinematics>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnCinematicStarted.AddListener(delegate()
				{
					this._movementAllowed = false;
					this._currentSpeedH = 0f;
					this._currentSpeedV = 0f;
				});
				array[i].OnCinematicFinished.AddListener(delegate()
				{
					this._movementAllowed = true;
				});
			}
		}

		private void Update()
		{
			if (!this._movementAllowed)
			{
				return;
			}
			float target = UnityEngine.Input.GetAxis("Horizontal") * this.RunSpeed;
			this._currentSpeedH = this.IncrementTowards(this._currentSpeedH, target, this.Acceleration);
			float target2 = UnityEngine.Input.GetAxis("Vertical") * this.RunSpeed;
			this._currentSpeedV = this.IncrementTowards(this._currentSpeedV, target2, this.Acceleration);
			this._amountToMove.x = this._currentSpeedH;
			this._amountToMove.z = this._currentSpeedV;
			this._characterController.Move(this._amountToMove * Time.deltaTime);
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

		public float RunSpeed = 12f;

		public float Acceleration = 30f;

		private float _currentSpeedH;

		private float _currentSpeedV;

		private Vector3 _amountToMove;

		private int _totalJumps;

		private CharacterController _characterController;

		private bool _movementAllowed = true;
	}
}
