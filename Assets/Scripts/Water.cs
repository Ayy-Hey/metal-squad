using System;
using UnityEngine;

public class Water : MonoBehaviour
{
	private void Start()
	{
		this.OnStart();
	}

	private void LateUpdate()
	{
		this.OnUpdate();
	}

	private void OnStart()
	{
		this.savedOffset = this.MyRenderer.material.mainTextureOffset;
	}

	private void OnUpdate()
	{
		if (this.isFlowCamera)
		{
			Vector3 position = base.transform.position;
			position.x = CameraController.Instance.transform.position.x;
			base.transform.position = position;
		}
		this.savedOffset.x = Mathf.Repeat(Time.time * this.SPEED, this.MAX_X_Offset);
		this.MyRenderer.material.mainTextureOffset = this.savedOffset;
	}

	[SerializeField]
	private Renderer MyRenderer;

	[SerializeField]
	private float SPEED;

	[SerializeField]
	private float MAX_X_Offset = 1f;

	[SerializeField]
	private bool isFlowCamera = true;

	private Vector2 savedOffset;
}
