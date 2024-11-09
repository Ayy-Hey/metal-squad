using System;
using UnityEngine;

public class NextCamera : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			if (this.mCamera != null)
			{
				LeanTween.move(this.mCamera.gameObject, new Vector3(24.34f, 0.47f, -10f), 1f);
			}
			base.gameObject.SetActive(false);
		}
	}

	[SerializeField]
	private Camera mCamera;
}
