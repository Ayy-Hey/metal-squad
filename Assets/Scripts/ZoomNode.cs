using System;
using UnityEngine;

public class ZoomNode : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.rectTransNode)
		{
			this.rectTransNode = base.GetComponent<RectTransform>();
		}
		this.rectTransChilds = new RectTransform[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			this.rectTransChilds[i] = base.transform.GetChild(i).GetComponent<RectTransform>();
		}
		this.normalSize = this.rectTransNode.sizeDelta;
	}

	private void Reset()
	{
		this.OnValidate();
	}

	public void SetSizeNode(float size)
	{
		if (this.oldSize != size)
		{
			this.oldSize = size;
			this.rectTransNode.sizeDelta = this.normalSize * size;
			for (int i = 0; i < this.rectTransChilds.Length; i++)
			{
				this.rectTransChilds[i].localScale = Vector3.one * size;
			}
		}
	}

	public Action<bool> actionMider;

	public RectTransform rectTransNode;

	[SerializeField]
	private RectTransform[] rectTransChilds;

	[SerializeField]
	public Vector2 normalSize;

	private float oldSize;
}
