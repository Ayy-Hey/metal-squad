using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomScrollCtl : MonoBehaviour, IEndDragHandler, IDragHandler, IEventSystemHandler
{
	private void OnValidate()
	{
		this.scrollRect = base.GetComponent<ScrollRect>();
		if (this.horizontalLGContent && this.nodes.Count != 0)
		{
			RectOffset padding = this.horizontalLGContent.padding;
			int num = (int)(-this.nodes[0].normalSize.x * this.scaleSize / 2f + this.scrollRect.rectTransform().sizeDelta.x / 2f) + 1;
			this.horizontalLGContent.padding.right = num;
			padding.left = num;
		}
		if (this.nodes.Count > 0)
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (this.nodes[i] == null)
				{
					this.nodes.Remove(this.nodes[i]);
				}
			}
		}
	}

	private void OnEnable()
	{
		this.nodeWidth = this.nodes[0].rectTransNode.sizeDelta.x * 1.75f;
		if (this.idNodeMid == 0)
		{
			this.GoToNode(0);
		}
	}

	internal void ShowNode(int node)
	{
		base.StartCoroutine(this.ChangeNode(node));
	}

	private IEnumerator ChangeNode(int node)
	{
		yield return new WaitForSeconds(0.5f);
		this.GoToNode(node);
		yield break;
	}

	private void GoToNode(int node)
	{
		this.isEndDrag = (this.isDrag = false);
		this.posTarget.x = -this.nodes[node].rectTransNode.anchoredPosition.x;
		this.posTarget.y = this.rectTransContent.anchoredPosition.y;
		this.idTarget = node;
		this.isGoToTarget = true;
		this.scrollRect.enabled = false;
	}

	private void Update()
	{
		if (this.isDrag || this.isEndDrag || this.isGoToTarget)
		{
			this.OnChangeValue();
		}
		if (this.isEndDrag && Mathf.Abs(this.scrollRect.velocity.x) <= 50f)
		{
			this.GoToNode(this.idNodeMid);
		}
		if (this.isGoToTarget)
		{
			this.posTarget.x = -this.nodes[this.idTarget].rectTransNode.anchoredPosition.x;
			this.contentPos = this.rectTransContent.anchoredPosition;
			this.contentPos.x = Mathf.SmoothDamp(this.contentPos.x, this.posTarget.x, ref this.speedToMid, 0.3f);
			this.rectTransContent.anchoredPosition = this.contentPos;
			float num = Mathf.Abs(this.contentPos.x - this.posTarget.x);
			if (num < 1f)
			{
				this.isGoToTarget = false;
				this.scrollRect.enabled = true;
			}
		}
	}

	public void OnChangeValue()
	{
		this.minDistance = Mathf.Abs(this.rectTransContent.anchoredPosition.x + this.nodes[0].rectTransNode.anchoredPosition.x);
		this.idNodeMid = 0;
		for (int i = 0; i < this.nodes.Count; i++)
		{
			float num = Mathf.Abs(this.rectTransContent.anchoredPosition.x + this.nodes[i].rectTransNode.anchoredPosition.x);
			if (num <= this.minDistance)
			{
				this.minDistance = num;
				this.idNodeMid = i;
			}
		}
		for (int j = 0; j < this.nodes.Count; j++)
		{
			try
			{
				this.nodes[j].actionMider(j == this.idNodeMid);
			}
			catch (Exception ex)
			{
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.isEndDrag = true;
		this.isDrag = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.isDrag = true;
	}

	[SerializeField]
	private ScrollRect scrollRect;

	[SerializeField]
	private List<ZoomNode> nodes;

	[SerializeField]
	private RectTransform rectTransContent;

	[SerializeField]
	private HorizontalLayoutGroup horizontalLGContent;

	[SerializeField]
	private float scaleSize;

	[SerializeField]
	private float speedToMid = 10f;

	private float nodeWidth;

	private float minDistance;

	private int idNodeMid;

	private int idTarget;

	private Vector2 posTarget;

	private bool isEndDrag;

	private bool isDrag;

	private bool isGoToTarget;

	private Vector2 contentPos;
}
