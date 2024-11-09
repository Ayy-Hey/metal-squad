using System;
using UnityEngine;

public class ClearCollider : MonoBehaviour
{
	private void LateUpdate()
	{
		for (int i = 0; i < this.ArrCollider.Length; i++)
		{
			GameObject gameObject = this.ArrCollider[i];
			gameObject.SetActive(gameObject.transform.position.y > CameraController.Instance.BottomCamera());
		}
	}

	[SerializeField]
	private GameObject[] ArrCollider;
}
