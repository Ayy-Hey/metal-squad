using System;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (!this.boxCollider2D)
		{
			this.boxCollider2D = base.GetComponent<BoxCollider2D>();
			this.min = this.boxCollider2D.bounds.min;
			this.max = this.boxCollider2D.bounds.max;
		}
	}

	private void Reset()
	{
		this.OnValidate();
	}

	public Vector2 min;

	public Vector2 max;

	public BoxCollider2D boxCollider2D;
}
