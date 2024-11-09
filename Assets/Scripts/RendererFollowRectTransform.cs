using System;
using UnityEngine;

public class RendererFollowRectTransform : MonoBehaviour
{
	private void OnValidate()
	{
		this.Start();
	}

	private void Start()
	{
		if (this.renderer && this.rectTransformFollow)
		{
			Vector3 localScale = this.rectTransformFollow.root.localScale;
			Vector2 offsetMax = this.rectTransformFollow.offsetMax;
			Vector2 offsetMin = this.rectTransformFollow.offsetMin;
			Vector3 localScale2 = this.renderer.transform.localScale;
			Vector3 max = this.renderer.bounds.max;
			Vector3 min = this.renderer.bounds.min;
			float x = (offsetMax.x - offsetMin.x) * localScale.x * localScale2.x / (max.x - min.x);
			float y = (offsetMax.y - offsetMin.y) * localScale.y * localScale2.y / (max.y - min.y);
			this.renderer.transform.localScale = new Vector3(x, y, 1f);
		}
	}

	public Renderer renderer;

	public RectTransform rectTransformFollow;
}
