using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class SwitchCameraProjection : MonoBehaviour
	{
		private void Awake()
		{
			this.Switch();
		}

		private void OnGUI()
		{
			if (GUI.Button(new Rect((float)(Screen.width - 210), 10f, 200f, 30f), "Switch to " + this._cameraMode + " mode"))
			{
				PlayerPrefs.SetInt("orthoCamera", (!Camera.main.orthographic) ? 1 : 0);
				this.Switch();
			}
		}

		private void Switch()
		{
			Camera.main.orthographic = (PlayerPrefs.GetInt("orthoCamera", 0) == 1);
			this._cameraMode = ((!Camera.main.orthographic) ? "orthographic" : "perspective");
		}

		public string _cameraMode;
	}
}
