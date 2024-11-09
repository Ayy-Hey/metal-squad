using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class BackgroundMoveManager : MonoBehaviour
{
	private void Start()
	{
		if (this.zoomCameraPro != null)
		{
			ProCamera2DTriggerZoom proCamera2DTriggerZoom = this.zoomCameraPro;
			proCamera2DTriggerZoom.OnEnteredTrigger = (Action)Delegate.Combine(proCamera2DTriggerZoom.OnEnteredTrigger, new Action(delegate()
			{
				CameraController.Instance.parallaxLayer1.StartAuto();
				CameraController.Instance.parallaxLayer1.ActiveAllObject();
			}));
		}
	}

	[SerializeField]
	private ProCamera2DTriggerZoom zoomCameraPro;
}
