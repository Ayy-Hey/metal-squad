using System;
using UnityEngine;

public class Map2_5 : MonoBehaviour
{
	private void Start()
	{
		this.triggerBoundaryMoveBackground.OnTriggerEvent.AddListener(delegate()
		{
			CameraController.Instance.parallaxLayer1.transform.position = new Vector3(CameraController.Instance.parallaxLayer1.transform.position.x, -10f, 0f);
		});
	}

	public CustomTriggerBoundary triggerBoundaryMoveBackground;
}
