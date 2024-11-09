using System;
using UnityEngine;

public class Rada : MonoBehaviour
{
	private void Start()
	{
		this.start = base.transform.position.x;
		this.end = -this.start;
	}

	private void Update()
	{
		base.transform.Rotate(0f, 0f, -this.speed);
	}

	[HideInInspector]
	public float start;

	[HideInInspector]
	public float end;

	public float speed;
}
