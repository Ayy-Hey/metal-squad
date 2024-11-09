using System;
using UnityEngine;

public class ForgroundMove : MonoBehaviour
{
	private void OnEnable()
	{
		TriggerCamera triggerCamera = this.triggerCameraStart;
		triggerCamera.OnEnteredTrigger = (Action)Delegate.Combine(triggerCamera.OnEnteredTrigger, new Action(delegate()
		{
			this.isRunning = true;
		}));
		TriggerCamera triggerCamera2 = this.triggerCameraStop;
		triggerCamera2.OnEnteredTrigger = (Action)Delegate.Combine(triggerCamera2.OnEnteredTrigger, new Action(this.StopMoveObjects));
	}

	public void StopMoveObjects()
	{
		this.isRunning = false;
		foreach (GameObject gameObject in this.gameObjects)
		{
			gameObject.SetActive(true);
		}
	}

	[SerializeField]
	private Transform starPos;

	[SerializeField]
	private Transform endPos;

	[SerializeField]
	private GameObject[] gameObjects;

	[SerializeField]
	private float speed;

	private bool isRunning;

	[SerializeField]
	private TriggerCamera triggerCameraStart;

	[SerializeField]
	private TriggerCamera triggerCameraStop;
}
