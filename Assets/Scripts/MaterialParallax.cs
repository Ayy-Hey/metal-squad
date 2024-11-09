using System;
using UnityEngine;

[Serializable]
public class MaterialParallax
{
	public Renderer renderer;

	public float speed;

	public bool isRepeat = true;

	[HideInInspector]
	public Vector2 savedOffset;
}
