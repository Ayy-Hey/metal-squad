using System;
using UnityEngine;

public class EffectMap : MonoBehaviour
{
	private void Update()
	{
		Vector3 position = this.mCamera.position;
		position.x += 8f;
		position.z = this.transform.position.z;
		this.transform.position = position;
	}

	[SerializeField]
	private Transform mCamera;

	[SerializeField]
	private new Transform transform;
}
