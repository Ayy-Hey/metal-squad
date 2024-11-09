using System;
using UnityEngine;

public class SniperLaser : MonoBehaviour
{
	private void OnEnable()
	{
		this.lineRenderer.SetPosition(0, Vector3.zero);
	}

	private void Update()
	{
		RaycastHit2D hit = Physics2D.Raycast(base.transform.position, Vector3.right, 1000f, this.maskLaserSniper);
		if (hit)
		{
			float x = Vector2.Distance(hit.point, base.transform.position);
			Vector2 v = new Vector2(x, 0f);
			this.lineRenderer.SetPosition(1, v);
		}
	}

	[SerializeField]
	private LayerMask maskLaserSniper;

	[SerializeField]
	private LineRenderer lineRenderer;
}
