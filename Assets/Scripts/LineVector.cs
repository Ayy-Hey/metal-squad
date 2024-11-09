using System;
using UnityEngine;

public class LineVector : MonoBehaviour
{
	private void OnValidate()
	{
		try
		{
			this.lineRenderer = base.GetComponent<LineRenderer>();
			this.lineRenderer.positionCount = this.transformBodyPoins.Length;
			this.lineRenderer.sortingLayerName = "Gameplay";
			this.lineRenderer.sortingOrder = 2;
			this.SetPositionLineRenderer();
		}
		catch
		{
		}
	}

	public void Active()
	{
		this.lineRenderer.enabled = true;
	}

	public void Deactive()
	{
		this.lineRenderer.enabled = false;
	}

	public void OnUpdate(float deltaTime)
	{
		this.SetPositionLineRenderer();
		this.ChangeOffset(deltaTime);
	}

	private void SetPositionLineRenderer()
	{
		if (this.transformBodyPoins == null)
		{
			return;
		}
		for (int i = 0; i < this.transformBodyPoins.Length; i++)
		{
			this.lineRenderer.SetPosition(i, this.transformBodyPoins[i].position);
		}
	}

	private void ChangeOffset(float deltaTime)
	{
		this.offset = this.lineRenderer.materials[0].mainTextureOffset;
		this.offset.x = this.offset.x + deltaTime;
		this.lineRenderer.materials[0].mainTextureOffset = this.offset;
	}

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private Transform[] transformBodyPoins;

	private Vector2 offset;
}
