using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class SetupCamera : MonoBehaviour
{
	private void Start()
	{
		if (this.zoomCameraPro != null)
		{
			ProCamera2DTriggerZoom proCamera2DTriggerZoom = this.zoomCameraPro;
			proCamera2DTriggerZoom.OnEnteredTrigger = (Action)Delegate.Combine(proCamera2DTriggerZoom.OnEnteredTrigger, new Action(delegate()
			{
				if (!this.isSetup)
				{
					this.isSetup = true;
					this.SetUpBaoundary();
					if (this.isOffsetCamera)
					{
						ProCamera2D.Instance.OffsetX = this.offset.x;
						ProCamera2D.Instance.OffsetY = this.offset.y;
					}
				}
			}));
		}
	}

	private void SetUpBaoundary()
	{
		if (this.isTop)
		{
			LeanTween.cancel(CameraController.Instance.gameObject);
			CameraController.Instance.NumericBoundaries.TopBoundary = this.boundaryTop;
		}
		if (this.isBottom)
		{
			LeanTween.cancel(CameraController.Instance.gameObject);
			CameraController.Instance.NumericBoundaries.BottomBoundary = this.boundaryBottom;
		}
		if (this.isLeft)
		{
			LeanTween.cancel(CameraController.Instance.gameObject);
			CameraController.Instance.NumericBoundaries.LeftBoundary = this.boundaryLeft;
		}
		if (this.isRight)
		{
			LeanTween.cancel(CameraController.Instance.gameObject);
			CameraController.Instance.NumericBoundaries.RightBoundary = this.boundaryRight;
		}
	}

	[SerializeField]
	[Header("---------------Boundary---------------")]
	private bool isTop;

	[SerializeField]
	private float boundaryTop;

	[SerializeField]
	private bool isBottom;

	[SerializeField]
	private float boundaryBottom;

	[SerializeField]
	private bool isLeft;

	[SerializeField]
	private float boundaryLeft;

	[SerializeField]
	private bool isRight;

	[SerializeField]
	private float boundaryRight;

	[Header("--------------Camera Offset-------------------")]
	[SerializeField]
	private bool isOffsetCamera;

	[SerializeField]
	private Vector2 offset = new Vector2(1f, 1.5f);

	[SerializeField]
	private ProCamera2DTriggerZoom zoomCameraPro;

	private bool isSetup;
}
