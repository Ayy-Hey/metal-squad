using System;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, this.velocity));
	}

	public float velocity = -3f;
}
