using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class RotateTowardsMouse : MonoBehaviour
	{
		private void Awake()
		{
			this._transform = base.transform;
		}

		private void Update()
		{
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			Vector3 vector = Camera.main.WorldToScreenPoint(this._transform.localPosition);
			Vector2 vector2 = new Vector2(mousePosition.x - vector.x, mousePosition.y - vector.y);
			float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			this._transform.rotation = Quaternion.Slerp(this._transform.rotation, Quaternion.Euler(0f, -num, 0f), this.Ease);
		}

		public float Ease = 0.15f;

		private Transform _transform;
	}
}
