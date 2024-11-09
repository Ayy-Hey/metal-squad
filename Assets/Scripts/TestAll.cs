using System;
using Spine;
using UnityEngine;

public class TestAll : MonoBehaviour
{
	private void OnValidate()
	{
	}

	private void Start()
	{
		this.m_Material = this.ren.material;
		this.m_Material.SetTextureScale("_MainTex", this.tilling);
		this.m_Material.SetTexture("_MainTex", this.texture);
	}

	private void OnComplete(TrackEntry trackEntry)
	{
	}

	private void Update()
	{
		this.offset = this.m_Material.mainTextureOffset;
		this.offset.x = this.offset.x - Time.deltaTime;
		this.m_Material.mainTextureOffset = this.offset;
	}

	private void SetMat()
	{
	}

	[SerializeField]
	private Renderer ren;

	[SerializeField]
	private Texture texture;

	private Material m_Material;

	public Vector2 offset;

	public Vector2 tilling;

	public float pushPower = 2f;
}
