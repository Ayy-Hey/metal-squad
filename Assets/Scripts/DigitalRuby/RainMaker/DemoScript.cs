using System;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.RainMaker
{
	public class DemoScript : MonoBehaviour
	{
		private void UpdateRain()
		{
			if (this.RainScript != null)
			{
				if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
				{
					this.RainScript.RainIntensity = 0f;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.RainScript.RainIntensity = 0.2f;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.RainScript.RainIntensity = 0.5f;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.RainScript.RainIntensity = 0.8f;
				}
			}
		}

		private void UpdateMovement()
		{
			float num = 5f * Time.deltaTime;
			if (UnityEngine.Input.GetKey(KeyCode.W))
			{
				Camera.main.transform.Translate(0f, 0f, num);
			}
			else if (UnityEngine.Input.GetKey(KeyCode.S))
			{
				Camera.main.transform.Translate(0f, 0f, -num);
			}
			if (UnityEngine.Input.GetKey(KeyCode.A))
			{
				Camera.main.transform.Translate(-num, 0f, 0f);
			}
			else if (UnityEngine.Input.GetKey(KeyCode.D))
			{
				Camera.main.transform.Translate(num, 0f, 0f);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.F))
			{
				this.FlashlightToggle.isOn = !this.FlashlightToggle.isOn;
			}
		}

		private void UpdateMouseLook()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.M))
			{
				this.MouseLookToggle.isOn = !this.MouseLookToggle.isOn;
			}
			if (!this.MouseLookToggle.isOn)
			{
				return;
			}
			if (this.axes == DemoScript.RotationAxes.MouseXAndY)
			{
				this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * this.sensitivityX;
				this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * this.sensitivityY;
				this.rotationX = DemoScript.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
				this.rotationY = DemoScript.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
				Quaternion rhs = Quaternion.AngleAxis(this.rotationX, Vector3.up);
				Quaternion rhs2 = Quaternion.AngleAxis(this.rotationY, -Vector3.right);
				base.transform.localRotation = this.originalRotation * rhs * rhs2;
			}
			else if (this.axes == DemoScript.RotationAxes.MouseX)
			{
				this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * this.sensitivityX;
				this.rotationX = DemoScript.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
				Quaternion rhs3 = Quaternion.AngleAxis(this.rotationX, Vector3.up);
				base.transform.localRotation = this.originalRotation * rhs3;
			}
			else
			{
				this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * this.sensitivityY;
				this.rotationY = DemoScript.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
				Quaternion rhs4 = Quaternion.AngleAxis(-this.rotationY, Vector3.right);
				base.transform.localRotation = this.originalRotation * rhs4;
			}
		}

		public void RainSliderChanged(float val)
		{
			this.RainScript.RainIntensity = val;
		}

		public void MouseLookChanged(bool val)
		{
			this.MouseLookToggle.isOn = val;
		}

		public void FlashlightChanged(bool val)
		{
			this.FlashlightToggle.isOn = val;
			this.Flashlight.enabled = val;
		}

		public void DawnDuskSliderChanged(float val)
		{
			this.Sun.transform.rotation = Quaternion.Euler(val, 0f, 0f);
		}

		public void FollowCameraChanged(bool val)
		{
			this.RainScript.FollowCamera = val;
		}

		private void Start()
		{
			this.originalRotation = base.transform.localRotation;
			BaseRainScript rainScript = this.RainScript;
			float num = 0.5f;
			this.RainSlider.value = num;
			rainScript.RainIntensity = num;
			this.RainScript.EnableWind = true;
		}

		private void Update()
		{
			this.UpdateRain();
			this.UpdateMovement();
			this.UpdateMouseLook();
		}

		public static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		public RainScript RainScript;

		public Toggle MouseLookToggle;

		public Toggle FlashlightToggle;

		public Slider RainSlider;

		public Light Flashlight;

		public GameObject Sun;

		private DemoScript.RotationAxes axes;

		private float sensitivityX = 15f;

		private float sensitivityY = 15f;

		private float minimumX = -360f;

		private float maximumX = 360f;

		private float minimumY = -60f;

		private float maximumY = 60f;

		private float rotationX;

		private float rotationY;

		private Quaternion originalRotation;

		private enum RotationAxes
		{
			MouseXAndY,
			MouseX,
			MouseY
		}
	}
}
