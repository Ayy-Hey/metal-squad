using System;
using UnityEngine;

public class ClearCharacter : MonoBehaviour
{
	private void LateUpdate()
	{
		if (!this.isFlowCamera)
		{
			return;
		}
		Vector3 position = base.transform.position;
		position.y = CameraController.Instance.BottomCamera();
		base.transform.position = position;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Rambo") || GameManager.Instance.StateManager.EState == EGamePlay.BEGIN || GameManager.Instance.StateManager.EState == EGamePlay.NONE || other.CompareTag("Boss"))
		{
			return;
		}
		IHealth component = other.gameObject.GetComponent<IHealth>();
		if (component != null)
		{
			component.AddHealthPoint(-10000f, EWeapon.EXCEPTION);
		}
	}

	[SerializeField]
	private bool isFlowCamera;
}
