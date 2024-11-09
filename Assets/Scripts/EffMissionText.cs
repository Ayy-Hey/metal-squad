using System;
using UnityEngine;
using UnityEngine.UI;

public class EffMissionText : MonoBehaviour
{
	public void Active()
	{
		this.canScroll = (this.text.preferredWidth > this.viewSize);
		if (!this.canScroll)
		{
			return;
		}
		float x = this.startPoint.x - this.viewSize - this.text.preferredWidth;
		float y = this.text.rectTransform.anchoredPosition.y;
		this.targetPoint = new Vector2(x, y);
		base.enabled = true;
	}

	public void Deactive()
	{
		if (this.canScroll)
		{
			base.enabled = false;
			this.text.rectTransform.anchoredPosition = new Vector2(-90f, this.text.rectTransform.anchoredPosition.y);
			this.canScroll = false;
		}
	}

	private void Update()
	{
		if (!this.canScroll)
		{
			return;
		}
		this.text.rectTransform.anchoredPosition = Vector2.MoveTowards(this.text.rectTransform.anchoredPosition, this.targetPoint, Time.deltaTime * 20f);
		if (this.text.rectTransform.anchoredPosition == this.targetPoint)
		{
			this.text.rectTransform.anchoredPosition = this.startPoint;
		}
	}

	public Text text;

	public Vector2 startPoint;

	public float viewSize;

	private Vector2 targetPoint;

	private bool canScroll;
}
