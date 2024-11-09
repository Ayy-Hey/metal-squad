using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class CustomCameraMap : MonoBehaviour
{
	private void Start()
	{
		this.triggerCamera.OnEnteredTrigger = delegate()
		{
			if (this.isTriger)
			{
				return;
			}
			this.isTriger = true;
			ProCamera2D.Instance.Zoom(this.AddSizeCamera, 1f, EaseType.EaseInOut);
			ProCamera2D.Instance.OffsetY = this.OffsetYPlayer;
		};
	}

	public ProCamera2DTriggerBoundaries triggerCamera;

	private bool isTriger;

	public float AddSizeCamera;

	public float OffsetYPlayer;
}
