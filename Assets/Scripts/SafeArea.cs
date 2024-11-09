using System;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
	private void Awake()
	{
		this.Panel = base.GetComponent<RectTransform>();
		this.Refresh();
	}

	private void Update()
	{
		this.Refresh();
	}

	private void Refresh()
	{
		Rect safeArea = this.GetSafeArea();
		if (safeArea != this.LastSafeArea)
		{
			this.ApplySafeArea(safeArea);
		}
	}

	private Rect GetSafeArea()
	{
		return Screen.safeArea;
	}

	private void ApplySafeArea(Rect r)
	{
		this.LastSafeArea = r;
		Vector2 position = r.position;
		Vector2 anchorMax = r.position + r.size;
		position.x /= (float)Screen.width;
		position.y /= (float)Screen.height;
		anchorMax.x /= (float)Screen.width;
		anchorMax.y /= (float)Screen.height;
		this.Panel.anchorMin = position;
		this.Panel.anchorMax = anchorMax;
		UnityEngine.Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}", new object[]
		{
			base.name,
			r.x,
			r.y,
			r.width,
			r.height,
			Screen.width,
			Screen.height
		});
	}

	private RectTransform Panel;

	private Rect LastSafeArea = new Rect(0f, 0f, 0f, 0f);
}
