using System;
using UnityEngine;

public class CameraBegin : MonoBehaviour
{
	public void Reset()
	{
		CameraController.Instance.ResetCameraPro();
	}
}
