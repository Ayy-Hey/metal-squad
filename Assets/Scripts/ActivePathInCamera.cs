using System;
using SWS;
using UnityEngine;

public class ActivePathInCamera : MonoBehaviour
{
	private bool IsInCamera(Transform trans)
	{
		float x = trans.position.x;
		return x >= CameraController.Instance.viewPos.minX && x <= CameraController.Instance.viewPos.maxX;
	}

	private void Update()
	{
		foreach (PathManager pathManager in WaypointManager.Paths.Values)
		{
			if (this.IsInCamera(pathManager.gameObject.transform))
			{
				pathManager.gameObject.GetComponent<PathAutoActive>().enabled = true;
			}
			else
			{
				pathManager.gameObject.GetComponent<PathAutoActive>().enabled = false;
			}
		}
	}
}
