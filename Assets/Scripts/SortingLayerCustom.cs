using System;
using UnityEngine;

public class SortingLayerCustom : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.renderer)
		{
			this.renderer = base.GetComponent<Renderer>();
		}
		int a = SortingLayer.layers.Length - 1;
		this.renderer.sortingLayerID = SortingLayer.layers[Mathf.Max(0, Mathf.Min(a, (int)this.sortingLayer))].id;
		this.renderer.sortingOrder = this.sortingOrder;
	}

	private void Reset()
	{
		this.OnValidate();
	}

	[SerializeField]
	private Renderer renderer;

	[SerializeField]
	private SortingLayerCustom.ELayerSorting sortingLayer;

	[SerializeField]
	private int sortingOrder;

	private enum ELayerSorting
	{
		Default,
		Background,
		Forceground,
		GUI_F,
		Gameplay,
		GUI_P,
		Tutorial,
		Loading
	}
}
