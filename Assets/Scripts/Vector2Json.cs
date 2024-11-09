using System;
using UnityEngine;

public class Vector2Json : MonoBehaviour
{
	public float x { get; set; }

	public float y { get; set; }

	public Vector2 Vt2
	{
		get
		{
			return new Vector2(this.x, this.y);
		}
	}
}
