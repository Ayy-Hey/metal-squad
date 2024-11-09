using System;
using UnityEngine;

public class TestCon : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.offset = this.mat.mainTextureOffset;
		this.offset.x = this.offset.x + Time.deltaTime;
		this.mat.mainTextureOffset = this.offset;
	}

	public Material mat;

	public Vector2 offset;
}
