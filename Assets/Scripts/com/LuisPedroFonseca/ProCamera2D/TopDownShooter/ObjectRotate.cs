using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class ObjectRotate : MonoBehaviour
	{
		private void Awake()
		{
			this._transform = base.transform;
		}

		private void LateUpdate()
		{
			this._transform.Rotate(this.Rotation);
		}

		public Vector3 Rotation = Vector3.one;

		private Transform _transform;
	}
}
