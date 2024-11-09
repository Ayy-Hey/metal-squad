using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class ObjectMove : MonoBehaviour
	{
		private void Awake()
		{
			this._transform = base.transform;
		}

		private void LateUpdate()
		{
			this._transform.position += this.Amplitude * (Mathf.Sin(6.28318548f * this.Frequency * Time.time) - Mathf.Sin(6.28318548f * this.Frequency * (Time.time - Time.deltaTime))) * Vector3.up;
		}

		public float Amplitude = 1f;

		public float Frequency = 1f;

		private Transform _transform;
	}
}
