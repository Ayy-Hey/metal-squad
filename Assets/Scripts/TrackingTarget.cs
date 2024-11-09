using System;
using UnityEngine;

public class TrackingTarget : MonoBehaviour
{
	private void OnEnable()
	{
		base.transform.position = this.target.position;
	}

	private void Update()
	{
		base.transform.position = this.target.position;
	}

	public Transform target;
}
