using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ETCArea : MonoBehaviour
{
	public ETCArea()
	{
		this.show = true;
	}

	public void Awake()
	{
		base.GetComponent<Image>().enabled = this.show;
	}

	public void ApplyPreset(ETCArea.AreaPreset preset)
	{
		RectTransform component = base.transform.parent.GetComponent<RectTransform>();
		switch (preset)
		{
		case ETCArea.AreaPreset.TopLeft:
			this.rectTransform().anchoredPosition = new Vector2(-component.rect.width / 4f, component.rect.height / 4f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width / 2f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, component.rect.height / 2f);
			this.rectTransform().anchorMin = new Vector2(0f, 1f);
			this.rectTransform().anchorMax = new Vector2(0f, 1f);
			this.rectTransform().anchoredPosition = new Vector2(this.rectTransform().sizeDelta.x / 2f, -this.rectTransform().sizeDelta.y / 2f);
			break;
		case ETCArea.AreaPreset.TopRight:
			this.rectTransform().anchoredPosition = new Vector2(component.rect.width / 4f, component.rect.height / 4f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width / 2f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, component.rect.height / 2f);
			this.rectTransform().anchorMin = new Vector2(1f, 1f);
			this.rectTransform().anchorMax = new Vector2(1f, 1f);
			this.rectTransform().anchoredPosition = new Vector2(-this.rectTransform().sizeDelta.x / 2f, -this.rectTransform().sizeDelta.y / 2f);
			break;
		case ETCArea.AreaPreset.BottomLeft:
			this.rectTransform().anchoredPosition = new Vector2(-component.rect.width / 4f, -component.rect.height / 4f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width / 2f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, component.rect.height / 2f);
			this.rectTransform().anchorMin = new Vector2(0f, 0f);
			this.rectTransform().anchorMax = new Vector2(0f, 0f);
			this.rectTransform().anchoredPosition = new Vector2(this.rectTransform().sizeDelta.x / 2f, this.rectTransform().sizeDelta.y / 2f);
			break;
		case ETCArea.AreaPreset.BottomRight:
			this.rectTransform().anchoredPosition = new Vector2(component.rect.width / 4f, -component.rect.height / 4f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width / 2f);
			this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, component.rect.height / 2f);
			this.rectTransform().anchorMin = new Vector2(1f, 0f);
			this.rectTransform().anchorMax = new Vector2(1f, 0f);
			this.rectTransform().anchoredPosition = new Vector2(-this.rectTransform().sizeDelta.x / 2f, this.rectTransform().sizeDelta.y / 2f);
			break;
		}
	}

	public bool show;

	public enum AreaPreset
	{
		Choose,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}
}
