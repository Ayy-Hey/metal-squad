using System;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.RainMaker
{
	public class DemoScript2D : MonoBehaviour
	{
		private void Start()
		{
			BaseRainScript rainScript = this.RainScript;
			float num = 0.5f;
			this.RainSlider.value = num;
			rainScript.RainIntensity = num;
			this.RainScript.EnableWind = true;
		}

		private void Update()
		{
			Vector3 vector = Camera.main.ViewportToWorldPoint(Vector3.zero);
			float num = Camera.main.ViewportToWorldPoint(Vector3.one).x - vector.x;
			if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
			{
				Camera.main.transform.Translate(Time.deltaTime * -(num * 0.1f), 0f, 0f);
			}
			else if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
			{
				Camera.main.transform.Translate(Time.deltaTime * (num * 0.1f), 0f, 0f);
			}
		}

		public void RainSliderChanged(float val)
		{
			this.RainScript.RainIntensity = val;
		}

		public void CollisionToggleChanged(bool val)
		{
			this.RainScript.CollisionMask = ((!val) ? 0 : -1);
		}

		public Slider RainSlider;

		public RainScript2D RainScript;
	}
}
