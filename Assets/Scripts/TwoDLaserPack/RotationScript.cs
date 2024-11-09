using System;
using UnityEngine;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	public class RotationScript : MonoBehaviour
	{
		private void Start()
		{
			this.transformCached = base.transform;
		}

		private void Update()
		{
			if (!this.rotationEnabled)
			{
				return;
			}
			this.transformCached.RotateAround(this.pivot.localPosition, Vector3.forward, this.rotationAmount);
		}

		public void OnHSliderChanged()
		{
			this.rotationAmount = this.hSlider.value;
		}

		public Slider hSlider;

		public Transform pivot;

		public bool rotationEnabled;

		public float rotationAmount;

		private Transform transformCached;
	}
}
